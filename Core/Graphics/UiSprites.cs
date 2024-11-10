using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewUI.Layout;
using StardewValley;
using StardewValley.Menus;

namespace StardewUI.Graphics;

/// <summary>
/// Included game sprites that are required for many UI/menu widgets.
/// </summary>
public static class UiSprites
{
    private static readonly IReadOnlyList<Rectangle> digitRects = Enumerable
        .Range(0, 10)
        .Select(i => new Rectangle(368 + 5 * i, 56, 5, 7))
        .ToList();
    private static Texture2D textBoxTexture => Game1.content.Load<Texture2D>("LooseSprites/textBox");

    /// <summary>
    /// Background for the a banner or "scroll" style text, often used for menu/dialogue titles.
    /// </summary>
    public static Sprite BannerBackground =>
        new(
            Game1.mouseCursors,
            SourceRect: new(325, 318, 25, 18),
            FixedEdges: new(12, 0),
            SliceSettings: new(Scale: 4)
        );

    /// <summary>
    /// Button with a darker background, usually the neutral state.
    /// </summary>
    public static Sprite ButtonDark =>
        new(Game1.mouseCursors, SourceRect: new(256, 256, 10, 10), FixedEdges: new(2), SliceSettings: new(Scale: 4));

    /// <summary>
    /// Button with a lighter background, usually used to show hover state.
    /// </summary>
    public static Sprite ButtonLight =>
        new(Game1.mouseCursors, SourceRect: new(267, 256, 10, 10), FixedEdges: new(2), SliceSettings: new(Scale: 4));

    /// <summary>
    /// A caret-style directional arrow pointing left.
    /// </summary>
    /// <remarks>
    /// Can be used to show expanded/collapsed state, or illustrate a movement direction.
    /// </remarks>
    public static Sprite CaretLeft => new(Game1.mouseCursors, SourceRect: new(480, 96, 24, 32));

    /// <summary>
    /// A caret-style directional arrow pointing right.
    /// </summary>
    /// <remarks>
    /// Can be used to show expanded/collapsed state, or illustrate a movement direction.
    /// </remarks>
    public static Sprite CaretRight => new(Game1.mouseCursors, SourceRect: new(448, 96, 24, 32));

    /// <summary>
    /// Checkbox with a green "X" through it.
    /// </summary>
    public static Sprite CheckboxChecked =>
        new(Game1.mouseCursors, SourceRect: new(236, 425, 9, 9), FixedEdges: new(1), SliceSettings: new(Scale: 4));

    /// <summary>
    /// Unchecked checkbox, i.e. only the border.
    /// </summary>
    public static Sprite CheckboxUnchecked =>
        new(Game1.mouseCursors, SourceRect: new(227, 425, 9, 9), FixedEdges: new(1), SliceSettings: new(Scale: 4));

    /// <summary>
    /// Border/background sprite for an individual control, such as a button. Less prominent than
    /// <see cref="MenuBorder"/>.
    /// </summary>
    public static Sprite ControlBorder => new(Game1.menuTexture, SourceRect: new(0, 256, 60, 60), FixedEdges: new(16));

    /// <summary>
    /// List of sprites for the outlined "tiny digits" 0-9, in that order.
    /// </summary>
    public static IReadOnlyList<Sprite> Digits =>
        digitRects.Select(rect => new Sprite(Game1.mouseCursors, rect)).ToList();

    /// <summary>
    /// Background of a drop-down menu.
    /// </summary>
    public static Sprite DropDownBackground =>
        new(
            Game1.mouseCursors,
            SourceRect: OptionsDropDown.dropDownBGSource,
            FixedEdges: new(1),
            SliceSettings: new(Scale: 4)
        );

    /// <summary>
    /// Button to pull down a drop-down menu.
    /// </summary>
    public static Sprite DropDownButton =>
        new(
            Game1.mouseCursors,
            SourceRect: OptionsDropDown.dropDownButtonSource,
            FixedEdges: new(1, 3, 3, 3),
            SliceSettings: new(Scale: 4)
        );

    /// <summary>
    /// Simpler, lighter horizontal divider than the <see cref="MenuHorizontalDivider"/>, used as a horizontal rule to
    /// separate content areas without sectioning the entire menu.
    /// </summary>
    public static Sprite GenericHorizontalDivider => new(Game1.menuTexture, SourceRect: new(64, 412, 64, 8));

    /// <summary>
    /// Large down arrow, used for macro navigation.
    /// </summary>
    public static Sprite LargeDownArrow => new(Game1.mouseCursors, SourceRect: new(0, 64, 64, 64));

    /// <summary>
    /// Large left arrow, used for macro navigation.
    /// </summary>
    public static Sprite LargeLeftArrow => new(Game1.mouseCursors, SourceRect: new(0, 256, 64, 64));

    /// <summary>
    /// Large right arrow, used for macro navigation.
    /// </summary>
    public static Sprite LargeRightArrow => new(Game1.mouseCursors, SourceRect: new(0, 192, 64, 64));

    /// <summary>
    /// Large up arrow, used for macro navigation.
    /// </summary>
    public static Sprite LargeUpArrow => new(Game1.mouseCursors, SourceRect: new(64, 64, 64, 64));

    /// <summary>
    /// Background used for the in-game menu, not including borders.
    /// </summary>
    public static Sprite MenuBackground => new(Game1.menuTexture, SourceRect: new(64, 128, 64, 64));

    /// <summary>
    /// Modified 9-slice sprite used for the menu border, based on menu "tiles". Used for drawing the outer border of an
    /// entire menu UI.
    /// </summary>
    public static Sprite MenuBorder =>
        new(
            Game1.menuTexture,
            SourceRect: new(0, 0, 256, 256),
            FixedEdges: new(64),
            SliceSettings: new(CenterX: 128, CenterY: 128, EdgesOnly: true)
        );

    /// <summary>
    /// The actual distance from the outer edges of the <see cref="MenuBorder"/> sprite to where the actual "border"
    /// really ends, in terms of pixels. The border tiles are quite large, so this tends to be needed in order to
    /// determine where the content should go without adding a ton of extra padding.
    /// </summary>
    public static Edges MenuBorderThickness => new(36, 36, 40, 36);

    /// <summary>
    /// Modified 9-slice sprite used for the menu's horizontal divider, meant to be drawn over top of the
    /// <see cref="MenuBorder"/> to denote separate "sub-panels" or "sections" of the menu to group logically very
    /// different menu functions (as opposed to lines on a grid).
    /// </summary>
    public static Sprite MenuHorizontalDivider =>
        new(
            Game1.menuTexture,
            SourceRect: new(0, 64, 256, 64),
            FixedEdges: new(64, 0),
            SliceSettings: new(CenterX: 128)
        );

    /// <summary>
    /// Margin adjustment to apply to content adjacent to a <see cref="MenuHorizontalDivider"/> to make content flush
    /// with the border; adjusts for internal sprite padding.
    /// </summary>
    public static Edges MenuHorizontalDividerMargin => new(-36, -20, -40, -20);

    /// <summary>
    /// Inset-style background and border, often used to hold an item or represent a slot.
    /// </summary>
    public static Sprite MenuSlotInset => new(Game1.menuTexture, SourceRect: new(0, 320, 60, 60), FixedEdges: new(9));

    /// <summary>
    /// Outset-style background and border, often used to hold an item or represent a slot.
    /// </summary>
    public static Sprite MenuSlotOutset => new(Game1.menuTexture, SourceRect: new(64, 320, 60, 60), FixedEdges: new(8));

    /// <summary>
    /// Single-line rectangular border with a slight inset look.
    /// </summary>
    public static Sprite MenuSlotTransparent =>
        new(Game1.menuTexture, SourceRect: new(128, 128, 64, 64), FixedEdges: new(4));

    /// <summary>
    /// Background for the scroll bar track (which the thumb is inside).
    /// </summary>
    public static Sprite ScrollBarTrack =>
        new(Game1.mouseCursors, SourceRect: new(403, 383, 6, 6), FixedEdges: new(2), SliceSettings: new(Scale: 4));

    /// <summary>
    /// Background of a slider control.
    /// </summary>
    public static Sprite SliderBackground =>
        new(
            Game1.mouseCursors,
            SourceRect: OptionsSlider.sliderBGSource,
            FixedEdges: new(2),
            SliceSettings: new(Scale: 4)
        );

    /// <summary>
    /// The movable part of a slider control ("button").
    /// </summary>
    public static Sprite SliderButton =>
        new(Game1.mouseCursors, SourceRect: OptionsSlider.sliderButtonRect, SliceSettings: new(Scale: 4));

    /// <summary>
    /// Small down arrow, typically used for scroll bars.
    /// </summary>
    public static Sprite SmallDownArrow => new(Game1.mouseCursors, SourceRect: new(421, 472, 11, 12));

    /// <summary>
    /// Small left arrow, typically used for top-level list navigation.
    /// </summary>
    public static Sprite SmallLeftArrow => new(Game1.mouseCursors, SourceRect: new(352, 495, 12, 11));

    /// <summary>
    /// Small right arrow, typically used for top-level list navigation.
    /// </summary>
    public static Sprite SmallRightArrow => new(Game1.mouseCursors, SourceRect: new(365, 495, 12, 11));

    /// <summary>
    /// Small up arrow, typically used for scroll bars.
    /// </summary>
    public static Sprite SmallUpArrow => new(Game1.mouseCursors, SourceRect: new(421, 459, 11, 12));

    /// <summary>
    /// A small green "+" icon.
    /// </summary>
    /// <remarks>
    /// Technically used to represent energy buffs, can sometimes be tinted to communicate a concept like "add to list".
    /// </remarks>
    public static Sprite SmallGreenPlus => new(Game1.mouseCursors, SourceRect: new(0, 428, 10, 10));

    /// <summary>
    /// Small and tall trash can, larger than the <see cref="TinyTrashCan"/> and more suitable for tall rows.
    /// </summary>
    public static Sprite SmallTrashCan => new(Game1.mouseCursors2, SourceRect: new(22, 11, 15, 20));

    /// <summary>
    /// Top-facing tab with no inner content, used for tab controls.
    /// </summary>
    public static Sprite TabTopEmpty =>
        new(
            Game1.mouseCursors,
            SourceRect: new(16, 368, 16, 16),
            FixedEdges: new(5, 5, 5, 1),
            SliceSettings: new(Scale: 4)
        );

    /// <summary>
    /// Border/background for a text input box.
    /// </summary>
    public static Sprite TextBox => new(textBoxTexture, FixedEdges: new(16, 12, 12, 12));

    /// <summary>
    /// Very small trash can, e.g. to be used in lists/subforms as "remove" button.
    /// </summary>
    public static Sprite TinyTrashCan => new(Game1.mouseCursors, SourceRect: new(323, 433, 9, 10));

    /// <summary>
    /// Thumb sprite used for vertical scroll bars.
    /// </summary>
    public static Sprite VerticalScrollThumb => new(Game1.mouseCursors, SourceRect: new(435, 463, 6, 10));
}
