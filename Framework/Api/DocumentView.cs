using StardewUI.Framework.Binding;
using StardewUI.Framework.Dom;

namespace StardewUI.Framework.Api;

/// <summary>
/// A view based on a <see cref="Document"/>.
/// </summary>
/// <remarks>
/// This view type mainly exists as glue for the API, to be used in a <see cref="ViewMenu{T}"/>.
/// </remarks>
/// <param name="viewNodeFactory">Factory for creating and binding <see cref="IViewNode"/>s.</param>
/// <param name="document">The StarML document describing the view.</param>
/// <param name="context">Data to be bound to the view.</param>
internal class DocumentView(IViewNodeFactory viewNodeFactory, Document document, object? context) : WrapperView
{
    // Initialized in CreateView
    private IViewNode tree = null!;

    protected override IView CreateView()
    {
        tree = viewNodeFactory.CreateNode(document.Root);
        tree.Context = context;
        tree.Update();
        return tree.View!;
    }

    public override void OnUpdate(TimeSpan elapsed)
    {
        base.OnUpdate(elapsed);
        tree.Update();
    }
}
