namespace StardewUI.Framework.Descriptors;

/// <summary>
/// Factory for obtaining descriptors, encapsulating both dynamic (reflection) and static (precompiled) descriptors.
/// </summary>
public static partial class DescriptorFactory
{
    /// <summary>
    /// Gets a descriptor for an arbitrary object type; typically used for binding targets.
    /// </summary>
    /// <param name="type">The object type.</param>
    /// <param name="lazy">Whether to create the descriptor with lazily-initialized members. Lazy descriptors are faster
    /// to create, but may have slower initial access times.</param>
    public static IObjectDescriptor GetObjectDescriptor(Type type, bool lazy = false)
    {
        return PrecompiledDescriptors.GetValueOrDefault(type) ?? ReflectionObjectDescriptor.ForType(type, lazy);
    }

    /// <summary>
    /// Gets a descriptor for a type that is assumed to be an <see cref="IView"/> implementation.
    /// </summary>
    /// <remarks>
    /// View descriptors include additional information about view-specific types, such as outlets.
    /// </remarks>
    /// <param name="type">The object type.</param>
    public static IViewDescriptor GetViewDescriptor(Type type)
    {
        return PrecompiledDescriptors.GetValueOrDefault(type) is IViewDescriptor viewDescriptor
            ? viewDescriptor
            : ReflectionViewDescriptor.ForViewType(type);
    }
}
