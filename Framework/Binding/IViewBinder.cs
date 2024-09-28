using StardewUI.Framework.Descriptors;
using StardewUI.Framework.Dom;

namespace StardewUI.Framework.Binding;

/// <summary>
/// Service for creating view bindings and their dependencies.
/// </summary>
public interface IViewBinder
{
    /// <summary>
    /// Creates a view binding.
    /// </summary>
    /// <param name="view">The view that will be bound.</param>
    /// <param name="element">The element data providing the literal or binding attributes.</param>
    /// <param name="data">The binding data, for any non-asset bindings using
    /// <see cref="Grammar.AttributeValueType.InputBinding"/>.</param>
    /// <returns>A view binding that can be used to propagate changes in the <paramref name="data"/> or any dependent
    /// assets to the <paramref name="view"/>.</returns>
    IViewBinding Bind(IView view, IElement element, object? data);

    /// <summary>
    /// Retrieves the descriptor for a view, which provides information about its properties.
    /// </summary>
    /// <remarks>
    /// Descriptors participate in view binding but may also be used for other purposes, such as updating child lists.
    /// </remarks>
    /// <param name="view">The view instance.</param>
    /// <returns>The descriptor for the <paramref name="view"/>.</returns>
    IViewDescriptor GetDescriptor(IView view);
}
