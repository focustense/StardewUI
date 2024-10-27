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

    private static readonly Dictionary<Type, ReflectionViewDescriptor> cache = [];

    private readonly IPropertyDescriptor? defaultOutletProperty;
    private readonly Dictionary<string, IPropertyDescriptor> namedOutletProperties =
        new(StringComparer.InvariantCultureIgnoreCase);
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
        if (!cache.TryGetValue(viewType, out var descriptor))
        {
            if (!typeof(IView).IsAssignableFrom(viewType))
            {
                throw new ArgumentException(
                    $"{viewType.Name} does not implement {typeof(IView).Name}.",
                    nameof(viewType)
                );
            }
            var innerDescriptor = ReflectionObjectDescriptor.ForType(viewType);
            descriptor = new(viewType, innerDescriptor);
            cache[viewType] = descriptor;
        }
        return descriptor;
    }

    private ReflectionViewDescriptor(Type viewType, IObjectDescriptor innerDescriptor)
    {
        this.innerDescriptor = innerDescriptor;
        var childrenProperties = viewType
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(prop =>
                prop.CanWrite
                && (
                    typeof(IView).IsAssignableFrom(prop.PropertyType)
                    || typeof(IEnumerable<IView>).IsAssignableFrom(prop.PropertyType)
                )
            );
        foreach (var property in childrenProperties)
        {
            var descriptor = innerDescriptor.GetProperty(property.Name);
            var outletName = property.GetCustomAttribute<OutletAttribute>()?.Name;
            if (!string.IsNullOrEmpty(outletName))
            {
                if (namedOutletProperties.TryGetValue(outletName, out var previousDescriptor))
                {
                    throw new DescriptorException(
                        $"Cannot add property '{property.Name}' as outlet '{outletName}' for type {viewType.Name}: "
                            + $"another property ('{previousDescriptor.Name}') has already been assigned to this outlet."
                    );
                }
                namedOutletProperties.Add(outletName, descriptor);
            }
            else
            {
                if (defaultOutletProperty is not null)
                {
                    throw new DescriptorException(
                        $"Cannot add property '{property.Name}' as the default outlet for type {viewType.Name}: "
                            + $"another property ('{defaultOutletProperty.Name}') has already been assigned to this outlet."
                    );
                }
                defaultOutletProperty = descriptor;
            }
        }
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
}
