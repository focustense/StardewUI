using Microsoft.Xna.Framework;
using StardewUI.Graphics;

namespace StardewUI.Layout;

/// <summary>
/// Provides independent layout for an <see cref="IView"/> relative to its parent.
/// </summary>
/// <remarks>
/// <para>
/// Floating elements do not participate in the normal layout (<see cref="IView.Measure"/>) of the view that owns them;
/// they are excluded entirely from the flow, and then provided with their own measurement and position using the final
/// bounds of the parent (i.e. those that result from its non-floating elements).
/// </para>
/// <para>
/// This is primarily useful for annotations, callouts, or elements that are intentionally drawn outside their logical
/// container such as scrollbars or sidebars. Floating views <b>can</b> receive focus and clicks, but do not actually
/// capture the cursor like an <see cref="Overlays.Overlay"/> would, and therefore shouldn't be used for modal UI.
/// </para>
/// <para>
/// In general it is preferred to use standard layout controls like <see cref="Widgets.Lane"/> over floating elements,
/// but there are specific cases that justify floats, such as the aforementioned scrollbar which should display
/// "outside" the container regardless of how nested the container itself is - i.e. the float must "break out" of the
/// normal flow.
/// </para>
/// </remarks>
/// <param name="view">The floating view to display/interact with.</param>
/// <param name="position">Specifies how to position the <paramref name="view"/> relative to the parent and its own
/// measured size.</param>
public class FloatingElement(IView view, FloatingPosition position)
{
    /// <summary>
    /// The element's defined position.
    /// </summary>
    public FloatingPosition Position { get; } = position;

    /// <summary>
    /// The view to display within this element.
    /// </summary>
    public IView View { get; } = view;

    private Vector2 offset;

    /// <summary>
    /// Creates a <see cref="ViewChild"/> with the floating element's view and current position.
    /// </summary>
    /// <remarks>
    /// The result can generally be used as if it were any other non-floating element, e.g. for dispatching clicks,
    /// focus searches and other events.
    /// </remarks>
    /// <returns>The current element represented as a <see cref="ViewChild"/>.</returns>
    public ViewChild AsViewChild()
    {
        return new(View, offset);
    }

    /// <summary>
    /// Draws the element at its current position.
    /// </summary>
    /// <param name="spriteBatch">Sprite batch to hold the drawing output.</param>
    public void Draw(ISpriteBatch spriteBatch)
    {
        using var _ = spriteBatch.SaveTransform();
        spriteBatch.Translate(offset);
        View.Draw(spriteBatch);
    }

    /// <summary>
    /// Measures the view's content and repositions the entire floating element if necessary.
    /// </summary>
    /// <param name="parentView">The view whose <see cref="View.FloatingElements"/> this element belongs to. Required
    /// for repositioning when the layout has changed.</param>
    /// <param name="wasParentDirty">Whether this measurement is being done because the parent's layout already changed
    /// and therefore a reposition is always required (<c>true</c>), or whether to reposition only if the floating
    /// element's internal layout has changed (<c>false</c>).</param>
    public void MeasureAndPosition(View parentView, bool wasParentDirty)
    {
        // Floating views don't participate in the normal flow, so they can't affect the layout of the parent or any
        // ancestors (thus we don't return a bool here).
        //
        // In terms of whether any work needs to be done, that can happen if *either* the floating view's layout changed
        // *or* the parent view's layout was changed for other reasons (meaning, the floating position may have changed,
        // if it is derived from parent bounds, even if the floating view's size is the same).
        bool wasViewDirty = View.Measure(parentView.OuterSize);
        if (!wasViewDirty && !wasParentDirty)
        {
            return;
        }
        offset = Position.GetOffset(View, parentView);
    }
}
