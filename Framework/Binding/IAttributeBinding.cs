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
    /// Updates a target view with the most recent source value.
    /// </summary>
    /// <param name="target">The view to receive the update.</param>
    /// <param name="force">If <c>true</c>, always re-publishes the latest value to the view even if the source value
    /// has not changed. Typically used for initial updates immediately after creation.</param>
    /// <returns><c>true</c> if the view was updated; <c>false</c> if the update was skipped because the source value
    /// had not changed.</returns>
    bool Update(IView target, bool force = false);
}
