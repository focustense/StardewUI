---
title: IViewEngine
description: Public API for StardewUI, abstracting away all implementation details of views and trees.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Interface IViewEngine

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework](index.md)  
Assembly: StardewUI.dll  

</div>

Public API for StardewUI, abstracting away all implementation details of views and trees.

```cs
public interface IViewEngine
```

## Members

### Methods

 | Name | Description |
| --- | --- |
| [CreateDrawableFromAsset(string)](#createdrawablefromassetstring) | Creates an [IViewDrawable](iviewdrawable.md) from the StarML stored in a game asset, as provided by a mod via SMAPI or Content Patcher. | 
| [CreateDrawableFromMarkup(string)](#createdrawablefrommarkupstring) | Creates an [IViewDrawable](iviewdrawable.md) from arbitrary markup. | 
| [CreateMenuControllerFromAsset(string, Object)](#createmenucontrollerfromassetstring-object) | Creates a menu from the StarML stored in a game asset, as provided by a mod via SMAPI or Content Patcher, and returns a controller for customizing the menu's behavior. | 
| [CreateMenuControllerFromMarkup(string, Object)](#createmenucontrollerfrommarkupstring-object) | Creates a menu from arbitrary markup, and returns a controller for customizing the menu's behavior. | 
| [CreateMenuFromAsset(string, Object)](#createmenufromassetstring-object) | Creates a menu from the StarML stored in a game asset, as provided by a mod via SMAPI or Content Patcher. | 
| [CreateMenuFromMarkup(string, Object)](#createmenufrommarkupstring-object) | Creates a menu from arbitrary markup. | 
| [EnableHotReloading(string)](#enablehotreloadingstring) | Starts monitoring this mod's directory for changes to assets managed by any of the `Register` methods, e.g. views and sprites. | 
| [RegisterSprites(string, string)](#registerspritesstring-string) | Registers a mod directory to be searched for sprite (and corresponding texture/sprite sheet data) assets. | 
| [RegisterViews(string, string)](#registerviewsstring-string) | Registers a mod directory to be searched for view (StarML) assets. Uses the `.sml` extension. | 

## Details

### Methods

#### CreateDrawableFromAsset(string)

Creates an [IViewDrawable](iviewdrawable.md) from the StarML stored in a game asset, as provided by a mod via SMAPI or Content Patcher.

```cs
StardewUI.Framework.IViewDrawable CreateDrawableFromAsset(string assetName);
```

##### Parameters

**`assetName`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The name of the StarML view asset in the content pipeline, e.g. `Mods/MyMod/Views/MyView`.

##### Returns

[IViewDrawable](iviewdrawable.md)

  An [IViewDrawable](iviewdrawable.md) for drawing directly to the [SpriteBatch](https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.SpriteBatch.html) of a rendering event or other draw handler.

##### Remarks

The [Context](iviewdrawable.md#context) and [MaxSize](iviewdrawable.md#maxsize) can be provided after creation.

-----

#### CreateDrawableFromMarkup(string)

Creates an [IViewDrawable](iviewdrawable.md) from arbitrary markup.

```cs
StardewUI.Framework.IViewDrawable CreateDrawableFromMarkup(string markup);
```

##### Parameters

**`markup`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The markup in StarML format.

##### Returns

[IViewDrawable](iviewdrawable.md)

  An [IViewDrawable](iviewdrawable.md) for drawing directly to the [SpriteBatch](https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.SpriteBatch.html) of a rendering event or other draw handler.

##### Remarks

The [Context](iviewdrawable.md#context) and [MaxSize](iviewdrawable.md#maxsize) can be provided after creation. 

**Warning:** Ad-hoc menus created this way cannot be cached, nor patched by other mods. Most mods should not use this API except for testing/experimentation.

-----

#### CreateMenuControllerFromAsset(string, Object)

Creates a menu from the StarML stored in a game asset, as provided by a mod via SMAPI or Content Patcher, and returns a controller for customizing the menu's behavior.

```cs
StardewUI.Framework.IMenuController CreateMenuControllerFromAsset(string assetName, System.Object context);
```

##### Parameters

**`assetName`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The name of the StarML view asset in the content pipeline, e.g. `Mods/MyMod/Views/MyView`.

**`context`** &nbsp; [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object)  
The context, or "model", for the menu's view, which holds any data-dependent values. **Note:** The type must implement [INotifyPropertyChanged](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged) in order for any changes to this data to be automatically reflected in the UI.

##### Returns

[IMenuController](imenucontroller.md)

  A controller object whose [Menu](imenucontroller.md#menu) is the created menu and whose other properties can be used to change menu-level behavior.

##### Remarks

The menu that is created is the same as the result of [CreateMenuFromMarkup(string, Object)](iviewengine.md#createmenufrommarkupstring-object). The menu is not automatically shown; to show it, use activeClickableMenu or equivalent.

-----

#### CreateMenuControllerFromMarkup(string, Object)

Creates a menu from arbitrary markup, and returns a controller for customizing the menu's behavior.

```cs
StardewUI.Framework.IMenuController CreateMenuControllerFromMarkup(string markup, System.Object context);
```

##### Parameters

**`markup`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The markup in StarML format.

**`context`** &nbsp; [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object)  
The context, or "model", for the menu's view, which holds any data-dependent values. **Note:** The type must implement [INotifyPropertyChanged](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged) in order for any changes to this data to be automatically reflected in the UI.

##### Returns

[IMenuController](imenucontroller.md)

  A controller object whose [Menu](imenucontroller.md#menu) is the created menu and whose other properties can be used to change menu-level behavior.

##### Remarks

**Warning:** Ad-hoc menus created this way cannot be cached, nor patched by other mods. Most mods should not use this API except for testing/experimentation.

-----

#### CreateMenuFromAsset(string, Object)

Creates a menu from the StarML stored in a game asset, as provided by a mod via SMAPI or Content Patcher.

```cs
StardewValley.Menus.IClickableMenu CreateMenuFromAsset(string assetName, System.Object context);
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
StardewValley.Menus.IClickableMenu CreateMenuFromMarkup(string markup, System.Object context);
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
void EnableHotReloading(string sourceDirectory);
```

##### Parameters

**`sourceDirectory`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
Optional source directory to watch and sync changes from. If not specified, or not a valid source directory, then hot reload will only pick up changes from within the live mod directory.

##### Remarks

If the `sourceDirectory` argument is specified, and points to a directory with the same asset structure as the mod, then an additional sync will be set up such that files modified in the `sourceDirectory` while the game is running will be copied to the active mod directory and subsequently reloaded. In other words, pointing this at the mod's `.csproj` directory allows hot reloading from the source files instead of the deployed mod's files. 

 Hot reload may impact game performance and should normally only be used during development and/or in debug mode.

-----

#### RegisterSprites(string, string)

Registers a mod directory to be searched for sprite (and corresponding texture/sprite sheet data) assets.

```cs
void RegisterSprites(string assetPrefix, string modDirectory);
```

##### Parameters

**`assetPrefix`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The prefix for all asset names, e.g. `Mods/MyMod/Sprites`. This can be any value but the same prefix must be used in `@AssetName` view bindings.

**`modDirectory`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The physical directory where the asset files are located, relative to the mod directory. Typically a path such as `assets/sprites`.

-----

#### RegisterViews(string, string)

Registers a mod directory to be searched for view (StarML) assets. Uses the `.sml` extension.

```cs
void RegisterViews(string assetPrefix, string modDirectory);
```

##### Parameters

**`assetPrefix`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The prefix for all asset names, e.g. `Mods/MyMod/Views`. This can be any value but the same prefix must be used in `include` elements and in API calls to create views.

**`modDirectory`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The physical directory where the asset files are located, relative to the mod directory. Typically a path such as `assets/views`.

-----

