using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace StardewUITest.StarML;

/// <summary>
/// Object descriptor based on reflection.
/// </summary>
public class ReflectionObjectDescriptor : IObjectDescriptor
{
    public IEnumerable<IBindingProperty> Properties => propertiesByName.Values;

    public Type TargetType { get; }

    private static readonly Dictionary<Type, ReflectionObjectDescriptor> cache = [];
    private readonly IReadOnlyDictionary<string, IBindingProperty> propertiesByName;

    /// <summary>
    /// Creates or retrieves a descriptor for a given object type.
    /// </summary>
    /// <param name="type">The object type.</param>
    /// <returns>The descriptor for the specified <paramref name="type"/>.</returns>
    public static ReflectionObjectDescriptor ForType(Type type)
    {
        if (!cache.TryGetValue(type, out var descriptor))
        {
            descriptor = CreateForType(type, (type, props) => new ReflectionObjectDescriptor(type, props));
            cache[type] = descriptor;
        }
        return descriptor;
    }

    /// <summary>
    /// Creates a new descriptor for a given object type, using the specified factory to create the instance.
    /// </summary>
    /// <typeparam name="T">The descriptor type.</typeparam>
    /// <param name="type">The object type.</param>
    /// <param name="create">Factory to create the <typeparamref name="T"/> descriptor, given the original
    /// <paramref name="type"/> and a dictionary of its binding properties by name.</param>
    /// <returns>A new descriptor for the specified <paramref name="type"/>.</returns>
    protected static T CreateForType<T>(Type type, Func<Type, IReadOnlyDictionary<string, IBindingProperty>, T> create)
        where T : ReflectionObjectDescriptor
    {
        var propertiesByName = type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(propInfo => propInfo.GetIndexParameters().Length == 0)
            .Select(ReflectionProperty.FromPropertyInfo)
            .ToDictionary(prop => prop.Name);
        return create(type, propertiesByName);
    }

    /// <summary>
    /// Initializes a new <see cref="ReflectionObjectDescriptor"/> with the given target type and properties.
    /// </summary>
    /// <param name="type">The <see cref="TargetType"/>.</param>
    /// <param name="propertiesByName">Dictionary of property names to the corresponding binding properties.</param>
    protected ReflectionObjectDescriptor(Type type, IReadOnlyDictionary<string, IBindingProperty> propertiesByName)
    {
        TargetType = type;
        this.propertiesByName = propertiesByName;
    }

    public bool TryGetProperty(string name, [MaybeNullWhen(false)] out IBindingProperty property)
    {
        return propertiesByName.TryGetValue(name, out property);
    }
}
