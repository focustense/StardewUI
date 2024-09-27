namespace StardewUI;

/// <summary>
/// Internal animator abstraction used for driving animations; does not need to know the target or value types, only to
/// have the ability to accept time ticks.
/// </summary>
internal interface IAnimator
{
    bool IsValid();
    void Tick(TimeSpan elapsed);
}
