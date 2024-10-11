using System.Runtime.CompilerServices;

namespace StardewUI.Framework.Converters;

/// <summary>
/// String converter for the XNA <see cref="Rectangle"/> type.
/// </summary>
public class RectangleConverter : IValueConverter<string, Rectangle>
{
    /// <inheritdoc />
    public Rectangle Convert(string value)
    {
        return Parse(value);
    }

    /// <summary>
    /// Parses a <see cref="Rectangle"/> value from its comma-separated string representation.
    /// </summary>
    /// <remarks>
    /// This is equivalent to <see cref="Convert"/> but does not require an instance.
    /// </remarks>
    /// <param name="value">String representation of a <see cref="Rectangle"/>, having 4 comma-separated integer
    /// values.</param>
    /// <returns>The parsed <see cref="Rectangle"/>.</returns>
    /// <exception cref="FormatException">Thrown when <paramref name="value"/> cannot be parsed.</exception>
    public static Rectangle Parse(string value)
    {
        var valueAsSpan = value.AsSpan();
        var left = ReadNextInt(ref valueAsSpan);
        var top = ReadNextInt(ref valueAsSpan);
        var width = ReadNextInt(ref valueAsSpan);
        var height = ReadNextInt(ref valueAsSpan);
        if (!valueAsSpan.Trim().IsEmpty)
        {
            throw new FormatException(
                $"Invalid Rectangle value '{value}'; unexpected text '{valueAsSpan}' after 4th value."
            );
        }
        return new(left, top, width, height);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int ReadNextInt(ref ReadOnlySpan<char> remaining)
    {
        int nextSeparatorIndex = remaining.IndexOf(',');
        var value = nextSeparatorIndex >= 0 ? int.Parse(remaining[0..nextSeparatorIndex]) : int.Parse(remaining);
        remaining = nextSeparatorIndex >= 0 ? remaining[(nextSeparatorIndex + 1)..] : [];
        return value;
    }
}
