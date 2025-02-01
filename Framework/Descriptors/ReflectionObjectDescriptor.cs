using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace StardewUI.Framework.Descriptors;

/// <summary>
/// Object descriptor based on reflection.
/// </summary>
public class ReflectionObjectDescriptor : IObjectDescriptor
{
    /// <summary>
    /// Gets or sets the function used to choose whether to create member descriptors in parallel.
    /// </summary>
    /// <remarks>
    /// This is typically linked to the mod configuration by the mod entry.
    /// </remarks>
    internal static Func<bool> EnableParallelCreation { get; set; } = () => false;

    /// <inheritdoc />
    public IEnumerable<string> MemberNames => membersByName.Keys;

    /// <inheritdoc />
    public bool SupportsChangeNotifications { get; }

    /// <inheritdoc />
    public Type TargetType { get; }

    private static readonly ConcurrentDictionary<Type, ReflectionObjectDescriptor> cache = [];

    private readonly IReadOnlyDictionary<string, MemberEntry> membersByName;
    private readonly Lazy<IPropertyDescriptor> thisDescriptor;

    /// <summary>
    /// Creates or retrieves a descriptor for a given object type.
    /// </summary>
    /// <param name="type">The object type.</param>
    /// <param name="lazy">Whether to use lazy initialization for individual members. Lazy member initialization can
    /// speed up initial descriptor creation time at the cost of slower initial access times for the members, which
    /// typically slows down bind times. This parameter is ignored for already-cached descriptors.</param>
    /// <returns>The descriptor for the specified <paramref name="type"/>.</returns>
    internal static ReflectionObjectDescriptor ForType(Type type, bool lazy = false)
    {
        return cache.GetOrAdd(type, _ => CreateDescriptor(type, lazy));
    }

    /// <summary>
    /// Invalidates a cached descriptor, removing its type from the cache.
    /// </summary>
    /// <remarks>
    /// This method is designed only to be invoked from an application metadata update (.NET Hot Reload, as opposed to
    /// StardewUI's own hot reload based on assets) and should never be called by user code.
    /// </remarks>
    /// <param name="type">The type to invalidate.</param>
    /// <returns><c>true</c> if the type was invalidated; <c>false</c> if no descriptor was cached.</returns>
    internal static bool Invalidate(Type type)
    {
        return cache.TryRemove(type, out _);
    }

    private static ReflectionObjectDescriptor CreateDescriptor(Type type, bool lazy)
    {
        using var _ = Trace.Begin(nameof(ReflectionObjectDescriptor), nameof(CreateDescriptor));
        var interfaces = type.GetInterfaces();
        var members = type.GetMembers(BindingFlags.Instance | BindingFlags.Public);
        var membersByName = EnableParallelCreation()
            ? members
                .AsParallel()
                .WithDegreeOfParallelism(Math.Clamp(Environment.ProcessorCount / 2, 2, 4))
                .Where(IsMemberSupported)
                .Distinct(MemberNameComparer.Instance)
                .ToLazyDictionary(CreateMemberDescriptor, lazy)
            : members
                .Where(IsMemberSupported)
                .Distinct(MemberNameComparer.Instance)
                .ToLazyDictionary(CreateMemberDescriptor, lazy);
        return new(type, interfaces, membersByName);
    }

    private static IMemberDescriptor CreateMemberDescriptor(MemberInfo member)
    {
        return member switch
        {
            FieldInfo field => LazyExpressionFieldDescriptor.FromFieldInfo(field),
            PropertyInfo prop => ReflectionPropertyDescriptor.FromPropertyInfo(prop),
            MethodInfo method => ReflectionMethodDescriptor.FromMethodInfo(method),
            EventInfo ev => ReflectionEventDescriptor.FromEventInfo(ev),
            _ => throw new DescriptorException(
                $"Invalid member type {member.MemberType} for descriptor of "
                    + $"{member.DeclaringType?.Name}.{member.Name}"
            ),
        };
    }

    private static bool IsMemberSupported(MemberInfo member)
    {
        return member switch
        {
            FieldInfo field => true,
            PropertyInfo prop => prop.GetIndexParameters().Length == 0,
            MethodInfo method => ReflectionMethodDescriptor.IsSupported(method),
            EventInfo ev => ReflectionEventDescriptor.IsSupported(ev),
            _ => false,
        };
    }

    /// <summary>
    /// Initializes a new <see cref="ReflectionObjectDescriptor"/> with the given target type and members.
    /// </summary>
    /// <param name="type">The <see cref="TargetType"/>.</param>
    /// <param name="interfaces">All interfaces implemented by the <see cref="TargetType"/>.</param>
    /// <param name="membersByName">Dictionary of member names to the corresponding member descriptors.</param>
    private ReflectionObjectDescriptor(
        Type type,
        IReadOnlyList<Type> interfaces,
        IReadOnlyDictionary<string, MemberEntry> membersByName
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
        return TryGetMember(name, MemberTypes.Event, out @event);
    }

    /// <inheritdoc />
    public bool TryGetMethod(string name, [MaybeNullWhen(false)] out IMethodDescriptor method)
    {
        return TryGetMember(name, MemberTypes.Method, out method);
    }

    /// <inheritdoc />
    public bool TryGetProperty(string name, [MaybeNullWhen(false)] out IPropertyDescriptor property)
    {
        if (name.Equals("this", StringComparison.OrdinalIgnoreCase))
        {
            property = thisDescriptor.Value;
            return true;
        }
        return TryGetMember(name, MemberTypes.Field | MemberTypes.Property, out property);
    }

    private bool TryGetMember<T>(string name, MemberTypes memberTypes, [MaybeNullWhen(false)] out T member)
        where T : IMemberDescriptor
    {
        member =
            membersByName.TryGetValue(name, out var entry)
            && (entry.MemberType & memberTypes) != 0
            && entry.Descriptor.Value is T typedMember
                ? typedMember
                : default;
        return member is not null;
    }
}

file static class MemberListExtensions
{
    public static IReadOnlyDictionary<string, MemberEntry> ToLazyDictionary<T>(
        this IEnumerable<T> source,
        Func<T, IMemberDescriptor> descriptorSelector,
        bool lazy
    )
        where T : MemberInfo
    {
        return source
            .Select(x =>
                (
                    key: x.Name,
                    type: x.MemberType,
                    descriptor: lazy
                        ? new Lazy<IMemberDescriptor>(() => descriptorSelector(x))
                        : new(descriptorSelector(x))
                )
            )
            .ToDictionary(x => x.key, x => new MemberEntry(x.type, x.descriptor));
    }

    public static IReadOnlyDictionary<string, MemberEntry> ToLazyDictionary<T>(
        this ParallelQuery<T> source,
        Func<T, IMemberDescriptor> descriptorSelector,
        bool lazy
    )
        where T : MemberInfo
    {
        return source
            .Select(x =>
                (
                    key: x.Name,
                    type: x.MemberType,
                    descriptor: lazy
                        ? new Lazy<IMemberDescriptor>(() => descriptorSelector(x))
                        : new(descriptorSelector(x))
                )
            )
            .ToDictionary(x => x.key, x => new MemberEntry(x.type, x.descriptor));
    }
}

internal record MemberEntry(MemberTypes MemberType, Lazy<IMemberDescriptor> Descriptor);

file class MemberNameComparer : IEqualityComparer<MemberInfo>
{
    public static MemberNameComparer Instance = new();

    public bool Equals(MemberInfo? x, MemberInfo? y)
    {
        return string.Equals(x?.Name, y?.Name);
    }

    public int GetHashCode([DisallowNull] MemberInfo obj)
    {
        return obj.Name.GetHashCode();
    }
}
