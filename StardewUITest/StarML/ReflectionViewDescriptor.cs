using StardewUI;
using System.Diagnostics.CodeAnalysis;

namespace StardewUITest.StarML;

/// <summary>
/// View descriptor based on reflection.
/// </summary>
public class ReflectionViewDescriptor : ReflectionObjectDescriptor, IViewDescriptor
{
    private static readonly Dictionary<Type, ReflectionViewDescriptor> cache = [];

    private readonly IBindingProperty? childrenProperty;

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
                throw new ArgumentException($"{viewType.Name} does not implement {typeof(IView).Name}.", nameof(viewType));
            }
            descriptor = CreateForType(viewType, (type, props) => new ReflectionViewDescriptor(type, props));
            cache[viewType] = descriptor;
        }
        return descriptor;
    }

    private ReflectionViewDescriptor(Type viewType, IReadOnlyDictionary<string, IBindingProperty> propertiesByName)
        : base(viewType, propertiesByName)
    {
        childrenProperty = propertiesByName.Values.FirstOrDefault(prop =>
            prop.CanWrite &&
            (typeof(IView).IsAssignableFrom(prop.ValueType) || typeof(IEnumerable<IView>).IsAssignableFrom(prop.ValueType)));
    }

    public bool TryGetChildrenProperty([MaybeNullWhen(false)] out IBindingProperty property)
    {
        property = childrenProperty;
        return childrenProperty is not null;
    }
}
