# Getting Started

If you've made a Stardew mod before, then setting up StardewUI should be a snap; it's the same as any other [SMAPI integration](https://stardewvalleywiki.com/Modding:Modder_Guide/APIs/Integrations).

## Installing the Framework

StardewUI is prerelease, so it isn't published on Nexus Mods or any other mod site yet.

1. Download the [latest release](https://github.com/focustense/StardewUI/releases). Make sure you download the `StardewUI x.y.z.zip` file, **not** the source code.
2. Open the .zip file and extract the `StardewUI` folder into your `Stardew Valley\Mods` folder.

The framework is now ready to use from your own mods.

## Adding the API

If this is your first time working with a framework mod, have a look at SMAPI's guide to [using an API](https://stardewvalleywiki.com/Modding:Modder_Guide/APIs/Integrations#Using_an_API). The abridged version is:

1. Download the entire [`IViewEngine.cs`](https://raw.githubusercontent.com/focustense/StardewUI/refs/heads/dev/Framework/IViewEngine.cs) to somewhere in your mod's source directory. (You can right-click, "Save link as..." from this page.)
2. From wherever you want to invoke the API—typically `ModEntry.cs`—add to the top of the file:
    ```cs
    using StardewUI.Framework;
    ```
3. Declare a field for the API:
    ```cs
    private IViewEngine viewEngine;
    ```
4. Obtain the API in a `GameLaunched` handler:
    ```cs
    viewEngine = Helper.ModRegistry.GetApi<IViewEngine>("focustense.StardewUI");
    ```

## Next Steps

At this point, you don't have any views or other assets; in order to get an actual UI up and running, you'll need to:

<div class="annotate" markdown>

1. Decide on a directory where your views and other assets will live inside your mod. (1)
2. [Register your asset path(s)](adding-ui-assets.md) so that the framework can find them.
3. Create a new view file using the `.sml` extension. See [editor setup](editor-setup.md) for recommended editors/configurations.
4. Ensure that the asset will be copied to your mod output. In Visual Studio, the file properties should look as follows:
   > ![Asset file properties](../images/screenshot-vs-asset-properties.png)
5. Author the view. See the [StarML guide](../framework/starml.md), or checkout the [examples](../examples/index.md) if you're feeling impatient.
6. Use the `IViewEngine` API you added earlier to [display the view](displaying-ui.md).

</div>

1.  We recommend `assets/views` for views and `assets/sprites` for sprites to be consistent with the examples and reference guides, but you can use any location(s) you prefer.

Note that steps 1-2 only need to be done one time. As long as you keep all your assets in one place (directory), then any new ones you add will be automatically picked up by the framework; no need to register every asset individually.

That's it! Have fun creating great UIs.