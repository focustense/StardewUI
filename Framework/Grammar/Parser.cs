using System;

namespace StardewUI.Framework.Grammar;

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
/// The different types of an <see cref="Attribute"/>, independent of its value.
/// </summary>
public enum AttributeType
{
    /// <summary>
    /// Sets or binds a property on the target view.
    /// </summary>
    Property,

    /// <summary>
    /// Affects the structure or hierarchy of the view tree, e.g. by making a node conditional or repeated.
    /// </summary>
    Structural,
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
    /// A read-only binding which obtains the value from a named game asset.
    /// </summary>
    AssetBinding,

    /// <summary>
    /// A one-way data binding which obtains the value from the context data and assigns it to the view.
    /// </summary>
    InputBinding,

    /// <summary>
    /// A one-way data binding which obtains the value from the view and assigns it to the context data.
    /// </summary>
    OutputBinding,

    /// <summary>
    /// A two-way data binding which both assigns the context data's value to the view, and the view's value to the
    /// context data, depending on which one was most recently changed.
    /// </summary>
    TwoWayBinding,
}

/// <summary>
/// Extensions for the <see cref="AttributeValueType"/> enum.
/// </summary>
public static class AttributeValueTypeExtensions
{
    /// <summary>
    /// Tests if a given <paramref name="valueType"/> is any type of context binding, regardless of its direction.
    /// </summary>
    /// <param name="valueType">The value type.</param>
    /// <returns><c>true</c> if the attribute binds to a context property; <c>false</c> if it is some other type of
    /// attribute such as <see cref="AttributeValueType.Literal"/> or
    /// <see cref="AttributeValueType.AssetBinding"/>.</returns>
    public static bool IsContextBinding(this AttributeValueType valueType)
    {
        return valueType == AttributeValueType.InputBinding
            || valueType == AttributeValueType.OutputBinding
            || valueType == AttributeValueType.TwoWayBinding;
    }
}

/// <summary>
/// A complete attribute assignment parsed from StarML.
/// </summary>
/// <param name="name">The attribute name.</param>
/// <param name="type">The type of the attribute itself, i.e. how its <paramref name="name"/> should be
/// interpreted.</param>
/// <param name="valueType">The type of the value expression, defining how the <paramref name="value"/> should be
/// interpreted.</param>
/// <param name="value">The literal value text.</param>
/// <param name="parentDepth">The depth to walk - i.e. number of parents to traverse - to find the context on which to
/// evaluate a context binding. Only valid if the <paramref name="valueType"/> is a type that matches
/// <see cref="AttributeValueTypeExtensions.IsContextBinding"/>.</param>
public readonly ref struct Attribute(
    ReadOnlySpan<char> name,
    AttributeType type,
    AttributeValueType valueType,
    ReadOnlySpan<char> value,
    uint parentDepth,
    ReadOnlySpan<char> parentType
)
{
    /// <summary>
    /// The attribute name.
    /// </summary>
    public ReadOnlySpan<char> Name { get; } = name;

    /// <summary>
    /// The depth to walk - i.e. number of parents to traverse - to find the context on which to evaluate a context
    /// binding. Exclusive with <see cref="ParentType"/> and only valid if the <paramref name="valueType"/> is a type
    /// that matches <see cref="AttributeValueTypeExtensions.IsContextBinding"/>.
    /// </summary>
    public uint ParentDepth { get; } = parentDepth;

    /// <summary>
    /// The type name of the parent to walk up to for a context redirect. Exclusive with <see cref="ParentDepth"/> and
    /// only valid if the <paramref name="valueType"/> is a type that matches
    /// <see cref="AttributeValueTypeExtensions.IsContextBinding"/>.
    /// </summary>
    public ReadOnlySpan<char> ParentType { get; } = parentType;

    /// <summary>
    /// The type of the attribute itself, i.e. how its <see cref="Name"/> should be interpreted.
    /// </summary>
    public AttributeType Type { get; } = type;

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
    /// Whether the end of the document has been reached.
    /// </summary>
    public readonly bool Eof => lexer.Eof;

    /// <summary>
    /// The current position in the document content.
    /// </summary>
    public readonly int Position => lexer.Position;

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
        lexer.ReadRequiredToken(
            TokenType.Name,
            TokenType.AttributeModifier,
            TokenType.TagEnd,
            TokenType.SelfClosingTagEnd
        );
        var attributeType = AttributeType.Property;
        switch (lexer.Current.Type)
        {
            case TokenType.AttributeModifier:
                attributeType = GetAttributeType(lexer.Current.Text);
                lexer.ReadRequiredToken(TokenType.Name);
                goto case TokenType.Name;
            case TokenType.Name:
                var attributeName = lexer.Current.Text;
                lexer.ReadRequiredToken(TokenType.Assignment);
                lexer.ReadRequiredToken(TokenType.Quote, TokenType.BindingStart);
                var valueType =
                    lexer.Current.Type == TokenType.BindingStart
                        ? AttributeValueType.InputBinding
                        : AttributeValueType.Literal;
                lexer.ReadRequiredToken(
                    TokenType.BindingModifier,
                    TokenType.BindingParentImmediate,
                    TokenType.BindingParentIndirect,
                    TokenType.Literal
                );
                if (lexer.Current.Type == TokenType.BindingModifier)
                {
                    valueType = GetBindingType(lexer.Current.Text);
                    lexer.ReadRequiredToken(
                        TokenType.BindingParentImmediate,
                        TokenType.BindingParentIndirect,
                        TokenType.Literal
                    );
                }
                uint parentDepth = 0;
                var parentType = ReadOnlySpan<char>.Empty;
                if (lexer.Current.Type == TokenType.BindingParentImmediate)
                {
                    if (!valueType.IsContextBinding())
                    {
                        throw ParserException.ForCurrentToken(
                            lexer,
                            $"Parent context modifier ({lexer.Current.Text}) is not allowed for attributes with type "
                                + $"{valueType}; the attribute must be an input, output or 2-way context binding."
                        );
                    }
                    do
                    {
                        parentDepth++;
                        lexer.ReadRequiredToken(TokenType.BindingParentImmediate, TokenType.Literal);
                    } while (lexer.Current.Type == TokenType.BindingParentImmediate);
                }
                else if (lexer.Current.Type == TokenType.BindingParentIndirect)
                {
                    if (!valueType.IsContextBinding())
                    {
                        throw ParserException.ForCurrentToken(
                            lexer,
                            $"Parent context modifier ({lexer.Current.Text}) is not allowed for attributes with type "
                                + $"{valueType}; the attribute must be an input, output or 2-way context binding."
                        );
                    }
                    lexer.ReadRequiredToken(TokenType.Literal);
                    parentType = lexer.Current.Text;
                    lexer.ReadRequiredToken(TokenType.NameSeparator);
                    lexer.ReadRequiredToken(TokenType.Literal);
                }
                var attributeValue = lexer.Current.Text;
                // We don't bother trying to enforce that the end token matches the start token because the Lexer will
                // have already failed to parse the literal if it doesn't match.
                lexer.ReadRequiredToken(TokenType.Quote, TokenType.BindingEnd);
                Attribute = new(attributeName, attributeType, valueType, attributeValue, parentDepth, parentType);
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

    private static AttributeType GetAttributeType(in ReadOnlySpan<char> token)
    {
        return token switch
        {
            "*" => AttributeType.Structural,
            _ => throw new ArgumentException($"Invalid attribute modifier: {token}", nameof(token)),
        };
    }

    private static AttributeValueType GetBindingType(in ReadOnlySpan<char> token)
    {
        return token switch
        {
            "<>" => AttributeValueType.TwoWayBinding,
            "<" => AttributeValueType.InputBinding,
            ">" => AttributeValueType.OutputBinding,
            "@" => AttributeValueType.AssetBinding,
            _ => throw new ArgumentException($"Invalid binding modifier: {token}", nameof(token)),
        };
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
