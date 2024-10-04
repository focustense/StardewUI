using StardewUI.Framework.Grammar;

namespace StardewUI.Framework.Dom;

/// <summary>
/// A node in a StarML document, encapsulating the tag, its attributes, and all child nodes.
/// </summary>
/// <remarks>
/// This is also the root of a <see cref="Document"/> and the visible result of a parser. While there is some memory and
/// performance cost associated with this intermediate representation before assembling the
/// <see cref="Binding.ViewNode"/>, it allows for document assets to be edited (patched) prior to binding.
/// </remarks>
/// <param name="Element">The element data for this node.</param>
/// <param name="ChildNodes">The children of this node.</param>
public record SNode(SElement Element, IReadOnlyList<SNode> ChildNodes)
{
    /// <summary>
    /// Gets the attributes of the associated <see cref="Element"/>.
    /// </summary>
    public IReadOnlyList<SAttribute> Attributes => Element.Attributes;

    /// <summary>
    /// Gets the tag of the associated <see cref="Element"/>.
    /// </summary>
    public string Tag => Element.Tag;

    private static readonly IReadOnlyList<SNode> EmptyChildren = [];

    /// <summary>
    /// Parses a node from the current state of a document reader.
    /// </summary>
    /// <param name="reader">The reader state, represent the content and current position.</param>
    /// <returns></returns>
    /// <exception cref="ParserException">Thrown when any invalid markup is encountered.</exception>
    internal static SNode Parse(ref DocumentReader reader)
    {
        int position = reader.Position;
        if (!reader.NextTag())
        {
            throw new ParserException("Document is missing root element.", position);
        }
        return Parse(ref reader, reader.Tag.Name);
    }

    private static SNode Parse(ref DocumentReader reader, ReadOnlySpan<char> openingTagName)
    {
        int tagStartPosition = reader.Position;
        var attributes = new List<SAttribute>();
        var events = new List<SEvent>();
        while (true)
        {
            switch (reader.NextMember())
            {
                case TagMember.Attribute:
                    attributes.Add(new(reader.Attribute));
                    break;
                case TagMember.Event:
                    var arguments = new List<SArgument>();
                    while (reader.NextArgument())
                    {
                        arguments.Add(new(reader.Argument));
                    }
                    events.Add(new(reader.Event, arguments));
                    break;
                default:
                    // Yes, we have to use a goto. A local function would be cleaner, but isn't allowed with the use of
                    // ref struct param.
                    goto TagFinished;
            }
        }
        TagFinished:
        // In most documents, the majority of nodes won't have children, so try to avoid allocating the list at all
        // until we know we need it.
        List<SNode>? childNodes = null;
        int tagEndPosition = reader.Position;
        while (reader.NextTag())
        {
            if (reader.Tag.IsClosingTag)
            {
                if (!reader.Tag.Name.Equals(openingTagName, StringComparison.OrdinalIgnoreCase))
                {
                    throw new ParserException(
                        $"Closing tag '{reader.Tag.Name}' at position {tagEndPosition} does not match opening tag "
                            + $"'{openingTagName}' starting at position {tagStartPosition}.",
                        tagEndPosition
                    );
                }
                return new(new(openingTagName.ToString(), attributes, events), childNodes ?? EmptyChildren);
            }
            else
            {
                childNodes ??= [];
                childNodes.Add(Parse(ref reader, reader.Tag.Name));
            }
            tagEndPosition = reader.Position;
        }
        throw new ParserException(
            $"Unclosed tag '{openingTagName}' beginning at position {tagStartPosition}.",
            reader.Position
        );
    }
}
