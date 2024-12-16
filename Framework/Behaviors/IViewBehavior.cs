﻿namespace StardewUI.Framework.Behaviors;

/// <summary>
/// Provides methods for attaching arbitrary data-dependent behavior to a view.
/// </summary>
/// <remarks>
/// Add-ons should normally not use this interface directly, and instead derive from
/// <see cref="ViewBehavior{TView, TData}"/> for type safety.
/// </remarks>
public interface IViewBehavior : IDisposable
{
    /// <summary>
    /// The type of data that the behavior accepts in <see cref="SetData(object)"/>.
    /// </summary>
    Type DataType { get; }

    /// <summary>
    /// The type of view that the behavior accepts in <see cref="SetView(IView)"/>.
    /// </summary>
    Type ViewType { get; }

    /// <summary>
    /// Checks whether the behavior is allowed to <see cref="Update"/>.
    /// </summary>
    /// <remarks>
    /// Implementations can override this in order to selectively disable updates. Typically, updates will be disabled
    /// when the behavior cannot run due to not having an attached view or data.
    /// </remarks>
    /// <returns><c>true</c> to continue running <see cref="Update"/> ticks, <c>false</c> to skip updates.</returns>
    bool CanUpdate();

    /// <summary>
    /// Updates the behavior's current data.
    /// </summary>
    /// <param name="data">The new data.</param>
    void SetData(object? data);

    /// <summary>
    /// Updates the behavior's attached view.
    /// </summary>
    /// <param name="view">The new view.</param>
    void SetView(IView? view);

    /// <summary>
    /// Runs on every update tick.
    /// </summary>
    /// <param name="elapsed">Time elapsed since the last update.</param>
    void Update(TimeSpan elapsed);
}
