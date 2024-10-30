namespace StardewUI.Layout;

/// <summary>
/// Controls the visibility of an <see cref="IView"/>.
/// </summary>
public enum Visibility
{
    /// <summary>
    /// The view is visible.
    /// </summary>
    Visible,

    /// <summary>
    /// The view is hidden.
    /// </summary>
    /// <remarks>
    /// Hidden views still participate in layout, but are not actually drawn. In a lane, grid, etc., there will be an
    /// empty space where the view would otherwise appear.
    /// </remarks>
    Hidden,
}
