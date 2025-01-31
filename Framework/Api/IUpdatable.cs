namespace StardewUI.Framework.Api;

/// <summary>
/// Provides a method to perform per-frame updates.
/// </summary>
/// <remarks>
/// Used on types such as the <see cref="ViewDrawable"/> that require regular updates but are not implicitly update by
/// any part of the vanilla game logic such as <see cref="ViewMenu"/>. To receive updates, the instance must be
/// tracked in a <see cref="ViewEngine"/>.
/// </remarks>
internal interface IUpdatable
{
    /// <summary>
    /// Whether this instance has been disposed and should no longer receive updates.
    /// </summary>
    /// <remarks>
    /// This is handled by the <see cref="ViewEngine"/> and used to remove stale instances.
    /// </remarks>
    bool IsDisposed { get; }

    /// <summary>
    /// Performs the scheduled update.
    /// </summary>
    /// <param name="elapsed">Time elapsed since last game tick.</param>
    void Update(TimeSpan elapsed);
}
