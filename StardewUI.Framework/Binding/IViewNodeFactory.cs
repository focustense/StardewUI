using StardewUI.Framework.Dom;

namespace StardewUI.Framework.Binding;

/// <summary>
/// High-level abstraction for translating node trees into bound view trees.
/// </summary>
public interface IViewNodeFactory
{
    /// <summary>
    /// Creates a bound view node, and all descendants, from parsed node data.
    /// </summary>
    /// <param name="node">The node data.</param>
    /// <returns>An <see cref="IViewNode"/> providing the <see cref="IView"/> bound with the node's attributes and
    /// children, which automatically applies changes on each <see cref="IViewNode.Update"/>.</returns>
    IViewNode CreateNode(SNode node);
}
