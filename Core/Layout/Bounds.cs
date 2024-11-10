using Microsoft.Xna.Framework;

namespace StardewUI.Layout;

/// <summary>
/// A bounding rectangle using floating-point dimensions.
/// </summary>
/// <param name="Position">The top-left position.</param>
/// <param name="Size">The width and height.</param>
[DuckType]
public record Bounds(Vector2 Position, Vector2 Size) : IOffsettable<Bounds>
{
    /// <summary>
    /// Empty bounds, used for invalid results.
    /// </summary>
    public static readonly Bounds Empty = new(Vector2.Zero, Vector2.Zero);

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
    /// Checks if an entire bounding rectangle is fully within these bounds.
    /// </summary>
    /// <param name="bounds">The other bounds.</param>
    /// <returns><c>true</c> if <paramref name="bounds"/> are a subset of the current instance; <c>false</c> if the two
    /// bounds do not overlap or only overlap partially.</returns>
    public bool ContainsBounds(Bounds bounds)
    {
        return Intersection(bounds) == bounds;
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

    /// <summary>
    /// Computes the intersection of this <see cref="Bounds"/> with another instance.
    /// </summary>
    /// <param name="other">The other bounds to intersect with.</param>
    /// <returns>A new <see cref="Bounds"/> whose area is the intersection of this instance and
    /// <paramref name="other"/>, or <see cref="Empty"/> if they do not overlap.</returns>
    public Bounds Intersection(Bounds other)
    {
        var left = MathF.Max(Left, other.Left);
        var right = MathF.Min(Right, other.Right);
        if (right <= left)
        {
            return Empty;
        }
        var top = MathF.Max(Top, other.Top);
        var bottom = MathF.Min(Bottom, other.Bottom);
        if (bottom <= top)
        {
            return Empty;
        }
        var position = new Vector2(left, top);
        var size = new Vector2(right - left, bottom - top);
        return new(position, size);
    }

    /// <summary>
    /// Checks if this <see cref="Bounds"/> intersects with another instance, without computing the intersection.
    /// </summary>
    /// <param name="other">The other bounds to check for intersection.</param>
    /// <returns>True if this <see cref="Bounds"/> and the <paramref name="other"/> bounds have any intersecting area,
    /// otherwise <c>false</c>.</returns>
    public bool IntersectsWith(Bounds other)
    {
        return other.Right > Left && other.Left < Right && other.Bottom > Top && other.Top < Bottom;
    }

    /// <summary>
    /// Offsets a <see cref="Bounds"/> by a given distance.
    /// </summary>
    /// <param name="distance">The offset distance.</param>
    /// <returns>A new <see cref="Bounds"/> with the same size as this instance and a <see cref="Position"/> offset by
    /// the specified <paramref name="distance"/>.</returns>
    public Bounds Offset(Vector2 distance)
    {
        return new(Position + distance, Size);
    }

    /// <summary>
    /// Computes the union of this <see cref="Bounds"/> with another instance.
    /// </summary>
    /// <param name="other">The other bounds to add to the union.</param>
    /// <returns>A new <see cref="Bounds"/> whose set is the union of this instance and <paramref name="other"/>; i.e.
    /// is exactly large enough to contain both bounds.</returns>
    public Bounds Union(Bounds other)
    {
        var left = MathF.Min(Left, other.Left);
        var top = MathF.Min(Top, other.Top);
        var right = MathF.Max(Right, other.Right);
        var bottom = MathF.Max(Bottom, other.Bottom);
        var position = new Vector2(left, top);
        var size = new Vector2(right - left, bottom - top);
        return new(position, size);
    }
}
