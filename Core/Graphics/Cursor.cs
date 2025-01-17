using Microsoft.Xna.Framework;

namespace StardewUI.Graphics;

/// <summary>
/// Defines a cursor that can be drawn instead of or in addition to the regular mouse pointer.
/// </summary>
/// <param name="Sprite">The sprite to draw.</param>
/// <param name="Size">Size with which to draw the <see cref="Sprite"/>, if different from the size of the sprite's
/// <see cref="Sprite.SourceRect"/>.</param>
/// <param name="Offset">Offset from the exact mouse position where the sprite should be drawn. This always refers to
/// the top-left corner of the sprite. If not specified, uses <see cref="DefaultOffset"/>.</param>
/// <param name="Tint">Tint color for the sprite. If not specified, uses <see cref="DefaultTint"/>.</param>
public record Cursor(Sprite Sprite, Point? Size = null, Point? Offset = null, Color? Tint = null)
{
    /// <summary>
    /// The default offset to apply when <see cref="Offset"/> is not specified.
    /// </summary>
    public static readonly Point DefaultOffset = new(16, 16);

    /// <summary>
    /// The default color to tint with when <see cref="Tint"/> is not specified.
    /// </summary>
    public static readonly Color DefaultTint = Color.White;
}
