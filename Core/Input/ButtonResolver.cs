using StardewModdingAPI;
using StardewValley;

namespace StardewUI.Input;

/// <summary>
/// The actions that a given button can trigger in a UI context. For details see <see cref="ButtonResolver"/>.
/// </summary>
public enum ButtonAction
{
    /// <summary>
    /// The button has no standard UI behavior.
    /// </summary>
    None,

    /// <summary>
    /// Used for primary interaction, or "left click".
    /// </summary>
    Primary,

    /// <summary>
    /// Used for secondary interaction, or "right click".
    /// </summary>
    Secondary,

    /// <summary>
    /// Cancels out of the current menu, overlay, etc.
    /// </summary>
    Cancel,
}

/// <summary>
/// Helper for resolving button state reported by vanilla menu code.
/// </summary>
/// <remarks>
/// <para>
/// Stardew's menu system is quite obnoxious about trying to "simplify" button handling and doesn't provide a lot of
/// escape hatches. In addition, the buttons it considers to be the same are not equivalent to the way normal gameplay
/// operates; for example, gamepad controls use A as the action button which is the same as a right-click when
/// interacting with the game world; however, in menus A is the same as a left-click while X is right click.
/// </para>
/// <para>
/// Going through this class can help identify the correct "function" of a button in a UI context as well as identify
/// which real button was actually pressed, the better to work with input suppressions and similar concerns.
/// </para>
/// </remarks>
public static class ButtonResolver
{
    /// <summary>
    /// Gets all buttons that can be resolved to a specific action.
    /// </summary>
    /// <param name="action">The UI action.</param>
    /// <returns>A sequence of <see cref="SButton"/> elements each of which is considered to perform the specified
    /// <paramref name="action"/>.</returns>
    public static IEnumerable<SButton> GetActionButtons(ButtonAction action)
    {
        return action switch
        {
            ButtonAction.Primary => [SButton.ControllerA, .. Game1.options.useToolButton.Select(b => b.ToSButton())],
            ButtonAction.Secondary => [SButton.ControllerX, .. Game1.options.actionButton.Select(b => b.ToSButton())],
            ButtonAction.Cancel => [SButton.ControllerB, .. Game1.options.menuButton.Select(b => b.ToSButton())],
            _ => [],
        };
    }

    /// <summary>
    /// Determines the action that should be performed by a button.
    /// </summary>
    /// <param name="button">The action button.</param>
    /// <returns>The action for the specified <paramref name="button"/>.</returns>
    public static ButtonAction GetButtonAction(SButton button)
    {
        ButtonAction? controllerAction = button switch
        {
            SButton.ControllerA => ButtonAction.Primary,
            SButton.ControllerX => ButtonAction.Secondary,
            SButton.ControllerB => ButtonAction.Cancel,
            _ => null,
        };
        if (controllerAction.HasValue)
        {
            return controllerAction.Value;
        }
        if (Game1.options.useToolButton.Any(b => b.ToSButton() == button))
        {
            return ButtonAction.Primary;
        }
        if (Game1.options.actionButton.Any(b => b.ToSButton() == button))
        {
            return ButtonAction.Secondary;
        }
        if (Game1.options.menuButton.Any(b => b.ToSButton() == button))
        {
            return ButtonAction.Cancel;
        }
        return ButtonAction.None;
    }

    /// <summary>
    /// Attempts to determine the actual physically pressed key for a "representative" or logical button reported by the
    /// underlying menu system that may not actually be down.
    /// </summary>
    /// <param name="logicalButton">The button that was reported.</param>
    /// <returns>A button that performs the same action as <paramref name="logicalButton"/> and is currently pressed; or
    /// the original <paramref name="logicalButton"/> if no match can be found.</returns>
    public static SButton GetPressedButton(SButton logicalButton)
    {
        if (UI.InputHelper.IsDown(logicalButton))
        {
            return logicalButton;
        }
        var action = GetButtonAction(logicalButton);
        if (action == ButtonAction.None)
        {
            return logicalButton;
        }
        return GetActionButtons(action).Where(UI.InputHelper.IsDown).Cast<SButton?>().FirstOrDefault() ?? logicalButton;
    }
}
