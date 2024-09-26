using Microsoft.Xna.Framework;
using StardewUI;
using StardewUI.Framework.Binding;
using StardewUI.Framework.Content;
using StardewUI.Framework.Converters;
using StardewUI.Framework.Dom;
using StardewUI.Framework.Grammar;
using StardewUI.Framework.Sources;
using StardewValley;
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

    class FakeAssetCache : IAssetCache
    {
        private readonly Dictionary<string, object> assets = [];

        public IAssetCacheEntry<T> Get<T>(string name) where T : notnull
        {
            return assets.TryGetValue(name, out var asset)
                ? new FakeAssetCacheEntry<T>((T)asset)
                : throw new KeyNotFoundException($"Asset '{name}' not registered.");
        }

        public void Put<T>(string name, T asset)
            where T : notnull
        {
            assets[name] = asset;
        }
    }

    class FakeAssetCacheEntry<T>(T asset) : IAssetCacheEntry<T>
    {
        public T Asset { get; } = asset;

        public bool IsExpired => false;
    }

    [Fact]
    public void TestBindings()
    {
        var viewFactory = new ViewFactory();
        var assetCache = new FakeAssetCache();
        var valueSourceFactory = new ValueSourceFactory(assetCache);
        var valueConverterFactory = new ValueConverterFactory();
        var attributeBindingFactory = new AttributeBindingFactory(valueSourceFactory, valueConverterFactory);
        var viewBinder = new ReflectionViewBinder(attributeBindingFactory);

        var element = new SElement("label", [
            new SAttribute("max-lines", AttributeValueType.Literal, "1"),
            new SAttribute("color", AttributeValueType.Binding, "Color"),
            new SAttribute("text", AttributeValueType.Binding, "Name"),
        ]);
        var view = viewFactory.CreateView(element.Tag);
        var model = new ModelWithNotify() { Name = "Test text", Color = Color.Blue };
        using var viewBinding = viewBinder.Bind(view, element, model);

        var label = (Label)view;
        Assert.Equal(1, label.MaxLines);
        Assert.Equal("Test text", label.Text);
        Assert.Equal(Color.Blue, label.Color);

        model.Name = "New text";
        viewBinding.Update();
        Assert.Equal("New text", label.Text);
    }

    [Fact]
    public void TestNodes()
    {
        var viewFactory = new ViewFactory();
        var assetCache = new FakeAssetCache();
        assetCache.Put("TestSprite", UiSprites.ButtonDark);
        var valueSourceFactory = new ValueSourceFactory(assetCache);
        var valueConverterFactory = new ValueConverterFactory();
        var attributeBindingFactory = new AttributeBindingFactory(valueSourceFactory, valueConverterFactory);
        var viewBinder = new ReflectionViewBinder(attributeBindingFactory);

        var root = new SElement("lane", [
            new SAttribute("orientation", AttributeValueType.Literal, "vertical"),
            new SAttribute("horizontal-content-alignment", AttributeValueType.Literal, "middle"),
            new SAttribute("vertical-content-alignment", AttributeValueType.Literal, "end"),
        ]);
        var child1 = new SElement("image", [
            // TODO: How can we handle complex attributes like the Layout in particular?
            //new SAttribute("width", AttributeValueType.Literal, "400"),
            new SAttribute("scale", AttributeValueType.Literal, "3.0"),
            new SAttribute("sprite", AttributeValueType.Binding, "@TestSprite"),
        ]);
        var child2 = new SElement("label", [
            new SAttribute("font", AttributeValueType.Literal, "dialogue"),
            new SAttribute("text", AttributeValueType.Binding, "HeaderText"),
        ]);
        var tree = new ViewNode(
            viewFactory, viewBinder, root, [
                new ViewNode(viewFactory, viewBinder, child1, []),
                new ViewNode(viewFactory, viewBinder, child2, []),
            ]);
        tree.Update();

        var rootView = tree.View as Lane;
        Assert.NotNull(rootView);
        Assert.Equal(Orientation.Vertical, rootView.Orientation);
        Assert.Equal(Alignment.Middle, rootView.HorizontalContentAlignment);
        Assert.Equal(Alignment.End, rootView.VerticalContentAlignment);
        Assert.Collection(
            rootView.Children,
            child =>
            {
                var image = Assert.IsType<Image>(child);
                Assert.Equal(3.0f, image.Scale);
                Assert.Equal(UiSprites.ButtonDark, image.Sprite);
            },
            child =>
            {
                var label = Assert.IsType<Label>(child);
                Assert.Equal(Game1.dialogueFont, label.Font);
                // TODO: Should really be empty string, not null, because type is non-nullable. But
                // this is hard to figure out with reflection-based bindings.
                // Assert.Equal("", label.Text);
                Assert.Null(label.Text);
            });

        var model = new { HeaderText = "Some text" };
        tree.Context = model;
        tree.Update();

        Assert.Equal("Some text", ((Label)rootView.Children[1]).Text);
    }
}