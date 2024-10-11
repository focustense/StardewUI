using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using PropertyChanged.SourceGenerator;
using StardewModdingAPI;
using StardewUI;
using StardewUI.Framework.Binding;
using StardewUI.Framework.Content;
using StardewUI.Framework.Converters;
using StardewUI.Framework.Dom;
using StardewUI.Framework.Grammar;
using StardewUI.Framework.Sources;
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
                ? (FakeAssetCacheEntry<T>)(asset)
                : throw new KeyNotFoundException($"Asset '{name}' not registered.");
        }

        public void Put<T>(string name, T asset)
            where T : notnull
        {
            if (assets.TryGetValue(name, out var oldValue) && oldValue is FakeAssetCacheEntry<T> oldEntry)
            {
                oldEntry.IsExpired = true;
            }
            assets[name] = new FakeAssetCacheEntry<T>(asset);
        }
    }

    class FakeAssetCacheEntry<T>(T asset) : IAssetCacheEntry<T>
    {
        public T Asset { get; } = asset;

        public bool IsExpired { get; set; }
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
        var eventBindingFactory = new EventBindingFactory(valueSourceFactory, valueConverterFactory);
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
            ],
            []
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
            [new SAttribute("is-checked", "Checked", ValueType: AttributeValueType.TwoWayBinding)],
            []
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
    public void TestEndToEnd()
    {
        var viewNodeFactory = new ViewNodeFactory(
            viewFactory,
            valueSourceFactory,
            valueConverterFactory,
            viewBinder,
            assetCache
        );
        assetCache.Put("Mods/TestMod/TestSprite", UiSprites.SmallTrashCan);

        string markup =
            @"<lane orientation=""vertical"" horizontal-content-alignment=""middle"" vertical-content-alignment=""end"">
                <image scale=""3.5"" sprite={@Mods/TestMod/TestSprite} />
                <label max-lines=""2"" text={HeaderText} />
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
                Assert.Equal(2, label.MaxLines);
                Assert.Equal("", label.Text);
            }
        );

        var model = new { HeaderText = "Some text" };
        tree.Context = BindingContext.Create(model);
        tree.Update();

        Assert.Equal("Some text", ((Label)rootView.Children[1]).Text);
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

        var dropdown = Assert.IsType<DropDownList<object>>(tree.Views.SingleOrDefault());
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

    private IViewNode BuildTreeFromMarkup(string markup, object model)
    {
        var viewNodeFactory = new ViewNodeFactory(
            viewFactory,
            valueSourceFactory,
            valueConverterFactory,
            viewBinder,
            assetCache
        );
        var document = Document.Parse(markup);
        var tree = viewNodeFactory.CreateNode(document.Root);
        tree.Context = BindingContext.Create(model);
        tree.Update();
        return tree;
    }
}
