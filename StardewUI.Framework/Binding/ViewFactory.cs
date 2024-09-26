using System.Globalization;
using System.Reflection;
using System.Text;
using StardewUI.Framework.Converters;
using StardewUI.Framework.Descriptors;
using StardewUI.Framework.Grammar;
using StardewUI.Framework.Sources;

namespace StardewUI.Framework.Binding;

public interface IAttribute
{
    string Name { get; }
    AttributeValueType ValueType { get; }
    string Value { get; }
}

public record SAttribute(string Name, AttributeValueType ValueType, string Value) : IAttribute;

public interface IElement
{
    string Tag { get; }
    IReadOnlyList<IAttribute> Attributes { get; }
}

public record SElement(string Tag, IReadOnlyList<SAttribute> Attributes) : IElement
{
    IReadOnlyList<IAttribute> IElement.Attributes => Attributes;
}

public interface IViewFactory
{
    IView CreateView(string tagName);
}

public class ViewFactory : IViewFactory
{
    public IView CreateView(string tagName)
    {
        return tagName.ToLowerInvariant() switch
        {
            "banner" => new Banner(),
            "button" => new Button(),
            "checkbox" => new CheckBox(),
            // TODO: Can we handle drop-down lists of different types by looking at the attributes?
            "dropdownlist" => new DropDownList<string>(),
            "expander" => new Expander(),
            "frame" => new Frame(),
            "grid" => new Grid(),
            "image" => new Image(),
            "label" => new Label(),
            "lane" => new Lane(),
            "marquee" => new Marquee(),
            "panel" => new Panel(),
            "scrollableview" => new ScrollableView(),
            "slider" => new Slider(),
            "spacer" => new Spacer(),
            "textinput" => new TextInput(),
            //"tinynumberlabel" => new TinyNumberLabel(),
            _ => throw new ArgumentException($"Unsupported view type: {tagName}", nameof(tagName)),
        };
    }
}

public interface IViewBinder
{
    IViewBinding Bind(IView view, IElement element, object? data);

    IViewDescriptor GetDescriptor(IView view);
}

public interface IViewBinding : IDisposable
{
    bool Update();
}

class ViewBinding(IView view, IReadOnlyList<IAttributeBinding> attributeBindings) : IViewBinding
{
    private readonly WeakReference<IView> viewRef = new(view);

    private bool isDisposed;

    public void Dispose()
    {
        foreach (var binding in attributeBindings)
        {
            binding.Dispose();
        }
        isDisposed = true;
        GC.SuppressFinalize(this);
    }

    public bool Update()
    {
        if (isDisposed)
        {
            throw new ObjectDisposedException(nameof(ViewBinding));
        }
        if (!viewRef.TryGetTarget(out var view))
        {
            return false;
        }
        bool anyChanged = false;
        foreach (var binding in attributeBindings)
        {
            anyChanged |= binding.Update(view);
        }
        return anyChanged;
    }
}

public interface IAttributeBinding : IDisposable
{
    bool Update(IView target, bool force = false);
}

public interface IAttributeBindingFactory
{
    IAttributeBinding CreateBinding(IViewDescriptor viewDescriptor, IAttribute attribute, BindingContext? context);
}

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
        IValueConverter<TSource, TDest> Converter,
        IPropertyDescriptor<TDest> Destination
    ) : IAttributeBinding, IDisposable
    {
        public void Dispose()
        {
            if (Source is IDisposable sourceDisposable)
            {
                sourceDisposable.Dispose();
            }
            if (Converter is IDisposable converterDisposable)
            {
                converterDisposable.Dispose();
            }
            GC.SuppressFinalize(this);
        }

        public bool Update(IView target, bool force)
        {
            if (!(Source.Update() || force))
            {
                return false;
            }
            if (Source.Value is not null)
            {
                var destValue = Converter.Convert(Source.Value);
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

    private readonly Dictionary<(Type, string), LocalBindingFactory> cache = [];

    public IAttributeBinding CreateBinding(
        IViewDescriptor viewDescriptor,
        IAttribute attribute,
        BindingContext? context
    )
    {
        var propertyKey = (viewDescriptor.TargetType, attribute.Name);
        // TODO: Cache propertyName with the binding factory
        var propertyName = GetPropertyName(attribute.Name);
        if (!cache.TryGetValue(propertyKey, out var bindingFactory))
        {
            var property = viewDescriptor.GetProperty(propertyName);
            if (!property.CanWrite)
            {
                throw new BindingException(
                    $"Cannot bind to non-writable property '{propertyName}' of type {viewDescriptor.TargetType.Name}."
                );
            }
            var typedBindingMethod = typeof(AttributeBindingFactory).GetMethod(
                nameof(CreateTypedBinding),
                BindingFlags.NonPublic | BindingFlags.Instance
            )!;
            var sourceType = valueSourceFactory.GetValueType(attribute, property, context);
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
        var converter = valueConverterFactory.GetConverter<TSource, TDest>();
        return new AttributeBinding<TSource, TDest>(source, converter, property);
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

public class ReflectionViewBinder(IAttributeBindingFactory attributeBindingFactory) : IViewBinder
{
    public IViewBinding Bind(IView view, IElement element, object? data)
    {
        var viewDescriptor = GetDescriptor(view);
        var context = data is not null ? BindingContext.Create(data) : null;
        var attributeBindings = element
            .Attributes.Select(attribute => attributeBindingFactory.CreateBinding(viewDescriptor, attribute, context))
            .ToList();
        // Initial forced update since some binding types (e.g. literals) never have updates.
        foreach (var attributeBinding in attributeBindings)
        {
            attributeBinding.Update(view, force: true);
        }
        var viewBinding = new ViewBinding(view, attributeBindings);
        return viewBinding;
    }

    public IViewDescriptor GetDescriptor(IView view)
    {
        return ReflectionViewDescriptor.ForViewType(view.GetType());
    }
}
