using System.Runtime.CompilerServices;

namespace StardewUI.Framework.Binding;

/// <summary>
/// Tracks context instances that already had updates dispatched this frame, to prevent duplication.
/// </summary>
/// <remarks>
/// Used by the <see cref="ContextUpdatingNodeDecorator"/>.
/// </remarks>
public class ContextUpdateTracker
{
    /// <summary>
    /// Global instance for entire framework.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Entire view trees may share the same context data; this is entirely up to the client(s). Therefore, the same
    /// tracker instance must be shared by all nodes in order to prevent duplication.
    /// </para>
    /// <para>
    /// The instance is expected to be reset in the <c>ModEntry</c> or similar entry point.
    /// </para>
    /// </remarks>
    public static readonly ContextUpdateTracker Instance = new();

    private static readonly object marker = new();

    private readonly ConditionalWeakTable<object, object> tracked = [];

    private ContextUpdateTracker() { }

    /// <summary>
    /// Resets all state; to be called at the beginning of each frame.
    /// </summary>
    public void Reset()
    {
        tracked.Clear();
    }

    /// <summary>
    /// Tracks the update of a context instance so that <see cref="WasAlreadyUpdated(object)"/> returns <c>true</c>
    /// for the given <paramref name="contextData"/> until <see cref="Reset"/> is called.
    /// </summary>
    /// <param name="contextData">The context that was updated.</param>
    public void TrackUpdate(object contextData)
    {
        tracked.Add(contextData, marker);
    }

    /// <summary>
    /// Checks if a context instance already received an update tick dispatch for the current frame.
    /// </summary>
    /// <param name="contextData">The context that may have been previously updated.</param>
    /// <returns><c>true</c> if the <paramref name="contextData"/> already received an update tick, otherwise
    /// <c>false</c>.</returns>
    public bool WasAlreadyUpdated(object contextData)
    {
        return tracked.TryGetValue(contextData, out _);
    }
}
