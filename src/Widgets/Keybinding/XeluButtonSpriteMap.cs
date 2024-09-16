using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;

namespace StardewUI.Widgets.Keybinding;

/// <summary>
/// Controller/keyboard sprite map based on Xelu's CC0 pack: https://thoseawesomeguys.com/prompts/
/// </summary>
/// <remarks>
/// Uses specific sprites (Xbox-based) per gamepad button, with a fallback for unknown buttons. All keyboard keys use
/// the same placeholder border/background sprite with the expectation of having the key name drawn inside, in order to
/// at least be consistent with Stardew's fonts.
/// </remarks>
/// <param name="gamepad">Gamepad texture atlas, loaded from the mod's copy of <c>GamepadButtons.png</c>.</param>
/// <param name="keyboard">Keyboard texture atlas, loaded from the mod's copy of <c>KeyboardKeys.png</c>.</param>
/// <param name="mouse">Mouse texture atlas, loaded from the mod's copy of <c>MouseButtons.png</c>.</param>
public class XeluButtonSpriteMap(Texture2D gamepad, Texture2D keyboard, Texture2D mouse) : ButtonSpriteMap
{
    private const int COLUMN_COUNT = 4;

    private static readonly Edges KeyboardFixedEdges = new(32);
    private static readonly SliceSettings SliceSettings = new(Scale: 0.32f);
    private static readonly Point SpriteSize = new(100, 100);

    protected override Sprite ControllerBlank => new(gamepad, GetSourceRect(16));

    protected override Sprite KeyboardBlank => new(keyboard, GetSourceRect(0), KeyboardFixedEdges);

    protected override Sprite MouseLeft => new(mouse, GetSourceRect(1));

    protected override Sprite MouseMiddle => new(mouse, GetSourceRect(2));

    protected override Sprite MouseRight => new(mouse, GetSourceRect(3));

    protected override Sprite? Get(SButton button)
    {
        int? gamepadSpriteIndex = button switch
        {
            SButton.ControllerX => 0,
            SButton.ControllerA => 1,
            SButton.ControllerB => 2,
            SButton.ControllerY => 3,
            SButton.LeftTrigger => 4,
            SButton.RightTrigger => 5,
            SButton.LeftShoulder => 6,
            SButton.RightShoulder => 7,
            SButton.ControllerBack => 8,
            SButton.ControllerStart => 9,
            SButton.LeftStick => 10,
            SButton.RightStick => 11,
            SButton.DPadUp => 12,
            SButton.DPadDown => 13,
            SButton.DPadLeft => 14,
            SButton.DPadRight => 15,
            // There's no SButton for "D-pad without any pressed", so to make this sprite accessible we can use a key
            // that should never be used for any other in-game function.
            SButton.Sleep => 17,
            SButton.LeftThumbstickDown
            or SButton.LeftThumbstickLeft
            or SButton.LeftThumbstickRight
            or SButton.LeftThumbstickUp => 18,
            SButton.RightThumbstickDown
            or SButton.RightThumbstickRight
            or SButton.RightThumbstickRight
            or SButton.RightThumbstickUp => 19,
            _ => null,
        };
        return gamepadSpriteIndex.HasValue
            ? new(gamepad, GetSourceRect(gamepadSpriteIndex.Value), SliceSettings: SliceSettings)
            : null;
    }

    private static Rectangle GetSourceRect(int spriteIndex)
    {
        int row = spriteIndex / COLUMN_COUNT;
        int column = spriteIndex % COLUMN_COUNT;
        return new(new Point(column * SpriteSize.X, row * SpriteSize.Y), SpriteSize);
    }
}
