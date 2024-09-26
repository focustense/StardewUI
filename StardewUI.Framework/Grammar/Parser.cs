﻿namespace StardewUI.Framework.Grammar;

/// <summary>
/// Information about a parsed tag in StarML.
/// </summary>
/// <param name="name">The tag name.</param>
/// <param name="isClosingTag">Whether or not the tag is a closing tag, either in regular <c>&lt;/tag&gt;</c> form or
/// the end of a self-closing tag (<c>/&gt;</c>) after the tag attributes.</param>
public readonly ref struct TagInfo(ReadOnlySpan<char> name, bool isClosingTag)
{
    public ReadOnlySpan<char> Name { get; } = name;
    public bool IsClosingTag { get; } = isClosingTag;
}

/// <summary>
/// Types allowed for the value of an <see cref="Attribute"/>.
/// </summary>
public enum AttributeValueType
{
    /// <summary>
    /// The value is the literal string in the markup, i.e. it is the actual string representation of the target data
    /// type such as an integer, enumeration or another string.
    /// </summary>
    Literal,

    /// <summary>
    /// The value is an instruction for creating a data binding, used to obtain the real value from a different source.
    /// </summary>
    Binding,
}

/// <summary>
/// A complete attribute assignment parsed from StarML.
/// </summary>
/// <param name="name">The attribute name.</param>
/// <param name="valueType">The type of the value expression, defining how the <paramref name="value"/> should be
/// interpreted.</param>
/// <param name="value">The literal value text.</param>
public readonly ref struct Attribute(ReadOnlySpan<char> name, AttributeValueType valueType, ReadOnlySpan<char> value)
{
    /// <summary>
    /// The attribute name.
    /// </summary>
    public ReadOnlySpan<char> Name { get; } = name;

    /// <summary>
    /// The literal value text.
    /// </summary>
    public ReadOnlySpan<char> Value { get; } = value;

    /// <summary>
    /// The type of the value expression, defining how the <paramref name="Value"/> should be interpreted.
    /// </summary>
    public AttributeValueType ValueType { get; } = valueType;
}

/// <summary>
/// Reads elements and associated attributes from a StarML document (content string).
/// </summary>
/// <param name="lexer">The lexer that reads syntax tokens from the document.</param>
public ref struct DocumentReader(Lexer lexer)
{
    /// <summary>
    /// The attribute that was just read, if the previous <see cref="NextAttribute"/> returned <c>true</c>; otherwise,
    /// an empty attribute.
    /// </summary>
    public Attribute Attribute { get; private set; }

    /// <summary>
    /// The tag that was just read, if the previous <see cref="NextTag"/> returned <c>true</c>; otherwise, an empty tag.
    /// </summary>
    /// <remarks>
    /// The tag remains valid as attributes are read; i.e. <see cref="NextAttribute"/> will never change this value.
    /// </remarks>
    public TagInfo Tag { get; private set; }

    private Lexer lexer = lexer;
    private bool wasTagSelfClosed;

    /// <summary>
    /// Initializes a new <see cref="DocumentReader"/> from the specified text, creating an implicit lexer.
    /// </summary>
    /// <param name="text">The markup text.</param>
    public DocumentReader(ReadOnlySpan<char> text)
        : this(new Lexer(text)) { }

    /// <summary>
    /// Reads the next <see cref="Attribute"/>. Only valid in a tag scope, i.e. after a call to <see cref="NextTag"/>
    /// returns <c>true</c>.
    /// </summary>
    /// <returns><c>true</c> if an attribute was read; <c>false</c> if there are no more attributes to read for the
    /// current element.</returns>
    /// <exception cref="ParserException">Thrown when the current position is not within a tag, or when unparseable
    /// attribute data is encountered.</exception>
    public bool NextAttribute()
    {
        if (Tag.Name.Length == 0)
        {
            throw new ParserException("Cannot read an attribute when outside of an element.", lexer.Position);
        }
        if (Tag.IsClosingTag)
        {
            return false;
        }
        lexer.ReadRequiredToken(TokenType.Name, TokenType.TagEnd, TokenType.SelfClosingTagEnd);
        switch (lexer.Current.Type)
        {
            case TokenType.Name:
                var attributeName = lexer.Current.Text;
                lexer.ReadRequiredToken(TokenType.Assignment);
                lexer.ReadRequiredToken(TokenType.Quote, TokenType.BindingStart);
                var valueType =
                    lexer.Current.Type == TokenType.BindingStart
                        ? AttributeValueType.Binding
                        : AttributeValueType.Literal;
                lexer.ReadRequiredToken(TokenType.Literal);
                var attributeValue = lexer.Current.Text;
                // We don't bother trying to enforce that the end token matches the start token because the Lexer will
                // have already failed to parse the literal if it doesn't match.
                lexer.ReadRequiredToken(TokenType.Quote, TokenType.BindingEnd);
                Attribute = new(attributeName, valueType, attributeValue);
                return true;
            case TokenType.SelfClosingTagEnd:
                wasTagSelfClosed = true;
                goto default;
            default:
                Attribute = default;
                return false;
        }
    }

    /// <summary>
    /// Reads the next <see cref="Tag"/>, discarding any remaining attributes for the current tag.
    /// </summary>
    /// <returns><c>true</c> if an attribute was read; <c>false</c> if the end of the document was reached.</returns>
    /// <exception cref="ParserException">Thrown when the next tag is malformed or otherwise unparseable.</exception>
    public bool NextTag()
    {
        while (Attribute.Name.Length > 0)
        {
            NextAttribute();
        }
        if (wasTagSelfClosed)
        {
            Tag = new(Tag.Name, true);
            wasTagSelfClosed = false;
            return true;
        }
        if (!lexer.ReadOptionalToken(TokenType.OpeningTagStart, TokenType.ClosingTagStart))
        {
            Tag = default;
            return false;
        }
        bool isClosingTag = lexer.Current.Type == TokenType.ClosingTagStart;
        lexer.ReadRequiredToken(TokenType.Name);
        var tagName = lexer.Current.Text;
        if (isClosingTag)
        {
            lexer.ReadRequiredToken(TokenType.TagEnd);
        }
        Tag = new(tagName, isClosingTag);
        return true;
    }
}

/// <summary>
/// The exception that is thrown when a <see cref="DocumentReader"/> encounters invalid content.
/// </summary>
/// <param name="message">The message that describes the error.</param>
/// <param name="position">The position within the markup text where the error was encountered.</param>
public class ParserException(string message, int position) : Exception(message)
{
    internal static ParserException ForCurrentToken(in Lexer lexer, string message)
    {
        var tokenPosition = lexer.Position - lexer.Current.Text.Length;
        return new(message, tokenPosition);
    }

    /// <summary>
    /// The position within the markup text where the error was encountered.
    /// </summary>
    public int Position { get; } = position;
}
