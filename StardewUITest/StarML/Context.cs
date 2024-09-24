namespace StardewUITest.StarML;

/// <summary>
/// Context, or scope, of a bound view, providing the backing data and tools for accessing its properties.
/// </summary>
/// <param name="Descriptor">Descriptor of the <see cref="Data"/> type, used to read current values.</param>
/// <param name="Data">The bound data.</param>
public record Context(IObjectDescriptor Descriptor, object Data)
{
    /// <summary>
    /// Creates a <see cref="Context"/> from the specified data, automatically building a new descriptor if the data
    /// type has not been previously seen.
    /// </summary>
    /// <param name="data">The bound data.</param>
    /// <returns>A new <see cref="Context"/> whose <see cref="Data"/> is the specified <paramref name="data"/> and whose
    /// <see cref="Descriptor"/> is the descriptor of <paramref name="data"/>'s runtime type.</returns>
    public static Context Create(object data)
    {
        var descriptor = ReflectionObjectDescriptor.ForType(data.GetType());
        return new(descriptor, data);
    }
}