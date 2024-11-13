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
    public static ReflectionViewDescriptor ForViewType(Type viewType)
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
                var innerDescriptor = ReflectionObjectDescriptor.ForType(viewType);
                return new(viewType, innerDescriptor);
            }
        );
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
        viewType
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .AsParallel()
            .Where(prop =>
                prop.CanWrite
                && (
                    typeof(IView).IsAssignableFrom(prop.PropertyType)
                    || typeof(IEnumerable<IView>).IsAssignableFrom(prop.PropertyType)
                )
            )
            .ForAll(property =>
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
            });
        return (defaultOutlet, namedOutlets);
    }
}
