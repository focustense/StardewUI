using System.Diagnostics.CodeAnalysis;

namespace StardewUI.Framework.Converters;

/// <summary>
/// Factory that automatically implements casting conversions, where the source type can be assigned directly to the
/// destination type.
/// </summary>
public class CastingValueConverterFactory : IValueConverterFactory
{
    public bool TryGetConverter<TSource, TDestination>(
        [MaybeNullWhen(false)] out IValueConverter<TSource, TDestination> converter
    )
    {
        converter = typeof(TDestination).IsAssignableFrom(typeof(TSource))
            ? (IValueConverter<TSource, TDestination>)CastingValueConverter<TSource, TDestination>.Instance
            : null;
        return converter is not null;
    }
}

/// <summary>
/// A value converter that performs a direct cast; used when source and destination types are not exactly the same, but
/// the source type is directly assignable to the destination type.
/// </summary>
/// <typeparam name="TSource">The type of value to be converted.</typeparam>
/// <typeparam name="TDestination">The converted value type.</typeparam>
class CastingValueConverter<TSource, TDestination> : IValueConverter<TSource, TDestination>
{
    /// <summary>
    /// Global converter instance, per value type.
    /// </summary>
    public static readonly CastingValueConverter<TSource, TDestination> Instance = new();

    private CastingValueConverter() { }

    public TDestination Convert(TSource value)
    {
        return (TDestination)(object)value!;
    }
}
