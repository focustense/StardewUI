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
                : throw new InvalidOperationException("Cannot access a failed or expired asset.");

        public bool IsValid => entry.TryGetTarget(out var target) && target.IsValid;
    }

    // What we physically store in the cache. Mutable, so that we can update the IsExpired flag.
    class InternalEntry(object? asset, bool initiallyValid = true)
    {
        public object? Asset { get; set; } = asset;

        public bool IsValid { get; private set; } = initiallyValid;

        public void Invalidate()
        {
            if (Asset is null)
            {
                return;
            }
            Asset = default;
            IsValid = false;
        }
    }

    private readonly BackoffTracker<string> backoff = new(BackoffRule.Default);
    private readonly IGameContentHelper content;
    private readonly Dictionary<string, InternalEntry> entries = [];
    private readonly IMonitor monitor;

    /// <summary>
    /// Initializes a new instance of <see cref="AssetCache"/>.
    /// </summary>
    /// <param name="content">SMAPI content helper, used to load the assets.</param>
    /// <param name="events">SMAPI content events, used to detect invalidation.</param>
    /// <param name="monitor">Mod monitor for logging changes and errors.</param>
    public AssetCache(IGameContentHelper content, IContentEvents events, IMonitor monitor)
    {
        this.content = content;
        this.monitor = monitor;
        events.AssetsInvalidated += Content_AssetsInvalidated;
    }

    /// <inheritdoc />
    public IAssetCacheEntry<T> Get<T>(string name)
        where T : notnull
    {
        if (!entries.TryGetValue(name, out var entry) || !entry.IsValid)
        {
            var isValid = TryLoad<T>(name, out var asset);
            entry = new(asset, isValid);
            entries[name] = entry;
        }
        return new ExternalEntry<T>(new(entry));
    }

    /// <summary>
    /// Handles reocurring game updates. Used to update backoff and allow retrying of failed assets.
    /// </summary>
    /// <param name="elapsed">Time elapsed since last game tick.</param>
    internal void Update(TimeSpan elapsed)
    {
        backoff.Tick(elapsed);
    }

    private void Content_AssetsInvalidated(object? sender, AssetsInvalidatedEventArgs e)
    {
        foreach (var name in e.NamesWithoutLocale)
        {
            if (entries.TryGetValue(name.Name, out var entry))
            {
                entry.Invalidate();
                entries.Remove(name.Name);
            }
        }
    }

    private bool TryLoad<T>(string name, out T? result)
        where T : notnull
    {
        try
        {
            bool success = backoff.TryRun(name, () => content.Load<T>(name), out result);
            if (success)
            {
                monitor.Log($"Asset loaded: {name}", LogLevel.Debug);
            }
            return success;
        }
        catch (Exception ex)
        {
            monitor.Log($"Failed loading asset '{name}': {ex}", LogLevel.Error);
            result = default;
            return false;
        }
    }
}
