namespace StardewUI.Framework.Content;

/// <summary>
/// Resolution scope that can accept qualified keys and examine the entire mod registry.
/// </summary>
/// <remarks>
/// Unqualified keys will default to a single inner scope.
/// </remarks>
/// <param name="defaultScope">The default or "calling" scope that should handle all unqualified requests. If not
/// specified, only fully qualified requests will be resolved.</param>
/// <param name="registry">Mod registry for locating mods corresponding to qualified IDs.</param>
internal class GlobalResolutionScope(IResolutionScope? defaultScope, IModRegistry registry) : IResolutionScope
{
    /// <summary>
    /// Creates a new global scope whose default resolution is for a specified mod.
    /// </summary>
    /// <param name="uniqueId">The <see cref="IManifest.UniqueID"/> of the mod used for default resolution.</param>
    /// <param name="registry">Registry of all installed mods, i.e. as accessed through
    /// <see cref="IModHelper.ModRegistry"/>.</param>
    /// <returns>A scope whose default resolution searches the mod with specified <paramref name="uniqueId"/>.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no installed mod can be found with the specified
    /// <paramref name="uniqueId"/>.</exception>
    public static GlobalResolutionScope ForDefaultMod(string uniqueId, IModRegistry registry)
    {
        var defaultScope = ModResolutionScope.ForModId(uniqueId, registry);
        return new(defaultScope, registry);
    }

    /// <inheritdoc />
    public Translation? GetTranslation(string key)
    {
        // Even if the key has a namespace separator (':'), that doesn't prove it's a qualified key, since SMAPI
        // translations don't restrict that character (or any character) from being part of the key itself.
        //
        // Therefore we always check for a translation in the default scope first, and only consider a possible
        // namespace if no local result is found.
        var defaultResult = defaultScope?.GetTranslation(key);
        if (defaultResult?.HasValue() == true)
        {
            return defaultResult;
        }

        var separatorIndex = key.IndexOf(':');
        if (separatorIndex < 0)
        {
            return defaultResult;
        }

        var modId = key[..separatorIndex];
        var localKey = key[(separatorIndex + 1)..];
        return ModResolutionScope.ForModId(modId, registry).GetTranslation(localKey);
    }
}
