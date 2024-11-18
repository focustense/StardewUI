namespace StardewUI.Framework.Dom;

/// <summary>
/// Provides a method to transform nodes into other nodes.
/// </summary>
/// <remarks>
/// Transformers are a form of preprocessing that apply before a view is bound; they operate on the parsed DOM content
/// but not the runtime/bound nodes.
/// </remarks>
public interface INodeTransformer
{
    /// <summary>
    /// Transforms a node.
    /// </summary>
    /// <param name="source">The node to transform.</param>
    /// <returns>The transformed nodes, if any transform was applied, or a single-element list with the original
    /// <paramref name="source"/> if the transformation is not applicable to this node.</returns>
    IReadOnlyList<SNode> Transform(SNode source);
}
