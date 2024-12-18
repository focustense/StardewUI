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
    /// Gets the current value bound for this attribute, regardless of the view's actual value.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method performs conversion (if necessary) from the source type to the destination type, but does not look
    /// at the destination view itself. It can be used to determine what the view's value "should be", which is part of
    /// the behavior system.
    /// </para>
    /// <para>
    /// Alternatively, this can be thought of as the value that the view would have after calling
    /// <see cref="UpdateView"/> with the <c>force</c> argument set to <c>true</c>.
    /// </para>
    /// </remarks>
    /// <returns>The currently bound value, or <c>null</c> if the value does not exist or cannot be determined, for
    /// example in the case of an <see cref="Grammar.AttributeValueType.OutputBinding"/>.</returns>
    object? GetBoundValue();

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
