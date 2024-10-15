using System.Text;
using StardewUI.Framework.Grammar;

namespace StardewUI.Framework.Binding;

/// <summary>
/// Encapsulates a single bound node in a view tree.
/// </summary>
public interface IViewNode : IDisposable
{
    /// <summary>
    /// The children of this node.
    /// </summary>
    /// <remarks>
    /// Node children represent views in potentia. Every DOM node maps to (at least) one <see cref="IViewNode"/>, but
    /// views are created lazily and may not exist for nodes with conditional attributes or other rules.
    /// </remarks>
    IReadOnlyList<IViewNode> ChildNodes { get; }

    /// <summary>
    /// The currently-bound context data, used as the source for any <see cref="AttributeValueType.InputBinding"/>,
    /// <see cref="AttributeValueType.OutputBinding"/> or <see cref="AttributeValueType.TwoWayBinding"/> attributes.
    /// </summary>
    BindingContext? Context { get; set; }

    /// <summary>
    /// The views for this node, if any have been created.
    /// </summary>
    IReadOnlyList<IView> Views { get; }

    /// <summary>
    /// Prints the string representation of this node.
    /// </summary>
    /// <param name="sb">The output builder to receive to the node's text.</param>
    /// <param name="includeChildren">Whether or not to include the string representation of any/all child nodes between
    /// this node's opening and close tags. If this is <c>false</c> or there are no children, it will be formatted as a
    /// self-closing tag.</param>
    void Print(StringBuilder sb, bool includeChildren);

    /// <summary>
    /// Clears any <see cref="Views"/> associated with this node and resets it to the default state before it was bound.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Propagates the request down to <see cref="ChildNodes"/>, but is not required to clear <see cref="ChildNodes"/>
    /// and does not affect the <see cref="Context"/> assignment.
    /// </para>
    /// <para>
    /// This is used to "unbind" the target of a structural node like <see cref="ConditionalNode"/> and in some cases
    /// prepare it for subsequent reuse.
    /// </para>
    /// </remarks>
    void Reset();

    /// <summary>
    /// Performs the regular per-frame update for this node.
    /// </summary>
    /// <param name="elapsed">Time elapsed since last update.</param>
    /// <returns><c>true</c> if any aspect of the view tree from this level downward was changed, i.e. as a result of
    /// a new <see cref="Context"/>, changed context properties, invalidated assets, or the <see cref="View"/> being
    /// created for the first time; <c>false</c> if no changes were made.</returns>
    bool Update(TimeSpan elapsed);
}
