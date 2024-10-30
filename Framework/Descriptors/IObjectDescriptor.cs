using System.Diagnostics.CodeAnalysis;

namespace StardewUI.Framework.Descriptors;

/// <summary>
/// Describes a type of object that participates in view binding, either as the target or the source.
/// </summary>
/// <remarks>
/// The binding target is independent of the actual object instance; it provides methods and data to support interacting
/// with any object of the given <see cref="TargetType"/>.
/// </remarks>
public interface IObjectDescriptor
{
    /// <summary>
    /// Whether or not objects of this type can notify about data changes; that is, if the type implements
    /// <see cref="System.ComponentModel.INotifyPropertyChanged"/>.
    /// </summary>
    bool SupportsChangeNotifications { get; }

    /// <summary>
    /// The type being described, which owns or inherits each of the available members.
    /// </summary>
    Type TargetType { get; }

    /// <summary>
    /// Retrieves a named event on the <see cref="TargetType"/>.
    /// </summary>
    /// <param name="name">The event name.</param>
    /// <returns>The <see cref="IEventDescriptor"/> whose <see cref="IMemberDescriptor.Name"/> is
    /// <paramref name="name"/>.</returns>
    /// <exception cref="DescriptorException">Thrown when no event exists with the specified
    /// <paramref name="name"/>.</exception>
    IEventDescriptor GetEvent(string name)
    {
        return TryGetEvent(name, out var @event)
            ? @event
            : throw new DescriptorException($"Type {TargetType.Name} does not have an event named '{name}'.");
    }

    /// <summary>
    /// Retrieves a named method of the <see cref="TargetType"/>.
    /// </summary>
    /// <remarks>
    /// Overloaded methods are not supported. If different signatures are required, use optional parameters.
    /// </remarks>
    /// <param name="name">The method name.</param>
    /// <returns>The <see cref="IMethodDescriptor"/> whose <see cref="IMemberDescriptor.Name"/> is
    /// <paramref name="name"/>.</returns>
    /// <exception cref="DescriptorException">Thrown when no method exists with the specified
    /// <paramref name="name"/>.</exception>
    IMethodDescriptor GetMethod(string name)
    {
        return TryGetMethod(name, out var method)
            ? method
            : throw new DescriptorException($"Type {TargetType.Name} does not have a method named '{name}'.");
    }

    /// <summary>
    /// Retrieves a named property of the <see cref="TargetType"/>.
    /// </summary>
    /// <param name="name">The property name.</param>
    /// <returns>The <see cref="IPropertyDescriptor"/> whose <see cref="IMemberDescriptor.Name"/> is
    /// <paramref name="name"/>.</returns>
    /// <exception cref="DescriptorException">Thrown when no property exists with the specified
    /// <paramref name="name"/>.</exception>
    IPropertyDescriptor GetProperty(string name)
    {
        return TryGetProperty(name, out var property)
            ? property
            : throw new DescriptorException($"Type {TargetType.Name} does not have a property named '{name}'.");
    }

    /// <summary>
    /// Attempts to retrieve a named event on the <see cref="TargetType"/>.
    /// </summary>
    /// <param name="name">The event name.</param>
    /// <param name="event">When this method returns, holds a reference to the <see cref="IEventDescriptor"/> whose
    /// <see cref="IMemberDescriptor.Name"/> is <paramref name="name"/>, or <c>null</c> if no event was found with the
    /// given name.</param>
    /// <returns><c>true</c> if the named event was found, otherwise <c>false</c>.</returns>
    bool TryGetEvent(string name, [MaybeNullWhen(false)] out IEventDescriptor @event);

    /// <summary>
    /// Attempts to retrieve a named method of the <see cref="TargetType"/>.
    /// </summary>
    /// <remarks>
    /// Overloaded methods are not supported. If different signatures are required, use optional parameters.
    /// </remarks>
    /// <param name="name">The method name.</param>
    /// <param name="method">When this method returns, holds a reference to the <see cref="IMethodDescriptor"/> whose
    /// <see cref="IMemberDescriptor.Name"/> is <paramref name="name"/>, or <c>null</c> if no method was found with the
    /// given name.</param>
    /// <returns><c>true</c> if the named method was found, otherwise <c>false</c>.</returns>
    bool TryGetMethod(string name, [MaybeNullWhen(false)] out IMethodDescriptor method);

    /// <summary>
    /// Attempts to retrieve a named property of the <see cref="TargetType"/>.
    /// </summary>
    /// <param name="name">The property name.</param>
    /// <param name="property">When this method returns, holds a reference to the <see cref="IPropertyDescriptor"/>
    /// whose <see cref="IMemberDescriptor.Name"/> is <paramref name="name"/>, or <c>null</c> if no property was found
    /// with the given name.</param>
    /// <returns><c>true</c> if the named property was found, otherwise <c>false</c>.</returns>
    bool TryGetProperty(string name, [MaybeNullWhen(false)] out IPropertyDescriptor property);
}
