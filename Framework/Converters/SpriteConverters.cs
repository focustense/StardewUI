using Microsoft.Xna.Framework.Graphics;
using StardewValley.ItemTypeDefinitions;

namespace StardewUI.Framework.Converters;

/// <summary>
/// Converts data from a game item to its corresponding sprite.
/// </summary>
public class ItemSpriteConverter : IValueConverter<ParsedItemData, Sprite>
{
    /// <inheritdoc />
    public Sprite Convert(ParsedItemData value)
    {
        return new(value.GetTexture(), value.GetSourceRect());
    }
}

/// <summary>
/// Converts a tuple with a texture and source rectangle (within the texture) to a sprite record.
/// </summary>
public class TextureRectSpriteConverter : IValueConverter<Tuple<Texture2D, Rectangle>, Sprite>
{
    /// <inheritdoc />
    public Sprite Convert(Tuple<Texture2D, Rectangle> value)
    {
        return new(value.Item1, value.Item2);
    }
}

/// <summary>
/// Converts a texture to a sprite record, using the texture's entire bounds as the source rectangle.
/// </summary>
public class TextureSpriteConverter : IValueConverter<Texture2D, Sprite>
{
    /// <inheritdoc />
    public Sprite Convert(Texture2D value)
    {
        return new(value);
    }
}
