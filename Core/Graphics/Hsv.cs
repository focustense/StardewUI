using Microsoft.Xna.Framework;

namespace StardewUI.Graphics;

/// <summary>
/// A color in the HSV space.
/// </summary>
/// <param name="Hue">The color <see href="https://en.wikipedia.org/wiki/Hue">hue</see>.</param>
/// <param name="Saturation">The color <see href="https://en.wikipedia.org/wiki/Colorfulness">saturation</see>.</param>
/// <param name="Value">The color value or <see href="https://en.wikipedia.org/wiki/Brightness">brightness</see>.</param>
public record Hsv(int Hue, float Saturation, float Value)
{
    /// <summary>
    /// Converts an RGB color to its HSV equivalent.
    /// </summary>
    /// <param name="color">The RGB color.</param>
    /// <returns>The <paramref name="color"/> converted to HSV components.</returns>
    public static Hsv FromRgb(Color color)
    {
        float r = color.R / 255f;
        float g = color.G / 255f;
        float b = color.B / 255f;
        float value = MathF.Max(r, MathF.Max(g, b));
        float chroma = value - MathF.Min(r, MathF.Min(g, b));
        if (chroma == 0)
        {
            return new(0, 0, value);
        }
        float saturation = chroma / value;
        float hue;
        if (r == value)
        {
            hue = ((g - b) / chroma + 6) % 6;
        }
        else if (g == value)
        {
            hue = (b - r) / chroma + 2;
        }
        else
        {
            hue = (r - g) / chroma + 4;
        }
        return new((int)MathF.Round(hue * 60), saturation, value);
    }

    /// <summary>
    /// Converts this color to its RGB equivalent.
    /// </summary>
    /// <param name="alpha">Optional alpha component if not 1 (fully opaque).</param>
    /// <returns>The RGB color.</returns>
    public Color ToRgb(float alpha = 1)
    {
        float hue = Math.Clamp(Hue, 0, 360) / 60f;
        float chroma = Value * Saturation;
        float x = chroma * (1 - MathF.Abs(hue % 2 - 1));
        float m = Value - chroma;
        float r1,
            g1,
            b1;
        if (hue < 1)
        {
            (r1, g1, b1) = (chroma, x, 0);
        }
        else if (hue < 2)
        {
            (r1, g1, b1) = (x, chroma, 0);
        }
        else if (hue < 3)
        {
            (r1, g1, b1) = (0, chroma, x);
        }
        else if (hue < 4)
        {
            (r1, g1, b1) = (0, x, chroma);
        }
        else if (hue < 5)
        {
            (r1, g1, b1) = (x, 0, chroma);
        }
        else
        {
            (r1, g1, b1) = (chroma, 0, x);
        }
        return new(r1 + m, g1 + m, b1 + m, alpha);
    }
}
