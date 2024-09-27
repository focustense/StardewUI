using StardewUI.Framework.Binding;
using StardewUI.Framework.Dom;

namespace StardewUI.Framework.Api;

/// <summary>
///
/// </summary>
/// <param name="viewNodeFactory">Factory for creating and binding <see cref="IViewNode"/>s.</param>
/// <param name="document">The StarML document describing the view.</param>
/// <param name="context">Data to be bound to the view.</param>
internal class DocumentViewMenu(IViewNodeFactory viewNodeFactory, Document document, object? context)
    : ViewMenu<DocumentView>
{
    protected override DocumentView CreateView()
    {
        return new(viewNodeFactory, document, context);
    }
}
