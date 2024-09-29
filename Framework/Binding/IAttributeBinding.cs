namespace StardewUI.Framework.Binding;

/// <summary>
/// Binding instance for a single attribute on a single view.
/// </summary>
/// <remarks>
/// <para>
/// Encapsulates the source of the value and provides a method to update the target view if the value has changed.
/// </para>
/// <para>
/// This is primarily for internal use, as a way of tracking fine-grained changes to views instead of having to rebind
/// the entire view when anything changes.
/// </para>
/// </remarks>
public interface IAttributeBinding : IDisposable
{
    /// <summary>
    /// The name of the bound property on the destination view.
    /// </summary>
    string DestinationPropertyName { get; }

    /// <summary>
    /// The data flow direction for this binding.
    /// </summary>
    BindingDirection Direction { get; }

    /// <summary>
    /// Updates the source to match the view's current value.
    /// </summary>
    /// <remarks>
    /// Allowed when the <see cref="Direction"/> is either <see cref="BindingDirection.Out"/> or
    /// <see cref="BindingDirection.InOut"/>.
    /// </remarks>
    /// <param name="target">The bound view.</param>
    void UpdateSource(IView target);

    /// <summary>
    /// Updates a target view with the most recent source value.
    /// </summary>
    /// <remarks>
    /// Allowed when the <see cref="Direction"/> is either <see cref="BindingDirection.In"/> or
    /// <see cref="BindingDirection.InOut"/>.
    /// </remarks>
    /// <param name="target">The view to receive the update.</param>
    /// <param name="force">If <c>true</c>, always re-publishes the latest value to the view even if the source value
    /// has not changed. Typically used for initial updates immediately after creation.</param>
    /// <returns><c>true</c> if the view was updated; <c>false</c> if the update was skipped because the source value
    /// had not changed.</returns>
    bool UpdateView(IView target, bool force = false);
}
