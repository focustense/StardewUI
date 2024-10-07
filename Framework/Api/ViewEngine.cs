using StardewUI.Framework.Binding;
using StardewUI.Framework.Content;
using StardewUI.Framework.Dom;
using StardewUI.Framework.Sources;
using StardewValley.Menus;

namespace StardewUI.Framework.Api;

/// <summary>
/// Implementation for the public <see cref="IViewEngine"/> API.
/// </summary>
/// <param name="assetCache">Cache for obtaining document assets. Used for asset-based views.</param>
/// <param name="helper">SMAPI mod helper for the API consumer mod (not for StardewUI).</param>
/// <param name="viewNodeFactory">Factory for creating and binding <see cref="IViewNode"/>s.</param>
/// <param name="monitor">Monitor for logging events.</param>
/// <param name="enableHotReloading">Whether to enable automatic hot reloading by watching the mod's physical files in
/// the mod directory.</param>
public class ViewEngine(IAssetCache assetCache, IModHelper helper, IViewNodeFactory viewNodeFactory, IMonitor monitor)
    : IViewEngine
{
    private readonly AssetRegistry assetRegistry = new(helper, monitor);

    public IClickableMenu CreateMenuFromAsset(string assetName, object? context = null)
    {
        var documentSource = new AssetValueSource<Document>(assetCache, assetName);
        return new DocumentViewMenu(viewNodeFactory, documentSource, context);
    }

    public IClickableMenu CreateMenuFromMarkup(string markup, object? context = null)
    {
        var document = Document.Parse(markup);
        var documentSource = new ConstantValueSource<Document>(document);
        return new DocumentViewMenu(viewNodeFactory, documentSource, context);
    }

    public void EnableHotReloading()
    {
        assetRegistry.EnableHotReloading();
    }

    public void RegisterSprites(string assetPrefix, string modDirectory)
    {
        assetRegistry.RegisterSprites(assetPrefix, modDirectory);
    }

    public void RegisterViews(string assetPrefix, string modDirectory)
    {
        assetRegistry.RegisterViews(assetPrefix, modDirectory);
    }
}
