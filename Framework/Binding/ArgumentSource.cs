using StardewUI.Framework.Converters;
using StardewUI.Framework.Descriptors;
using StardewUI.Framework.Sources;

namespace StardewUI.Framework.Binding;

/// <summary>
/// Provides a method to obtain the value of a single argument to a method call, such as an event handler.
/// </summary>
internal interface IArgumentSource
{
    /// <summary>
    /// Gets the current argument value.
    /// </summary>
    /// <param name="eventArgs">The current event arguments, if being read from an event binding.</param>
    object? GetValue(object? eventArgs);
}

/// <summary>
/// Provides an argument value based on an arbitrary <see cref="IValueSource"/>.
/// </summary>
/// <remarks>
/// Causes the <paramref name="source"/> to update every time the value is read, and therefore is designed to be read
/// relatively infrequently, i.e. only when the method is actually being called.
/// </remarks>
/// <param name="source">The source of the argument value.</param>
internal class BoundArgumentSource(IValueSource source) : IArgumentSource
{
    public object? GetValue(object? _)
    {
        source.Update();
        return source.Value;
    }
}

/// <summary>
/// Helper for creating generic <see cref="EventArgumentSource{TEventArgs, TSource, TDestination}"/> using types known
/// only at runtime.
/// </summary>
internal static class EventArgumentSource
{
    private static readonly Dictionary<(Type, Type, Type), IArgumentSource> cache = [];

    /// <summary>
    /// Creates a new, typed <see cref="EventArgumentSource{TEventArgs, TSource, TDestination}"/>.
    /// </summary>
    /// <param name="eventArgsType">The type of the event argument object, generally deriving from
    /// <see cref="EventArgs"/>.</param>
    /// <param name="destinationType">The type actually needed for the invocation argument, which may be different from
    /// the event property's type.</param>
    /// <param name="propertyDescriptor">Descriptor with the accessor and metadata for the origin property on the
    /// <paramref name="eventArgsType"/>.</param>
    /// <param name="converterFactory">Factory for creating <see cref="IValueConverter{TSource, TDestination}"/>
    /// instances, used to convert values from the event arguments to the <paramref name="destinationType"/>.</param>
    public static IArgumentSource Create(
        Type eventArgsType,
        Type destinationType,
        IPropertyDescriptor propertyDescriptor,
        IValueConverterFactory converterFactory
    )
    {
        using var _ = Trace.Begin(nameof(IArgumentSource), nameof(Create));
        var key = (eventArgsType, propertyDescriptor.ValueType, destinationType);
        if (!cache.TryGetValue(key, out var source))
        {
            source = (IArgumentSource)
                typeof(EventArgumentSource<,,>)
                    .MakeGenericType(eventArgsType, propertyDescriptor.ValueType, destinationType)
                    .GetConstructor([typeof(IPropertyDescriptor), typeof(IValueConverterFactory)])!
                    .Invoke([propertyDescriptor, converterFactory]);
            cache.Add(key, source);
        }
        return source;
    }
}

/// <summary>
/// Provides an argument value to an event handler by reading a property of the event arguments.
/// </summary>
/// <typeparam name="TEventArgs">The type of the event argument object, generally deriving from
/// <see cref="EventArgs"/>.</typeparam>
/// <typeparam name="TSource">Type of property to read from the <typeparamref name="TEventArgs"/> object.</typeparam>
/// <typeparam name="TDestination">The type actually needed for the invocation argument, which may be different from the
/// event property's type.</typeparam>
/// <param name="propertyDescriptor">Descriptor with the accessor and metadata for the origin property on the
/// <typeparamref name="TEventArgs"/> type.</param>
/// <param name="converterFactory">Factory for creating <see cref="IValueConverter{TSource, TDestination}"/>
/// instances, used to convert <typeparamref name="TSource"/> to <typeparamref name="TDestination"/>.</param>
internal class EventArgumentSource<TEventArgs, TSource, TDestination>(
    IPropertyDescriptor propertyDescriptor,
    IValueConverterFactory converterFactory
) : IArgumentSource
{
    private readonly IPropertyDescriptor<TSource> propertyDescriptor = (IPropertyDescriptor<TSource>)propertyDescriptor;
    private readonly IValueConverter<TSource, TDestination> converter = converterFactory.GetRequiredConverter<
        TSource,
        TDestination
    >();

    public object? GetValue(object? eventArgs)
    {
        var sourceValue = eventArgs is not null ? propertyDescriptor.GetValue(eventArgs) : default;
        return sourceValue is not null ? converter.Convert(sourceValue) : null;
    }
}
