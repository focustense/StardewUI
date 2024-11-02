using StardewUI.Framework.Converters;

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

    /// <summary>
    /// Provides user-defined type conversions in addition to the standard conversions.
    /// </summary>
    /// <remarks>
    /// All user-defined converters have lower priority than the built-in converters, except for the duck-type
    /// converters and <see cref="StringConverterFactory"/> which are always considered last. Within the set of
    /// user-defined converters, the priority is based on inverted load order; the <em>last</em> add-on that registered
    /// a converter able to handle a particular type conversion will be the one chosen, as long as none of the standard
    /// conversions can apply.
    /// </remarks>
    IValueConverterFactory? ValueConverterFactory => null;
}
