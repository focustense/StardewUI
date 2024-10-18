namespace StardewUI.Framework.Descriptors;

/// <summary>
/// Describes a single event on some type.
/// </summary>
public interface IEventDescriptor : IMemberDescriptor
{
    /// <summary>
    /// Descriptor for the type of event object (arguments), generally a subtype of <see cref="EventArgs"/>.
    /// </summary>
    IObjectDescriptor? ArgsTypeDescriptor { get; }

    /// <summary>
    /// Number of parameters that the <c>Invoke</c> method of the <see cref="DelegateType"/> accepts.
    /// </summary>
    int DelegateParameterCount { get; }

    /// <summary>
    /// The type (subtype of <see cref="Delegate"/>) that can be added/removed from the event handlers.
    /// </summary>
    Type DelegateType { get; }

    /// <summary>
    /// Adds an event handler.
    /// </summary>
    /// <param name="target">The instance of the <see cref="IMemberDescriptor.DeclaringType"/> on which to subscribe to
    /// events.</param>
    /// <param name="handler">The handler to run when the event is raised; must be assignable to the
    /// <see cref="DelegateType"/>.</param>
    void Add(object target, Delegate handler);

    /// <summary>
    /// Removes an event handler.
    /// </summary>
    /// <param name="target">The instance of the <see cref="IMemberDescriptor.DeclaringType"/> on which to unsubscribe
    /// from events.</param>
    /// <param name="handler">The handler that was previously registered, i.e. via <see cref="Add"/>.</param>
    void Remove(object target, Delegate handler);
}
