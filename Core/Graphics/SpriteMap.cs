using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;

namespace StardewUI.Graphics;

/// <summary>
/// General implementation of an <see cref="ISpriteMap{T}"/> that can be prepared in a variety of ways.
/// </summary>
/// <remarks>
/// Can be constructed directly, but it is normally recommended to use <see cref="SpriteMapBuilder{T}"/>. Applies basic
/// placeholder logic that considers only the <paramref name="defaultSprite"/> to be a placeholder.
/// </remarks>
/// <typeparam name="T">Type of key for which to obtain sprites.</typeparam>
/// <param name="sprites">Map of keys to sprites.</param>
/// <param name="defaultSprite">Default sprite to show when looking up a key without a corresponding sprite.</param>
public class SpriteMap<T>(IReadOnlyDictionary<T, Sprite> sprites, Sprite defaultSprite) : ISpriteMap<T>
{
    /// <inheritdoc />
    public Sprite Get(T key, out bool isPlaceholder)
    {
        if (sprites.TryGetValue(key, out var sprite))
        {
            isPlaceholder = false;
            return sprite;
        }
        isPlaceholder = true;
        return defaultSprite;
    }
}

/// <summary>
/// Builder interface for a <see cref="SpriteMap{T}"/> using a single texture source.
/// </summary>
/// <remarks>
/// Works by maintaining a virtual "cursor" which can be moved to capture the next sprite, and adding either one sprite
/// at a time with a specific size, or several with the same size, wrapping around when necessary.
/// </remarks>
/// <typeparam name="T">Type of key for which to obtain sprites.</typeparam>
public class SpriteMapBuilder<T>(Texture2D texture)
    where T : notnull
{
    private readonly Dictionary<T, Sprite> sprites = [];

    private int cursorX;
    private int cursorY;
    private Sprite defaultSprite = new(Game1.staminaRect);
    private int paddingX;
    private int paddingY;
    private int spriteHeight;
    private int spriteWidth;

    /// <summary>
    /// Adds a single sprite.
    /// </summary>
    /// <param name="key">The key for the sprite.</param>
    /// <param name="width">Optional override width, otherwise the most recent <see cref="Size"/> will be used. Custom
    /// widths only apply to this sprite and will <b>not</b> affect the size of any subsequent additions.</param>
    /// <param name="height">Optional override height, otherwise the most recent <see cref="Size"/> will be used. Custom
    /// heights only apply to this sprite and will <b>not</b> affect the size of any subsequent additions.</param>
    /// <returns>The current builder instance.</returns>
    /// <exception cref="ArgumentException">Thrown when the combination of current position and current or overridden
    /// width and height would place the sprite outside the texture bounds.</exception>
    public SpriteMapBuilder<T> Add(T key, int? width = null, int? height = null)
    {
        return Add(key, new Rectangle(cursorX, cursorY, width ?? spriteWidth, height ?? spriteHeight));
    }

    /// <summary>
    /// Adds a sprite using its specific position and size in the texture.
    /// </summary>
    /// <remarks>
    /// After adding, moves the cursor to the top-right of the <paramref name="sourceRect"/>, unless it would be
    /// horizontally out of bounds, in which case it wraps to the beginning (X = 0) of the next row.
    /// </remarks>
    /// <param name="key">The key for the sprite.</param>
    /// <param name="sourceRect">The exact position and size of the sprite.</param>
    /// <returns>The current builder instance.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="sourceRect"/> is not fully within the boundaries
    /// of the source texture.</exception>
    public SpriteMapBuilder<T> Add(T key, Rectangle sourceRect)
    {
        if (sourceRect.Width <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(sourceRect),
                $"Rectangle {sourceRect} has invalid zero or negative width."
            );
        }
        if (sourceRect.Height <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(sourceRect),
                $"Rectangle {sourceRect} has invalid zero or negative height."
            );
        }
        if (!IsInBounds(sourceRect))
        {
            throw new ArgumentException(
                $"Rectangle {sourceRect} is outside the texture bounds ({texture.Width}, {texture.Height}).",
                nameof(sourceRect)
            );
        }
        sprites.Add(key, new(texture, sourceRect));
        cursorX = sourceRect.Right + paddingX;
        cursorY = sourceRect.Top;
        if (cursorX >= texture.Width)
        {
            cursorX = 0;
            cursorY += sourceRect.Height + paddingY;
        }
        return this;
    }

    /// <summary>
    /// Adds a sequence of sprites, starting from the current cursor position and using the most recently configured
    /// <see cref="Size"/> and <see cref="Padding"/> to advance the cursor after each element.
    /// </summary>
    /// <remarks>
    /// Wraps to the beginning of the next row (X = 0) when the end of a row is reached.
    /// </remarks>
    /// <param name="keys">The keys for the sprites to add, in the same left-to-right order that they appear in the
    /// source texture.</param>
    /// <returns>The current builder instance.</returns>
    /// <exception cref="ArgumentException">Thrown if the bottom-right boundary of the source texture is reached and
    /// there are still elements remaining to be added.</exception>
    public SpriteMapBuilder<T> Add(IEnumerable<T> keys)
    {
        foreach (var key in keys)
        {
            Add(key);
        }
        return this;
    }

    /// <summary>
    /// Adds a sequence of sprites, starting from the current cursor position and using the most recently configured
    /// <see cref="Size"/> and <see cref="Padding"/> to advance the cursor after each element.
    /// </summary>
    /// <remarks>
    /// Wraps to the beginning of the next row (X = 0) when the end of a row is reached.
    /// </remarks>
    /// <param name="keys">The keys for the sprites to add, in the same left-to-right order that they appear in the
    /// source texture.</param>
    /// <returns>The current builder instance.</returns>
    /// <exception cref="ArgumentException">Thrown if the bottom-right boundary of the source texture is reached and
    /// there are still elements remaining to be added.</exception>
    public SpriteMapBuilder<T> Add(params T[] keys)
    {
        return Add(keys.AsEnumerable());
    }

    /// <summary>
    /// Builds a new <see cref="SpriteMap{T}"/> from the registered sprites.
    /// </summary>
    public SpriteMap<T> Build()
    {
        return new(sprites, defaultSprite);
    }

    /// <summary>
    /// Configures the default sprite for unknown keys to use an existing sprite that has already been registered.
    /// </summary>
    /// <param name="key">Key of the previously-added sprite to use as default.</param>
    /// <returns>The current builder instance.</returns>
    public SpriteMapBuilder<T> Default(T key)
    {
        defaultSprite = sprites[key];
        return this;
    }

    /// <summary>
    /// Configures the default sprite for unknown keys to use a custom sprite.
    /// </summary>
    /// <param name="sprite">The sprite to use as default.</param>
    /// <returns>The current builder instance.</returns>
    public SpriteMapBuilder<T> Default(Sprite sprite)
    {
        defaultSprite = sprite;
        return this;
    }

    /// <summary>
    /// Moves the current cursor position by a specified offset.
    /// </summary>
    /// <param name="x">Horizontal offset from current position.</param>
    /// <param name="y">Vertical offset from current position.</param>
    /// <returns>The current builder instance.</returns>
    public SpriteMapBuilder<T> MoveBy(int x, int y)
    {
        cursorX += x;
        cursorY += y;
        return this;
    }

    /// <summary>
    /// Moves the current cursor position by a specified offset.
    /// </summary>
    /// <param name="p">The horizontal (X) and vertical (Y) offsets from the current position.</param>
    /// <returns>The current builder instance.</returns>
    public SpriteMapBuilder<T> MoveBy(Point p)
    {
        return MoveBy(p.X, p.Y);
    }

    /// <summary>
    /// Moves the cursor to a specific coordinate.
    /// </summary>
    /// <remarks>
    /// Generally used when dealing with semi-regular spritesheets having distinct areas that are individually uniform
    /// but different from each other, e.g. a row of 10x10 placed in an empty area of a 32x32 sheet.
    /// </remarks>
    /// <param name="x">The new X coordinate of the cursor.</param>
    /// <param name="y">The new Y coordinate of the cursor.</param>
    /// <returns>The current builder instance.</returns>
    public SpriteMapBuilder<T> MoveTo(int x, int y)
    {
        cursorX = x;
        cursorY = y;
        return this;
    }

    /// <summary>
    /// Moves the cursor to a specific coordinate.
    /// </summary>
    /// <remarks>
    /// Generally used when dealing with semi-regular spritesheets having distinct areas that are individually uniform
    /// but different from each other, e.g. a row of 10x10 placed in an empty area of a 32x32 sheet.
    /// </remarks>
    /// <param name="p">The horizontal (X) and vertical (Y) coordinates for the cursor.</param>
    /// <returns>The current builder instance.</returns>
    public SpriteMapBuilder<T> MoveTo(Point p)
    {
        return MoveTo(p.X, p.Y);
    }

    /// <summary>
    /// Configures the padding between sprites.
    /// </summary>
    /// <remarks>
    /// Applies only to new sprites added afterward; will not affect sprites previously added.
    /// </remarks>
    /// <param name="x">Horizontal padding from the right edge of one sprite to the left edge of the next. Added
    /// whenever advancing from left to right.</param>
    /// <param name="y">Vertical padding from the bottom edge of one sprite to the top edge of the next. Added whenever
    /// wrapping from the end of a row to the beginning of the next row.</param>
    /// <returns>The current builder instance.</returns>
    public SpriteMapBuilder<T> Padding(int x, int y)
    {
        paddingX = x;
        paddingY = y;
        return this;
    }

    /// <summary>
    /// Configures the padding between sprites.
    /// </summary>
    /// <remarks>
    /// Applies only to new sprites added afterward; will not affect sprites previously added.
    /// </remarks>
    /// <param name="p">A point containing the horizontal (X) and vertical (Y) padding values. See
    /// <see cref="Padding(int, int)"/>.</param>
    /// <returns>The current builder instance.</returns>
    public SpriteMapBuilder<T> Padding(Point p)
    {
        return Padding(p.X, p.Y);
    }

    /// <summary>
    /// Configures the pixel size per sprite.
    /// </summary>
    /// <remarks>
    /// Applies only to new sprites added afterward; will not affect sprites previously added.
    /// </remarks>
    /// <param name="width">The pixel width for newly-added sprites.</param>
    /// <param name="height">The pixel height for newly-added sprites.</param>
    /// <returns>The current builder instance.</returns>
    public SpriteMapBuilder<T> Size(int width, int height)
    {
        spriteWidth = width;
        spriteHeight = height;
        return this;
    }

    /// <summary>
    /// Configures the pixel size per sprite.
    /// </summary>
    /// <remarks>
    /// Applies only to new sprites added afterward; will not affect sprites previously added.
    /// </remarks>
    /// <param name="p">A point containing the width (X) and height (Y) for newly-added sprites.</param>
    /// <returns>The current builder instance.</returns>
    public SpriteMapBuilder<T> Size(Point p)
    {
        return Size(p.X, p.Y);
    }

    private bool IsInBounds(Rectangle sourceRect)
    {
        return sourceRect.Left >= 0
            && sourceRect.Top >= 0
            && sourceRect.Right <= texture.Width
            && sourceRect.Bottom <= texture.Height;
    }
}
