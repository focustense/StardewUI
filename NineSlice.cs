using Microsoft.Xna.Framework;

namespace SupplyChain.UI;

public class NineSlice(Sprite sprite)
{
    public Sprite Sprite { get; init; } = sprite;

    private readonly Rectangle[,] sourceGrid = GetGrid(
        sprite.SourceRect ?? sprite.Texture.Bounds,
        sprite.FixedEdges ?? Edges.NONE,
        sprite.SliceSettings);

    private Rectangle[,]? destinationGrid;

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
        for (int y = 0; y < destinationGrid.GetLength(0); y++)
        {
            for (int x = 0; x < destinationGrid.GetLength(1); x++)
            {
                if ((Sprite.SliceSettings?.EdgesOnly ?? false) && x == 1 && y == 1)
                {
                    continue;
                }
                var sourceRect = sourceGrid[y, x];
                if (sourceRect.Width == 0 || sourceRect.Height == 0)
                {
                    // If some or all of the fixed edges are zero, then there is nothing to draw for that part and we
                    // can skip some wasted cycles trying to "draw" it.
                    continue;
                }
                b.Draw(Sprite.Texture, destinationGrid[y, x], sourceRect, tint);
            }
        }
    }

    /// <summary>
    /// Prepares the layout for next <see cref="Draw"/>.
    /// </summary>
    /// <param name="destinationRect">The rectangular area that the drawn sprite should fill.</param>
    public void Layout(Rectangle destinationRect)
    {
        destinationGrid = GetGrid(destinationRect, Sprite.FixedEdges ?? Edges.NONE);
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
}
