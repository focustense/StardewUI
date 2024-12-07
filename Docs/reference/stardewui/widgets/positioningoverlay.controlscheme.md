---
title: PositioningOverlay.ControlScheme
description: Configures the mapping of buttons to positioning actions in a PositioningOverlay.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class PositioningOverlay.ControlScheme

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Widgets](index.md)  
Assembly: StardewUI.dll  

</div>

Configures the mapping of buttons to positioning actions in a [PositioningOverlay](positioningoverlay.md).

```cs
public class PositioningOverlay.ControlScheme
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ ControlScheme

## Remarks

For all [IReadOnlyList&lt;T&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlylist-1) properties, **any** of the buttons can be pressed in order to perform that function; it is primarily intended to support left-stick/d-pad equivalency and WASD/arrow-key equivalency. Button combinations are not supported. 

 Keyboard control schemes only specify the fine movements; alignments are always controlled using number/numpad keys for each of the 9 possibilities.

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [ControlScheme()](#controlscheme) |  | 

### Properties

 | Name | Description |
| --- | --- |
| [FineDown](#finedown) | Buttons to shift the content down one pixel by modifying the [Offset](../layout/ninegridplacement.md#offset) of the [ContentPlacement](positioningoverlay.md#contentplacement). | 
| [FineLeft](#fineleft) | Buttons to shift the content left one pixel by modifying the [Offset](../layout/ninegridplacement.md#offset) of the [ContentPlacement](positioningoverlay.md#contentplacement). | 
| [FineRight](#fineright) | Buttons to shift the content right one pixel by modifying the [Offset](../layout/ninegridplacement.md#offset) of the [ContentPlacement](positioningoverlay.md#contentplacement). | 
| [FineUp](#fineup) | Buttons to shift the content up one pixel by modifying the [Offset](../layout/ninegridplacement.md#offset) of the [ContentPlacement](positioningoverlay.md#contentplacement). | 

## Details

### Constructors

#### ControlScheme()



```cs
public ControlScheme();
```

-----

### Properties

#### FineDown

Buttons to shift the content down one pixel by modifying the [Offset](../layout/ninegridplacement.md#offset) of the [ContentPlacement](positioningoverlay.md#contentplacement).

```cs
public System.Collections.Generic.IReadOnlyList<StardewModdingAPI.SButton> FineDown { get; set; }
```

##### Property Value

[IReadOnlyList](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlylist-1)<SButton>

-----

#### FineLeft

Buttons to shift the content left one pixel by modifying the [Offset](../layout/ninegridplacement.md#offset) of the [ContentPlacement](positioningoverlay.md#contentplacement).

```cs
public System.Collections.Generic.IReadOnlyList<StardewModdingAPI.SButton> FineLeft { get; set; }
```

##### Property Value

[IReadOnlyList](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlylist-1)<SButton>

-----

#### FineRight

Buttons to shift the content right one pixel by modifying the [Offset](../layout/ninegridplacement.md#offset) of the [ContentPlacement](positioningoverlay.md#contentplacement).

```cs
public System.Collections.Generic.IReadOnlyList<StardewModdingAPI.SButton> FineRight { get; set; }
```

##### Property Value

[IReadOnlyList](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlylist-1)<SButton>

-----

#### FineUp

Buttons to shift the content up one pixel by modifying the [Offset](../layout/ninegridplacement.md#offset) of the [ContentPlacement](positioningoverlay.md#contentplacement).

```cs
public System.Collections.Generic.IReadOnlyList<StardewModdingAPI.SButton> FineUp { get; set; }
```

##### Property Value

[IReadOnlyList](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlylist-1)<SButton>

-----

