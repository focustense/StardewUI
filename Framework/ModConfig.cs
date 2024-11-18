using StardewUI.Framework.Diagnostics;

namespace StardewUI.Framework;

/// <summary>
/// Configuration settings for StardewUI.Framework.
/// </summary>
public class ModConfig
{
    /// <summary>
    /// Settings related to performance optimization.
    /// </summary>
    public PerformanceConfig Performance { get; set; } = new();

    /// <summary>
    /// Settings related to performance tracing.
    /// </summary>
    public TraceConfig Tracing { get; set; } = new();
}

/// <summary>
/// Configuration sub-settings providing control over performance tweaks.
/// </summary>
public class PerformanceConfig
{
    /// <summary>
    /// Whether to process member descriptors of a view or model in parallel.
    /// </summary>
    /// <remarks>
    /// Parallel processing will often make first-time loads slower rather than faster, due to scheduling overhead and
    /// some synchronization. This may be beneficial if types with hundreds of fields/properties are involved.
    /// </remarks>
    public bool EnableParallelDescriptors { get; set; }

    /// <summary>
    /// Whether to warm up StardewUI's reflection cache on a background thread during game start.
    /// </summary>
    /// <remarks>
    /// This will usually improve first-time loads of menus by 10-20%, and tends to have no cost/imperceptible cost on
    /// app startup, since it uses only one background thread (no parallelism) and other startup tasks tend to run on
    /// main thread only.
    /// </remarks>
    public bool EnableReflectionWarmup { get; set; } = true;
}
