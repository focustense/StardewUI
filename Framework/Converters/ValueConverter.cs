namespace StardewUI.Framework.Converters;

/// <summary>
/// Provides a method to convert between value types.
/// </summary>
/// <typeparam name="TSource">The type of value to be converted.</typeparam>
/// <typeparam name="TDestination">The converted value type.</typeparam>
public interface IValueConverter<TSource, TDestination>
{
    /// <summary>
    /// Converts a value from the source type to the destination type.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The converted value.</returns>
    TDestination Convert(TSource value);
}

/// <summary>
/// Generic delegating converter, accepting a conversion function.
/// </summary>
/// <typeparam name="TSource">The type of value to be converted.</typeparam>
/// <typeparam name="TDestination">The converted value type.</typeparam>
/// <param name="convert">Function to convert a <typeparamref name="TSource"/> to a
/// <typeparamref name="TDestination"/>.</param>
public class ValueConverter<TSource, TDestination>(Func<TSource, TDestination> convert)
    : IValueConverter<TSource, TDestination>
{
    /// <inheritdoc />
    public TDestination Convert(TSource value)
    {
        return convert(value);
    }
}
