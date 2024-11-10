using StardewUI.Framework.Converters;

namespace StardewUITestAddon;

internal class StringToKeySplineConverter : IValueConverter<string, KeySpline>
{
    public KeySpline Convert(string value)
    {
        value = value.ToLowerInvariant();
        if (value.StartsWith("ease"))
        {
            value = value[4..];
        }
        return value.ToLowerInvariant() switch
        {
            "linear" => KeySpline.Linear,
            "insine" => KeySpline.EaseInSine,
            "outsine" => KeySpline.EaseOutSine,
            "inoutsine" => KeySpline.EaseInOutSine,
            "inquad" => KeySpline.EaseInQuad,
            "outquad" => KeySpline.EaseOutQuad,
            "inoutquad" => KeySpline.EaseInOutQuad,
            "incubic" => KeySpline.EaseInCubic,
            "outcubic" => KeySpline.EaseOutCubic,
            "inoutcubic" => KeySpline.EaseInOutCubic,
            "inquart" => KeySpline.EaseInQuart,
            "outquart" => KeySpline.EaseOutQuart,
            "inoutquart" => KeySpline.EaseInOutQuart,
            "inquint" => KeySpline.EaseInQuint,
            "outquint" => KeySpline.EaseOutQuint,
            "inoutquint" => KeySpline.EaseInOutQuint,
            _ => ParseCustomBezier(value),
        };
    }

    private static KeySpline ParseCustomBezier(string value)
    {
        var values = value.Split(',');
        if (values.Length != 4)
        {
            throw new FormatException(
                $"Invalid cubic bezier value '{value}'. "
                    + $"Must be 4 numbers separated by commas, found {values.Length}."
            );
        }
        return new(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2]), float.Parse(values[3]));
    }
}
