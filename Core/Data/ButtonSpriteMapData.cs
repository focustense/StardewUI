using StardewUI.Graphics;

namespace StardewUI.Data;

/// <summary>
/// JSON configuration data for a <see cref="Graphics.ButtonSpriteMap"/>.
/// </summary>
/// <remarks>
/// The data is based on having the sprites themselves registered as assets, e.g. via <see cref="SpriteSheetData"/>.
/// </remarks>
public class ButtonSpriteMapData
{
    /// <summary>
    /// Map of buttons to the asset name used for that button's sprite.
    /// </summary>
    /// <remarks>
    /// Specific button sprites <b>replace</b> the <see cref="ControllerBlank"/> or <see cref="KeyboardBlank"/> and
    /// therefore must include both the border and inner icon/text.
    /// </remarks>
    public Dictionary<SButton, string> Buttons { get; set; } = [];

    /// <summary>
    /// Name of the sprite asset to use for <see cref="ButtonSpriteMap.ControllerBlank"/>.
    /// </summary>
    /// <remarks>
    /// Used as the background for any controller button lacking its own unique sprite; the button name is rendered
    /// inside as regular text.
    /// </remarks>
    public string ControllerBlank { get; set; } = "";

    /// <summary>
    /// Name of the sprite asset to use for <see cref="ButtonSpriteMap.KeyboardBlank"/>.
    /// </summary>
    /// <remarks>
    /// Used as the background for any key lacking its own unique sprite; the key name is rendered inside as regular
    /// text.
    /// </remarks>
    public string KeyboardBlank { get; set; } = "";

    /// <summary>
    /// Name of the sprite asset to use for <see cref="ButtonSpriteMap.MouseLeft"/>.
    /// </summary>
    public string MouseLeft { get; set; } = "";

    /// <summary>
    /// Name of the sprite asset to use for <see cref="ButtonSpriteMap.MouseMiddle"/>.
    /// </summary>
    public string MouseMiddle { get; set; } = "";

    /// <summary>
    /// Name of the sprite asset to use for <see cref="ButtonSpriteMap.MouseRight"/>.
    /// </summary>
    public string MouseRight { get; set; } = "";
}
