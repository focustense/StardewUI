using System.Globalization;
using System.Text;

namespace StardewUI.Framework.Binding;

/// <summary>
/// Extensions for text types such as <c>ReadOnlySpan&lt;char&gt;.</c>
/// </summary>
internal static class TextExtensions
{
    private static readonly Rune DASH = new('-');

    /// <summary>
    /// Converts a kebab-case string (lowercase with hyphens) to UpperCamelCase or PascalCase.
    /// </summary>
    /// <param name="text">The text string to convert.</param>
    /// <returns>The <paramref name="text"/> with case conversion applied.</returns>
    public static string ToUpperCamelCase(this ReadOnlySpan<char> text)
    {
        using var _ = Trace.Begin(nameof(TextExtensions), nameof(ToUpperCamelCase));
        var sb = new StringBuilder(text.Length);
        bool capitalizeNext = true;
        foreach (var rune in text.EnumerateRunes())
        {
            if (rune == DASH)
            {
                capitalizeNext = true;
                continue;
            }
            if (capitalizeNext)
            {
                sb.Append(Rune.ToUpper(rune, CultureInfo.CurrentUICulture));
                capitalizeNext = false;
            }
            else
            {
                sb.Append(Rune.ToLower(rune, CultureInfo.CurrentUICulture));
            }
        }
        return sb.ToString();
    }
}
