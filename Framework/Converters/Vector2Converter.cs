namespace StardewUI.Framework.Converters;

/// <summary>
/// String converter for the XNA <see cref="Vector2"/> type.
/// </summary>
public class Vector2Converter : IValueConverter<string, Vector2>
{
    public Vector2 Convert(string value)
    {
        var valueAsSpan = value.AsSpan();
        var separatorIndex = valueAsSpan.IndexOf(',');
        if (separatorIndex < 0)
        {
            throw new FormatException($"Invalid Vector2 string '{value}'. Vector2 strings must be of the form 'X,Y'.");
        }
        float x = float.Parse(valueAsSpan[..separatorIndex]);
        float y = float.Parse(valueAsSpan[(separatorIndex + 1)..]);
        return new(x, y);
    }
}
