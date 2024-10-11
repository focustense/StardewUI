namespace StardewUI.Framework.Converters;

/// <summary>
/// String converter for the <see cref="LayoutParameters"/> type.
/// </summary>
/// <remarks>
/// Currently only supports specifying the normal width and height, not min/max values.
/// </remarks>
public class LayoutConverter : IValueConverter<string, LayoutParameters>
{
    /// <inheritdoc />
    public LayoutParameters Convert(string value)
    {
        var valueAsSpan = value.AsSpan().Trim();
        var separatorIndex = valueAsSpan.IndexOf(' ');
        var widthValue = separatorIndex >= 0 ? valueAsSpan[0..separatorIndex] : valueAsSpan;
        var width = ParseDimension(widthValue);
        var height = separatorIndex >= 0 ? ParseDimension(valueAsSpan[(separatorIndex + 1)..]) : width;
        return new() { Width = width, Height = height };
    }

    private static Length ParseDimension(ReadOnlySpan<char> value)
    {
        return value switch
        {
            "content" => Length.Content(),
            "stretch" => Length.Stretch(),
            [.., 'p', 'x'] => Length.Px(float.Parse(value[..^2])),
            [.., '%'] => Length.Percent(float.Parse(value[..^1])),
            _ => throw new FormatException(
                $"Invalid layout dimension '{value}'. "
                    + "Must be one of: 'content', 'stretch', or a number followed by 'px' or '%'."
            ),
        };
    }
}
