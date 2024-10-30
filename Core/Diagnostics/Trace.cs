namespace StardewUI.Diagnostics;

/// <summary>
/// Provides methods to toggle tracing and write to the current trace.
/// </summary>
public static class Trace
{
    /// <summary>
    /// Gets or sets whether tracing is active.
    /// </summary>
    /// <remarks>
    /// While inactive, all calls to any of the <see cref="Begin"/> overloads are ignored and return <c>null</c>.
    /// If tracing is active, then setting this to <c>false</c> will cause the trace file to be written automatically.
    /// </remarks>
    public static bool IsTracing
    {
        get => Writer?.IsTracing ?? false;
        set
        {
            if (value)
            {
                if (Writer is null)
                {
                    throw new InvalidOperationException(
                        $"Cannot start tracing before the {nameof(Writer)} has been set."
                    );
                }
                Writer.BeginTrace();
            }
            else
            {
                Writer?.EndTrace();
            }
        }
    }

    /// <summary>
    /// The configured writer.
    /// </summary>
    internal static ITraceWriter? Writer { get; set; }

    /// <summary>
    /// Begins tracking a new operation (slice).
    /// </summary>
    /// <remarks>
    /// Slices must be disposed in the opposite order in which they are created, otherwise the final trace may be
    /// considered invalid.
    /// </remarks>
    /// <param name="name">The name that should appear in the trace log/visualization.</param>
    /// <returns>A disposable instance which, when disposed, stops tracking this operation and records the duration it
    /// took, for subsequent writing to the trace file.</returns>
    public static IDisposable? Begin(string name)
    {
        return Writer?.IsTracing == true ? Writer.BeginSlice(name) : null;
    }

    /// <summary>
    /// Begins tracking a new operation (slice).
    /// </summary>
    /// <remarks>
    /// Slices must be disposed in the opposite order in which they are created, otherwise the final trace may be
    /// considered invalid.
    /// </remarks>
    /// <param name="callerName">Reference to the name (e.g. type name) of the object performing the traced
    /// operation.</param>
    /// <param name="memberName">Name of the member (method or property) about to begin execution.</param>
    /// <returns>A disposable instance which, when disposed, stops tracking this operation and records the duration it
    /// took, for subsequent writing to the trace file.</returns>
    public static IDisposable? Begin(Func<string> callerName, string memberName)
    {
        if (Writer?.IsTracing != true)
        {
            return null;
        }
        var sliceName = $"{callerName()}.{memberName}";
        return Writer.BeginSlice(sliceName);
    }

    /// <summary>
    /// Begins tracking a new operation (slice).
    /// </summary>
    /// <remarks>
    /// Slices must be disposed in the opposite order in which they are created, otherwise the final trace may be
    /// considered invalid.
    /// </remarks>
    /// <param name="caller">Reference to the object performing the traced operation.</param>
    /// <param name="memberName">Name of the member (method or property) about to begin execution.</param>
    /// <returns>A disposable instance which, when disposed, stops tracking this operation and records the duration it
    /// took, for subsequent writing to the trace file.</returns>
    public static IDisposable? Begin(object caller, string memberName)
    {
        // The check is technically redundant, but can avoid unnecessary repeated allocations from the closure.
        if (Writer?.IsTracing != true)
        {
            return null;
        }
        return caller is string name ? Begin(() => name, memberName) : Begin(() => caller.GetType().Name, memberName);
    }
}
