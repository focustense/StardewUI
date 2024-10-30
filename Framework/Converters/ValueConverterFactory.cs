﻿using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework;
using StardewUI.Layout;

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
        TryRegister<string, bool>(bool.Parse);

        // Convenience defaults for non-primitive types that are commonly specified as literals.
        TryRegister(new ColorConverter());
        TryRegister<string, Edges>(Edges.Parse);
        TryRegister(new GridItemLayoutConverter());
        TryRegister(new LayoutConverter());
        TryRegister(new NamedFontConverter());
        TryRegister(new PointConverter());
        TryRegister(new RectangleConverter());
        TryRegister(new Vector2Converter());

        // Edges are better to bind as numbers, so we can use tuples and XNA equivalents in some cases.
        TryRegister<int, Edges>(all => new(all));
        TryRegister<Tuple<int, int>, Edges>(t => new(t.Item1, t.Item2));
        TryRegister<Point, Edges>(p => new(p.X, p.Y));
        TryRegister<Vector2, Edges>(v => new((int)v.X, (int)v.Y));
        TryRegister<Tuple<int, int, int, int>, Edges>(t => new(t.Item1, t.Item2, t.Item3, t.Item4));
        TryRegister<Tuple<Point, Point>, Edges>(t => new(t.Item1.X, t.Item1.Y, t.Item2.X, t.Item2.Y));
        TryRegister<Tuple<Vector2, Vector2>, Edges>(t =>
            new((int)t.Item1.X, (int)t.Item1.Y, (int)t.Item2.X, (int)t.Item2.Y)
        );
        TryRegister<Vector4, Edges>(v => new((int)v.X, (int)v.Y, (int)v.Z, (int)v.W));
        // And the reverse conversions, where applicable...
        TryRegister<Edges, Tuple<int, int, int, int>>(e => Tuple.Create(e.Left, e.Top, e.Right, e.Bottom));
        TryRegister<Edges, Vector4>(e => new(e.Left, e.Top, e.Right, e.Bottom));

        // Bounds are similar to edges, except we never accept them as inputs, so only need reverse conversions.
        TryRegister<Bounds, Tuple<float, float, float, float>>(b => Tuple.Create(b.Left, b.Top, b.Right, b.Bottom));
        TryRegister<Bounds, Tuple<Vector2, Vector2>>(b => Tuple.Create(b.Position, b.Size));
        TryRegister<Bounds, Vector4>(b => new(b.Left, b.Top, b.Right, b.Bottom));
        TryRegister<Bounds, Rectangle>(b => new(b.Position.ToPoint(), b.Size.ToPoint()));

        // Several converters for just the Sprite type, as it can be surprisingly complex and users won't have the exact
        // type.
        TryRegister(new ItemSpriteConverter());
        TryRegister(new TextureSpriteConverter());
        TryRegister(new TextureRectSpriteConverter());

        // Most enums are fine using the standard string-to-enum conversion.
        Register(new EnumNameConverterFactory());

        // If source and destination are the same, use a pass-through converter.
        Register(new IdentityValueConverterFactory());
        Register(new AnyCastConverterFactory());
        Register(new CastingValueConverterFactory());
        Register(new NullableConverterFactory(this));

        // Anything can generally be converted to a string using the default converter.
        Register(new StringConverterFactory());
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
