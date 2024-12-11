using Microsoft.Xna.Framework;
using StardewUI.Graphics;

namespace StardewUI.Framework.Converters;

/// <summary>
/// String converter for the <see cref="Transform"/> type.
/// </summary>
/// <remarks>
/// <para>
/// Valid strings must be a semicolon-separated list of one of the valid transform properties, followed by a colon,
/// followed by the property value.
/// </para>
/// <para>
/// Valid property names include: <c>translate</c>, <c>translateX</c>, <c>translateY</c>, <c>rotate</c>,
/// <c>scale</c>, <c>scaleX</c> and <c>scaleY</c>.
/// </para>
/// <para>
/// The value following each property should be a number, except for <c>translate</c> and <c>scale</c> which should
/// be a <see cref="Vector2"/> compatible string such as <c>2, -4</c> instead. Rotation values are interpreted as
/// instead of radians for improved readability.
/// </para>
/// </remarks>
public class TransformConverter : IValueConverter<string, Transform>
{
    /// <inheritdoc />
    /// <exception cref="FormatException">Thrown when <paramref name="value"/> is not in the correct format or specifies
    /// invalid transformation properties.</exception>
    public Transform Convert(string value)
    {
        var remaining = value.AsSpan();
        Vector2 scale = Vector2.One;
        float rotation = 0;
        Vector2 translation = Vector2.Zero;
        while (!remaining.IsEmpty)
        {
            var tokenSeparatorIndex = remaining.IndexOf(';');
            var token = tokenSeparatorIndex >= 0 ? remaining[..tokenSeparatorIndex] : remaining;
            remaining = tokenSeparatorIndex >= 0 ? remaining[(tokenSeparatorIndex + 1)..] : [];
            var valueSeparatorIndex = token.IndexOf(':');
            if (valueSeparatorIndex < 0)
            {
                throw new FormatException(
                    $"Invalid component '{token}' in transform string. Transform components must consist of a name and "
                        + "value, separated by a colon ':'."
                );
            }
            var componentName = token[..valueSeparatorIndex].Trim();
            var componentValue = token[(valueSeparatorIndex + 1)..].Trim();
            // csharpier-ignore-start
            switch (componentName)
            {
                case ['S' or 's', 'C' or 'c', 'A' or 'a', 'L' or 'l', 'E' or 'e', 'X' or 'x' or 'Y' or 'y']:
                    scale = componentName[^1] switch
                    {
                        'X' or 'x' => new(float.Parse(componentValue), scale.Y),
                        _ => new(scale.X, float.Parse(componentValue)),
                    };
                    break;
                case ['S' or 's', 'C' or 'c', 'A' or 'a', 'L' or 'l', 'E' or 'e']:
                    scale = float.TryParse(componentValue, out var uniformScale)
                        ? new(uniformScale, uniformScale)
                        : Vector2Converter.Parse(componentValue);
                    break;
                case ['R' or 'r', 'O' or 'o', 'T' or 't', 'A' or 'a', 'T' or 't', 'E' or 'e']:
                    rotation = MathHelper.ToRadians(float.Parse(componentValue));
                    break;
                case [
                    'T' or 't', 'R' or 'r', 'A' or 'a', 'N' or 'n', 'S' or 's', 'L' or 'l', 'A' or 'a', 'T' or 't', 'E' or 'e',
                    'X' or 'x' or 'Y' or 'y',
                ]:
                    translation = componentName[^1] switch
                    {
                        'X' or 'x' => new(float.Parse(componentValue), translation.Y),
                        _ => new(translation.X, float.Parse(componentValue)),
                    };
                    break;
                case ['T' or 't', 'R' or 'r', 'A' or 'a', 'N' or 'n', 'S' or 's', 'L' or 'l', 'A' or 'a', 'T' or 't', 'E' or 'e']:
                    translation = Vector2Converter.Parse(componentValue);
                    break;
                default:
                    throw new FormatException($"Unrecognized transform component '{componentName}.");
            }
            // csharpier-ignore-end
        }
        return new(scale, rotation, translation);
    }
}
