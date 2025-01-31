---
title: ButtonSpriteMapData
description: JSON configuration data for a ButtonSpriteMap.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class ButtonSpriteMapData

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Data](index.md)  
Assembly: StardewUI.dll  

</div>

JSON configuration data for a [ButtonSpriteMap](../graphics/buttonspritemap.md).

```cs
public class ButtonSpriteMapData
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ ButtonSpriteMapData

## Remarks

The data is based on having the sprites themselves registered as assets, e.g. via `StardewUI.Data.SpriteSheetData`.

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [ButtonSpriteMapData()](#buttonspritemapdata) |  | 

### Properties

 | Name | Description |
| --- | --- |
| [Buttons](#buttons) | Map of buttons to the asset name used for that button's sprite. | 
| [ControllerBlank](#controllerblank) | Name of the sprite asset to use for [ControllerBlank](../graphics/buttonspritemap.md#controllerblank). | 
| [KeyboardBlank](#keyboardblank) | Name of the sprite asset to use for [KeyboardBlank](../graphics/buttonspritemap.md#keyboardblank). | 
| [MouseLeft](#mouseleft) | Name of the sprite asset to use for [MouseLeft](../graphics/buttonspritemap.md#mouseleft). | 
| [MouseMiddle](#mousemiddle) | Name of the sprite asset to use for [MouseMiddle](../graphics/buttonspritemap.md#mousemiddle). | 
| [MouseRight](#mouseright) | Name of the sprite asset to use for [MouseRight](../graphics/buttonspritemap.md#mouseright). | 

## Details

### Constructors

#### ButtonSpriteMapData()



```cs
public ButtonSpriteMapData();
```

-----

### Properties

#### Buttons

Map of buttons to the asset name used for that button's sprite.

```cs
public System.Collections.Generic.Dictionary<StardewModdingAPI.SButton, string> Buttons { get; set; }
```

##### Property Value

[Dictionary](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2)<SButton, [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)>

##### Remarks

Specific button sprites **replace** the [ControllerBlank](buttonspritemapdata.md#controllerblank) or [KeyboardBlank](buttonspritemapdata.md#keyboardblank) and therefore must include both the border and inner icon/text.

-----

#### ControllerBlank

Name of the sprite asset to use for [ControllerBlank](../graphics/buttonspritemap.md#controllerblank).

```cs
public string ControllerBlank { get; set; }
```

##### Property Value

[string](https://learn.microsoft.com/en-us/dotnet/api/system.string)

##### Remarks

Used as the background for any controller button lacking its own unique sprite; the button name is rendered inside as regular text.

-----

#### KeyboardBlank

Name of the sprite asset to use for [KeyboardBlank](../graphics/buttonspritemap.md#keyboardblank).

```cs
public string KeyboardBlank { get; set; }
```

##### Property Value

[string](https://learn.microsoft.com/en-us/dotnet/api/system.string)

##### Remarks

Used as the background for any key lacking its own unique sprite; the key name is rendered inside as regular text.

-----

#### MouseLeft

Name of the sprite asset to use for [MouseLeft](../graphics/buttonspritemap.md#mouseleft).

```cs
public string MouseLeft { get; set; }
```

##### Property Value

[string](https://learn.microsoft.com/en-us/dotnet/api/system.string)

-----

#### MouseMiddle

Name of the sprite asset to use for [MouseMiddle](../graphics/buttonspritemap.md#mousemiddle).

```cs
public string MouseMiddle { get; set; }
```

##### Property Value

[string](https://learn.microsoft.com/en-us/dotnet/api/system.string)

-----

#### MouseRight

Name of the sprite asset to use for [MouseRight](../graphics/buttonspritemap.md#mouseright).

```cs
public string MouseRight { get; set; }
```

##### Property Value

[string](https://learn.microsoft.com/en-us/dotnet/api/system.string)

-----

