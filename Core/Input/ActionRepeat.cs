namespace StardewUI.Input;

/// <summary>
/// Configures the repeat rate of an action used in an <see cref="ActionState{T}"/>.
/// </summary>
/// <param name="RepeatInterval">The interval between repetitions of the action, while the key is held.</param>
/// <param name="InitialDelay">Initial delay after the first press, before any repetitions are allowed.</param>
[DuckType]
public record ActionRepeat(TimeSpan RepeatInterval, TimeSpan? InitialDelay = null)
{
    /// <summary>
    /// Configures an action to repeat continuously, i.e. to run again on every frame as long as the trigger keys are
    /// still held.
    /// </summary>
    public static readonly ActionRepeat Continuous = new(TimeSpan.Zero, null);

    /// <summary>
    /// Default repetition setting suitable for most UI scenarios.
    /// </summary>
    /// <remarks>
    /// Not perfectly consistent (nor intended to be consistent) with vanilla game settings, which are all over the
    /// place depending on which key/button is being considered.
    /// </remarks>
    public static readonly ActionRepeat Default = new(TimeSpan.FromMilliseconds(50), TimeSpan.FromMilliseconds(500));

    /// <summary>
    /// Configures an action to never repeat, no matter how long the trigger keys are held.
    /// </summary>
    public static readonly ActionRepeat None = new(TimeSpan.MaxValue, null);
}
