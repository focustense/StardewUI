using StardewUI.Framework.Content;
using StardewUI.Framework.Descriptors;
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
    /// <inheritdoc />
    public IViewBinding Bind(IView view, IElement element, BindingContext? context, IResolutionScope resolutionScope)
    {
        using var _ = Trace.Begin(this, nameof(Bind));
        var viewDescriptor = GetDescriptor(view);
        var attributeBindings = element
            // Only property attributes are bound to the view; others affect the outer hierarchy.
            .Attributes.AsParallel()
            .WithDegreeOfParallelism(Math.Max(1, Environment.ProcessorCount / 4))
            // Ideally, attribute order should not affect the outcome, but in some rare cases it can, e.g. if a
            // drop-down has two-way bindings on both the selected index and selected item.
            //
            // Order preservation won't "fix" that, but it will make the behavior consistent, and avoid frustrating
            // scenarios where the UI sometimes appears to do what it's supposed to and sometimes doesn't, depending on
            // how the binding happened to get set up the first time.
            .AsOrdered()
            .Where(attribute => attribute.Type == AttributeType.Property)
            .Select(attribute =>
                attributeBindingFactory.TryCreateBinding(viewDescriptor, attribute, context, resolutionScope)
            )
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
            .Events.AsParallel()
            .WithDegreeOfParallelism(Math.Max(1, Environment.ProcessorCount / 4))
            // Events do not need the same ordering treatment as attributes because event order is defined by the order
            // in which those events are actually fired, not the order they are bound in.
            // Performance is slightly improved by allowing order to be ignored.
            .Select(@event => eventBindingFactory.TryCreateBinding(view, viewDescriptor, @event, context))
            .Where(binding => binding is not null)
            .Cast<IEventBinding>()
            .ToList();
        var viewBinding = new ViewBinding(view, attributeBindings, eventBindings);
        return viewBinding;
    }

    /// <inheritdoc />
    public IViewDescriptor GetDescriptor(IView view)
    {
        return DescriptorFactory.GetViewDescriptor(view.GetType());
    }
}
