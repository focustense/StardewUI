---
title: ShadowLayers
description: Describes which shadow layers will be drawn, for widgets such as Label that support layered shadows.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Enum ShadowLayers

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Widgets](index.md)  
Assembly: StardewUI.dll  

</div>

Describes which shadow layers will be drawn, for widgets such as [Label](label.md) that support layered shadows.

```cs
[System.Flags]
public enum ShadowLayers
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ [ValueType](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype) ⇦ [Enum](https://learn.microsoft.com/en-us/dotnet/api/system.enum) ⇦ ShadowLayers

## Fields

 | Name | Value | Description |
| --- | --- | --- |
| <a id="none">None</a> | 0 | No layers; the shadow will not be drawn. | 
| <a id="diagonal">Diagonal</a> | 1 | Diagonal shadow layer, with both a horizontal and vertical offset from the content. | 
| <a id="horizontal">Horizontal</a> | 2 | Horizontal shadow layer, using only the horizontal offset from content and ignoring vertical offset. | 
| <a id="horizontalanddiagonal">HorizontalAndDiagonal</a> | 3 | Combination of [Horizontal](shadowlayers.md#horizontal) and [Diagonal](shadowlayers.md#diagonal) layers. | 
| <a id="vertical">Vertical</a> | 4 | Vertical shadow layer, using only the vertical offset from content and ignoring horizontal offset. | 
| <a id="verticalanddiagonal">VerticalAndDiagonal</a> | 5 | Combination of [Vertical](shadowlayers.md#vertical) and [Diagonal](shadowlayers.md#diagonal) layers. | 
| <a id="horizontalandvertical">HorizontalAndVertical</a> | 6 | Combination of [Horizontal](shadowlayers.md#horizontal) and [Vertical](shadowlayers.md#vertical) layers. | 
| <a id="all">All</a> | 7 | Includes all individual shadow layers. | 

