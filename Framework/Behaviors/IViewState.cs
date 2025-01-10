using StardewUI.Framework.Descriptors;

namespace StardewUI.Framework.Behaviors;

/// <summary>
/// Provides access to all state-based overrides associated with a view.
/// </summary>
public interface IViewState
{
    /// <summary>
    /// Event raised when a flag changes, i.e. as the outcome of <see cref="SetFlag"/>.
    /// </summary>
    event EventHandler<FlagEventArgs> FlagChanged;

    /// <summary>
    /// Retrieves the default value for a given property.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The default value is the value that will be used when there are no states for that property, i.e. when
    /// <see cref="GetProperty{T}"/> returns <c>null</c> for the specified <paramref name="propertyName"/> or when the
    /// property's states are empty.
    /// </para>
    /// <para>
    /// Defaults are real-time; if the property is linked via data binding, then the default value is the value that is
    /// currently bound.
    /// </para>
    /// </remarks>
    /// <typeparam name="T">The property value type.</typeparam>
    /// <param name="propertyName">The property name.</param>
    /// <returns>The default value for the specified <paramref name="propertyName"/>.</returns>
    public T GetDefaultValue<T>(string propertyName);

    /// <summary>
    /// Gets the current value of a flag, if one is set.
    /// </summary>
    /// <param name="name">The flag name.</param>
    /// <typeparam name="T">Type of the flag value.</typeparam>
    /// <returns>The flag value, or the default of <typeparamref name="T"/> if not set.</returns>
    T? GetFlag<T>(string name);

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
    /// Sets an arbitrary flag that other behaviors can read and/or be notified about.
    /// </summary>
    /// <param name="name">The flag name.</param>
    /// <param name="value">The flag value, or <c>null</c> to unset.</param>
    void SetFlag(string name, object? value);

    /// <summary>
    /// Writes the active overrides to the target view.
    /// </summary>
    /// <param name="view">The view that should receive the state/overrides.</param>
    void Write(IView view);
}
