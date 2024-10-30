using Microsoft.Xna.Framework;
using StardewUI.Layout;

namespace StardewUI.Overlays;

/// <summary>
/// Definition of an overlay - i.e. a UI element that displays over all other UI.
/// </summary>
public interface IOverlay
{
    /// <summary>
    /// Event raised when the overlay is closed - i.e. removed from the current context stack.
    /// </summary>
    public event EventHandler<EventArgs>? Close;

    /// <summary>
    /// The parent of this overlay, used for positioning. If not specified, then the overlay will be positioned
    /// relative to the entire UI viewport.
    /// </summary>
    IView? Parent { get; }

    /// <summary>
    /// Horizontal alignment of the overlay relative to the <see cref="Parent"/> edge.
    /// </summary>
    /// <remarks>
    /// Specifies which edge of the overlay is used for positioning, regardless of which parent edge it is aligning to.
    /// For example, a <see cref="HorizontalAlignment"/> of <see cref="Alignment.Start"/> and a
    /// <see cref="HorizontalParentAlignment"/> of <see cref="Alignment.End"/> means that the overlay's left edge will
    /// be aligned to the parent's right edge; similarly, if both are set to <see cref="Alignment.Start"/>, then the
    /// overlay's left edge is aligned to the parent's <em>left</em> edge.
    /// </remarks>
    Alignment HorizontalAlignment { get; }

    /// <summary>
    /// Specifies which edge of the <see cref="Parent"/> (or screen, if no parent is specified) will be used to align
    /// the overlay edge denoted by its <see cref="HorizontalAlignment"/>.
    /// </summary>
    /// <remarks>
    /// For example, a <see cref="HorizontalAlignment"/> of <see cref="Alignment.Start"/> and a
    /// <see cref="HorizontalParentAlignment"/> of <see cref="Alignment.End"/> means that the overlay's left edge will
    /// be aligned to the parent's right edge; similarly, if both are set to <see cref="Alignment.Start"/>, then the
    /// overlay's left edge is aligned to the parent's <em>left</em> edge.
    /// </remarks>
    Alignment HorizontalParentAlignment { get; }

    /// <summary>
    /// Vertical alignment of the overlay relative to the <see cref="Parent"/> edge.
    /// </summary>
    /// <remarks>
    /// Specifies which edge of the overlay is used for positioning, regardless of which parent edge it is aligning to.
    /// For example, a <see cref="VerticalAlignment"/> of <see cref="Alignment.Start"/> and a
    /// <see cref="VerticalParentAlignment"/> of <see cref="Alignment.End"/> means that the overlay's top edge will
    /// be aligned to the parent's bottom edge; similarly, if both are set to <see cref="Alignment.Start"/>, then the
    /// overlay's top edge is aligned to the parent's <em>top</em> edge.
    /// </remarks>
    Alignment VerticalAlignment { get; }

    /// <summary>
    /// Specifies which edge of the <see cref="Parent"/> (or screen, if no parent is specified) will be used to align
    /// the overlay edge denoted by its <see cref="VerticalAlignment"/>.
    /// </summary>
    /// <remarks>
    /// For example, a <see cref="VerticalAlignment"/> of <see cref="Alignment.Start"/> and a
    /// <see cref="VerticalParentAlignment"/> of <see cref="Alignment.End"/> means that the overlay's top edge will
    /// be aligned to the parent's bottom edge; similarly, if both are set to <see cref="Alignment.Start"/>, then the
    /// overlay's top edge is aligned to the parent's <em>top</em> edge.
    /// </remarks>
    Alignment VerticalParentAlignment { get; }

    /// <summary>
    /// Additional pixel offset to apply to the overlay's position, after alignments.
    /// </summary>
    Vector2 ParentOffset { get; }

    /// <summary>
    /// Whether the overlay wants to capture all keyboard and gamepad inputs, i.e. prevent them from being dispatched
    /// to the parent menu.
    /// </summary>
    /// <remarks>
    /// This is not necessary to trap focus, which happens automatically; only to capture buttons/keys that would
    /// normally have a navigation function, like triggers/shoulders for paging, E/Esc/GamepadB for cancellation, etc.
    /// Overlays that enable capturing should provide their own way for the user to escape using keyboard/gamepad,
    /// although it is always possible to click the mouse outside the overlay to dismiss it (and implicitly stop the
    /// capturing).
    /// </remarks>
    bool CapturingInput => false;

    /// <summary>
    /// Amount to dim whatever is underneath the overlay.
    /// </summary>
    /// <remarks>
    /// This is an alpha value for a black overlay, so the higher value (between 0 and 1) the darker the content
    /// underneath the overlay. These apply individually to each overlay, so multiple stacked overlays will dim not only
    /// the underlying main view but also any previous overlays.
    /// </remarks>
    float DimmingAmount => 0;

    /// <summary>
    /// The view to be displayed/interacted with as an overlay.
    /// </summary>
    IView View { get; }

    /// <summary>
    /// Runs when the overlay is removed from the active stack.
    /// </summary>
    void OnClose();

    /// <summary>
    /// Runs on every game update tick.
    /// </summary>
    /// <param name="elapsed">The amount of real time elapsed since the last tick.</param>
    void Update(TimeSpan elapsed)
    {
        View.OnUpdate(elapsed);
    }
}

/// <summary>
/// A basic overlay with immutable properties.
/// </summary>
/// <param name="view">The <see cref="IOverlay.View"/>.</param>
/// <param name="parent">The <see cref="IOverlay.Parent"/>.</param>
/// <param name="horizontalAlignment">The <see cref="IOverlay.HorizontalAlignment"/>.</param>
/// <param name="horizontalParentAlignment">The <see cref="IOverlay.HorizontalParentAlignment"/>.</param>
/// <param name="verticalAlignment">The <see cref="IOverlay.VerticalAlignment"/>.</param>
/// <param name="verticalParentAlignment">The <see cref="IOverlay.VerticalParentAlignment"/>.</param>
/// <param name="parentOffset">The <see cref="IOverlay.ParentOffset"/>.</param>
public class Overlay(
    IView view,
    IView? parent = null,
    Alignment horizontalAlignment = Alignment.Middle,
    Alignment horizontalParentAlignment = Alignment.Middle,
    Alignment verticalAlignment = Alignment.Middle,
    Alignment verticalParentAlignment = Alignment.Middle,
    Vector2 parentOffset = default
) : IOverlay
{
    /// <summary>
    /// Raised when the overlay is removed from the active stack.
    /// </summary>
    public event EventHandler<EventArgs>? Close;

    /// <inheritdoc cref="OverlayContext.Pop()"/>.
    /// <remarks>
    /// Applies to the ambient <see cref="OverlayContext"/>, and is ignored if no context is available.
    /// </remarks>
    public static IOverlay? Pop()
    {
        return OverlayContext.Current?.Pop();
    }

    /// <summary>
    /// Pushes an overlay to the front.
    /// </summary>
    /// <remarks>
    /// <para>
    /// If the specified <paramref name="overlay"/> is already in the stack, then it will be moved from its previous
    /// position to the front.
    /// </para>
    /// <para>
    /// Applies to the ambient <see cref="OverlayContext"/>, and is ignored if no context is available.
    /// </para>
    /// </remarks>
    public static void Push(IOverlay overlay)
    {
        OverlayContext.Current?.Push(overlay);
    }

    /// <summary>
    /// Removes a specific overlay from the stack, regardless of its position.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This is most often invoked by an overlay needing to dismiss itself, e.g. an overlay with an "OK" or "Close"
    /// button.
    /// </para>
    /// <para>
    /// Applies to the ambient <see cref="OverlayContext"/>, and is ignored if no context is available.
    /// </para>
    /// </remarks>
    /// <param name="overlay">The overlay to remove.</param>
    /// <returns><c>true</c> if the <paramref name="overlay"/> was removed; <c>false</c> if it was not active.</returns>
    public static bool Remove(IOverlay overlay)
    {
        return OverlayContext.Current?.Remove(overlay) ?? false;
    }

    /// <inheritdoc/>
    public void OnClose()
    {
        Close?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Registers an action to be run when the overlay is closed.
    /// </summary>
    /// <remarks>
    /// Typically chained to the constructor when creating a new overlay.
    /// </remarks>
    /// <param name="onClose">The action to run on close.</param>
    /// <returns>The current <see cref="Overlay"/> instance.</returns>
    public Overlay OnClose(Action onClose)
    {
        Close += (_, _) => onClose();
        return this;
    }

    /// <inheritdoc/>
    public IView? Parent { get; } = parent;

    /// <inheritdoc/>
    public Alignment HorizontalAlignment { get; } = horizontalAlignment;

    /// <inheritdoc/>
    public Alignment HorizontalParentAlignment { get; } = horizontalParentAlignment;

    /// <inheritdoc/>
    public Alignment VerticalAlignment { get; } = verticalAlignment;

    /// <inheritdoc/>
    public Alignment VerticalParentAlignment { get; } = verticalParentAlignment;

    /// <inheritdoc/>
    public Vector2 ParentOffset { get; } = parentOffset;

    /// <inheritdoc/>
    public IView View { get; } = view;
}
