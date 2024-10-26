---
title: XeluButtonSpriteMap.SpriteTheme
description: Available theme variants for certain sprites.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Enum XeluButtonSpriteMap.SpriteTheme

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Graphics](index.md)  
Assembly: StardewUI.dll  

</div>

Available theme variants for certain sprites.

```cs
public enum XeluButtonSpriteMap.SpriteTheme
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ [ValueType](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype) ⇦ [Enum](https://learn.microsoft.com/en-us/dotnet/api/system.enum) ⇦ SpriteTheme

## Remarks

Applies to the keyboard and mouse sprites, but not controller (Xbox style) sprites.

## Fields

 | Name | Value | Description |
| --- | --- | --- |
| <a id="dark">Dark</a> | 0 | Black and dark gray, with white highlights (e.g. for pressed mouse button). | 
| <a id="light">Light</a> | 1 | White and light gray, with red highlights (e.g. for pressed mouse button). | 
| <a id="stardew">Stardew</a> | 2 | Custom theme mimicking the Stardew yellow-orange palette; falls back to [Light](xelubuttonspritemap.spritetheme.md#light) for non-customized sprites. | 

