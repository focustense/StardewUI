using System.Diagnostics.CodeAnalysis;

namespace StardewUI.Framework.Converters;

/// <summary>
/// Factory that automatically implements identity conversions, where the source and destination type are the same.
/// </summary>
/// <remarks>
/// Used for most model and asset bindings.
/// </remarks>
public class IdentityValueConverterFactory : IValueConverterFactory
{
    public bool TryGetConverter<TSource, TDestination>(
        [MaybeNullWhen(false)] out IValueConverter<TSource, TDestination> converter
    )
    {
        converter =
            typeof(TSource) == typeof(TDestination)
                ? (IValueConverter<TSource, TDestination>)IdentityValueConverter<TSource>.Instance
                : null;
        return converter is not null;
    }
}

/// <summary>
/// A pass-through value converter, for when the source and destination types are the same.
/// </summary>
/// <typeparam name="T">The value type.</typeparam>
class IdentityValueConverter<T> : IValueConverter<T, T>
{
    /// <summary>
    /// Global converter instance, per value type.
    /// </summary>
    public static readonly IdentityValueConverter<T> Instance = new();

    private IdentityValueConverter() { }

    public T Convert(T value)
    {
        return value;
    }
}
