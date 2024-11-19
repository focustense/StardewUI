using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using StardewUI.Widgets;

namespace StardewUI.Framework.Descriptors;

/// <summary>
/// View descriptor based on reflection.
/// </summary>
public class ReflectionViewDescriptor : IViewDescriptor
{
    /// <inheritdoc />
    public bool SupportsChangeNotifications => innerDescriptor.SupportsChangeNotifications;

    /// <inheritdoc />
    public Type TargetType => innerDescriptor.TargetType;

    private static readonly ConcurrentDictionary<Type, ReflectionViewDescriptor> cache = [];

    private readonly IPropertyDescriptor? defaultOutletProperty;
    private readonly IReadOnlyDictionary<string, IPropertyDescriptor> namedOutletProperties;
    private readonly IObjectDescriptor innerDescriptor;

    /// <summary>
    /// Derives a view descriptor for a given view type.
    /// </summary>
    /// <param name="viewType">The view type; must be a type implementing <see cref="IView"/>.</param>
    /// <returns>The descriptor for the specified <paramref name="viewType"/>.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="viewType"/> does not implement
    /// <see cref="IView"/>.</exception>
    internal static ReflectionViewDescriptor ForViewType(Type viewType)
    {
        using var _ = Trace.Begin(nameof(ReflectionViewDescriptor), nameof(ForViewType));
        return cache.GetOrAdd(
            viewType,
            static viewType =>
            {
                if (!typeof(IView).IsAssignableFrom(viewType))
                {
                    throw new ArgumentException(
                        $"{viewType.Name} does not implement {typeof(IView).Name}.",
                        nameof(viewType)
                    );
                }
                var innerDescriptor = DescriptorFactory.GetObjectDescriptor(viewType);
                return new(viewType, innerDescriptor);
            }
        );
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

    private ReflectionViewDescriptor(Type viewType, IObjectDescriptor innerDescriptor)
    {
        this.innerDescriptor = innerDescriptor;
        var (defaultOutletProperty, namedOutletProperties) = GetOutletProperties(viewType, innerDescriptor);
        this.defaultOutletProperty = defaultOutletProperty;
        this.namedOutletProperties = namedOutletProperties;
    }

    /// <inheritdoc />
    public bool TryGetChildrenProperty(string? outletName, [MaybeNullWhen(false)] out IPropertyDescriptor property)
    {
        if (!string.IsNullOrEmpty(outletName))
        {
            namedOutletProperties.TryGetValue(outletName, out property);
        }
        else
        {
            property = defaultOutletProperty;
        }
        return property is not null;
    }

    /// <inheritdoc />
    public bool TryGetEvent(string name, [MaybeNullWhen(false)] out IEventDescriptor @event)
    {
        return innerDescriptor.TryGetEvent(name, out @event);
    }

    /// <inheritdoc />
    public bool TryGetMethod(string name, [MaybeNullWhen(false)] out IMethodDescriptor method)
    {
        return innerDescriptor.TryGetMethod(name, out method);
    }

    /// <inheritdoc />
    public bool TryGetProperty(string name, [MaybeNullWhen(false)] out IPropertyDescriptor property)
    {
        return innerDescriptor.TryGetProperty(name, out property);
    }

    private static (
        IPropertyDescriptor? defaultOutlet,
        IReadOnlyDictionary<string, IPropertyDescriptor> namedOutlets
    ) GetOutletProperties(Type viewType, IObjectDescriptor innerDescriptor)
    {
        IPropertyDescriptor? defaultOutlet = null;
        var namedOutlets = new ConcurrentDictionary<string, IPropertyDescriptor>(
            StringComparer.InvariantCultureIgnoreCase
        );
        var properties = viewType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
        if (ReflectionObjectDescriptor.EnableParallelCreation())
        {
            properties
                .AsParallel()
                .WithDegreeOfParallelism(Math.Clamp(Environment.ProcessorCount / 2, 2, 4))
                .Where(IsOutlet)
                .ForAll(AddOutlet);
        }
        else
        {
            foreach (var property in properties)
            {
                if (IsOutlet(property))
                {
                    AddOutlet(property);
                }
            }
        }
        return (defaultOutlet, namedOutlets);

        void AddOutlet(PropertyInfo property)
        {
            var descriptor = innerDescriptor.GetProperty(property.Name);
            var outletName = property.GetCustomAttribute<OutletAttribute>()?.Name;
            if (!string.IsNullOrEmpty(outletName))
            {
                var previousDescriptor = namedOutlets.GetOrAdd(outletName, descriptor);
                if (previousDescriptor != descriptor)
                {
                    throw new DescriptorException(
                        $"Cannot add property '{property.Name}' as outlet '{outletName}' for type {viewType.Name}: "
                            + $"another property ('{previousDescriptor.Name}') has already been assigned to this outlet."
                    );
                }
            }
            else
            {
                if (Interlocked.CompareExchange(ref defaultOutlet, descriptor, null) is not null)
                {
                    throw new DescriptorException(
                        $"Cannot add property '{property.Name}' as the default outlet for type {viewType.Name}: "
                            + $"another property ('{defaultOutlet.Name}') has already been assigned to this outlet."
                    );
                }
            }
        }
    }

    private static bool IsOutlet(PropertyInfo property)
    {
        return property.CanWrite
            && (
                typeof(IView).IsAssignableFrom(property.PropertyType)
                || typeof(IEnumerable<IView>).IsAssignableFrom(property.PropertyType)
            );
    }
}
