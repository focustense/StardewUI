using System.Collections.Concurrent;
using System.Reflection;
using StardewUI.Framework.Converters;

namespace StardewUI.Framework.Sources;

/// <summary>
/// Helpers for creating instances of the generic <see cref="ConvertedValueSource{TSource, T}"/> when some of the types
/// are unknown at compile time.
/// </summary>
public static class ConvertedValueSource
{
    delegate IValueSource CreateValueSourceDelegate(
        IValueSource original,
        IValueConverterFactory converterFactory,
        bool twoWay
    );

    private static readonly MethodInfo createInternalMethod = typeof(ConvertedValueSource).GetMethod(
        nameof(CreateInternal),
        BindingFlags.Static | BindingFlags.NonPublic
    )!;
    private static readonly ConcurrentDictionary<(Type, Type), CreateValueSourceDelegate> creationCache = [];

    /// <summary>
    /// Creates a converted source with a specified output type, using an original source with unknown value type.
    /// </summary>
    /// <param name="original">The original value source.</param>
    /// <param name="destinationType">The type to convert to.</param>
    /// <param name="converterFactory">Factory for creating instances of
    /// <see cref="IValueConverter{TSource, TDestination}"/>.</param>
    /// <param name="twoWay">Whether the resulting <see cref="IValueSource"/> should be able to convert in the reverse
    /// direction, i.e. for two-way bindings, by setting <see cref="IValueSource.Value"/>.</param>
    public static IValueSource Create(
        IValueSource original,
        Type destinationType,
        IValueConverterFactory converterFactory,
        bool twoWay = false
    )
    {
        var creator = GetCreatorDelegate(original.ValueType, destinationType);
        return creator(original, converterFactory, twoWay);
    }

    /// <summary>
    /// Creates a converted source with a specified output type, using an original source with unknown value type.
    /// </summary>
    /// <typeparam name="T">The converted value type.</typeparam>
    /// <param name="original">The original value source.</param>
    /// <param name="converterFactory">Factory for creating instances of
    /// <see cref="IValueConverter{TSource, TDestination}"/>.</param>
    /// <param name="twoWay">Whether the resulting <see cref="IValueSource"/> should be able to convert in the reverse
    /// direction, i.e. for two-way bindings, by setting <see cref="IValueSource.Value"/>.</param>
    public static IValueSource<T> Create<T>(
        IValueSource original,
        IValueConverterFactory converterFactory,
        bool twoWay = false
    )
    {
        return (IValueSource<T>)Create(original, typeof(T), converterFactory, twoWay);
    }

    /// <summary>
    /// Pre-initializes some reflection state in order to make future invocations faster.
    /// </summary>
    /// <typeparam name="TSource">The type of the original value.</typeparam>
    /// <typeparam name="TDestination">The converted value type.</typeparam>
    internal static void Warmup<TSource, TDestination>()
    {
        creationCache.TryAdd((typeof(TSource), typeof(TDestination)), CreateInternal<TSource, TDestination>);
    }

    /// <summary>
    /// Pre-initializes some reflection state in order to make future invocations faster.
    /// </summary>
    /// <param name="sourceType">The type of the original value.</param>
    /// <param name="destinationType">The converted value type.</param>
    internal static void Warmup(Type sourceType, Type destinationType)
    {
        GetCreatorDelegate(sourceType, destinationType);
    }

    private static IValueSource CreateInternal<TSource, T>(
        IValueSource original,
        IValueConverterFactory converterFactory,
        bool twoWay
    )
    {
        var inputConverter = converterFactory.GetConverter<TSource, T>();
        var outputConverter = twoWay ? converterFactory.GetConverter<T, TSource>() : null;
        return new ConvertedValueSource<TSource, T>((IValueSource<TSource>)original, inputConverter, outputConverter);
    }

    private static CreateValueSourceDelegate GetCreatorDelegate(Type sourceType, Type destinationType)
    {
        var typeKey = (sourceType, destinationType);
        return creationCache.GetOrAdd(
            typeKey,
            _ =>
                createInternalMethod
                    .MakeGenericMethod(sourceType, destinationType)
                    .CreateDelegate<CreateValueSourceDelegate>()
        );
    }
}

/// <summary>
/// A value source that wraps another <see cref="IValueSource{T}"/> and performs automatic conversion.
/// </summary>
/// <typeparam name="TSource">The original value type, i.e. of the <paramref name="source"/>.</typeparam>
/// <typeparam name="T">The converted value type.</typeparam>
/// <param name="source">The original value source.</param>
/// <param name="inputConverter">A converter that converts from <typeparamref name="TSource"/> to
/// <typeparamref name="T"/>. If this is <c>null</c>, then this instance's <see cref="Value"/> will always be
/// <c>null</c> and <see cref="CanRead"/> will be <c>false</c> regardless of the underlying <paramref name="source"/>'s
/// readability.</param>
/// <param name="outputConverter">A converter that converts from <typeparamref name="T"/> to
/// <typeparamref name="TSource"/>. If this is <c>null</c>, then this instance cannot accept any assignments to the
/// <see cref="Value"/> property, and <see cref="CanWrite"/> will always be <c>false</c> regardless of the underlying
/// <paramref name="source"/>'s writability.</param>
public class ConvertedValueSource<TSource, T>(
    IValueSource<TSource> source,
    IValueConverter<TSource, T>? inputConverter,
    IValueConverter<T, TSource>? outputConverter
) : IValueSource<T>
{
    /// <inheritdoc />
    public bool CanRead => source.CanRead && inputConverter is not null;

    /// <inheritdoc />
    public bool CanWrite => source.CanWrite && outputConverter is not null;

    /// <inheritdoc />
    public string DisplayName => $"Convert({source.DisplayName} -> {typeof(T).Name})";

    /// <inheritdoc />
    public T? Value
    {
        get => value;
        set
        {
            if (!EqualityComparer<T>.Default.Equals(value, this.value))
            {
                this.value = value;
                if (outputConverter is null)
                {
                    throw new NotSupportedException(
                        $"Conversion from {typeof(TSource).Name} to {typeof(T).Name} is not available."
                    );
                }
                source.Value = value is not null ? outputConverter.Convert(value) : default;
            }
        }
    }

    object? IValueSource.Value
    {
        get => Value;
        set => Value = value is not null ? (T)value : default;
    }

    /// <inheritdoc />
    public Type ValueType => typeof(T);

    private T? value = ReadValue(source, inputConverter);

    /// <inheritdoc />
    public bool Update(bool force = false)
    {
        if (source.Update(force))
        {
            value = ReadValue(source, inputConverter);
            return true;
        }
        return false;
    }

    private static T? ReadValue(IValueSource<TSource> source, IValueConverter<TSource, T>? converter)
    {
        return source.CanRead && source.Value is not null && converter is not null
            ? converter.Convert(source.Value)
            : default;
    }
}
