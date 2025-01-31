using StardewModdingAPI.Events;

namespace StardewUI.Framework;

/// <summary>
/// Helper for lazy-loading a single asset as game data, sourced from mod data.
/// </summary>
/// <typeparam name="T">The asset type.</typeparam>
internal class LazyAsset<T>
    where T : class
{
    public T Asset => GetOrLoadAsset();

    private readonly IGameContentHelper gameContent;
    private readonly string name;
    private readonly string physicalPath;

    private T? asset;

    /// <summary>
    /// Initializes the asset and configures it for lazy loading.
    /// </summary>
    /// <param name="helper">Helper for the mod that owns the real asset.</param>
    /// <param name="name">The asset name in the content pipeline.</param>
    /// <param name="physicalPath">The physical location of the asset, relative to the mod directory.</param>
    public LazyAsset(IModHelper helper, string name, string physicalPath)
    {
        gameContent = helper.GameContent;
        this.name = name;
        this.physicalPath = physicalPath;
        helper.Events.Content.AssetRequested += ContentEvents_AssetRequested;
        helper.Events.Content.AssetsInvalidated += ContentEvents_AssetsInvalidated;
    }

    private void ContentEvents_AssetRequested(object? sender, AssetRequestedEventArgs e)
    {
        if (e.Name.IsEquivalentTo(name))
        {
            e.LoadFromModFile<T>(physicalPath, AssetLoadPriority.Exclusive);
        }
    }

    private void ContentEvents_AssetsInvalidated(object? sender, AssetsInvalidatedEventArgs e)
    {
        if (e.NamesWithoutLocale.Any(assetName => assetName.IsEquivalentTo(name)))
        {
            asset = null;
        }
    }

    private T GetOrLoadAsset()
    {
        return asset is not null ? asset : gameContent.Load<T>(name);
    }
}
