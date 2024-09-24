using System.Linq.Expressions;
using System.Reflection;

namespace StardewUITest.StarML;

public interface IReflectionCache
{
    IProperty GetProperty(Type receiverType, string propertyName);
    IProperty<T> GetProperty<T>(Type receiverType, string propertyName);
}

public interface IProperty
{
    bool CanRead { get; }

    bool CanWrite { get; }

    Type ValueType { get; }
}

public interface IProperty<T> : IProperty
{
    T GetValue(object source);

    void SetValue(object target, T value);
}

public class ReflectionCache : IReflectionCache
{
    private readonly Dictionary<(Type, string), IProperty> properties = [];

    public IProperty GetProperty(Type declaringType, string propertyName)
    {
        var key = (declaringType, propertyName);
        if (!properties.TryGetValue(key, out var property))
        {
            var propertyInfo = declaringType.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public)
                ?? throw new BindingException($"{declaringType.Name} does not have a public instance property named '{propertyName}'.");
            if (propertyInfo.GetIndexParameters().Length > 0)
            {
                throw new BindingException($"Cannot bind to indexed property '{propertyName}' of type {declaringType.Name}.");
            }
            property = ReflectionProperty.FromPropertyInfo(propertyInfo);
        }
        return property;
    }

    public IProperty<T> GetProperty<T>(Type declaringType, string propertyName)
    {
        return (IProperty<T>)GetProperty(declaringType, propertyName);
    }
}

internal static class ReflectionProperty
{
    private static readonly Type reflectionPropertyType = typeof(ReflectionProperty<,>);

    public static IProperty FromPropertyInfo(PropertyInfo propertyInfo)
    {
        var genericType = reflectionPropertyType.MakeGenericType(propertyInfo.DeclaringType!, propertyInfo.PropertyType);
        return (IProperty)genericType.GetConstructor([typeof(PropertyInfo)])!.Invoke([propertyInfo]);
    }
}

internal class ReflectionProperty<T, TValue> : IProperty<TValue>
{
    private readonly Func<T, TValue>? getter;
    private readonly Action<T, TValue>? setter;

    public bool CanRead => getter is not null;

    public bool CanWrite => setter is not null;

    public Type ValueType { get; }

    public ReflectionProperty(PropertyInfo propertyInfo)
    {
        ValueType = propertyInfo.PropertyType;
        if (propertyInfo.CanRead)
        {
            var getMethod = propertyInfo.GetGetMethod()!;
            getter = getMethod.CreateDelegate<Func<T, TValue>>();
        }
        if (propertyInfo.CanWrite)
        {
            var setMethod = propertyInfo.GetSetMethod()!;
            setter = setMethod.CreateDelegate<Action<T, TValue>>();
        }
    }

    public TValue GetValue(object source)
    {
        if (getter is null)
        {
            throw new InvalidOperationException("Property does not have a public getter.");
        }
        return getter((T)source);
    }

    public void SetValue(object target, TValue value)
    {
        if (setter is null)
        {
            throw new InvalidOperationException("Property does not have a public setter.");
        }
        setter((T)target, value);
    }
}