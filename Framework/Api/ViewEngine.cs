using StardewModdingAPI.Events;
using StardewUI.Framework.Binding;
using StardewUI.Framework.Content;
using StardewUI.Framework.Dom;
using StardewUI.Framework.Sources;
using StardewValley.Menus;

namespace StardewUI.Framework.Api;

/// <summary>
/// Implementation for the public <see cref="IViewEngine"/> API.
/// </summary>
public class ViewEngine : IViewEngine
{
    /// <summary>
    /// Source resolver for resolving documents created by this view engine back to their original mod.
    /// </summary>
    public ISourceResolver SourceResolver => assetRegistry;

    private readonly IAssetCache assetCache;
    private readonly AssetRegistry assetRegistry;
    private readonly List<WeakReference<IUpdatable>> updatables = [];
    private readonly IViewNodeFactory viewNodeFactory;

    /// <summary>
    /// Initializes a new <see cref="ViewEngine"/> instance.
    /// </summary>
    /// <param name="assetCache">Cache for obtaining document assets. Used for asset-based views.</param>
    /// <param name="helper">SMAPI mod helper for the API consumer mod (not for StardewUI).</param>
    /// <param name="viewNodeFactory">Factory for creating and binding <see cref="IViewNode"/>s.</param>
    public ViewEngine(IAssetCache assetCache, IModHelper helper, IViewNodeFactory viewNodeFactory)
    {
        this.assetCache = assetCache;
        this.viewNodeFactory = viewNodeFactory;
        assetRegistry = new(helper);

        helper.Events.GameLoop.UpdateTicked += GameLoop_UpdateTicked;
    }

    /// <inheritdoc />
    public IViewDrawable CreateDrawableFromAsset(string assetName)
    {
        var documentSource = new AssetValueSource<Document>(assetCache, assetName);
        var view = new DocumentView(viewNodeFactory, documentSource);
        var drawable = new ViewDrawable(view);
        updatables.Add(new(drawable));
        return drawable;
    }

    /// <inheritdoc />
    public IViewDrawable CreateDrawableFromMarkup(string markup)
    {
        var document = Document.Parse(markup);
        var documentSource = new ConstantValueSource<Document>(document);
        var view = new DocumentView(viewNodeFactory, documentSource);
        var drawable = new ViewDrawable(view);
        updatables.Add(new(drawable));
        return drawable;
    }

    /// <inheritdoc />
    public IMenuController CreateMenuControllerFromAsset(string assetName, object? context = null)
    {
        var documentSource = new AssetValueSource<Document>(assetCache, assetName);
        return new DocumentViewMenu(viewNodeFactory, documentSource, context);
    }

    /// <inheritdoc />
    public IMenuController CreateMenuControllerFromMarkup(string markup, object? context = null)
    {
        var document = Document.Parse(markup);
        var documentSource = new ConstantValueSource<Document>(document);
        return new DocumentViewMenu(viewNodeFactory, documentSource, context);
    }

    /// <inheritdoc />
    public IClickableMenu CreateMenuFromAsset(string assetName, object? context = null)
    {
        return CreateMenuControllerFromAsset(assetName, context).Menu;
    }

    /// <inheritdoc />
    public IClickableMenu CreateMenuFromMarkup(string markup, object? context = null)
    {
        return CreateMenuControllerFromMarkup(markup, context).Menu;
    }

    /// <inheritdoc />
    public void EnableHotReloading(string? sourceDirectory = null)
    {
        assetRegistry.EnableHotReloading(sourceDirectory);
    }

    /// <inheritdoc />
    public void PreloadAssets()
    {
        Task.Run(assetRegistry.PreloadAsync)
            .ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    Logger.Log($"An unexpected error occurred during asset preloading: {t.Exception}", LogLevel.Error);
                }
            });
    }

    /// <inheritdoc />
    public void RegisterCustomData(string assetPrefix, string modDirectory)
    {
        assetRegistry.RegisterCustomData(assetPrefix, modDirectory);
    }

    /// <inheritdoc />
    public void RegisterSprites(string assetPrefix, string modDirectory)
    {
        assetRegistry.RegisterSprites(assetPrefix, modDirectory);
    }

    /// <inheritdoc />
    public void RegisterViews(string assetPrefix, string modDirectory)
    {
        assetRegistry.RegisterViews(assetPrefix, modDirectory);
    }

    private void GameLoop_UpdateTicked(object? sender, UpdateTickedEventArgs e)
    {
        updatables.RemoveAll(updatableRef =>
        {
            if (updatableRef.TryGetTarget(out var updatable) && !updatable.IsDisposed)
            {
                updatable.Update(Game1.currentGameTime.ElapsedGameTime);
                return false;
            }
            return true;
        });
    }
}
