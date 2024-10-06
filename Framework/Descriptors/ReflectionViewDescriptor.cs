using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace StardewUI.Framework.Descriptors;

/// <summary>
/// View descriptor based on reflection.
/// </summary>
public class ReflectionViewDescriptor : IViewDescriptor
{
    public Type TargetType => innerDescriptor.TargetType;

    private static readonly Dictionary<Type, ReflectionViewDescriptor> cache = [];

    private readonly IPropertyDescriptor? childrenProperty;
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
        var childrenProperty = viewType
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .FirstOrDefault(prop =>
                prop.CanWrite
                && (
                    typeof(IView).IsAssignableFrom(prop.PropertyType)
                    || typeof(IEnumerable<IView>).IsAssignableFrom(prop.PropertyType)
                )
            );
        this.childrenProperty = childrenProperty is not null
            ? innerDescriptor.GetProperty(childrenProperty.Name)
            : null;
    }

    public bool TryGetChildrenProperty([MaybeNullWhen(false)] out IPropertyDescriptor property)
    {
        property = childrenProperty;
        return childrenProperty is not null;
    }

    public bool TryGetEvent(string name, [MaybeNullWhen(false)] out IEventDescriptor @event)
    {
        return innerDescriptor.TryGetEvent(name, out @event);
    }

    public bool TryGetMethod(string name, [MaybeNullWhen(false)] out IMethodDescriptor method)
    {
        return innerDescriptor.TryGetMethod(name, out method);
    }

    public bool TryGetProperty(string name, [MaybeNullWhen(false)] out IPropertyDescriptor property)
    {
        return innerDescriptor.TryGetProperty(name, out property);
    }
}
