using StardewModdingAPI.Utilities;

namespace StardewUI.Framework.Diagnostics;

/// <summary>
/// Configures the tracing behavior for StardewUI.
/// </summary>
public class TraceConfig
{
    /// <summary>
    /// Whether to show HUD notifications when tracing is started or stopped.
    /// </summary>
    /// <remarks>
    /// Notifications are always written to the SMAPI console log, but will not show in-game unless this setting is
    /// enabled.
    /// </remarks>
    public bool EnableHudNotifications { get; set; } = true;

    /// <summary>
    /// Directory where traces should be written.
    /// </summary>
    /// <remarks>
    /// Unless an absolute path is specified, the directory is relative to Stardew's data directory, i.e. the same
    /// directory where <c>Saves</c> and <c>ErrorLogs</c> are written.
    /// </remarks>
    public string OutputDirectory { get; set; } = "Traces";

    /// <summary>
    /// Hotkey(s) used to toggle tracing.
    /// </summary>
    public KeybindList ToggleHotkeys { get; set; } =
        new KeybindList(new Keybind(SButton.LeftControl, SButton.LeftShift, SButton.F6));
}
