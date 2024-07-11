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
}
