using System.Text;

namespace StardewUI.Framework.Dom;

/// <summary>
/// Element in a StarML document, including the tag and all enclosed attributes.
/// </summary>
public interface IElement
{
    /// <summary>
    /// The parsed tag name.
    /// </summary>
    string Tag { get; }

    /// <summary>
    /// The parsed list of attributes applied to this instance of the tag.
    /// </summary>
    IReadOnlyList<IAttribute> Attributes { get; }

    /// <summary>
    /// The parsed list of events applied to this instance of the tag.
    /// </summary>
    IReadOnlyList<IEvent> Events { get; }

    /// <summary>
    /// Prints the textual representation of this element.
    /// </summary>
    /// <param name="sb">Builder to receive the element's text output.</param>
    /// <param name="asSelfClosing">Whether to print the element as a self-closing tag, i.e. whether to include a
    /// <c>/</c> character before the closing <c>&gt;</c>.</param>
    void Print(StringBuilder sb, bool asSelfClosing = false)
    {
        sb.Append('<');
        sb.Append(Tag);
        foreach (var attribute in Attributes)
        {
            sb.Append(' ');
            attribute.Print(sb);
        }
        foreach (var @event in Events)
        {
            sb.Append(' ');
            @event.Print(sb);
        }
        if (asSelfClosing)
        {
            sb.Append('/');
        }
        sb.Append('>');
    }

    /// <summary>
    /// Prints the closing tag for this element.
    /// </summary>
    /// <param name="sb">Builder to receive the element's text output.</param>
    void PrintClosingTag(StringBuilder sb)
    {
        sb.Append("</");
        sb.Append(Tag);
        sb.Append('>');
    }
}

/// <summary>
/// Record implementation of a StarML <see cref="IElement"/>.
/// </summary>
/// <param name="Tag">The tag name.</param>
/// <param name="Attributes">The attributes applied to this tag.</param>
/// <param name="Events">The events applied to this tag.</param>
public record SElement(string Tag, IReadOnlyList<SAttribute> Attributes, IReadOnlyList<SEvent> Events) : IElement
{
    IReadOnlyList<IAttribute> IElement.Attributes => Attributes;

    IReadOnlyList<IEvent> IElement.Events => Events;
}
