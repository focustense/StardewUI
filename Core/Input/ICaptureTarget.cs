namespace StardewUI.Input;

/// <summary>
/// Denotes a view or other UI element that can be the active <see cref="StardewValley.IKeyboardSubscriber"/>. Allows
/// view hosts to provide deterministic release, e.g. when the mouse is clicked outside the target.
/// </summary>
/// <remarks>
/// This is primarily intended to work by checking if the <see cref="StardewValley.KeyboardDispatcher.Subscriber"/>
/// implements this interface, and if it's <see cref="CapturingView"/> belongs to the current click/focus tree. To work
/// correctly, both of these conditions must be met.
/// </remarks>
public interface ICaptureTarget
{
    /// <summary>
    /// The view that initiated the capturing. May be the same object as the <see cref="ICaptureTarget"/>, or may be the
    /// "owner" of a hidden <see cref="StardewValley.Menus.TextBox"/> or other
    /// <see cref="StardewValley.IKeyboardSubscriber"/>.
    /// </summary>
    IView CapturingView { get; }

    /// <summary>
    /// Stops input capturing from this target.
    /// </summary>
    void ReleaseCapture();
}
