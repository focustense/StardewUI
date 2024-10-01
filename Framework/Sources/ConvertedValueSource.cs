using System.Reflection;
using StardewUI.Framework.Converters;

namespace StardewUI.Framework.Sources;

/// <summary>
/// Helpers for creating instances of the generic <see cref="ConvertedValueSource{TSource, T}"/> when some of the types
/// are unknown at compile time.
/// </summary>
public static class ConvertedValueSource
{
    delegate IValueSource CreateValueSourceDelegate(IValueSource original, IValueConverterFactory converterFactory);

    private static readonly MethodInfo createInternalMethod = typeof(ConvertedValueSource).GetMethod(
        nameof(CreateInternal),
        BindingFlags.Static | BindingFlags.NonPublic
    )!;
    private static readonly Dictionary<(Type, Type), CreateValueSourceDelegate> creationCache = [];

    /// <summary>
    /// Creates a converted source with a specified output type, using an original source with unknown value type.
    /// </summary>
    /// <typeparam name="T">The converted value type.</typeparam>
    /// <param name="original">The original value source.</param>
    /// <param name="converterFactory">Factory for creating instances of
    /// <see cref="IValueConverter{TSource, TDestination}"/>.</param>
    public static IValueSource<T> Create<T>(IValueSource original, IValueConverterFactory converterFactory)
    {
        var typeKey = (original.ValueType, typeof(T));
        if (!creationCache.TryGetValue(typeKey, out var creator))
        {
            creator = createInternalMethod
                .MakeGenericMethod(original.ValueType, typeof(T))
                .CreateDelegate<CreateValueSourceDelegate>();
            creationCache.Add(typeKey, creator);
        }
        return (IValueSource<T>)creator(original, converterFactory);
    }

    private static IValueSource CreateInternal<TSource, T>(
        IValueSource original,
        IValueConverterFactory converterFactory
    )
    {
        var inputConverter = converterFactory.GetConverter<TSource, T>();
        var outputConverter = converterFactory.GetConverter<T, TSource>();
        return new ConvertedValueSource<TSource, T>((IValueSource<TSource>)original, inputConverter, outputConverter);
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
    public bool CanRead => source.CanRead && inputConverter is not null;

    public bool CanWrite => source.CanWrite && outputConverter is not null;

    public string DisplayName => $"Convert({source.DisplayName} -> {typeof(T).Name})";

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

    public Type ValueType => typeof(T);

    private T? value = ReadValue(source, inputConverter);

    public bool Update()
    {
        if (source.Update())
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
