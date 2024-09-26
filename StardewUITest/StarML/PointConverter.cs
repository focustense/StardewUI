using Microsoft.Xna.Framework;

namespace StardewUITest.StarML;

/// <summary>
/// String converter for the XNA <see cref="Point"/> type.
/// </summary>
public class PointConverter : IValueConverter<string, Point>
{
    public Point Convert(string value)
    {
        var valueAsSpan = value.AsSpan();
        var separatorIndex = valueAsSpan.IndexOf(',');
        if (separatorIndex < 0)
        {
            throw new FormatException($"Invalid Point string '{value}'. Point strings must be of the form 'X,Y'.");
        }
        int x = int.Parse(valueAsSpan[..separatorIndex]);
        int y = int.Parse(valueAsSpan[(separatorIndex + 1)..]);
        return new(x, y);
    }
}
