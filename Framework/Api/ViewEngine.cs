using StardewUI.Framework.Binding;
using StardewUI.Framework.Dom;
using StardewValley.Menus;

namespace StardewUI.Framework.Api;

/// <summary>
/// Implementation for the public <see cref="IViewEngine"/> API.
/// </summary>
/// <param name="content">SMAPI content helper; used for referencing named <see cref="Document"/> assets.</param>
/// <param name="viewNodeFactory">Factory for creating and binding <see cref="IViewNode"/>s.</param>
public class ViewEngine(IGameContentHelper content, IViewNodeFactory viewNodeFactory) : IViewEngine
{
    public IClickableMenu CreateMenuFromAsset(string assetName, object? context = null)
    {
        var document = content.Load<Document>(assetName);
        return new DocumentViewMenu(viewNodeFactory, document, context);
    }

    public IClickableMenu CreateMenuFromMarkup(string markup, object? context = null)
    {
        var document = Document.Parse(markup);
        return new DocumentViewMenu(viewNodeFactory, document, context);
    }
}
