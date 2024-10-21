namespace StardewUI.Layout;

/// <summary>
/// Signals that an <see cref="IView"/> implements tab controls.
/// </summary>
/// <remarks>
/// Tab controls are a gamepad function. While tabs are usually clickable and have their own navigation logic,
/// controller users should be able to use the trigger buttons to navigate tabs, which this interface enables.
/// </remarks>
public interface ITabbable
{
    /// <summary>
    /// Advance to the next top-level tab.
    /// </summary>
    /// <returns><c>true</c> if any navigation was performed; <c>false</c> if there are no more tabs.</returns>
    bool NextTab();

    /// <summary>
    /// Advance to the previous top-level tab.
    /// </summary>
    /// <returns><c>true</c> if any navigation was performed; <c>false</c> if there are no more tabs.</returns>
    bool PreviousTab();
}
