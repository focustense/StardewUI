using System.Collections.Concurrent;
using System.Reflection;
using StardewUI.Events;
using StardewUI.Framework.Descriptors;

namespace StardewUI.Framework.Binding;

/// <summary>
/// Helper for creating generic <see cref="EventBinding{TEventArgs, TResult}"/> instances efficiently using types known
/// only at runtime.
/// </summary>
internal static class EventBinding
{
    delegate IEventBinding Factory(
        object eventTarget,
        IEventDescriptor eventDescriptor,
        object destinationContext,
        IMethodDescriptor destinationMethod,
        IArgumentSource[] argumentSources
    );

    private static readonly ConcurrentDictionary<(Type, Type), Factory> cache = [];
    private static readonly MethodInfo createInternalMethod = typeof(EventBinding).GetMethod(
        nameof(CreateInternal),
        BindingFlags.Static | BindingFlags.NonPublic
    )!;

    /// <summary>
    /// Creates a new event binding.
    /// </summary>
    /// <param name="eventTarget">The target that will raise the event, i.e. the view.</param>
    /// <param name="eventDescriptor">Descriptor (name, types, arguments, etc.) for the event.</param>
    /// <param name="destinationContext">Target object on which the <paramref name="destinationMethod"/> should be
    /// invoked.</param>
    /// <param name="destinationMethod">Method to invoke when the specified event is raised.</param>
    /// <param name="argumentSources">Sources to provide the argument values to the
    /// <paramref name="destinationMethod"/>.</param>
    /// <returns></returns>
    public static IEventBinding Create(
        object eventTarget,
        IEventDescriptor eventDescriptor,
        object destinationContext,
        IMethodDescriptor destinationMethod,
        IArgumentSource[] argumentSources
    )
    {
        using var _ = Trace.Begin(nameof(EventBinding), nameof(Create));
        var argsType = eventDescriptor.ArgsTypeDescriptor?.TargetType ?? typeof(object);
        var returnType = destinationMethod.ReturnType;
        if (returnType == typeof(void))
        {
            returnType = typeof(object);
        }
        var key = (argsType, returnType);
        var factory = cache.GetOrAdd(
            key,
            _ => createInternalMethod.MakeGenericMethod(argsType, returnType).CreateDelegate<Factory>()
        );
        return factory(eventTarget, eventDescriptor, destinationContext, destinationMethod, argumentSources);
    }

    private static IEventBinding CreateInternal<TEventArgs, TResult>(
        object eventTarget,
        IEventDescriptor eventDescriptor,
        object destinationContext,
        IMethodDescriptor destinationMethod,
        IArgumentSource[] argumentSources
    )
    {
        return new EventBinding<TEventArgs, TResult>(
            eventTarget,
            eventDescriptor,
            destinationContext,
            (IMethodDescriptor<TResult>)destinationMethod,
            argumentSources
        );
    }
}

/// <summary>
/// Internal, transient state of an event binding created by the <see cref="EventBindingFactory"/>.
/// </summary>
internal class EventBinding<TEventArgs, TResult> : IEventBinding
{
    private readonly IArgumentSource[] argumentSources;
    private readonly object destinationContext;
    private readonly IMethodDescriptor<TResult> destinationMethod;
    private readonly IEventDescriptor eventDescriptor;
    private readonly object eventTarget;
    private readonly Delegate handlerDelegate;

    public EventBinding(
        object eventTarget,
        IEventDescriptor eventDescriptor,
        object destinationContext,
        IMethodDescriptor<TResult> destinationMethod,
        IArgumentSource[] argumentSources
    )
    {
        using var _ = Trace.Begin(this, "ctor");

        this.eventTarget = eventTarget;
        this.eventDescriptor = eventDescriptor;
        this.destinationContext = destinationContext;
        this.destinationMethod = destinationMethod;
        this.argumentSources = argumentSources;

        int paramCount = eventDescriptor.DelegateParameterCount;
        Delegate internalDelegate = paramCount switch
        {
            0 => HandleEmpty,
            1 => HandleWithArgs,
            2 => HandleWithSenderAndArgs,
            _ => throw new BindingException(
                $"Invalid number of argument types in event descriptor; expected 0-2, got {paramCount}."
            ),
        };
        // There is probably a more efficient way to do this? However, just using the internalDelegate without going
        // through the conversion will throw an incompatible cast exception when casting to e.g. EventHandler<T>.
        // Worst case, maybe cache the result by DelegateType so that at least we aren't repeating this work.
        handlerDelegate = internalDelegate.Method.CreateDelegate(eventDescriptor.DelegateType, this);
        eventDescriptor.Add(eventTarget, handlerDelegate);
    }

    public void Dispose()
    {
        eventDescriptor.Remove(eventTarget, handlerDelegate);
        GC.SuppressFinalize(this);
    }

    private object?[] CreateMethodArguments(TEventArgs? eventArgs)
    {
        var args = new object?[destinationMethod.ArgumentTypes.Length];
        for (int i = 0; i < destinationMethod.ArgumentTypes.Length; i++)
        {
            args[i] = i < argumentSources.Length ? argumentSources[i].GetValue(eventArgs) : Type.Missing;
        }
        return args;
    }

    private void HandleEmpty()
    {
        InvokeDestinationMethod(default);
    }

    private void HandleWithArgs(TEventArgs eventArgs)
    {
        InvokeDestinationMethod(eventArgs);
    }

    private void HandleWithSenderAndArgs(object _, TEventArgs eventArgs)
    {
        InvokeDestinationMethod(eventArgs);
    }

    private void InvokeDestinationMethod(TEventArgs? eventArgs)
    {
        using var _ = Trace.Begin(this, nameof(InvokeDestinationMethod));
        var methodArgs = CreateMethodArguments(eventArgs);
        var result = destinationMethod.Invoke(destinationContext, methodArgs);
        if (eventArgs is BubbleEventArgs bubbleArgs && result is bool handled && handled)
        {
            bubbleArgs.Handled = true;
        }
    }
}
