using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewUI;

/// <summary>
/// Definition for a scalable sprite.
/// </summary>
/// <param name="Texture">The texture containing the sprite's pixel data.</param>
/// <param name="SourceRect">The inner area of the <paramref name="Texture"/> in which the specific image is located, or
/// <c>null</c> to draw the entire texture.</param>
/// <param name="FixedEdges">The thickness of each "fixed" edge to use with 9-patch/9-slice scaling. Specifying these
/// values can prevent corner distortion for images that have been designed for such scaling. See
/// <see href="https://en.wikipedia.org/wiki/9-slice_scaling"/>. for a detailed explanation.</param>
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
/// <param name="CenterX">The origin (left) point where the horizontal "center" is considered to start, or <c>null</c>
/// to start where the left fixed edge ends.</param>
/// <param name="CenterY">The origin (top) point where the vertical "center" is considered to start, or <c>null</c> to
/// start where the top fixed edge ends..</param>
/// <param name="Scale">Scale to apply to the slices themselves; for example, if a 16x16 source draws to a 64x64 target,
/// and a scale of 2 is used, then a 2x3 border slice would draw as 16x24 (normal 8x16, multiplied by 2).</param>
/// <param name="EdgesOnly">If <c>true</c>, then only the outer 8 edge segments should be drawn, and the 9th
/// (horizontal and vertical middle, i.e. "background") segment will be ignored.</param>
public record SliceSettings(
    int? CenterX = null,
    SliceCenterPosition CenterXPosition = SliceCenterPosition.Start,
    int? CenterY = null,
    SliceCenterPosition CenterYPosition = SliceCenterPosition.Start,
    int Scale = 1,
    bool EdgesOnly = false
);

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
