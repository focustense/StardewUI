# Displaying UI

Before you can display any UI, you'll need to have:

1. [Initialized the Framework API](index.md#adding-the-api);
2. Authored one or more [StarML views](../framework/starml.md) and set them up to be [deployed](index.md#next-steps); and
3. [Registered the asset path](adding-ui-assets.md#adding-views) of your view(s).

Once all those steps are completed, it's time to bring that UI to life.

## Menus

To create a menu from a StardewUI view, use the `CreateMenu` methods:

=== "Menu From Asset"

    ```cs
    var context = new SomeViewModel(...);
    IClickableMenu menu = viewEngine.CreateMenuFromAsset(
        "Mods/authorName.ModName/Views/SomeView",
        context);
    ```

=== "Menu From Markup"

    ```cs
    var context = new SomeViewModel();
    string markup =
        @"<lane layout=""500px content"" orientation=""vertical"">
            <label font=""dialogue"" text=""Things"" />
            <label *repeat={Things} text={DisplayName} />
        </lane>";
    IClickableMenu menu = viewEngine.CreateMenuFromMarkup(markup, context);
    ```

Where `Mods/authorName.ModName/Views/SomeView` is the concatenation of the view's [asset prefix](adding-ui-assets.md#adding-views), e.g. `Mods/authorName.ModName/Views`, with the StarML file name/path, e.g. `SomeView` (excluding the `.sml` extension).

Once created, the menu is shown the same way as a vanilla menu, e.g. by assigning it to `Game1.activeClickableMenu`, or as a child menu of another menu, or to `TitleMenu.subMenu`, and so on.

!!! danger

    The `CreateMenuFromMarkup` method is only intended for testing, or for very unusual scenarios where the UI needs to be constructed completely on the fly and the combination of [data bindings](../framework/starml.md#attribute-flavors) and [structural attributes](../framework/starml.md#structural-attributes) such as `*switch` and `*if` aren't sufficient, which should be rare.
    
    Programmatically-constructed StarML must be parsed every time the view is created and won't benefit from hot reload or from many of StardewUI's performance optimizations.
    
    `CreateMenuFromAsset` is always the recommended entry point and should be used whenever possible.

Several complete [examples](https://github.com/focustense/StardewUI/blob/fba9ea25465af4caa9b341441b3a54cf3f8ba6d3/TestMod/ModEntry.cs#L58) are provided in the test mod:

```cs
var context = new
{
    HeaderText = "Example Menu Title",
    ItemData = ItemRegistry.GetData("(O)117"),
};
Game1.activeClickableMenu = viewEngine.CreateMenuFromAsset(
    "Mods/focustense.StardewUITest/Views/TestView",
    context);
```

## HUD

The game's HUD – Heads Up Display – refers to the persistent UI that is drawn over top of the game world, such as the date/time widget, health/energy bars, and so on.

### Using the API

The recommended way to display HUD-style UI is to create a Drawable from StarML. The API for this is very similar to the [menu](#menus) API:

=== "Drawable From Asset"

    ```cs
    IViewDrawable drawable =
        viewEngine.CreateDrawableFromAsset("Mods/authorName.ModName/Views/SomeView");
    drawable.Context = new SomeViewModel(...);
    // Optional, only needed if the view is not fixed-size
    drawable.MaxSize = new(maxWidth, maxHeight);
    ```

=== "Drawable From Markup"

    ```cs
    var context = new SomeViewModel();
    string markup =
        @"<lane layout=""500px content"" orientation=""vertical"">
            <label font=""dialogue"" text=""Things"" />
            <label *repeat={Things} text={DisplayName} />
        </lane>";
    IViewDrawable drawable = viewEngine.CreateDrawableFromMarkup(markup);
    drawable.Context = new SomeViewModel(...);
    // Optional, only needed if the view is not fixed-size
    drawable.MaxSize = new(maxWidth, maxHeight);
    ```

!!! danger

    Avoid using the `CreateDrawableFromMarkup` variant in release builds for the same reasons described in [menus](#menus).

`IViewDrawable` implements `IDisposable`. If you are done with an instance and never going to draw it again, make sure to call its `Dispose()` method. StardewUI will attempt to detect unreachable drawables and stop updating them, but this is less reliable and will take longer than a drawable that was disposed correctly, potentially degrading game performance over time.

The test mod also has a simple [HUD example](https://github.com/focustense/StardewUI/blob/440cb50d6bdd61228128585f29dee595527d369b/TestMod/ModEntry.cs#L117):

```cs
private void ToggleHud()
{
    if (hudWidget is not null)
    {
        hudWidget.Dispose();
        hudWidget = null;
    }
    else
    {
        hudWidget = viewEngine.CreateDrawableFromAsset(
            "Mods/focustense.StardewUITest/Views/Example-Hud");
        hudWidget.Context = new { Title = "I'm a HUD!" };
    }
}
```

### Using Views Directly

!!! warning "Advanced usage warning"

    This section is provided for disambiguation, since there are more differences between the Core Library and Framework usage for HUD-style UI than there are for menus. If you aren't already using the Core Library to handle specialized scenarios or [extend the framework](../framework/extensions.md), ignore this section and use the [drawable API](#using-the-api) instead.

When working with the [core library](../library/index.md), the `IViewDrawable` abstraction is not involved; instead, `IView` can be used directly with a `SpriteBatch`.

[A Fishing Sea](https://github.com/focustense/StardewFishingSea) uses this older method for its HUD:

!!! example

    ```cs
    internal sealed class ModEntry : Mod
    {
        private IView? seedFishPreview;
        
        public override void Entry(IModHelper helper)
        {
            seedFishPreview = new SeedFishInfoView();
            
            helper.Events.Display.RenderedHud += Display_RenderedHud;
        }
        
        private void Display_RenderedHud(object? sender, RenderedHudEventArgs e)
        {
            seedFishPreview.Measure(new(500, 500));
            var overlayBatch = new PropagatedSpriteBatch(
                spriteBatch,
                Transform.FromTranslation(new Vector2(0, 100))
            );
            SeedFishPreview.Draw(overlayBatch);
        }
    }
    ```

The above is substantially simplified from the actual mod code for brevity. There are only 3 steps to the process:

1. Create an `IView` or `IDrawable` instance and save it (do not recreate the view on every frame).
2. Each frame, call its `Measure` method, providing the maximum width/height it should be allowed to use; the actual width/height may be smaller.
3. After measuring—usually immediately afterward—Call the view's `Draw` method with the desired position.

!!! note "Important"

    Unless the view is completely static (that is, neither its content nor its position ever changes), you must call `Measure` on every frame. This method is responsible for layout, and changes to a view's properties, such as the text of a label, will not be reflected until the next layout. Do not be concerned about frame performance; StardewUI is very careful to [limit work to the parts that changed](../concepts.md#dirty-checking).

## Other Scenarios

These are considered advanced usage and not recommended for StardewUI beginners; to learn more, head to the respective page:

- [Overlays](../library/overlays.md)