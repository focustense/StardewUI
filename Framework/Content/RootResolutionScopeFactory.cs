using StardewUI.Framework.Dom;

namespace StardewUI.Framework.Content;

/// <summary>
/// Factory for creating <see cref="IResolutionScope"/> instances, game-wide.
/// </summary>
/// <remarks>
/// This type is meant to be driven from StardewUI's entry point, and given access to all source resolvers (i.e. all
/// view engines) in order to reliably determine the default scope for any given <see cref="Document"/>.
/// </remarks>
/// <param name="registry">Mod registry for locating mods corresponding to qualified IDs.</param>
internal class RootResolutionScopeFactory(IModRegistry registry) : IResolutionScopeFactory
{
    private readonly IResolutionScope fallbackScope = new GlobalResolutionScope(null, registry);
    private readonly List<ISourceResolver> sourceResolvers = [];

    /// <summary>
    /// Registers a source resolver.
    /// </summary>
    /// <remarks>
    /// This should be called with the new resolver whenever a new mod connects to the Framework API and creates a view
    /// engine.
    /// </remarks>
    /// <param name="resolver">The new resolver.</param>
    public void AddSourceResolver(ISourceResolver resolver)
    {
        sourceResolvers.Add(resolver);
    }

    /// <inheritdoc />
    public IResolutionScope CreateForDocument(Document document)
    {
        foreach (var resolver in sourceResolvers)
        {
            if (resolver.TryGetProvidingModId(document, out var modId))
            {
                return GlobalResolutionScope.ForDefaultMod(modId, registry);
            }
        }
        return fallbackScope;
    }
}
