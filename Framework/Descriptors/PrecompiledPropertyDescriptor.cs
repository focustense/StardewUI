namespace StardewUI.Framework.Descriptors;

/// <summary>
/// Statically-typed implementation of an <see cref="IPropertyDescriptor{T}"/> with predefined attributes.
/// </summary>
/// <typeparam name="TTarget">The property's declaring type.</typeparam>
/// <typeparam name="TProperty">The property value type.</typeparam>
/// <param name="name">The event name.</param>
/// <param name="isField">Whether or not this descriptor is really for an unwrapped field.</param>
/// <param name="isAutoProperty">Whether or not the property is auto-implemented.</param>
/// <param name="getter">Function to retrieve the current property value from a target instance.</param>
/// <param name="setter">Function to set the current property value on a target instance.</param>
public class PrecompiledPropertyDescriptor<TTarget, TProperty>(
    string name,
    bool isField,
    bool isAutoProperty,
    Func<TTarget, TProperty>? getter,
    Action<TTarget, TProperty>? setter
) : IPropertyDescriptor<TProperty>
{
    /// <inheritdoc />
    public bool CanRead => getter is not null;

    /// <inheritdoc />
    public bool CanWrite => setter is not null;

    /// <inheritdoc />
    public Type DeclaringType => typeof(TTarget);

    /// <inheritdoc />
    public bool IsAutoProperty => isAutoProperty;

    /// <inheritdoc />
    public bool IsField => isField;

    /// <inheritdoc />
    public string Name => name;

    /// <inheritdoc />
    public Type ValueType => typeof(TProperty);

    /// <inheritdoc />
    public TProperty GetValue(object source)
    {
        if (getter is null)
        {
            throw new InvalidOperationException("Property does not have a public getter.");
        }
        return getter((TTarget)source);
    }

    /// <inheritdoc />
    public void SetValue(object target, TProperty value)
    {
        if (setter is null)
        {
            throw new InvalidOperationException("Property does not have a public setter.");
        }
        setter((TTarget)target, value);
    }
}
