---
title: ViewEngine
description: Implementation for the public IViewEngine API.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class ViewEngine

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Api](index.md)  
Assembly: StardewUI.dll  

</div>

Implementation for the public [IViewEngine](../iviewengine.md) API.

```cs
public class ViewEngine : StardewUI.Framework.IViewEngine
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ ViewEngine

**Implements**  
[IViewEngine](../iviewengine.md)

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [ViewEngine(IAssetCache, IModHelper, IViewNodeFactory)](#viewengineiassetcache-imodhelper-iviewnodefactory) | Initializes a new [ViewEngine](viewengine.md) instance. | 

### Properties

 | Name | Description |
| --- | --- |
| [SourceResolver](#sourceresolver) | Source resolver for resolving documents created by this view engine back to their original mod. | 

### Methods

 | Name | Description |
| --- | --- |
| [CreateDrawableFromAsset(string)](#createdrawablefromassetstring) | Creates an [IViewDrawable](../iviewdrawable.md) from the StarML stored in a game asset, as provided by a mod via SMAPI or Content Patcher. | 
| [CreateDrawableFromMarkup(string)](#createdrawablefrommarkupstring) | Creates an [IViewDrawable](../iviewdrawable.md) from arbitrary markup. | 
| [CreateMenuControllerFromAsset(string, Object)](#createmenucontrollerfromassetstring-object) | Creates a menu from the StarML stored in a game asset, as provided by a mod via SMAPI or Content Patcher, and returns a controller for customizing the menu's behavior. | 
| [CreateMenuControllerFromMarkup(string, Object)](#createmenucontrollerfrommarkupstring-object) | Creates a menu from arbitrary markup, and returns a controller for customizing the menu's behavior. | 
| [CreateMenuFromAsset(string, Object)](#createmenufromassetstring-object) | Creates a menu from the StarML stored in a game asset, as provided by a mod via SMAPI or Content Patcher. | 
| [CreateMenuFromMarkup(string, Object)](#createmenufrommarkupstring-object) | Creates a menu from arbitrary markup. | 
| [EnableHotReloading(string)](#enablehotreloadingstring) | Starts monitoring this mod's directory for changes to assets managed by any of the `Register` methods, e.g. views and sprites. | 
| [RegisterCustomData(string, string)](#registercustomdatastring-string) | Registers a mod directory to be searched for special-purpose mod data, i.e. that is not either views or sprites. | 
| [RegisterSprites(string, string)](#registerspritesstring-string) | Registers a mod directory to be searched for sprite (and corresponding texture/sprite sheet data) assets. | 
| [RegisterViews(string, string)](#registerviewsstring-string) | Registers a mod directory to be searched for view (StarML) assets. Uses the `.sml` extension. | 

## Details

### Constructors

#### ViewEngine(IAssetCache, IModHelper, IViewNodeFactory)

Initializes a new [ViewEngine](viewengine.md) instance.

```cs
public ViewEngine(StardewUI.Framework.Content.IAssetCache assetCache, StardewModdingAPI.IModHelper helper, StardewUI.Framework.Binding.IViewNodeFactory viewNodeFactory);
```

##### Parameters

**`assetCache`** &nbsp; [IAssetCache](../content/iassetcache.md)  
Cache for obtaining document assets. Used for asset-based views.

**`helper`** &nbsp; IModHelper  
SMAPI mod helper for the API consumer mod (not for StardewUI).

**`viewNodeFactory`** &nbsp; [IViewNodeFactory](../binding/iviewnodefactory.md)  
Factory for creating and binding [IViewNode](../binding/iviewnode.md)s.

-----

### Properties

#### SourceResolver

Source resolver for resolving documents created by this view engine back to their original mod.

```cs
public StardewUI.Framework.Content.ISourceResolver SourceResolver { get; }
```

##### Property Value

[ISourceResolver](../content/isourceresolver.md)

-----

### Methods

#### CreateDrawableFromAsset(string)

Creates an [IViewDrawable](../iviewdrawable.md) from the StarML stored in a game asset, as provided by a mod via SMAPI or Content Patcher.

```cs
public StardewUI.Framework.IViewDrawable CreateDrawableFromAsset(string assetName);
```

##### Parameters

**`assetName`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The name of the StarML view asset in the content pipeline, e.g. `Mods/MyMod/Views/MyView`.

##### Returns

[IViewDrawable](../iviewdrawable.md)

  An [IViewDrawable](../iviewdrawable.md) for drawing directly to the [SpriteBatch](https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.SpriteBatch.html) of a rendering event or other draw handler.

##### Remarks

The [Context](../iviewdrawable.md#context) and [MaxSize](../iviewdrawable.md#maxsize) can be provided after creation.

-----

#### CreateDrawableFromMarkup(string)

Creates an [IViewDrawable](../iviewdrawable.md) from arbitrary markup.

```cs
public StardewUI.Framework.IViewDrawable CreateDrawableFromMarkup(string markup);
```

##### Parameters

**`markup`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The markup in StarML format.

##### Returns

[IViewDrawable](../iviewdrawable.md)

  An [IViewDrawable](../iviewdrawable.md) for drawing directly to the [SpriteBatch](https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.SpriteBatch.html) of a rendering event or other draw handler.

##### Remarks

The [Context](../iviewdrawable.md#context) and [MaxSize](../iviewdrawable.md#maxsize) can be provided after creation. 

**Warning:** Ad-hoc menus created this way cannot be cached, nor patched by other mods. Most mods should not use this API except for testing/experimentation.

-----

#### CreateMenuControllerFromAsset(string, Object)

Creates a menu from the StarML stored in a game asset, as provided by a mod via SMAPI or Content Patcher, and returns a controller for customizing the menu's behavior.

```cs
public StardewUI.Framework.IMenuController CreateMenuControllerFromAsset(string assetName, System.Object context);
```

##### Parameters

**`assetName`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The name of the StarML view asset in the content pipeline, e.g. `Mods/MyMod/Views/MyView`.

**`context`** &nbsp; [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object)  
The context, or "model", for the menu's view, which holds any data-dependent values. **Note:** The type must implement [INotifyPropertyChanged](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged) in order for any changes to this data to be automatically reflected in the UI.

##### Returns

[IMenuController](../imenucontroller.md)

  A controller object whose [Menu](../imenucontroller.md#menu) is the created menu and whose other properties can be used to change menu-level behavior.

##### Remarks

The menu that is created is the same as the result of [CreateMenuFromMarkup(string, Object)](../iviewengine.md#createmenufrommarkupstring-object). The menu is not automatically shown; to show it, use activeClickableMenu or equivalent.

-----

#### CreateMenuControllerFromMarkup(string, Object)

Creates a menu from arbitrary markup, and returns a controller for customizing the menu's behavior.

```cs
public StardewUI.Framework.IMenuController CreateMenuControllerFromMarkup(string markup, System.Object context);
```

##### Parameters

**`markup`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The markup in StarML format.

**`context`** &nbsp; [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object)  
The context, or "model", for the menu's view, which holds any data-dependent values. **Note:** The type must implement [INotifyPropertyChanged](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged) in order for any changes to this data to be automatically reflected in the UI.

##### Returns

[IMenuController](../imenucontroller.md)

  A controller object whose [Menu](../imenucontroller.md#menu) is the created menu and whose other properties can be used to change menu-level behavior.

##### Remarks

**Warning:** Ad-hoc menus created this way cannot be cached, nor patched by other mods. Most mods should not use this API except for testing/experimentation.

-----

#### CreateMenuFromAsset(string, Object)

Creates a menu from the StarML stored in a game asset, as provided by a mod via SMAPI or Content Patcher.

```cs
public StardewValley.Menus.IClickableMenu CreateMenuFromAsset(string assetName, System.Object context);
```

##### Parameters

**`assetName`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The name of the StarML view asset in the content pipeline, e.g. `Mods/MyMod/Views/MyView`.

**`context`** &nbsp; [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object)  
The context, or "model", for the menu's view, which holds any data-dependent values. **Note:** The type must implement [INotifyPropertyChanged](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged) in order for any changes to this data to be automatically reflected in the UI.

##### Returns

IClickableMenu

  A menu object which can be shown using the game's standard menu APIs such as activeClickableMenu.

##### Remarks

Does not make the menu active. To show it, use activeClickableMenu or equivalent.

-----

#### CreateMenuFromMarkup(string, Object)

Creates a menu from arbitrary markup.

```cs
public StardewValley.Menus.IClickableMenu CreateMenuFromMarkup(string markup, System.Object context);
```

##### Parameters

**`markup`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The markup in StarML format.

**`context`** &nbsp; [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object)  
The context, or "model", for the menu's view, which holds any data-dependent values. **Note:** The type must implement [INotifyPropertyChanged](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged) in order for any changes to this data to be automatically reflected in the UI.

##### Returns

IClickableMenu

  A menu object which can be shown using the game's standard menu APIs such as activeClickableMenu.

##### Remarks

**Warning:** Ad-hoc menus created this way cannot be cached, nor patched by other mods. Most mods should not use this API except for testing/experimentation.

-----

#### EnableHotReloading(string)

Starts monitoring this mod's directory for changes to assets managed by any of the `Register` methods, e.g. views and sprites.

```cs
public void EnableHotReloading(string sourceDirectory);
```

##### Parameters

**`sourceDirectory`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
Optional source directory to watch and sync changes from. If not specified, or not a valid source directory, then hot reload will only pick up changes from within the live mod directory.

##### Remarks

If the `sourceDirectory` argument is specified, and points to a directory with the same asset structure as the mod, then an additional sync will be set up such that files modified in the `sourceDirectory` while the game is running will be copied to the active mod directory and subsequently reloaded. In other words, pointing this at the mod's `.csproj` directory allows hot reloading from the source files instead of the deployed mod's files. 

 Hot reload may impact game performance and should normally only be used during development and/or in debug mode.

-----

#### RegisterCustomData(string, string)

Registers a mod directory to be searched for special-purpose mod data, i.e. that is not either views or sprites.

```cs
public void RegisterCustomData(string assetPrefix, string modDirectory);
```

##### Parameters

**`assetPrefix`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The prefix for all asset names, **excluding** the category which is deduced from the file extension as described in the remarks. For example, given a value of `Mods/MyMod`, a file named `foo.buttonspritemap.json` would be referenced in views as `@Mods/MyMod/ButtonSpriteMaps/Foo`.

**`modDirectory`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The physical directory where the asset files are located, relative to the mod directory. Typically a path such as `assets/ui` or `assets/ui/data`.

##### Remarks

Allowed extensions for files in this folder and their corresponding data types are: 

  - `.buttonspritemap.json` - [ButtonSpriteMapData](https://focustense.github.io/StardewUI/reference/stardewui/data/buttonspritemapdata/)

-----

#### RegisterSprites(string, string)

Registers a mod directory to be searched for sprite (and corresponding texture/sprite sheet data) assets.

```cs
public void RegisterSprites(string assetPrefix, string modDirectory);
```

##### Parameters

**`assetPrefix`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The prefix for all asset names, e.g. `Mods/MyMod/Sprites`. This can be any value but the same prefix must be used in `@AssetName` view bindings.

**`modDirectory`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The physical directory where the asset files are located, relative to the mod directory. Typically a path such as `assets/sprites` or `assets/ui/sprites`.

-----

#### RegisterViews(string, string)

Registers a mod directory to be searched for view (StarML) assets. Uses the `.sml` extension.

```cs
public void RegisterViews(string assetPrefix, string modDirectory);
```

##### Parameters

**`assetPrefix`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The prefix for all asset names, e.g. `Mods/MyMod/Views`. This can be any value but the same prefix must be used in `include` elements and in API calls to create views.

**`modDirectory`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The physical directory where the asset files are located, relative to the mod directory. Typically a path such as `assets/views` or `assets/ui/views`.

-----

