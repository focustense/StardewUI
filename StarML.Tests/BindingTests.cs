using Microsoft.Xna.Framework;
using StardewUI;
using StardewUITest.StarML;
using System.ComponentModel;
using Xunit.Abstractions;

namespace StarML.Tests;

public class BindingTests(ITestOutputHelper output)
{
    class Model
    {
        public Color Color { get; set; }

        public string Name { get; set; } = "";
    }

    class ModelWithNotify : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public Color Color
        {
            get => color;
            set
            {
                if (value == color)
                {
                    return;
                }
                color = value;
                PropertyChanged?.Invoke(this, new(nameof(Color)));
            }
        }

        public string Name
        {
            get => name;
            set
            {
                if (value == name)
                {
                    return;
                }
                name = value;
                PropertyChanged?.Invoke(this, new(nameof(Name)));
            }
        }

        private Color color;
        private string name = "";
    }

    class ValueConverterFactory : IValueConverterFactory
    {
        public IValueConverter<TSource, TDest> GetConverter<TSource, TDest>()
        {
            if (typeof(TSource) == typeof(TDest))
            {
                return (IValueConverter<TSource, TDest>)IdentityValueConverter<TSource>.Instance;
            }
            else if (typeof(TSource) == typeof(string) && typeof(TDest) == typeof(int))
            {
                return (IValueConverter<TSource, TDest>)new ValueConverter<string, int>(int.Parse);
            }
            throw new NotImplementedException("Value converter not registered.");
        }
    }

    class IdentityValueConverter<T> : IValueConverter<T, T>
    {
        public static readonly IdentityValueConverter<T> Instance = new();

        private IdentityValueConverter() { }

        public T Convert(T value)
        {
            return value;
        }
    }

    class ValueConverter<TSource, TDest>(Func<TSource, TDest> convert) : IValueConverter<TSource, TDest>
    {
        public TDest Convert(TSource value)
        {
            return convert(value);
        }
    }

    class FakeAssetCache : IAssetCache
    {
        public IAssetCacheEntry<T> Get<T>(string name) where T : notnull
        {
            throw new NotImplementedException();
        }
    }

    [Fact]
    public void TestBindings()
    {
        var viewFactory = new ViewFactory();
        var reflectionCache = new ReflectionCache();
        var assetCache = new FakeAssetCache();
        var valueSourceFactory = new ValueSourceFactory(assetCache, reflectionCache);
        var valueConverterFactory = new ValueConverterFactory();
        var attributeBindingFactory = new AttributeBindingFactory(reflectionCache, valueSourceFactory, valueConverterFactory);
        var viewBinder = new ReflectionViewBinder(attributeBindingFactory);

        var element = new SElement("label", [
            new SAttribute("max-lines", AttributeValueType.Literal, "1"),
            new SAttribute("color", AttributeValueType.Binding, "Color"),
            new SAttribute("text", AttributeValueType.Binding, "Name"),
        ]);
        var view = viewFactory.CreateView(element.Tag);
        var model = new ModelWithNotify() { Name = "Test text", Color = Color.Blue };
        viewBinder.Bind(view, element, model);

        var label = (Label)view;
        Assert.Equal(1, label.MaxLines);
        Assert.Equal("Test text", label.Text);
        Assert.Equal(Color.Blue, label.Color);

        model.Name = "New text";
        viewBinder.Update();
        Assert.Equal("New text", label.Text);
    }
}