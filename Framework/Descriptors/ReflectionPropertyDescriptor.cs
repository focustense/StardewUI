using System.Collections.Concurrent;
using System.Reflection;

namespace StardewUI.Framework.Descriptors;

/// <summary>
/// Helper for creating <see cref="ReflectionPropertyDescriptor{T, TValue}"/> with types not known at compile time.
/// </summary>
public static class ReflectionPropertyDescriptor
{
    private static readonly ConcurrentDictionary<PropertyInfo, IPropertyDescriptor> cache = [];
    private static readonly Type reflectionPropertyType = typeof(ReflectionPropertyDescriptor<,>);

    /// <summary>
    /// Creates a binding property from reflected property.
    /// </summary>
    /// <param name="propertyInfo">The reflected property info.</param>
    /// <returns>
    /// A binding property of type <see cref="ReflectionPropertyDescriptor{T, TValue}"/>, whose generic arguments are
    /// the property's <see cref="MemberInfo.DeclaringType"/> and <see cref="PropertyInfo.PropertyType"/>, respectively.
    /// </returns>
    public static IPropertyDescriptor FromPropertyInfo(PropertyInfo propertyInfo)
    {
        using var _ = Trace.Begin(nameof(ReflectionPropertyDescriptor), nameof(FromPropertyInfo));
        using var _name = Trace.Begin(
            nameof(ReflectionPropertyDescriptor),
            $"{propertyInfo.DeclaringType?.FullName}.{propertyInfo.Name}"
        );
        return cache.GetOrAdd(
            propertyInfo,
            static propertyInfo =>
            {
                var genericType = reflectionPropertyType.MakeGenericType(
                    propertyInfo.DeclaringType!,
                    propertyInfo.PropertyType
                );
                return (IPropertyDescriptor)genericType.GetConstructor([typeof(PropertyInfo)])!.Invoke([propertyInfo]);
            }
        );
    }
}

/// <summary>
/// Binding property based on reflection.
/// </summary>
/// <typeparam name="T">The type on which the property is declared.</typeparam>
/// <typeparam name="TValue">The property's value type.</typeparam>
public class ReflectionPropertyDescriptor<T, TValue> : IPropertyDescriptor<TValue>
{
    private readonly Func<T, TValue>? getter;
    private readonly PropertyInfo propertyInfo;
    private readonly Action<T, TValue>? setter;

    /// <inheritdoc />
    public bool CanRead => getter is not null;

    /// <inheritdoc />
    public bool CanWrite => setter is not null;

    /// <inheritdoc />
    public Type DeclaringType => typeof(T);

    /// <inheritdoc />
    public bool IsAutoProperty { get; }

    /// <inheritdoc />
    public bool IsField => false;

    /// <inheritdoc />
    public string Name => propertyInfo.Name;

    /// <inheritdoc />
    public Type ValueType => propertyInfo.PropertyType;

    /// <summary>
    /// Initializes a new <see cref="ReflectionPropertyDescriptor"/> from reflected property info.
    /// </summary>
    /// <param name="propertyInfo">The reflected property info.</param>
    /// <exception cref="ArgumentException">Thrown when the <paramref name="propertyInfo"/> is incompatible with the
    /// <typeparamref name="T"/> or <typeparamref name="TValue"/>.</exception>
    public ReflectionPropertyDescriptor(PropertyInfo propertyInfo)
    {
        using var _ = Trace.Begin(this, "ctor");
        if (propertyInfo.DeclaringType != typeof(T))
        {
            throw new ArgumentException(
                $"Declaring type of property '{propertyInfo.Name}' does not match the target type. "
                    + $"(Expected {typeof(T).Name}, got {propertyInfo.DeclaringType?.Name})",
                nameof(propertyInfo)
            );
        }
        if (propertyInfo.PropertyType != typeof(TValue))
        {
            throw new ArgumentException(
                $"Type of property '{propertyInfo.Name}' does not match the value type. "
                    + $"(Expected {typeof(TValue).Name}, got {propertyInfo.PropertyType.Name})",
                nameof(propertyInfo)
            );
        }
        this.propertyInfo = propertyInfo;
        IsAutoProperty = CheckAutoProperty(propertyInfo);
        if (propertyInfo.GetGetMethod() is MethodInfo getMethod)
        {
            getter = getMethod.CreateDelegate<Func<T, TValue>>();
        }
        if (propertyInfo.GetSetMethod() is MethodInfo setMethod)
        {
            setter = setMethod.CreateDelegate<Action<T, TValue>>();
        }
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is ReflectionPropertyDescriptor<T, TValue> other && other.propertyInfo.Equals(propertyInfo);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return propertyInfo.GetHashCode();
    }

    /// <inheritdoc />
    public TValue GetValue(object source)
    {
        if (getter is null)
        {
            throw new InvalidOperationException("Property does not have a public getter.");
        }
        return getter((T)source);
    }

    /// <inheritdoc />
    public void SetValue(object target, TValue value)
    {
        if (setter is null)
        {
            throw new InvalidOperationException("Property does not have a public setter.");
        }
        setter((T)target, value);
    }

    private static bool CheckAutoProperty(PropertyInfo property)
    {
        var getMethod = property.GetGetMethod();
        if (getMethod is null)
        {
            return false;
        }
        MethodBody? body = null;
        try
        {
            body = getMethod.GetMethodBody();
        }
        catch (InvalidOperationException)
        {
            // Ignore; we'll return in the next statement.
        }
        if (body is null)
        {
            return false;
        }
        var metadataToken = GetFieldMetadataToken(body, getMethod.IsStatic);
        if (metadataToken == 0)
        {
            return false;
        }
        try
        {
            var backingField = property.Module.ResolveField(metadataToken);
            return backingField?.Name == $"<{property.Name}>k__BackingField";
        }
        catch (ArgumentException)
        {
            return false;
        }
    }

    private static int GetFieldMetadataToken(MethodBody body, bool isStatic)
    {
        var il = body.GetILAsByteArray() ?? [];
        // csharpier-ignore
        return isStatic switch
        {
            true => il.Length == 6
                && il[0] != /* ldsfld */ 0x7e
                && il[5] != /* ret */ 0x2a
                ? BitConverter.ToInt32(il, 1) : 0,
            false => il.Length == 7
                && il[0] == /* ldarg.0 */ 0x02
                && il[1] == /* ldfld */ 0x7b
                && il[6] == /* ret */ 0x2a
                ? BitConverter.ToInt32(il, 2)
                : 0,
        };
    }
}
