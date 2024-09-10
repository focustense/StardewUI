using StardewValley;

namespace StardewUI;

/// <summary>
/// Included game sprites that are required for many UI/menu widgets.
/// </summary>
public static class UiSprites
{
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
    /// Border/background sprite for an individual control, such as a button. Less prominent than
    /// <see cref="MenuBorder"/>.
    /// </summary>
    public static Sprite ControlBorder =>
        new(Game1.menuTexture, SourceRect: new(0, 256, 60, 60), FixedEdges: new(16));

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
    public static Sprite MenuHorizontalDivider => new(
        Game1.menuTexture,
        SourceRect: new(0, 64, 256, 64),
        FixedEdges: new(64, 0),
        SliceSettings: new(CenterX: 128));

    /// <summary>
    /// Margin adjustment to apply to content adjacent to a <see cref="MenuHorizontalDivider"/> to make content flush
    /// with the border; adjusts for internal sprite padding.
    /// </summary>
    public static Edges MenuHorizontalDividerMargin => new(-36, -20, -40, -20);

    /// <summary>
    /// Background for the scroll bar track (which the thumb is inside).
    /// </summary>
    public static Sprite ScrollBarTrack =>
        new(
            Game1.mouseCursors,
            SourceRect: new(403, 383, 6, 6),
            FixedEdges: new(2),
            SliceSettings: new(Scale: 4)
        );

    /// <summary>
    /// Background of a slider control.
    /// </summary>
    public static Sprite SliderBackground => new(
        Game1.mouseCursors,
        SourceRect: OptionsSlider.sliderBGSource,
        FixedEdges: new(2),
        SliceSettings: new(Scale: 4));

    /// <summary>
    /// The movable part of a slider control ("button").
    /// </summary>
    public static Sprite SliderButton => new(
        Game1.mouseCursors,
        SourceRect: OptionsSlider.sliderButtonRect,
        SliceSettings: new(Scale: 4));

    /// <summary>
    /// Small up arrow, typically used for scroll bars.
    /// </summary>
    public static Sprite SmallUpArrow => new(Game1.mouseCursors, SourceRect: new(421, 459, 11, 12));

    /// <summary>
    /// Small down arrow, typically used for scroll bars.
    /// </summary>
    public static Sprite SmallDownArrow =>
        new(Game1.mouseCursors, SourceRect: new(421, 472, 11, 12));

    /// <summary>
    /// Thumb sprite used for vertical scroll bars.
    /// </summary>
    public static Sprite VerticalScrollThumb =>
        new(Game1.mouseCursors, SourceRect: new(435, 463, 6, 10));
}

