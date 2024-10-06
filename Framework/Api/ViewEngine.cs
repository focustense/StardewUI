using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI.Events;
using StardewUI.Framework.Binding;
using StardewUI.Framework.Dom;
using StardewValley.Menus;

namespace StardewUI.Framework.Api;

/// <summary>
/// Implementation for the public <see cref="IViewEngine"/> API.
/// </summary>
/// <param name="contentHelper">SMAPI content helper; used for referencing named <see cref="Document"/> assets.</param>
/// <param name="viewNodeFactory">Factory for creating and binding <see cref="IViewNode"/>s.</param>
/// <param name="contentEvents">SMAPI content events; used to register views and other assets.</param>
/// <param name="monitor">Monitor for logging events.</param>
public class ViewEngine(
    IGameContentHelper contentHelper,
    IViewNodeFactory viewNodeFactory,
    IContentEvents contentEvents,
    IMonitor monitor
) : IViewEngine
{
    private readonly AssetRegistry assetRegistry = new(contentHelper, contentEvents, monitor);

    public IClickableMenu CreateMenuFromAsset(string assetName, object? context = null)
    {
        var document = contentHelper.Load<Document>(assetName);
        return new DocumentViewMenu(viewNodeFactory, document, context);
    }

    public IClickableMenu CreateMenuFromMarkup(string markup, object? context = null)
    {
        var document = Document.Parse(markup);
        return new DocumentViewMenu(viewNodeFactory, document, context);
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
