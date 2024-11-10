using StardewUI.Layout;

namespace StardewUI.Framework.Converters;

/// <summary>
/// String converter for the <see cref="LayoutParameters"/> type.
/// </summary>
public class LayoutConverter : IValueConverter<string, LayoutParameters>
{
    /// <inheritdoc />
    public LayoutParameters Convert(string value)
    {
        var valueAsSpan = value.AsSpan().Trim();
        var separatorIndex = valueAsSpan.IndexOf(' ');
        var widthValue = separatorIndex >= 0 ? valueAsSpan[0..separatorIndex] : valueAsSpan;
        var (width, minWidth, maxWidth) = ParseComponents(widthValue);
        var (height, minHeight, maxHeight) =
            separatorIndex >= 0 ? ParseComponents(valueAsSpan[(separatorIndex + 1)..]) : (width, minWidth, maxWidth);
        return new()
        {
            Width = width,
            MinWidth = minWidth,
            MaxWidth = maxWidth,
            Height = height,
            MinHeight = minHeight,
            MaxHeight = maxHeight,
        };
    }

    private static (Length, float?, float?) ParseComponents(ReadOnlySpan<char> value)
    {
        value = value.Trim();
        int rangeStartIndex = value.IndexOf('[');
        if (rangeStartIndex < 0)
        {
            return (Length.Parse(value), null, null);
        }
        var length = Length.Parse(value[..rangeStartIndex]);
        var rangeString = value[rangeStartIndex..];
        if (!value.EndsWith("]"))
        {
            throw new FormatException($"Invalid layout range '{rangeString}': missing closing ']'.");
        }
        rangeString = rangeString[1..^1];
        var rangeSeparatorIndex = rangeString.IndexOf("..");
        if (rangeSeparatorIndex < 0)
        {
            throw new FormatException(
                $"Invalid layout range '{rangeString}': missing range separator '..' between min and max."
            );
        }
        var minString = rangeString[..rangeSeparatorIndex].Trim();
        float? minValue = minString.IsEmpty ? null : float.Parse(minString);
        var maxString = rangeString[(rangeSeparatorIndex + 2)..].Trim();
        float? maxValue = maxString.IsEmpty ? null : float.Parse(maxString);
        return (length, minValue, maxValue);
    }
}
