using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace StardewUI.Framework.Descriptors;

/// <summary>
/// Object descriptor based on reflection.
/// </summary>
public class ReflectionObjectDescriptor : IObjectDescriptor
{
    public Type TargetType { get; }

    private static readonly Dictionary<Type, ReflectionObjectDescriptor> cache = [];

    private readonly IReadOnlyDictionary<string, Lazy<IEventDescriptor>> eventsByName;
    private readonly IReadOnlyDictionary<string, Lazy<IMethodDescriptor>> methodsByName;
    private readonly IReadOnlyDictionary<string, Lazy<IPropertyDescriptor>> propertiesByName;

    /// <summary>
    /// Creates or retrieves a descriptor for a given object type.
    /// </summary>
    /// <param name="type">The object type.</param>
    /// <returns>The descriptor for the specified <paramref name="type"/>.</returns>
    public static ReflectionObjectDescriptor ForType(Type type)
    {
        if (!cache.TryGetValue(type, out var descriptor))
        {
            descriptor = CreateDescriptor(type);
            cache[type] = descriptor;
        }
        return descriptor;
    }

    private static ReflectionObjectDescriptor CreateDescriptor(Type type)
    {
        var allMembers = type.GetMembers(BindingFlags.Instance | BindingFlags.Public);
        var propertiesByName = allMembers
            .OfType<PropertyInfo>()
            .Where(prop => prop.GetIndexParameters().Length == 0)
            .ToLazyDictionary(ReflectionPropertyDescriptor.FromPropertyInfo);
        var methodsByName = allMembers
            .OfType<MethodInfo>()
            .Where(method => !method.IsAbstract && !method.IsGenericMethod)
            .DistinctBy(method => method.Name)
            .ToLazyDictionary(ReflectionMethodDescriptor.FromMethodInfo);
        var eventsByName = allMembers
            .OfType<EventInfo>()
            .Where(ev => ev.EventHandlerType is not null)
            .ToLazyDictionary(ReflectionEventDescriptor.FromEventInfo);
        return new(type, propertiesByName, methodsByName, eventsByName);
    }

    /// <summary>
    /// Initializes a new <see cref="ReflectionObjectDescriptor"/> with the given target type and members.
    /// </summary>
    /// <param name="type">The <see cref="TargetType"/>.</param>
    /// <param name="propertiesByName">Dictionary of property names to the corresponding property descriptors.</param>
    /// <param name="methodsByName">Dictionary of method names to the corresponding method descriptors.</param>
    /// <param name="eventsByName">Dictionary of event names to the corresponding event descriptors.</param>
    protected ReflectionObjectDescriptor(
        Type type,
        IReadOnlyDictionary<string, Lazy<IPropertyDescriptor>> propertiesByName,
        IReadOnlyDictionary<string, Lazy<IMethodDescriptor>> methodsByName,
        IReadOnlyDictionary<string, Lazy<IEventDescriptor>> eventsByName
    )
    {
        TargetType = type;
        this.propertiesByName = propertiesByName;
        this.methodsByName = methodsByName;
        this.eventsByName = eventsByName;
    }

    public bool TryGetEvent(string name, [MaybeNullWhen(false)] out IEventDescriptor @event)
    {
        bool exists = eventsByName.TryGetValue(name, out var lazyEvent);
        @event = lazyEvent?.Value;
        return exists;
    }

    public bool TryGetMethod(string name, [MaybeNullWhen(false)] out IMethodDescriptor method)
    {
        bool exists = methodsByName.TryGetValue(name, out var lazyMethod);
        method = lazyMethod?.Value;
        return exists;
    }

    public bool TryGetProperty(string name, [MaybeNullWhen(false)] out IPropertyDescriptor property)
    {
        bool exists = propertiesByName.TryGetValue(name, out var lazyProperty);
        property = lazyProperty?.Value;
        return exists;
    }
}

file static class MemberListExtensions
{
    public static IReadOnlyDictionary<string, Lazy<TDescriptor>> ToLazyDictionary<T, TDescriptor>(
        this IEnumerable<T> source,
        Func<T, TDescriptor> descriptorSelector
    )
        where T : MemberInfo
    {
        return source
            .Select(x => (key: x.Name, descriptor: new Lazy<TDescriptor>(() => descriptorSelector(x))))
            .ToDictionary(x => x.key, x => x.descriptor);
    }
}
