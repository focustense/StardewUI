using System.Reflection;

namespace StardewUI.Framework.Descriptors;

/// <summary>
/// Implementation of a field descriptor based on reflection.
/// </summary>
/// <remarks>
/// Reflection has the quickest setup time of available methods, but the slowest overall access/write time.
/// </remarks>
/// <typeparam name="TValue">The field's value type.</typeparam>
/// <param name="field">The reflected field.</param>
public class ReflectionFieldDescriptor<TValue>(FieldInfo field) : IPropertyDescriptor<TValue>
{
    /// <inheritdoc />
    /// <remarks>
    /// For fields, always returns <c>true</c>.
    /// </remarks>
    public bool CanRead => true;

    /// <inheritdoc />
    public bool CanWrite => !field.IsInitOnly && !field.IsLiteral;

    /// <inheritdoc />
    public Type DeclaringType => field.DeclaringType!;

    /// <inheritdoc />
    public bool IsAutoProperty => false;

    /// <inheritdoc />
    public bool IsField => true;

    /// <inheritdoc />
    public string Name => field.Name;

    /// <inheritdoc />
    public Type ValueType => field.FieldType;

    /// <inheritdoc />
    public TValue GetValue(object source)
    {
        return (TValue)field.GetValue(source)!;
    }

    /// <inheritdoc />
    public void SetValue(object target, TValue value)
    {
        field.SetValue(target, value);
    }
}
