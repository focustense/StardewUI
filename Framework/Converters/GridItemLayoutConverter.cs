namespace StardewUI.Framework.Converters;

/// <summary>
/// String converter for a <see cref="GridItemLayout"/>.
/// </summary>
public class GridItemLayoutConverter : IValueConverter<string, GridItemLayout>
{
    public GridItemLayout Convert(string value)
    {
        var valueAsSpan = value.ToLower().AsSpan();
        int separatorIndex = valueAsSpan.IndexOf(':');
        if (separatorIndex < 0)
        {
            throw new FormatException($"Invalid grid layout '{value}'; must have the format 'type: value'.");
        }
        var variantName = valueAsSpan[0..separatorIndex];
        var variantValue = valueAsSpan[(separatorIndex + 1)..];
        return variantName.Trim() switch
        {
            "count" => new GridItemLayout.Count(int.Parse(variantValue)),
            "length" => new GridItemLayout.Length(int.Parse(variantValue)),
            _ => throw new FormatException(
                $"Invalid variant type '{variantName}' for {typeof(GridItemLayout).Name}. "
                    + "Supported variants are 'count' or 'length'."
            ),
        };
    }
}
