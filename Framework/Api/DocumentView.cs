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
/// <param name="context">Context containing data to be bound to the view.</param>
internal class DocumentView(IViewNodeFactory viewNodeFactory, Document document, BindingContext? context) : WrapperView
{
    // Initialized in CreateView
    private IViewNode tree = null!;

    protected override IView CreateView()
    {
        tree = viewNodeFactory.CreateNode(document.Root);
        tree.Context = context;
        tree.Update();
        return tree.Views.Count switch
        {
            0 => throw new BindingException(
                "No root view detected. Either the document is empty, or has a structural attribute applied to the "
                    + "root view causing it to be excluded."
            ),
            1 => tree.Views[0],
            _ => throw new BindingException(
                $"Multiple ({tree.Views.Count}) root views detected. The root of a document must resolve to exactly "
                    + "one view (no *repeat or other multiple-emitting elements)."
            ),
        };
    }

    public override void OnUpdate(TimeSpan elapsed)
    {
        base.OnUpdate(elapsed);
        tree.Update();
    }
}
