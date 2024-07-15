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
        if (position.X < 0 || position.Y < 0 || position.X > view.OuterSize.X || position.Y > view.OuterSize.Y)
        {
            yield break;
        }
        var child = new ViewChild(view, Vector2.Zero);
        do
        {
            position -= child.Position;
            yield return child.View;
            child = child.View.GetChildAt(position);
        } while (child is not null);
    }

    /// <summary>
    /// Takes an existing view path and resolves it with child coordinates for the view at each level.
    /// </summary>
    /// <param name="view">The root view.</param>
    /// <param name="path">The path from root down to some descendant, such as the path returned by
    /// <see cref="GetPathToPosition(IView, Vector2)"/>.</param>
    /// <returns>A sequence of <see cref="ViewChild"/> elements, starting at the <paramref name="view"/>, where each
    /// child's <see cref="ViewChild.Position"/> is the child's most current location within its parent.</returns>
    public static IEnumerable<ViewChild> ResolveChildPath(this IView view, IEnumerable<IView> path)
    {
        yield return new(view, Vector2.Zero);
        path = path.SkipWhile(v => v == view);
        var parent = view;
        foreach (var descendant in path)
        {
            var childPosition = parent.GetChildPosition(descendant);
            if (childPosition is null)
            {
                yield break;
            }
            yield return new(descendant, childPosition.Value);
            parent = descendant;
        }
    }
}
