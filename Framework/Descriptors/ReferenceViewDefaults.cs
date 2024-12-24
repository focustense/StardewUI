namespace StardewUI.Framework.Descriptors;

/// <summary>
/// View defaults based on a reference view.
/// </summary>
/// <remarks>
/// References views are dummy views created explicitly for the purposes of deriving default values, and never displayed
/// or written to.
/// </remarks>
/// <param name="view">The reference view whose properties are set to the assumed defaults.</param>
public class ReferenceViewDefaults(IView view) : IViewDefaults
{
    private readonly IViewDescriptor descriptor = DescriptorFactory.GetViewDescriptor(view.GetType());

    /// <inheritdoc />
    public T GetDefaultValue<T>(string propertyName)
    {
        var property = (IPropertyDescriptor<T>)descriptor.GetProperty(propertyName);
        return property.GetValue(view);
    }
}
