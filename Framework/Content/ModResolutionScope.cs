using HarmonyLib;

namespace StardewUI.Framework.Content;

/// <summary>
/// Resolution scope for a single mod; expects unqualified keys only.
/// </summary>
/// <param name="translations">Helper for the mod's translations.</param>
internal class ModResolutionScope(ITranslationHelper translations) : IResolutionScope
{
    private static readonly Dictionary<string, ModResolutionScope> scopesByModId = [];

    /// <summary>
    /// Creates or retrieves the scope for a specific mod, given its ID.
    /// </summary>
    /// <param name="uniqueId">The <see cref="IManifest.UniqueID"/> of the mod.</param>
    /// <param name="registry">Registry of all installed mods, i.e. as accessed through
    /// <see cref="IModHelper.ModRegistry"/>.</param>
    /// <returns>A resolution scope for the mod whose ID is the <paramref name="uniqueId"/>.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no installed mod can be found with the specified
    /// <paramref name="uniqueId"/>.</exception>
    public static ModResolutionScope ForModId(string uniqueId, IModRegistry registry)
    {
        if (!scopesByModId.TryGetValue(uniqueId, out var scope))
        {
            scope = CreateScope(uniqueId, registry);
            scopesByModId.Add(uniqueId, scope);
        }
        return scope;
    }

    /// <inheritdoc />
    public Translation GetTranslation(string key)
    {
        return translations.Get(key);
    }

    private static ModResolutionScope CreateScope(string uniqueId, IModRegistry registry)
    {
        var modInfo =
            registry.Get(uniqueId)
            ?? throw new KeyNotFoundException($"Required mod '{uniqueId}' is not loaded or not installed.");
        var traverse = new Traverse(modInfo);
        var translations =
            traverse.Property<IMod?>("Mod").Value?.Helper.Translation
            ?? traverse.Property<IContentPack?>("ContentPack").Value?.Translation
            ?? throw new InvalidOperationException(
                $"Could not obtain translation helper for mod '{uniqueId}' as it appears to be neither a SMAPI mod "
                    + "nor a content pack."
            );
        return new(translations);
    }
}
