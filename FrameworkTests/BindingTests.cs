using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using PropertyChanged.SourceGenerator;
using StardewUI;
using StardewUI.Framework.Binding;
using StardewUI.Framework.Content;
using StardewUI.Framework.Converters;
using StardewUI.Framework.Dom;
using StardewUI.Framework.Grammar;
using StardewUI.Framework.Sources;
using StardewValley;
using Xunit.Abstractions;

namespace StarML.Tests;

public partial class BindingTests
{
    class Model
    {
        public Color Color { get; set; }

        public string Name { get; set; } = "";
    }

    partial class ModelWithNotify : INotifyPropertyChanged
    {
        [Notify]
        private Color color;

        [Notify]
        private string name = "";
    }

    class FakeAssetCache : IAssetCache
    {
        private readonly Dictionary<string, object> assets = [];

        public IAssetCacheEntry<T> Get<T>(string name)
            where T : notnull
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

    private readonly FakeAssetCache assetCache;
    private readonly ITestOutputHelper output;
    private readonly IValueConverterFactory valueConverterFactory;
    private readonly IValueSourceFactory valueSourceFactory;
    private readonly IViewFactory viewFactory;
    private readonly IViewBinder viewBinder;

    public BindingTests(ITestOutputHelper output)
    {
        this.output = output;
        viewFactory = new ViewFactory();
        assetCache = new FakeAssetCache();
        valueSourceFactory = new ValueSourceFactory(assetCache);
        valueConverterFactory = new ValueConverterFactory();
        var attributeBindingFactory = new AttributeBindingFactory(valueSourceFactory, valueConverterFactory);
        viewBinder = new ReflectionViewBinder(attributeBindingFactory);
    }

    [Fact]
    public void Update_WithInputBindings_WritesContextToView()
    {
        var element = new SElement(
            "label",
            [
                new SAttribute("max-lines", "1"),
                new SAttribute("color", "Color", ValueType: AttributeValueType.InputBinding),
                new SAttribute("text", "Name", ValueType: AttributeValueType.InputBinding),
            ]
        );
        var view = viewFactory.CreateView(element.Tag);
        var model = new ModelWithNotify() { Name = "Test text", Color = Color.Blue };
        using var viewBinding = viewBinder.Bind(view, element, BindingContext.Create(model));

        var label = (Label)view;
        Assert.Equal(1, label.MaxLines);
        Assert.Equal("Test text", label.Text);
        Assert.Equal(Color.Blue, label.Color);

        model.Name = "New text";
        viewBinding.Update();
        Assert.Equal("New text", label.Text);
    }

    partial class OutputBindingTestModel : INotifyPropertyChanged
    {
        [Notify]
        private bool @checked;

        [Notify]
        private Vector2 size;
    }

    [Fact]
    public void Update_WithOutputBindings_WritesViewToContext()
    {
        var element = new SElement(
            "checkbox",
            [
                new SAttribute("layout", "200px 20px"),
                new SAttribute("is-checked", "Checked", ValueType: AttributeValueType.OutputBinding),
                new SAttribute("outer-size", "Size", ValueType: AttributeValueType.OutputBinding),
            ]
        );
        var view = viewFactory.CreateView(element.Tag);
        var model = new OutputBindingTestModel { Checked = false, Size = Vector2.Zero };
        using var viewBinding = viewBinder.Bind(view, element, BindingContext.Create(model));

        // Initial bind should generally not cause immediate output sync, because we assume the view isn't completely
        // stable or fully initialized yet.

        Assert.False(model.Checked);
        Assert.Equal(Vector2.Zero, model.Size);

        var checkbox = (CheckBox)view;
        checkbox.Measure(new Vector2(1000, 1000));
        viewBinding.Update();
        Assert.False(model.Checked);
        Assert.Equal(new Vector2(200, 20), model.Size);

        checkbox.IsChecked = true;
        viewBinding.Update();
        Assert.True(model.Checked);
        Assert.Equal(new Vector2(200, 20), model.Size);
    }

    [Fact]
    public void Update_WithInOutBindings_WritesBothDirections()
    {
        var element = new SElement(
            "checkbox",
            [new SAttribute("is-checked", "Checked", ValueType: AttributeValueType.TwoWayBinding)]
        );
        var view = viewFactory.CreateView(element.Tag);
        var model = new OutputBindingTestModel { Checked = true };
        using var viewBinding = viewBinder.Bind(view, element, BindingContext.Create(model));

        var checkbox = (CheckBox)view;
        Assert.True(model.Checked);
        Assert.True(checkbox.IsChecked);

        // No changes, nothing should happen here.
        viewBinding.Update();
        Assert.True(model.Checked);
        Assert.True(checkbox.IsChecked);

        // Simulate click to uncheck
        checkbox.IsChecked = false;
        viewBinding.Update();
        Assert.False(model.Checked);

        // Now the context is updated from some other source
        model.Checked = true;
        viewBinding.Update();
        Assert.True(checkbox.IsChecked);
    }

    [Fact]
    public void TestNodes()
    {
        assetCache.Put("TestSprite", UiSprites.ButtonDark);

        var root = new SElement(
            "lane",
            [
                new SAttribute("orientation", "vertical"),
                new SAttribute("horizontal-content-alignment", "middle"),
                new SAttribute("vertical-content-alignment", "end"),
            ]
        );
        var child1 = new SElement(
            "image",
            [
                // TODO: How can we handle complex attributes like the Layout in particular?
                //new SAttribute("width", AttributeValueType.Literal, "400"),
                new SAttribute("scale", "3.0"),
                new SAttribute("sprite", "TestSprite", ValueType: AttributeValueType.AssetBinding),
            ]
        );
        var child2 = new SElement(
            "label",
            [
                new SAttribute("font", "dialogue"),
                new SAttribute("text", "HeaderText", ValueType: AttributeValueType.InputBinding),
            ]
        );
        var tree = new ViewNode(
            valueSourceFactory,
            viewFactory,
            viewBinder,
            root,
            [
                new ViewNode(valueSourceFactory, viewFactory, viewBinder, child1, []),
                new ViewNode(valueSourceFactory, viewFactory, viewBinder, child2, []),
            ]
        );
        tree.Update();

        var rootView = tree.Views.SingleOrDefault() as Lane;
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
                Assert.Equal("", label.Text);
            }
        );

        var model = new { HeaderText = "Some text" };
        tree.Context = BindingContext.Create(model);
        tree.Update();

        Assert.Equal("Some text", ((Label)rootView.Children[1]).Text);
    }

    [Fact]
    public void TestEndToEnd()
    {
        var viewNodeFactory = new ViewNodeFactory(viewFactory, valueSourceFactory, valueConverterFactory, viewBinder);
        assetCache.Put("Mods/TestMod/TestSprite", UiSprites.SmallTrashCan);

        string markup =
            @"<lane orientation=""vertical"" horizontal-content-alignment=""middle"" vertical-content-alignment=""end"">
                <image scale=""3.5"" sprite={{@Mods/TestMod/TestSprite}} />
                <label font=""dialogue"" text={{HeaderText}} />
            </lane>";
        var document = Document.Parse(markup);
        var tree = viewNodeFactory.CreateNode(document.Root);
        tree.Update();

        var rootView = tree.Views.SingleOrDefault() as Lane;
        Assert.NotNull(rootView);
        Assert.Equal(Orientation.Vertical, rootView.Orientation);
        Assert.Equal(Alignment.Middle, rootView.HorizontalContentAlignment);
        Assert.Equal(Alignment.End, rootView.VerticalContentAlignment);
        Assert.Collection(
            rootView.Children,
            child =>
            {
                var image = Assert.IsType<Image>(child);
                Assert.Equal(3.5f, image.Scale);
                Assert.Equal(UiSprites.SmallTrashCan, image.Sprite);
            },
            child =>
            {
                var label = Assert.IsType<Label>(child);
                Assert.Equal(Game1.dialogueFont, label.Font);
                Assert.Equal("", label.Text);
            }
        );

        var model = new { HeaderText = "Some text" };
        tree.Context = BindingContext.Create(model);
        tree.Update();

        Assert.Equal("Some text", ((Label)rootView.Children[1]).Text);
    }

    partial class ConditionalBindingTestModel : INotifyPropertyChanged
    {
        [Notify]
        private bool firstLineVisible;

        [Notify]
        private bool secondLineVisible;

        [Notify]
        private bool thirdLineVisible;
    }

    [Fact]
    public void WhenConditionalBindingBecomesTrue_AddsView()
    {
        string markup =
            @"<lane>
                <label *if={{FirstLineVisible}} text=""First Line"" />
                <label text=""Second Line"" />
                <label *if={{ThirdLineVisible}} text=""Third Line"" />
            </lane>";
        var model = new ConditionalBindingTestModel { FirstLineVisible = true };
        var tree = BuildTreeFromMarkup(markup, model);

        var rootView = tree.Views.SingleOrDefault() as Lane;
        Assert.NotNull(rootView);
        Assert.Collection(
            rootView.Children,
            child =>
            {
                var label = Assert.IsType<Label>(child);
                Assert.Equal("First Line", label.Text);
            },
            child =>
            {
                var label = Assert.IsType<Label>(child);
                Assert.Equal("Second Line", label.Text);
            }
        );

        model.ThirdLineVisible = true;
        tree.Update();
        Assert.Collection(
            rootView.Children,
            child =>
            {
                var label = Assert.IsType<Label>(child);
                Assert.Equal("First Line", label.Text);
            },
            child =>
            {
                var label = Assert.IsType<Label>(child);
                Assert.Equal("Second Line", label.Text);
            },
            child =>
            {
                var label = Assert.IsType<Label>(child);
                Assert.Equal("Third Line", label.Text);
            }
        );
    }

    [Fact]
    public void WhenConditionalBindingBecomesFalse_RemovesView()
    {
        string markup =
            @"<lane>
                <label *if={{FirstLineVisible}} text=""First Line"" />
                <label *if={{SecondLineVisible}} text=""Second Line"" />
                <label *if=""false"" text=""Third Line"" />
            </lane>";
        var model = new ConditionalBindingTestModel { FirstLineVisible = true, SecondLineVisible = true };
        var tree = BuildTreeFromMarkup(markup, model);

        var rootView = tree.Views.SingleOrDefault() as Lane;
        Assert.NotNull(rootView);
        Assert.Collection(
            rootView.Children,
            child =>
            {
                var label = Assert.IsType<Label>(child);
                Assert.Equal("First Line", label.Text);
            },
            child =>
            {
                var label = Assert.IsType<Label>(child);
                Assert.Equal("Second Line", label.Text);
            }
        );

        model.FirstLineVisible = false;
        tree.Update();
        Assert.Collection(
            rootView.Children,
            child =>
            {
                var label = Assert.IsType<Label>(child);
                Assert.Equal("Second Line", label.Text);
            }
        );
    }

    partial class SwitchCaseLiteralTestModel : INotifyPropertyChanged
    {
        [Notify]
        private int whichItem;
    }

    [Fact]
    public void WhenCaseMatchesDirectChildLiteral_RendersView()
    {
        string markup =
            @"<lane *switch={{WhichItem}}>
                <label text=""Always"" />
                <label *case=""1"" text=""Item 1"" />
                <label *case=""2"" text=""Item 2"" />
                <label *case=""3"" text=""Item 3"" />
            </lane>";
        var model = new SwitchCaseLiteralTestModel() { WhichItem = 3 };
        var tree = BuildTreeFromMarkup(markup, model);

        var rootView = tree.Views.SingleOrDefault() as Lane;
        Assert.NotNull(rootView);
        Assert.Collection(
            rootView.Children,
            child =>
            {
                var label = Assert.IsType<Label>(child);
                Assert.Equal("Always", label.Text);
            },
            child =>
            {
                var label = Assert.IsType<Label>(child);
                Assert.Equal("Item 3", label.Text);
            }
        );

        model.WhichItem = 2;
        tree.Update();
        Assert.Collection(
            rootView.Children,
            child =>
            {
                var label = Assert.IsType<Label>(child);
                Assert.Equal("Always", label.Text);
            },
            child =>
            {
                var label = Assert.IsType<Label>(child);
                Assert.Equal("Item 2", label.Text);
            }
        );
    }

    partial class SwitchCaseBindingTestModel : INotifyPropertyChanged
    {
        public enum Selection
        {
            Foo,
            Bar,
        };

        [Notify]
        private Selection current = Selection.Foo;

        [Notify]
        private Selection first = Selection.Foo;

        [Notify]
        private Selection second = Selection.Bar;
    }

    // TODO: This test would be more useful if implemented using narrowed contexts, i.e. where the bound value is
    // actually on the item itself. But we don't have those types of attributes yet.
    [Fact]
    public void WhenCaseMatchesDirectChildBinding_RendersView()
    {
        string markup =
            @"<lane *switch={{Current}}>
                <label *case={{First}} text=""Item 1"" />
                <label *case={{Second}} text=""Item 2"" />
            </lane>";
        var model = new SwitchCaseBindingTestModel();
        var tree = BuildTreeFromMarkup(markup, model);

        var rootView = tree.Views.SingleOrDefault() as Lane;
        Assert.NotNull(rootView);
        Assert.Collection(
            rootView.Children,
            child =>
            {
                var label = Assert.IsType<Label>(child);
                Assert.Equal("Item 1", label.Text);
            }
        );

        model.Current = SwitchCaseBindingTestModel.Selection.Bar;
        tree.Update();
        Assert.Collection(
            rootView.Children,
            child =>
            {
                var label = Assert.IsType<Label>(child);
                Assert.Equal("Item 2", label.Text);
            }
        );
    }

    partial class RepeatingElement : INotifyPropertyChanged
    {
        [Notify]
        private string name = "";
    }

    partial class RepeatingModel : INotifyPropertyChanged
    {
        [Notify]
        private List<RepeatingElement> items = [];
    }

    [Fact]
    public void WhenRepeating_RendersViewPerElement()
    {
        string markup =
            @"<lane>
                <label *repeat={{Items}} text={{Name}} />
            </lane>";
        var model = new RepeatingModel()
        {
            Items = [new() { Name = "Foo" }, new() { Name = "Bar" }, new() { Name = "Baz" }],
        };
        var tree = BuildTreeFromMarkup(markup, model);

        var rootView = tree.Views.SingleOrDefault() as Lane;
        Assert.NotNull(rootView);
        Assert.Collection(
            rootView.Children,
            child =>
            {
                var label = Assert.IsType<Label>(child);
                Assert.Equal("Foo", label.Text);
            },
            child =>
            {
                var label = Assert.IsType<Label>(child);
                Assert.Equal("Bar", label.Text);
            },
            child =>
            {
                var label = Assert.IsType<Label>(child);
                Assert.Equal("Baz", label.Text);
            }
        );
    }

    [Fact]
    public void WhenRepeating_AndItemChanges_UpdatesView()
    {
        string markup =
            @"<lane>
                <label *repeat={{Items}} text={{Name}} />
            </lane>";
        var model = new RepeatingModel()
        {
            Items = [new() { Name = "Foo" }, new() { Name = "Bar" }, new() { Name = "Baz" }],
        };
        var tree = BuildTreeFromMarkup(markup, model);

        model.Items[1].Name = "Quux";
        tree.Update();

        var rootView = tree.Views.SingleOrDefault() as Lane;
        Assert.NotNull(rootView);
        Assert.Collection(
            rootView.Children,
            child =>
            {
                var label = Assert.IsType<Label>(child);
                Assert.Equal("Foo", label.Text);
            },
            child =>
            {
                var label = Assert.IsType<Label>(child);
                Assert.Equal("Quux", label.Text);
            },
            child =>
            {
                var label = Assert.IsType<Label>(child);
                Assert.Equal("Baz", label.Text);
            }
        );
    }

    [Fact]
    public void WhenRepeating_AndCollectionChanges_RebuildsAllViews()
    {
        string markup =
            @"<lane>
                <label *repeat={{Items}} text={{Name}} />
            </lane>";
        var model = new RepeatingModel()
        {
            Items = [new() { Name = "Foo" }, new() { Name = "Bar" }, new() { Name = "Baz" }],
        };
        var tree = BuildTreeFromMarkup(markup, model);

        model.Items = [new() { Name = "abc" }, new() { Name = "def" }, new() { Name = "xyz" }];
        tree.Update();

        var rootView = tree.Views.SingleOrDefault() as Lane;
        Assert.NotNull(rootView);
        Assert.Collection(
            rootView.Children,
            child =>
            {
                var label = Assert.IsType<Label>(child);
                Assert.Equal("abc", label.Text);
            },
            child =>
            {
                var label = Assert.IsType<Label>(child);
                Assert.Equal("def", label.Text);
            },
            child =>
            {
                var label = Assert.IsType<Label>(child);
                Assert.Equal("xyz", label.Text);
            }
        );
    }

    partial class RepeatingObservableModel : INotifyPropertyChanged
    {
        [Notify]
        private ObservableCollection<RepeatingElement> items = [];
    }

    [Fact]
    public void WhenRepeating_AndNotifyingCollectionChanges_UpdatesAffectedViews()
    {
        string markup =
            @"<lane>
                <label *repeat={{Items}} text={{Name}} />
            </lane>";
        var model = new RepeatingObservableModel()
        {
            Items = [new() { Name = "Foo" }, new() { Name = "Bar" }, new() { Name = "Baz" }],
        };
        var tree = BuildTreeFromMarkup(markup, model);

        model.Items.Move(0, 1);
        model.Items.Insert(0, new() { Name = "Quux" });
        model.Items.Insert(2, new() { Name = "Plip" });
        tree.Update();

        var rootView = tree.Views.SingleOrDefault() as Lane;
        Assert.NotNull(rootView);
        Assert.Collection(
            rootView.Children,
            child =>
            {
                var label = Assert.IsType<Label>(child);
                Assert.Equal("Quux", label.Text);
            },
            child =>
            {
                var label = Assert.IsType<Label>(child);
                Assert.Equal("Bar", label.Text);
            },
            child =>
            {
                var label = Assert.IsType<Label>(child);
                Assert.Equal("Plip", label.Text);
            },
            child =>
            {
                var label = Assert.IsType<Label>(child);
                Assert.Equal("Foo", label.Text);
            },
            child =>
            {
                var label = Assert.IsType<Label>(child);
                Assert.Equal("Baz", label.Text);
            }
        );

        model.Items.RemoveAt(2);
        model.Items.RemoveAt(0);
        model.Items.RemoveAt(2);
        tree.Update();
        Assert.Collection(
            rootView.Children,
            child =>
            {
                var label = Assert.IsType<Label>(child);
                Assert.Equal("Bar", label.Text);
            },
            child =>
            {
                var label = Assert.IsType<Label>(child);
                Assert.Equal("Foo", label.Text);
            }
        );
    }

    partial class ContextTestModel : INotifyPropertyChanged
    {
        public partial class OuterData : INotifyPropertyChanged
        {
            [Notify]
            private MiddleData? middle;
        }

        public partial class MiddleData : INotifyPropertyChanged
        {
            [Notify]
            private InnerData? inner;
        }

        public partial class InnerData : INotifyPropertyChanged
        {
            [Notify]
            private string text = "";
        }

        [Notify]
        private OuterData? outer;
    }

    [Fact]
    public void WhenAnyContextChanges_UpdatesAllContextModifiers()
    {
        string markup =
            @"<frame *context={{Outer}}>
                <panel>
                    <panel *context={{Middle}}>
                        <lane *context={{Inner}}>
                            <label text={{Text}} />
                        </lane>
                    </panel>
                </panel>
            </frame>";
        var model = new ContextTestModel();
        var tree = BuildTreeFromMarkup(markup, model);

        var outerFrame = Assert.IsType<Frame>(tree.Views.SingleOrDefault());
        var outerPanel = Assert.IsType<Panel>(outerFrame.Content);
        var middlePanel = Assert.IsType<Panel>(outerPanel.Children.FirstOrDefault());
        var innerLane = Assert.IsType<Lane>(middlePanel.Children.FirstOrDefault());
        var label = Assert.IsType<Label>(innerLane.Children.FirstOrDefault());

        Assert.Equal("", label.Text);

        model.Outer = new() { Middle = new() { Inner = new() { Text = "foo" } } };
        tree.Update();

        Assert.Equal("foo", label.Text);

        model.Outer.Middle = new() { Inner = new() { Text = "bar" } };
        tree.Update();

        Assert.Equal("bar", label.Text);

        model.Outer.Middle.Inner = new() { Text = "baz" };
        tree.Update();

        Assert.Equal("baz", label.Text);
    }

    private IViewNode BuildTreeFromMarkup(string markup, object model)
    {
        var viewNodeFactory = new ViewNodeFactory(viewFactory, valueSourceFactory, valueConverterFactory, viewBinder);
        var document = Document.Parse(markup);
        var tree = viewNodeFactory.CreateNode(document.Root);
        tree.Context = BindingContext.Create(model);
        tree.Update();
        return tree;
    }
}
