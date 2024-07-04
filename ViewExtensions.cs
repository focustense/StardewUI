using Microsoft.Xna.Framework;

namespace SupplyChain.UI;

/// <summary>
/// Commonly-used extensions for the <see cref="IView"/> interface.
/// </summary>
public static class ViewExtensions
{
    /// <summary>
    /// Retrieves a path to the view at a given position.
    /// </summary>
    /// <param name="view">The view at which to start the search.</param>
    /// <param name="position">The position to search for, in coordinates relative to the
    /// <paramref name="view"/>.</param>
    /// <returns>A sequence ending with the lowest-level <see cref="IView"/> overlapping the specified
    /// <paramref name="position"/> and preceded by all parent views, starting with the root <paramref name="view"/>.
    /// If no match is found, returns an empty sequence.</returns>
    public static IEnumerable<IView> GetPathToPosition(this IView view, Vector2 position)
    {
        if (position.X < 0 || position.Y < 0 || position.X > view.ActualSize.X || position.Y > view.ActualSize.Y)
        {
            yield break;
        }
        yield return view;
        foreach (var child in view.GetChildren())
        {
            var childPosition = position - child.Position;
            var childPath = child.View.GetPathToPosition(childPosition) ?? [];
            bool hasChildPath = false;
            foreach (var childResult in childPath)
            {
                hasChildPath = true;
                yield return childResult;
            }
            // With a non-overlapping layout, there should only be one valid path. In the event that's not the case, for
            // some unforeseen reason, make sure that we only return a single traversal, since a single sequence
            // containing multiple "paths" would be very difficult to unwind and parse.
            if (hasChildPath)
            {
                break;
            }
        }
    }

    /// <summary>
    /// Retrieves the view at a given position.
    /// </summary>
    /// <param name="view">The view at which to start the search.</param>
    /// <param name="position">The position to search for, in coordinates relative to the
    /// <paramref name="view"/>.</param>
    /// <returns>The lowest-level view overlapping the specified <paramref name="position"/>, or <c>null</c> if there is
    /// no view at that position. May return the <paramref name="view"/> itself if the coordinates are somewhere within
    /// that view but not within any child view.</returns>
    public static IView? GetViewAtPosition(this IView view, Vector2 position)
    {
        return view.GetPathToPosition(position)?.LastOrDefault();
    }
}
