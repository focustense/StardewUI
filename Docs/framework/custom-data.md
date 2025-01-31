# Custom Data

The primary asset types used with StardewUI are [Views](../getting-started/adding-ui-assets.md#adding-views) and [Sprites](../getting-started/adding-ui-assets.md#adding-sprites). Some parts of the library/framework may also use specialized data types that users may override or add to. These are collectively referred to as _Custom Data_.

## Types of Custom Data

"Custom" in this context means that authors can supply their own version of the data, but the data type itself is defined by the Framework; it is more like a self-contained [content pack](https://stardewvalleywiki.com/Modding:Content_packs) than a place for arbitrary storage.

The supported types are:

/// html | div.no-code-break

| Data Type | File Extension | Asset Path | Purpose |
| --- | --- | --- | --- |
| [`ButtonSpriteMapData`](https://focustense.github.io/StardewUI/reference/stardewui/data/buttonspritemapdata/) | `.buttonspritemap.json` | `SpriteMaps` | Provides the keyboard/gamepad prompt sprites to be used in a [Keybind](../library/standard-views.md#keybind), [Keybind Editor](../library/standard-views.md#keybind-editor) or similar widget.

///

## Registering Custom Data

The registration process for custom data is very similar to other [asset registration](../getting-started/adding-ui-assets.md):

```cs
viewEngine.RegisterCustomData($"Mods/author.ModName", "assets/uidata");
```

Note that although all asset paths are entirely convention-based, meaning you can specify any asset path you want as long as you are consistent with it in content lookups and [asset bindings](starml.md#attribute-flavors), the recommendation is **not** to use a suffix for the data path. In other words, do not add `/Data` to the end where you might otherwise add `/Sprites`, `/Views`, etc.

The reason for this is that custom data is already considered to be "mixed", and the file extension indicated in the table above will automatically map to the corresponding asset path suffix that indicates its type. For example, given the above registration line, a file named `Buttons.buttonspritemap.json` would end up having the path `@Mods/author.ModName/SpriteMaps/Buttons`.