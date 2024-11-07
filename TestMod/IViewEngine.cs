using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Menus;

namespace StardewUI.Framework;

/// <summary>
/// Public API for StardewUI, abstracting away all implementation details of views and trees.
/// </summary>
public interface IViewEngine
{
    /// <summary>
    /// Creates an <see cref="IViewDrawable"/> from the StarML stored in a game asset, as provided by a mod via SMAPI or
    /// Content Patcher.
    /// </summary>
    /// <remarks>
    /// The <see cref="IViewDrawable.Context"/> and <see cref="IViewDrawable.MaxSize"/> can be provided after creation.
    /// </remarks>
    /// <param name="assetName">The name of the StarML view asset in the content pipeline, e.g.
    /// <c>Mods/MyMod/Views/MyView</c>.</param>
    /// <returns>An <see cref="IViewDrawable"/> for drawing directly to the <see cref="SpriteBatch"/> of a rendering
    /// event or other draw handler.</returns>
    IViewDrawable CreateDrawableFromAsset(string assetName);

    /// <summary>
    /// Creates an <see cref="IViewDrawable"/> from arbitrary markup.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <see cref="IViewDrawable.Context"/> and <see cref="IViewDrawable.MaxSize"/> can be provided after creation.
    /// </para>
    /// <para>
    /// <b>Warning:</b> Ad-hoc menus created this way cannot be cached, nor patched by other mods. Most mods should not
    /// use this API except for testing/experimentation.
    /// </para>
    /// </remarks>
    /// <param name="markup">The markup in StarML format.</param>
    /// <returns>An <see cref="IViewDrawable"/> for drawing directly to the <see cref="SpriteBatch"/> of a rendering
    /// event or other draw handler.</returns>
    IViewDrawable CreateDrawableFromMarkup(string markup);

    /// <summary>
    /// Creates a menu from the StarML stored in a game asset, as provided by a mod via SMAPI or Content Patcher.
    /// </summary>
    /// <remarks>
    /// Does not make the menu active. To show it, use <see cref="Game1.activeClickableMenu"/> or equivalent.
    /// </remarks>
    /// <param name="assetName">The name of the StarML view asset in the content pipeline, e.g.
    /// <c>Mods/MyMod/Views/MyView</c>.</param>
    /// <param name="context">The context, or "model", for the menu's view, which holds any data-dependent values.
    /// <b>Note:</b> The type must implement <see cref="INotifyPropertyChanged"/> in order for any changes to this data
    /// to be automatically reflected in the UI.</param>
    /// <returns>A menu object which can be shown using the game's standard menu APIs such as
    /// <see cref="Game1.activeClickableMenu"/>.</returns>
    IClickableMenu CreateMenuFromAsset(string assetName, object? context = null);

    /// <summary>
    /// Creates a menu from arbitrary markup.
    /// </summary>
    /// <remarks>
    /// <b>Warning:</b> Ad-hoc menus created this way cannot be cached, nor patched by other mods. Most mods should not
    /// use this API except for testing/experimentation.
    /// </remarks>
    /// <param name="markup">The markup in StarML format.</param>
    /// <param name="context">The context, or "model", for the menu's view, which holds any data-dependent values.
    /// <b>Note:</b> The type must implement <see cref="INotifyPropertyChanged"/> in order for any changes to this data
    /// to be automatically reflected in the UI.</param>
    /// <returns>A menu object which can be shown using the game's standard menu APIs such as
    /// <see cref="Game1.activeClickableMenu"/>.</returns>
    IClickableMenu CreateMenuFromMarkup(string markup, object? context = null);

    /// <summary>
    /// Starts monitoring this mod's directory for changes to assets managed by any of the <c>Register</c> methods, e.g.
    /// views and sprites.
    /// </summary>
    /// <remarks>
    /// May impact game performance and should normally only be used during development and/or in debug mode.
    /// </remarks>
    /// <param name="callerFilePath">Caller file path, used to add a watcher to the source directory so that edits in the source directory can be copied to the mod directory for hot reload, set to null to disable.</param>
    void EnableHotReloading([CallerFilePath] string? callerFilePath = null);

    /// <summary>
    /// Registers a mod directory to be searched for sprite (and corresponding texture/sprite sheet data) assets.
    /// </summary>
    /// <param name="assetPrefix">The prefix for all asset names, e.g. <c>Mods/MyMod/Sprites</c>. This can be any value
    /// but the same prefix must be used in <c>@AssetName</c> view bindings.</param>
    /// <param name="modDirectory">The physical directory where the asset files are located, relative to the mod
    /// directory. Typically a path such as <c>assets/sprites</c>.</param>
    void RegisterSprites(string assetPrefix, string modDirectory);

    /// <summary>
    /// Registers a mod directory to be searched for view (StarML) assets. Uses the <c>.sml</c> extension.
    /// </summary>
    /// <param name="assetPrefix">The prefix for all asset names, e.g. <c>Mods/MyMod/Views</c>. This can be any value
    /// but the same prefix must be used in <c>include</c> elements and in API calls to create views.</param>
    /// <param name="modDirectory">The physical directory where the asset files are located, relative to the mod
    /// directory. Typically a path such as <c>assets/views</c>.</param>
    public void RegisterViews(string assetPrefix, string modDirectory);
}

/// <summary>
/// Provides methods to update and draw a simple, non-interactive UI component, such as a HUD widget.
/// </summary>
public interface IViewDrawable : IDisposable
{
    /// <summary>
    /// The current size required for the content.
    /// </summary>
    /// <remarks>
    /// Use for calculating the correct position for a <see cref="Draw(SpriteBatch, Vector2)"/>, especially for elements
    /// that should be aligned to the center or right edge of the viewport.
    /// </remarks>
    Vector2 ActualSize { get; }

    /// <summary>
    /// The context, or "model", for the menu's view, which holds any data-dependent values.
    /// </summary>
    /// <remarks>
    /// The type must implement <see cref="INotifyPropertyChanged"/> in order for any changes to this data to be
    /// automatically reflected on the next <see cref="Draw(SpriteBatch, Vector2)"/>.
    /// </remarks>
    object? Context { get; set; }

    /// <summary>
    /// The maximum size, in pixels, allowed for this content.
    /// </summary>
    /// <remarks>
    /// If no value is specified, then the content is allowed to use the entire <see cref="Game1.uiViewport"/>.
    /// </remarks>
    Vector2? MaxSize { get; set; }

    /// <summary>
    /// Draws the current contents.
    /// </summary>
    /// <param name="b">Target sprite batch.</param>
    /// <param name="position">Position on the screen or viewport to use as the top-left corner.</param>
    void Draw(SpriteBatch b, Vector2 position);
}
