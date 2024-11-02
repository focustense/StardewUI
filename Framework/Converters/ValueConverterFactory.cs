using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace StardewUI.Framework.Converters;

/// <summary>
/// Factory for obtaining instance of <see cref="IValueConverter{TSource, TDestination}"/>.
/// </summary>
public interface IValueConverterFactory
{
    /// <summary>
    /// Gets a converter from a given source type to a given destination type.
    /// </summary>
    /// <typeparam name="TSource">The type of value to be converted.</typeparam>
    /// <typeparam name="TDestination">The converted value type.</typeparam>
    /// <returns>A converter that converts from <typeparamref name="TSource"/> to <typeparamref name="TDestination"/>,
    /// or <c>null</c> if the conversion is not supported.</returns>
    IValueConverter<TSource, TDestination>? GetConverter<TSource, TDestination>()
    {
        return TryGetConverter<TSource, TDestination>(out var converter)
            ? converter
            : throw new NotSupportedException(
                $"No value converter registered for {typeof(TSource).Name} -> {typeof(TDestination).Name}."
            );
    }

    /// <summary>
    /// Gets a converter from a given source type to a given destination type, throwing if the conversion is not
    /// supported.
    /// </summary>
    /// <typeparam name="TSource">The type of value to be converted.</typeparam>
    /// <typeparam name="TDestination">The converted value type.</typeparam>
    /// <returns>A converter that converts from <typeparamref name="TSource"/> to
    /// <typeparamref name="TDestination"/>.</returns>
    /// <exception cref="NotSupportedException">Thrown when there is no registered converter that supports conversion
    /// from <typeparamref name="TSource"/> to <typeparamref name="TDestination"/>.</exception>
    IValueConverter<TSource, TDestination> GetRequiredConverter<TSource, TDestination>()
    {
        return TryGetConverter<TSource, TDestination>(out var converter)
            ? converter
            : throw new NotSupportedException(
                $"No value converter registered for {typeof(TSource).Name} -> {typeof(TDestination).Name}."
            );
    }

    /// <summary>
    /// Attempts to obtain a converter from a given source type to a given destination type.
    /// </summary>
    /// <typeparam name="TSource">The type of value to be converted.</typeparam>
    /// <typeparam name="TDestination">The converted value type.</typeparam>
    /// <param name="converter">If the method returns <c>true</c>, holds the converter that converts between the
    /// specified types; otherwise <c>null</c>.</param>
    /// <returns><c>true</c> if the conversion is supported, otherwise <c>false</c>.</returns>
    bool TryGetConverter<TSource, TDestination>(
        [MaybeNullWhen(false)] out IValueConverter<TSource, TDestination> converter
    );

    /// <summary>
    /// Attempts to obtain a converter from a given source type to a given destination type.
    /// </summary>
    /// <param name="sourceType">The type of value to be converted.</param>
    /// <param name="destinationType">The converted value type.</param>
    /// <param name="converter">If the method returns <c>true</c>, holds the converter that converts between the
    /// specified types; otherwise <c>null</c>.</param>
    /// <returns><c>true</c> if the conversion is supported, otherwise <c>false</c>.</returns>
    bool TryGetConverter(Type sourceType, Type destinationType, [MaybeNullWhen(false)] out IValueConverter converter)
    {
        object?[] parameters = [null];
        object? result = ValueConverterFactoryHelpers
            .TryGetConverterMethodDef.MakeGenericMethod(sourceType, destinationType)
            .Invoke(this, parameters);
        converter = parameters[0] as IValueConverter;
        return (bool)result!;
    }
}

file static class ValueConverterFactoryHelpers
{
    public static readonly MethodInfo TryGetConverterMethodDef = typeof(IValueConverterFactory).GetMethod(
        nameof(IValueConverterFactory.TryGetConverter),
        2,
        [
            typeof(IValueConverter<,>)
                .MakeGenericType(Type.MakeGenericMethodParameter(0), Type.MakeGenericMethodParameter(1))
                .MakeByRefType(),
        ]
    )!;
}
