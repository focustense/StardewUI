using StardewModdingAPI.Utilities;

namespace StardewUI.Widgets.Keybinding;

/// <summary>
/// Extensions for SMAPI's keybind types.
/// </summary>
public static class KeybindExtensions
{
    /// <summary>
    /// Checks if two keybind instances are equal.
    /// </summary>
    /// <remarks>
    /// The comparison is order-sensitive; two keybinds with the same buttons in a different order are considered to be
    /// unequal to each other.
    /// </remarks>
    /// <param name="keybind">The first keybind to compare.</param>
    /// <param name="other">The second keybind to compare.</param>
    /// <returns><c>true</c> if <paramref name="keybind"/> has the same <see cref="Keybind.Buttons"/> as
    /// <paramref name="other"/> and in the same order, otherwise <c>false</c>.</returns>
    public static bool KeybindEquals(this Keybind keybind, Keybind other)
    {
        return keybind.Buttons.SequenceEqual(other.Buttons);
    }

    /// <summary>
    /// Checks if two keybind lists are equal.
    /// </summary>
    /// <remarks>
    /// The comparison is order-sensitive; two keybind lists with the same keybinds in a different order, or with any
    /// two keybinds having keys in a different order
    /// </remarks>
    /// <param name="keybindList">The first keybind list to compare.</param>
    /// <param name="other">The second keybind list to compare.</param>
    /// <returns><c>true</c> if both <paramref name="keybindList"/> and <paramref name="other"/> contain the exact same
    /// keybinds in the same order, otherwise <c>false</c>.</returns>
    public static bool KeybindEquals(this KeybindList keybindList, KeybindList other)
    {
        if (keybindList.Keybinds.Length != other.Keybinds.Length)
        {
            return false;
        }
        return !keybindList.Keybinds.Where((t, i) => !t.KeybindEquals(other.Keybinds[i])).Any();
    }
}
