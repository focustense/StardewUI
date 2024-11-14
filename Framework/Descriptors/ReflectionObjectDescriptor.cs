using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using StardewValley;

namespace StardewUI.Framework.Descriptors;

/// <summary>
/// Object descriptor based on reflection.
/// </summary>
public class ReflectionObjectDescriptor : IObjectDescriptor
{
    /// <inheritdoc />
    public bool SupportsChangeNotifications { get; }

    /// <inheritdoc />
    public Type TargetType { get; }

    private static readonly ConcurrentDictionary<Type, ReflectionObjectDescriptor> cache = [];

    private readonly IReadOnlyDictionary<string, Lazy<IMemberDescriptor>> membersByName;
    private readonly Lazy<IPropertyDescriptor> thisDescriptor;

    /// <summary>
    /// Creates or retrieves a descriptor for a given object type.
    /// </summary>
    /// <param name="type">The object type.</param>
    /// <returns>The descriptor for the specified <paramref name="type"/>.</returns>
    public static ReflectionObjectDescriptor ForType(Type type)
    {
        return cache.GetOrAdd(type, CreateDescriptor);
    }

    private static ReflectionObjectDescriptor CreateDescriptor(Type type)
    {
        using var _ = Trace.Begin(nameof(ReflectionObjectDescriptor), nameof(CreateDescriptor));
        var interfaces = type.GetInterfaces();
        var membersByName = type.GetMembers(BindingFlags.Instance | BindingFlags.Public)
            .AsParallel()
            .WithDegreeOfParallelism(Math.Max(1, Environment.ProcessorCount / 2))
            .Where(member =>
                member switch
                {
                    FieldInfo field => true,
                    PropertyInfo prop => prop.GetIndexParameters().Length == 0,
                    MethodInfo method => !method.IsAbstract && !method.IsGenericMethod,
                    EventInfo ev => ev.EventHandlerType is not null,
                    _ => false,
                }
            )
            // DistinctBy doesn't have a Parallel implementation so we use GroupBy instead.
            .GroupBy(member => member.Name)
            .Select(g => g.First())
            .ToLazyDictionary(member =>
                member switch
                {
                    FieldInfo field => LazyExpressionFieldDescriptor.FromFieldInfo(field) as IMemberDescriptor,
                    PropertyInfo prop => ReflectionPropertyDescriptor.FromPropertyInfo(prop),
                    MethodInfo method => ReflectionMethodDescriptor.FromMethodInfo(method),
                    EventInfo ev => ReflectionEventDescriptor.FromEventInfo(ev),
                    _ => throw new DescriptorException(
                        $"Invalid member type {member.MemberType} for descriptor of "
                            + $"{member.DeclaringType?.Name}.{member.Name}"
                    ),
                }
            );
        return new(type, interfaces, membersByName);
    }

    /// <summary>
    /// Initializes a new <see cref="ReflectionObjectDescriptor"/> with the given target type and members.
    /// </summary>
    /// <param name="type">The <see cref="TargetType"/>.</param>
    /// <param name="interfaces">All interfaces implemented by the <see cref="TargetType"/>.</param>
    /// <param name="membersByName">Dictionary of member names to the corresponding member descriptors.</param>
    protected ReflectionObjectDescriptor(
        Type type,
        IReadOnlyList<Type> interfaces,
        IReadOnlyDictionary<string, Lazy<IMemberDescriptor>> membersByName
    )
    {
        TargetType = type;
        this.membersByName = membersByName;
        thisDescriptor = new(() => ThisPropertyDescriptor.ForTypeUncached(type));
        SupportsChangeNotifications = interfaces.Any(t => t == typeof(INotifyPropertyChanged));
    }

    /// <inheritdoc />
    public bool TryGetEvent(string name, [MaybeNullWhen(false)] out IEventDescriptor @event)
    {
        return TryGetMember(name, out @event);
    }

    /// <inheritdoc />
    public bool TryGetMethod(string name, [MaybeNullWhen(false)] out IMethodDescriptor method)
    {
        return TryGetMember(name, out method);
    }

    /// <inheritdoc />
    public bool TryGetProperty(string name, [MaybeNullWhen(false)] out IPropertyDescriptor property)
    {
        if (name.Equals("this", StringComparison.OrdinalIgnoreCase))
        {
            property = thisDescriptor.Value;
            return true;
        }
        return TryGetMember(name, out property);
    }

    private bool TryGetMember<T>(string name, [MaybeNullWhen(false)] out T member)
        where T : IMemberDescriptor
    {
        member =
            membersByName.TryGetValue(name, out var lazyMember) && lazyMember.Value is T typedMember
                ? typedMember
                : default;
        return member is not null;
    }
}

file static class MemberListExtensions
{
    public static IReadOnlyDictionary<string, Lazy<TDescriptor>> ToLazyDictionary<T, TDescriptor>(
        this ParallelQuery<T> source,
        Func<T, TDescriptor> descriptorSelector
    )
        where T : MemberInfo
    {
        return source
            .Select(x => (key: x.Name, descriptor: new Lazy<TDescriptor>(() => descriptorSelector(x))))
            .ToDictionary(x => x.key, x => x.descriptor);
    }
}
