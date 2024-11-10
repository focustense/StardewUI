namespace StardewUI.Widgets;

/// <summary>
/// Describes which shadow layers will be drawn, for widgets such as <see cref="Label"/> that support layered shadows.
/// </summary>
[Flags]
public enum ShadowLayers
{
    /// <summary>
    /// No layers; the shadow will not be drawn.
    /// </summary>
    None = 0,

    /// <summary>
    /// Diagonal shadow layer, with both a horizontal and vertical offset from the content.
    /// </summary>
    Diagonal = 1,

    /// <summary>
    /// Horizontal shadow layer, using only the horizontal offset from content and ignoring vertical offset.
    /// </summary>
    Horizontal = 2,

    /// <summary>
    /// Combination of <see cref="Horizontal"/> and <see cref="Diagonal"/> layers.
    /// </summary>
    HorizontalAndDiagonal = 3,

    /// <summary>
    /// Vertical shadow layer, using only the vertical offset from content and ignoring horizontal offset.
    /// </summary>
    Vertical = 4,

    /// <summary>
    /// Combination of <see cref="Vertical"/> and <see cref="Diagonal"/> layers.
    /// </summary>
    VerticalAndDiagonal = 5,

    /// <summary>
    /// Combination of <see cref="Horizontal"/> and <see cref="Vertical"/> layers.
    /// </summary>
    HorizontalAndVertical = 6,

    /// <summary>
    /// Includes all individual shadow layers.
    /// </summary>
    All = 7,
}
