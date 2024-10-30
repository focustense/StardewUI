using StardewUI.Framework.Diagnostics;

namespace StardewUI.Framework;

/// <summary>
/// Configuration settings for StardewUI.Framework.
/// </summary>
public class ModConfig
{
    /// <summary>
    /// Settings related to performance tracing.
    /// </summary>
    public TraceConfig Tracing { get; set; } = new();
}
