namespace StardewUI.Framework.Converters;

/// <summary>
/// Represents a conversion that is not supported or not allowed.
/// </summary>
/// <remarks>
/// This type is generally not used except as a placeholder when something requires an
/// <see cref="IValueConverter{TSource, TDestination}"/> but no conversion is supposed to occur, e.g. the input
/// converter on an out binding or vice versa.
/// </remarks>
internal class InvalidConverter<TSource, TDestination> : IValueConverter<TSource, TDestination>
{
    /// <summary>
    /// The converter instance for the specified conversion.
    /// </summary>
    public static readonly InvalidConverter<TSource, TDestination> Instance = new();

    public TDestination Convert(TSource value)
    {
        throw new NotImplementedException();
    }
}
