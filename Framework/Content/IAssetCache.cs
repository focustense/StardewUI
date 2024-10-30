using StardewUI.Framework.Sources;

namespace StardewUI.Framework.Content;

/// <summary>
/// Cache used for asset-based view bindings.
/// </summary>
/// <remarks>
/// Similar to SMAPI's content helpers, but instead of providing just the current asset at the time of the request,
/// returns entry objects with an expired flag for effective (and performant) use in <see cref="IValueSource.Update"/>.
/// </remarks>
public interface IAssetCache
{
    /// <summary>
    /// Retrieves the current entry for a given asset name.
    /// </summary>
    /// <remarks>
    /// If the asset was invalidated by SMAPI and has not yet been reloaded, then this will trigger a reload.
    /// </remarks>
    /// <typeparam name="T">The asset type.</typeparam>
    /// <param name="name">Name of the asset.</param>
    /// <returns>A cache entry object that contains the most current asset data, and an expired flag to detect if the
    /// asset is no longer valid in the future.</returns>
    IAssetCacheEntry<T> Get<T>(string name)
        where T : notnull;
}

/// <summary>
/// Entry retrieved from an <see cref="IAssetCache"/>.
/// </summary>
/// <typeparam name="T">Type of cached asset.</typeparam>
public interface IAssetCacheEntry<T>
{
    /// <summary>
    /// The cached asset.
    /// </summary>
    T Asset { get; }

    /// <summary>
    /// Whether or not the <see cref="Asset"/> is valid and can be accessed.
    /// </summary>
    /// <remarks>
    /// Invalid assets either failed to load or have been invalidated at the source and may be disposed (if
    /// <see cref="IDisposable"/>) or otherwise unusable. Consumers of the cache entry <b>must not</b> attempt to read
    /// or use the <see cref="Asset"/> property of an invalid asset.
    /// </remarks>
    bool IsValid { get; }
}
