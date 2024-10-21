namespace StardewUI.Diagnostics;

/// <summary>
/// Abstract output writer for performance traces.
/// </summary>
/// <remarks>
/// This is an internal helper meant for use by the <see cref="Trace"/> utility and should not be used directly by mods.
/// </remarks>
public interface ITraceWriter
{
    /// <summary>
    /// Whether or not a trace has been started, and not yet ended.
    /// </summary>
    bool IsTracing { get; }

    /// <inheritdoc cref="Trace.Begin(string)" />
    /// <exception cref="InvalidOperationException">Thrown if no trace is currently active.</exception>
    IDisposable BeginSlice(string name);

    /// <summary>
    /// Starts a new trace.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when a trace is already active.</exception>
    void BeginTrace();

    /// <summary>
    /// Ends the current trace and writes all recorded data to a new trace file in the output directory.
    /// </summary>
    /// <returns>The name of the trace file that was written.</returns>
    /// <exception cref="InvalidOperationException">Thrown if no trace is currently active.</exception>
    void EndTrace();
}
