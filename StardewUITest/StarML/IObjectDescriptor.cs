using System.Diagnostics.CodeAnalysis;

namespace StardewUITest.StarML;

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
    /// All bindable properties on the <see cref="TargetType"/>.
    /// </summary>
    IEnumerable<IBindingProperty> Properties { get; }

    /// <summary>
    /// The type targeted object type which owns or inherits each of the <see cref="Properties"/>.
    /// </summary>
    Type TargetType { get; }

    /// <summary>
    /// Retrieves the property of the <see cref="TargetType"/> with a given name.
    /// </summary>
    /// <param name="name">The property name.</param>
    /// <returns>The <see cref="IBindingProperty"/> whose <see cref="IBindingProperty.Name"/> is
    /// <paramref name="name"/>.</returns>
    /// <exception cref="BindingException">Thrown when no property exists with the specified
    /// <paramref name="name"/>.</exception>
    IBindingProperty GetProperty(string name)
    {
        return TryGetProperty(name, out var property)
            ? property
            : throw new BindingException($"Type {TargetType.Name} does not have a property named '{name}'.");
    }

    /// <summary>
    /// Attempts to retrieve the property of the <see cref="TargetType"/> with a given name.
    /// </summary>
    /// <param name="name">The property name.</param>
    /// <param name="property">When this method returns, holds a reference to the <see cref="IBindingProperty"/> whose
    /// <see cref="IBindingProperty.Name"/> is <paramref name="name"/>, or <c>null</c> if no property was found with the
    /// given name.</param>
    /// <returns><c>true</c> if the named property was found, otherwise <c>false</c>.</returns>
    bool TryGetProperty(string name, [MaybeNullWhen(false)] out IBindingProperty property);
}
