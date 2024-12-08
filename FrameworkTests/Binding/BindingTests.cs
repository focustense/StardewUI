using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using PropertyChanged.SourceGenerator;
using StardewModdingAPI;
using StardewUI.Events;
using StardewUI.Framework.Binding;
using StardewUI.Framework.Content;
using StardewUI.Framework.Converters;
using StardewUI.Framework.Dom;
using StardewUI.Framework.Grammar;
using StardewUI.Framework.Sources;
using StardewUI.Framework.Views;
using StardewUI.Graphics;
using StardewUI.Layout;
using StardewUI.Widgets;
using StardewValley;
using Xunit.Abstractions;

namespace StardewUI.Framework.Tests.Binding;

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
                ? (FakeAssetCacheEntry<T>)(asset)
                : throw new KeyNotFoundException($"Asset '{name}' not registered.");
        }

        public void Put<T>(string name, T asset)
            where T : notnull
        {
            if (assets.TryGetValue(name, out var oldValue) && oldValue is FakeAssetCacheEntry<T> oldEntry)
            {
                oldEntry.IsValid = false;
            }
            assets[name] = new FakeAssetCacheEntry<T>(asset);
        }
    }

    class FakeAssetCacheEntry<T>(T asset) : IAssetCacheEntry<T>
    {
        public T Asset { get; } = asset;

        public bool IsValid { get; set; } = true;
    }

    class FakeResolutionScope : IResolutionScope
    {
        private readonly Dictionary<string, string> translations = [];

        public void AddTranslation(string key, string translation)
        {
            translations.Add(key, translation);
        }

        public Translation? GetTranslation(string key)
        {
            // Not used in tests; we implement GetTranslationValue instead.
            return null;
        }

        public string GetTranslationValue(string key)
        {
            return translations.GetValueOrDefault(key) ?? "";
        }
    }

    class FakeResolutionScopeFactory : IResolutionScopeFactory
    {
        public FakeResolutionScope DefaultScope { get; } = new FakeResolutionScope();

        private readonly Dictionary<Document, FakeResolutionScope> perDocumentScopes = [];

        public void AddForDocument(Document document, FakeResolutionScope scope)
        {
            perDocumentScopes.Add(document, scope);
        }

        public IResolutionScope CreateForDocument(Document document)
        {
            return perDocumentScopes.TryGetValue(document, out var scope) ? scope : DefaultScope;
        }
    }

    private readonly FakeAssetCache assetCache;
    private readonly FakeResolutionScopeFactory resolutionScopeFactory;
    private readonly FakeResolutionScope resolutionScope;
    private readonly IValueConverterFactory valueConverterFactory;
    private readonly IValueSourceFactory valueSourceFactory;
    private readonly IViewFactory viewFactory;
    private readonly IViewBinder viewBinder;

    public BindingTests(ITestOutputHelper output)
    {
        Logger.Monitor = new TestMonitor(output);
        viewFactory = new RootViewFactory([]);
        assetCache = new FakeAssetCache();
        valueSourceFactory = new ValueSourceFactory(assetCache);
        valueConverterFactory = new RootValueConverterFactory([]);
        var attributeBindingFactory = new AttributeBindingFactory(valueSourceFactory, valueConverterFactory);
        var eventBindingFactory = new EventBindingFactory(valueSourceFactory, valueConverterFactory);
        resolutionScopeFactory = new FakeResolutionScopeFactory();
        resolutionScope = resolutionScopeFactory.DefaultScope;
        viewBinder = new ReflectionViewBinder(attributeBindingFactory, eventBindingFactory);
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
            ],
            []
        );
        var view = viewFactory.CreateView(element.Tag);
        var model = new ModelWithNotify() { Name = "Test text", Color = Color.Blue };
        using var viewBinding = viewBinder.Bind(view, element, BindingContext.Create(model), resolutionScope);

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
            ],
            []
        );
        var view = viewFactory.CreateView(element.Tag);
        var model = new OutputBindingTestModel { Checked = false, Size = Vector2.Zero };
        using var viewBinding = viewBinder.Bind(view, element, BindingContext.Create(model), resolutionScope);

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
            [new SAttribute("is-checked", "Checked", ValueType: AttributeValueType.TwoWayBinding)],
            []
        );
        var view = viewFactory.CreateView(element.Tag);
        var model = new OutputBindingTestModel { Checked = true };
        using var viewBinding = viewBinder.Bind(view, element, BindingContext.Create(model), resolutionScope);

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
            ],
            []
        );
        var child1 = new SElement(
            "image",
            [
                // TODO: How can we handle complex attributes like the Layout in particular?
                //new SAttribute("width", AttributeValueType.Literal, "400"),
                new SAttribute("scale", "3.0"),
                new SAttribute("sprite", "TestSprite", ValueType: AttributeValueType.AssetBinding),
            ],
            []
        );
        var child2 = new SElement(
            "label",
            [
                new SAttribute("max-lines", "2"),
                new SAttribute("text", "HeaderText", ValueType: AttributeValueType.InputBinding),
            ],
            []
        );
        var tree = CreateViewNode(root, [new(CreateViewNode(child1)), new(CreateViewNode(child2))]);
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
                Assert.Equal(2, label.MaxLines);
                Assert.Equal("", label.Text);
            }
        );

        var model = new { HeaderText = "Some text" };
        tree.Context = BindingContext.Create(model);
        tree.Update();

        Assert.Equal("Some text", ((Label)rootView.Children[1]).Text);

        ViewNode CreateViewNode(SElement element, IViewNode.Child[]? children = null)
        {
            return new ViewNode(
                valueSourceFactory,
                valueConverterFactory,
                viewFactory,
                viewBinder,
                element,
                resolutionScope
            )
            {
                Children = children ?? [],
            };
        }
    }

    [Fact]
    public void TestEndToEnd()
    {
        var viewNodeFactory = new ViewNodeFactory(
            viewFactory,
            valueSourceFactory,
            valueConverterFactory,
            viewBinder,
            assetCache,
            resolutionScopeFactory
        );
        assetCache.Put("Mods/TestMod/TestSprite", UiSprites.SmallTrashCan);

        string markup =
            @"<lane orientation=""vertical"" horizontal-content-alignment=""middle"" vertical-content-alignment=""end"">
                <image scale=""3.5"" sprite={@Mods/TestMod/TestSprite} />
                <label max-lines=""2"" text={HeaderText} />
            </lane>";
        var document = Document.Parse(markup);
        var tree = viewNodeFactory.CreateNode(document);
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
                Assert.Equal(2, label.MaxLines);
                Assert.Equal("", label.Text);
            }
        );

        var model = new { HeaderText = "Some text" };
        tree.Context = BindingContext.Create(model);
        tree.Update();

        Assert.Equal("Some text", ((Label)rootView.Children[1]).Text);
    }

    [Fact]
    public void WhenOneTimeBinding_UpdatesFirstTimeOnly()
    {
        string markup =
            @"<lane>
                <label text={Name} />
                <label text={<:Name} />
            </lane>";
        var model = new ModelWithNotify() { Name = "First" };
        var tree = BuildTreeFromMarkup(markup, model);

        var lane = Assert.IsType<Lane>(tree.Views.SingleOrDefault());
        var label1 = Assert.IsType<Label>(lane.Children[0]);
        var label2 = Assert.IsType<Label>(lane.Children[1]);

        Assert.Equal("First", label1.Text);
        Assert.Equal("First", label2.Text);

        model.Name = "Second";
        tree.Update();

        Assert.Equal("Second", label1.Text);
        Assert.Equal("First", label2.Text);
    }

    [Fact]
    public void WhenBoundToTranslation_UpdatesWithTranslationValue()
    {
        resolutionScope.AddTranslation("TranslationKey", "Hello");
        string markup = @"<label text={#TranslationKey} />";
        var tree = BuildTreeFromMarkup(markup, new());

        var label = Assert.IsType<Label>(tree.Views.SingleOrDefault());

        Assert.Equal("Hello", label.Text);
    }

    class FieldBindingTestModel
    {
        public string Name = "";
        public string Description = "";
    }

    [Fact]
    public void WhenModelContainsFields_BindsInitialValues()
    {
        string markup = @"<label text={Name} tooltip={Description} />";
        var model = new FieldBindingTestModel() { Name = "Foo", Description = "Bar" };
        var tree = BuildTreeFromMarkup(markup, model);

        var label = Assert.IsType<Label>(tree.Views.SingleOrDefault());

        Assert.Equal("Foo", label.Text);
        Assert.Equal("Bar", label.Tooltip);
    }

    partial class DropDownTestModel : INotifyPropertyChanged
    {
        public Func<object, string> FormatItem { get; } = item => $"Item {item}";

        [Notify]
        private List<object> items = [];

        [Notify]
        private object selectedItem = 3;
    }

    [Fact]
    public void WhenModelIsConvertible_BindsWithConversion()
    {
        string markup = @"<dropdown options={Items} selected-option={<>SelectedItem} option-format={FormatItem} />";
        var model = new DropDownTestModel { Items = [3, 7, 15] };
        var tree = BuildTreeFromMarkup(markup, model);

        var dropdown = Assert.IsType<DynamicDropDownList>(tree.Views.SingleOrDefault());
        dropdown.SelectedIndex = 1;
        tree.Update();

        Assert.Equal(7, model.SelectedItem);
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
                <label *if={FirstLineVisible} text=""First Line"" />
                <label text=""Second Line"" />
                <label *if={ThirdLineVisible} text=""Third Line"" />
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
                <label *if={FirstLineVisible} text=""First Line"" />
                <label *if={SecondLineVisible} text=""Second Line"" />
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
            @"<lane *switch={WhichItem}>
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

    [Fact]
    public void WhenCaseUsedInSingleChildLayout_RendersOneChild()
    {
        string markup =
            @"<frame *switch={WhichItem}>
                <label *case=""1"" text=""Item 1"" />
                <label *case=""2"" text=""Item 2"" />
                <label *case=""3"" text=""Item 3"" />
            </frame>";
        var model = new SwitchCaseLiteralTestModel() { WhichItem = 3 };
        var tree = BuildTreeFromMarkup(markup, model);

        var rootView = Assert.IsType<Frame>(tree.Views.SingleOrDefault());
        Assert.NotNull(rootView);
        var label = Assert.IsType<Label>(rootView.Content);
        Assert.Equal("Item 3", label.Text);

        model.WhichItem = 2;
        tree.Update();
        label = Assert.IsType<Label>(rootView.Content);
        Assert.Equal("Item 2", label.Text);
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

    [Fact]
    public void WhenCaseMatchesDirectChildBinding_RendersView()
    {
        string markup =
            @"<lane *switch={Current}>
                <label *case={First} text=""Item 1"" />
                <label *case={Second} text=""Item 2"" />
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

    [Fact]
    public void WhenCaseMatchesIndirectChildBinding_RendersView()
    {
        string markup =
            @"<lane *switch={Current}>
                <frame layout=""24px 24px"">
                    <label *case=""Foo"" text=""Item 1"" />
                    <label *case=""Bar"" text=""Item 2"" />
                </frame>
            </lane>";
        var model = new SwitchCaseBindingTestModel() { Current = SwitchCaseBindingTestModel.Selection.Bar };
        var tree = BuildTreeFromMarkup(markup, model);

        var rootView = Assert.IsType<Lane>(tree.Views.SingleOrDefault());
        var frame = Assert.IsType<Frame>(rootView.Children[0]);
        var label = Assert.IsType<Label>(frame.Content);
        Assert.Equal("Item 2", label.Text);

        model.Current = SwitchCaseBindingTestModel.Selection.Foo;
        tree.Update();
        label = Assert.IsType<Label>(frame.Content);
        Assert.Equal("Item 1", label.Text);
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
                <label *repeat={Items} text={Name} />
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
    public void WhenRepeating_PropagatesResolutionScope()
    {
        resolutionScope.AddTranslation("RepeatTranslationKey", "Hello");
        string markup = @"<label *repeat={Items} text={#RepeatTranslationKey} />";
        var model = new RepeatingModel()
        {
            Items = [new() { Name = "Foo" }, new() { Name = "Bar" }, new() { Name = "Baz" }],
        };
        var tree = BuildTreeFromMarkup(markup, model);

        Assert.Collection(
            tree.Views,
            child =>
            {
                var label = Assert.IsType<Label>(child);
                Assert.Equal("Hello", label.Text);
            },
            child =>
            {
                var label = Assert.IsType<Label>(child);
                Assert.Equal("Hello", label.Text);
            },
            child =>
            {
                var label = Assert.IsType<Label>(child);
                Assert.Equal("Hello", label.Text);
            }
        );
    }

    [Fact]
    public void WhenRepeating_AndItemChanges_UpdatesView()
    {
        string markup =
            @"<lane>
                <label *repeat={Items} text={Name} />
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
                <label *repeat={Items} text={Name} />
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
                <label *repeat={Items} text={Name} />
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
            @"<frame *context={Outer}>
                <panel>
                    <panel *context={Middle}>
                        <lane *context={Inner}>
                            <label text={Text} />
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

    partial class ContextWalkingTestModel : INotifyPropertyChanged
    {
        [Notify]
        private int maxLines;

        [Notify]
        private List<ItemData> items = [];

        public partial class ItemData : INotifyPropertyChanged
        {
            [Notify]
            private Color color = Color.White;

            [Notify]
            private ItemInnerData? inner;
        }

        public partial class ItemInnerData : INotifyPropertyChanged
        {
            [Notify]
            private string text = "";
        }
    }

    [Fact]
    public void WhenAncestorContextChanges_UpdatesDescendantsWithDistanceRedirects()
    {
        string markup =
            @"<lane orientation=""vertical"">
                <lane *repeat={Items}>
                    <frame *context={Inner}>
                        <label max-lines={^^MaxLines} color={^Color} text={Text} />
                    </frame>
                </lane>
            </lane>";
        var model = new ContextWalkingTestModel()
        {
            MaxLines = 1,
            Items =
            [
                new()
                {
                    Color = Color.White,
                    Inner = new() { Text = "Item 1" },
                },
                new() { Color = Color.Aqua },
            ],
        };
        var tree = BuildTreeFromMarkup(markup, model);

        var rootView = Assert.IsType<Lane>(tree.Views.SingleOrDefault());
        Assert.Collection(
            rootView.Children,
            child =>
            {
                var lane = Assert.IsType<Lane>(child);
                var frame = Assert.IsType<Frame>(lane.Children.SingleOrDefault());
                var label = Assert.IsType<Label>(frame.Content);
                Assert.Equal(1, label.MaxLines);
                Assert.Equal(Color.White, label.Color);
                Assert.Equal("Item 1", label.Text);
            },
            child =>
            {
                var lane = Assert.IsType<Lane>(child);
                var frame = Assert.IsType<Frame>(lane.Children.SingleOrDefault());
                Assert.IsType<Label>(frame.Content);
                // It's not useful to assert anything about the label here, because we didn't set the Inner data and
                // therefore it doesn't have a context. One of the quirks of this system is that the context cannot be
                // walked unless the current context is set; thus even the ^ and ^^ attributes do nothing at this point.
            }
        );

        model.MaxLines = 2;
        model.Items[1].Inner = new() { Text = "Item 2" };
        tree.Update();

        Assert.Collection(
            rootView.Children,
            child =>
            {
                var lane = Assert.IsType<Lane>(child);
                var frame = Assert.IsType<Frame>(lane.Children.SingleOrDefault());
                var label = Assert.IsType<Label>(frame.Content);
                Assert.Equal(2, label.MaxLines);
                Assert.Equal(Color.White, label.Color);
                Assert.Equal("Item 1", label.Text);
            },
            child =>
            {
                var lane = Assert.IsType<Lane>(child);
                var frame = Assert.IsType<Frame>(lane.Children.SingleOrDefault());
                var label = Assert.IsType<Label>(frame.Content);
                Assert.Equal(2, label.MaxLines);
                Assert.Equal(Color.Aqua, label.Color);
                Assert.Equal("Item 2", label.Text);
            }
        );

        model.Items[0].Color = Color.Yellow;
        model.Items[0].Inner!.Text = "Item 3";
        model.Items[1].Color = Color.Lime;
        tree.Update();

        Assert.Collection(
            rootView.Children,
            child =>
            {
                var lane = Assert.IsType<Lane>(child);
                var frame = Assert.IsType<Frame>(lane.Children.SingleOrDefault());
                var label = Assert.IsType<Label>(frame.Content);
                Assert.Equal(2, label.MaxLines);
                Assert.Equal(Color.Yellow, label.Color);
                Assert.Equal("Item 3", label.Text);
            },
            child =>
            {
                var lane = Assert.IsType<Lane>(child);
                var frame = Assert.IsType<Frame>(lane.Children.SingleOrDefault());
                var label = Assert.IsType<Label>(frame.Content);
                Assert.Equal(2, label.MaxLines);
                Assert.Equal(Color.Lime, label.Color);
                Assert.Equal("Item 2", label.Text);
            }
        );
    }

    [Fact]
    public void WhenAncestorContextChanges_UpdatesDescendantsWithTypeRedirects()
    {
        string markup =
            @"<lane orientation=""vertical"">
                <lane *repeat={Items}>
                    <frame *context={Inner}>
                        <label max-lines={~ContextWalkingTestModel.MaxLines} color={~ItemData.Color} text={Text} />
                    </frame>
                </lane>
            </lane>";
        var model = new ContextWalkingTestModel()
        {
            MaxLines = 1,
            Items =
            [
                new()
                {
                    Color = Color.White,
                    Inner = new() { Text = "Item 1" },
                },
                new() { Color = Color.Aqua },
            ],
        };
        var tree = BuildTreeFromMarkup(markup, model);

        var rootView = Assert.IsType<Lane>(tree.Views.SingleOrDefault());
        Assert.Collection(
            rootView.Children,
            child =>
            {
                var lane = Assert.IsType<Lane>(child);
                var frame = Assert.IsType<Frame>(lane.Children.SingleOrDefault());
                var label = Assert.IsType<Label>(frame.Content);
                Assert.Equal(1, label.MaxLines);
                Assert.Equal(Color.White, label.Color);
                Assert.Equal("Item 1", label.Text);
            },
            child =>
            {
                var lane = Assert.IsType<Lane>(child);
                var frame = Assert.IsType<Frame>(lane.Children.SingleOrDefault());
                Assert.IsType<Label>(frame.Content);
                // It's not useful to assert anything about the label here, because we didn't set the Inner data and
                // therefore it doesn't have a context. One of the quirks of this system is that the context cannot be
                // walked unless the current context is set; thus even the ^ and ^^ attributes do nothing at this point.
            }
        );

        model.MaxLines = 2;
        model.Items[0].Color = Color.Yellow;
        model.Items[0].Inner!.Text = "Item 3";
        model.Items[1].Color = Color.Lime;
        model.Items[1].Inner = new() { Text = "Item 2" };
        tree.Update();

        Assert.Collection(
            rootView.Children,
            child =>
            {
                var lane = Assert.IsType<Lane>(child);
                var frame = Assert.IsType<Frame>(lane.Children.SingleOrDefault());
                var label = Assert.IsType<Label>(frame.Content);
                Assert.Equal(2, label.MaxLines);
                Assert.Equal(Color.Yellow, label.Color);
                Assert.Equal("Item 3", label.Text);
            },
            child =>
            {
                var lane = Assert.IsType<Lane>(child);
                var frame = Assert.IsType<Frame>(lane.Children.SingleOrDefault());
                var label = Assert.IsType<Label>(frame.Content);
                Assert.Equal(2, label.MaxLines);
                Assert.Equal(Color.Lime, label.Color);
                Assert.Equal("Item 2", label.Text);
            }
        );
    }

    partial class SingleIncludeModel : INotifyPropertyChanged
    {
        [Notify]
        private string assetName = "";

        [Notify]
        private string labelText = "";

        [Notify]
        private Sprite? sprite;
    }

    [Fact]
    public void WhenIncludedViewBindsToTranslation_UsesResolutionScopeForViewDocument()
    {
        resolutionScope.AddTranslation("OuterKey", "Foo");
        var includedDocument = Document.Parse(@"<label text={#IncludedKey} />");
        var includedScope = new FakeResolutionScope();
        includedScope.AddTranslation("IncludedKey", "Bar");
        resolutionScopeFactory.AddForDocument(includedDocument, includedScope);
        assetCache.Put("IncludedView", includedDocument);

        string markup =
            @"<lane>
                <label text={#OuterKey} />
                <include name=""IncludedView"" />
            </lane>";
        var tree = BuildTreeFromMarkup(markup, new());

        var lane = Assert.IsType<Lane>(tree.Views.SingleOrDefault());
        var outerLabel = Assert.IsType<Label>(lane.Children[0]);
        Assert.Equal("Foo", outerLabel.Text);
        var innerLabel = Assert.IsType<Label>(lane.Children[1]);
        Assert.Equal("Bar", innerLabel.Text);
    }

    [Fact]
    public void WhenIncludedDataChanges_UpdatesView()
    {
        assetCache.Put("LabelView", Document.Parse(@"<label text={LabelText} />"));
        string markup =
            @"<frame>
                <include name=""LabelView"" />
            </frame>";
        var model = new SingleIncludeModel() { LabelText = "Foo" };
        var tree = BuildTreeFromMarkup(markup, model);

        var rootView = Assert.IsType<Frame>(tree.Views.SingleOrDefault());
        var label = Assert.IsType<Label>(rootView.Content);
        Assert.Equal("Foo", label.Text);

        model.LabelText = "Bar";
        tree.Update();

        Assert.Equal(label, rootView.Content);
        Assert.Equal("Bar", label.Text);
    }

    [Fact]
    public void WhenIncludedNameChanges_ReplacesView()
    {
        assetCache.Put("LabelView", Document.Parse(@"<label text={LabelText} />"));
        assetCache.Put("ImageView", Document.Parse(@"<image sprite={Sprite} />"));
        string markup =
            @"<frame>
                <include name={AssetName} />
            </frame>";
        var model = new SingleIncludeModel()
        {
            AssetName = "LabelView",
            LabelText = "Foo",
            Sprite = UiSprites.SmallGreenPlus,
        };
        var tree = BuildTreeFromMarkup(markup, model);

        var rootView = Assert.IsType<Frame>(tree.Views.SingleOrDefault());
        var label = Assert.IsType<Label>(rootView.Content);
        Assert.Equal("Foo", label.Text);

        model.AssetName = "ImageView";
        tree.Update();

        var image = Assert.IsType<Image>(rootView.Content);
        Assert.Equal(UiSprites.SmallGreenPlus, image.Sprite);
    }

    [Fact]
    public void WhenIncludedAssetExpires_ReloadsAndReplacesView()
    {
        assetCache.Put("IncludedView", Document.Parse(@"<label text={LabelText} />"));
        string markup =
            @"<frame>
                <include name=""IncludedView"" />
            </frame>";
        var model = new SingleIncludeModel() { LabelText = "Foo", Sprite = UiSprites.ButtonDark };
        var tree = BuildTreeFromMarkup(markup, model);

        var rootView = Assert.IsType<Frame>(tree.Views.SingleOrDefault());
        var label = Assert.IsType<Label>(rootView.Content);
        Assert.Equal("Foo", label.Text);

        assetCache.Put("IncludedView", Document.Parse(@"<image sprite={Sprite} tooltip={LabelText} />"));
        tree.Update();

        var image = Assert.IsType<Image>(rootView.Content);
        Assert.Equal(UiSprites.ButtonDark, image.Sprite);
        Assert.Equal("Foo", image.Tooltip);
    }

    partial class NestedIncludeModel : INotifyPropertyChanged
    {
        [Notify]
        private InnerData inner = new();

        public partial class InnerData
        {
            [Notify]
            private string labelText = "";
        }
    }

    [Fact]
    public void WhenIncludedExplicitContextChanges_UpdatesView()
    {
        assetCache.Put("LabelView", Document.Parse(@"<label text={LabelText} />"));
        string markup =
            @"<frame>
                <include name=""LabelView"" *context={Inner} />
            </frame>";
        var model = new NestedIncludeModel() { Inner = new() { LabelText = "Foo" } };
        var tree = BuildTreeFromMarkup(markup, model);

        var rootView = Assert.IsType<Frame>(tree.Views.SingleOrDefault());
        var label = Assert.IsType<Label>(rootView.Content);
        Assert.Equal("Foo", label.Text);

        model.Inner = new() { LabelText = "Bar" };
        tree.Update();

        Assert.Equal(label, rootView.Content);
        Assert.Equal("Bar", label.Text);
    }

    partial class RepeatingIncludeModel : INotifyPropertyChanged
    {
        [Notify]
        private ObservableCollection<InnerData> items = [];

        [Notify]
        private int maxLines;

        public partial class InnerData
        {
            [Notify]
            private string labelText = "";
        }
    }

    [Fact]
    public void WhenIncludedImplicitContextChanges_UpdatesView()
    {
        assetCache.Put("LabelView", Document.Parse(@"<label max-lines={^MaxLines} text={LabelText} />"));
        string markup =
            @"<lane>
                <include name=""LabelView"" *repeat={Items} />
            </lane>";
        var model = new RepeatingIncludeModel()
        {
            Items = [new() { LabelText = "Foo" }, new() { LabelText = "Bar" }],
            MaxLines = 3,
        };
        var tree = BuildTreeFromMarkup(markup, model);

        var rootView = Assert.IsType<Lane>(tree.Views.SingleOrDefault());
        Assert.Collection(
            rootView.Children,
            child =>
            {
                var label = Assert.IsType<Label>(child);
                Assert.Equal(3, label.MaxLines);
                Assert.Equal("Foo", label.Text);
            },
            child =>
            {
                var label = Assert.IsType<Label>(child);
                Assert.Equal(3, label.MaxLines);
                Assert.Equal("Bar", label.Text);
            }
        );

        model.Items[0] = new() { LabelText = "Baz" };
        model.Items[1] = new() { LabelText = "Quux" };
        model.Items.Add(new() { LabelText = "Meep" });
        tree.Update();

        Assert.Collection(
            rootView.Children,
            child =>
            {
                var label = Assert.IsType<Label>(child);
                Assert.Equal(3, label.MaxLines);
                Assert.Equal("Baz", label.Text);
            },
            child =>
            {
                var label = Assert.IsType<Label>(child);
                Assert.Equal(3, label.MaxLines);
                Assert.Equal("Quux", label.Text);
            },
            child =>
            {
                var label = Assert.IsType<Label>(child);
                Assert.Equal(3, label.MaxLines);
                Assert.Equal("Meep", label.Text);
            }
        );
    }

    partial class MultipleOutletsModel : INotifyPropertyChanged
    {
        public string ContentText { get; set; } = "";
        public string HeaderCollapsedText { get; set; } = "";
        public string HeaderExpandedText { get; set; } = "";
        public bool IsCollapsed => !IsExpanded;

        [Notify]
        private bool isExpanded;
    }

    [Fact]
    public void WhenMultipleOutletsTargeted_SetsViewPerOutlet()
    {
        string markup =
            @"<expander is-expanded={<>IsExpanded}>
                <frame *outlet=""header"" *if={IsExpanded}>
                    <label text={:HeaderExpandedText} />
                </frame>
                <frame *outlet=""header"" *if={IsCollapsed}>
                    <label text={:HeaderCollapsedText} />
                </frame>
                <panel>
                    <label text={:ContentText} />
                </panel>
            </expander>";
        var model = new MultipleOutletsModel
        {
            HeaderCollapsedText = "Expand",
            HeaderExpandedText = "Collapse",
            ContentText = "Content Text",
        };
        var tree = BuildTreeFromMarkup(markup, model);

        var expander = Assert.IsType<Expander>(tree.Views.SingleOrDefault());
        var header = Assert.IsType<Frame>(expander.Header);
        var headerLabel = Assert.IsType<Label>(header.Content);
        Assert.Equal("Expand", headerLabel.Text);
        var content = Assert.IsType<Panel>(expander.Content);
        var contentLabel = Assert.IsType<Label>(content.Children[0]);
        Assert.Equal("Content Text", contentLabel.Text);

        model.IsExpanded = true;
        tree.Update();

        header = Assert.IsType<Frame>(expander.Header);
        headerLabel = Assert.IsType<Label>(header.Content);
        Assert.Equal("Collapse", headerLabel.Text);
    }

    [Fact]
    public void WhenSimpleNodeHasFloatAttribute_AddsFloatingElement()
    {
        string markup =
            @"<panel>
                <label text=""foo"" />
                <label *float=""above"" text=""bar"" />
                <label text=""baz"" />
                <label *float=""before; -10, 4"" text=""quux"" />
            </panel>";
        var tree = BuildTreeFromMarkup(markup, new());

        var panel = Assert.IsType<Panel>(tree.Views.SingleOrDefault());
        Assert.Collection(
            panel.Children,
            child =>
            {
                var label = Assert.IsType<Label>(child);
                Assert.Equal("foo", label.Text);
            },
            child =>
            {
                var label = Assert.IsType<Label>(child);
                Assert.Equal("baz", label.Text);
            }
        );
        Assert.Collection(
            panel.FloatingElements,
            fe =>
            {
                // Floating positions end up as function delegates, so the only way to verify them is to actually
                // compute the final position against some dummy sizes.
                Assert.Equal(new Vector2(0, -24), fe.Position.GetOffset(new Vector2(80, 24), new Vector2(200, 50)));
                var label = Assert.IsType<Label>(fe.View);
                Assert.Equal("bar", label.Text);
            },
            fe =>
            {
                Assert.Equal(new Vector2(-90, 4), fe.Position.GetOffset(new Vector2(80, 24), new Vector2(200, 50)));
                var label = Assert.IsType<Label>(fe.View);
                Assert.Equal("quux", label.Text);
            }
        );
    }

    partial class ConditionalFloatModel : INotifyPropertyChanged
    {
        [Notify]
        private bool showFloat;
    }

    [Fact]
    public void WhenConditionalNodeHasFloatAttribute_AddsOrRemovesFloatingElement()
    {
        string markup =
            @"<panel>
                <label *float=""after"" *if={ShowFloat} text=""foo"" />
            </panel>";
        var model = new ConditionalFloatModel();
        var tree = BuildTreeFromMarkup(markup, model);

        var panel = Assert.IsType<Panel>(tree.Views.SingleOrDefault());
        Assert.Empty(panel.Children);
        Assert.Empty(panel.FloatingElements);

        model.ShowFloat = true;
        tree.Update();

        Assert.Empty(panel.Children);
        var fe = Assert.Single(panel.FloatingElements);
        Assert.Equal(new Vector2(200, 0), fe.Position.GetOffset(new Vector2(80, 24), new Vector2(200, 50)));
        var label = Assert.IsType<Label>(fe.View);
        Assert.Equal("foo", label.Text);
    }

    class RepeatingFloatModel
    {
        public IReadOnlyList<Badge> Badges { get; set; } = [];

        public class Badge(string text, Func<Vector2, Vector2, Vector2> position)
        {
            public Func<Vector2, Vector2, Vector2> Position => position;
            public string Text => text;
        }
    }

    [Fact]
    public void WhenRepeatingNodeHasFloatAttribute_AddsAllAsFloatingElements()
    {
        string markup =
            @"<panel>
                <label *repeat={Badges} *float={Position} text={Text} />
            </panel>";
        var model = new RepeatingFloatModel()
        {
            Badges =
            [
                new("foo", (floatSize, parentSize) => new(parentSize.X - floatSize.X, 0)),
                new("bar", (floatSize, parentSize) => new(parentSize.X - floatSize.X, 20)),
                new("baz", (floatSize, parentSize) => new(parentSize.X - floatSize.X, 40)),
            ],
        };
        var tree = BuildTreeFromMarkup(markup, model);

        var panel = Assert.IsType<Panel>(tree.Views.SingleOrDefault());
        Assert.Collection(
            panel.FloatingElements,
            fe =>
            {
                Assert.Equal(new Vector2(120, 0), fe.Position.GetOffset(new Vector2(80, 24), new Vector2(200, 50)));
                var label = Assert.IsType<Label>(fe.View);
                Assert.Equal("foo", label.Text);
            },
            fe =>
            {
                Assert.Equal(new Vector2(120, 20), fe.Position.GetOffset(new Vector2(80, 24), new Vector2(200, 50)));
                var label = Assert.IsType<Label>(fe.View);
                Assert.Equal("bar", label.Text);
            },
            fe =>
            {
                Assert.Equal(new Vector2(120, 40), fe.Position.GetOffset(new Vector2(80, 24), new Vector2(200, 50)));
                var label = Assert.IsType<Label>(fe.View);
                Assert.Equal("baz", label.Text);
            }
        );
    }

    class EventTestModel
    {
        public int Delta { get; } = 5;
        public List<Item> Items { get; set; } = [];

        public void IncrementItem(string id, int delta, SButton button)
        {
            var item = Items.Find(item => item.Id == id);
            Assert.NotNull(item);
            int multiplier = button == SButton.ControllerX ? 5 : 1;
            item.Quantity += delta * multiplier;
        }

        public class Item(string id)
        {
            public string Id { get; } = id;
            public int Quantity { get; set; }
        }
    }

    [Fact]
    public void WhenViewRaisesEvent_InvokesBoundMethod()
    {
        string markup =
            @"<lane>
                <frame *repeat={Items}>
                    <button click=|^IncrementItem(Id, ^Delta, $Button)| />
                </frame>
            </lane>";
        var model = new EventTestModel() { Items = [new("foo"), new("bar"), new("baz")] };
        var tree = BuildTreeFromMarkup(markup, model);

        var rootView = Assert.IsType<Lane>(tree.Views.SingleOrDefault());
        var bazFrame = Assert.IsType<Frame>(rootView.Children[2]);
        var bazButton = Assert.IsType<Button>(bazFrame.Content);

        bazButton.OnClick(new(Vector2.Zero, SButton.ControllerA));

        Assert.Equal(5, model.Items[2].Quantity);

        bazButton.OnClick(new(Vector2.Zero, SButton.ControllerX));

        Assert.Equal(30, model.Items[2].Quantity);
    }

    class ManyToOneEventTestModel
    {
        public Outer? Data { get; set; }
        public List<(string, int)> Results { get; } = [];
        public int Value { get; set; }

        public void AddResult(string s, int i)
        {
            Results.Add((s, i));
        }

        public record Outer(int Value, string Name, Inner Inner1, Inner Inner2);

        public record Inner(int Value);
    }

    // Our goal with this test is simply to be as punishing as possible in terms of caching behavior and ambiguity
    // around scoping and argument types. We use the same event bound to the same handler, but always on different
    // views, with context that might be different or the same, arguments that might be bound or literal, and
    // ambiguous names all over the place (everything is intentionally "Value").
    [Fact]
    public void WhenSameEventBoundForManyViews_InvokesWithSeparateArgs()
    {
        string markup =
            @"<frame *context={Data} click=|AddResult(""frame"", Value)|>
                <lane click=|^AddResult(Name, Value)|>
                    <panel>
                        <frame *context={Inner1}>
                            <image click=|^^AddResult(""image1"", Value)| />
                        </frame>
                        <frame *context={Inner2}>
                            <image click=|~ManyToOneEventTestModel.AddResult(""image2"", Value)| />
                        </frame>
                    </panel>
                    <button click=|^AddResult(""button"", ""999"")| />
                </lane>
            </frame>";
        var model = new ManyToOneEventTestModel { Data = new(10, "nameFromData", new(50), new(51)), Value = 3 };
        var tree = BuildTreeFromMarkup(markup, model);
        var frame = Assert.IsType<Frame>(tree.Views.SingleOrDefault());
        var lane = Assert.IsType<Lane>(frame.Content);
        var panel = Assert.IsType<Panel>(lane.Children[0]);
        var image1 = Assert.IsType<Image>(Assert.IsType<Frame>(panel.Children[0]).Content);
        var image2 = Assert.IsType<Image>(Assert.IsType<Frame>(panel.Children[1]).Content);
        var button = Assert.IsType<Button>(lane.Children[1]);

        var dummyEventArgs = new ClickEventArgs(Vector2.Zero, SButton.ControllerA);
        image1.OnClick(dummyEventArgs);
        button.OnClick(dummyEventArgs);
        panel.OnClick(dummyEventArgs); // Should do nothing, no event bound
        frame.OnClick(dummyEventArgs);
        lane.OnClick(dummyEventArgs);
        image2.OnClick(dummyEventArgs);
        button.OnClick(dummyEventArgs);

        Assert.Equal(
            [("image1", 50), ("button", 999), ("frame", 3), ("nameFromData", 10), ("image2", 51), ("button", 999)],
            model.Results
        );
    }

    class OptionalParamsEventTestModel
    {
        public List<(string, int)> Results { get; } = [];

        public void AddResult(string name, int value, int toAdd = 0, bool alsoAddOne = false)
        {
            Results.Add((name, value + toAdd + (alsoAddOne ? 1 : 0)));
        }
    }

    [Fact]
    public void WhenEventBoundWithOptionalParameters_InvokesWithDefinedArgs()
    {
        string markup =
            @"<panel>
                <image click=|AddResult(""image1"", ""10"", ""5"", ""true"")| />
                <image click=|AddResult(""image2"", ""20"", ""3"")| />
                <button click=|AddResult(""button"", ""30"")| />
            </panel>";
        var model = new OptionalParamsEventTestModel();
        var tree = BuildTreeFromMarkup(markup, model);
        var panel = Assert.IsType<Panel>(tree.Views.SingleOrDefault());
        var image1 = Assert.IsType<Image>(panel.Children[0]);
        var image2 = Assert.IsType<Image>(panel.Children[1]);
        var button = Assert.IsType<Button>(panel.Children[2]);

        var dummyEventArgs = new ClickEventArgs(Vector2.Zero, SButton.ControllerA);
        image1.OnClick(dummyEventArgs);
        image2.OnClick(dummyEventArgs);
        button.OnClick(dummyEventArgs);

        Assert.Equal([("image1", 16), ("image2", 23), ("button", 30)], model.Results);
    }

    partial class NoParamsEventTestModel : INotifyPropertyChanged
    {
        public float PreviousMoney { get; private set; }

        [Notify]
        private float money;

        public void HandleChange()
        {
            PreviousMoney = Money;
        }
    }

    [Fact]
    public void WhenEventBoundWithNoParameters_InvokesWithReceiverOnly()
    {
        string markup = @"<slider min=""50"" max=""200"" value={<>Money} value-change=|HandleChange()| />";
        var model = new NoParamsEventTestModel() { Money = 100 };
        var tree = BuildTreeFromMarkup(markup, model);
        var slider = Assert.IsType<Slider>(tree.Views.SingleOrDefault());

        slider.Value = 150;

        Assert.Equal(100, model.PreviousMoney);
    }

    partial class TabsTestModel : INotifyPropertyChanged
    {
        public enum Tab
        {
            One,
            Two,
            Three,
        };

        public string HeaderText { get; set; } = "";
        public string Page1Text { get; set; } = "";
        public string Page2Text { get; set; } = "";
        public string Page3Text { get; set; } = "";

        [Notify]
        private Tab selectedTab;

        public void ChangeTab(Tab tab)
        {
            SelectedTab = tab;
        }
    }

    [Fact]
    public void WhenEventHandlerChangesModel_UpdatesViewContent()
    {
        string markup =
            @"<lane orientation=""vertical"" horizontal-content-alignment=""middle"">
                <banner background-border-thickness=""48,0"" padding=""12"" text={HeaderText} />
                <lane orientation=""horizontal"" horizontal-content-alignment=""middle"">
                    <button text=""Tab 1"" click=|ChangeTab(""One"")|/>
                    <button text=""Tab 2"" click=|ChangeTab(""Two"")|/>
                    <button text=""Tab 3"" click=|ChangeTab(""Three"")|/>
                </lane>
                <frame *switch={SelectedTab} layout=""200px 200px"" margin=""0,16,0,0"" padding=""32,24"">
                    <label *case=""One"" text={Page1Text} />
                    <label *case=""Two"" text={Page2Text} />
                    <label *case=""Three"" text={Page3Text} />
                </frame>
            </lane>";
        var model = new TabsTestModel()
        {
            HeaderText = "Tabbed Menu",
            Page1Text = "This is the first page.",
            Page2Text = "This is the second page.",
            Page3Text = "This is the third page.",
        };
        var tree = BuildTreeFromMarkup(markup, model);

        var root = Assert.IsType<Lane>(tree.Views.SingleOrDefault());
        var banner = Assert.IsType<Banner>(root.Children[0]);
        Assert.Equal("Tabbed Menu", banner.Text);
        var tabsLane = Assert.IsType<Lane>(root.Children[1]);
        var tab2Button = Assert.IsType<Button>(tabsLane.Children[1]);
        var tab3Button = Assert.IsType<Button>(tabsLane.Children[2]);
        var contentFrame = Assert.IsType<Frame>(root.Children[2]);

        var label = Assert.IsType<Label>(contentFrame.Content);
        Assert.Equal("This is the first page.", label.Text);

        var dummyEventArgs = new ClickEventArgs(Vector2.Zero, SButton.ControllerA);
        tab2Button.OnClick(dummyEventArgs);
        tree.Update();

        label = Assert.IsType<Label>(contentFrame.Content);
        Assert.Equal("This is the second page.", label.Text);

        tab3Button.OnClick(dummyEventArgs);
        tree.Update();

        label = Assert.IsType<Label>(contentFrame.Content);
        Assert.Equal("This is the third page.", label.Text);
    }

    class BubbleTestModel
    {
        public int Counter { get; private set; }

        public void HandleVoid()
        {
            Counter++;
        }

        public bool HandleWithResult(bool result)
        {
            Counter++;
            return result;
        }
    }

    [Fact]
    public void WhenEventHandlerReturnsVoid_AllowsBubbling()
    {
        string markup =
            @"<frame click=|HandleVoid()|>
                <button layout=""10px 10px"" click=|HandleVoid()| />
            </frame>";
        var model = new BubbleTestModel();
        var tree = BuildTreeFromMarkup(markup, model);

        var root = Assert.IsType<Frame>(tree.Views.SingleOrDefault());
        root.Measure(new(100, 100)); // Otherwise event won't dispatch to children.
        root.OnClick(new(new(5, 5), SButton.ControllerA));

        Assert.Equal(2, model.Counter);
    }

    [Fact]
    public void WhenEventHandlerReturnsFalse_AllowsBubbling()
    {
        string markup =
            @"<frame click=|HandleVoid()|>
                <button layout=""10px 10px"" click=|HandleWithResult(""false"")| />
            </frame>";
        var model = new BubbleTestModel();
        var tree = BuildTreeFromMarkup(markup, model);

        var root = Assert.IsType<Frame>(tree.Views.SingleOrDefault());
        root.Measure(new(100, 100));
        root.OnClick(new(new(5, 5), SButton.ControllerA));

        Assert.Equal(2, model.Counter);
    }

    [Fact]
    public void WhenEventHandlerReturnsTrue_PreventsBubbling()
    {
        string markup =
            @"<frame click=|HandleVoid()|>
                <button layout=""10px 10px"" click=|HandleWithResult(""true"")| />
            </frame>";
        var model = new BubbleTestModel();
        var tree = BuildTreeFromMarkup(markup, model);

        var root = Assert.IsType<Frame>(tree.Views.SingleOrDefault());
        root.Measure(new(100, 100));
        root.OnClick(new(new(5, 5), SButton.ControllerA));

        Assert.Equal(1, model.Counter);
    }

    enum DropdownTestEnum
    {
        Foo,
        Bar,
        Baz,
        Qux,
    }

    partial class TypedDropdownTestModel : INotifyPropertyChanged
    {
        [Notify]
        private List<DropdownTestEnum> items = [];

        [Notify]
        private Func<DropdownTestEnum, string>? itemFormat;

        [Notify]
        private int selectedIndex;

        [Notify]
        private DropdownTestEnum selectedItem;
    }

    [Fact]
    public void WhenDynamicDropdownTypesAreConsistent_BindsAndUpdates()
    {
        string markup =
            @"<dropdown options={Items}
                        selected-index={<>SelectedIndex}
                        selected-option={<>SelectedItem}
                        option-format={ItemFormat}
            />";
        var model = new TypedDropdownTestModel()
        {
            Items = [DropdownTestEnum.Foo, DropdownTestEnum.Bar, DropdownTestEnum.Baz],
        };
        var tree = BuildTreeFromMarkup(markup, model);

        var dropdown = Assert.IsType<DynamicDropDownList>(tree.Views.SingleOrDefault());
        Assert.Equal(0, dropdown.SelectedIndex);
        Assert.Equal(0, model.SelectedIndex);
        Assert.Equal(DropdownTestEnum.Foo, dropdown.SelectedOption?.Value);
        Assert.Equal(DropdownTestEnum.Foo, model.SelectedItem);
        Assert.Equal("Foo", dropdown.SelectedOptionText);

        model.SelectedIndex = 1;
        tree.Update();

        Assert.Equal(1, dropdown.SelectedIndex);
        Assert.Equal(1, model.SelectedIndex);
        Assert.Equal(DropdownTestEnum.Bar, dropdown.SelectedOption?.Value);
        Assert.Equal(DropdownTestEnum.Bar, model.SelectedItem);
        Assert.Equal("Bar", dropdown.SelectedOptionText);

        model.SelectedItem = DropdownTestEnum.Baz;
        tree.Update();

        Assert.Equal(2, dropdown.SelectedIndex);
        Assert.Equal(2, model.SelectedIndex);
        Assert.Equal(DropdownTestEnum.Baz, dropdown.SelectedOption?.Value);
        Assert.Equal(DropdownTestEnum.Baz, model.SelectedItem);
        Assert.Equal("Baz", dropdown.SelectedOptionText);

        model.ItemFormat = v => new string(v.ToString().ToLower().Reverse().ToArray());
        model.SelectedItem = DropdownTestEnum.Qux;
        model.Items = [DropdownTestEnum.Baz, DropdownTestEnum.Qux, DropdownTestEnum.Foo];
        tree.Update();

        Assert.Equal(1, dropdown.SelectedIndex);
        Assert.Equal(1, model.SelectedIndex);
        Assert.Equal(DropdownTestEnum.Qux, dropdown.SelectedOption?.Value);
        Assert.Equal(DropdownTestEnum.Qux, model.SelectedItem);
        Assert.Equal("xuq", dropdown.SelectedOptionText);
    }

    class UpdateOuterModel
    {
        public UpdateInnerModel Inner { get; set; } = new();
        public int UpdateCount { get; set; }

        public void Update()
        {
            UpdateCount++;
        }
    }

    class UpdateInnerModel
    {
        public TimeSpan ElapsedTotal { get; set; }

        public void Update(TimeSpan elapsed)
        {
            ElapsedTotal += elapsed;
        }
    }

    [Fact]
    public void WhenContextHasUpdateMethod_RunsEachTick()
    {
        // The markup deliberately attaches to the `Inner` context twice in order to verify that there isn't a duplicate
        // update tick; it should be one per context, not one per context *binding*.
        string markup =
            @"<panel>
                <lane>
                    <frame *if=""true"" *context={Inner}>
                        <button text=""Cancel"" />
                    </frame>
                    <frame *if=""true"" *context={Inner}>
                        <button text=""OK"" />
                    </frame>
                </lane>
            </panel>";
        var model = new UpdateOuterModel();
        var tree = BuildTreeFromMarkup(markup, model);

        // Initial update, no time elapsed (see below).
        Assert.Equal(1, model.UpdateCount);
        Assert.Equal(TimeSpan.Zero, model.Inner.ElapsedTotal);

        // Since there is no actual game loop running in these tests, we have to reset the tracker manually.
        ContextUpdateTracker.Instance.Reset();
        tree.Update(TimeSpan.FromMilliseconds(50));
        // This should be ignored because the tracker hasn't reset.
        tree.Update(TimeSpan.FromMilliseconds(60));

        Assert.Equal(2, model.UpdateCount);
        Assert.Equal(TimeSpan.FromMilliseconds(50), model.Inner.ElapsedTotal);

        // Make double-plus sure that reset actually resets
        ContextUpdateTracker.Instance.Reset();
        tree.Update(TimeSpan.FromMilliseconds(40));

        Assert.Equal(3, model.UpdateCount);
        Assert.Equal(TimeSpan.FromMilliseconds(90), model.Inner.ElapsedTotal);
    }

    class UpdateInvalidReturnModel
    {
        public int UpdateCount { get; set; }

        public bool Update()
        {
            UpdateCount++;
            return true;
        }
    }

    [Fact]
    public void WhenUpdateMethodHasInvalidReturnType_IgnoresForTick()
    {
        string markup = @"<label text=""Hello"" />";
        var model = new UpdateInvalidReturnModel();
        var tree = BuildTreeFromMarkup(markup, model);

        // BuildTreeFromMarkup would fire the first update anyway, but this adds a little more certainty.
        tree.Update();

        Assert.Equal(0, model.UpdateCount);
    }

    class UpdateInvalidArgsModel
    {
        public int UpdateCount { get; set; }

        public void Update(TimeSpan elapsed, int arg)
        {
            UpdateCount++;
        }
    }

    [Fact]
    public void WhenUpdateMethodHasInvalidArgumentTypes_IgnoresForTick()
    {
        string markup = @"<label text=""Hello"" />";
        var model = new UpdateInvalidArgsModel();
        var tree = BuildTreeFromMarkup(markup, model);

        // BuildTreeFromMarkup would fire the first update anyway, but this adds a little more certainty.
        tree.Update();

        Assert.Equal(0, model.UpdateCount);
    }

    partial class TemplateBindingModel : INotifyPropertyChanged
    {
        [Notify]
        private bool option1;

        [Notify]
        private bool option2;
    }

    [Fact]
    public void WhenBoundWithTemplateNodes_ExpandsTemplates()
    {
        string markup =
            @"<template name=""heading"">
                <label color=""red"" text={&text} />
            </template>

            <lane orientation=""vertical"">
                <heading text=""Heading 1"" />
                <row title=""Option 1""><checkbox is-checked={<>Option1}/></row>
                <heading text=""Heading 2"" />
                <row title=""Option 2""><checkbox is-checked={<>Option2}/></row>
            </lane>

            <template name=""row"">
                <lane vertical-content-alignment=""middle"">
                    <label layout=""400px content"" text={&title} />
                    <outlet />
                </lane>
            </template>";
        var model = new TemplateBindingModel() { Option1 = true };
        var tree = BuildTreeFromMarkup(markup, model);

        var root = Assert.IsType<Lane>(tree.Views.SingleOrDefault());
        Assert.Equal(Orientation.Vertical, root.Orientation);
        CheckBox option1Checkbox = null!;
        CheckBox option2Checkbox = null!;
        Assert.Collection(
            root.Children,
            child =>
            {
                var label = Assert.IsType<Label>(child);
                Assert.Equal(Color.Red, label.Color);
                Assert.Equal("Heading 1", label.Text);
            },
            child =>
            {
                var row = Assert.IsType<Lane>(child);
                Assert.Equal(Alignment.Middle, row.VerticalContentAlignment);
                Assert.Collection(
                    row.Children,
                    child =>
                    {
                        var label = Assert.IsType<Label>(child);
                        Assert.Equal(Length.Px(400), label.Layout.Width);
                        Assert.Equal(Length.Content(), label.Layout.Height);
                        Assert.Equal("Option 1", label.Text);
                    },
                    child =>
                    {
                        option1Checkbox = Assert.IsType<CheckBox>(child);
                        Assert.True(option1Checkbox.IsChecked);
                    }
                );
            },
            child =>
            {
                var label = Assert.IsType<Label>(child);
                Assert.Equal(Color.Red, label.Color);
                Assert.Equal("Heading 2", label.Text);
            },
            child =>
            {
                var row = Assert.IsType<Lane>(child);
                Assert.Equal(Alignment.Middle, row.VerticalContentAlignment);
                Assert.Collection(
                    row.Children,
                    child =>
                    {
                        var label = Assert.IsType<Label>(child);
                        Assert.Equal(Length.Px(400), label.Layout.Width);
                        Assert.Equal(Length.Content(), label.Layout.Height);
                        Assert.Equal("Option 2", label.Text);
                    },
                    child =>
                    {
                        option2Checkbox = Assert.IsType<CheckBox>(child);
                        Assert.False(option2Checkbox.IsChecked);
                    }
                );
            }
        );

        option1Checkbox.IsChecked = false;
        tree.Update();
        Assert.False(model.Option1);

        model.Option2 = true;
        tree.Update();
        Assert.True(option2Checkbox.IsChecked);
    }

    partial class TemplateBindingEventOuterModel
    {
        public string Arg1 { get; private set; } = "";
        public int Arg2 { get; private set; }
        public string Arg3 { get; private set; } = "";
        public TemplateBindingEventInnerModel? Inner { get; set; }

        public void Handle(string arg1, int arg2, string arg3)
        {
            Arg1 = arg1;
            Arg2 = arg2;
            Arg3 = arg3;
        }
    }

    partial class TemplateBindingEventInnerModel
    {
        public int Id { get; set; }
    }

    [Fact]
    public void WhenBoundWithTemplateNodes_ExpandsEventHandlerArguments()
    {
        // It might seem silly that we're passing in the same "Count" property that's already in the model as an event
        // argument, but it's just for testing the template binding.
        string markup =
            @"<panel *context={Inner}>
                <foo bar=""abc"" id={Id} />
            </panel>

            <template name=""foo"">
                <button click=|^Handle(""dummy"", &id, &bar)| />
            </template>";
        var model = new TemplateBindingEventOuterModel() { Inner = new() { Id = 38 } };
        var tree = BuildTreeFromMarkup(markup, model);

        var panel = Assert.IsType<Panel>(tree.Views.SingleOrDefault());
        var button = Assert.IsType<Button>(panel.Children.SingleOrDefault());
        button.OnClick(new(Vector2.Zero, SButton.ControllerA));

        Assert.Equal("dummy", model.Arg1);
        Assert.Equal(38, model.Arg2);
        Assert.Equal("abc", model.Arg3);
    }

    [Fact]
    public void WhenTemplateInvocationHasStructuralAttributes_AppliesToExpandedNodes()
    {
        string markup =
            @"<lane>
                <foo *repeat={this} color=""blue"" />
            </lane>

            <template name=""foo"">
                <label text={this} color={&color} />
            </template>";
        var model = new List<string> { "aaa", "bbb", "ccc" };
        var tree = BuildTreeFromMarkup(markup, model);

        var lane = Assert.IsType<Lane>(tree.Views.SingleOrDefault());
        Assert.Collection(
            lane.Children,
            child =>
            {
                var label = Assert.IsType<Label>(child);
                Assert.Equal("aaa", label.Text);
                Assert.Equal(Color.Blue, label.Color);
            },
            child =>
            {
                var label = Assert.IsType<Label>(child);
                Assert.Equal("bbb", label.Text);
                Assert.Equal(Color.Blue, label.Color);
            },
            child =>
            {
                var label = Assert.IsType<Label>(child);
                Assert.Equal("ccc", label.Text);
                Assert.Equal(Color.Blue, label.Color);
            }
        );
    }

    [Fact]
    public void WhenTemplateHasTemplateBoundStructuralAttribute_ExpandsValue()
    {
        string markup =
            @"<lane>
                <foo show-baz={ShowBaz} />
            </lane>

            <template name=""foo"">
                <label text=""bar"" />
                <label *if={&show-baz} text=""baz"" />
            </template>";
        var model = new { ShowBaz = true };
        var tree = BuildTreeFromMarkup(markup, model);

        var lane = Assert.IsType<Lane>(tree.Views.SingleOrDefault());
        Assert.Collection(
            lane.Children,
            child =>
            {
                var label = Assert.IsType<Label>(child);
                Assert.Equal("bar", label.Text);
            },
            child =>
            {
                var label = Assert.IsType<Label>(child);
                Assert.Equal("baz", label.Text);
            }
        );
    }

    [Fact]
    public void WhenTemplateReferencesOtherTemplate_ExpandsInOrder()
    {
        string markup =
            @"<lane orientation=""vertical"">
                <foo color=""green"" text1=""A"" text2=""B"" text3=""C"" />
                <foo color=""yellow"" text1=""X"" text2=""Y"" text3=""Z"" />
            </lane>

            <template name=""foo"">
                <bar>
                    <baz color={&color} text={&text1} />
                    <baz color=""Black"" text={&text2} />
                    <baz *outlet=""quux"" color={&color} text={&text3} />
                </bar>
            </template>

            <template name=""bar"">
                <outlet />
                <outlet name=""quux"" />
                <spacer />
            </template>

            <template name=""baz"">
                <label color={&color} text={&text} />
            </template>";
        var tree = BuildTreeFromMarkup(markup, new());

        var root = Assert.IsType<Lane>(tree.Views.SingleOrDefault());
        Assert.Equal(Orientation.Vertical, root.Orientation);
        Assert.Collection(
            root.Children,
            child => AssertLabel(Color.Green, "A", child),
            child => AssertLabel(Color.Black, "B", child),
            child => AssertLabel(Color.Green, "C", child),
            child => Assert.IsType<Spacer>(child),
            child => AssertLabel(Color.Yellow, "X", child),
            child => AssertLabel(Color.Black, "Y", child),
            child => AssertLabel(Color.Yellow, "Z", child),
            child => Assert.IsType<Spacer>(child)
        );

        static void AssertLabel(Color color, string text, IView view)
        {
            var label = Assert.IsType<Label>(view);
            Assert.Equal(color, label.Color);
            Assert.Equal(text, label.Text);
        }
    }

    private IViewNode BuildTreeFromMarkup(string markup, object model)
    {
        var viewNodeFactory = new ViewNodeFactory(
            viewFactory,
            valueSourceFactory,
            valueConverterFactory,
            viewBinder,
            assetCache,
            resolutionScopeFactory
        );
        var document = Document.Parse(markup);
        var tree = viewNodeFactory.CreateNode(document);
        tree.Context = BindingContext.Create(model);
        tree.Update();
        return tree;
    }
}

file static class ViewNodeExtensions
{
    // Small helper to avoid having to specify the 'elapsed' argument in tests, where it is usually meaningless.
    public static bool Update(this IViewNode node)
    {
        return node.Update(TimeSpan.Zero);
    }
}
