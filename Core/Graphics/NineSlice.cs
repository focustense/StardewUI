using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewUI.Layout;

namespace StardewUI.Graphics;

/// <summary>
/// Draws sprites according to a <see href="https://en.wikipedia.org/wiki/9-slice_scaling">nine-slice scale</see>.
/// </summary>
/// <param name="sprite">The source sprite.</param>
public class NineSlice(Sprite sprite)
{
    /// <summary>
    /// The source sprite.
    /// </summary>
    public Sprite Sprite { get; init; } = sprite;

    private readonly Rectangle[,] sourceGrid = GetGrid(
        sprite.SourceRect ?? sprite.Texture.Bounds,
        sprite.FixedEdges ?? Edges.NONE,
        sprite.SliceSettings
    );

    private Rectangle[,]? destinationGrid;
    private SimpleRotation? rotation;

    /// <summary>
    /// Draws the sprite to an <see cref="ISpriteBatch"/>, applying 9-slice scaling if specified.
    /// </summary>
    /// <param name="b">Output sprite batch.</param>
    /// <param name="tint">Optional tint multiplier color.</param>
    /// <param name="effects">Sprite effect to apply, e.g. for drawing flipped.</param>
    public void Draw(ISpriteBatch b, Color? tint = null, SpriteEffects effects = SpriteEffects.None)
    {
        if (destinationGrid is null)
        {
            // Layout has not been performed.
            return;
        }
        var rotationAngle = rotation?.Angle() ?? 0;
        var rotationCenter = new Vector2(0.5f, 0.5f);
        using var _transform = b.SaveTransform();
        for (int sourceY = 0; sourceY < sourceGrid.GetLength(0); sourceY++)
        {
            for (int sourceX = 0; sourceX < sourceGrid.GetLength(1); sourceX++)
            {
                if ((Sprite.SliceSettings?.EdgesOnly ?? false) && sourceX == 1 && sourceY == 1)
                {
                    continue;
                }
                var (destX, destY) = RotateGridIndices(sourceX, sourceY, rotation, effects);
                var sourceRect = sourceGrid[sourceY, sourceX];
                if (sourceRect.Width == 0 || sourceRect.Height == 0)
                {
                    // If some or all of the fixed edges are zero, then there is nothing to draw for that part and we
                    // can skip some wasted cycles trying to "draw" it.
                    continue;
                }
                var destinationRect = destinationGrid[destY, destX];
                if (rotation.HasValue)
                {
                    var location = destinationRect.Location.ToVector2();
                    var transformOrigin = new TransformOrigin(
                        rotationCenter,
                        rotationCenter * destinationRect.Size.ToVector2()
                    );
                    b.Translate(location);
                    b.Rotate(rotationAngle, transformOrigin);
                    b.Draw(
                        Sprite.Texture,
                        new Rectangle(Point.Zero, destinationRect.Size),
                        sourceRect,
                        tint,
                        0,
                        effects
                    );
                    // We reverse the transformations explicitly instead of using SaveTransform() in this loop in order
                    // to avoid the possibility of repeatedly restoring a partially-local transform that needs to be
                    // collapsed and trigger a new sprite batch each time. If the negative rotation reverts the local
                    // rotation back to 0 - which is what should usually happen - then the subsequent translation is
                    // "free" in that it won't force a new global transform, but will also preserve any global transform
                    // that may have been lazily created after the very first translate/rotate.
                    b.Rotate(-rotationAngle, transformOrigin);
                    b.Translate(-location);
                }
                else
                {
                    b.Draw(Sprite.Texture, destinationRect, sourceRect, tint, effects: effects);
                }
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
        var sliceScale = Sprite.SliceSettings?.Scale ?? 1;
        if (rotation is not null)
        {
            destinationEdges = destinationEdges.Rotate(rotation.Value);
        }
        this.rotation = rotation;
        destinationGrid = GetGrid(destinationRect, destinationEdges, sliceScale: sliceScale);
    }

    private static Rectangle[,] GetGrid(
        Rectangle bounds,
        Edges fixedEdges,
        SliceSettings? settings = null,
        // We pass sliceScale separately (even though it is in SliceSettings) because it actually applies only to the
        // destination rect, not the source, whereas the SliceSettings are used for source but not destination.
        float sliceScale = 1
    )
    {
        var left = bounds.X;
        var top = bounds.Y;
        if (sliceScale != 1)
        {
            fixedEdges *= sliceScale;
        }
        var centerStartX = left + fixedEdges.Left;
        var centerStartY = top + fixedEdges.Top;
        var centerEndX = bounds.Right - fixedEdges.Right;
        var centerEndY = bounds.Bottom - fixedEdges.Bottom;
        if (settings?.CenterX is int cx)
        {
            if (settings.CenterXPosition == SliceCenterPosition.Start)
            {
                centerStartX = cx;
            }
            else
            {
                centerEndX = cx;
            }
        }
        if (settings?.CenterY is int cy)
        {
            if (settings.CenterYPosition == SliceCenterPosition.Start)
            {
                centerStartY = cy;
            }
            else
            {
                centerEndY = cy;
            }
        }
        var innerWidth = centerEndX - centerStartX;
        var innerHeight = centerEndY - centerStartY;
        var startRight = bounds.Right - fixedEdges.Right;
        var startBottom = bounds.Bottom - fixedEdges.Bottom;
        return new Rectangle[3, 3]
        {
            {
                new(left, top, fixedEdges.Left, fixedEdges.Top),
                new(centerStartX, top, innerWidth, fixedEdges.Top),
                new(startRight, top, fixedEdges.Right, fixedEdges.Top),
            },
            {
                new(left, centerStartY, fixedEdges.Left, innerHeight),
                new(centerStartX, centerStartY, innerWidth, innerHeight),
                new(startRight, centerStartY, fixedEdges.Right, innerHeight),
            },
            {
                new(left, startBottom, fixedEdges.Left, fixedEdges.Bottom),
                new(centerStartX, startBottom, innerWidth, fixedEdges.Bottom),
                new(startRight, startBottom, fixedEdges.Right, fixedEdges.Bottom),
            },
        };
    }

    private static (int x, int y) RotateGridIndices(
        int x,
        int y,
        SimpleRotation? rotation,
        SpriteEffects effects = SpriteEffects.None
    )
    {
        if ((effects & SpriteEffects.FlipHorizontally) != 0)
        {
            x = 2 - x;
        }
        if ((effects & SpriteEffects.FlipVertically) != 0)
        {
            y = 2 - y;
        }
        return rotation switch
        {
            SimpleRotation.QuarterClockwise => (2 - y, x),
            SimpleRotation.QuarterCounterclockwise => (y, 2 - x),
            SimpleRotation.Half => (2 - x, 2 - y),
            _ => (x, y),
        };
    }
}
