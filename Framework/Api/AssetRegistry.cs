using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewUI.Framework.Dom;

namespace StardewUI.Framework.Api;

/// <summary>
/// Helper for registering UI assets and collections of assets.
/// </summary>
/// <remarks>
/// Methods here are generally wrapped by the <see cref="IViewEngine"/>.
/// </remarks>
internal class AssetRegistry
{
    record DirectoryMapping(string AssetPrefix, string ModDirectory);

    // Sprites maintain a reference to their Texture2D, so they should never be kept alive in their record form.
    // Instead, we can cache everything else about them (just their "data") and recreate them for any given Texture2D,
    // which is essentially how helpers like UiSprites behave.
    record SpriteCacheEntry(string TextureAssetName, Func<Texture2D, Sprite> Selector);

    private readonly IGameContentHelper gameContent;
    private readonly IMonitor monitor;
    private readonly List<DirectoryMapping> spriteDirectories = [];
    private readonly List<DirectoryMapping> viewDirectories = [];

    // Finer-grained cache than spriteSheetCache, for individual sprites.
    private readonly Dictionary<string, SpriteCacheEntry> spriteCache = new(StringComparer.OrdinalIgnoreCase);

    // SpriteSheetData is just JSON data, so there shouldn't be any special concerns for caching these aside from
    // invalidating them when SMAPI does.
    private readonly Dictionary<string, SpriteSheetData> spriteSheetCache = new(StringComparer.OrdinalIgnoreCase);

    // SMAPI doesn't like us to cache assets that it manages, especially textures. However, it also doesn't like us to
    // repeatedly call IGameContentHelper.Load repeatedly in a single frame, which is what we'd have to do in
    // TryLoadSprite without the cache, possibly dozens/hundreds of times in a single binding frame.
    // So instead we do cache, but use a WeakReference to avoid keeping them alive longer than SMAPI wants to, and we
    // also have to monitor them for invalidation and also ensure they're not disposed before using them.
    private readonly Dictionary<string, WeakReference<Texture2D>> textureCache = new(StringComparer.OrdinalIgnoreCase);

    public AssetRegistry(IGameContentHelper gameContent, IContentEvents contentEvents, IMonitor monitor)
    {
        this.gameContent = gameContent;
        this.monitor = monitor;

        contentEvents.AssetRequested += Content_AssetRequested;
        contentEvents.AssetsInvalidated += Content_AssetsInvalidated;
    }

    /// <inheritdoc cref="IViewEngine.RegisterSprites(string, string)" />
    public void RegisterSprites(string assetPrefix, string modDirectory)
    {
        if (!assetPrefix.EndsWith('/'))
        {
            assetPrefix += '/';
        }
        spriteDirectories.Add(new(assetPrefix, modDirectory));
    }

    /// <inheritdoc cref="IViewEngine.RegisterViews(string, string)" />
    public void RegisterViews(string assetPrefix, string modDirectory)
    {
        if (!assetPrefix.EndsWith('/'))
        {
            assetPrefix += '/';
        }
        viewDirectories.Add(new(assetPrefix, modDirectory));
    }

    private void Content_AssetRequested(object? sender, AssetRequestedEventArgs e)
    {
        var _ =
            (e.DataType == typeof(Document) && TryLoadAsset<object>(viewDirectories, e, "sml"))
            || (e.DataType == typeof(Texture2D) && TryLoadAsset<Texture2D>(spriteDirectories, e, "png"))
            || (
                e.DataType == typeof(SpriteSheetData)
                && TryLoadAsset<SpriteSheetData>(spriteDirectories, e, "json", "@data")
            )
            || (e.DataType == typeof(Sprite) && TryLoadSprite(e));
    }

    private void Content_AssetsInvalidated(object? sender, AssetsInvalidatedEventArgs e)
    {
        foreach (var assetName in e.Names)
        {
            textureCache.Remove(assetName.Name);
            spriteCache.Remove(assetName.Name);
            if (spriteSheetCache.Remove(assetName.Name))
            {
                // Invalidating an entire sprite sheet means we should also invalidate all of its sprites.
                // Here we make the same assumptions as TryLoadSprite, namely that sprite assets are of the form
                // "Path/To/Spritesheet:SpriteName".
                var dependentKeys = spriteCache.Keys.Where(key =>
                    key.StartsWith(assetName.Name, StringComparison.OrdinalIgnoreCase)
                );
                foreach (var key in dependentKeys)
                {
                    spriteCache.Remove(key);
                }
            }
        }
    }

    private static string? GetRelativeAssetPath(AssetRequestedEventArgs e, string prefix)
    {
        return e.Name.StartsWith(prefix) ? e.Name.Name[prefix.Length..] : null;
    }

    private SpriteSheetData GetSpriteSheetData(string assetName)
    {
        if (!spriteSheetCache.TryGetValue(assetName, out var data))
        {
            try
            {
                data = gameContent.Load<SpriteSheetData>(assetName + "@data");
            }
            catch (Exception ex)
            {
                monitor.Log($"Error loading sprite sheet data for '{assetName}': {ex}", LogLevel.Error);
                throw;
            }
            spriteSheetCache.Add(assetName, data);
        }
        return data;
    }

    private Texture2D GetTexture(string assetName)
    {
        if (textureCache.TryGetValue(assetName, out var textureRef))
        {
            if (textureRef.TryGetTarget(out var cachedTexture) && !cachedTexture.IsDisposed)
            {
                return cachedTexture;
            }
        }
        Texture2D texture;
        try
        {
            texture = gameContent.Load<Texture2D>(assetName);
        }
        catch (Exception ex)
        {
            monitor.Log($"Error loading texture for '{assetName}': {ex}", LogLevel.Error);
            throw;
        }
        textureCache[assetName] = new(texture);
        return texture;
    }

    private static bool TryLoadAsset<T>(
        IReadOnlyList<DirectoryMapping> directories,
        AssetRequestedEventArgs e,
        string extension,
        string? suffix = null
    )
        where T : notnull
    {
        foreach (var (assetPrefix, modDirectory) in directories)
        {
            var relativePath = GetRelativeAssetPath(e, assetPrefix);
            if (!string.IsNullOrEmpty(relativePath))
            {
                if (suffix is not null && relativePath.EndsWith(suffix))
                {
                    relativePath = relativePath[..^suffix.Length];
                }
                var modPath = Path.Combine(modDirectory, relativePath + '.' + extension);
                e.LoadFromModFile<T>(modPath, AssetLoadPriority.Low);
                return true;
            }
        }
        return false;
    }

    private bool TryLoadSprite(AssetRequestedEventArgs e)
    {
        if (!spriteCache.TryGetValue(e.Name.Name, out var entry))
        {
            foreach (var (assetPrefix, modDirectory) in spriteDirectories)
            {
                // Sprite paths should be of the form "Path/To/Spritesheet:SpriteName".
                //
                // We can assume there shouldn't be multiple colons in the path (as colons are not valid file system
                // characters), however we should _not_ assume that the asset/file path is not in a subdirectory.
                var relativePath = GetRelativeAssetPath(e, assetPrefix);
                if (string.IsNullOrEmpty(relativePath))
                {
                    continue;
                }
                var separatorIndex = relativePath.IndexOf(':');
                if (separatorIndex < 0)
                {
                    continue;
                }
                var spriteSheetAssetName = assetPrefix + relativePath[..separatorIndex];
                var spriteSheetData = GetSpriteSheetData(spriteSheetAssetName);
                var textureAssetName = !string.IsNullOrEmpty(spriteSheetData.TextureAssetName)
                    ? spriteSheetData.TextureAssetName
                    : spriteSheetAssetName;
                var localName = relativePath[(separatorIndex + 1)..];
                if (!spriteSheetData.Sprites.TryGetValue(localName, out var spriteData))
                {
                    continue;
                }
                entry = new(textureAssetName, spriteData.CreateSprite);
                spriteCache.Add(e.Name.Name, entry);
                break;
            }
        }
        if (entry is not null)
        {
            e.LoadFrom(
                () =>
                {
                    var texture = GetTexture(entry.TextureAssetName);
                    return entry.Selector(texture);
                },
                AssetLoadPriority.Low
            );
            return true;
        }
        return false;
    }
}
