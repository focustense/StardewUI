using Microsoft.Xna.Framework;
using StardewUI.Framework.Converters;
using StardewUI.Graphics;

namespace StardewUI.Framework.Tests.Converters;

public class TransformConverterTests
{
    public static TheoryData<string, Transform> Data =>
        new()
        {
            { "translate: -20, 50", Transform.FromTranslation(new(-20, 50)) },
            { "ScaleY: 0.5; Rotate: 45; ScaleX: 2", new(new(2f, 0.5f), MathF.PI / 4, Vector2.Zero) },
            { "sCaLe:0.3,0.5;tRANsLAte:12,-16;", new(new(0.3f, 0.5f), 0, new(12, -16)) },
        };

    private readonly TransformConverter converter = new();

    [Theory]
    [MemberData(nameof(Data))]
    public void ConvertsStringToTransform(string value, Transform expectedTransform)
    {
        var transform = converter.Convert(value);

        Assert.Equal(expectedTransform, transform);
    }
}
