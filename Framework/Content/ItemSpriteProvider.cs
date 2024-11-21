using System.Diagnostics.CodeAnalysis;
using StardewUI.Graphics;

namespace StardewUI.Framework.Content;

/// <summary>
/// Provides sprites for in-game items based on their IDs.
/// </summary>
internal static class ItemSpriteProvider
{
    /// <summary>
    /// Asset name prefix that should be used for all sprite assets.
    /// </summary>
    public static readonly string AssetNamePrefix = "Item/";

    /// <summary>
    /// Retrieves a sprite by its asset name, or throws if the asset name is not valid.
    /// </summary>
    /// <param name="name">The name of the sprite, which should be the <see cref="AssetNamePrefix"/> followed by a
    /// qualified item ID.</param>
    /// <returns>The <see cref="Sprite"/> for the specified item, or an error sprite if not available.</returns>
    /// <exception cref="ArgumentException">Thrown when the asset name does not have the correct prefix.</exception>
    public static Sprite GetSprite(string name)
    {
        return TryGetSprite(name, out var sprite)
            ? sprite
            : throw new ArgumentException(
                $"Invalid format '{name}' for an item sprite asset; must start with the prefix '{AssetNamePrefix}.",
                nameof(name)
            );
    }

    /// <summary>
    /// Attempts to retrieve a sprite by its asset name.
    /// </summary>
    /// <param name="name">The name of the sprite, which should be the <see cref="AssetNamePrefix"/> followed by a
    /// qualified item ID.</param>
    /// <param name="sprite">If the method returns <c>true</c>, contains the <see cref="Sprite"/> for the specified item
    /// or an error item if unavailable; otherwise, <c>null</c>.</param>
    /// <returns><c>true</c> if the <paramref name="name"/> was a valid asset name for an item sprite; otherwise,
    /// <c>false</c>.</returns>
    public static bool TryGetSprite(string name, [MaybeNullWhen(false)] out Sprite sprite)
    {
        if (!name.StartsWith(AssetNamePrefix))
        {
            sprite = null;
            return false;
        }
        var itemId = name[AssetNamePrefix.Length..];
        var itemData = ItemRegistry.GetDataOrErrorItem(itemId);
        sprite = new(itemData.GetTexture(), itemData.GetSourceRect(), SliceSettings: new(Scale: 4));
        return true;
    }
}
