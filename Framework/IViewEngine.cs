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
    /// Creates a menu from the StarML stored in a game asset, as provided by a mod via SMAPI or Content Patcher, and
    /// returns a controller for customizing the menu's behavior.
    /// </summary>
    /// <remarks>
    /// The menu that is created is the same as the result of <see cref="CreateMenuFromMarkup(string, object?)"/>. The
    /// menu is not automatically shown; to show it, use <see cref="Game1.activeClickableMenu"/> or equivalent.
    /// </remarks>
    /// <param name="assetName">The name of the StarML view asset in the content pipeline, e.g.
    /// <c>Mods/MyMod/Views/MyView</c>.</param>
    /// <param name="context">The context, or "model", for the menu's view, which holds any data-dependent values.
    /// <b>Note:</b> The type must implement <see cref="INotifyPropertyChanged"/> in order for any changes to this data
    /// to be automatically reflected in the UI.</param>
    /// <returns>A controller object whose <see cref="IMenuController.Menu"/> is the created menu and whose other
    /// properties can be used to change menu-level behavior.</returns>
    IMenuController CreateMenuControllerFromAsset(string assetName, object? context = null);

    /// <summary>
    /// Creates a menu from arbitrary markup, and returns a controller for customizing the menu's behavior.
    /// </summary>
    /// <remarks>
    /// <b>Warning:</b> Ad-hoc menus created this way cannot be cached, nor patched by other mods. Most mods should not
    /// use this API except for testing/experimentation.
    /// </remarks>
    /// <param name="markup">The markup in StarML format.</param>
    /// <param name="context">The context, or "model", for the menu's view, which holds any data-dependent values.
    /// <b>Note:</b> The type must implement <see cref="INotifyPropertyChanged"/> in order for any changes to this data
    /// to be automatically reflected in the UI.</param>
    /// <returns>A controller object whose <see cref="IMenuController.Menu"/> is the created menu and whose other
    /// properties can be used to change menu-level behavior.</returns>
    IMenuController CreateMenuControllerFromMarkup(string markup, object? context = null);

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
    /// <para>
    /// If the <paramref name="sourceDirectory"/> argument is specified, and points to a directory with the same asset
    /// structure as the mod, then an additional sync will be set up such that files modified in the
    /// <c>sourceDirectory</c> while the game is running will be copied to the active mod directory and subsequently
    /// reloaded. In other words, pointing this at the mod's <c>.csproj</c> directory allows hot reloading from the
    /// source files instead of the deployed mod's files.
    /// </para>
    /// <para>
    /// Hot reload may impact game performance and should normally only be used during development and/or in debug mode.
    /// </para>
    /// </remarks>
    /// <param name="sourceDirectory">Optional source directory to watch and sync changes from. If not specified, or not
    /// a valid source directory, then hot reload will only pick up changes from within the live mod directory.</param>
    void EnableHotReloading(string? sourceDirectory = null);

    /// <summary>
    /// Begins preloading assets found in this mod's registered asset directories.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Preloading is performed in the background, and can typically help reduce first-time latency for showing menus or
    /// drawables, without any noticeable lag in game startup.
    /// </para>
    /// <para>
    /// Must be called after asset registration (<see cref="RegisterViews"/>, <see cref="RegisterSprites"/> and so on)
    /// in order to be effective, and must not be called more than once per mod otherwise errors or crashes may occur.
    /// </para>
    /// </remarks>
    void PreloadAssets();

    /// <summary>
    /// Declares that the specified context types will be used as either direct arguments or subproperties in one or
    /// more subsequent <c>CreateMenu</c> or <c>CreateDrawable</c> APIs, and instructs the framework to begin inspecting
    /// those types and optimizing for later use.
    /// </summary>
    /// <remarks>
    /// Data binding to mod-defined types uses reflection, which can become expensive when loading a very complex menu
    /// and/or binding to a very complex model for the first time. Preloading can perform this work in the background
    /// instead of causing latency when opening the menu.
    /// </remarks>
    /// <param name="types">The types that the mod expects to use as context.</param>
    void PreloadModels(params Type[] types);

    /// <summary>
    /// Registers a mod directory to be searched for special-purpose mod data, i.e. that is not either views or sprites.
    /// </summary>
    /// <remarks>
    /// Allowed extensions for files in this folder and their corresponding data types are:
    /// <list type="bullet">
    /// <item><c>.buttonspritemap.json</c> - <see href="https://focustense.github.io/StardewUI/reference/stardewui/data/buttonspritemapdata/">ButtonSpriteMapData</see></item>
    /// </list>
    /// </remarks>
    /// <param name="assetPrefix">The prefix for all asset names, <b>excluding</b> the category which is deduced from
    /// the file extension as described in the remarks. For example, given a value of <c>Mods/MyMod</c>, a file named
    /// <c>foo.buttonspritemap.json</c> would be referenced in views as <c>@Mods/MyMod/ButtonSpriteMaps/Foo</c>.</param>
    /// <param name="modDirectory">The physical directory where the asset files are located, relative to the mod
    /// directory. Typically a path such as <c>assets/ui</c> or <c>assets/ui/data</c>.</param>
    void RegisterCustomData(string assetPrefix, string modDirectory);

    /// <summary>
    /// Registers a mod directory to be searched for sprite (and corresponding texture/sprite sheet data) assets.
    /// </summary>
    /// <param name="assetPrefix">The prefix for all asset names, e.g. <c>Mods/MyMod/Sprites</c>. This can be any value
    /// but the same prefix must be used in <c>@AssetName</c> view bindings.</param>
    /// <param name="modDirectory">The physical directory where the asset files are located, relative to the mod
    /// directory. Typically a path such as <c>assets/sprites</c> or <c>assets/ui/sprites</c>.</param>
    void RegisterSprites(string assetPrefix, string modDirectory);

    /// <summary>
    /// Registers a mod directory to be searched for view (StarML) assets. Uses the <c>.sml</c> extension.
    /// </summary>
    /// <param name="assetPrefix">The prefix for all asset names, e.g. <c>Mods/MyMod/Views</c>. This can be any value
    /// but the same prefix must be used in <c>include</c> elements and in API calls to create views.</param>
    /// <param name="modDirectory">The physical directory where the asset files are located, relative to the mod
    /// directory. Typically a path such as <c>assets/views</c> or <c>assets/ui/views</c>.</param>
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

/// <summary>
/// Wrapper for a mod-managed <see cref="IClickableMenu"/> that allows further customization of menu-level properties
/// not accessible to StarML or data binding.
/// </summary>
public interface IMenuController : IDisposable
{
    /// <summary>
    /// Event raised after the menu has been closed.
    /// </summary>
    event Action Closed;

    /// <summary>
    /// Event raised when the menu is about to close.
    /// </summary>
    /// <remarks>
    /// This has the same lifecycle as <see cref="IClickableMenu.cleanupBeforeExit"/>.
    /// </remarks>
    event Action Closing;

    /// <summary>
    /// Gets or sets a function that returns whether or not the menu can be closed.
    /// </summary>
    /// <remarks>
    /// This is equivalent to implementing <see cref="IClickableMenu.readyToClose"/>.
    /// </remarks>
    Func<bool>? CanClose { get; set; }

    /// <summary>
    /// Gets or sets an action that <b>replaces</b> the default menu-close behavior.
    /// </summary>
    /// <remarks>
    /// Most users should leave this property unset. It is intended for use in unusual contexts, such as replacing the
    /// mod settings in a Generic Mod Config Menu integration. Setting any non-null value to this property will suppress
    /// the default behavior of <see cref="IClickableMenu.exitThisMenu(bool)"/> entirely, so the caller is responsible
    /// for handling all possible scenarios (e.g. child of another menu, or sub-menu of the title menu).
    /// </remarks>
    Action? CloseAction { get; set; }

    /// <summary>
    /// Offset from the menu view's top-right edge to draw the close button.
    /// </summary>
    /// <remarks>
    /// Only applies when <see cref="EnableCloseButton"/> has been called at least once.
    /// </remarks>
    Vector2 CloseButtonOffset { get; set; }

    /// <summary>
    /// Whether to automatically close the menu when a mouse click is detected outside the bounds of the menu and any
    /// floating elements.
    /// </summary>
    /// <remarks>
    /// This setting is primarily intended for submenus and makes them behave more like overlays.
    /// </remarks>
    bool CloseOnOutsideClick { get; set; }

    /// <summary>
    /// Sound to play when closing the menu.
    /// </summary>
    string CloseSound { get; set; }

    /// <summary>
    /// How much the menu should dim the entire screen underneath.
    /// </summary>
    /// <remarks>
    /// The default dimming is appropriate for most menus, but if the menu is being drawn as a delegate of some other
    /// macro-menu, then it can be lowered or removed (set to <c>0</c>) entirely.
    /// </remarks>
    float DimmingAmount { get; set; }

    /// <summary>
    /// Gets the menu, which can be opened using <see cref="Game1.activeClickableMenu"/>, or as a child menu.
    /// </summary>
    IClickableMenu Menu { get; }

    /// <summary>
    /// Gets or sets a function that returns the top-left position of the menu.
    /// </summary>
    /// <remarks>
    /// Setting any non-null value will disable the auto-centering functionality, and is equivalent to setting the
    /// <see cref="IClickableMenu.xPositionOnScreen"/> and <see cref="IClickableMenu.yPositionOnScreen"/> fields.
    /// </remarks>
    Func<Point>? PositionSelector { get; set; }

    /// <summary>
    /// Removes any cursor attachment previously set by <see cref="SetCursorAttachment"/>.
    /// </summary>
    void ClearCursorAttachment();

    /// <summary>
    /// Closes the menu.
    /// </summary>
    /// <remarks>
    /// This method allows programmatic closing of the menu. It performs the same action that would be performed by
    /// pressing one of the configured menu keys (e.g. ESC), clicking the close button, etc., and follows the same
    /// rules, i.e. will not allow closing if <see cref="CanClose"/> is <c>false</c>.
    /// </remarks>
    void Close();

    /// <summary>
    /// Configures the menu to display a close button on the upper-right side.
    /// </summary>
    /// <remarks>
    /// If no <paramref name="texture"/> is specified, then all other parameters are ignored and the default close
    /// button sprite is drawn. Otherwise, a custom sprite will be drawn using the specified parameters.
    /// </remarks>
    /// <param name="texture">The source image/tile sheet containing the button image.</param>
    /// <param name="sourceRect">The location within the <paramref name="texture"/> where the image is located, or
    /// <c>null</c> to draw the entire <paramref name="texture"/>.</param>
    /// <param name="scale">Scale to apply, if the destination size should be different from the size of the
    /// <paramref name="sourceRect"/>.</param>
    void EnableCloseButton(Texture2D? texture = null, Rectangle? sourceRect = null, float scale = 4f);

    /// <summary>
    /// Begins displaying a cursor attachment, i.e. a sprite that follows the mouse cursor.
    /// </summary>
    /// <remarks>
    /// The cursor is shown in addition to, not instead of, the normal mouse cursor.
    /// </remarks>
    /// <param name="texture">The source image/tile sheet containing the cursor image.</param>
    /// <param name="sourceRect">The location within the <paramref name="texture"/> where the image is located, or
    /// <c>null</c> to draw the entire <paramref name="texture"/>.</param>
    /// <param name="size">Destination size for the cursor sprite, if different from the size of the
    /// <paramref name="sourceRect"/>.</param>
    /// <param name="offset">Offset between the actual mouse position and the top-left corner of the drawn
    /// cursor sprite.</param>
    /// <param name="tint">Optional tint color to apply to the drawn cursor sprite.</param>
    void SetCursorAttachment(
        Texture2D texture,
        Rectangle? sourceRect = null,
        Point? size = null,
        Point? offset = null,
        Color? tint = null
    );

    /// <summary>
    /// Configures the menu's gutter widths/heights.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Gutters are areas of the screen that the menu should not occupy. These are typically used with a menu whose root
    /// view uses <see cref="Layout.Length.Stretch"/> for one of its <see cref="IView.Layout"/> dimensions, and allows
    /// limiting the max width/height relative to the viewport size.
    /// </para>
    /// <para>
    /// The historical reason for gutters is <see href="https://en.wikipedia.org/wiki/Overscan">overscan</see>, however
    /// they are still commonly used for aesthetic reasons.
    /// </para>
    /// </remarks>
    /// <param name="left">The gutter width on the left side of the viewport.</param>
    /// <param name="top">The gutter height at the top of the viewport.</param>
    /// <param name="right">The gutter width on the right side of the viewport. The default value of <c>-1</c> specifies
    /// that the <paramref name="left"/> value should be mirrored on the right.</param>
    /// <param name="bottom">The gutter height at the bottom of the viewport. The default value of <c>-1</c> specifies
    /// that the <paramref name="top"/> value should be mirrored on the bottom.</param>
    void SetGutters(int left, int top, int right = -1, int bottom = -1);
}

/// <summary>
/// Extensions for the <see cref="IViewEngine"/> interface.
/// </summary>
internal static class ViewEngineExtensions
{
    /// <summary>
    /// Starts monitoring this mod's directory for changes to assets managed by any of the <see cref="IViewEngine"/>'s
    /// <c>Register</c> methods, e.g. views and sprites, and attempts to set up an additional sync from the mod's
    /// project (source) directory to the deployed mod directory so that hot reloads can be initiated from the IDE.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Callers should normally omit the <paramref name="callerFilePath"/> parameter in their call; this will cause it
    /// to be replaced at compile time with the actual file path of the caller, and used to automatically detect the
    /// project path.
    /// </para>
    /// <para>
    /// If detection/sync fails due to an unusual project structure, consider providing an exact path directly to
    /// <see cref="IViewEngine.EnableHotReloading(string)"/> instead of using this extension.
    /// </para>
    /// <para>
    /// Hot reload may impact game performance and should normally only be used during development and/or in debug mode.
    /// </para>
    /// </remarks>
    /// <param name="viewEngine">The view engine API.</param>
    /// <param name="callerFilePath">Do not pass in this argument, so that <see cref="CallerFilePathAttribute"/> can
    /// provide the correct value on build.</param>
    public static void EnableHotReloadingWithSourceSync(
        this IViewEngine viewEngine,
        [CallerFilePath] string? callerFilePath = null
    )
    {
        viewEngine.EnableHotReloading(FindProjectDirectory(callerFilePath));
    }

    // Attempts to determine the project root directory given the path to an arbitrary source file by walking up the
    // directory tree until it finds a directory containing a file with .csproj extension.
    private static string? FindProjectDirectory(string? sourceFilePath)
    {
        if (string.IsNullOrEmpty(sourceFilePath))
        {
            return null;
        }
        for (var dir = Directory.GetParent(sourceFilePath); dir is not null; dir = dir.Parent)
        {
            if (dir.EnumerateFiles("*.csproj").Any())
            {
                return dir.FullName;
            }
        }
        return null;
    }
}
