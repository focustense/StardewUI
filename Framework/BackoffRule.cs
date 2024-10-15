namespace StardewUI.Framework;

/// <summary>
/// Defines the rules for exponential backoff.
/// </summary>
/// <param name="initialDuration">Duration to wait before the first retry attempt.</param>
/// <param name="maxDuration">Maximum duration to wait before any retry attempt; i.e. no matter how many retries have
/// already occurred for a given key, it will not extend the duration any longer than this.</param>
/// <param name="multiplier">Amount to multiply the current duration on each subsequent retry, starting from
/// <paramref name="initialDuration"/> and going no higher than <paramref name="maxDuration"/>.</param>
public class BackoffRule(TimeSpan initialDuration, TimeSpan maxDuration, float multiplier)
{
    /// <summary>
    /// Standard backoff rule deemed suitable for most types of UI retries.
    /// </summary>
    /// <remarks>
    /// Uses an initial delay of 50 ms, maximum delay of 5 s, and multiplier of 4.
    /// </remarks>
    public static readonly BackoffRule Default = new(TimeSpan.FromMilliseconds(50), TimeSpan.FromSeconds(5), 4f);

    /// <summary>
    /// Gets the duration to wait before the first retry attempt.
    /// </summary>
    public TimeSpan InitialDuration { get; } = initialDuration;

    /// <summary>
    /// Gets the duration to wait before the next retry attempt.
    /// </summary>
    /// <param name="previousDuration">The wait duration that was used on the previous attempt.</param>
    public TimeSpan GetNextDuration(TimeSpan previousDuration)
    {
        if (previousDuration >= maxDuration)
        {
            return maxDuration;
        }
        var nextDuration = previousDuration * multiplier;
        return nextDuration < maxDuration ? nextDuration : maxDuration;
    }
}
