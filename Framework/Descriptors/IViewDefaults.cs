namespace StardewUI.Framework.Descriptors;

/// <summary>
/// Provides access to the default values of a view's properties.
/// </summary>
/// <remarks>
/// These defaults are not part of the <see cref="IViewDescriptor"/> or <see cref="IPropertyDescriptor{T}"/> interfaces
/// because they cannot be reliably detected through reflection alone; instead they require support from a source
/// generator, if known at compile time, or a dummy/"blank" instance of the view created at runtime otherwise.
/// </remarks>
public interface IViewDefaults
{
    /// <summary>
    /// Gets the default value for the named property.
    /// </summary>
    /// <typeparam name="T">The property value type.</typeparam>
    /// <param name="propertyName">The property name.</param>
    /// <returns>The default value of the specified property for a newly-created view.</returns>
    T GetDefaultValue<T>(string propertyName);
}
