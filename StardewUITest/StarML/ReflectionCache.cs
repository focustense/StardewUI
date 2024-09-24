using System.Reflection;

namespace StardewUITest.StarML;

public interface IReflectionCache
{
    IBindingProperty GetProperty(Type receiverType, string propertyName);
    IBindingProperty<T> GetProperty<T>(Type receiverType, string propertyName);
}

public class ReflectionCache : IReflectionCache
{
    private readonly Dictionary<(Type, string), IBindingProperty> properties = [];

    public IBindingProperty GetProperty(Type declaringType, string propertyName)
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

    public IBindingProperty<T> GetProperty<T>(Type declaringType, string propertyName)
    {
        return (IBindingProperty<T>)GetProperty(declaringType, propertyName);
    }
}