using System.Collections.Immutable;
using StardewUI.Framework.Descriptors;

namespace StardewUI.Framework.Binding;

/// <summary>
/// Context, or scope, of a bound view, providing the backing data and tools for accessing its properties.
/// </summary>
/// <param name="Descriptor">Descriptor of the <see cref="Data"/> type, used to read current values.</param>
/// <param name="Data">The bound data.</param>
/// <param name="Parent">The parent context from which this context was derived, if any.</param>
public record BindingContext(IObjectDescriptor Descriptor, object Data, BindingContext? Parent)
{
    /// <summary>
    /// Creates a <see cref="BindingContext"/> from the specified data, automatically building a new descriptor if the
    /// data type has not been previously seen.
    /// </summary>
    /// <param name="data">The bound data.</param>
    /// <param name="parent">The parent context from which this context was derived, if any.</param>
    /// <returns>A new <see cref="BindingContext"/> whose <see cref="Data"/> is the specified <paramref name="data"/>
    /// and whose <see cref="Descriptor"/> is the descriptor of <paramref name="data"/>'s runtime type.</returns>
    public static BindingContext Create(object data, BindingContext? parent = null)
    {
        var descriptor = ReflectionObjectDescriptor.ForType(data.GetType());
        return new(descriptor, data, parent);
    }

    /// <summary>
    /// Traverses up the specified depth to find an ancestor of this context.
    /// </summary>
    /// <param name="depth">Number of parents to traverse.</param>
    /// <returns>The <see cref="BindingContext"/> that is <paramref name="depth"/> levels higher than the current
    /// context, or <c>null</c> if the context stack size is less than the specified depth.</returns>
    public BindingContext? GetAncestor(int depth)
    {
        // In the vast majority of cases, depth will be 0, so short-circuit these cases to save a bit of performance.
        if (depth == 0)
        {
            return this;
        }
        var context = this;
        for (int i = 0; i < depth; i++)
        {
            if (context.Parent is null)
            {
                break;
            }
            context = context.Parent;
        }
        return context;
    }
}
