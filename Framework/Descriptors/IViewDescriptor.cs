using System.Diagnostics.CodeAnalysis;

namespace StardewUI.Framework.Descriptors;

/// <summary>
/// Describes a type of view that can be used in a view binding.
/// </summary>
/// <remarks>
/// The binding target is independent of the actual <see cref="StardewUI.IView"/> instance; it provides methods and data
/// to support interacting with any view of the given <see cref="IObjectDescriptor.TargetType"/>.
/// </remarks>
public interface IViewDescriptor : IObjectDescriptor
{
    /// <summary>
    /// Retrieves the property of the <see cref="IObjectDescriptor.TargetType"/> that holds the view's children/content.
    /// </summary>
    /// <returns>The view children property.</returns>
    /// <exception cref="DescriptorException">Thrown when the <see cref="IObjectDescriptor.TargetType"/> lacks any
    /// visible property that could be used to hold child views.</exception>
    IPropertyDescriptor GetChildrenProperty()
    {
        return TryGetChildrenProperty(out var property)
            ? property
            : throw new DescriptorException(
                $"Type {TargetType.Name} does not have any property that supports child views."
            );
    }

    /// <summary>
    /// Attempts to retrieve the property of the <see cref="IObjectDescriptor.TargetType"/> that holds the view's
    /// children/content.
    /// </summary>
    /// <param name="property">When this method returns, holds a reference to the <see cref="IPropertyDescriptor"/> that
    /// holds the view's children/content, or <c>null</c> if no such property is available.</param>
    /// <returns><c>true</c> if a children/content property was found, otherwise <c>false</c>.</returns>
    bool TryGetChildrenProperty([MaybeNullWhen(false)] out IPropertyDescriptor property);
}
