using StardewUI.Framework.Dom;

namespace StardewUI.Framework.Binding;

/// <summary>
/// Default in-game view engine.
/// </summary>
/// <param name="viewFactory">Factory for creating views, based on their tag names.</param>
/// <param name="viewBinder">Binding service used to create <see cref="IViewBinding"/> instances that detect changes to
/// data or assets and propagate them to the bound <see cref="IView"/>.</param>
public class ViewNodeFactory(IViewFactory viewFactory, IViewBinder viewBinder) : IViewNodeFactory
{
    public IViewNode CreateNode(SNode node)
    {
        var childNodes = node.ChildNodes.Select(CreateNode);
        return new ViewNode(viewFactory, viewBinder, node.Element, childNodes);
    }
}
