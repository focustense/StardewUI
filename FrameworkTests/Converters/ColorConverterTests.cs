using Microsoft.Xna.Framework;
using StardewUI.Framework.Converters;

namespace StardewUI.Framework.Tests.Converters;

public class ColorConverterTests
{
    public static TheoryData<string, Color> Data =>
        new()
        {
            { "yellow", Color.Yellow },
            { "LightBlue", Color.LightBlue },
            { "dArKOrAnGe", Color.DarkOrange },
            { "#345", new(0x33, 0x44, 0x55) },
            { "#34a8", new(0x33, 0x44, 0xaa, 0x88) },
            { "#123456", new(0x12, 0x34, 0x56) },
            { "#12345678", new(0x12, 0x34, 0x56, 0x78) },
        };

    private readonly ColorConverter converter = new();

    [Theory]
    [MemberData(nameof(Data))]
    public void ConvertsStringToColor(string value, Color expectedColor)
    {
        var color = converter.Convert(value);

        Assert.Equal(expectedColor, color);
    }
}
