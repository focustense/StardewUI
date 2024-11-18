using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace StardewUI.Framework.Converters;

/// <summary>
/// Factory that implements automatic conversion between nullable and non-nullable types.
/// </summary>
/// <param name="innerFactory">The converter factory to handle conversion of the element type(s).</param>
public class NullableConverterFactory(IValueConverterFactory innerFactory) : IValueConverterFactory
{
    /// <summary>
    /// Pre-initializes some reflection state in order to make future invocations faster.
    /// </summary>
    internal static void Warmup()
    {
        typeof(NullableToNullableConverter<,>).MakeGenericType(typeof(int), typeof(int));
        typeof(NonNullableToNullableConverter<,>).MakeGenericType(typeof(int), typeof(int));
        typeof(NullableToNonNullableConverter<,>).MakeGenericType(typeof(int), typeof(int));
    }

    /// <inheritdoc />
    public bool TryGetConverter<TSource, TDestination>(
        [MaybeNullWhen(false)] out IValueConverter<TSource, TDestination> converter
    )
    {
        var sourceElementType = GetNullableElementType(typeof(TSource));
        var destinationElementType = GetNullableElementType(typeof(TDestination));
        if (sourceElementType is null && destinationElementType is null)
        {
            converter = null;
            return false;
        }
        var converterDefinition = sourceElementType is not null
            ? destinationElementType is not null
                ? typeof(NullableToNullableConverter<,>)
                : typeof(NullableToNonNullableConverter<,>)
            : typeof(NonNullableToNullableConverter<,>);
        var converterType = converterDefinition.MakeGenericType(
            sourceElementType ?? typeof(TSource),
            destinationElementType ?? typeof(TDestination)
        );
        var factoryMethod = converterType.GetMethod("TryCreate", BindingFlags.Public | BindingFlags.Static)!;
        converter = (IValueConverter<TSource, TDestination>?)factoryMethod.Invoke(null, [innerFactory]);
        return converter is not null;
    }

    private static Type? GetNullableElementType(Type type)
    {
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)
            ? type.GetGenericArguments()[0]
            : null;
    }

    class NullableToNonNullableConverter<TSource, TDestination>(IValueConverter<TSource, TDestination> innerConverter)
        : IValueConverter<TSource?, TDestination>
        where TSource : struct
    {
        public static NullableToNonNullableConverter<TSource, TDestination>? TryCreate(IValueConverterFactory factory)
        {
            return factory.TryGetConverter<TSource, TDestination>(out var converter) ? new(converter) : null;
        }

        public TDestination Convert(TSource? source)
        {
            return source.HasValue ? innerConverter.Convert(source.Value) : default!;
        }
    }

    class NullableToNullableConverter<TSource, TDestination>(IValueConverter<TSource, TDestination> innerConverter)
        : IValueConverter<TSource?, TDestination?>
        where TSource : struct
        where TDestination : struct
    {
        public static NullableToNullableConverter<TSource, TDestination>? TryCreate(IValueConverterFactory factory)
        {
            return factory.TryGetConverter<TSource, TDestination>(out var converter) ? new(converter) : null;
        }

        public TDestination? Convert(TSource? source)
        {
            return source.HasValue ? innerConverter.Convert(source.Value) : null;
        }
    }

    class NonNullableToNullableConverter<TSource, TDestination>(IValueConverter<TSource, TDestination> innerConverter)
        : IValueConverter<TSource, TDestination?>
        where TDestination : struct
    {
        public static NonNullableToNullableConverter<TSource, TDestination>? TryCreate(IValueConverterFactory factory)
        {
            return factory.TryGetConverter<TSource, TDestination>(out var converter) ? new(converter) : null;
        }

        public TDestination? Convert(TSource source)
        {
            return innerConverter.Convert(source);
        }
    }
}
