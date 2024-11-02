using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewUI.Framework.Converters;
using StardewUI.Graphics;
using StardewUI.Layout;
using StardewValley;

namespace StardewUI.Framework.Tests;

public class DuckTypeClassConverterFactoryTests
{
    class SourceClass
    {
        public string Bar = "";

        public int Foo { get; set; }

        [DuckProperty("unused")]
        [DuckProperty("baz")]
        public float RenamedBaz { get; set; }

        public string Option { get; set; } = "A";
    }

    enum OptionEnum
    {
        A,
        B,
        C,
    }

    [DuckType]
    class DestinationClass(int foo, string bar)
    {
        public int Foo { get; } = foo;

        public string Bar { get; set; } = bar;

        public float Baz { get; set; }

        public OptionEnum Option { get; set; }
    }

    private readonly DuckTypeClassConverterFactory factory;
    private readonly IValueConverterFactory rootFactory;

    public DuckTypeClassConverterFactoryTests()
    {
        var rootFactory = new RootValueConverterFactory();
        rootFactory.Register(new EnumNameConverterFactory());
        rootFactory.Register(new IdentityValueConverterFactory());
        factory = new(rootFactory);
        rootFactory.Register(factory);
        this.rootFactory = rootFactory;
    }

    [Fact]
    public void ConvertsConstructorArgs()
    {
        var converter = rootFactory.GetRequiredConverter<SourceClass, DestinationClass>();

        var source = new SourceClass() { Foo = 34, Bar = "Hello" };
        var destination = converter.Convert(source);

        Assert.Equal(34, destination.Foo);
        Assert.Equal("Hello", destination.Bar);
    }

    [Fact]
    public void ConvertsMatchingSourceField()
    {
        var converter = rootFactory.GetRequiredConverter<SourceClass, DestinationClass>();

        var source = new SourceClass() { Bar = "Hello" };
        var destination = converter.Convert(source);

        Assert.Equal("Hello", destination.Bar);
    }

    [Fact]
    public void ConvertsRenamedProperty()
    {
        var converter = rootFactory.GetRequiredConverter<SourceClass, DestinationClass>();

        var source = new SourceClass() { RenamedBaz = 123.45f };
        var destination = converter.Convert(source);

        Assert.Equal(123.45f, destination.Baz);
    }

    [Fact]
    public void ConvertsPropertyWithValueTypeConversion()
    {
        var converter = rootFactory.GetRequiredConverter<SourceClass, DestinationClass>();

        var source = new SourceClass() { Option = "C" };
        var destination = converter.Convert(source);

        Assert.Equal(OptionEnum.C, destination.Option);
    }

    record ExternalEdges(int Left, int Top, int Right, int Bottom);

    record ExternalSliceSettings(float Scale);

    record ExternalFullSprite(
        Texture2D Texture,
        Rectangle? SourceRect = null,
        ExternalEdges? FixedEdges = null,
        ExternalSliceSettings? SliceSettings = null
    );

    record ExternalPartialSprite(Texture2D Texture, Rectangle? SourceRect = null, ExternalEdges? FixedEdges = null);

    [Fact]
    public void ConvertsNestedDuckTypesWithExplicitParams()
    {
        var converter = rootFactory.GetRequiredConverter<ExternalFullSprite, Sprite>();

        // Game1.staminaRect won't be assigned here, but we're not interested in that for this test.
        // Many other unit and real-world tests already confirm that Texture2D objects can pass through.
        // This test covers the other duck-typed properties, we just have to give it *some* texture (even if we lie
        // about it having a value) so that the parameter exists, because Sprite requires it in the ctor.
        var source = new ExternalFullSprite(
            Game1.staminaRect,
            SourceRect: new(100, 200, 40, 60),
            FixedEdges: new(2, 4, 6, 8),
            SliceSettings: new(4.5f)
        );
        var destination = converter.Convert(source);

        Assert.Equal(new Rectangle(100, 200, 40, 60), destination.SourceRect);
        Assert.Equal(new Edges(2, 4, 6, 8), destination.FixedEdges);
        Assert.Equal(new SliceSettings(Scale: 4.5f), destination.SliceSettings);
    }

    [Fact]
    public void ConvertsNestedDuckTypesWithDefaultParams()
    {
        var converter = rootFactory.GetRequiredConverter<ExternalPartialSprite, Sprite>();

        var source = new ExternalPartialSprite(
            Game1.staminaRect,
            SourceRect: new(100, 200, 40, 60),
            FixedEdges: new(2, 4, 6, 8)
        );
        var destination = converter.Convert(source);

        Assert.Equal(new Rectangle(100, 200, 40, 60), destination.SourceRect);
        Assert.Equal(new Edges(2, 4, 6, 8), destination.FixedEdges);
        Assert.Null(destination.SliceSettings);
    }

    // Important to test with at least one struct - the required opcodes are different.
    struct SymmetricalEdges
    {
        [DuckProperty("Horizontal")]
        public int H { get; set; }

        [DuckProperty("Vertical")]
        public int V { get; set; }
    }

    [Fact]
    public void WhenLowerRankedConstructorMatches_ConvertsWithBestConstructor()
    {
        var converter = rootFactory.GetRequiredConverter<SymmetricalEdges, Edges>();

        var source = new SymmetricalEdges { H = 12, V = 5 };
        var destination = converter.Convert(source);

        Assert.Equal(12, destination.Left);
        Assert.Equal(12, destination.Right);
        Assert.Equal(5, destination.Top);
        Assert.Equal(5, destination.Bottom);
    }

    class RectLikeEdges
    {
        [DuckProperty("Left")]
        public int X;

        [DuckProperty("Top")]
        public int Y;

        public int Width { get; set; }

        public int Height { get; set; }

        [DuckProperty("Right")]
        public int X2 => X + Width;

        [DuckProperty("Bottom")]
        public int Y2 => Y + Height;
    }

    [Fact]
    public void WhenSourceHasComputedProperties_ConvertsAsRegularProperties()
    {
        var converter = rootFactory.GetRequiredConverter<RectLikeEdges, Edges>();

        var source = new RectLikeEdges
        {
            X = 50,
            Y = 20,
            Width = 16,
            Height = 9,
        };
        var destination = converter.Convert(source);

        Assert.Equal(50, destination.Left);
        Assert.Equal(66, destination.Right);
        Assert.Equal(20, destination.Top);
        Assert.Equal(29, destination.Bottom);
    }
}
