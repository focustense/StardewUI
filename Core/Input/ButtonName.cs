using StardewModdingAPI;

namespace StardewUI.Input;

/// <summary>
/// A simple utility classes to get a printable name for a button or key.
/// </summary>
/// <remarks>
/// Mostly used for keyboard prompts and fallback prompts for gamepad buttons without a dedicated sprite.
/// </remarks>
internal static class ButtonName
{
    /// <summary>
    /// Gets the (non-localized) display name for a button.
    /// </summary>
    /// <param name="button">The button to label.</param>
    /// <returns>The name of the <paramref name="button"/> if known; otherwise, the enum name.</returns>
    public static string ForButton(SButton button)
    {
        return button switch
        {
            SButton.ControllerA => "A",
            SButton.ControllerB => "B",
            SButton.ControllerBack => "[]",
            SButton.ControllerStart => "=",
            SButton.ControllerX => "X",
            SButton.ControllerY => "Y",
            SButton.D0 => "0",
            SButton.D1 => "1",
            SButton.D2 => "2",
            SButton.D3 => "3",
            SButton.D4 => "4",
            SButton.D5 => "5",
            SButton.D6 => "6",
            SButton.D7 => "7",
            SButton.D8 => "8",
            SButton.D9 => "9",
            SButton.DPadUp => "^",
            SButton.DPadDown => "v",
            SButton.DPadLeft => "<",
            SButton.DPadRight => ">",
            SButton.LeftShoulder => "LB",
            SButton.LeftTrigger => "LT",
            SButton.NumPad0 => "0",
            SButton.NumPad1 => "1",
            SButton.NumPad2 => "2",
            SButton.NumPad3 => "3",
            SButton.NumPad4 => "4",
            SButton.NumPad5 => "5",
            SButton.NumPad6 => "6",
            SButton.NumPad7 => "7",
            SButton.NumPad8 => "8",
            SButton.NumPad9 => "9",
            SButton.RightShoulder => "RB",
            SButton.RightTrigger => "RT",
            _ => button.ToString(),
        };
    }
}
