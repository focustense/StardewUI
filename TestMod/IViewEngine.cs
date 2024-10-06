using System.ComponentModel;
using StardewValley.Menus;

namespace StardewUI.Framework;

/// <summary>
/// Public API for StardewUI, abstracting away all implementation details of views and trees.
/// </summary>
public interface IViewEngine
{
    /// <summary>
    /// Creates a menu from the StarML stored in a game asset, as provided by a mod via SMAPI or Content Patcher.
    /// </summary>
    /// <remarks>
    /// Does not make the menu active. To show it, use <see cref="Game1.activeClickableMenu"/> or equivalent.
    /// </remarks>
    /// <param name="assetName">The name of the asset in the content pipeline, e.g. <c>Mods/MyMod/UI/MyMenu</c>.</param>
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
