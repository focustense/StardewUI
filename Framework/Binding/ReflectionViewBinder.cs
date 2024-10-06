﻿using StardewUI.Framework.Descriptors;
using StardewUI.Framework.Dom;
using StardewUI.Framework.Grammar;

namespace StardewUI.Framework.Binding;

/// <summary>
/// An <see cref="IViewBinder"/> implementation using reflected view descriptors.
/// </summary>
/// <param name="attributeBindingFactory">Factory for creating the <see cref="IAttributeBinding"/> instances used to
/// bind individual attributes of the view.</param>
/// <param name="eventBindingFactory">Factory for creating the <see cref="IEventBinding"/> instances used to bind events
/// raised by the view.</param>
public class ReflectionViewBinder(
    IAttributeBindingFactory attributeBindingFactory,
    IEventBindingFactory eventBindingFactory
) : IViewBinder
{
    public IViewBinding Bind(IView view, IElement element, BindingContext? context)
    {
        var viewDescriptor = GetDescriptor(view);
        var attributeBindings = element
            // Only property attributes are bound to the view; others affect the outer hierarchy.
            .Attributes.Where(attribute => attribute.Type == AttributeType.Property)
            .Select(attribute => attributeBindingFactory.TryCreateBinding(viewDescriptor, attribute, context))
            .Where(binding => binding is not null)
            .Cast<IAttributeBinding>()
            .ToList();
        // Initial forced update since some binding types (e.g. literals) never have updates.
        foreach (var attributeBinding in attributeBindings)
        {
            if (attributeBinding.Direction.IsIn())
            {
                attributeBinding.UpdateView(view, force: true);
            }
        }
        var eventBindings = element
            .Events.Select(@event => eventBindingFactory.TryCreateBinding(view, viewDescriptor, @event, context))
            .Where(binding => binding is not null)
            .Cast<IEventBinding>()
            .ToList();
        var viewBinding = new ViewBinding(view, attributeBindings, eventBindings);
        return viewBinding;
    }

    public IViewDescriptor GetDescriptor(IView view)
    {
        return ReflectionViewDescriptor.ForViewType(view.GetType());
    }
}
