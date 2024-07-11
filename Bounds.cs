using Microsoft.Xna.Framework;

namespace SupplyChain.UI;

/// <summary>
/// A bounding rectangle using floating-point dimensions.
/// </summary>
/// <param name="Position">The top-left position.</param>
/// <param name="Size">The width and height.</param>
public record Bounds(Vector2 Position, Vector2 Size)
{
    /// <summary>
    /// The Y value at the bottom edge of the bounding rectangle.
    /// </summary>
    public float Bottom => Position.Y + Size.Y;

    /// <summary>
    /// The X value at the left edge of the bounding rectangle.
    /// </summary>
    public float Left => Position.X;

    /// <summary>
    /// The X value at the right edge of the bounding rectangle.
    /// </summary>
    public float Right => Position.X + Size.X;

    /// <summary>
    /// The Y value at the top edge of the bounding rectangle.
    /// </summary>
    public float Top => Position.Y;

    /// <summary>
    /// Gets the point at the center of the bounding rectangle.
    /// </summary>
    public Vector2 Center()
    {
        return new(Position.X + Size.X / 2, Position.Y + Size.Y / 2);
    }

    /// <summary>
    /// Checks if a given point is within the bounds.
    /// </summary>
    /// <param name="point">The point to check.</param>
    /// <returns><c>true</c> if <paramref name="point"/> is inside these bounds; otherwise <c>false</c>.</returns>
    public bool ContainsPoint(Vector2 point)
    {
        return point.X >= Left && point.X < Right && point.Y >= Top && point.Y < Bottom;
    }
}
