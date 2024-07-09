using Microsoft.Xna.Framework;

namespace SupplyChain.UI;

/// <summary>
/// Provides information about a view that is the child of another view. Used for interactions.
/// </summary>
/// <param name="View">The child view.</param>
/// <param name="Position">The position of the <paramref name="View"/>, relative to the parent.</param>
public record ViewChild(IView View, Vector2 Position)
{
    /// <summary>
    /// Gets the point at the exact center of the view.
    /// </summary>
    public Point Center()
    {
        return new Vector2(
            Position.X + View.ActualSize.X / 2,
            Position.Y + View.ActualSize.Y / 2)
            .ToPoint();
    }

    /// <summary>
    /// Checks if a given point, relative to the view's parent, is within the bounds of this child.
    /// </summary>
    /// <param name="point">The point to test.</param>
    /// <returns><c>true</c> if <paramref name="point"/> is within the parent-relative bounds of this child; otherwise
    /// <c>false</c>.</returns>
    public bool ContainsPoint(Vector2 point)
    {
        return point.X >= Position.X
            && point.X < Position.X + View.ActualSize.X
            && point.Y >= Position.Y
            && point.Y < Position.Y + View.ActualSize.Y;
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
    public ViewChild? FocusSearch(Vector2 contentPosition, Direction direction)
    {
        return View.FocusSearch(contentPosition - Position, direction)?.Offset(Position);
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
}
