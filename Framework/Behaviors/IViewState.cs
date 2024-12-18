using StardewUI.Framework.Descriptors;

namespace StardewUI.Framework.Behaviors;

/// <summary>
/// Provides access to all state-based overrides associated with a view.
/// </summary>
public interface IViewState
{
    /// <summary>
    /// Gets the override states for the specified property, creating a new one if it does not already exist.
    /// </summary>
    /// <typeparam name="T">The property value type.</typeparam>
    /// <param name="propertyName">The property name.</param>
    /// <returns>The state overrides for the specified property on the current view.</returns>
    IPropertyStates<T> GetOrAddProperty<T>(string propertyName);

    /// <summary>
    /// Gets the override states for the specified property, if any exist.
    /// </summary>
    /// <param name="propertyName">The property name.</param>
    /// <returns>The state overrides for the specified property, or <c>null</c> if none have been added.</returns>
    IPropertyStates<T>? GetProperty<T>(string propertyName);

    /// <summary>
    /// Writes the active overrides to the target view.
    /// </summary>
    /// <param name="view">The view that should receive the state/overrides.</param>
    void Write(IView view);
}
