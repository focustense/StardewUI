using StardewUI.Framework.Content;
using StardewUI.Framework.Dom;

namespace StardewUI.Framework.Binding;

/// <summary>
/// High-level abstraction for translating node trees into bound view trees.
/// </summary>
public interface IViewNodeFactory
{
    /// <summary>
    /// Creates a bound view node, and all descendants, from the root of a parsed <see cref="Document"/>.
    /// </summary>
    /// <remarks>
    /// This method automatically infers the correct <see cref="IResolutionScope"/>, so it does not require an explicit
    /// scope to be given.
    /// </remarks>
    /// <param name="document">The markup document.</param>
    /// <returns>An <see cref="IViewNode"/> providing the <see cref="IView"/> bound with the node's attributes and
    /// children, which automatically applies changes on each <see cref="IViewNode.Update"/>.</returns>
    IViewNode CreateNode(Document document);

    /// <summary>
    /// Creates a bound view node, and all descendants, from parsed node data.
    /// </summary>
    /// <param name="node">The node data.</param>
    /// <param name="nodeTransformers">Transformers to run on each document node before using it to create a runtime
    /// (bound) view node.</param>
    /// <param name="resolutionScope">Scope for resolving externalized attributes, such as translation keys.</param>
    /// <returns>An <see cref="IViewNode"/> providing the <see cref="IView"/> bound with the node's attributes and
    /// children, which automatically applies changes on each <see cref="IViewNode.Update"/>.</returns>
    IViewNode CreateNode(
        SNode node,
        IReadOnlyList<INodeTransformer> nodeTransformers,
        IResolutionScope resolutionScope
    );
}
