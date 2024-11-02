using StardewUI.Framework.Converters;

namespace StardewUI.Framework.Tests;

public class DuckTypeEnumConverterFactoryTests
{
    private readonly DuckTypeEnumConverterFactory factory = new();

    enum SourceEnum
    {
        Foo = 1,
        Bar = 6,
        Baz = 14,
        Quux = 99,
    }

    enum DestinationEnum
    {
        Unknown,
        Baz = 1,
        Bar = 33,
        Foo = 50,
    }

    [Fact]
    public void WhenEnumsHaveCommonFields_ConvertsMatchedFields()
    {
        Assert.True(factory.TryGetConverter<SourceEnum, DestinationEnum>(out var converter));

        Assert.Equal(DestinationEnum.Foo, converter.Convert(SourceEnum.Foo));
        Assert.Equal(DestinationEnum.Bar, converter.Convert(SourceEnum.Bar));
        Assert.Equal(DestinationEnum.Baz, converter.Convert(SourceEnum.Baz));
    }

    [Fact]
    public void WhenEnumsPartiallyDiffer_UsesDefaultValueForUnmatchedFields()
    {
        Assert.True(factory.TryGetConverter<SourceEnum, DestinationEnum>(out var converter));

        Assert.Equal(DestinationEnum.Unknown, converter.Convert(SourceEnum.Quux));
        Assert.Equal(DestinationEnum.Unknown, converter.Convert((SourceEnum)1235));
    }

    enum NonMatchingEnum
    {
        Far = 1,
        Boo = 2,
        Squaz = 3,
    }

    [Fact]
    public void WhenEnumsHaveNoCommonFields_DoesNotCreateConverter()
    {
        Assert.False(factory.TryGetConverter<NonMatchingEnum, DestinationEnum>(out _));
    }
}
