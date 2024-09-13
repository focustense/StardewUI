namespace StardewUI.Widgets.Keybinding;

/// <summary>
/// Specifies the exact type of keybind supported by a widget using a
/// <see cref="StardewModdingAPI.Utilities.KeybindList"/>.
/// </summary>
public enum KeybindType
{
    /// <summary>
    /// The binding is a single <see cref="StardewModdingAPI.SButton"/> and does not support key combinations.
    /// </summary>
    SingleButton,

    /// <summary>
    /// The binding is a single <see cref="StardewModdingAPI.Utilities.Keybind"/>, supporting exactly one combination
    /// of keys that must all be pressed.
    /// </summary>
    SingleKeybind,

    /// <summary>
    /// The binding is a real <see cref="StardewModdingAPI.Utilities.KeybindList"/>, which can handle any number of
    /// individual <see cref="StardewModdingAPI.Utilities.Keybind"/>s each with their own key combination.
    /// </summary>
    MultipleKeybinds,
}
