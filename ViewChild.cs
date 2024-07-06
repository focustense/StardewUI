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
}
