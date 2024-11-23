﻿using System.Runtime.CompilerServices;
using System.Text;

namespace StardewUI.Framework.Grammar;

/// <summary>
/// Types of tokens allowed in StarML.
/// </summary>
public enum TokenType
{
    /// <summary>
    /// Unknown token; used when a lexer has not been initialized, or has reached the end of its content.
    /// </summary>
    Unknown,

    /// <summary>
    /// Start of an opening tag, i.e. the <c>&lt;</c> character without a subsequent <c>/</c>.
    /// </summary>
    OpeningTagStart,

    /// <summary>
    /// Start of a closing tag, i.e. the <c>&lt;/</c> character sequence.
    /// </summary>
    ClosingTagStart,

    /// <summary>
    /// End of a regular opening or closing tag, i.e. the <c>&gt;</c> character.
    /// </summary>
    TagEnd,

    /// <summary>
    /// End of a self-closing tag, i.e. the <c>/&gt;</c> character sequence.
    /// </summary>
    SelfClosingTagEnd,

    /// <summary>
    /// A valid name, i.e. of an element (tag) or attribute.
    /// </summary>
    Name,

    /// <summary>
    /// The dot (<c>.</c>) character used to separate components of a name.
    /// </summary>
    NameSeparator,

    /// <summary>
    /// Modifier token designating the type of an attribute; the <c>*</c> character (structural).
    /// </summary>
    AttributeModifier,

    /// <summary>
    /// A string of literal text, as found within a quoted or bound attribute.
    /// </summary>
    Literal,

    /// <summary>
    /// The <c>=</c> character, as used in an attribute syntax such as <c>attr="value"</c>.
    /// </summary>
    Assignment,

    /// <summary>
    /// Double quote character (<c>"</c>) used to start or terminate a <see cref="Literal"/> string.
    /// </summary>
    Quote,

    /// <summary>
    /// A pair of opening braces (<c>{{</c>), used to start a binding expression for an attribute value.
    /// </summary>
    BindingStart,

    /// <summary>
    /// A pair of closing braces (<c>}}</c>), used to end a binding expression for an attribute value.
    /// </summary>
    BindingEnd,

    /// <summary>
    /// An explicit binding modifier; one of <c>@</c> (asset), <c>#</c> (translation), <c>&amp;</c> (template),
    /// <c>&lt;</c> (input only), <c>&gt;</c> (output only) or <c>&lt;&gt;</c> (two-way).
    /// </summary>
    BindingModifier,

    /// <summary>
    /// A caret (<c>^</c>) used in a binding expression, indicating a walk up to the parent context.
    /// </summary>
    ContextParent,

    /// <summary>
    /// A tilde (<c>~</c>) used in a binding expression, indicating traversal up to a parent with a named type.
    /// </summary>
    ContextAncestor,

    /// <summary>
    /// The pipe (<c>|</c>) character, which is used to start and end event bindings.
    /// </summary>
    Pipe,

    /// <summary>
    /// The left parenthesis (<c>(</c>) character, used to start an argument list.
    /// </summary>
    ArgumentListStart,

    /// <summary>
    /// The right parenthesis (<c>(</c>) character, used to end an argument list.
    /// </summary>
    ArgumentListEnd,

    /// <summary>
    /// Prefix character for an argument, e.g. <c>$</c> to refer to an event property.
    /// </summary>
    ArgumentPrefix,

    /// <summary>
    /// The comma (<c>,</c>) character, used to separator arguments in an argument list.
    /// </summary>
    ArgumentSeparator,

    /// <summary>
    /// Slash followed by asterisk (<c>/*</c>) indicating start of a comment block.
    /// </summary>
    CommentStart,

    /// <summary>
    /// Asterisk followed by slash (<c>*/</c>) indicating end of a comment block.
    /// </summary>
    CommentEnd,
}

/// <summary>
/// A token emitted by the StarML <see cref="Lexer"/>.
/// </summary>
/// <param name="type">The token type.</param>
/// <param name="text">The exact text of the token in the original markup.</param>
public readonly ref struct Token(TokenType type, ReadOnlySpan<char> text)
{
    /// <summary>
    /// The token type.
    /// </summary>
    public ReadOnlySpan<char> Text { get; } = text;

    /// <summary>
    /// The exact text of the token in the original markup.
    /// </summary>
    public TokenType Type { get; } = type;

    /// <inheritdoc />
    public override string ToString()
    {
        return $"[{Type}, '{Text}']";
    }
}

/// <summary>
/// Consumes raw StarML content as a token stream.
/// </summary>
/// <param name="text">The markup text.</param>
public ref struct Lexer(ReadOnlySpan<char> text)
{
    enum Mode
    {
        Default,
        Quoted,
        Binding,
        Event,
        ArgumentList,
        Comment,
    }

    readonly record struct TokenInfo(TokenType Type, int Length);

    /// <summary>
    /// The most recent token that was read, if the previous call to <see cref="MoveNext"/> was successful; otherwise,
    /// an empty token.
    /// </summary>
    public Token Current { get; private set; }

    /// <summary>
    /// Whether the lexer is at the end of the content, either at the very end or with only trailing whitespace.
    /// </summary>
    public readonly bool Eof => text.IsWhiteSpace();

    /// <summary>
    /// The current position in the markup text, i.e. the position at the <em>end</em> the <see cref="Current"/> token.
    /// </summary>
    public readonly int Position => position;

    private static readonly Rune HYPHEN = new('-');
    private static readonly Rune UNDERSCORE = new('_');

    private Mode mode;
    private bool nameSeparatorEnabled;
    private Mode parentMode;
    private int position;
    private ReadOnlySpan<char> text = text;

    /// <summary>
    /// Returns a reference to this <see cref="Lexer"/>.
    /// </summary>
    /// <remarks>
    /// Implementing this, along with <see cref="Current"/> and <see cref="MoveNext()"/>, allows it to be used in a
    /// <c>foreach</c> loop without having to implement <see cref="IEnumerable{T}"/>, which is not allowed on a <c>ref
    /// struct</c>.
    /// </remarks>
    public readonly Lexer GetEnumerator()
    {
        return this;
    }

    /// <summary>
    /// Reads the next token into <see cref="Current"/> and advances the <see cref="Position"/>.
    /// </summary>
    /// <returns><c>true</c> if a token was read; <c>false</c> if the end of the content was reached.</returns>
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
                if (mode == Mode.Quoted)
                {
                    mode = parentMode;
                    parentMode = Mode.Default;
                }
                else
                {
                    parentMode = mode;
                    mode = Mode.Quoted;
                }
                break;
            case TokenType.BindingStart:
                mode = Mode.Binding;
                break;
            case TokenType.BindingEnd:
                mode = Mode.Default;
                nameSeparatorEnabled = false;
                break;
            case TokenType.ContextAncestor when mode == Mode.Binding || mode == Mode.ArgumentList:
                nameSeparatorEnabled = true;
                break;
            case TokenType.Pipe:
                mode = mode == Mode.Event ? Mode.Default : Mode.Event;
                break;
            case TokenType.ArgumentListStart:
                mode = Mode.ArgumentList;
                break;
            case TokenType.ArgumentSeparator:
                nameSeparatorEnabled = false;
                break;
            case TokenType.ArgumentListEnd:
                nameSeparatorEnabled = false;
                mode = Mode.Event;
                break;
            case TokenType.CommentStart:
                parentMode = mode;
                mode = Mode.Comment;
                break;
            case TokenType.CommentEnd:
                mode = parentMode;
                break;
        }
        Current = new(tokenType, text[..tokenLength]);
        text = text[tokenLength..];
        position += tokenLength;
        return true;
    }

    /// <summary>
    /// Attempts to read the next token and, if successful, validates that it has a specific type.
    /// </summary>
    /// <param name="expectedTypes">The <see cref="TokenType"/>s allowed for the next token.</param>
    /// <returns><c>true</c> if a token was read and was one of the <paramref name="expectedTypes"/>; <c>false</c> if
    /// the end of the content was reached.</returns>
    /// <exception cref="LexerException">Thrown when a token is successfully read, but does not match any of the
    /// <paramref name="expectedTypes"/>.</exception>
    public bool ReadOptionalToken(params TokenType[] expectedTypes)
    {
        return MoveNext(false, expectedTypes);
    }

    /// <summary>
    /// Reads the next token and validates that it has a specific type.
    /// </summary>
    /// <param name="expectedTypes">The <see cref="TokenType"/>s allowed for the next token.</param>
    /// <exception cref="LexerException">Thrown when a token cannot be read due to reaching the end of the content, or
    /// when a token is successfully read but does not match any of the <paramref name="expectedTypes"/>.</exception>
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

        // reached the start of a comment block, step over.
        if (Current.Type == TokenType.CommentStart)
        {
            MoveNext(); // comment literal token
            MoveNext(); // comment end token
            MoveNext(); // real next token
        }

        if (!expectedTypes.Contains(Current.Type))
        {
            throw new LexerException(
                $"Invalid {Current.Type} token at position {previousPosition}. "
                    + $"Expected one of the following: {FormatExpectedTypes()}",
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
            Mode.Comment => text switch
            {
                ['-', '-', '>', ..] => new(TokenType.CommentEnd, 3),
                _ => ReadLiteralStringUntil("-->"),
            },
            Mode.Quoted => text switch
            {
                ['"', ..] => new(TokenType.Quote, 1),
                _ => ReadLiteralStringUntil("\""),
            },
            Mode.Binding => text switch
            {
                ['}', '}', ..] => new(TokenType.BindingEnd, 2),
                ['}', ..] => new(TokenType.BindingEnd, 1),
                ['^', ..] => new(TokenType.ContextParent, 1),
                ['~', ..] => new(TokenType.ContextAncestor, 1),
                ['.', ..] => new(TokenType.NameSeparator, 1),
                ['<', '>', ..] => new(TokenType.BindingModifier, 2),
                ['<', ':', ..] => new(TokenType.BindingModifier, 2),
                [':', ..] => new(TokenType.BindingModifier, 1),
                ['<', ..] => new(TokenType.BindingModifier, 1),
                ['>', ..] => new(TokenType.BindingModifier, 1),
                ['@', ..] => new(TokenType.BindingModifier, 1),
                ['#', ..] => new(TokenType.BindingModifier, 1),
                ['&', ..] => new(TokenType.BindingModifier, 1),
                _ => nameSeparatorEnabled ? ReadLiteralStringUntil("}", ".") : ReadLiteralStringUntil("}"),
            },
            Mode.Event => text switch
            {
                ['|', ..] => new(TokenType.Pipe, 1),
                ['(', ..] => new(TokenType.ArgumentListStart, 1),
                ['^', ..] => new(TokenType.ContextParent, 1),
                ['~', ..] => new(TokenType.ContextAncestor, 1),
                ['.', ..] => new(TokenType.NameSeparator, 1),
                _ => ReadName(),
            },
            Mode.ArgumentList => text switch
            {
                [')', ..] => new(TokenType.ArgumentListEnd, 1),
                ['^', ..] => new(TokenType.ContextParent, 1),
                ['~', ..] => new(TokenType.ContextAncestor, 1),
                ['.', ..] => new(TokenType.NameSeparator, 1),
                [',', ..] => new(TokenType.ArgumentSeparator, 1),
                ['"', ..] => new(TokenType.Quote, 1),
                ['$', ..] => new(TokenType.ArgumentPrefix, 1),
                ['&', ..] => new(TokenType.ArgumentPrefix, 1),
                _ => ReadName(),
            },
            _ => text switch
            {
                ['<', '!', '-', '-', ..] => new(TokenType.CommentStart, 4),
                ['-', '-', '>', ..] => new(TokenType.CommentEnd, 3),
                ['<', '/', ..] => new(TokenType.ClosingTagStart, 2),
                ['<', ..] => new(TokenType.OpeningTagStart, 1),
                ['/', '>', ..] => new(TokenType.SelfClosingTagEnd, 2),
                ['>', ..] => new(TokenType.TagEnd, 1),
                ['=', ..] => new(TokenType.Assignment, 1),
                ['"', ..] => new(TokenType.Quote, 1),
                ['{', '{', ..] => new(TokenType.BindingStart, 2),
                ['{', ..] => new(TokenType.BindingStart, 1),
                ['}', '}', ..] => new(TokenType.BindingEnd, 2),
                ['}', ..] => new(TokenType.BindingEnd, 1),
                ['*', ..] => new(TokenType.AttributeModifier, 1),
                ['|', ..] => new(TokenType.Pipe, 1),
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

    private readonly TokenInfo ReadLiteralStringUntil(ReadOnlySpan<char> terminator1, ReadOnlySpan<char> terminator2)
    {
        var terminatorIndex1 = text.IndexOf(terminator1);
        var terminatorIndex2 = text.IndexOf(terminator2);
        int terminatorIndex =
            terminatorIndex1 < 0 ? terminatorIndex2
            : terminatorIndex2 < 0 ? terminatorIndex1
            : Math.Min(terminatorIndex1, terminatorIndex2);
        if (terminatorIndex < 0)
        {
            throw LexerException($"Unterminated literal; expected closing '{terminator1}' or '{terminator2}'.");
        }
        return new(TokenType.Literal, terminatorIndex);
    }

    private readonly TokenInfo ReadName()
    {
        int length = 0;
        foreach (var rune in text.EnumerateRunes())
        {
            if (length == 0 && !Rune.IsLetter(rune))
            {
                throw LexerException($"Invalid name; must start with a letter, but found {rune}.");
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

/// <summary>
/// The exception that is thrown when a <see cref="Lexer"/> fails to process the markup it is given.
/// </summary>
/// <param name="message">The message that describes the error.</param>
/// <param name="position">The position within the markup text where the error was encountered.</param>
public class LexerException(string message, int position) : Exception(message)
{
    /// <summary>
    /// The position within the markup text where the error was encountered.
    /// </summary>
    public int Position { get; } = position;
}
