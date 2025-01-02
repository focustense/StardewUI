using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI.Events;
using StardewUI.Graphics;

namespace StardewUI.Framework;

/// <summary>
/// Sprite maps included with the framework, e.g. for button prompts, keybinds, etc.
/// </summary>
internal class SpriteMaps(IModHelper helper)
{
    /// <summary>
    /// Asset name prefix that should be used for all sprite assets.
    /// </summary>
    public static readonly string AssetNamePrefix = "Mods/StardewUI/SpriteMaps/";

    /// <summary>
    /// Sprite sheet containing the sprites for gamepad buttons.
    /// </summary>
    public Texture2D KeybindButtons => keybindButtonsTexture.Asset;

    /// <summary>
    /// Sprite sheet containing the sprites used for keyboard keys.
    /// </summary>
    public Texture2D KeybindKeys => keybindKeysTexture.Asset;

    /// <summary>
    /// Sprite sheet containing the sprites used for mouse buttons.
    /// </summary>
    public Texture2D MouseButtons => mouseButtonsTexture.Asset;

    /// <summary>
    /// Sprite sheet containing arrows used in button prompts.
    /// </summary>
    public Texture2D PromptArrows => promptArrowsTexture.Asset;

    private readonly LazyAsset<Texture2D> keybindButtonsTexture = new(
        helper,
        $"Mods/{helper.ModContent.ModID}/KeybindButtons",
        "assets/XboxButtons.png"
    );

    private readonly LazyAsset<Texture2D> keybindKeysTexture = new(
        helper,
        $"Mods/{helper.ModContent.ModID}/KeybindKeys",
        "assets/KeyboardKeys.png"
    );

    private readonly LazyAsset<Texture2D> mouseButtonsTexture = new(
        helper,
        $"Mods/{helper.ModContent.ModID}/MouseButtons",
        "assets/MouseButtons.png"
    );

    private readonly LazyAsset<Texture2D> promptArrowsTexture = new(
        helper,
        $"Mods/{helper.ModContent.ModID}/PromptArrows",
        "assets/PromptArrows.png"
    );

    /// <summary>
    /// Gets the sprite map for gamepad buttons/keyboard keys used in keybinds/button prompts.
    /// </summary>
    /// <param name="keyboardThemeName">Name of the color theme to use for keyboard button prompts.</param>
    /// <param name="mouseThemeName">Name of the color theme to use for mouse button prompts.</param>
    /// <param name="sliceScale">The <see cref="SliceSettings.Scale"/> to apply to any 9-slice sprites.</param>
    public ISpriteMap<SButton> GetButtonSpriteMap(
        string? keyboardThemeName = null,
        string? mouseThemeName = null,
        float sliceScale = 1.0f
    )
    {
        return new XeluButtonSpriteMap(KeybindButtons, KeybindKeys, MouseButtons)
        {
            KeyboardTheme = ParseOptionalTheme(keyboardThemeName, XeluButtonSpriteMap.SpriteTheme.Stardew),
            MouseTheme = ParseOptionalTheme(mouseThemeName, XeluButtonSpriteMap.SpriteTheme.Stardew),
            SliceScale = sliceScale,
        };
    }

    /// <summary>
    /// Gets the sprite map for arrows used in positioning and other UI prompts.
    /// </summary>
    public ISpriteMap<Direction> GetDirectionSpriteMap()
    {
        return new SpriteMapBuilder<Direction>(PromptArrows)
            .Size(64, 64)
            .Add(Direction.North, Direction.South, Direction.West, Direction.East)
            .Build();
    }

    private static XeluButtonSpriteMap.SpriteTheme ParseOptionalTheme(
        string? themeName,
        XeluButtonSpriteMap.SpriteTheme defaultValue
    )
    {
        if (string.IsNullOrEmpty(themeName) || themeName.Equals("default", StringComparison.OrdinalIgnoreCase))
        {
            return defaultValue;
        }
        return Enum.TryParse<XeluButtonSpriteMap.SpriteTheme>(themeName, true, out var result)
            ? result
            : throw new ArgumentException(
                $"Invalid theme name '{themeName}' specified for button sprites.",
                nameof(themeName)
            );
    }
}
