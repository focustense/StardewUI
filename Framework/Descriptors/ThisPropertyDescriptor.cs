using System.Reflection;

namespace StardewUI.Framework.Descriptors;

/// <summary>
/// Helper for obtaining a <see cref="ThisPropertyDescriptor{T}"/> using a type known only at runtime.
/// </summary>
public static class ThisPropertyDescriptor
{
    /// <summary>
    /// Gets the <see cref="ThisPropertyDescriptor{T}"/> corresponding to a specified type.
    /// </summary>
    /// <param name="type">The object type.</param>
    /// <returns>The <see cref="ThisPropertyDescriptor{T}"/> instance for the given <paramref name="type"/>.</returns>
    public static IPropertyDescriptor ForTypeUncached(Type type)
    {
        // This isn't cached because it's intended to only be called from a cached context, e.g. in
        // ReflectionObjectDescriptor.
        return (IPropertyDescriptor)
            typeof(ThisPropertyDescriptor<>)
                .MakeGenericType(type)
                .GetField(nameof(ThisPropertyDescriptor<object>.Instance), BindingFlags.Public | BindingFlags.Static)!
                .GetValue(null)!;
    }
}

/// <summary>
/// Special descriptor used for "this" references in argument/attribute bindings, allowing them to reference the current
/// context instead of a property on it.
/// </summary>
/// <typeparam name="T">The object type.</typeparam>
public class ThisPropertyDescriptor<T> : IPropertyDescriptor<T>
{
    /// <summary>
    /// Gets the singleton descriptor instance for the current object/property type.
    /// </summary>
    public static readonly ThisPropertyDescriptor<T> Instance = new();

    /// <inheritdoc />
    public bool CanRead => true;

    /// <inheritdoc />
    public bool CanWrite => false;

    /// <inheritdoc />
    public Type DeclaringType => typeof(T);

    /// <inheritdoc />
    public bool IsAutoProperty => false;

    /// <inheritdoc />
    public bool IsField => false;

    /// <inheritdoc />
    public string Name => "this";

    /// <inheritdoc />
    public Type ValueType => typeof(T);

    private ThisPropertyDescriptor() { }

    /// <inheritdoc />
    public T GetValue(object source)
    {
        return (T)source;
    }

    /// <inheritdoc />
    public void SetValue(object target, T value)
    {
        throw new NotSupportedException("'this' properties cannot be written.");
    }
}
