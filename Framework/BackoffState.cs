namespace StardewUI.Framework;

/// <summary>
/// State of an exponential backoff, e.g. as used in a <see cref="BackoffTracker{T}"/>.
/// </summary>
/// <param name="initialDuration">The initial duration to wait for a retry.</param>
public class BackoffState(TimeSpan initialDuration)
{
    /// <summary>
    /// The most recent duration waited/waiting for a retry.
    /// </summary>
    public TimeSpan Duration { get; set; } = initialDuration;

    /// <summary>
    /// Time elapsed waiting for the current/next retry.
    /// </summary>
    public TimeSpan Elapsed { get; set; }
}
