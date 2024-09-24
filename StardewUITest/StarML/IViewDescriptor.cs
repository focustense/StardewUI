using System.Diagnostics.CodeAnalysis;

namespace StardewUITest.StarML;

/// <summary>
/// Describes a type of view that can be used in a view binding.
/// </summary>
/// <remarks>
/// The binding target is independent of the actual <see cref="StardewUI.IView"/> instance; it provides methods and data
/// to support interacting with any view of the given <see cref="TargetType"/>.
/// </remarks>
public interface IViewDescriptor : IObjectDescriptor
{
    /// <summary>
    /// Retrieves the property of the <see cref="TargetType"/> that holds the view's children/content.
    /// </summary>
    /// <returns>The view children property.</returns>
    /// <exception cref="BindingException">Thrown when the <see cref="TargetType"/> lacks any visible property that
    /// could be used to hold child views.</exception>
    IBindingProperty GetChildrenProperty()
    {
        return TryGetChildrenProperty(out var property)
            ? property
            : throw new BindingException($"Type {TargetType.Name} does not have any property that supports child views.");
    }

    /// <summary>
    /// Attempts to retrieve the property of the <see cref="TargetType"/> that holds the view's children/content.
    /// </summary>
    /// <param name="property">When this method returns, holds a reference to the <see cref="IBindingProperty"/> that
    /// holds the view's children/content, or <c>null</c> if no such property is available.</param>
    /// <returns><c>true</c> if a children/content property was found, otherwise <c>false</c>.</returns>
    bool TryGetChildrenProperty([MaybeNullWhen(false)] out IBindingProperty property);
}
