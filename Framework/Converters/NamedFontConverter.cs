using Microsoft.Xna.Framework.Graphics;

namespace StardewUI.Framework.Converters;

/// <summary>
/// Converter for fonts that are already built into the game, i.e. found on <see cref="Game1"/>.
/// </summary>
/// <remarks>
/// Does not account for fonts added as separate assets, which require bound attributes and not literals.
/// </remarks>
public class NamedFontConverter : IValueConverter<string, SpriteFont>
{
    public SpriteFont Convert(string value)
    {
        return value switch
        {
            "dialogue" => Game1.dialogueFont,
            "small" => Game1.smallFont,
            "tiny" => Game1.tinyFont,
            _ => throw new FormatException($"The font '{value}' is not a supported standard game font."),
        };
    }
}
