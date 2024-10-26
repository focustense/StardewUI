---
title: UiViewport
description: Utilities relating to the game's UI viewport.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class UiViewport

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI](index.md)  
Assembly: StardewUI.dll  

</div>

Utilities relating to the game's UI viewport.

```cs
public static class UiViewport
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ UiViewport

## Members

### Methods

 | Name | Description |
| --- | --- |
| [GetMaxSize()](#getmaxsize) | Gets the maximum size for the entire viewport. | 

## Details

### Methods

#### GetMaxSize()

Gets the maximum size for the entire viewport.

```cs
public static Microsoft.Xna.Framework.Point GetMaxSize();
```

##### Returns

[Point](https://docs.monogame.net/api/Microsoft.Xna.Framework.Point.html)

  The game's uiViewport, constrained to the viewport of the current [GraphicsDevice](https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.GraphicsDevice.html).

-----

