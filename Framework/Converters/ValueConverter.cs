namespace StardewUI.Framework.Converters;

/// <summary>
/// Provides a method to convert between arbitrary types.
/// </summary>
/// <remarks>
/// This is a non-generic version of the <see cref="IValueConverter{TSource, TDestination}"/> that should normally only
/// be used by framework code. Avoid implementing this directly; instead prefer the generic version, which implicitly
/// implements this interface.
/// </remarks>
public interface IValueConverter
{
    /// <summary>
    /// The type of object that this converts to; the result type of the <see cref="Convert"/> method.
    /// </summary>
    Type DestinationType { get; }

    /// <summary>
    /// The type of object this converts from; the <c>value</c> argument to the <see cref="Convert"/> method.
    /// </summary>
    Type SourceType { get; }

    /// <summary>
    /// Converts a value from the <see cref="SourceType"/> to the <see cref="DestinationType"/>.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The converted value.</returns>
    /// <exception cref="InvalidCastException">Thrown when <paramref name="value"/> is not an instance of
    /// <see cref="SourceType"/>.</exception>
    object? Convert(object value);
}

/// <summary>
/// Provides a method to convert between value types.
/// </summary>
/// <typeparam name="TSource">The type of value to be converted.</typeparam>
/// <typeparam name="TDestination">The converted value type.</typeparam>
public interface IValueConverter<in TSource, out TDestination> : IValueConverter
{
    Type IValueConverter.DestinationType => typeof(TDestination);

    Type IValueConverter.SourceType => typeof(TSource);

    /// <summary>
    /// Converts a value from the source type to the destination type.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The converted value.</returns>
    TDestination Convert(TSource value);

    object? IValueConverter.Convert(object value)
    {
        return Convert((TSource)value);
    }
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
