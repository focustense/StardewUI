using Microsoft.Xna.Framework;

namespace SupplyChain.UI;

/// <summary>
/// Draws sprites according to a nine-slice scale.
/// </summary>
/// <remarks>
/// See <see href="https://en.wikipedia.org/wiki/9-slice_scaling"/>.
/// </remarks>
/// <param name="sprite">The source sprite.</param>
public class NineSlice(Sprite sprite)
{
    public Sprite Sprite { get; init; } = sprite;

    private readonly Rectangle[,] sourceGrid = GetGrid(
        sprite.SourceRect ?? sprite.Texture.Bounds,
        sprite.FixedEdges ?? Edges.NONE,
        sprite.SliceSettings);

    private Rectangle[,]? destinationGrid;
    private SimpleRotation? rotation;

    /// <summary>
    /// Draws the sprite to an <see cref="ISpriteBatch"/>, applying 9-slice scaling if specified.
    /// </summary>
    /// <param name="b">Output sprite batch.</param>
    /// <param name="tint">Optional tint multiplier color.</param>
    public void Draw(ISpriteBatch b, Color? tint = null)
    {
        if (destinationGrid is null)
        {
            // Layout has not been performed.
            return;
        }
        var rotationAngle = rotation?.Angle() ?? 0;
        for (int sourceY = 0; sourceY < sourceGrid.GetLength(0); sourceY++)
        {
            for (int sourceX = 0; sourceX < sourceGrid.GetLength(1); sourceX++)
            {
                if ((Sprite.SliceSettings?.EdgesOnly ?? false) && sourceX == 1 && sourceY == 1)
                {
                    continue;
                }
                var (destX, destY) = RotateGridIndices(sourceX, sourceY, rotation);
                var sourceRect = sourceGrid[sourceY, sourceX];
                if (sourceRect.Width == 0 || sourceRect.Height == 0)
                {
                    // If some or all of the fixed edges are zero, then there is nothing to draw for that part and we
                    // can skip some wasted cycles trying to "draw" it.
                    continue;
                }
                var destinationRect = destinationGrid[destY, destX];
                var rotationOrigin = rotation.HasValue ? sourceRect.Size.ToVector2() / 2 : Vector2.Zero;
                if (rotation.HasValue)
                {
                    // Unfortunately, setting the origin ALSO affects positioning, and XNA is very confusing about how
                    // it really works. Essentially, the "destination rect" is not a rect at all, but rather the origin
                    // point (which we've just set to be the center) and width/height.
                    // So we need to completely recalculate it here, which means first computing the scale to figure out
                    // what the effect of changing the *source* origin is on the *destination* position.
                    var scale = destinationRect.Size.ToVector2() / sourceRect.Size.ToVector2();
                    destinationRect.Offset(rotationOrigin * scale);
                }
                b.Draw(Sprite.Texture, destinationRect, sourceRect, tint, rotationAngle, rotationOrigin);
            }
        }
    }

    /// <summary>
    /// Prepares the layout for next <see cref="Draw"/>.
    /// </summary>
    /// <param name="destinationRect">The rectangular area that the drawn sprite should fill.</param>
    /// <param name="rotation">Rotation to apply to the source sprite, if any.</param>
    public void Layout(Rectangle destinationRect, SimpleRotation? rotation = null)
    {
        var destinationEdges = Sprite.FixedEdges ?? Edges.NONE;
        if (rotation is not null)
        {
            destinationEdges = destinationEdges.Rotate(rotation.Value);
        }
        this.rotation = rotation;
        destinationGrid = GetGrid(destinationRect, destinationEdges);
    }

    private static Rectangle[,] GetGrid(Rectangle bounds, Edges fixedEdges, SliceSettings? settings = null)
    {
        var left = bounds.X;
        var top = bounds.Y;
        var centerX = settings?.CenterX is int cx ? cx : left + fixedEdges.Left;
        var centerY = settings?.CenterY is int cy ? cy : top + fixedEdges.Top;
        var innerWidth = bounds.Right - fixedEdges.Right - centerX;
        var innerHeight = bounds.Bottom - fixedEdges.Bottom - centerY;
        var startRight = bounds.Right - fixedEdges.Right;
        var startBottom = bounds.Bottom - fixedEdges.Bottom;
        return new Rectangle[3, 3]
        {
            {
                new(left, top, fixedEdges.Left, fixedEdges.Top),
                new(centerX, top, innerWidth, fixedEdges.Top),
                new(startRight, top, fixedEdges.Right, fixedEdges.Top),
            },
            {
                new(left, centerY, fixedEdges.Left, innerHeight),
                new(centerX, centerY, innerWidth, innerHeight),
                new(startRight, centerY, fixedEdges.Right, innerHeight),
            },
            {
                new(left, startBottom, fixedEdges.Left, fixedEdges.Bottom),
                new(centerX, startBottom, innerWidth, fixedEdges.Bottom),
                new(startRight, startBottom, fixedEdges.Right, fixedEdges.Bottom),
            }
        };
    }

    private static (int x, int y) RotateGridIndices(int x, int y, SimpleRotation? rotation)
    {
        return rotation switch
        {
            SimpleRotation.QuarterClockwise => (2 - y, x),
            SimpleRotation.QuarterCounterclockwise => (y, 2 - x),
            SimpleRotation.Half => (2 - x, 2 - y),
            _ => (x, y),
        };
    }
}
