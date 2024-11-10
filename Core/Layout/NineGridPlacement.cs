using System.Collections.Immutable;
using Microsoft.Xna.Framework;

namespace StardewUI.Layout;

/// <summary>
/// Model for content placement along a nine-segment grid, i.e. all possible combinations of horizontal and vertical
/// <see cref="Alignment"/>.
/// </summary>
/// <param name="HorizontalAlignment">Content alignment along the horizontal axis.</param>
/// <param name="VerticalAlignment">Content alignment along the vertical axis.</param>
/// <param name="Offset">Absolute axis-independent pixel offset.</param>
[DuckType]
public record NineGridPlacement(Alignment HorizontalAlignment, Alignment VerticalAlignment, Point Offset = new())
{
    /// <summary>
    /// Represents an adjacent placement; the result of <see cref="GetNeighbors(bool)"/>.
    /// </summary>
    /// <param name="Direction">The direction of traversal for this neighbor.</param>
    /// <param name="Placement">The neighboring placement.</param>
    public record Neighbor(Direction Direction, NineGridPlacement Placement);

    /// <summary>
    /// All the standard placements with no <see cref="Offset"/>, arranged from bottom-left to top-right.
    /// </summary>
    public static readonly IImmutableList<NineGridPlacement> StandardPlacements = ImmutableArray.Create(
        new NineGridPlacement[]
        {
            new(Alignment.Start, Alignment.End),
            new(Alignment.Middle, Alignment.End),
            new(Alignment.End, Alignment.End),
            new(Alignment.Start, Alignment.Middle),
            new(Alignment.Middle, Alignment.Middle),
            new(Alignment.End, Alignment.Middle),
            new(Alignment.Start, Alignment.Start),
            new(Alignment.Middle, Alignment.Start),
            new(Alignment.End, Alignment.Start),
        }
    );

    /// <summary>
    /// Gets the <see cref="NineGridPlacement"/> for an alignment pair that resolves to a specified exact position.
    /// </summary>
    /// <param name="position">The target position on screen or within the container.</param>
    /// <param name="size">The size of the viewport or container.</param>
    /// <param name="horizontalAlignment">The desired horizontal alignment.</param>
    /// <param name="verticalAlignment">The desired vertical alignment.</param>
    /// <returns>A <see cref="NineGridPlacement"/> whose <see cref="HorizontalAlignment"/> and
    /// <see cref="VerticalAlignment"/> match the <paramref name="horizontalAlignment"/> and
    /// <paramref name="verticalAlignment"/>, respectively, and whose <see cref="GetPosition"/> will resolve to exactly
    /// the specified <paramref name="position"/>.</returns>
    public static NineGridPlacement AtPosition(
        Vector2 position,
        Vector2 size,
        Alignment horizontalAlignment,
        Alignment verticalAlignment
    )
    {
        var basePosition = GetPosition(size, horizontalAlignment, verticalAlignment);
        var offset = position - basePosition;
        return new(horizontalAlignment, verticalAlignment, offset.ToPoint());
    }

    private static readonly Direction[] neighborDirections =
    [
        Direction.North,
        Direction.South,
        Direction.East,
        Direction.West,
    ];

    /// <summary>
    /// Checks if another <see cref="NineGridPlacement"/> has the same alignments as this one, regardless of offset.
    /// </summary>
    /// <param name="other">The instance to compare.</param>
    /// <returns><c>true</c> if the <paramref name="other"/> instance has the same alignments, otherwise
    /// <c>false</c>.</returns>
    public bool EqualsIgnoringOffset(NineGridPlacement other)
    {
        return other.HorizontalAlignment == HorizontalAlignment && other.VerticalAlignment == VerticalAlignment;
    }

    /// <summary>
    /// Calculates what margin should be applied to the content container in order to achieve the <see cref="Offset"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Based on the model of a <see cref="Widgets.Panel"/> or <see cref="Widgets.Frame"/> whose layout is set to
    /// <see cref="LayoutParameters.Fill"/> its container and who will adopt the <see cref="HorizontalAlignment"/> and
    /// <see cref="VerticalAlignment"/> of this placement as its own
    /// <see cref="Widgets.Panel.HorizontalContentAlignment"/> and <see cref="Widgets.Panel.VerticalContentAlignment"/>
    /// (or equivalent for other view types).
    /// </para>
    /// <para>
    /// Depending on the particular alignments, this can apply either positive or negative margin to either the start
    /// or end axis (or both).
    /// </para>
    /// </remarks>
    /// <returns>The margin required to apply the current <see cref="Offset"/> to a layout container whose content
    /// alignment matches the current <see cref="HorizontalAlignment"/> and <see cref="VerticalAlignment"/>.</returns>
    public Edges GetMargin()
    {
        var (x, y) = Offset;
        var (marginLeft, marginRight) = HorizontalAlignment switch
        {
            Alignment.Start => (x, 0),
            Alignment.Middle => (x, -x),
            Alignment.End => (0, -x),
            _ => (0, 0),
        };
        var (marginTop, marginBottom) = VerticalAlignment switch
        {
            Alignment.Start => (y, 0),
            Alignment.Middle => (y, -y),
            Alignment.End => (0, -y),
            _ => (0, 0),
        };
        return new(marginLeft, marginTop, marginRight, marginBottom);
    }

    /// <summary>
    /// Gets the <see cref="NineGridPlacement"/>s that neighbor the current placement, i.e. are reachable in a single
    /// <see cref="Snap(Direction, bool)"/>.
    /// </summary>
    /// <param name="avoidMiddle">Whether to avoid the exact center, i.e. having both <see cref="HorizontalAlignment"/>
    /// and <see cref="VerticalAlignment"/> be <see cref="Alignment.Middle"/>. This is often used for positioning HUD
    /// elements which typically are not useful to show in the middle of the screen, and the positioning UI may want to
    /// use that space for button prompts instead.</param>
    public IEnumerable<Neighbor> GetNeighbors(bool avoidMiddle = false)
    {
        foreach (var direction in neighborDirections)
        {
            if (Snap(direction, avoidMiddle) is NineGridPlacement neighbor)
            {
                yield return new(direction, neighbor);
            }
        }
    }

    /// <summary>
    /// Computes the aligned pixel position relative to a bounded size.
    /// </summary>
    /// <param name="size">The size of the container.</param>
    /// <returns>The aligned position, relative to the container.</returns>
    public Vector2 GetPosition(Vector2 size)
    {
        return GetPosition(size, HorizontalAlignment, VerticalAlignment) + Offset.ToVector2();
    }

    /// <summary>
    /// Checks if this placement is aligned to the exact center of the container, not counting <see cref="Offset"/>.
    /// </summary>
    public bool IsMiddle()
    {
        return HorizontalAlignment == Alignment.Middle && VerticalAlignment == Alignment.Middle;
    }

    /// <summary>
    /// Keeps the same alignments, but pushes the content farther in a specific direction.
    /// </summary>
    /// <param name="direction">Direction of the additional offset.</param>
    /// <param name="distance">Pixel distance to offset in the specified <paramref name="direction"/>.</param>
    /// <returns>A new <see cref="NineGridPlacement"/> whose alignments are the same as the current instance and whose
    /// <see cref="Offset"/> represents a move from the current offset in the specified <paramref name="direction"/>
    /// with the specified <paramref name="distance"/>.</returns>
    public NineGridPlacement Nudge(Direction direction, int distance = 1)
    {
        var newOffset = direction switch
        {
            Direction.North => new(0, -distance),
            Direction.South => new(0, distance),
            Direction.West => new(-distance, 0),
            Direction.East => new(distance, 0),
            _ => Point.Zero,
        };
        return new(HorizontalAlignment, VerticalAlignment, Offset + newOffset);
    }

    /// <summary>
    /// Snaps to an adjacent grid cell.
    /// </summary>
    /// <remarks>
    /// Causes the <see cref="Offset"/> to be reset for the newly-created placement.
    /// </remarks>
    /// <param name="direction">Direction in which to move.</param>
    /// <param name="avoidMiddle">Whether to avoid the exact center, i.e. having both <see cref="HorizontalAlignment"/>
    /// and <see cref="VerticalAlignment"/> be <see cref="Alignment.Middle"/>. This is often used for positioning HUD
    /// elements which typically are not useful to show in the middle of the screen, and the positioning UI may want to
    /// use that space for button prompts instead.</param>
    /// <returns>A new <see cref="NineGridPlacement"/> representing the adjacent cell in the specified
    /// <paramref name="direction"/>, or <c>null</c> if there is no adjacent cell (e.g. trying to snap
    /// <see cref="Direction.West"/> from a placement that is already at the horizontal <see cref="Alignment.Start"/>).
    /// </returns>
    public NineGridPlacement? Snap(Direction direction, bool avoidMiddle = false)
    {
        Alignment? horizontal = HorizontalAlignment;
        Alignment? vertical = VerticalAlignment;
        switch (direction)
        {
            case Direction.North:
                vertical = vertical switch
                {
                    Alignment.End => (horizontal == Alignment.Middle && avoidMiddle)
                        ? Alignment.Start
                        : Alignment.Middle,
                    Alignment.Middle => Alignment.Start,
                    _ => null,
                };
                break;
            case Direction.South:
                vertical = vertical switch
                {
                    Alignment.Start => (horizontal == Alignment.Middle && avoidMiddle)
                        ? Alignment.End
                        : Alignment.Middle,
                    Alignment.Middle => Alignment.End,
                    _ => null,
                };
                break;
            case Direction.West:
                horizontal = horizontal switch
                {
                    Alignment.End => (vertical == Alignment.Middle && avoidMiddle) ? Alignment.Start : Alignment.Middle,
                    Alignment.Middle => Alignment.Start,
                    _ => null,
                };
                break;
            case Direction.East:
                horizontal = horizontal switch
                {
                    Alignment.Start => (vertical == Alignment.Middle && avoidMiddle) ? Alignment.End : Alignment.Middle,
                    Alignment.Middle => Alignment.End,
                    _ => null,
                };
                break;
            default:
                throw new ArgumentException($"Invalid direction: {direction}", nameof(direction));
        }
        return horizontal.HasValue && vertical.HasValue
            ? new NineGridPlacement(horizontal.Value, vertical.Value)
            : null;
    }

    private static Vector2 GetPosition(Vector2 size, Alignment horizontalAlignment, Alignment verticalAlignment)
    {
        var x = horizontalAlignment switch
        {
            Alignment.Middle => size.X / 2,
            Alignment.End => size.X,
            _ => 0,
        };
        var y = verticalAlignment switch
        {
            Alignment.Middle => size.Y / 2,
            Alignment.End => size.Y,
            _ => 0,
        };
        return new(x, y);
    }
}
