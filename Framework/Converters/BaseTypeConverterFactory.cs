using System.Diagnostics.CodeAnalysis;

namespace StardewUI.Framework.Converters;

/// <summary>
/// Allows implicit conversion from a type's ancestor to the destination type, if the source type does not have its own
/// explicitly-defined conversion but a base type does.
/// </summary>
/// <param name="innerFactory">The converter factory to handle conversion of ancestor types.</param>
public class BaseTypeConverterFactory(IValueConverterFactory innerFactory) : IValueConverterFactory
{
    /// <inheritdoc />
    public bool TryGetConverter<TSource, TDestination>(
        [MaybeNullWhen(false)] out IValueConverter<TSource, TDestination> converter
    )
    {
        for (var baseType = typeof(TSource).BaseType; IsValidType(baseType); baseType = baseType.BaseType)
        {
            if (innerFactory.TryGetConverter(baseType, typeof(TDestination), out var baseConverter))
            {
                converter = (IValueConverter<TSource, TDestination>)baseConverter;
                return true;
            }
        }
        converter = null;
        return false;
    }

    private static bool IsValidType([NotNullWhen(true)] Type? type)
    {
        return type is not null && type != typeof(object) && type != typeof(ValueType);
    }
}
