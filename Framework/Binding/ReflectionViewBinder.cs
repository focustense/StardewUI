using StardewUI.Framework.Descriptors;
using StardewUI.Framework.Dom;

namespace StardewUI.Framework.Binding;

/// <summary>
/// An <see cref="IViewBinder"/> implementation using reflected view descriptors.
/// </summary>
/// <param name="attributeBindingFactory">Factory for creating the <see cref="IAttributeBinding"/> instances used to
/// bind individual attributes of the view.</param>
public class ReflectionViewBinder(IAttributeBindingFactory attributeBindingFactory) : IViewBinder
{
    public IViewBinding Bind(IView view, IElement element, object? data)
    {
        var viewDescriptor = GetDescriptor(view);
        var context = data is not null ? BindingContext.Create(data) : null;
        var attributeBindings = element
            .Attributes.Select(attribute => attributeBindingFactory.CreateBinding(viewDescriptor, attribute, context))
            .ToList();
        // Initial forced update since some binding types (e.g. literals) never have updates.
        foreach (var attributeBinding in attributeBindings)
        {
            attributeBinding.Update(view, force: true);
        }
        var viewBinding = new ViewBinding(view, attributeBindings);
        return viewBinding;
    }

    public IViewDescriptor GetDescriptor(IView view)
    {
        return ReflectionViewDescriptor.ForViewType(view.GetType());
    }
}
