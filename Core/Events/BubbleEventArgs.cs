namespace StardewUI.Events;

/// <summary>
/// Base class for events that can bubble up to parents from descendant views.
/// </summary>
public class BubbleEventArgs : EventArgs
{
    /// <summary>
    /// Whether or not the view receiving the event handled the event. Set to <c>true</c> to prevent bubbling.
    /// </summary>
    public bool Handled { get; set; }
}
