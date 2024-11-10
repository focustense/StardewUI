using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework;
using StardewUI.Input;
using StardewUI.Layout;

namespace StardewUI;

/// <summary>
/// Provides information about a view that is the child of another view. Used for interactions.
/// </summary>
/// <param name="View">The child view.</param>
/// <param name="Position">The position of the <paramref name="View"/>, relative to the parent.</param>
public record ViewChild(IView View, Vector2 Position) : IOffsettable<ViewChild>
{
    /// <summary>
    /// Returns a copy of this instance as a weak version that does not keep the <see cref="IView"/> alive.
    /// </summary>
    /// <remarks>
    /// Use whenever it is necessary to store a reference to both the view and its position in places where the view may
    /// disappear from scope, e.g. a deep descendant of a menu's root view.
    /// </remarks>
    internal WeakViewChild AsWeak()
    {
        return new(View, Position);
    }

    /// <summary>
    /// Gets the point at the exact center of the view.
    /// </summary>
    public Vector2 Center()
    {
        return (Position + View.ContentBounds.Center());
    }

    /// <summary>
    /// Gets the nearest whole pixel point at the exact center of the view.
    /// </summary>
    public Point CenterPoint()
    {
        return Center().ToPoint();
    }

    /// <summary>
    /// Checks if a given point, relative to the view's parent, is within the bounds of this child.
    /// </summary>
    /// <param name="point">The point to test.</param>
    /// <returns><c>true</c> if <paramref name="point"/> is within the parent-relative bounds of this child; otherwise
    /// <c>false</c>.</returns>
    public bool ContainsPoint(Vector2 point)
    {
        return View.ContainsPoint(point - Position);
    }

    /// <summary>
    /// Performs a focus search on the referenced view.
    /// </summary>
    /// <remarks>
    /// This is equivalent to <see cref="IView.FocusSearch"/> but implicitly handles its own <see cref="Position"/>, so
    /// it can be used recursively without directly adjusting any coordinates.
    /// </remarks>
    /// <param name="contentPosition">The current position, relative to the parent that owns this child.</param>
    /// <param name="direction">The direction of cursor movement.</param>
    /// <returns>The next focusable view reached by moving in the specified <paramref name="direction"/>, or <c>null</c>
    /// if there are no focusable descendants that are possible to reach in that direction.</returns>
    public FocusSearchResult? FocusSearch(Vector2 contentPosition, Direction direction)
    {
        return View.FocusSearch(contentPosition - Position, direction)?.Offset(Position);
    }

    /// <summary>
    /// Returns a <see cref="Bounds"/> representing the parent-relative layout bounds of this child.
    /// </summary>
    /// <remarks>
    /// Equivalent to the <see cref="IView.ActualBounds"/> offset by this child's <see cref="Position"/>.
    /// </remarks>
    public Bounds GetActualBounds()
    {
        return View.ActualBounds.Offset(Position);
    }

    /// <summary>
    /// Returns a <see cref="Bounds"/> representing the parent-relative content bounds of this child.
    /// </summary>
    /// <remarks>
    /// Equivalent to the <see cref="IView.ContentBounds"/> offset by this child's <see cref="Position"/>.
    /// </remarks>
    public Bounds GetContentBounds()
    {
        return View.ContentBounds.Offset(Position);
    }

    /// <summary>
    /// Returns a sequence of <see cref="Bounds"/> representing the parent-relative bounds of this child's own floating
    /// elements and those of all its descendants.
    /// </summary>
    public IEnumerable<Bounds> GetFloatingBounds()
    {
        return View.FloatingBounds.Select(bounds => bounds.Offset(Position));
    }

    /// <summary>
    /// Offsets the position by a given distance.
    /// </summary>
    /// <param name="distance">The offset distance.</param>
    /// <returns>A copy of the current <see cref="ViewChild"/> having the same <see cref="View"/> and a
    /// <see cref="Position"/> offset by <paramref name="distance"/>.</returns>
    public ViewChild Offset(Vector2 distance)
    {
        return new(View, Position + distance);
    }

    /// <summary>
    /// Checks if a view can be reached by travelling from a given point in a given direction.
    /// </summary>
    /// <param name="origin">The origin point.</param>
    /// <param name="direction">The direction from <paramref name="origin"/>.</param>
    /// <returns><c>true</c> if the view's boundaries either already contain the <paramref name="origin"/> or are in the
    /// specified <paramref name="direction"/> from the <paramref name="origin"/>; otherwise <c>false</c>.</returns>
    public bool IsInDirection(Vector2 origin, Direction direction)
    {
        var relativePosition = origin - Position;
        var bounds = View.ActualBounds;
        return direction switch
        {
            Direction.North => relativePosition.Y >= bounds.Top,
            Direction.South => relativePosition.Y < bounds.Bottom,
            Direction.West => relativePosition.X >= bounds.Left,
            Direction.East => relativePosition.X < bounds.Right,
            _ => false,
        };
    }
}

/// <summary>
/// A variant of <see cref="ViewChild"/> that uses a weak <see cref="IView"/> reference, safe to store without keeping
/// the underlying view alive.
/// </summary>
internal class WeakViewChild
{
    private readonly Vector2 position;
    private readonly WeakReference<IView> viewRef;

    [SuppressMessage("Style", "IDE0290:Use primary constructor", Justification = "Would probably cause a memory leak")]
    public WeakViewChild(IView view, Vector2 position)
    {
        viewRef = new(view);
        this.position = position;
    }

    /// <summary>
    /// Tries to resolve this instance into a normal <see cref="ViewChild"/> with strong view reference.
    /// </summary>
    /// <param name="viewChild">Set to a <see cref="ViewChild"/> with live <see cref="IView"/>, if the underlying view
    /// is still alive; otherwise <c>null</c>.</param>
    /// <returns><c>true</c> if the underlying <see cref="IView"/> was still alive; otherwise <c>false</c>.</returns>
    public bool TryResolve([MaybeNullWhen(false)] out ViewChild viewChild)
    {
        if (viewRef.TryGetTarget(out var view))
        {
            viewChild = new(view, position);
            return true;
        }
        viewChild = null;
        return false;
    }
}
