using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace StardewUI.Framework.Content;

/// <summary>
/// Provides all built-in <see cref="UiSprites"/> for use in asset queries.
/// </summary>
internal static class UiSpriteProvider
{
    /// <summary>
    /// Asset name prefix that should be used for all sprite assets.
    /// </summary>
    public static readonly string AssetNamePrefix = "Mods/StardewUI/Sprites/";

    // It is "probably" OK to do this using reflection, since it only ever has to be done once.
    // However, since we own UiSprites, this could be sped up considerably by simply hard-coding all the entries, or,
    // better yet, using a code generator.
    private static readonly Dictionary<string, Func<Sprite>> sprites = typeof(UiSprites)
        .GetProperties(BindingFlags.Static | BindingFlags.Public)
        .Where(prop => prop.PropertyType == typeof(Sprite) && prop.CanRead && prop.GetIndexParameters().Length == 0)
        .ToDictionary(prop => prop.Name, prop => prop.GetGetMethod()!.CreateDelegate<Func<Sprite>>());

    /// <summary>
    /// Retrieves a sprite by its asset name, or throws if not found.
    /// </summary>
    /// <param name="name">The name of the sprite, which is the property name in <see cref="UiSprites"/>.</param>
    /// <returns>The <see cref="Sprite"/> with the specified asset <paramref name="name"/>.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the asset name does match a known sprite.</exception>
    public static Sprite GetSprite(string name)
    {
        return TryGetSprite(name, out var sprite)
            ? sprite
            : throw new KeyNotFoundException($"No sprite is registered under the asset name '{name}'.");
    }

    /// <summary>
    /// Attempts to retrieve a sprite by its asset name.
    /// </summary>
    /// <param name="name">The name of the sprite, which is the property name in <see cref="UiSprites"/>.</param>
    /// <param name="sprite">If the method returns <c>true</c>, contains the <see cref="Sprite"/> assigned to the
    /// specified <paramref name="name"/>; otherwise, <c>null</c>.</param>
    /// <returns><c>true</c> if a built-in sprite with the specified <paramref name="name"/> exists; otherwise,
    /// <c>false</c>.</returns>
    public static bool TryGetSprite(string name, [MaybeNullWhen(false)] out Sprite sprite)
    {
        if (!name.StartsWith(AssetNamePrefix))
        {
            sprite = null;
            return false;
        }
        var unprefixedName = name[AssetNamePrefix.Length..];
        if (sprites.TryGetValue(unprefixedName, out var selector))
        {
            sprite = selector();
            return true;
        }
        sprite = null;
        return false;
    }
}
