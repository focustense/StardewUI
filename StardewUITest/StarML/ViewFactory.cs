using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewUI;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace StardewUITest.StarML;

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
            "marquee" => new Marquee(),
            "panel" => new Panel(),
            "scrollableview" => new ScrollableView(),
            "slider" => new Slider(),
            "spacer" => new Spacer(),
            "textinput" => new TextInput(),
            //"tinynumberlabel" => new TinyNumberLabel(),
            _ => throw new ArgumentException($"Unsupported view type: {tagName}", nameof(tagName))
        };
    }
}

public interface IViewBinder
{
    IViewBinding Bind(IView view, IElement element, object? data);
}

public interface IViewBinding : IDisposable
{
    void Update();
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

    public void Update()
    {
        if (isDisposed)
        {
            throw new ObjectDisposedException(nameof(ViewBinding));
        }
        if (!viewRef.TryGetTarget(out var view))
        {
            return;
        }
        foreach (var binding in attributeBindings)
        {
            binding.Update(view);
        }
    }
}

public interface IValueConverter<TSource, TDest>
{
    TDest Convert(TSource value);
}

public interface IValueConverterFactory
{
    IValueConverter<TSource, TDest> GetConverter<TSource, TDest>();
}

public interface IAttributeBinding : IDisposable
{
    void Update(IView target, bool force = false);
}

public interface IAttributeBindingFactory
{
    IAttributeBinding CreateBinding(IViewDescriptor viewDescriptor, IAttribute attribute, Context? context);
}

public class AttributeBindingFactory(IValueSourceFactory valueSourceFactory, IValueConverterFactory valueConverterFactory) : IAttributeBindingFactory
{
    delegate IAttributeBinding LocalBindingFactory(IViewDescriptor viewDescriptor, IAttribute attribute, string propertyName, Context? context);

    record AttributeBinding<TSource, TDest>(IValueSource<TSource> Source, IValueConverter<TSource, TDest> Converter, IBindingProperty<TDest> Destination) : IAttributeBinding, IDisposable
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

        public void Update(IView target, bool force)
        {
            if ((Source.Update() || force) && Source.Value is not null)
            {
                var destValue = Converter.Convert(Source.Value);
                Destination.SetValue(target, destValue);
            }
        }
    }

    private static readonly Rune DASH = new('-');

    private readonly Dictionary<(Type, string), LocalBindingFactory> cache = [];

    public IAttributeBinding CreateBinding(IViewDescriptor viewDescriptor, IAttribute attribute, Context? context)
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
                    $"Cannot bind to non-writable property '{propertyName}' of type {viewDescriptor.TargetType.Name}.");
            }
            var typedBindingMethod = typeof(AttributeBindingFactory).GetMethod(nameof(CreateTypedBinding), BindingFlags.NonPublic | BindingFlags.Instance)!;
            var sourceType = valueSourceFactory.GetValueType(attribute, property, context);
            var typedBindingGenericMethod = typedBindingMethod.MakeGenericMethod(sourceType, property.ValueType);
            bindingFactory = typedBindingGenericMethod.CreateDelegate<LocalBindingFactory>(this);
            cache.Add(propertyKey, bindingFactory);
        }
        return bindingFactory(viewDescriptor, attribute, propertyName, context);
    }

    private IAttributeBinding CreateTypedBinding<TSource, TDest>(IViewDescriptor viewDescriptor, IAttribute attribute, string propertyName, Context context)
        where TSource : notnull
    {
        var property = (IBindingProperty<TDest>)viewDescriptor.GetProperty(propertyName);
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
        var viewDescriptor = ReflectionViewDescriptor.ForViewType(view.GetType());
        var context = data is not null ? Context.Create(data) : null;
        var attributeBindings = element.Attributes
            .Select(attribute => attributeBindingFactory.CreateBinding(viewDescriptor, attribute, context))
            .ToList();
        // Initial forced update since some binding types (e.g. literals) never have updates.
        foreach (var attributeBinding in attributeBindings)
        {
            attributeBinding.Update(view, force: true);
        }
        var viewBinding = new ViewBinding(view, attributeBindings);
        return viewBinding;
    }
}

public class BindingException : Exception
{
    public BindingException()
    {
    }

    public BindingException(string? message) : base(message)
    {
    }

    public BindingException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}