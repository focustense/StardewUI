using Microsoft.Xna.Framework;

namespace StardewUI.Input;

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
    /// of the <see cref="Path"/>, and applies its content offset to either the first element of the current
    /// <see cref="Path"/> (if non-empty) or the <see cref="Target"/> (if the path is empty).
    /// </summary>
    /// <remarks>
    /// Used to propagate results correctly up the view hierarchy in a focus search. This is called by
    /// <see cref="View.FocusSearch"/> and should not be called in overrides of
    /// <see cref="View.FindFocusableDescendant"/>.
    /// </remarks>
    /// <param name="parent">The parent that contains the current result.</param>
    /// <param name="position">The content offset relative to the <paramref name="parent"/>.</param>
    public FocusSearchResult AsChild(IView parent, Vector2 position)
    {
        var root = new ViewChild(parent, Vector2.Zero);
        var (target, path) = OffsetTargetOrPath(position);
        return new(target, path.Prepend(root));
    }

    /// <summary>
    /// Applies a local offset to a search result.
    /// </summary>
    /// <remarks>
    /// Used to propagate the child position into a search result produced by that child. For example, view A is a
    /// layout with positioned child view C, which yields a search result targeting view Z in terms of its (C's) local
    /// coordinates. Applying the offset will adjust either the first element of the <see cref="Path"/>, if non-empty,
    /// or the <see cref="Target"/> itself if <see cref="Path"/> is empty. No other elements of the <see cref="Path"/>
    /// will be modified, as each element is already positioned relative to its parent preceding it in the list.
    /// </remarks>
    /// <param name="distance">The distance to offset the <see cref="Target"/> and first element of
    /// <see cref="Path"/>.</param>
    /// <returns>A new <see cref="FocusSearchResult"/> with the <paramref name="distance"/> offset applied.</returns>
    public FocusSearchResult Offset(Vector2 distance)
    {
        var (target, path) = OffsetTargetOrPath(distance);
        return new(target, path);
    }

    private (ViewChild target, IEnumerable<ViewChild> path) OffsetTargetOrPath(Vector2 distance)
    {
        var pathEnumerator = Path.GetEnumerator();
        if (pathEnumerator.MoveNext())
        {
            var first = pathEnumerator.Current;
            return (Target, pathEnumerator.ToEnumerable().Prepend(first.Offset(distance)));
        }
        return (Target.Offset(distance), []);
    }
}
