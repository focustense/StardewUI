using StardewUI.Framework.Descriptors;

namespace StardewUI.Framework.Binding;

/// <summary>
/// View defaults that provide the current data-bound values for any bound attributes/properties and fall back to the
/// original ("blank") view defaults for unbound properties.
/// </summary>
/// <param name="original">The original or reference defaults for the managed view type.</param>
/// <param name="attributes">The bound property attributes for the view's node.</param>
public class BoundViewDefaults(IViewDefaults original, IEnumerable<IAttributeBinding> attributes) : IViewDefaults
{
    private readonly Dictionary<string, IAttributeBinding> attributes = attributes.ToDictionary(
        attr => attr.DestinationPropertyName,
        StringComparer.OrdinalIgnoreCase
    );

    /// <inheritdoc />
    public T GetDefaultValue<T>(string propertyName)
    {
        return attributes.TryGetValue(propertyName, out var binding)
            ? (T)binding.GetBoundValue()!
            : original.GetDefaultValue<T>(propertyName);
    }
}
