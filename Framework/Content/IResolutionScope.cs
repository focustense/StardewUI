namespace StardewUI.Framework.Content;

/// <summary>
/// Defines a scope in which certain types of external and potentially ambiguous binding attributes may be resolved.
/// </summary>
/// <remarks>
/// In general, resolution scopes are used where a document may include an unqualified or short-form reference, for
/// which such a reference means to look in the same mod that originally provided that document.
/// </remarks>
public interface IResolutionScope
{
    /// <summary>
    /// Attempts to obtain a translation value with the given key.
    /// </summary>
    /// <param name="key">The qualified or unqualified translation key. Unqualified keys are identical to their name in
    /// the translation file (i.e. in <c>i18n/default.json</c>), while qualified keys include a prefix with the specific
    /// mod, e.g. <c>authorname.ModName:TranslationKey</c>.</param>
    /// <returns>
    /// One of:
    /// <list type="bullet">
    /// <item>The translation, if available in the current language</item>
    /// <item>The default-language (usually English) string, if the <paramref name="key"/> exists but no translation is
    /// available;</item>
    /// <item>A translation with placeholder text, if the <paramref name="key"/> resolves to a known mod (or if the
    /// scope points to a default mod) but the mod is missing that specific key;</item>
    /// <item><c>null</c>, if the <paramref name="key"/> is unqualified and no default scope is available.</item>
    /// </list>
    /// </returns>
    Translation? GetTranslation(string key);

    /// <summary>
    /// Attempts to obtain the string value of a translation with the given key.
    /// </summary>
    /// <remarks>
    /// Unless specifically overridden, calling this is normally the same as <see cref="GetTranslation(string)"/>,
    /// except that it returns <see cref="string"/> instead of <see cref="Translation"/>, of which the latter is not
    /// instantiable outside of SMAPI's internals. To maintain consistency, code that <em>reads</em> translations should
    /// always use <c>GetTranslationValue</c>, while code that <em>provides</em> translations (i.e. implementations of
    /// <see cref="IResolutionScope"/>) should only implement <c>GetTranslation</c>, and leave this method alone.
    /// </remarks>
    /// <param name="key">The qualified or unqualified translation key. Unqualified keys are identical to their name in
    /// the translation file (i.e. in <c>i18n/default.json</c>), while qualified keys include a prefix with the specific
    /// mod, e.g. <c>authorname.ModName:TranslationKey</c>.</param>
    /// <returns>
    /// One of:
    /// <list type="bullet">
    /// <item>The translation, if available in the current language</item>
    /// <item>The default-language (usually English) string, if the <paramref name="key"/> exists but no translation is
    /// available;</item>
    /// <item>A translation with placeholder text, if the <paramref name="key"/> cannot be resolved to a known
    /// translation in a known mod.</item>
    /// </list>
    /// </returns>
    string GetTranslationValue(string key)
    {
        return GetTranslation(key)?.ToString() ?? $"(no translation:{key})";
    }
}
