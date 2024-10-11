using System.Reflection;

namespace StardewUI.Framework.Descriptors;

/// <summary>
/// Helper for creating <see cref="IEventDescriptor"/> instances using reflection.
/// </summary>
public static class ReflectionEventDescriptor
{
    private static readonly Dictionary<EventInfo, IEventDescriptor> cache = [];
    private static readonly MethodInfo createTypedDescriptorMethod = typeof(ReflectionEventDescriptor).GetMethod(
        nameof(CreateTypedDescriptor),
        BindingFlags.Static | BindingFlags.NonPublic
    )!;

    /// <summary>
    /// Creates or retrieves a descriptor for a given event.
    /// </summary>
    /// <param name="eventInfo">The event info.</param>
    /// <returns>The descriptor for the specified <paramref name="eventInfo"/>.</returns>
    public static IEventDescriptor FromEventInfo(EventInfo eventInfo)
    {
        if (!cache.TryGetValue(eventInfo, out var descriptor))
        {
            descriptor = CreateDescriptor(eventInfo);
            cache.Add(eventInfo, descriptor);
        }
        return descriptor;
    }

    private static IEventDescriptor CreateDescriptor(EventInfo eventInfo)
    {
        if (eventInfo.DeclaringType is null)
        {
            throw new TargetException($"Event {eventInfo.Name} is missing a declaring type.");
        }
        if (eventInfo.EventHandlerType is null)
        {
            throw new TargetException(
                $"Event {eventInfo.DeclaringType.Name}.{eventInfo.Name} is missing a handler type."
            );
        }
        if (eventInfo.AddMethod is null)
        {
            throw new TargetException(
                $"Event {eventInfo.DeclaringType.Name}.{eventInfo.Name} is missing an Add method."
            );
        }
        if (eventInfo.RemoveMethod is null)
        {
            throw new TargetException(
                $"Event {eventInfo.DeclaringType.Name}.{eventInfo.Name} is missing a Remove method."
            );
        }
        return (IEventDescriptor)
            createTypedDescriptorMethod
                .MakeGenericMethod(eventInfo.DeclaringType, eventInfo.EventHandlerType)
                .Invoke(null, [eventInfo])!;
    }

    private static IEventDescriptor CreateTypedDescriptor<TTarget, THandler>(EventInfo eventInfo)
        where THandler : Delegate
    {
        // Creating explicit delegates for adding/removing means that we have to know, and cache with, the event's
        // declaring type, which is more information than what is needed for EventInfo.AddEventHandler and
        // EventInfo.RemoveHandler (whose signatures accept any target and delegate combination). However, once the
        // proxy is established this way, it has been shown to be more than 2x faster than pure reflection; and in any
        // given session, there should be many more instances of adding/removing events occuring (e.g. every time any
        // type of view is bound in any type of menu) than there will be of initializing these descriptors.
        var addMethod = eventInfo.AddMethod!.CreateDelegate<Action<TTarget, THandler>>();
        var removeMethod = eventInfo.RemoveMethod!.CreateDelegate<Action<TTarget, THandler>>();
        var invokeMethod = eventInfo.EventHandlerType!.GetMethod("Invoke")!;
        if (invokeMethod.ReturnType != typeof(void))
        {
            throw new TargetException(
                $"Event {eventInfo.DeclaringType!.Name}.{eventInfo.Name} uses a delegate with invalid return type "
                    + $"{invokeMethod.ReturnType.Name} (must be void)."
            );
        }
        var argsType = GetArgsType(invokeMethod, () => $"{eventInfo.DeclaringType!.Name}.{eventInfo.Name}");
        var argsTypeDescriptor = argsType is not null ? ReflectionObjectDescriptor.ForType(argsType) : null;
        return new ReflectionEventDescriptor<TTarget, THandler>(
            eventInfo,
            invokeMethod,
            argsTypeDescriptor,
            addMethod,
            removeMethod
        );
    }

    private static Type? GetArgsType(MethodInfo invokeMethod, Func<string> eventDisplayName)
    {
        var invokeParams = invokeMethod.GetParameters();
        // The most common event should have a signature of (object sender, FooEventArgs e).
        // The second-most common, and very far behind, would be Action<FooEventArgs> or just Action<Foo>.
        // Other, esoteric delegate types aren't supported as there's no way to know their signature in advance.
        //
        // It's possible to build expression trees, or use reflection-based invocation, or use other various tradeoffs
        // of complexity vs. runtime performance vs. setup performance, but non-standard delegate types should be so
        // rare that it's not worth the cost of writing and maintaining them.
        //
        // (This is especially true since we only care about events on View types, which we own, not Context/Model data
        // types provided by the user.)
        return invokeParams.Length switch
        {
            0 => null,
            1 => invokeParams[0].ParameterType,
            2 => invokeParams[1].ParameterType,
            _ => throw new TargetException(
                $"Event {eventDisplayName()} uses a delegate with too many ({invokeParams.Length}) parameters. "
                    + "Supported event delegates are either EventHandler<TEventArgs> (object, TEventArgs) or "
                    + "Action<TEventArgs>."
            ),
        };
    }
}

/// <summary>
/// Reflection-based implementation of an event descriptor.
/// </summary>
/// <typeparam name="TTarget">The type that declares the event.</typeparam>
/// <typeparam name="THandler">The event handler (delegate) type.</typeparam>
public class ReflectionEventDescriptor<TTarget, THandler> : IEventDescriptor
    where THandler : Delegate
{
    private readonly Action<TTarget, THandler> add;
    private readonly EventInfo eventInfo;
    private readonly Action<TTarget, THandler> remove;

    /// <inheritdoc />
    public IObjectDescriptor? ArgsTypeDescriptor { get; }

    /// <inheritdoc />
    public int DelegateParameterCount { get; }

    /// <inheritdoc />
    public Type DelegateType => typeof(THandler);

    /// <inheritdoc />
    public Type DeclaringType => eventInfo.DeclaringType!;

    /// <inheritdoc />
    public string Name => eventInfo.Name;

    internal ReflectionEventDescriptor(
        EventInfo eventInfo,
        MethodInfo invokeMethod,
        IObjectDescriptor? argsTypeDescriptor,
        Action<TTarget, THandler> add,
        Action<TTarget, THandler> remove
    )
    {
        this.eventInfo = eventInfo;
        ArgsTypeDescriptor = argsTypeDescriptor;
        DelegateParameterCount = invokeMethod.GetParameters().Length;
        this.add = add;
        this.remove = remove;
    }

    /// <inheritdoc />
    public void Add(object target, Delegate handler)
    {
        add((TTarget)target, (THandler)handler);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is ReflectionEventDescriptor<TTarget, THandler> other && other.eventInfo.Equals(eventInfo);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return eventInfo.GetHashCode();
    }

    /// <inheritdoc />
    public void Remove(object target, Delegate handler)
    {
        remove((TTarget)target, (THandler)handler);
    }
}
