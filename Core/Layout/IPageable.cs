namespace StardewUI.Layout;

/// <summary>
/// Signals that an <see cref="IView"/> implements paging controls.
/// </summary>
/// <remarks>
/// Paging controls are a gamepad function. While next/previous arrows are usually clickable and have their own
/// navigation logic, controller users generally should be able to use the shoulder buttons to navigate pages, which
/// this interface enables.
/// </remarks>
public interface IPageable
{
    /// <summary>
    /// Advance to the next page, within the current tab.
    /// </summary>
    /// <returns><c>true</c> if any navigation was performed; <c>false</c> if there are no more pages.</returns>
    bool NextPage();

    /// <summary>
    /// Advance to the previous page, within the current tab.
    /// </summary>
    /// <returns><c>true</c> if any navigation was performed; <c>false</c> if there are no more pages.</returns>
    bool PreviousPage();
}
