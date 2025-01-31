using Microsoft.Xna.Framework;

namespace StardewUI;

/// <summary>
/// Commonly-used extensions for the <see cref="IView"/> interface and related types.
/// </summary>
public static class ViewExtensions
{
    /// <summary>
    /// Returns the focusable component of the path to a view, typically a cursor target.
    /// </summary>
    /// <param name="path">The view path.</param>
    /// <returns>The sequence of <paramref name="path"/> elements ending with the last view for which
    /// <see cref="IView.IsFocusable"/> is <c>true</c>. If there are no focusable views in the path, returns an empty
    /// sequence.</returns>
    public static IEnumerable<ViewChild> FocusablePath(this IEnumerable<ViewChild> path)
    {
        var list = path as IReadOnlyList<ViewChild> ?? path.ToArray();
        for (int i = list.Count - 1; i >= 0; i--)
        {
            if (list[i].View.IsFocusable)
            {
                return list.Take(i + 1);
            }
        }
        return [];
    }

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
    /// <param name="preferFocusable"><c>true</c> to prioritize a focusable child over a non-focusable child with a higher
    /// z-index in case of overlap; <c>false</c> to always use the topmost child.</param>
    /// <param name="requirePointerEvents">Whether to exclude views whose <see cref="IView.PointerEventsEnabled"/> is
    /// currently <c>false</c>. This short-circuits the pathing; if any ancestor of a view has pointer events disabled
    /// then it cannot be part of the path.</param>
    /// <returns>A sequence of <see cref="ViewChild"/> elements with the <see cref="IView"/> and position (relative to
    /// parent) at each level, starting with the specified <paramref name="view"/> and ending with the lowest-level
    /// <see cref="IView"/> that still overlaps with the specified <paramref name="position"/>.
    /// If no match is found, returns an empty sequence.</returns>
    public static IEnumerable<ViewChild> GetPathToPosition(
        this IView view,
        Vector2 position,
        bool preferFocusable = false,
        bool requirePointerEvents = true
    )
    {
        if ((requirePointerEvents && !view.PointerEventsEnabled) || !view.ContainsPoint(position))
        {
            yield break;
        }
        var child = new ViewChild(view, Vector2.Zero);
        do
        {
            position -= child.Position;
            yield return child;
            child = child.View.GetChildAt(position, preferFocusable, requirePointerEvents);
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
    /// <see cref="GetPathToPosition(IView, Vector2, bool, bool)"/>.</param>
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

    /// <summary>
    /// Sorts a sequence of children in ascending z-order.
    /// </summary>
    /// <remarks>
    /// Order is preserved between views with the same <see cref="IView.ZIndex"/>, so the resulting sequence will have
    /// a primary order of z-index (lower indices first) and a secondary order of original sequence position. This is
    /// the correct order for drawing views.
    /// </remarks>
    /// <param name="children">The view children.</param>
    /// <param name="focusPriority"><c>true</c> to sort focusable children first regardless of z-index; <c>false</c> to
    /// ignore <see cref="IView.IsFocusable"/>.</param>
    /// <returns>The <paramref name="children"/> ordered by the view's <see cref="IView.ZIndex"/> and original sequence
    /// order.</returns>
    public static IEnumerable<ViewChild> ZOrder(this IEnumerable<ViewChild> children, bool focusPriority = false)
    {
        // OrderBy is a stable sort so we don't need to do anything extra to preserve original sequence order.
        return focusPriority
            ? children.OrderByDescending(child => child.View.IsFocusable).ThenBy(child => child.View.ZIndex)
            : children.OrderBy(child => child.View.ZIndex);
    }

    /// <summary>
    /// Sorts a sequence of children in descending z-order.
    /// </summary>
    /// <remarks>
    /// The resulting sequence will have an order such that views with higher <see cref="IView.ZIndex"/> appear first,
    /// and views with the same z-index will appear in the <em>reverse</em> order of the original sequence. This is the
    /// correct order for handling cursor events and any other actions that need to operate on the "topmost" view first.
    /// </remarks>
    /// <param name="children">The view children.</param>
    /// <param name="focusPriority"><c>true</c> to sort focusable children first regardless of z-index; <c>false</c> to
    /// ignore <see cref="IView.IsFocusable"/>.</param>
    /// <returns></returns>
    public static IEnumerable<ViewChild> ZOrderDescending(
        this IEnumerable<ViewChild> children,
        bool focusPriority = false
    )
    {
        // With OrderByDescending, the stable sort works against us here, because it reverses the order of the key but
        // does NOT reverse the original sequence order. To get the correct result, we have to be explicit about the
        // sequence order.
        var indexedChildren = children.Select((child, index) => (child, index));
        var priorityChildren = focusPriority
            ? indexedChildren
                .OrderByDescending(x => x.child.View.IsFocusable)
                .ThenByDescending(x => x.child.View.ZIndex)
            : indexedChildren.OrderByDescending(x => x.child.View.ZIndex);
        return priorityChildren.ThenByDescending(x => x.index).Select(x => x.child);
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
