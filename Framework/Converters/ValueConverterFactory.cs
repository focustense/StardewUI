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

/// <summary>
/// Standard implementation of <see cref="IValueConverterFactory"/> that allows registering new converters.
/// </summary>
public class ValueConverterFactory : IValueConverterFactory
{
    /// <summary>
    /// The list of factories currently registered.
    /// </summary>
    protected List<IValueConverterFactory> Factories => factories;

    private readonly Dictionary<(Type, Type), IValueConverter?> converters = [];
    private readonly List<IValueConverterFactory> factories = [];

    /// <summary>
    /// Initializes a new <see cref="ValueConverterFactory"/> instance.
    /// </summary>
    public ValueConverterFactory() { }

    /// <summary>
    /// Registers a delegate factory that may be used to obtain a converter for which there is no explicit registration.
    /// </summary>
    /// <remarks>
    /// Use when a single converter may handle many input or output types, e.g. string-to-enum conversions.
    /// </remarks>
    /// <param name="factory">The delegate factory.</param>
    public void Register(IValueConverterFactory factory)
    {
        factories.Add(factory);
    }

    /// <inheritdoc />
    public bool TryGetConverter(
        Type sourceType,
        Type destinationType,
        [MaybeNullWhen(false)] out IValueConverter converter
    )
    {
        using var _ = Trace.Begin(this, nameof(TryGetConverter));
        var key = (sourceType, destinationType);
        if (converters.TryGetValue(key, out var cached))
        {
            converter = cached;
        }
        else
        {
            converter = factories
                .Select(factory => factory.TryGetConverter(sourceType, destinationType, out var inner) ? inner : null)
                .Where(converter => converter is not null)
                .FirstOrDefault();
            converters[key] = converter;
        }
        return converter is not null;
    }

    /// <inheritdoc />
    public bool TryGetConverter<TSource, TDestination>(
        [MaybeNullWhen(false)] out IValueConverter<TSource, TDestination> converter
    )
    {
        using var _ = Trace.Begin(this, nameof(TryGetConverter));
        var key = (typeof(TSource), typeof(TDestination));
        if (converters.TryGetValue(key, out var cached))
        {
            converter = cached as IValueConverter<TSource, TDestination>;
        }
        else
        {
            converter = factories
                .Select(factory => factory.TryGetConverter<TSource, TDestination>(out var inner) ? inner : null)
                .Where(converter => converter is not null)
                .FirstOrDefault();
            converters[key] = converter;
        }
        return converter is not null;
    }

    /// <summary>
    /// Attempts to register a new converter.
    /// </summary>
    /// <typeparam name="TSource">The type of value to be converted.</typeparam>
    /// <typeparam name="TDestination">The converted value type.</typeparam>
    /// <param name="converter">The converter that handles this conversion.</param>
    /// <returns><c>true</c> if the <paramref name="converter"/> was registered for the specified types; <c>false</c> if
    /// there was already a registration or cached converter for those types.</returns>
    public bool TryRegister<TSource, TDestination>(IValueConverter<TSource, TDestination> converter)
    {
        return converters.TryAdd((typeof(TSource), typeof(TDestination)), converter);
    }

    /// <summary>
    /// Attempts to register a new converter.
    /// </summary>
    /// <typeparam name="TSource">The type of value to be converted.</typeparam>
    /// <typeparam name="TDestination">The converted value type.</typeparam>
    /// <param name="convert">Function to convert from <typeparamref name="TSource"/> to
    /// <typeparamref name="TDestination"/>.</param>
    /// <returns><c>true</c> if the conversion function was registered for the specified types; <c>false</c> if there
    /// was already a registration or cached converter for those types.</returns>
    public bool TryRegister<TSource, TDestination>(Func<TSource, TDestination> convert)
    {
        var key = (typeof(TSource), typeof(TDestination));
        var converter = new ValueConverter<TSource, TDestination>(convert);
        return converters.TryAdd(key, converter);
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
