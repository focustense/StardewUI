using StardewUI.Framework.Converters;
using StardewUI.Graphics;
using StardewValley;

namespace StardewUITestAddon;

internal class ItemIdToSpriteConverter : IValueConverter<string, Sprite>
{
    public Sprite Convert(string value)
    {
        var itemData = ItemRegistry.GetDataOrErrorItem(value);
        return new(itemData.GetTexture(), itemData.GetSourceRect());
    }
}
