namespace StardewUI.Framework.Grammar;

public readonly ref struct Attribute(ReadOnlySpan<char> name, AttributeValueType valueType, ReadOnlySpan<char> value)
{
    public ReadOnlySpan<char> Name { get; } = name;
    public ReadOnlySpan<char> Value { get; } = value;
    public AttributeValueType ValueType { get; } = valueType;
}

public enum AttributeValueType
{
    Literal,
    Binding,
}

public readonly ref struct TagInfo(ReadOnlySpan<char> name, bool isClosingTag)
{
    public ReadOnlySpan<char> Name { get; } = name;
    public bool IsClosingTag { get; } = isClosingTag;
}

public ref struct DocumentReader(Lexer lexer)
{
    public Attribute Attribute { get; private set; }
    public TagInfo Tag { get; private set; }

    private Lexer lexer = lexer;
    private bool wasTagSelfClosed;

    public DocumentReader(ReadOnlySpan<char> text)
        : this(new Lexer(text)) { }

    public bool NextAttribute()
    {
        if (Tag.Name.Length == 0)
        {
            throw new InvalidOperationException("Cannot read an attribute when outside of an element.");
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

    public bool NextElement()
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

public class ParserException(string message, int position) : Exception(message)
{
    public static ParserException ForCurrentToken(in Lexer lexer, string message)
    {
        var tokenPosition = lexer.Position - lexer.Current.Text.Length;
        return new(message, tokenPosition);
    }

    public int Position { get; } = position;
}
