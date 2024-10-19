using System.Diagnostics.CodeAnalysis;

namespace StardewUI.Framework.Converters;

/// <summary>
/// Factory supporting conversions to and from <see cref="IAnyCast"/>.
/// </summary>
public class AnyCastConverterFactory : IValueConverterFactory
{
    /// <inheritdoc />
    public bool TryGetConverter<TSource, TDestination>(
        [MaybeNullWhen(false)] out IValueConverter<TSource, TDestination> converter
    )
    {
        converter = null;
        if (typeof(TDestination) == typeof(IAnyCast))
        {
            converter = (IValueConverter<TSource, TDestination>)ToAnyCastConverter<TSource>.Instance;
        }
        else if (typeof(TSource) == typeof(IAnyCast))
        {
            converter = (IValueConverter<TSource, TDestination>)FromAnyCastConverter<TDestination>.Instance;
        }
        return converter is not null;
    }
}

class FromAnyCastConverter<TDestination> : IValueConverter<IAnyCast, TDestination>
{
    /// <summary>
    /// Global converter instance, per value type.
    /// </summary>
    public static readonly FromAnyCastConverter<TDestination> Instance = new();

    public TDestination Convert(IAnyCast value)
    {
        return (TDestination)value.Value!;
    }
}

class ToAnyCastConverter<TSource> : IValueConverter<TSource, IAnyCast>
{
    /// <summary>
    /// Global converter instance, per value type.
    /// </summary>
    public static readonly ToAnyCastConverter<TSource> Instance = new();

    public IAnyCast Convert(TSource value)
    {
        return new AnyCastValue(value);
    }
}
