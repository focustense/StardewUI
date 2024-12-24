namespace StardewUI.Framework.Grammar;

/// <summary>
/// Information about a parsed tag in StarML.
/// </summary>
/// <param name="name">The tag name.</param>
/// <param name="isClosingTag">Whether or not the tag is a closing tag, either in regular <c>&lt;/tag&gt;</c> form or
/// the end of a self-closing tag (<c>/&gt;</c>) after the tag attributes.</param>
public readonly ref struct TagInfo(ReadOnlySpan<char> name, bool isClosingTag)
{
    /// <summary>
    /// The tag name.
    /// </summary>
    public ReadOnlySpan<char> Name { get; } = name;

    /// <summary>
    /// Whether or not the tag is a closing tag, either in regular <c>&lt;/tag&gt;</c> form or the end of a self-closing
    /// tag (<c>/&gt;</c>) after the tag attributes.
    /// </summary>
    public bool IsClosingTag { get; } = isClosingTag;
}

/// <summary>
/// The type of tag member read, resulting from a call to <see cref="DocumentReader.NextMember"/>.
/// </summary>
public enum TagMember
{
    /// <summary>
    /// No member was read, i.e. the reader reached the end of the tag.
    /// </summary>
    None,

    /// <summary>
    /// A regular attribute, which binds or writes to a property of the target view.
    /// </summary>
    Attribute,

    /// <summary>
    /// An event attribute, which attaches an event handler to the target view.
    /// </summary>
    Event,
}

/// <summary>
/// Reads elements and associated attributes from a StarML document (content string).
/// </summary>
/// <param name="lexer">The lexer that reads syntax tokens from the document.</param>
public ref struct DocumentReader(Lexer lexer)
{
    /// <summary>
    /// The argument that was just read, if the previous <see cref="NextArgument"/> returned <c>true</c>; otherwise, an
    /// empty argument.
    /// </summary>
    public Argument Argument { get; private set; }

    /// <summary>
    /// The attribute that was just read, if the previous <see cref="NextMember"/> returned
    /// <see cref="TagMember.Attribute"/>; otherwise, an empty attribute.
    /// </summary>
    public Attribute Attribute { get; private set; }

    /// <summary>
    /// Whether the end of the document has been reached.
    /// </summary>
    public readonly bool Eof => lexer.Eof;

    /// <summary>
    /// The event that was just read, if the previous <see cref="NextMember"/> returned <see cref="TagMember.Event"/>;
    /// otherwise, an empty event.
    /// </summary>
    public Event Event { get; private set; }

    /// <summary>
    /// The current position in the document content.
    /// </summary>
    public readonly int Position => lexer.Position;

    /// <summary>
    /// The tag that was just read, if the previous <see cref="NextTag"/> returned <c>true</c>; otherwise, an empty tag.
    /// </summary>
    /// <remarks>
    /// The tag remains valid as attributes are read; i.e. <see cref="ReadNextAttribute"/> will never change this value.
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
    /// Reads the next <see cref="Argument"/>, if the current scope is within an argument list.
    /// </summary>
    /// <returns><c>true</c> if an argument was read; <c>false</c> if there are no more arguments in the list or if the
    /// current position was not within an argument list.</returns>
    public bool NextArgument()
    {
        // Doing this check allows the "read remaining arguments" branch in NextMember to work correctly even if a
        // caller already read to the end of the argument list.
        if (lexer.Current.Type != TokenType.ArgumentListEnd)
        {
            lexer.ReadRequiredToken(
                TokenType.Quote,
                TokenType.ContextParent,
                TokenType.ContextAncestor,
                TokenType.ArgumentPrefix,
                TokenType.Name,
                TokenType.ArgumentListEnd
            );
        }
        var defaultExpressionType = ArgumentExpressionType.ContextBinding;
        switch (lexer.Current.Type)
        {
            case TokenType.ArgumentListEnd:
                Argument = default;
                return false;
            case TokenType.Quote:
                lexer.ReadRequiredToken(TokenType.Literal);
                var quotedExpression = lexer.Current.Text;
                lexer.ReadRequiredToken(TokenType.Quote);
                lexer.ReadRequiredToken(TokenType.ArgumentSeparator, TokenType.ArgumentListEnd);
                Argument = new(ArgumentExpressionType.Literal, quotedExpression, 0, []);
                return true;
            case TokenType.ArgumentPrefix:
                defaultExpressionType = lexer.Current.Text switch
                {
                    "$" => ArgumentExpressionType.EventBinding,
                    "&" => ArgumentExpressionType.TemplateBinding,
                    _ => throw ParserException.ForCurrentToken(
                        lexer,
                        $"Invalid prefix '{lexer.Current.Text}' found for method argument."
                    ),
                };
                lexer.ReadRequiredToken(TokenType.Name);
                goto default;
            default:
                ReadContextRedirectTokens(
                    AttributeValueType.InputBinding,
                    TokenType.Name,
                    out var parentDepth,
                    out var parentType
                );
                var expression = lexer.Current.Text;
                lexer.ReadRequiredToken(TokenType.ArgumentSeparator, TokenType.ArgumentListEnd);
                Argument = new(defaultExpressionType, expression, parentDepth, parentType);
                return true;
        }
    }

    /// <summary>
    /// Reads the next <see cref="Attribute"/>. Only valid in a tag scope, i.e. after a call to <see cref="NextTag"/>
    /// returns <c>true</c>.
    /// </summary>
    /// <returns><c>true</c> if an attribute was read; <c>false</c> if there are no more attributes to read for the
    /// current element.</returns>
    /// <exception cref="ParserException">Thrown when the current position is not within a tag, or when unparseable
    /// attribute data is encountered.</exception>
    public TagMember NextMember()
    {
        if (Tag.Name.Length == 0)
        {
            throw new ParserException("Cannot read an attribute when outside of an element.", lexer.Position);
        }
        if (Tag.IsClosingTag)
        {
            return TagMember.None;
        }
        if (lexer.Current.Type == TokenType.ArgumentListStart || !Argument.Expression.IsEmpty)
        {
            while (NextArgument()) { }
        }
        if (lexer.Current.Type == TokenType.ArgumentListEnd)
        {
            if (Event.EventName.Length > 0)
            {
                lexer.ReadRequiredToken(TokenType.Pipe);
            }
        }
        lexer.ReadRequiredToken(
            TokenType.Name,
            TokenType.AttributeModifier,
            TokenType.TagEnd,
            TokenType.SelfClosingTagEnd
        );
        Attribute = default;
        Event = default;
        var attributeType = AttributeType.Property;
        bool isNegated = false;
        switch (lexer.Current.Type)
        {
            case TokenType.AttributeModifier:
                attributeType = GetAttributeType(lexer.Current.Text);
                lexer.ReadRequiredToken(TokenType.Name, TokenType.Negation);
                if (lexer.Current.Type == TokenType.Negation)
                {
                    isNegated = true;
                    lexer.ReadRequiredToken(TokenType.Name);
                }
                goto case TokenType.Name;
            case TokenType.Name:
                var attributeName = lexer.Current.Text;
                lexer.ReadRequiredToken(TokenType.Assignment);
                lexer.ReadRequiredToken(TokenType.Quote, TokenType.BindingStart, TokenType.Pipe);
                if (lexer.Current.Type == TokenType.Pipe)
                {
                    if (attributeType != AttributeType.Property)
                    {
                        throw ParserException.ForCurrentToken(
                            lexer,
                            $"Invalid event binding specified for attribute with type {attributeType}. "
                                + "Events are only allowed on normal, undecorated attribute names."
                        );
                    }
                    ReadNextEvent(attributeName);
                    return TagMember.Event;
                }
                else
                {
                    ReadNextAttribute(attributeType, attributeName, isNegated);
                    return TagMember.Attribute;
                }
            case TokenType.SelfClosingTagEnd:
                wasTagSelfClosed = true;
                goto default;
            default:
                Attribute = default;
                Event = default;
                return TagMember.None;
        }
    }

    /// <summary>
    /// Reads the next <see cref="Tag"/>, discarding any remaining attributes for the current tag.
    /// </summary>
    /// <returns><c>true</c> if an attribute was read; <c>false</c> if the end of the document was reached.</returns>
    /// <exception cref="ParserException">Thrown when the next tag is malformed or otherwise unparseable.</exception>
    public bool NextTag()
    {
        if (!Attribute.Name.IsEmpty || !Event.EventName.IsEmpty)
        {
            while (NextMember() != TagMember.None) { }
        }
        if (wasTagSelfClosed)
        {
            Tag = new(Tag.Name, true);
            wasTagSelfClosed = false;
            return true;
        }
        do
        {
            if (!lexer.ReadOptionalToken(TokenType.OpeningTagStart, TokenType.ClosingTagStart, TokenType.CommentStart))
            {
                Tag = default;
                return false;
            }
            if (lexer.Current.Type == TokenType.CommentStart)
            {
                // Consume the comment. Currently we don't include these in the parsed document.
                lexer.ReadRequiredToken(TokenType.Literal);
                lexer.ReadRequiredToken(TokenType.CommentEnd);
            }
        } while (lexer.Current.Type == TokenType.CommentEnd);
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
            "+" => AttributeType.Behavior,
            _ => throw new ArgumentException($"Invalid attribute modifier: {token}", nameof(token)),
        };
    }

    private static AttributeValueType GetBindingType(in ReadOnlySpan<char> token)
    {
        return token switch
        {
            "<>" => AttributeValueType.TwoWayBinding,
            "<:" or ":" => AttributeValueType.OneTimeBinding,
            "<" => AttributeValueType.InputBinding,
            ">" => AttributeValueType.OutputBinding,
            "@" => AttributeValueType.AssetBinding,
            "#" => AttributeValueType.TranslationBinding,
            "&" => AttributeValueType.TemplateBinding,
            _ => throw new ArgumentException($"Invalid binding modifier: {token}", nameof(token)),
        };
    }

    private AttributeValueType ReadContextRedirectTokens(
        AttributeValueType valueType,
        TokenType expressionTokenType,
        out uint parentDepth,
        out ReadOnlySpan<char> parentType
    )
    {
        parentDepth = 0;
        parentType = [];
        if (lexer.Current.Type == TokenType.ContextParent)
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
                lexer.ReadRequiredToken(TokenType.ContextParent, expressionTokenType);
            } while (lexer.Current.Type == TokenType.ContextParent);
        }
        else if (lexer.Current.Type == TokenType.ContextAncestor)
        {
            if (!valueType.IsContextBinding())
            {
                throw ParserException.ForCurrentToken(
                    lexer,
                    $"Parent context modifier ({lexer.Current.Text}) is not allowed for attributes with type "
                        + $"{valueType}; the attribute must be an input, output or 2-way context binding."
                );
            }
            lexer.ReadRequiredToken(expressionTokenType);
            parentType = lexer.Current.Text;
            lexer.ReadRequiredToken(TokenType.NameSeparator);
            lexer.ReadRequiredToken(expressionTokenType);
        }
        return valueType;
    }

    private void ReadNextAttribute(AttributeType type, ReadOnlySpan<char> name, bool isNegated)
    {
        var valueType =
            lexer.Current.Type == TokenType.BindingStart ? AttributeValueType.InputBinding : AttributeValueType.Literal;
        lexer.ReadRequiredToken(
            TokenType.BindingModifier,
            TokenType.ContextParent,
            TokenType.ContextAncestor,
            TokenType.Literal,
            TokenType.Quote
        );
        if (lexer.Current.Type == TokenType.Quote)
        {
            Attribute = new(name, type, isNegated, valueType, "", 0, "");
            return;
        }
        if (lexer.Current.Type == TokenType.BindingModifier)
        {
            valueType = GetBindingType(lexer.Current.Text);
            lexer.ReadRequiredToken(TokenType.ContextParent, TokenType.ContextAncestor, TokenType.Literal);
        }
        ReadContextRedirectTokens(valueType, TokenType.Literal, out var parentDepth, out var parentType);
        var attributeValue = lexer.Current.Text;
        // We don't bother trying to enforce that the end token matches the start token because the Lexer will
        // have already failed to parse the literal if it doesn't match.
        lexer.ReadRequiredToken(TokenType.Quote, TokenType.BindingEnd);
        Attribute = new(name, type, isNegated, valueType, attributeValue, parentDepth, parentType);
    }

    private void ReadNextEvent(ReadOnlySpan<char> name)
    {
        lexer.ReadRequiredToken(TokenType.ContextParent, TokenType.ContextAncestor, TokenType.Name);
        ReadContextRedirectTokens(
            AttributeValueType.InputBinding,
            TokenType.Name,
            out var parentDepth,
            out var parentType
        );
        var handlerName = lexer.Current.Text;
        lexer.ReadRequiredToken(TokenType.ArgumentListStart);
        Event = new(name, handlerName, parentDepth, parentType);
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
