using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using SkiaSharp;
using StardewUI.Data;
using StardewUI.Framework.Dom;

namespace StardewUI.Framework.Api;

/// <summary>
/// Preloads assets for an <see cref="AssetRegistry"/> so that they can be immediately or very quickly available upon
/// the first request.
/// </summary>
/// <param name="basePath">Base directory (absolute path) of the associated mod.</param>
internal class AssetPreloader(string basePath)
{
    private record RawTextureData(int Width, int Height, Color[] Data);

    private readonly Dictionary<string, SpriteSheetData> spriteSheets = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, RawTextureData> textures = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, Document> views = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Forgets a preloaded asset. Used in hot reload scenarios when an asset is invalidated.
    /// </summary>
    /// <param name="assetName">The asset name.</param>
    public void Evict(string assetName)
    {
        spriteSheets.Remove(assetName);
        textures.Remove(assetName);
        views.Remove(assetName);
    }

    /// <summary>
    /// Starts preloading all assets in the configured directories.
    /// </summary>
    /// <param name="viewDirectories">Directories containing views, relative to the calling mod.</param>
    /// <param name="spriteDirectories">Directories containing sprite textures and data, relative to the calling
    /// mod.</param>
    /// <returns>A task that completes when all assets have been preloaded.</returns>
    public async Task Preload(
        IEnumerable<DirectoryMapping> viewDirectories,
        IEnumerable<DirectoryMapping> spriteDirectories
    )
    {
        // In theory it's possible to use a separate async task for every individual file to be read from disk in order
        // to maximize concurrency of I/O and CPU tasks. In practice, this type of thing is more likely to exhaust the
        // thread pool and/or I/O ports than it is to deliver any real speedup.
        //
        // However, a discrete number of parallel tasks can still help slightly, and these map pretty well to the
        // different asset types (views, sprites and custom data) when using one task each.
        await Task.WhenAll(
            Task.Run(() => PreloadViews(viewDirectories)),
            Task.Run(() => PreloadSprites(spriteDirectories))
        );
    }

    /// <summary>
    /// Gets a preloaded sprite sheet configuration, and removes it from the preload cache if found.
    /// </summary>
    /// <param name="assetName">The asset name.</param>
    /// <returns>The preloaded sprite sheet data, or <c>null</c> if no such asset was found during preload.</returns>
    public SpriteSheetData? GetAndRemoveSpriteSheetData(string assetName)
    {
        if (spriteSheets.TryGetValue(assetName, out var spriteSheetData))
        {
            spriteSheets.Remove(assetName);
            return spriteSheetData;
        }
        return null;
    }

    /// <summary>
    /// Gets a preloaded texture, and removes its data from the preload cache if found.
    /// </summary>
    /// <remarks>
    /// Textures must always be created on the game's main thread, so retrieving a preloaded texture is not free.
    /// However, it is usually cheaper than loading it directly through SMAPI or XNA's pipeline.
    /// </remarks>
    /// <param name="assetName">The asset name.</param>
    /// <returns>A <see cref="Texture2D"/> with the preloaded texture data, or <c>null</c> if preloaded texture data is
    /// available.</returns>
    public Texture2D? GetAndRemoveTexture(string assetName)
    {
        if (textures.TryGetValue(assetName, out var raw))
        {
            textures.Remove(assetName);
            var texture = new Texture2D(Game1.graphics.GraphicsDevice, raw.Width, raw.Height) { Name = assetName };
            texture.SetData(raw.Data);
            return texture;
        }
        return null;
    }

    /// <summary>
    /// Gets a preloaded view, and removes it from the preload cache if found.
    /// </summary>
    /// <param name="assetName">The asset name.</param>
    /// <returns>The preloaded view document, or <c>null</c> if no such asset was found during preload.</returns>
    public Document? GetAndRemoveView(string assetName)
    {
        if (views.TryGetValue(assetName, out var document))
        {
            views.Remove(assetName);
            return document;
        }
        return null;
    }

    IEnumerable<string> EnumerateFiles(string relativeDirectory)
    {
        var directory = Path.Combine(basePath, relativeDirectory);
        return Directory.EnumerateFiles(
            directory,
            "*.*",
            new EnumerationOptions() { RecurseSubdirectories = true, MaxRecursionDepth = 10 }
        );
    }

    private async Task PreloadSprites(IEnumerable<DirectoryMapping> spriteDirectories)
    {
        foreach (var spriteDirectory in spriteDirectories)
        {
            var spriteFiles = EnumerateFiles(spriteDirectory.ModDirectory);
            foreach (var spriteFile in spriteFiles)
            {
                var ext = Path.GetExtension(spriteFile);
                var isPng = ext.Equals(".png", StringComparison.OrdinalIgnoreCase);
                var isJson = !isPng && ext.Equals(".json", StringComparison.OrdinalIgnoreCase);
                if (!isPng && !isJson)
                {
                    return;
                }
                var assetName = ResolveAssetName(spriteDirectory, spriteFile);
                if (isJson)
                {
                    assetName += "@data";
                }
                await (
                    isPng ? PreloadTextureData(assetName, spriteFile) : PreloadSpriteSheetData(assetName, spriteFile)
                );
            }
        }
    }

    private async Task PreloadSpriteSheetData(string assetName, string filePath)
    {
        var json = await File.ReadAllTextAsync(filePath);
        try
        {
            var data = JsonConvert.DeserializeObject<SpriteSheetData>(json);
            if (data is not null)
            {
                spriteSheets.Add(assetName, data);
                Logger.Log($"Preloaded sprite sheet data for '{assetName}' from {filePath}.", LogLevel.Info);
            }
            else
            {
                Logger.Log($"Skipped preloading empty sprite sheet at {filePath}.", LogLevel.Warn);
            }
        }
        catch (JsonException ex)
        {
            Logger.Log($"Skipped preloading {filePath} due to a JSON error: {ex}", LogLevel.Warn);
        }
    }

    private async Task PreloadTextureData(string assetName, string filePath)
    {
        var fileData = await File.ReadAllBytesAsync(filePath);
        SKPMColor[] rawPixels;
        int width;
        int height;
        {
            using var bitmap = SKBitmap.Decode(fileData);
            if (bitmap is null)
            {
                Logger.Log(
                    $"Skipped preloading {filePath} because it does not appear to be a valid PNG image.",
                    LogLevel.Warn
                );
                return;
            }
            rawPixels = SKPMColor.PreMultiply(bitmap.Pixels);
            width = bitmap.Width;
            height = bitmap.Height;
        }
        var pixels = GC.AllocateUninitializedArray<Color>(rawPixels.Length);
        for (int i = 0; i < pixels.Length; i++)
        {
            var rawPixel = rawPixels[i];
            pixels[i] =
                rawPixel.Alpha == 0
                    ? Color.Transparent
                    : new Color(r: rawPixel.Red, g: rawPixel.Green, b: rawPixel.Blue, alpha: rawPixel.Alpha);
        }
        textures.Add(assetName, new(width, height, pixels));
        Logger.Log($"Preloaded texture data for '{assetName}' from {filePath}.", LogLevel.Info);
    }

    private async Task PreloadView(string assetName, string filePath)
    {
        var ext = Path.GetExtension(filePath);
        if (
            !ext.EndsWith(".sml", StringComparison.OrdinalIgnoreCase)
            && !ext.EndsWith(".starml", StringComparison.OrdinalIgnoreCase)
        )
        {
            return;
        }
        var document = await DocumentLoader.TryLoadFromFileAsync(new(filePath));
        if (document is null)
        {
            Logger.Log($"Skipped preloading {filePath} due to load errors.", LogLevel.Warn);
            return;
        }
        views.Add(assetName, document);
        Logger.Log($"Preloaded view '{assetName}' from {filePath}.", LogLevel.Info);
    }

    private async Task PreloadViews(IEnumerable<DirectoryMapping> viewDirectories)
    {
        foreach (var viewDirectory in viewDirectories)
        {
            var viewFiles = EnumerateFiles(viewDirectory.ModDirectory);
            foreach (var viewFile in viewFiles)
            {
                var assetName = ResolveAssetName(viewDirectory, viewFile);
                await PreloadView(assetName, viewFile);
            }
        }
    }

    private string ResolveAssetName(DirectoryMapping directoryMapping, string absoluteFilePath)
    {
        var relativePath = Path.GetRelativePath(
            Path.Combine(basePath, directoryMapping.ModDirectory),
            absoluteFilePath
        );
        return directoryMapping.AssetPrefix + Path.ChangeExtension(relativePath, null);
    }
}
