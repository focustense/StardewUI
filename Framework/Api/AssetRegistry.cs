using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewUI.Framework.Content;
using StardewUI.Framework.Dom;
using StardewUI.Graphics;

namespace StardewUI.Framework.Api;

/// <summary>
/// Helper for registering UI assets and collections of assets.
/// </summary>
/// <remarks>
/// Methods here are generally wrapped by the <see cref="IViewEngine"/>.
/// </remarks>
internal class AssetRegistry : ISourceResolver
{
    record DirectoryMapping(string AssetPrefix, string ModDirectory);

    // Sprites maintain a reference to their Texture2D, so they should never be kept alive in their record form.
    // Instead, we can cache everything else about them (just their "data") and recreate them for any given Texture2D,
    // which is essentially how helpers like UiSprites behave.
    record SpriteCacheEntry(string TextureAssetName, Func<Texture2D, Sprite> Selector);

    private readonly IModHelper helper;
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

    private ConcurrentBag<string> assetsToInvalidate = [];
    private ConcurrentDictionary<string, string> changedFiles = [];
    private readonly Timer fileRetryTimer;
    private FileSystemWatcher? hotReloadWatcher;
    private FileSystemWatcher? sourceSyncWatcher;

    public AssetRegistry(IModHelper helper)
    {
        this.helper = helper;

        fileRetryTimer = new(FileRetryTimerCallback);

        var contentEvents = helper.Events.Content;
        contentEvents.AssetRequested += Content_AssetRequested;
        contentEvents.AssetsInvalidated += Content_AssetsInvalidated;
    }

    /// <summary></summary>
    /// <param name="callerFilePath"></param>
    /// <param name="sourceDirectory"></param>
    /// <returns></returns>
    private static bool TryFindSourceDirectory(string callerFilePath, [NotNullWhen(true)] out string? sourceDirectory)
    {
        sourceDirectory = null;
        DirectoryInfo? dirInfo = Directory.GetParent(callerFilePath);
        while (dirInfo != null)
        {
            foreach (FileInfo file in dirInfo.GetFiles())
            {
                if (file.Extension == ".csproj")
                {
                    sourceDirectory = dirInfo.FullName;
                    return true;
                }
            }
            dirInfo = dirInfo.Parent;
        }
        return false;
    }

    /// <summary>
    /// Starts monitoring the file system for changes to any of the mod's assets.
    /// </summary>
    /// <param name="sourceDirectory">Source directory to watch and sync changes from.</param>
    public void EnableHotReloading(string? sourceDirectory = null)
    {
        if (hotReloadWatcher is not null)
        {
            return;
        }
        helper.Events.GameLoop.UpdateTicked += GameLoop_UpdateTicked;
        hotReloadWatcher = new FileSystemWatcher(helper.DirectoryPath) { IncludeSubdirectories = true };
        hotReloadWatcher.Changed += HotReloadWatcher_Changed;
        hotReloadWatcher.EnableRaisingEvents = true;
        fileRetryTimer.Change(TimeSpan.Zero, TimeSpan.FromMilliseconds(100));
        Logger.Log($"[Hot Reload] Watching {helper.DirectoryPath}...", LogLevel.Debug);
        if (sourceSyncWatcher is not null || sourceDirectory == null || !Directory.Exists(sourceDirectory))
        {
            return;
        }
        sourceSyncWatcher = new FileSystemWatcher(sourceDirectory) { IncludeSubdirectories = true };
        sourceSyncWatcher.Changed += SourceSyncWatcher_Changed;
        sourceSyncWatcher.EnableRaisingEvents = true;
        Logger.Log($"[Hot Reload] Will sync changes from {sourceDirectory}...", LogLevel.Debug);
    }

    /// <inheritdoc cref="IViewEngine.RegisterSprites(string, string)" />
    public void RegisterSprites(string assetPrefix, string modDirectory)
    {
        if (!assetPrefix.EndsWith('/'))
        {
            assetPrefix += '/';
        }
        spriteDirectories.Add(new(assetPrefix, PathUtilities.NormalizePath(modDirectory)));
    }

    /// <inheritdoc cref="IViewEngine.RegisterViews(string, string)" />
    public void RegisterViews(string assetPrefix, string modDirectory)
    {
        if (!assetPrefix.EndsWith('/'))
        {
            assetPrefix += '/';
        }
        viewDirectories.Add(new(assetPrefix, PathUtilities.NormalizePath(modDirectory)));
    }

    /// <inheritdoc cref="IViewEngine.RegisterViews(string, string)" />
    public bool TryGetProvidingModId(Document document, [MaybeNullWhen(false)] out string modId)
    {
        var filePath = SourceResolver.GetDocumentSourcePath(document);
        var directoryPath = PathUtilities.NormalizePath(Path.GetDirectoryName(filePath));
        modId =
            !string.IsNullOrEmpty(directoryPath)
            && viewDirectories.Any(dir =>
                directoryPath.StartsWith(
                    Path.Combine(helper.DirectoryPath, dir.ModDirectory),
                    StringComparison.OrdinalIgnoreCase
                )
            )
                ? modId = helper.Translation.ModID
                : null;
        return modId is not null;
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
        var isMainThread = Game1.IsOnMainThread();
        foreach (var assetName in e.Names)
        {
            var key = assetName.Name;
            if (key.EndsWith("@data"))
            {
                key = key[..^5];
            }
            if (textureCache.Remove(key))
            {
                Logger.Log($"Evicted texture from cache: {assetName.Name}", LogLevel.Debug);
            }
            if (spriteCache.Remove(key))
            {
                Logger.Log($"Evicted sprite data from cache: {assetName.Name}", LogLevel.Debug);
            }
            if (spriteSheetCache.Remove(key))
            {
                Logger.Log($"Evicted sprite sheet data from cache: {assetName.Name}", LogLevel.Debug);
                // Invalidating an entire sprite sheet means we should also invalidate all of its sprites.
                // Here we make the same assumptions as TryLoadSprite, namely that sprite assets are of the form
                // "Path/To/Spritesheet:SpriteName".
                var dependentKeys = spriteCache.Keys.Where(otherKey =>
                    otherKey.StartsWith(key, StringComparison.OrdinalIgnoreCase)
                );
                foreach (var dependentKey in dependentKeys)
                {
                    if (isMainThread)
                    {
                        helper.GameContent.InvalidateCache(dependentKey);
                    }
                    else
                    {
                        assetsToInvalidate.Add(dependentKey);
                    }
                }
            }
        }
    }

    private void FileRetryTimerCallback(object? _)
    {
        var changedFiles = this.changedFiles;
        this.changedFiles = [];
        foreach (var (physicalPath, assetName) in changedFiles)
        {
            if (!TryInvalidateFile(physicalPath, assetName))
            {
                this.changedFiles.TryAdd(physicalPath, assetName);
            }
        }
    }

    private void HotReloadWatcher_Changed(object sender, FileSystemEventArgs e)
    {
        if (!TryInvalidateFile(e.FullPath, e.Name ?? ""))
        {
            changedFiles.TryAdd(e.FullPath, e.Name!);
        }
    }

    private void SyncFile(IReadOnlyList<DirectoryMapping> directories, string fullPath, string relativePath)
    {
        string deployedPath = PathUtilities.NormalizePath(Path.Join(hotReloadWatcher!.Path, relativePath));
        foreach (var mapping in directories)
        {
            if (relativePath.StartsWith(mapping.ModDirectory))
            {
                try
                {
                    Logger.Log($"Sync {relativePath} ({mapping.AssetPrefix})", LogLevel.Debug);
                    File.Copy(fullPath, deployedPath, true);
                }
                catch (Exception ex)
                {
                    Logger.Log($"Failed to sync changed file '{fullPath}' to '{fullPath}': {ex}", LogLevel.Warn);
                    Logger.Log($"Stop watching {sourceSyncWatcher!.Path}", LogLevel.Debug);
                    sourceSyncWatcher.Changed -= SourceSyncWatcher_Changed;
                }
            }
        }
        return;
    }

    /// <summary>Copy file from source to deployed, if a file of same relative path exists on target</summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SourceSyncWatcher_Changed(object sender, FileSystemEventArgs e)
    {
        var relativePath = Path.GetRelativePath(sourceSyncWatcher!.Path, e.FullPath);
        switch (Path.GetExtension(e.Name))
        {
            case ".sml":
                SyncFile(viewDirectories, e.FullPath, relativePath);
                break;
            case ".json":
                SyncFile(spriteDirectories, e.FullPath, relativePath);
                break;
            case ".png":
                SyncFile(spriteDirectories, e.FullPath, relativePath);
                break;
        }
    }

    private void GameLoop_UpdateTicked(object? sender, UpdateTickedEventArgs e)
    {
        MainThreadUpdate();
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
                data = helper.GameContent.Load<SpriteSheetData>(assetName + "@data");
            }
            catch (Exception ex)
            {
                Logger.Log($"Error loading sprite sheet data for '{assetName}': {ex}", LogLevel.Error);
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
            texture = helper.GameContent.Load<Texture2D>(assetName);
        }
        catch (Exception ex)
        {
            Logger.Log($"Error loading texture for '{assetName}': {ex}", LogLevel.Error);
            throw;
        }
        textureCache[assetName] = new(texture);
        return texture;
    }

    private void InvalidateFile(
        IReadOnlyList<DirectoryMapping> directories,
        string relativePath,
        string assetSuffix = ""
    )
    {
        Logger.Log($"File '{relativePath}' was changed; invalidating asset.", LogLevel.Debug);
        var isMainThread = Game1.IsOnMainThread();
        relativePath = PathUtilities.NormalizePath(Path.ChangeExtension(relativePath, null));
        foreach (var (assetPrefix, modDirectory) in directories)
        {
            if (relativePath.StartsWith(modDirectory))
            {
                // Mod directory path won't have the trailing path separator, so we need to add 1 to length.
                var assetName = assetPrefix + relativePath[(modDirectory.Length + 1)..] + assetSuffix;
                if (isMainThread)
                {
                    helper.GameContent.InvalidateCache(assetName);
                }
                else
                {
                    assetsToInvalidate.Add(assetName);
                }
                break;
            }
        }
    }

    // Performs scheduled tasks that must be run on the main thread.
    // In particular, cache invalidation for objects such as textures is dangerous to run in the background.
    private void MainThreadUpdate()
    {
        var assetsToInvalidate = this.assetsToInvalidate;
        this.assetsToInvalidate = [];
        while (assetsToInvalidate.TryTake(out var assetName))
        {
            helper.GameContent.InvalidateCache(assetName);
        }
    }

    private bool TryInvalidateFile(string physicalPath, string assetName)
    {
        // Tweak: Don't invalidate the asset until we know we can read from it, otherwise we can get an access exception
        // when trying to load it on the next frame.
        //
        // This handles the typical case when FSW might fire a couple of events, the first one or two while the file is
        // still locked; it might behave badly for pathological cases like copying a gigabyte-long file over hundreds of
        // frames.
        //
        // Also, it helps to do this on the FSW thread or a background thread (e.g. timer) instead of checking it in the
        // Update loop, as this way we won't block the update loop. The subsequent load always happens on main thread,
        // but we can do the lock checking and invalidation scheduling in the background.
        try
        {
            using var _ = File.OpenRead(physicalPath);
        }
        catch (IOException)
        {
            return false;
        }
        switch (Path.GetExtension(assetName))
        {
            case ".sml":
                InvalidateFile(viewDirectories, assetName);
                break;
            case ".json":
                InvalidateFile(spriteDirectories, assetName, "@data");
                break;
            case ".png":
                InvalidateFile(spriteDirectories, assetName);
                break;
        }
        return true;
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
