namespace StardewUI.Animation;

/// <summary>
/// Internal animator abstraction used for driving animations; does not need to know the target or value types, only to
/// have the ability to accept time ticks.
/// </summary>
internal interface IAnimator
{
    /// <summary>
    /// Checks if the animator can still animate, e.g. if it still has a valid target.
    /// </summary>
    bool IsValid();

    /// <summary>
    /// Advances the animation.
    /// </summary>
    /// <param name="elapsed">The time elapsed since the previous tick.</param>
    void Tick(TimeSpan elapsed);
}
