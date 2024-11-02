using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewUI.Layout;

namespace StardewUI.Graphics;

/// <summary>
/// Definition for a scalable sprite.
/// </summary>
/// <param name="Texture">The texture containing the sprite's pixel data.</param>
/// <param name="SourceRect">The inner area of the <paramref name="Texture"/> in which the specific image is located, or
/// <c>null</c> to draw the entire texture.</param>
/// <param name="FixedEdges">The thickness of each "fixed" edge to use with 9-patch/9-slice scaling. Specifying these
/// values can prevent corner distortion for images that have been designed for such scaling. See
/// <see href="https://en.wikipedia.org/wiki/9-slice_scaling">Nine-Slice Scaling</see> for a detailed
/// explanation.</param>
/// <param name="SliceSettings">Additional settings for the scaling and slicing behavior.</param>
[DuckType]
public record Sprite(
    Texture2D Texture,
    Rectangle? SourceRect = null,
    Edges? FixedEdges = null,
    SliceSettings? SliceSettings = null
)
{
    /// <summary>
    /// The size (width/height) of the sprite, in pixels.
    /// </summary>
    public Point Size => SourceRect?.Size ?? Texture.Bounds.Size;
}

/// <summary>
/// Additional nine-slice settings for dealing with certain "unique" structures.
/// </summary>
/// <param name="CenterX">The X position to use for the horizontal center slices, or <c>null</c> to start where the left
/// fixed edge ends.</param>
/// <param name="CenterXPosition">Specifies whether the <see cref="CenterX"/> should be understood as the start position
/// or the end position of the horizontal center slice.</param>
/// <param name="CenterY">The Y position to use for the vertical center slices, or <c>null</c> to start where the top
/// fixed edge ends.</param>
/// <param name="CenterYPosition">Specifies whether the <see cref="CenterY"/> should be understood as the start position
/// or the end position of the vertical center slice.</param>
/// <param name="Scale">Scale to apply to the slices themselves; for example, if a 16x16 source draws to a 64x64 target,
/// and a scale of 2 is used, then a 2x3 border slice would draw as 16x24 (normal 8x16, multiplied by 2).</param>
/// <param name="EdgesOnly">If <c>true</c>, then only the outer 8 edge segments should be drawn, and the 9th
/// (horizontal and vertical middle, i.e. "background") segment will be ignored.</param>
[DuckType]
public record SliceSettings(
    int? CenterX = null,
    SliceCenterPosition CenterXPosition = SliceCenterPosition.Start,
    int? CenterY = null,
    SliceCenterPosition CenterYPosition = SliceCenterPosition.Start,
    float Scale = 1,
    bool EdgesOnly = false
)
{
    /// <summary>
    /// Creates a copy of this <see cref="SliceSettings"/> with a different scale.
    /// </summary>
    /// <param name="newScale">The scale to use.</param>
    /// <returns>A copy of this <see cref="SliceSettings"/> with its <see cref="Scale"/> set to
    /// <paramref name="newScale"/>.</returns>
    public SliceSettings WithScale(float newScale)
    {
        return new(CenterX, CenterXPosition, CenterY, CenterYPosition, newScale, EdgesOnly);
    }
}

/// <summary>
/// Specifies which side the center position of a <see cref="SliceSettings"/> instance is on.
/// </summary>
public enum SliceCenterPosition
{
    /// <summary>
    /// The specified center position is the start of the center segment.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The center segment is adjacent to the end segment, and there is a gap between the right/top of the start segment
    /// and the left/bottom of the center segment.
    /// </para>
    /// <example>
    /// Example of a horizontal center position using this setting:
    /// <code>
    /// +---------------------------------------------+
    /// | [Top Left] XXXXXXX [Top Center] [Top Right] |
    /// | [Mid Left] XXXXXXX [Mid Center] [Mid Right] |
    /// | [Bot Left] XXXXXXX [Bot Center] [Bot Right] |
    /// +---------------------------------------------+
    /// </code>
    /// </example>
    /// </remarks>
    Start,

    /// <summary>
    /// The specified center position is the end of the center segment.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The center segment is adjacent to the start segment, and there is a gap between the right/top of the center
    /// segment and the left/bottom of the end segment.
    /// </para>
    /// <example>
    /// Example of a horizontal center position using this setting:
    /// <code>
    /// +---------------------------------------------+
    /// | [Top Left] [Top Center] XXXXXXX [Top Right] |
    /// | [Mid Left] [Mid Center] XXXXXXX [Mid Right] |
    /// | [Bot Left] [Bot Center] XXXXXXX [Bot Right] |
    /// +---------------------------------------------+
    /// </code>
    /// </example>
    /// </remarks>
    End,
}
