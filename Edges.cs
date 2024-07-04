using Microsoft.Xna.Framework;

namespace SupplyChain.UI;

/// <summary>
/// Describes a set of edge dimensions, e.g. for margin or padding.
/// </summary>
/// <param name="Left">The left edge.</param>
/// <param name="Top">The top edge.</param>
/// <param name="Right">The right edge.</param>
/// <param name="Bottom">The bottom edge.</param>
public record Edges(int Left, int Top, int Right, int Bottom)
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
    /// Initializes a new <see cref="Edges"/> with all edges set to the same value.
    /// </summary>
    /// <param name="all">Common value for all edges.</param>
    public Edges(int all) : this(all, all, all, all) { }

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

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(Left);
        hashCode.Add(Top);
        hashCode.Add(Right);
        hashCode.Add(Bottom);
        return hashCode.ToHashCode();
    }
}
