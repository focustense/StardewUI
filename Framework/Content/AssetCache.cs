using StardewModdingAPI.Events;

namespace StardewUI.Framework.Content;

/// <summary>
/// Standard in-game implementation of the asset cache based on SMAPI's helpers and events.
/// </summary>
public class AssetCache : IAssetCache
{
    // The actual instance of IAssetCacheEntry<T> that we supply to callers. Implemented as a wrapper around the
    // InternalEntry type to hide its inherent mutability and provide some safety checks.
    class ExternalEntry<T>(WeakReference<InternalEntry> entry) : IAssetCacheEntry<T>
    {
        public T Asset { get; } =
            entry.TryGetTarget(out var target)
                ? (T)target.Asset!
                : throw new InvalidOperationException("Cannot access an expired asset.");

        public bool IsExpired => !entry.TryGetTarget(out var target) || target.IsExpired;
    }

    // What we physically store in the cache. Mutable, so that we can update the IsExpired flag.
    class InternalEntry(object asset)
    {
        public object? Asset { get; set; } = asset;

        public bool IsExpired { get; private set; }

        public void Invalidate()
        {
            if (Asset is null)
            {
                return;
            }
            Asset = default;
            IsExpired = true;
        }
    }

    private readonly IGameContentHelper content;
    private readonly Dictionary<string, InternalEntry> entries = [];

    /// <summary>
    /// Initializes a new instance of <see cref="AssetCache"/>.
    /// </summary>
    /// <param name="content">SMAPI content helper, used to load the assets.</param>
    /// <param name="events">SMAPI content events, used to detect invalidation.</param>
    public AssetCache(IGameContentHelper content, IContentEvents events)
    {
        this.content = content;
        events.AssetsInvalidated += Content_AssetsInvalidated;
    }

    /// <inheritdoc />
    public IAssetCacheEntry<T> Get<T>(string name)
        where T : notnull
    {
        if (!entries.TryGetValue(name, out var entry))
        {
            var asset = content.Load<T>(name);
            entry = new(asset);
            entries.Add(name, entry);
        }
        else
        {
            entry.Asset ??= content.Load<T>(name);
        }
        return new ExternalEntry<T>(new(entry));
    }

    private void Content_AssetsInvalidated(object? sender, AssetsInvalidatedEventArgs e)
    {
        foreach (var name in e.NamesWithoutLocale)
        {
            if (entries.TryGetValue(name.BaseName, out var entry))
            {
                entry.Invalidate();
                entries.Remove(name.BaseName);
            }
        }
    }
}
