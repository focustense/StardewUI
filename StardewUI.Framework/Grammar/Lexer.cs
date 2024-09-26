using System.Runtime.CompilerServices;
using System.Text;

namespace StardewUI.Framework.Grammar;

public enum TokenType
{
    Unknown,
    OpeningTagStart,
    ClosingTagStart,
    TagEnd,
    SelfClosingTagEnd,
    Name,
    Literal,
    Assignment,
    Quote,
    BindingStart,
    BindingEnd,
}

public readonly ref struct Token(TokenType type, ReadOnlySpan<char> text)
{
    public ReadOnlySpan<char> Text { get; } = text;
    public TokenType Type { get; } = type;

    public override string ToString()
    {
        return $"[{Type}, '{Text}']";
    }
}

public ref struct Lexer(ReadOnlySpan<char> text)
{
    enum Mode
    {
        Default,
        Quoted,
        Binding,
    }

    readonly record struct TokenInfo(TokenType Type, int Length);

    public Token Current { get; private set; }
    public readonly int Position => position;

    private static readonly Rune HYPHEN = new('-');
    private static readonly Rune UNDERSCORE = new('_');

    private Mode mode;
    private int position;
    private ReadOnlySpan<char> text = text;

    public readonly Lexer GetEnumerator()
    {
        return this;
    }

    public bool MoveNext()
    {
        int previousLength = text.Length;
        text = text.TrimStart();
        if (text.Length == 0)
        {
            return false;
        }
        position += previousLength - text.Length;
        var (tokenType, tokenLength) = ReadNextToken();
        switch (tokenType)
        {
            case TokenType.Quote:
                mode = mode == Mode.Quoted ? Mode.Default : Mode.Quoted;
                break;
            case TokenType.BindingStart:
                mode = Mode.Binding;
                break;
            case TokenType.BindingEnd:
                mode = Mode.Default;
                break;
        }
        Current = new(tokenType, text[..tokenLength]);
        text = text[tokenLength..];
        position += tokenLength;
        return true;
    }

    public bool ReadOptionalToken(params TokenType[] expectedTypes)
    {
        return MoveNext(false, expectedTypes);
    }

    public void ReadRequiredToken(params TokenType[] expectedTypes)
    {
        MoveNext(true, expectedTypes);
    }

    private bool MoveNext(bool isRequired, TokenType[] expectedTypes)
    {
        int previousPosition = position;
        if (!MoveNext())
        {
            if (!isRequired)
            {
                return false;
            }
            throw new LexerException(
                $"Missing token(s) at end of content. Expected one of the following: {FormatExpectedTypes()}",
                previousPosition
            );
        }
        else if (!expectedTypes.Contains(Current.Type))
        {
            throw new LexerException(
                $"Invalid token at position {previousPosition}. Expected one of the following: {FormatExpectedTypes()}",
                previousPosition
            );
        }
        return true;

        string FormatExpectedTypes()
        {
            return "[" + string.Join(", ", expectedTypes) + "]";
        }
    }

    private readonly TokenInfo ReadNextToken()
    {
        return mode switch
        {
            Mode.Quoted => text switch
            {
                ['"', ..] => new(TokenType.Quote, 1),
                _ => ReadLiteralStringUntil("\""),
            },
            Mode.Binding => text switch
            {
                ['}', '}', ..] => new(TokenType.BindingEnd, 2),
                _ => ReadLiteralStringUntil("}}"),
            },
            _ => text switch
            {
                ['<', '/', ..] => new(TokenType.ClosingTagStart, 2),
                ['<', ..] => new(TokenType.OpeningTagStart, 1),
                ['/', '>', ..] => new(TokenType.SelfClosingTagEnd, 2),
                ['>', ..] => new(TokenType.TagEnd, 1),
                ['=', ..] => new(TokenType.Assignment, 1),
                ['"', ..] => new(TokenType.Quote, 1),
                ['{', '{', ..] => new(TokenType.BindingStart, 2),
                ['}', '}', ..] => new(TokenType.BindingEnd, 2),
                _ => ReadName(),
            },
        };
    }

    private readonly TokenInfo ReadLiteralStringUntil(ReadOnlySpan<char> terminator)
    {
        var terminatorIndex = text.IndexOf(terminator);
        if (terminatorIndex < 0)
        {
            throw LexerException($"Unterminated literal; expected closing '{terminator}'.");
        }
        return new(TokenType.Literal, terminatorIndex);
    }

    private readonly TokenInfo ReadName()
    {
        if (text.Length == 0)
        {
            throw LexerException("Invalid opening/closing tag at end of text.");
        }
        int length = 0;
        foreach (var rune in text.EnumerateRunes())
        {
            if (length == 0 && !Rune.IsLetter(rune))
            {
                throw LexerException($"Invalid tag name; must start with a letter, but found {rune}.");
            }
            if (!IsValidForName(rune))
            {
                break;
            }
            length += rune.Utf16SequenceLength;
        }
        return length > 0
            ? new(TokenType.Name, length)
            : throw LexerException("Reached end of text while reading name.");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsValidForName(Rune rune)
    {
        return Rune.IsLetterOrDigit(rune) || rune == HYPHEN || rune == UNDERSCORE;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private readonly Exception LexerException(string message, int positionOffset = 0)
    {
        return new LexerException(message, position + positionOffset);
    }
}

public class LexerException(string message, int position) : Exception(message)
{
    public int Position { get; } = position;
}
