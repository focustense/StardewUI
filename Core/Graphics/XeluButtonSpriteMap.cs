using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewUI.Layout;

namespace StardewUI.Graphics;

/// <summary>
/// Controller/keyboard sprite map based on <see href="https://thoseawesomeguys.com/prompts/">Xelu's CC0 pack</see>.
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
    /// <summary>
    /// Available theme variants for certain sprites.
    /// </summary>
    /// <remarks>
    /// Applies to the keyboard and mouse sprites, but not controller (Xbox style) sprites.
    /// </remarks>
    public enum SpriteTheme
    {
        /// <summary>
        /// Black and dark gray, with white highlights (e.g. for pressed mouse button).
        /// </summary>
        Dark,

        /// <summary>
        /// White and light gray, with red highlights (e.g. for pressed mouse button).
        /// </summary>
        Light,

        /// <summary>
        /// Custom theme mimicking the Stardew yellow-orange palette; falls back to <see cref="Light"/> for
        /// non-customized sprites.
        /// </summary>
        Stardew,
    }

    /// <summary>
    /// The active theme for keyboard sprites.
    /// </summary>
    public SpriteTheme KeyboardTheme { get; set; } = SpriteTheme.Stardew;

    /// <summary>
    /// The active theme for mouse sprites.
    /// </summary>
    public SpriteTheme MouseTheme { get; set; } = SpriteTheme.Stardew;

    /// <summary>
    /// Scale to apply to nine-slice sprites, specifically keyboard blanks.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This setting exists because the sprite assets are much larger than the space available for
    /// a typical keybind image in a menu, which - very unusually for Stardew - means they need to
    /// be scaled down, not up. However, some UIs (e.g. some overlays) may want to display these
    /// sprites at their normal size or larger, and in these cases, should not scale the slices
    /// because the borders would look strange or hard to see.
    /// </para>
    /// <para>
    /// In general, considering the base dimensions of 100x100, a comfortable size for menus
    /// targeting roughly 48px button height should use roughly 1/3 scale (0.3). Overlays and other
    /// UIs intending to render the sprite at full size (or larger) can leave the default of 1.
    /// </para>
    /// </remarks>
    public float SliceScale { get; set; } = 1f;

    private const int COLUMN_COUNT = 4;

    private static readonly Edges KeyboardFixedEdges = new(32);
    private static readonly Point SpriteSize = new(100, 100);

    /// <inheritdoc />
    protected override Sprite ControllerBlank => new(gamepad, GetSourceRect(16));

    /// <inheritdoc />
    protected override Sprite KeyboardBlank =>
        new(keyboard, GetKeyboardBlankSourceRect(), KeyboardFixedEdges, SliceSettings: new(Scale: SliceScale));

    /// <inheritdoc />
    protected override Sprite MouseLeft => new(mouse, GetMouseSourceRect(1));

    /// <inheritdoc />
    protected override Sprite MouseMiddle => new(mouse, GetMouseSourceRect(2));

    /// <inheritdoc />
    protected override Sprite MouseRight => new(mouse, GetMouseSourceRect(3));

    /// <inheritdoc />
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
            ? new(gamepad, GetSourceRect(gamepadSpriteIndex.Value), SliceSettings: new(Scale: SliceScale))
            : null;
    }

    private Rectangle GetKeyboardBlankSourceRect()
    {
        var spriteIndex = KeyboardTheme switch
        {
            SpriteTheme.Dark => 0,
            SpriteTheme.Light => 1,
            _ => 2,
        };
        return GetSourceRect(spriteIndex);
    }

    private Rectangle GetMouseSourceRect(int buttonIndex)
    {
        var baseIndex = MouseTheme == SpriteTheme.Dark ? 4 : 0;
        return GetSourceRect(baseIndex + buttonIndex);
    }

    private static Rectangle GetSourceRect(int spriteIndex)
    {
        int row = spriteIndex / COLUMN_COUNT;
        int column = spriteIndex % COLUMN_COUNT;
        return new(new Point(column * SpriteSize.X, row * SpriteSize.Y), SpriteSize);
    }
}
