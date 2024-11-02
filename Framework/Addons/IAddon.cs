namespace StardewUI.Framework.Addons;

/// <summary>
/// Entry point for a UI add-on.
/// </summary>
/// <remarks>
/// Add-ons are a plugin-like system that allow mods to extend the UI capabilities through new views, tags, converters,
/// and other features. All add-ons must be registered via <see cref="UI.RegisterAddon(IAddon)"/>.
/// </remarks>
public interface IAddon
{
    /// <summary>
    /// Unique ID for this addon.
    /// </summary>
    /// <remarks>
    /// Prevents two copies of the same addon from trying to run at the same time, and allows other addons to depend on
    /// the features of this one by adding it to their <see cref="Dependencies"/>.
    /// </remarks>
    string Id { get; }

    /// <summary>
    /// List of dependencies, each corresponding to another <see cref="IAddon.Id"/>, required by this addon.
    /// </summary>
    /// <remarks>
    /// Dependencies will always be loaded first. If any dependencies are missing, or if a cycle is detected (e.g. addon
    /// A depends on B which depends on A again) then the addon will be prevented from loading.
    /// </remarks>
    IReadOnlyList<string> Dependencies => [];
}
