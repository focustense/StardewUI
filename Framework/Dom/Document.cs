using StardewUI.Framework.Grammar;

namespace StardewUI.Framework.Dom;

/// <summary>
/// A standalone StarML document.
/// </summary>
/// <param name="Root">The top-level node.</param>
public record Document(SNode Root)
{
    /// <summary>
    /// Parses a <see cref="Document"/> from its original markup text.
    /// </summary>
    /// <param name="text">The StarML markup text.</param>
    /// <returns>The parsed document as a DOM tree.</returns>
    /// <exception cref="ParserException">Thrown when any invalid markup is encountered.</exception>
    public static Document Parse(ReadOnlySpan<char> text)
    {
        var reader = new DocumentReader(text);
        var root = SNode.Parse(ref reader);
        if (!reader.Eof)
        {
            throw new ParserException(
                "Invalid content at end of string. StarML documents must have exactly one root element and no "
                    + "other trailing content except whitespace.",
                reader.Position
            );
        }
        return new(root);
    }
}
