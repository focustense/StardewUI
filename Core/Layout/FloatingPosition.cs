using Microsoft.Xna.Framework;

namespace StardewUI.Layout;

/// <summary>
/// Describes the position of a <see cref="FloatingElement"/>.
/// </summary>
/// <param name="offsetSelector">Calculates the position offset (relative to the parent) of the floating view. Takes the
/// measured floating view size, and then the parent size, as arguments.</param>
[DuckType]
public class FloatingPosition(Func<Vector2, Vector2, Vector2> offsetSelector)
{
    /// <summary>
    /// Positions the floating element immediately above the parent view, so that its bottom edge is flush with the
    /// parent's top edge.
    /// </summary>
    public static readonly FloatingPosition AboveParent = new((viewSize, _) => new(0, -viewSize.Y));

    /// <summary>
    /// Positions the floating element immediately to the right of (after) the parent view, so that its left edge is
    /// flush with the parent's right edge.
    /// </summary>
    public static readonly FloatingPosition AfterParent = new((_, parentSize) => new(parentSize.X, 0));

    /// <summary>
    /// Positions the floating element immediately to the left of (before) the parent view, so that its right edge is
    /// flush with the parent's left edge.
    /// </summary>
    public static readonly FloatingPosition BeforeParent = new((viewSize, _) => new(-viewSize.X, 0));

    /// <summary>
    /// Positions the floating element immediately below the parent view, so that its top edge is flush with the
    /// parent's bottom edge.
    /// </summary>
    public static readonly FloatingPosition BelowParent = new((_, parentSize) => new(0, parentSize.Y));

    /// <summary>
    /// Calculates the final position of the floating view.
    /// </summary>
    /// <param name="view">The floating view to position.</param>
    /// <param name="parentView">The parent relative to which the floating view is being positioned.</param>
    /// <returns>The final position where the <paramref name="view"/> should be drawn.</returns>
    public Vector2 GetOffset(IView view, View parentView)
    {
        return offsetSelector(view.OuterSize, parentView.OuterSize);
    }
}
