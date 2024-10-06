namespace StardewUI.Framework.Binding;

/// <summary>
/// Binding instance for a single event on a single view.
/// </summary>
/// <remarks>
/// Removes/unsubscribes the event handler when disposed.
/// </remarks>
public interface IEventBinding : IDisposable { }
