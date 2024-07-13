using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SupplyChain.UI;

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
    SliceSettings? SliceSettings = null);

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
public record SliceSettings(int? CenterX = null, int? CenterY = null, int Scale = 1, bool EdgesOnly = false);