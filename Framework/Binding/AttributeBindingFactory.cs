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

    class AttributeBinding<TSource, TDest>(
        IValueSource<TSource> source,
        IValueConverter<TSource, TDest> inputConverter,
        IValueConverter<TDest, TSource> outputConverter,
        IPropertyDescriptor<TDest> destination,
        BindingDirection direction
    ) : IAttributeBinding, IDisposable
    {
        public string DestinationPropertyName => destination.Name;

        public BindingDirection Direction => direction;

        private TDest? lastBoundValue;

        public void Dispose()
        {
            if (source is IDisposable sourceDisposable)
            {
                sourceDisposable.Dispose();
            }
            if (inputConverter is IDisposable inputConverterDisposable)
            {
                inputConverterDisposable.Dispose();
            }
            if (outputConverter is IDisposable outputConverterDisposable)
            {
                outputConverterDisposable.Dispose();
            }
            GC.SuppressFinalize(this);
        }

        public object? GetBoundValue()
        {
            return lastBoundValue;
        }

        public void UpdateSource(IView target)
        {
            using var _ = Trace.Begin(this, nameof(UpdateSource));
            if (!destination.CanRead)
            {
                throw new BindingException(
                    $"Cannot read value from non-readable property {destination.DeclaringType.Name}."
                        + $"{destination.Name} for writing back to {source.DisplayName}."
                );
            }
            if (!source.CanWrite)
            {
                throw new BindingException($"Cannot write a value back to non-writable source {source.DisplayName}.");
            }
            var destValue = destination.GetValue(target);
            lastBoundValue = destValue;
            if (destValue is not null)
            {
                source.Value = outputConverter.Convert(destValue);
            }
            else
            {
                source.Value = default;
            }
        }

        public bool UpdateView(IView target, bool force)
        {
            using var _ = Trace.Begin(this, nameof(UpdateView));
            if (!source.CanRead)
            {
                throw new BindingException($"Cannot read a value from non-readable source {source.DisplayName}.");
            }
            if (!destination.CanWrite)
            {
                throw new BindingException(
                    $"Cannot write a value to non-writable property "
                        + $"{destination.DeclaringType.Name}.{destination.Name}."
                );
            }
            var previousValue = source.Value;
            if (!(source.Update() || force))
            {
                return false;
            }
            if (source.Value is not null)
            {
                var destValue = inputConverter.Convert(source.Value);
                destination.SetValue(target, destValue);
                lastBoundValue = destValue;
            }
            else
            {
                destination.SetValue(target, default!);
                lastBoundValue = default;
            }
            return true;
        }
    }

    private static readonly MethodInfo typedBindingMethod = typeof(AttributeBindingFactory).GetMethod(
        nameof(CreateTypedBinding),
        BindingFlags.NonPublic | BindingFlags.Instance
    )!;

    private readonly ConcurrentDictionary<(Type, string, Type), LocalBindingFactory> bindingFactoryCache = [];
    private readonly ConcurrentDictionary<(Type, Type), LocalBindingFactory> createTypedBindingMethodCache = new();

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
            _ => GetLocalBindingFactory(sourceType, property.ValueType)
        );
        return bindingFactory(viewDescriptor, attribute, propertyName, context, resolutionScope);
    }

    /// <summary>
    /// Prepares the reflection cache for a future binding from the specified source type to destination type.
    /// </summary>
    /// <remarks>
    /// This does not actually create a binding and is safe to call during startup. This overload is preferred over
    /// <see cref="Warmup(Type, Type)"/> when possible, as it does not use any reflection, and can be called from the
    /// main thread.
    /// </remarks>
    /// <typeparam name="TSource">The type of the source (e.g. attribute/context) value.</typeparam>
    /// <typeparam name="TDestination">The type of the destination (e.g. view) value.</typeparam>
    internal void Warmup<TSource, TDestination>()
    {
        createTypedBindingMethodCache.TryAdd(
            (typeof(TSource), typeof(TDestination)),
            CreateTypedBinding<TSource, TDestination>
        );
    }

    /// <summary>
    /// Prepares the reflection cache for a future binding from the specified source type to destination type.
    /// </summary>
    /// <param name="sourceType">The type of the source (e.g. attribute/context) value.</param>
    /// <param name="destinationType">The type of the destination (e.g. view) value.</param>
    internal void Warmup(Type sourceType, Type destinationType)
    {
        GetLocalBindingFactory(sourceType, destinationType);
    }

    private IAttributeBinding CreateTypedBinding<TSource, TDest>(
        IViewDescriptor viewDescriptor,
        IAttribute attribute,
        string propertyName,
        BindingContext? context,
        IResolutionScope resolutionScope
    )
    {
        if (attribute.ValueType == AttributeValueType.TemplateBinding)
        {
            throw new BindingException(
                $"Template binding attribute '{attribute}' is invalid here; template bindings may only be used from "
                    + "within a <template> node."
            );
        }
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

    private LocalBindingFactory GetLocalBindingFactory(Type sourceType, Type destinationType)
    {
        // MakeGenericMethod can be expensive, so it helps to also cache the method itself, independently of the
        // attribute it's being associated with, in case many attributes use the same types (which they will).
        return createTypedBindingMethodCache.GetOrAdd(
            (sourceType, destinationType),
            _ =>
            {
                using var _trace = Trace.Begin(this, nameof(GetLocalBindingFactory));
                return typedBindingMethod
                    .MakeGenericMethod(sourceType, destinationType)
                    .CreateDelegate<LocalBindingFactory>(this);
            }
        );
    }
}
