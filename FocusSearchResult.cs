using Microsoft.Xna.Framework;

namespace SupplyChain.UI;

/// <summary>
/// The result of a <see cref="IView.FocusSearch"/>. Identifies the specific view/position found, as well as the path
/// to that view from the search root.
/// </summary>
/// <param name="Target">The specific view that can/will be focused.</param>
/// <param name="Path">The path from root to <see cref="Target"/>, in top-down order.</param>
public record FocusSearchResult(ViewChild Target, IEnumerable<IView> Path)
{
    /// <summary>
    /// Returns a transformed <see cref="FocusSearchResult"/> that adds a view (generally the caller) to the beginning
    /// of the <see cref="Path"/>, and applies its content offset to the <see cref="Target"/>.
    /// </summary>
    /// <remarks>
    /// Used to propagate results correctly up the view hierarchy in a focus search.
    /// </remarks>
    /// <param name="parent">The parent that contains the current result.</param>
    /// <param name="offset">The content offset of the <paramref name="parent"/>.</param>
    public FocusSearchResult AsChild(IView parent, Vector2? offset = null)
    {
        return new(Target.Offset(offset ?? Vector2.Zero), Path.Prepend(parent));
    }
}
