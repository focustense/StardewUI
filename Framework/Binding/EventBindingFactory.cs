using System.Collections.Concurrent;
using StardewUI.Framework.Converters;
using StardewUI.Framework.Descriptors;
using StardewUI.Framework.Dom;
using StardewUI.Framework.Sources;

namespace StardewUI.Framework.Binding;

/// <summary>
/// Service for creating <see cref="IEventBinding"/> instances for a view's events, and subscribing the handlers.
/// </summary>
public interface IEventBindingFactory
{
    /// <summary>
    /// Attempts to creates a new event binding.
    /// </summary>
    /// <param name="view">The view to bind to; the target that will raise the bound event.</param>
    /// <param name="viewDescriptor">Descriptor for the bound view, providing access to its events.</param>
    /// <param name="event">The event data.</param>
    /// <param name="context">The binding context, including the type descriptor and handler methods.</param>
    /// <returns>The created binding, or <c>null</c> if the arguments do not support creating a binding, such as an
    /// <paramref name="event"/> bound to a <c>null</c> value of <paramref name="context"/>.</returns>
    IEventBinding? TryCreateBinding(IView view, IViewDescriptor viewDescriptor, IEvent @event, BindingContext? context);
}

/// <summary>
/// Reflection-based implementation of an <see cref="IEventBindingFactory"/>.
/// </summary>
public class EventBindingFactory(IValueSourceFactory valueSourceFactory, IValueConverterFactory valueConverterFactory)
    : IEventBindingFactory
{
    // For caching:
    // - View type and event name map 1:1 with the delegate signature of the event itself
    // - Handler context type and method name map 1:1 with the signature of the target method
    // - Argument list and current context type combine to produce different outputs and need to be baked in.
    //
    // So cache key would seem to have to be all 6 of these params. We could also trade memory for performance by
    // caching on event itself (which includes declaring type) and handler method (same). Since these can be (and
    // generally are) inherited, we don't have to cache as much - but it also means that each time the method is
    // called, we need to look up the descriptors first.
    //
    // Since descriptors are cached, this is probably a minor efficiency loss vs. the memory gain.
    //
    // N.B. All descriptor types are expected to override Equals and GetHashCode, although since descriptors themselves
    // are generally cached, it may not be important. The IEvent should also be an instance of SEvent, a record type,
    // which implements the value equality required to use correctly as a cache key.
    record CacheKey(IEventDescriptor Event, IMethodDescriptor Handler, IEvent EventAttribute, Type BoundContextType);

    delegate IEventBinding? LocalBindingFactory(IView view, BindingContext viewContext, BindingContext handlerContext);

    private static readonly ConcurrentDictionary<CacheKey, LocalBindingFactory> cache = [];

    /// <inheritdoc />
    public IEventBinding? TryCreateBinding(
        IView view,
        IViewDescriptor viewDescriptor,
        IEvent @event,
        BindingContext? context
    )
    {
        using var _ = Trace.Begin(this, nameof(TryCreateBinding));
        var eventDescriptor = viewDescriptor.GetEvent(@event.Name.AsSpan().ToUpperCamelCase());
        var handlerContext = context?.Redirect(@event.ContextRedirect);
        if (handlerContext is null)
        {
            // Events can never be bound without a handler context since there would be nothing to invoke it on.
            // Statics are also not supported since there would be no way to infer the declaring type.
            return null;
        }
        // Could possibly use ctor injection of a "descriptor factory" but at the moment this is YAGNI since there is no
        // other way to get a descriptor anyway. If we add codegen/sourcegen as a means for creating descriptors then
        // it might be worth changing.
        IObjectDescriptor handlerContextDescriptor = DescriptorFactory.GetObjectDescriptor(
            handlerContext.Data.GetType()
        );
        var handlerMethod = handlerContextDescriptor.GetMethod(@event.HandlerName);

        var cacheKey = new CacheKey(eventDescriptor, handlerMethod, @event, context!.GetType());
        var localFactory = cache.GetOrAdd(
            cacheKey,
            _ =>
                (view, viewContext, handlerContext) =>
                    TryCreateHandlerBinding(view, eventDescriptor, handlerMethod, @event, viewContext, handlerContext)
        );
        return localFactory(view, context, handlerContext);
    }

    private IEventBinding? TryCreateHandlerBinding(
        IView view,
        IEventDescriptor eventDescriptor,
        IMethodDescriptor handlerMethod,
        IEvent @event,
        BindingContext viewContext,
        BindingContext handlerContext
    )
    {
        using var _ = Trace.Begin(this, nameof(TryCreateHandlerBinding));
        int requiredArgumentCount = handlerMethod.ArgumentTypes.Length - handlerMethod.OptionalArgumentCount;
        if (@event.Arguments.Count < requiredArgumentCount)
        {
            throw new BindingException(
                $"Cannot bind a call with {@event.Arguments.Count} to method "
                    + $"{handlerMethod.DeclaringType.Name}.{handlerMethod.Name} ({requiredArgumentCount} argument(s) "
                    + "required)."
            );
        }
        int argumentSourceCount = Math.Max(@event.Arguments.Count, requiredArgumentCount);
        var argumentSources = new IArgumentSource[argumentSourceCount];
        for (int i = 0; i < argumentSourceCount; i++)
        {
            var argumentData = @event.Arguments[i];
            var destinationType = handlerMethod.ArgumentTypes[i];
            if (argumentData.Type == Grammar.ArgumentExpressionType.EventBinding)
            {
                if (eventDescriptor.ArgsTypeDescriptor is null)
                {
                    throw new BindingException(
                        $"Cannot bind event property '{argumentData.Expression}' for event "
                            + $"{eventDescriptor.DeclaringType.Name}.{eventDescriptor.Name} because the delegate type "
                            + $"{eventDescriptor.DelegateType.Name} does not include an event args parameter."
                    );
                }
                var eventPropertyDescriptor = eventDescriptor.ArgsTypeDescriptor.GetProperty(argumentData.Expression);
                argumentSources[i] = EventArgumentSource.Create(
                    eventDescriptor.ArgsTypeDescriptor.TargetType,
                    destinationType,
                    eventPropertyDescriptor,
                    valueConverterFactory
                );
            }
            else
            {
                var valueType =
                    valueSourceFactory.GetValueType(argumentData, viewContext)
                    ?? throw new BindingException(
                        $"Couldn't determine value type for argument {i} of event "
                            + $"{eventDescriptor.DeclaringType.Name}.{eventDescriptor.Name} ({argumentData}). "
                            + "Either the context is incorrect or the property does not exist."
                    );
                var originalValueSource = valueSourceFactory.GetValueSource(valueType, argumentData, viewContext);
                var convertedValueSource = ConvertedValueSource.Create(
                    originalValueSource,
                    destinationType,
                    valueConverterFactory
                );
                argumentSources[i] = new BoundArgumentSource(convertedValueSource);
            }
        }
        return EventBinding.Create(view, eventDescriptor, handlerContext.Data, handlerMethod, argumentSources);
    }
}
