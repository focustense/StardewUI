namespace StardewUI.Framework.Converters;

/// <summary>
/// String converter for the XNA <see cref="Vector2"/> type.
/// </summary>
public class Vector2Converter : IValueConverter<string, Vector2>
{
    /// <inheritdoc />
    public Vector2 Convert(string value)
    {
        return Parse(value);
    }

    /// <summary>
    /// Parses a <see cref="Vector2"/> from a comma-separated value pair.
    /// </summary>
    /// <param name="value">The string value.</param>
    /// <returns>The parsed <see cref="Vector2"/>.</returns>
    /// <exception cref="FormatException">Thrown when <paramref name="value"/> is not a valid format.</exception>
    public static Vector2 Parse(ReadOnlySpan<char> value)
    {
        var separatorIndex = value.IndexOf(',');
        if (separatorIndex < 0)
        {
            throw new FormatException($"Invalid Vector2 string '{value}'. Vector2 strings must be of the form 'X,Y'.");
        }
        float x = float.Parse(value[..separatorIndex]);
        float y = float.Parse(value[(separatorIndex + 1)..]);
        return new(x, y);
    }
}
