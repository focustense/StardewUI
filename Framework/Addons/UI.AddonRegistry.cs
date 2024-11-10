using StardewUI.Framework.Addons;

namespace StardewUI;

public static partial class UI
{
    /// <summary>
    /// The add-ons registered so far.
    /// </summary>
    internal static readonly Dictionary<string, IAddon> addons = [];

    /// <summary>
    /// Registers a UI add-on (mod extension).
    /// </summary>
    /// <remarks>
    /// <para>
    /// Add-ons are resolved in the game's <see cref="StardewModdingAPI.Events.IGameLoopEvents.GameLaunched"/> event.
    /// Therefore, mods providing add-ons must register them as early as possible, typically in their
    /// <see cref="Mod.Entry(IModHelper)"/> method, but if that is too early, then in a <c>GameLaunched</c> handler of
    /// their own with a high <see cref="StardewModdingAPI.Events.EventPriority"/>.
    /// </para>
    /// <para>
    /// Types provided by add-ons (views, converters, etc.) will not actually be used until a UI is created, so add-ons
    /// may employ lazy/deferred loading if they need to postpone some critical operations until after the game is fully
    /// loaded, other APIs are initialized, etc.
    /// </para>
    /// </remarks>
    /// <param name="addon">The add-on definition.</param>
    public static void RegisterAddon(IAddon addon)
    {
        if (addons.TryGetValue(addon.Id, out var previousAddon))
        {
            Logger.Log(
                $"Not registering add-on '{addon.Id}' from assembly '{addon.GetType().Assembly.FullName}' "
                    + $"because an add-on with the same ID was already registered by assembly "
                    + $"'{previousAddon.GetType().Assembly.FullName}'."
            );
            return;
        }
        addons.Add(addon.Id, addon);
    }

    /// <summary>
    /// For all add-ons registered so far, determines a correct load order such that all dependencies are met.
    /// </summary>
    /// <remarks>
    /// Any add-ons whose dependencies cannot be satisfied will be excluded from the load order, and errors will be
    /// logged for any dependencies that failed to load.
    /// </remarks>
    /// <returns>A safe load order for registered add-ons.</returns>
    internal static IReadOnlyList<IAddon> ResolveLoadOrder()
    {
        var loadOrder = new List<IAddon>();
        var resolvingPath = new Stack<IAddon>();
        var resolvedIds = new HashSet<string>();
        foreach (var addon in addons.Values)
        {
            ResolveLoadOrder(addon, resolvingPath, loadOrder.Add, resolvedIds);
        }
        return loadOrder;
    }

    private static bool ResolveLoadOrder(
        IAddon addon,
        Stack<IAddon> resolvingPath,
        Action<IAddon> load,
        HashSet<string> resolvedIds
    )
    {
        if (resolvedIds.Contains(addon.Id))
        {
            return true;
        }
        if (resolvingPath.Contains(addon))
        {
            var cycleText = string.Join(" -> ", resolvingPath.Append(addon).Select(a => a.Id));
            Logger.Log(
                $"Unable to load UI add-on '{addon.Id}' because of a cycle in its dependencies: {cycleText}",
                LogLevel.Error
            );
            return false;
        }
        resolvingPath.Push(addon);
        try
        {
            foreach (var dependencyId in addon.Dependencies)
            {
                if (!addons.TryGetValue(dependencyId, out var dependency))
                {
                    Logger.Log(
                        $"Unable to load UI add-on '{addon.Id}' because one of its dependencies, '{dependencyId}', "
                            + "is not installed.",
                        LogLevel.Error
                    );
                    return false;
                }
                if (!ResolveLoadOrder(dependency, resolvingPath, load, resolvedIds))
                {
                    return false;
                }
            }
            load(addon);
            resolvedIds.Add(addon.Id);
            return true;
        }
        finally
        {
            resolvingPath.Pop();
        }
    }
}
