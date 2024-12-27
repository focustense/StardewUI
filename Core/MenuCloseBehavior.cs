using StardewValley.Menus;

namespace StardewUI;

/// <summary>
/// Available behaviors for closing a <see cref="ViewMenu"/>.
/// </summary>
public enum MenuCloseBehavior
{
    /// <summary>
    /// Use the game's default closing logic.
    /// </summary>
    /// <remarks>
    /// Menu button presses (default: E, ESC, and gamepad B) on regular standalone or child menus will close the menu
    /// implicitly, via the game's update loop, using <see cref="IClickableMenu.exitThisMenu(bool)"/>. Submenus of the
    /// <see cref="TitleMenu"/> will close the menu explicitly, using the same method, when the equivalent button press
    /// is directly handled.
    /// </remarks>
    Default,

    /// <summary>
    /// Block the game's default closing logic, but allow the menu to be closed explicitly via its
    /// <see cref="ViewMenu.Close"/> method.
    /// </summary>
    /// <remarks>
    /// Causes <see cref="IClickableMenu.readyToClose"/> to return <c>false</c> at all times so that the game's own
    /// update loop will never close the menu; instead, the menu's <see cref="ViewMenu.CustomClose"/> method will be
    /// called whenever a menu button is pressed, which becomes fully responsible for the menu's removal.
    /// </remarks>
    Custom,

    /// <summary>
    /// Do not allow the menu to be closed by any means.
    /// </summary>
    /// <remarks>
    /// Causes <see cref="IClickableMenu.readyToClose"/> to return <c>false</c> at all times, and ignores requests to
    /// close the menu via the menu buttons. If the menu is configured with a clickable close button (X image), it will
    /// be hidden when the menu is in this state.
    /// </remarks>
    Disabled,
}
