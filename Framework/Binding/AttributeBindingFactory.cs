using System.Collections.Concurrent;
using System.Reflection;
using StardewUI.Framework.Content;
using StardewUI.Framework.Converters;
using StardewUI.Framework.Descriptors;
using StardewUI.Framework.Dom;
using StardewUI.Framework.Grammar;
using StardewUI.Framework.Sources;

namespace StardewUI.Framework.Binding;

/// <summary>
/// Service for creating <see cref="IAttributeBinding"/> instances for the individual attributes of a bound view.
/// </summary>
public interface IAttributeBindingFactory
{
    /// <summary>
    /// Attempts to creates a new attribute binding.
    /// </summary>
    /// <param name="viewDescriptor">Descriptor for the bound view, providing access to its properties.</param>
    /// <param name="attribute">The attribute data.</param>
    /// <param name="context">The binding context, including the bound data and descriptor for the data type.</param>
    /// <param name="resolutionScope">Scope for resolving externalized attributes, such as translation keys.</param>
    /// <returns>The created binding, or <c>null</c> if the arguments do not support creating a binding, such as an
    /// <paramref name="attribute"/> bound to a <c>null</c> value of <paramref name="context"/>.</returns>
    IAttributeBinding? TryCreateBinding(
        IViewDescriptor viewDescriptor,
        IAttribute attribute,
        BindingContext? context,
        IResolutionScope resolutionScope
    );
}

/// <summary>
/// A general <see cref="IAttributeBindingFactory"/> implementation using dependency injection for all resolution.
/// </summary>
/// <param name="valueSourceFactory">The factory responsible for creating <see cref="IValueSource{T}"/> instances from
/// attribute data.</param>
/// <param name="valueConverterFactory">The factory responsible for creating
/// <see cref="IValueConverter{TSource, TDestination}"/> instances, used to convert bound values to the types required
/// by the target view.</param>
public class AttributeBindingFactory(
    IValueSourceFactory valueSourceFactory,
    IValueConverterFactory valueConverterFactory
) : IAttributeBindingFactory
{
    delegate IAttributeBinding LocalBindingFactory(
        IViewDescriptor viewDescriptor,
        IAttribute attribute,
        string propertyName,
        BindingContext? context,
        IResolutionScope resolutionScope
    );

    record AttributeBinding<TSource, TDest>(
        IValueSource<TSource> Source,
        IValueConverter<TSource, TDest> InputConverter,
        IValueConverter<TDest, TSource> OutputConverter,
        IPropertyDescriptor<TDest> Destination,
        BindingDirection Direction
    ) : IAttributeBinding, IDisposable
    {
        public string DestinationPropertyName => Destination.Name;

        public void Dispose()
        {
            if (Source is IDisposable sourceDisposable)
            {
                sourceDisposable.Dispose();
            }
            if (InputConverter is IDisposable inputConverterDisposable)
            {
                inputConverterDisposable.Dispose();
            }
            if (OutputConverter is IDisposable outputConverterDisposable)
            {
                outputConverterDisposable.Dispose();
            }
            GC.SuppressFinalize(this);
        }

        public void UpdateSource(IView target)
        {
            using var _ = Trace.Begin(this, nameof(UpdateSource));
            if (!Destination.CanRead)
            {
                throw new BindingException(
                    $"Cannot read value from non-readable property {Destination.DeclaringType.Name}."
                        + $"{Destination.Name} for writing back to {Source.DisplayName}."
                );
            }
            if (!Source.CanWrite)
            {
                throw new BindingException($"Cannot write a value back to non-writable source {Source.DisplayName}.");
            }
            var destValue = Destination.GetValue(target);
            if (destValue is not null)
            {
                Source.Value = OutputConverter.Convert(destValue);
            }
            else
            {
                Source.Value = default;
            }
        }

        public bool UpdateView(IView target, bool force)
        {
            using var _ = Trace.Begin(this, nameof(UpdateView));
            if (!Source.CanRead)
            {
                throw new BindingException($"Cannot read a value from non-readable source {Source.DisplayName}.");
            }
            if (!Destination.CanWrite)
            {
                throw new BindingException(
                    $"Cannot write a value to non-writable property "
                        + $"{Destination.DeclaringType.Name}.{Destination.Name}."
                );
            }
            var previousValue = Source.Value;
            if (!(Source.Update() || force))
            {
                return false;
            }
            if (Source.Value is not null)
            {
                var destValue = InputConverter.Convert(Source.Value);
                Destination.SetValue(target, destValue);
            }
            else
            {
                Destination.SetValue(target, default!);
            }
            return true;
        }
    }

    private readonly ConcurrentDictionary<(Type, string, Type), LocalBindingFactory> bindingFactoryCache = [];
    private readonly ConcurrentDictionary<(Type, Type), MethodInfo> genericMethodCache = [];

    /// <inheritdoc />
    public IAttributeBinding? TryCreateBinding(
        IViewDescriptor viewDescriptor,
        IAttribute attribute,
        BindingContext? context,
        IResolutionScope resolutionScope
    )
    {
        using var _ = Trace.Begin(this, nameof(TryCreateBinding));
        var propertyName = attribute.Name.AsSpan().ToUpperCamelCase();
        var property = viewDescriptor.GetProperty(propertyName);
        // For literal attributes and asset bindings, the source type will always be the same - either a string, or the
        // target property type, respectively. However, for a context binding, the source type belongs to the context
        // and we can't cache until we know what it is.
        var sourceType = valueSourceFactory.GetValueType(attribute, property, context);
        if (sourceType is null)
        {
            return null;
        }
        var propertyKey = (viewDescriptor.TargetType, attribute.Name, sourceType);
        var bindingFactory = bindingFactoryCache.GetOrAdd(
            propertyKey,
            _ =>
            {
                var typedBindingMethod = typeof(AttributeBindingFactory).GetMethod(
                    nameof(CreateTypedBinding),
                    BindingFlags.NonPublic | BindingFlags.Instance
                )!;
                // MakeGenericMethod can be expensive, so it helps to also cache the method itself, independently of the
                // attribute it's being associated with, in case many attributes use the same types (which they will).
                var typedBindingGenericMethod = genericMethodCache.GetOrAdd(
                    (sourceType, property.ValueType),
                    _ => typedBindingMethod.MakeGenericMethod(sourceType, property.ValueType)
                );
                return typedBindingGenericMethod.CreateDelegate<LocalBindingFactory>(this);
            }
        );
        return bindingFactory(viewDescriptor, attribute, propertyName, context, resolutionScope);
    }

    private IAttributeBinding CreateTypedBinding<TSource, TDest>(
        IViewDescriptor viewDescriptor,
        IAttribute attribute,
        string propertyName,
        BindingContext? context,
        IResolutionScope resolutionScope
    )
        where TSource : notnull
    {
        using var _ = Trace.Begin(this, nameof(CreateTypedBinding));
        var property = (IPropertyDescriptor<TDest>)viewDescriptor.GetProperty(propertyName);
        var source = valueSourceFactory.GetValueSource<TSource>(attribute, context, resolutionScope);
        var direction = GetBindingDirection(attribute);
        if (direction.IsIn())
        {
            if (!source.CanRead)
            {
                throw new BindingException(
                    $"Cannot create an in/in-out binding from non-readable source {source.DisplayName}."
                );
            }
            else if (!property.CanWrite)
            {
                throw new BindingException(
                    $"Cannot create an in/in-out binding on non-writable destination property "
                        + $"{property.DeclaringType.Name}.{property.Name}."
                );
            }
        }
        if (direction.IsOut())
        {
            if (!source.CanWrite)
            {
                throw new BindingException(
                    $"Cannot create an out/in-out binding from non-writable source {source.DisplayName}."
                );
            }
            else if (!property.CanRead)
            {
                throw new BindingException(
                    $"Cannot create an out/in-out binding on non-readable destination property "
                        + $"{property.DeclaringType.Name}.{property.Name}."
                );
            }
        }
        var inputConverter = direction.IsIn()
            ? valueConverterFactory.GetRequiredConverter<TSource, TDest>()
            : InvalidConverter<TSource, TDest>.Instance;
        var outputConverter = direction.IsOut()
            ? valueConverterFactory.GetRequiredConverter<TDest, TSource>()
            : InvalidConverter<TDest, TSource>.Instance;
        return new AttributeBinding<TSource, TDest>(source, inputConverter, outputConverter, property, direction);
    }

    private static BindingDirection GetBindingDirection(IAttribute attribute)
    {
        return attribute.ValueType switch
        {
            AttributeValueType.OutputBinding => BindingDirection.Out,
            AttributeValueType.TwoWayBinding => BindingDirection.InOut,
            _ => BindingDirection.In,
        };
    }
}
