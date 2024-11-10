using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using StardewUI.Framework.Dom;

namespace StardewUI.Framework.Content;

/// <summary>
/// Provides a method to connect a parsed <see cref="Document"/> back to the mod that provided it.
/// </summary>
public interface ISourceResolver
{
    /// <summary>
    /// Attempts to determine which mod is the originator of some markup document.
    /// </summary>
    /// <param name="document">The markup document.</param>
    /// <param name="modId">Holds the <see cref="IManifest.UniqueID"/> of the originating mod, if the method returns
    /// <c>true</c>, otherwise <c>null</c>.</param>
    /// <returns><c>true</c> if the <paramref name="document"/> source was successfully resolved, otherwise
    /// <c>false</c>.</returns>
    bool TryGetProvidingModId(Document document, [MaybeNullWhen(false)] out string modId);
}

/// <summary>
/// Maps <see cref="Document"/> instances to the sources they were loaded from.
/// </summary>
internal class SourceResolver
{
    private static readonly ConditionalWeakTable<Document, string> loadedDocumentSources = [];

    /// <summary>
    /// Gets the absolute file path that was used to load a given document.
    /// </summary>
    /// <param name="document">The loaded document.</param>
    /// <returns>The path to the <paramref name="document"/> source file on disk, or <c>null</c> if unknown.</returns>
    public static string? GetDocumentSourcePath(Document document)
    {
        return loadedDocumentSources.TryGetValue(document, out var source) ? source : null;
    }

    /// <summary>
    /// Updates the tracked file path for a loaded document.
    /// </summary>
    /// <remarks>
    /// Uses weak references, so this will not keep a <see cref="Document"/> alive if it is no longer in use.
    /// </remarks>
    /// <param name="document">The loaded document.</param>
    /// <param name="path">The path to the <paramref name="document"/> source file on disk.</param>
    public static void SetDocumentSourcePath(Document document, string path)
    {
        loadedDocumentSources.AddOrUpdate(document, path);
    }
}
