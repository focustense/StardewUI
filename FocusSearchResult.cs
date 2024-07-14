using Microsoft.Xna.Framework;

namespace SupplyChain.UI;

/// <summary>
/// The result of a <see cref="IView.FocusSearch"/>. Identifies the specific view/position found, as well as the path
/// to that view from the search root.
/// </summary>
/// <param name="Target">The specific view that can/will be focused, with a <see cref="ViewChild.Position"/> relative to
/// the search root.</param>
/// <param name="Path">The path from root to <see cref="Target"/>, in top-down order; each element's
/// <see cref="ViewChild.Position"/> is relative to the parent, <b>not</b> the search root as <paramref name="Target"/>
/// is.</param>
public record FocusSearchResult(ViewChild Target, IEnumerable<ViewChild> Path)
{
    /// <summary>
    /// Returns a transformed <see cref="FocusSearchResult"/> that adds a view (generally the caller) to the beginning
    /// of the <see cref="Path"/>, and applies its content offset to the <see cref="Target"/>.
    /// </summary>
    /// <remarks>
    /// Used to propagate results correctly up the view hierarchy in a focus search. This is called by
    /// <see cref="View.FocusSearch"/> and should not be called in overrides of
    /// <see cref="View.FindFocusableDescendant"/>.
    /// </remarks>
    /// <param name="parent">The parent that contains the current result.</param>
    /// <param name="position">The content offset of the <paramref name="parent"/>.</param>
    public FocusSearchResult AsChild(IView parent, Vector2 position)
    {
        return new(Target.Offset(position), OffsetFirst(Path, position).Prepend(new(parent, Vector2.Zero)));
    }

    /// <summary>
    /// Applies a local offset to a search result.
    /// </summary>
    /// <remarks>
    /// Used to propagate the child position into a search result produced by that child. For example, view X is a
    /// layout with positioned child view Y, which yields a search result targeting view Z in terms of its (Y's) local
    /// coordinates. Applying the offset will adjust the <see cref="Target"/> position to be relative to the position of
    /// Y within X, and also adjust the <see cref="ViewChild.Position"/> of the first element of <see cref="Path"/> to
    /// be the child position instead of <see cref="Vector2.Zero"/>, but will not modify any other <c>Path</c> elements
    /// as each element is positioned relative to its parent preceding it in the list.
    /// </remarks>
    /// <param name="distance">The distance to offset the <see cref="Target"/> and first element of
    /// <see cref="Path"/>.</param>
    /// <returns>A new <see cref="FocusSearchResult"/> with the <paramref name="distance"/> offset applied.</returns>
    public FocusSearchResult Offset(Vector2 distance)
    {
        return new(Target.Offset(distance), OffsetFirst(Path, distance));
    }

    private static IEnumerable<ViewChild> OffsetFirst(IEnumerable<ViewChild> path, Vector2 distance)
    {
        bool isFirst = true;
        foreach (var child in path)
        {
            yield return isFirst ? child.Offset(distance) : child;
            isFirst = false;
        }
    }
}
