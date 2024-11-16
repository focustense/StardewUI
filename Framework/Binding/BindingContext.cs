using System.Collections.Concurrent;
using StardewUI.Framework.Descriptors;
using StardewUI.Framework.Dom;

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
        var descriptor = DescriptorFactory.GetObjectDescriptor(data.GetType());
        return new(descriptor, data, parent);
    }

    // Used to cache redirects by type name.
    private static readonly ConcurrentDictionary<(Type, string), bool> typeNameMatches = [];

    /// <summary>
    /// Resolves a redirected context, using this context as the starting point.
    /// </summary>
    /// <param name="redirect">The redirect data.</param>
    /// <returns>The resolved <see cref="BindingContext"/>, or <c>null</c> if the <paramref name="redirect"/> does not
    /// resolve to a valid context.</returns>
    public BindingContext? Redirect(ContextRedirect? redirect)
    {
        return redirect switch
        {
            null => this,
            ContextRedirect.Distance(uint depth) => GetAncestor(depth),
            ContextRedirect.Type(string typeName) => GetAncestor(typeName),
            _ => null,
        };
    }

    private BindingContext? GetAncestor(uint depth)
    {
        var context = this;
        for (uint i = 0; i < depth; i++)
        {
            if (context.Parent is null)
            {
                break;
            }
            context = context.Parent;
        }
        return context;
    }

    private static bool DoesTypeNameMatch(Type type, string typeName)
    {
        var key = (type, typeName);
        return typeNameMatches.GetOrAdd(key, _ => HasMatchingBaseOrInterface(type, typeName));

        static bool HasMatchingBaseOrInterface(Type type, string typeName)
        {
            for (var baseType = type; baseType is not null; baseType = baseType.BaseType)
            {
                if (baseType.Name.Equals(typeName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            foreach (var interfaceType in type.GetInterfaces())
            {
                if (interfaceType.Name.Equals(typeName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
    }

    private BindingContext? GetAncestor(string typeName)
    {
        for (var context = this; context is not null; context = context.Parent)
        {
            if (DoesTypeNameMatch(context.Data.GetType(), typeName))
            {
                return context;
            }
        }
        return null;
    }
}
