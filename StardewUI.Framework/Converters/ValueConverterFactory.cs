using System.Diagnostics.CodeAnalysis;

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
    /// <returns>A converter that converts from <typeparamref name="TSource"/> to
    /// <typeparamref name="TDestination"/>.</returns>
    IValueConverter<TSource, TDestination> GetConverter<TSource, TDestination>()
    {
        return TryGetConverter<TSource, TDestination>(out var converter)
            ? converter
            : throw new NotImplementedException(
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
}

/// <summary>
/// Standard implementation of <see cref="IValueConverterFactory"/> that allows registering new converters.
/// </summary>
public class ValueConverterFactory : IValueConverterFactory
{
    private readonly Dictionary<(Type, Type), object?> converters = [];
    private readonly List<IValueConverterFactory> factories = [];

    /// <summary>
    /// Initializes a new <see cref="ValueConverterFactory"/> instance.
    /// </summary>
    public ValueConverterFactory()
    {
        // Automatically register string to primitive conversions.
        TryRegister<string, byte>(byte.Parse);
        TryRegister<string, sbyte>(sbyte.Parse);
        TryRegister<string, short>(short.Parse);
        TryRegister<string, ushort>(ushort.Parse);
        TryRegister<string, int>(int.Parse);
        TryRegister<string, uint>(uint.Parse);
        TryRegister<string, long>(long.Parse);
        TryRegister<string, ulong>(ulong.Parse);
        TryRegister<string, decimal>(decimal.Parse);
        TryRegister<string, float>(float.Parse);
        TryRegister<string, double>(double.Parse);

        // Convenience defaults for non-primitive types that are commonly specified as literals.
        TryRegister(new ColorConverter());
        TryRegister(new NamedFontConverter());
        TryRegister(new PointConverter());
        TryRegister(new Vector2Converter());

        // Most enums are fine using the standard string-to-enum conversion.
        Register(new EnumNameConverterFactory());

        // Final fallback for any value, do no conversion if source and destination are same.
        Register(new IdentityValueConverterFactory());
    }

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

    public bool TryGetConverter<TSource, TDestination>(
        [MaybeNullWhen(false)] out IValueConverter<TSource, TDestination> converter
    )
    {
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
