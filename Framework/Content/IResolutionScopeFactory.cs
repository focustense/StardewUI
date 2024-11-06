using StardewUI.Framework.Dom;

namespace StardewUI.Framework.Content;

/// <summary>
/// Factory for creating <see cref="IResolutionScope"/> instances.
/// </summary>
public interface IResolutionScopeFactory
{
    /// <summary>
    /// Obtains a resolution scope for a loaded document.
    /// </summary>
    /// <param name="document">The UI document in which tokens may be resolved.</param>
    /// <returns>An <see cref="IResolutionScope"/> for the specified <paramref name="document"/>, based on the
    /// best-known information about the document's source.</returns>
    IResolutionScope CreateForDocument(Document document);
}
