using System.Globalization;
using System.Reflection;
using System.Text;
using StardewUI.Framework.Converters;
using StardewUI.Framework.Descriptors;
using StardewUI.Framework.Dom;
using StardewUI.Framework.Sources;

namespace StardewUI.Framework.Binding;

/// <summary>
/// Service for creating <see cref="IAttributeBinding"/> instances for the individual attributes of a bound view.
/// </summary>
public interface IAttributeBindingFactory
{
    /// <summary>
    /// Creates a new attribute binding.
    /// </summary>
    /// <param name="viewDescriptor">Descriptor for the bound view, providing access to its properties.</param>
    /// <param name="attribute">The attribute data.</param>
    /// <param name="context">The binding context, including the bound data and descriptor for the data type.</param>
    IAttributeBinding CreateBinding(IViewDescriptor viewDescriptor, IAttribute attribute, BindingContext? context);
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
        BindingContext? context
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
                if (destValue is not null)
                {
                    Source.Value = OutputConverter.Convert(destValue);
                }
                else
                {
                    Source.Value = default;
                }
            }
        }

        public bool UpdateView(IView target, bool force)
        {
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

    private static readonly Rune DASH = new('-');

    private readonly Dictionary<(Type, string, Type), LocalBindingFactory> cache = [];

    public IAttributeBinding CreateBinding(
        IViewDescriptor viewDescriptor,
        IAttribute attribute,
        BindingContext? context
    )
    {
        var propertyName = GetPropertyName(attribute.Name);
        var property = viewDescriptor.GetProperty(propertyName);
        // For literal attributes and asset bindings, the source type will always be the same - either a string, or the
        // target property type, respectively. However, for a context binding, the source type belongs to the context
        // and we can't cache until we know what it is.
        var sourceType = valueSourceFactory.GetValueType(attribute, property, context);
        var propertyKey = (viewDescriptor.TargetType, attribute.Name, sourceType);
        if (!cache.TryGetValue(propertyKey, out var bindingFactory))
        {
            var typedBindingMethod = typeof(AttributeBindingFactory).GetMethod(
                nameof(CreateTypedBinding),
                BindingFlags.NonPublic | BindingFlags.Instance
            )!;
            var typedBindingGenericMethod = typedBindingMethod.MakeGenericMethod(sourceType, property.ValueType);
            bindingFactory = typedBindingGenericMethod.CreateDelegate<LocalBindingFactory>(this);
            cache.Add(propertyKey, bindingFactory);
        }
        return bindingFactory(viewDescriptor, attribute, propertyName, context);
    }

    private IAttributeBinding CreateTypedBinding<TSource, TDest>(
        IViewDescriptor viewDescriptor,
        IAttribute attribute,
        string propertyName,
        BindingContext context
    )
        where TSource : notnull
    {
        var property = (IPropertyDescriptor<TDest>)viewDescriptor.GetProperty(propertyName);
        var source = valueSourceFactory.GetValueSource<TSource>(attribute, context);
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
            ? valueConverterFactory.GetConverter<TSource, TDest>()
            : InvalidConverter<TSource, TDest>.Instance;
        var outputConverter = direction.IsOut()
            ? valueConverterFactory.GetConverter<TDest, TSource>()
            : InvalidConverter<TDest, TSource>.Instance;
        return new AttributeBinding<TSource, TDest>(source, inputConverter, outputConverter, property, direction);
    }

    private static BindingDirection GetBindingDirection(IAttribute attribute)
    {
        return attribute.ValueType switch
        {
            Grammar.AttributeValueType.OutputBinding => BindingDirection.Out,
            Grammar.AttributeValueType.TwoWayBinding => BindingDirection.InOut,
            _ => BindingDirection.In,
        };
    }

    private static string GetPropertyName(ReadOnlySpan<char> attributeName)
    {
        var sb = new StringBuilder(attributeName.Length);
        bool capitalizeNext = true;
        foreach (var rune in attributeName.EnumerateRunes())
        {
            if (rune == DASH)
            {
                capitalizeNext = true;
                continue;
            }
            if (capitalizeNext)
            {
                sb.Append(Rune.ToUpper(rune, CultureInfo.CurrentUICulture));
                capitalizeNext = false;
            }
            else
            {
                sb.Append(rune);
            }
        }
        return sb.ToString();
    }
}
