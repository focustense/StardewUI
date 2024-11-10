using Microsoft.Xna.Framework;

namespace StardewUI;

/// <summary>
/// Commonly-used extensions for the <see cref="IView"/> interface and related types.
/// </summary>
public static class ViewExtensions
{
    /// <summary>
    /// Retrieves a path to the default focus child/descendant of a view.
    /// </summary>
    /// <param name="view">The view at which to start the search.</param>
    /// <returns>A sequence of <see cref="ViewChild"/> elements with the <see cref="IView"/> and position (relative to
    /// parent) at each level, starting with the specified <paramref name="view"/> and ending with the lowest-level
    /// <see cref="IView"/> in the default focus path. If no focusable descendant is found, returns an empty
    /// sequence.</returns>
    public static IEnumerable<ViewChild> GetDefaultFocusPath(this IView view)
    {
        // It's possible to implement this recursively, but the default implementation of GetDefaultFocusChild is
        // already recursive, so adding a recursive enumerator on top of that makes debugging confusing and can have
        // unpredictable performance.
        // Technically, we could improve this even more by making IView implement the path method, so we aren't doing
        // double-lookups each time, but it would be at the cost of a much more difficult API to implement correctly.
        var currentChild = view.GetDefaultFocusChild();
        if (currentChild is null)
        {
            yield break;
        }
        while (true)
        {
            yield return currentChild;
            var nextChild = currentChild.View.GetDefaultFocusChild();
            if (nextChild is null || nextChild.View == currentChild.View)
            {
                break;
            }
            currentChild = nextChild;
        }
    }

    /// <summary>
    /// Retrieves a path to the view at a given position.
    /// </summary>
    /// <param name="view">The view at which to start the search.</param>
    /// <param name="position">The position to search for, in coordinates relative to the
    /// <paramref name="view"/>.</param>
    /// <returns>A sequence of <see cref="ViewChild"/> elements with the <see cref="IView"/> and position (relative to
    /// parent) at each level, starting with the specified <paramref name="view"/> and ending with the lowest-level
    /// <see cref="IView"/> that still overlaps with the specified <paramref name="position"/>.
    /// If no match is found, returns an empty sequence.</returns>
    public static IEnumerable<ViewChild> GetPathToPosition(this IView view, Vector2 position)
    {
        if (!view.ContainsPoint(position))
        {
            yield break;
        }
        var child = new ViewChild(view, Vector2.Zero);
        do
        {
            position -= child.Position;
            yield return child;
            child = child.View.GetChildAt(position);
        } while (child is not null);
    }

    /// <summary>
    /// Retrieves the path to a descendant view.
    /// </summary>
    /// <remarks>
    /// This method has worst-case O(N) performance, so avoid calling it in tight loops such as draw methods, and cache
    /// the result whenever possible.
    /// </remarks>
    /// <param name="view">The view at which to start the search.</param>
    /// <param name="descendant">The descendant view to search for.</param>
    /// <returns>A sequence of <see cref="ViewChild"/> elements with the <see cref="IView"/> and position (relative to
    /// parent) at each level, starting with the specified <paramref name="view"/> and ending with the specified
    /// <paramref name="descendant"/>. If no match is found, returns <c>null</c>.</returns>
    public static IEnumerable<ViewChild>? GetPathToView(this IView view, IView descendant)
    {
        var self = new ViewChild(view, Vector2.Zero);
        return GetPathToView(self, descendant);
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

    /// <summary>
    /// Converts a view path in parent-relative coordinates (e.g. from <see cref="GetPathToPosition"/> and transforms
    /// each element to have an absolute <see cref="ViewChild.Position"/>.
    /// </summary>
    /// <remarks>
    /// Since <see cref="ViewChild"/> does not specify whether the position is local (parent) or global (absolute), it
    /// is not possible to validate the incoming sequence and prevent a "double transformation". Callers are responsible
    /// for knowing whether or not the input sequence is local or global.
    /// </remarks>
    /// <param name="path">The path from root down to leaf view.</param>
    /// <returns>The <paramref name="path"/> with positions in global coordinates.</returns>
    public static IEnumerable<ViewChild> ToGlobalPositions(this IEnumerable<ViewChild> path)
    {
        var position = Vector2.Zero;
        foreach (var descendant in path)
        {
            yield return descendant.Offset(position);
            position += descendant.Position;
        }
    }

    private static IEnumerable<ViewChild>? GetPathToView(ViewChild parent, IView descendant)
    {
        if (parent.View == descendant)
        {
            return [parent];
        }
        foreach (var child in parent.View.GetChildren())
        {
            var childPath = GetPathToView(child, descendant);
            if (childPath is not null)
            {
                return [parent, .. childPath];
            }
        }
        return null;
    }
}
