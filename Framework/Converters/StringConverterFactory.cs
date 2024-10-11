using System.Diagnostics.CodeAnalysis;

namespace StardewUI.Framework.Converters;

/// <summary>
/// Factory that provides
/// </summary>
public class StringConverterFactory : IValueConverterFactory
{
    /// <inheritdoc />
    public bool TryGetConverter<TSource, TDestination>(
        [MaybeNullWhen(false)] out IValueConverter<TSource, TDestination> converter
    )
    {
        if (typeof(TDestination) == typeof(string))
        {
            converter = (IValueConverter<TSource, TDestination>)Converter<TSource>.Instance;
            return true;
        }
        converter = null;
        return false;
    }

    class Converter<T> : IValueConverter<T, string>
    {
        /// <summary>
        /// Global converter instance, per input type.
        /// </summary>
        public static readonly Converter<T> Instance = new();

        public string Convert(T value)
        {
            return value?.ToString() ?? "";
        }
    }
}
