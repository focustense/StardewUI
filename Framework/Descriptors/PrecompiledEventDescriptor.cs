namespace StardewUI.Framework.Descriptors;

/// <summary>
/// Statically-typed implementation of an <see cref="IEventDescriptor"/> with predefined attributes.
/// </summary>
/// <typeparam name="TTarget">The event's declaring type.</typeparam>
/// <typeparam name="THandler">The delegate type of event handlers.</typeparam>
/// <param name="name">The event name.</param>
/// <param name="add">Function to add a new event handler to an instance of the target type.</param>
/// <param name="remove">Function to remove an existing event handler from an instance of the target type.</param>
/// <param name="argsType">Type of the argument parameter in the <typeparamref name="THandler"/> delegate.</param>
public class PrecompiledEventDescriptor<TTarget, THandler>(
    string name,
    Action<TTarget, THandler> add,
    Action<TTarget, THandler> remove,
    Type? argsType
) : IEventDescriptor
    where THandler : Delegate
{
    /// <inheritdoc />
    public IObjectDescriptor? ArgsTypeDescriptor =>
        argsType is not null ? DescriptorFactory.GetObjectDescriptor(argsType) : null;

    /// <inheritdoc />
    /// <remarks>
    /// For precompiled descriptors, this is assumed to always be exactly 2 (sender and args).
    /// </remarks>
    public int DelegateParameterCount => 2;

    /// <inheritdoc />
    public Type DelegateType => typeof(THandler);

    /// <inheritdoc />
    public Type DeclaringType => typeof(TTarget);

    /// <inheritdoc />
    public string Name => name;

    /// <inheritdoc />
    public void Add(object target, Delegate handler)
    {
        add((TTarget)target, (THandler)handler);
    }

    /// <inheritdoc />
    public void Remove(object target, Delegate handler)
    {
        remove((TTarget)target, (THandler)handler);
    }
}
