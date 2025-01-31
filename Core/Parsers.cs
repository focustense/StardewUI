using System.Buffers.Binary;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;

namespace StardewUI;

/// <summary>
/// Utilities for parsing third-party types, generally related to MonoGame/XNA.
/// </summary>
public static class Parsers
{
    private static readonly Dictionary<string, Lazy<Color>> namedColors = typeof(Color)
        .GetProperties(BindingFlags.Public | BindingFlags.Static)
        .Where(property => property.PropertyType == typeof(Color))
        .Select(property => (name: property.Name, color: new Lazy<Color>(() => (Color)property.GetValue(null)!)))
        .ToDictionary(x => x.name, x => x.color, StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Parses a named or hex color as a <see cref="Color"/>.
    /// </summary>
    /// <remarks>
    /// Supports hex strings of the form <c>#rgb</c>, <c>#rrggbb</c>, or <c>#rrggbbaa</c>, as well as any of the
    /// MonoGame named color strings like <c>LimeGreen</c>.
    /// </remarks>
    /// <param name="value">A string containing a named or hex color.a</param>
    /// <returns>The parsed color.</returns>
    public static Color ParseColor(string value)
    {
        if (!value.StartsWith('#'))
        {
            if (namedColors.TryGetValue(value, out var color))
            {
                return color.Value;
            }
            throw new FormatException(
                $"'{value}' is not a valid named color or hex color in #rgb, #rrggbb or #rrggbbaa form."
            );
        }
        var hexColor = value.AsSpan()[1..];
        return hexColor.Length switch
        {
            3 or 4 => ParseRgba4(hexColor),
            6 or 8 => ParseRgba8(hexColor),
            _ => throw new FormatException(
                $"'{value}' is not a valid color string. "
                    + $"Hex colors must be 3, 4, 6 or 8 digits (got {hexColor.Length} instead)."
            ),
        };
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
    public static Rectangle ParseRectangle(string value)
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

    /// <inheritdoc cref="ParseVector2(ReadOnlySpan{char})" />
    public static Vector2 ParseVector2(string value)
    {
        return ParseVector2(value.AsSpan());
    }

    /// <summary>
    /// Parses a <see cref="Vector2"/> from a comma-separated value pair.
    /// </summary>
    /// <param name="value">The string value.</param>
    /// <returns>The parsed <see cref="Vector2"/>.</returns>
    /// <exception cref="FormatException">Thrown when <paramref name="value"/> is not a valid format.</exception>
    public static Vector2 ParseVector2(ReadOnlySpan<char> value)
    {
        return TryParseVector2(value, out var result)
            ? result
            : throw new FormatException(
                $"Invalid Vector2 string '{value}'. Vector2 strings must be of the form 'X,Y'."
            );
    }

    /// <summary>
    /// Attempts to parse a named or hex color as a <see cref="Color"/>.
    /// </summary>
    /// <remarks>
    /// Supports hex strings of the form <c>#rgb</c>, <c>#rrggbb</c>, or <c>#rrggbbaa</c>, as well as any of the
    /// MonoGame named color strings like <c>LimeGreen</c>.
    /// </remarks>
    /// <param name="value">A string containing a named or hex color.a</param>
    /// <param name="color">The result if successful, otherwise a default <see cref="Color"/>.</param>
    /// <returns><c>true</c> if the <paramref name="value"/> was successfully parsed into <paramref name="color"/>;
    /// <c>false</c> if the parsing was unsuccessful.</returns>
    public static bool TryParseColor(string value, out Color color)
    {
        color = default;
        if (!value.StartsWith('#'))
        {
            if (namedColors.TryGetValue(value, out var namedColor))
            {
                color = namedColor.Value;
                return true;
            }
            return false;
        }
        var hexColor = value.AsSpan()[1..];
        return hexColor.Length switch
        {
            3 or 4 => TryParseRgba4(hexColor, out color),
            6 or 8 => TryParseRgba8(hexColor, out color),
            _ => false,
        };
    }

    /// <summary>
    /// Attempts to parse a <see cref="Vector2"/> from a comma-separated value pair.
    /// </summary>
    /// <param name="value">The string value.</param>
    /// <param name="result">The result if successful, otherwise a default <see cref="Vector2"/></param>.
    /// <returns><c>true</c> if the <paramref name="value"/> was successfully parsed into <paramref name="result"/>;
    /// <c>false</c> if the parsing was unsuccessful.</returns>
    public static bool TryParseVector2(ReadOnlySpan<char> value, out Vector2 result)
    {
        var separatorIndex = value.IndexOf(',');
        if (separatorIndex < 0)
        {
            result = default;
            return false;
        }
        float x = float.Parse(value[..separatorIndex]);
        float y = float.Parse(value[(separatorIndex + 1)..]);
        result = new(x, y);
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Color ParseRgba4(ReadOnlySpan<char> hexColor)
    {
        int rgba = int.Parse(hexColor, NumberStyles.HexNumber);
        var (c1, c2, c3, c4) = (
            ((rgba & 0xf000) >> 12) * 17,
            ((rgba & 0x0f00) >> 8) * 17,
            ((rgba & 0x00f0) >> 4) * 17,
            (rgba & 0x000f) * 17
        );
        return hexColor.Length == 4 ? new(c1, c2, c3, c4) : new(c2, c3, c4);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Color ParseRgba8(ReadOnlySpan<char> hexColor)
    {
        uint rgba = uint.Parse(hexColor, NumberStyles.HexNumber);
        if (hexColor.Length == 6)
        {
            rgba = rgba << 8 | 0xff;
        }
        return new(BinaryPrimitives.ReverseEndianness(rgba));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int ReadNextInt(ref ReadOnlySpan<char> remaining)
    {
        int nextSeparatorIndex = remaining.IndexOf(',');
        var value = nextSeparatorIndex >= 0 ? int.Parse(remaining[0..nextSeparatorIndex]) : int.Parse(remaining);
        remaining = nextSeparatorIndex >= 0 ? remaining[(nextSeparatorIndex + 1)..] : [];
        return value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool TryParseRgba4(ReadOnlySpan<char> hexColor, out Color color)
    {
        if (!int.TryParse(hexColor, NumberStyles.HexNumber, null, out var rgba))
        {
            color = default;
            return false;
        }
        var (c1, c2, c3, c4) = (
            ((rgba & 0xf000) >> 12) * 17,
            ((rgba & 0x0f00) >> 8) * 17,
            ((rgba & 0x00f0) >> 4) * 17,
            (rgba & 0x000f) * 17
        );
        color = hexColor.Length == 4 ? new(c1, c2, c3, c4) : new(c2, c3, c4);
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool TryParseRgba8(ReadOnlySpan<char> hexColor, out Color color)
    {
        if (!uint.TryParse(hexColor, NumberStyles.HexNumber, null, out var rgba))
        {
            color = default;
            return false;
        }
        if (hexColor.Length == 6)
        {
            rgba = rgba << 8 | 0xff;
        }
        color = new(BinaryPrimitives.ReverseEndianness(rgba));
        return true;
    }
}
