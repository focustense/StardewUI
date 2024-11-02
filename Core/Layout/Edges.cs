using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using StardewUI.Graphics;

namespace StardewUI.Layout;

/// <summary>
/// Describes a set of edge dimensions, e.g. for margin or padding.
/// </summary>
/// <param name="Left">The left edge.</param>
/// <param name="Top">The top edge.</param>
/// <param name="Right">The right edge.</param>
/// <param name="Bottom">The bottom edge.</param>
[DuckType]
public record Edges(int Left = 0, int Top = 0, int Right = 0, int Bottom = 0)
{
    /// <summary>
    /// An <see cref="Edges"/> instance with all edges set to zero.
    /// </summary>
    public static readonly Edges NONE = new(0, 0, 0, 0);

    /// <summary>
    /// Gets the total value for all horizontal edges (<see cref="Left"/> + <see cref="Right"/>).
    /// </summary>
    public int Horizontal => Left + Right;

    /// <summary>
    /// The total size occupied by all edges.
    /// </summary>
    public Vector2 Total => new(Left + Right, Top + Bottom);

    /// <summary>
    /// Gets the total value for all vertical edges (<see cref="Top"/> + <see cref="Bottom"/>).
    /// </summary>
    public int Vertical => Top + Bottom;

    /// <summary>
    /// Parses an <see cref="Edges"/> value from a comma-separated string representation.
    /// </summary>
    /// <remarks>
    /// The behavior depends on the number of comma-separated tokens in the string, equivalent to the constructor
    /// overload with that number of parameters:
    /// <list type="bullet">
    /// <item>A single value will give all edges the same length</item>
    /// <item>Two values will set the horizontal (left/right) and vertical (top/bottom) lengths</item>
    /// <item>Four values will set each length individually</item>
    /// <item>Any other format will throw <see cref="FormatException"/>.</item>
    /// </list>
    /// </remarks>
    /// <param name="value">The formatted edges to parse.</param>
    /// <returns>The parsed <see cref="Edges"/>.</returns>
    /// <exception cref="FormatException">Thrown when the <paramref name="value"/> is not a valid string representation
    /// for <see cref="Edges"/>.</exception>
    public static Edges Parse(string value)
    {
        var valueAsSpan = value.AsSpan();
        // We use generic uninformative names for these variables because they mean different things depending on how
        // many of them appear in the string.
        int edge1 = ReadNextEdge(ref valueAsSpan);
        if (valueAsSpan.IsEmpty)
        {
            return new(edge1); // Same length for all edges
        }
        int edge2 = ReadNextEdge(ref valueAsSpan);
        if (valueAsSpan.IsEmpty)
        {
            return new(edge1, edge2); // Horizontal and vertical lengths
        }
        int edge3 = ReadNextEdge(ref valueAsSpan);
        int edge4 = ReadNextEdge(ref valueAsSpan);
        if (!valueAsSpan.IsEmpty)
        {
            throw new FormatException($"Too many edges specified in string '{value}' (cannot have more than 4).");
        }
        return new(edge1, edge2, edge3, edge4);
    }

    /// <summary>
    /// Initializes a new <see cref="Edges"/> with all edges set to the same value.
    /// </summary>
    /// <param name="all">Common value for all edges.</param>
    public Edges(int all)
        : this(all, all, all, all) { }

    /// <summary>
    /// Initialies a new <see cref="Edges"/> with symmetrical horizontal and vertical values.
    /// </summary>
    /// <param name="horizontal">Common value for the <see cref="Left"/> and <see cref="Right"/> edges.</param>
    /// <param name="vertical">Common value for the <see cref="Top"/> and <see cref="Bottom"/> edges.</param>
    public Edges(int horizontal, int vertical)
        : this(horizontal, vertical, horizontal, vertical) { }

    /// <inheritdoc/>
    /// <remarks>
    /// Overrides the default implementation to avoid using reflection on every frame during dirty checks.
    /// </remarks>
    public virtual bool Equals(Edges? other)
    {
        if (other is null)
        {
            return false;
        }
        return other.Left == Left && other.Top == Top && other.Right == Right && other.Bottom == Bottom;
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(Left);
        hashCode.Add(Top);
        hashCode.Add(Right);
        hashCode.Add(Bottom);
        return hashCode.ToHashCode();
    }

    /// <summary>
    /// Gets a copy of this instance with only the horizontal edges set (vertical edges zeroed out).
    /// </summary>
    public Edges HorizontalOnly()
    {
        return new(Left, 0, Right, 0);
    }

    /// <summary>
    /// Rotates the edges, transposing the individual edge values.
    /// </summary>
    /// <param name="rotation">The rotation type (angle).</param>
    /// <returns>A rotated copy of this <see cref="Edges"/> instance.</returns>
    public Edges Rotate(SimpleRotation rotation)
    {
        return rotation switch
        {
            SimpleRotation.QuarterClockwise => new(Bottom, Left, Top, Right),
            SimpleRotation.QuarterCounterclockwise => new(Top, Right, Bottom, Left),
            SimpleRotation.Half => new(Right, Bottom, Left, Top),
            _ => this,
        };
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{Left}, {Top}, {Right}, {Bottom}";
    }

    /// <summary>
    /// Gets a copy of this instance with only the vertical edges set (horizontal edges zeroed out).
    /// </summary>
    public Edges VerticalOnly()
    {
        return new(0, Top, 0, Bottom);
    }

    /// <summary>
    /// Negates an <see cref="Edges"/> value.
    /// </summary>
    /// <param name="value">The edges.</param>
    /// <returns>An <see cref="Edges"/> whose individual edge values are each the negation of the corresponding edge in
    /// <paramref name="value"/>.</returns>
    public static Edges operator -(Edges value)
    {
        return new(-value.Left, -value.Top, -value.Right, -value.Bottom);
    }

    /// <summary>
    /// Computes the sum of two <see cref="Edges"/> values.
    /// </summary>
    /// <param name="value1">The first value.</param>
    /// <param name="value2">The second value.</param>
    /// <returns>An <see cref="Edges"/> whose individual edge values are the sum of the corresponding edges from
    /// <paramref name="value1"/> and <paramref name="value2"/>.</returns>
    public static Edges operator +(Edges value1, Edges value2)
    {
        return new(
            value1.Left + value2.Left,
            value1.Top + value2.Top,
            value1.Right + value2.Right,
            value1.Bottom + value2.Bottom
        );
    }

    /// <summary>
    /// Computes the difference of two <see cref="Edges"/> values.
    /// </summary>
    /// <param name="value1">The first value, to subtract from.</param>
    /// <param name="value2">The second value, to be subtracted.</param>
    /// <returns>An <see cref="Edges"/> whose individual edge values are the difference of the corresponding edges
    /// between <paramref name="value1"/> and <paramref name="value2"/>.</returns>
    public static Edges operator -(Edges value1, Edges value2)
    {
        return new(
            value1.Left - value2.Left,
            value1.Top - value2.Top,
            value1.Right - value2.Right,
            value1.Bottom - value2.Bottom
        );
    }

    /// <summary>
    /// Scales an <see cref="Edges"/> value uniformly.
    /// </summary>
    /// <param name="value">The value to scale.</param>
    /// <param name="scale">The scale amount (multiplier).</param>
    /// <returns>An <see cref="Edges"/> whose individual edge values are the corresponding edges from
    /// <paramref name="value"/>, each multiplied by the <paramref name="scale"/>.</returns>
    public static Edges operator *(Edges value, int scale)
    {
        return new(value.Left * scale, value.Top * scale, value.Right * scale, value.Bottom * scale);
    }

    /// <summary>
    /// Scales an <see cref="Edges"/> value uniformly.
    /// </summary>
    /// <param name="value">The value to scale.</param>
    /// <param name="scale">The scale amount (multiplier).</param>
    /// <returns>An <see cref="Edges"/> whose individual edge values are the corresponding edges from
    /// <paramref name="value"/>, each multiplied by the <paramref name="scale"/>.</returns>
    public static Edges operator *(Edges value, float scale)
    {
        return new(
            (int)MathF.Round(value.Left * scale),
            (int)MathF.Round(value.Top * scale),
            (int)MathF.Round(value.Right * scale),
            (int)MathF.Round(value.Bottom * scale)
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int ReadNextEdge(ref ReadOnlySpan<char> remaining)
    {
        int nextSeparatorIndex = remaining.IndexOf(',');
        var value = nextSeparatorIndex >= 0 ? int.Parse(remaining[0..nextSeparatorIndex]) : int.Parse(remaining);
        remaining = nextSeparatorIndex >= 0 ? remaining[(nextSeparatorIndex + 1)..] : [];
        return value;
    }
}
