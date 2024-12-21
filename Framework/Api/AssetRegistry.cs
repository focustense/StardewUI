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

    // Sliding timeout per file when hot reloading. Helps prevent spam due to repeated writes on some OSes.
    private static readonly TimeSpan HotReloadDebounceDelay = TimeSpan.FromMilliseconds(50);

    // Error codes that are considered transient, likely to go away after a retry.
    private static readonly uint[] RetryableHResults =
    [
        0x80070020, // ERROR_SHARING_VIOLATION
        0x80070021, // ERROR_LOCK_VIOLATION
    ];

    // The game loads these on startup, and its content manager WILL try to reload them again from disk when requested
    // by asset path directly, unless we intercept the request and redirect it to the static field.
    private static readonly Dictionary<string, Func<Texture2D>> StaticTextures = new(StringComparer.OrdinalIgnoreCase)
    {
        { "LooseSprites\\Birds", () => Game1.birdsSpriteSheet },
        { "LooseSprites\\Lighting\\greenLight", () => Game1.cauldronLight },
        { "LooseSprites\\Lighting\\indoorWindowLight", () => Game1.indoorWindowLight },
        { "LooseSprites\\Lighting\\Lantern", () => Game1.lantern },
        { "LooseSprites\\Lighting\\sconceLight", () => Game1.sconceLight },
        { "LooseSprites\\Lighting\\windowLight", () => Game1.windowLight },
        { "LooseSprites\\shadow", () => Game1.shadowTexture },
        { "Maps\\MenuTiles", () => Game1.menuTexture },
        { "Maps\\MenuTilesUncolored", () => Game1.uncoloredMenuTexture },
        { "TileSheets\\BuffsIcon", () => Game1.buffsIcons },
        { "TileSheets\\emotes", () => Game1.emoteSpriteSheet },
        { "TileSheets\\Objects2", () => Game1.objectSpriteSheet_2 },
        { "TileSheets\\rain", () => Game1.rainTexture },
        { "TileSheets\\weapons", () => Tool.weaponsTexture },
        { Game1.animationsName, () => Game1.animations },
        { Game1.bigCraftableSpriteSheetName, () => Game1.bigCraftableSpriteSheet },
        { Game1.bobbersTextureName, () => Game1.bobbersTexture },
        { Game1.concessionsSpriteSheetName, () => Game1.concessionsSpriteSheet },
        { Game1.cropSpriteSheetName, () => Game1.cropSpriteSheet },
        { Game1.debrisSpriteSheetName, () => Game1.debrisSpriteSheet },
        { Game1.giftboxName, () => Game1.giftboxTexture },
        { Game1.mouseCursors1_6Name, () => Game1.mouseCursors_1_6 },
        { Game1.mouseCursors2Name, () => Game1.mouseCursors2 },
        { Game1.mouseCursorsName, () => Game1.mouseCursors },
        { Game1.objectSpriteSheetName, () => Game1.objectSpriteSheet },
        { Game1.toolSpriteSheetName, () => Game1.toolSpriteSheet },
    };

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
    private ConcurrentDictionary<string, string> changedModFiles = [];
    private ConcurrentDictionary<string, string> changedSourceFiles = [];
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
        hotReloadWatcher.Changed += Debounce(HotReloadWatcher_Changed, HotReloadDebounceDelay);
        hotReloadWatcher.EnableRaisingEvents = true;
        fileRetryTimer.Change(TimeSpan.Zero, TimeSpan.FromMilliseconds(100));
        Logger.Log($"[Hot Reload] Watching {helper.DirectoryPath}...", LogLevel.Debug);

        if (
            sourceSyncWatcher is not null
            || string.IsNullOrEmpty(sourceDirectory)
            || !Directory.Exists(sourceDirectory)
        )
        {
            return;
        }
        sourceSyncWatcher = new FileSystemWatcher(sourceDirectory) { IncludeSubdirectories = true };
        sourceSyncWatcher.Changed += Debounce(SourceSyncWatcher_Changed, HotReloadDebounceDelay);
        sourceSyncWatcher.EnableRaisingEvents = true;
        Logger.Log($"[Hot Reload] Syncing changes from {sourceDirectory} to {helper.DirectoryPath}...", LogLevel.Debug);
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

    private FileSystemEventHandler Debounce(FileSystemEventHandler handler, TimeSpan delay)
    {
        var pending = new ConcurrentDictionary<string, Timer>();
        return (sender, e) =>
        {
            if (pending.TryGetValue(e.FullPath, out var timer))
            {
                timer.Change(delay, Timeout.InfiniteTimeSpan);
            }
            else
            {
                pending.TryAdd(e.FullPath, new(TimerCallback, e.FullPath, delay, Timeout.InfiniteTimeSpan));
            }

            void TimerCallback(object? arg)
            {
                var fullPath = (string)arg!;
                if (pending.TryRemove(fullPath, out var doneTimer))
                {
                    doneTimer.Dispose();
                    handler(sender, e);
                }
            }
        };
    }

    private void FileRetryTimerCallback(object? _)
    {
        var changedModFiles = this.changedModFiles;
        this.changedModFiles = [];
        foreach (var (physicalPath, assetName) in changedModFiles)
        {
            if (!TryInvalidateFile(physicalPath, assetName))
            {
                this.changedModFiles.TryAdd(physicalPath, assetName);
            }
        }

        var changedSourceFiles = this.changedSourceFiles;
        this.changedSourceFiles = [];
        foreach (var (sourcePath, destinationPath) in changedSourceFiles)
        {
            SyncFile(sourcePath, destinationPath); // Will add back to changedSourceFiles if failed and retryable
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
        using var _ = Trace.Begin(this, nameof(GetSpriteSheetData));
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
        using var _ = Trace.Begin(this, nameof(GetTexture));
        using var _name = Trace.Begin("#" + assetName);
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
            texture = StaticTextures.TryGetValue(assetName.Replace('/', '\\'), out var staticTexture)
                ? staticTexture()
                : helper.GameContent.Load<Texture2D>(assetName);
        }
        catch (Exception ex)
        {
            Logger.Log($"Error loading texture for '{assetName}': {ex}", LogLevel.Error);
            throw;
        }
        textureCache[assetName] = new(texture);
        return texture;
    }

    private void HotReloadWatcher_Changed(object sender, FileSystemEventArgs e)
    {
        if (!TryInvalidateFile(e.FullPath, e.Name ?? ""))
        {
            changedModFiles.TryAdd(e.FullPath, e.Name!);
        }
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

    private static bool IsRetryable(Exception ex)
    {
        return ex is IOException io && RetryableHResults.Contains((uint)io.HResult);
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

    private void SourceSyncWatcher_Changed(object sender, FileSystemEventArgs e)
    {
        var relativePath = Path.GetRelativePath(sourceSyncWatcher!.Path, e.FullPath);
        switch (Path.GetExtension(e.Name))
        {
            case ".sml":
                SyncFile(viewDirectories, e.FullPath, relativePath);
                break;
            case ".json":
            case ".png":
                SyncFile(spriteDirectories, e.FullPath, relativePath);
                break;
        }
    }

    private void SyncFile(IReadOnlyList<DirectoryMapping> directories, string fullPath, string relativePath)
    {
        string deployedPath = PathUtilities.NormalizePath(Path.Combine(hotReloadWatcher!.Path, relativePath));
        foreach (var mapping in directories)
        {
            if (relativePath.StartsWith(mapping.ModDirectory))
            {
                Logger.Log(
                    $"File '{fullPath}' matches asset prefix '{mapping.AssetPrefix}'; syncing to {deployedPath}",
                    LogLevel.Debug
                );
                SyncFile(fullPath, deployedPath);
                return;
            }
        }
        Logger.Log(
            $"File '{fullPath}' does not match any registered asset prefix and will not be synchronized.",
            LogLevel.Info
        );
    }

    private void SyncFile(string sourcePath, string destinationPath)
    {
        try
        {
            File.Copy(sourcePath, destinationPath, true);
        }
        catch (Exception ex)
        {
            if (IsRetryable(ex))
            {
                changedSourceFiles[sourcePath] = destinationPath;
            }
            else
            {
                Logger.Log(
                    $"Unexpected error while syncing changed file '{sourcePath}' to '{destinationPath}': {ex}",
                    LogLevel.Warn
                );
                Logger.Log(
                    $"Source syncing from '{sourceSyncWatcher!.Path}' will be stopped to prevent a cascade.",
                    LogLevel.Debug
                );
                sourceSyncWatcher.Changed -= SourceSyncWatcher_Changed;
            }
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

    private bool TryLoadAsset<T>(
        IReadOnlyList<DirectoryMapping> directories,
        AssetRequestedEventArgs e,
        string extension,
        string? suffix = null
    )
        where T : notnull
    {
        using var _ = Trace.Begin(nameof(AssetRegistry), nameof(TryLoadAsset));
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
                if (!File.Exists(Path.Combine(helper.DirectoryPath, modPath)))
                {
                    return false;
                }
                e.LoadFromModFile<T>(modPath, AssetLoadPriority.Low);
                return true;
            }
        }
        return false;
    }

    private bool TryLoadSprite(AssetRequestedEventArgs e)
    {
        using var _ = Trace.Begin(this, nameof(TryLoadSprite));
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
