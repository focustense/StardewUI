﻿using System.Buffers.Binary;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace StardewUI.Framework.Converters;

/// <summary>
/// String converter for the XNA <see cref="Color"/> type.
/// </summary>
/// <remarks>
/// Supports hex strings of the form <c>#rgb</c>, <c>#rrggbb</c>, or <c>#rrggbbaa</c>, as well as any of the XNA named
/// color strings like <c>LimeGreen</c>.
/// </remarks>
public class ColorConverter : IValueConverter<string, Color>
{
    private static readonly Dictionary<string, Lazy<Color>> namedColors = typeof(Color)
        .GetProperties(BindingFlags.Public | BindingFlags.Static)
        .Where(property => property.PropertyType == typeof(Color))
        .Select(property => (name: property.Name, color: new Lazy<Color>(() => (Color)property.GetValue(null)!)))
        .ToDictionary(x => x.name, x => x.color, StringComparer.OrdinalIgnoreCase);

    /// <inheritdoc />
    public Color Convert(string value)
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
}
