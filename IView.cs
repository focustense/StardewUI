using Microsoft.Xna.Framework;

namespace SupplyChain.UI;

/// <summary>
/// Represents some arbitrary UI element or layout.
/// </summary>
public interface IView
{
    /// <summary>
    /// The true computed layout size resulting from a single <see cref="Measure"/> pass.
    /// </summary>
    Vector2 ActualSize { get; }

    /// <summary>
    /// The current layout parameters, which determine how <see cref="Measure"/> will behave.
    /// </summary>
    LayoutParameters Layout { get; set; }

    /// <summary>
    /// Draws the content for this view.
    /// </summary>
    /// <remarks>
    /// Drawing always happens after the measure pass, so <see cref="ContentSize"/> should be known and stable at this
    /// time, as long as the implementation itself is stable. Note that a position is not provided because the
    /// <see cref="ISpriteBatch"/> is pre-transformed; the top-left coordinates of this view are always (0, 0).
    /// </remarks>
    /// <param name="b">Sprite batch to hold the drawing output.</param>
    void Draw(ISpriteBatch b);

    /// <summary>
    /// Gets the current children of this view.
    /// </summary>
    IEnumerable<ViewChild> GetChildren();

    /// <summary>
    /// Checks whether or not the view is dirty - i.e. requires a new layout with a full <see cref="Measure"/>.
    /// </summary>
    /// <remarks>
    /// Typically, a view will be considered dirty if and only if one of the following are true:
    /// <list type="bullet">
    /// <item>The <see cref="Layout"/> has changed</item>
    /// <item>The content has changed in a way that could affect layout, e.g. the text has changed in a
    /// <see cref="LengthType.Content"/> configuration</item>
    /// <item>The <c>availableSize</c> is not the same as the previously-seen value (see remarks in
    /// <see cref="Measure"/>)</item>
    /// </list>
    /// A correct implementation is important for performance, as full layout can be very expensive to run on every
    /// frame.
    /// </remarks>
    /// <returns><c>true</c> if the view must be measured again; otherwise <c>false</c>.</returns>
    bool IsDirty();

    /// <summary>
    /// Computes the size of this view in the current pass, and stores the result in <see cref="ContentSize"/>.
    /// </summary>
    /// <remarks>
    /// Most views should save the value of <paramref name="availableSize"/> for use in <see cref="IsDirty"/> checks.
    /// </remarks>
    /// <param name="availableSize">The width/height that is still available in the container/parent.</param>
    /// <returns>Whether or not any layout was performed as a result of this pass. Callers may use this to propagate
    /// layout back up the tree, or perform expensive follow-up actions.</returns>
    bool Measure(Vector2 availableSize);
}
