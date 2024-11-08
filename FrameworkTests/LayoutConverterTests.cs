using StardewUI.Framework.Converters;
using StardewUI.Layout;

namespace StardewUI.Framework.Tests;

public class LayoutConverterTests
{
    public static TheoryData<string, LayoutParameters> Data =>
        new()
        {
            {
                "stretch",
                new() { Width = Length.Stretch(), Height = Length.Stretch() }
            },
            {
                "content stretch",
                new() { Width = Length.Content(), Height = Length.Stretch() }
            },
            {
                "40px",
                new() { Width = Length.Px(40), Height = Length.Px(40) }
            },
            {
                "200px content",
                new() { Width = Length.Px(200), Height = Length.Content() }
            },
            {
                "50% 16px",
                new() { Width = Length.Percent(50), Height = Length.Px(16) }
            },
            {
                "50%[100..800] 24px",
                new()
                {
                    Width = Length.Percent(50),
                    MinWidth = 100,
                    MaxWidth = 800,
                    Height = Length.Px(24),
                }
            },
            {
                "80%[..600] content[22..]",
                new()
                {
                    Width = Length.Percent(80),
                    MaxWidth = 600,
                    Height = Length.Content(),
                    MinHeight = 22,
                }
            },
        };

    private readonly LayoutConverter converter = new();

    [Theory]
    [MemberData(nameof(Data))]
    public void ConvertsStringToLayout(string value, LayoutParameters expectedLayout)
    {
        var layout = converter.Convert(value);

        Assert.Equal(expectedLayout, layout);
    }
}
