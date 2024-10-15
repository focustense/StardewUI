namespace StardewUI.Framework;

/// <summary>
/// Tracks exponential backoff times for a set of keys.
/// </summary>
/// <remarks>
/// Keys are stored in a <see cref="Dictionary{TKey, TValue}"/>, so must have unique hash codes for correct operation.
/// </remarks>
/// <typeparam name="T">Type of key or other object to track.</typeparam>
/// <param name="rule">Configures the backoff duration and scaling.</param>
internal class BackoffTracker<T>(BackoffRule rule)
    where T : notnull
{
    private readonly Dictionary<T, BackoffState> entries = [];

    /// <summary>
    /// Advances the timer on any pending keys, allowing them to be used again on the next
    /// <see cref="TryRun{TResult}(T, Func{TResult}, out TResult)"/> if they have waited long enough.
    /// </summary>
    /// <param name="elapsed">Time elapsed since last tick.</param>
    public void Tick(TimeSpan elapsed)
    {
        foreach (var entry in entries.Values)
        {
            entry.Elapsed += elapsed;
        }
    }

    /// <summary>
    /// Attempts to run an arbitrary action, if it is currently allowed by backoff rules.
    /// </summary>
    /// <remarks>
    /// If the action is run, but still fails (throws), then the timer for the specified <paramref name="key"/> will be
    /// reset and the next duration will be increased.
    /// </remarks>
    /// <typeparam name="TResult">Result type of the <paramref name="action"/>.</typeparam>
    /// <param name="key">Unique key representing the specific action to run, e.g. asset to load.</param>
    /// <param name="action">The action to run, if currently allowed.</param>
    /// <param name="result">Holds the result of any successful <paramref name="action"/>, or <c>null</c> if the action
    /// was not run or failed to run.</param>
    /// <returns><c>true</c> if the <paramref name="action"/> was allowed (i.e. does not have a pending delay due to
    /// backoff rules) and was successful; <c>false</c> if the action is delayed or failed.</returns>
    public bool TryRun<TResult>(T key, Func<TResult> action, out TResult? result)
    {
        result = default;
        entries.TryGetValue(key, out var entry);
        if (entry is not null && entry.Elapsed < entry.Duration)
        {
            return false;
        }
        try
        {
            result = action();
            if (entry is not null)
            {
                entries.Remove(key);
            }
            return true;
        }
        catch
        {
            if (entry is not null)
            {
                entry.Duration = rule.GetNextDuration(entry.Duration);
                entry.Elapsed = TimeSpan.Zero;
            }
            else
            {
                entries.Add(key, new(rule.InitialDuration));
            }
            throw;
        }
    }
}
