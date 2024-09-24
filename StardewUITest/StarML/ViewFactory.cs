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

public class ViewFactory
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

public interface IViewBinder : IDisposable
{
    void Bind(IView view, IElement element, object? context);
    void Update();
}

public interface IValueConverter<TSource, TDest>
{
    TDest Convert(TSource value);
}

public interface IValueSourceFactory
{
    IValueSource<T> GetValueSource<T>(IAttribute attribute, object? context) where T : notnull;

    Type GetValueType(IAttribute attribute, IBindingProperty property);
}

public class ValueSourceFactory(IAssetCache assetCache, IReflectionCache reflection) : IValueSourceFactory
{
    public IValueSource<T> GetValueSource<T>(IAttribute attribute, object? context) where T : notnull
    {
        return attribute.ValueType switch
        {
            AttributeValueType.Literal => (IValueSource<T>)new LiteralSource(attribute.Value),
            AttributeValueType.Binding => attribute.Value.StartsWith('@')
                ? new AssetValueSource<T>(assetCache, attribute.Value[1..])
                : new ContextPropertySource<T>(context, attribute.Value, reflection),
            _ => throw new ArgumentException($"Invalid attribute type {attribute.ValueType}.", nameof(attribute))
        };
    }

    public Type GetValueType(IAttribute attribute, IBindingProperty property)
    {
        return attribute.ValueType switch
        {
            AttributeValueType.Literal => typeof(string),
            AttributeValueType.Binding => property.ValueType,
            _ => throw new ArgumentException($"Invalid attribute type {attribute.ValueType}.", nameof(attribute))
        };
    }
}

public interface IValueSource<T>
{
    T? Value { get; }

    bool Update();
}

public class LiteralSource(string value) : IValueSource<string>
{
    public string Value => value;

    public bool Update()
    {
        return false;
    }
}

public interface IAssetCacheEntry<T>
{
    T Asset { get; }
    bool IsExpired { get; }
}

public interface IAssetCache
{
    IAssetCacheEntry<T> Get<T>(string name) where T : notnull;
}

public class AssetCache : IAssetCache
{
    class ExternalEntry<T>(WeakReference<InternalEntry> entry) : IAssetCacheEntry<T>
    {
        public T Asset { get; } = entry.TryGetTarget(out var target) ? (T)target.Asset! : throw new InvalidOperationException("Cannot access an expired asset.");

        public bool IsExpired => entry.TryGetTarget(out var target) ? target.IsExpired : true;
    }

    class InternalEntry(object asset)
    {
        public object? Asset { get; set; } = asset;

        public bool IsExpired { get; private set; }

        public void Invalidate()
        {
            if (Asset is null)
            {
                return;
            }
            Asset = default;
            IsExpired = true;
        }
    }

    private readonly IGameContentHelper content;
    private readonly Dictionary<string, InternalEntry> entries = [];

    public AssetCache(IGameContentHelper content, IContentEvents events)
    {
        this.content = content;
        events.AssetsInvalidated += Content_AssetsInvalidated;
    }

    public IAssetCacheEntry<T> Get<T>(string name) where T : notnull
    {
        if (!entries.TryGetValue(name, out var entry))
        {
            var asset = content.Load<T>(name);
            entry = new(asset);
            entries.Add(name, entry);
        }
        else
        {
            entry.Asset ??= content.Load<T>(name);
        }
        return new ExternalEntry<T>(new(entry));
    }

    private void Content_AssetsInvalidated(object? sender, AssetsInvalidatedEventArgs e)
    {
        foreach (var name in e.NamesWithoutLocale)
        {
            if (entries.TryGetValue(name.BaseName, out var entry))
            {
                entry.Invalidate();
            }
        }
    }
}

public class AssetValueSource<T> : IValueSource<T>
    where T : notnull
{
    public T? Value => throw new NotImplementedException();

    private readonly IAssetCache cache;
    private readonly string name;

    private IAssetCacheEntry<T>? cacheEntry;

    public AssetValueSource(IAssetCache cache, string name)
    {
        this.cache = cache;
        this.name = name;
    }

    public bool Update()
    {
        if (cacheEntry is null || cacheEntry.IsExpired)
        {
            cacheEntry = cache.Get<T>(name);
            return true;
        }
        return false;
    }
}

public class ContextPropertySource<T> : IValueSource<T>, IDisposable
    where T : notnull
{
    public T? Value
    {
        get => value;
    }

    private readonly object? context;
    private readonly IBindingProperty<T>? property;
    private readonly string propertyName;

    private bool isDirty = true;
    private T? value = default;

    public ContextPropertySource(object? context, string propertyName, IReflectionCache reflection)
    {
        this.context = context;
        this.property = context is not null ? reflection.GetProperty<T>(context.GetType(), propertyName) : null;
        this.propertyName = propertyName;
        if (context is INotifyPropertyChanged npc)
        {
            npc.PropertyChanged += Context_PropertyChanged;
        }
    }

    public void Dispose()
    {
        if (context is INotifyPropertyChanged npc)
        {
            npc.PropertyChanged -= Context_PropertyChanged;
        }
        GC.SuppressFinalize(this);
    }

    private void Context_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == propertyName)
        {
            isDirty = true;
        }
    }

    public bool Update()
    {
        if (!isDirty)
        {
            return false;
        }
        value = property is not null ? property.GetValue(context!) : default;
        isDirty = false;
        return true;
    }
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
    IAttributeBinding CreateBinding(IView view, IAttribute attribute, object? context);
}

public class AttributeBindingFactory(IReflectionCache reflection, IValueSourceFactory valueSourceFactory, IValueConverterFactory valueConverterFactory) : IAttributeBindingFactory
{
    delegate IAttributeBinding LocalBindingFactory(IView view, IAttribute attribute, string propertyName, object? context);

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

    public IAttributeBinding CreateBinding(IView view, IAttribute attribute, object? context)
    {
        var propertyKey = (view.GetType(), attribute.Name);
        // TODO: Cache propertyName with the binding factory
        var propertyName = GetPropertyName(attribute.Name);
        if (!cache.TryGetValue(propertyKey, out var bindingFactory))
        {
            var property = reflection.GetProperty(view.GetType(), propertyName);
            if (!property.CanWrite)
            {
                throw new BindingException($"Cannot bind to non-writable property '{propertyName}' of type {view.GetType().Name}.");
            }
            var typedBindingMethod = typeof(AttributeBindingFactory).GetMethod(nameof(CreateTypedBinding), BindingFlags.NonPublic | BindingFlags.Instance)!;
            var sourceType = valueSourceFactory.GetValueType(attribute, property);
            var typedBindingGenericMethod = typedBindingMethod.MakeGenericMethod(sourceType, property.ValueType);
            bindingFactory = typedBindingGenericMethod.CreateDelegate<LocalBindingFactory>(this);
            cache.Add(propertyKey, bindingFactory);
        }
        return bindingFactory(view, attribute, propertyName, context);
    }

    private IAttributeBinding CreateTypedBinding<TSource, TDest>(IView view, IAttribute attribute, string propertyName, object? context)
        where TSource : notnull
    {
        var property = reflection.GetProperty<TDest>(view.GetType(), propertyName);
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
    private readonly List<IAttributeBinding> bindings = [];

    private WeakReference<IView>? boundViewRef;

    public void Bind(IView view, IElement element, object? context)
    {
        boundViewRef = new(view);
        ClearBindings();
        foreach (var attribute in element.Attributes)
        {
            var binding = attributeBindingFactory.CreateBinding(view, attribute, context);
            bindings.Add(binding);
            // Initial forced update since some binding types (e.g. literals) never have updates.
            binding.Update(view, force: true);
        }
    }

    public void Dispose()
    {
        ClearBindings();
        boundViewRef = null;
        GC.SuppressFinalize(this);
    }

    public void Update()
    {
        if (boundViewRef is null)
        {
            return;
        }
        if (!boundViewRef.TryGetTarget(out var view))
        {
            boundViewRef = null;
            return;
        }
        foreach (var binding in bindings)
        {
            binding.Update(view);
        }
    }

    private void ClearBindings()
    {
        foreach (var binding in bindings)
        {
            binding.Dispose();
        }
        bindings.Clear();
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