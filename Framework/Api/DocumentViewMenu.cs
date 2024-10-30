using StardewUI.Framework.Binding;
using StardewUI.Framework.Dom;
using StardewUI.Framework.Sources;

namespace StardewUI.Framework.Api;

/// <summary>
///
/// </summary>
/// <param name="viewNodeFactory">Factory for creating and binding <see cref="IViewNode"/>s.</param>
/// <param name="documentSource">Source providing the StarML document describing the view.</param>
/// <param name="data">Data to be bound to the view.</param>
internal class DocumentViewMenu(IViewNodeFactory viewNodeFactory, IValueSource<Document> documentSource, object? data)
    : ViewMenu<DocumentView>
{
    protected override DocumentView CreateView()
    {
        var context = data is not null ? BindingContext.Create(data) : null;
        return new(viewNodeFactory, documentSource) { Context = context };
    }
}
