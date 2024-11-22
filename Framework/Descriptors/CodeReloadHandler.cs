using System.Reflection.Metadata;

[assembly: MetadataUpdateHandler(typeof(StardewUI.Framework.Descriptors.CodeReloadHandler))]

namespace StardewUI.Framework.Descriptors;

/// <summary>
/// Handles hot reloads of the application itself, i.e. from an attached debugger.
/// </summary>
internal static class CodeReloadHandler
{
    private static readonly List<WeakReference<IDescriptorDependent>> dependents = [];

    /// <summary>
    /// Registers an object that is descriptor-dependent to receive a notification when the descriptors change.
    /// </summary>
    /// <remarks>
    /// Dependents are tracked using weak references, so there is no need to unregister.
    /// </remarks>
    /// <param name="dependent">The dependent object.</param>
    internal static void RegisterDependent(IDescriptorDependent dependent)
    {
        dependents.Add(new(dependent));
    }

    /// <summary>
    /// Invoked as soon as any types are invalidated by a hot reload.
    /// </summary>
    /// <param name="types">The changed types.</param>
    internal static void ClearCache(Type[]? types)
    {
        if (types is null)
        {
            return;
        }
        bool anyDescriptorsChanged = false;
        foreach (var type in types)
        {
            if (ReflectionObjectDescriptor.Invalidate(type) || ReflectionViewDescriptor.Invalidate(type))
            {
                anyDescriptorsChanged = true;
                Logger.Log($"Cleared descriptor cache for {type.FullName}", LogLevel.Debug);
            }
        }
        if (anyDescriptorsChanged)
        {
            Logger.Log(
                "Type descriptors have been updated. You may need to close and reopen any open menus in order for all "
                    + "changes to take effect.",
                LogLevel.Info
            );
        }
    }

    /// <summary>
    /// Invoked after a hot reload completes.
    /// </summary>
    /// <param name="types">The changed types.</param>
    internal static void UpdateApplication(Type[]? types)
    {
        foreach (var dependentRef in dependents)
        {
            if (dependentRef.TryGetTarget(out var dependent))
            {
                dependent.InvalidateDescriptors();
            }
        }
        dependents.RemoveAll(dependentRef => dependentRef.TryGetTarget(out var _));
    }
}
