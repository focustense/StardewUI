---
title: PositioningOverlay.GamepadControlScheme
description: Configures the mapping of buttons to positioning actions in a PositioningOverlay. Includes the generic PositioningOverlay.ControlScheme settings as well as grid-movement settings specific to gamepads.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class PositioningOverlay.GamepadControlScheme

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Widgets](index.md)  
Assembly: StardewUI.dll  

</div>

Configures the mapping of buttons to positioning actions in a [PositioningOverlay](positioningoverlay.md). Includes the generic [ControlScheme](positioningoverlay.controlscheme.md) settings as well as grid-movement settings specific to gamepads.

```cs
public class PositioningOverlay.GamepadControlScheme : 
    StardewUI.Widgets.PositioningOverlay.ControlScheme
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ [ControlScheme](positioningoverlay.controlscheme.md) ⇦ GamepadControlScheme

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [GamepadControlScheme()](#gamepadcontrolscheme) |  | 

### Properties

 | Name | Description |
| --- | --- |
| [FineDown](positioningoverlay.controlscheme.md#finedown) | Buttons to shift the content down one pixel by modifying the [Offset](../layout/ninegridplacement.md#offset) of the [ContentPlacement](positioningoverlay.md#contentplacement).<br><span class="muted" markdown>(Inherited from [ControlScheme](positioningoverlay.controlscheme.md))</span> | 
| [FineLeft](positioningoverlay.controlscheme.md#fineleft) | Buttons to shift the content left one pixel by modifying the [Offset](../layout/ninegridplacement.md#offset) of the [ContentPlacement](positioningoverlay.md#contentplacement).<br><span class="muted" markdown>(Inherited from [ControlScheme](positioningoverlay.controlscheme.md))</span> | 
| [FineRight](positioningoverlay.controlscheme.md#fineright) | Buttons to shift the content right one pixel by modifying the [Offset](../layout/ninegridplacement.md#offset) of the [ContentPlacement](positioningoverlay.md#contentplacement).<br><span class="muted" markdown>(Inherited from [ControlScheme](positioningoverlay.controlscheme.md))</span> | 
| [FineUp](positioningoverlay.controlscheme.md#fineup) | Buttons to shift the content up one pixel by modifying the [Offset](../layout/ninegridplacement.md#offset) of the [ContentPlacement](positioningoverlay.md#contentplacement).<br><span class="muted" markdown>(Inherited from [ControlScheme](positioningoverlay.controlscheme.md))</span> | 
| [GridDown](#griddown) | Buttons to shift the content down by one grid cell by changing the [VerticalAlignment](../layout/ninegridplacement.md#verticalalignment) of the [ContentPlacement](positioningoverlay.md#contentplacement). [Start](../layout/alignment.md#start) becomes [Middle](../layout/alignment.md#middle) and [Middle](../layout/alignment.md#middle) becomes [End](../layout/alignment.md#end). | 
| [GridLeft](#gridleft) | Buttons to shift the content left by one grid cell by changing the [HorizontalAlignment](../layout/ninegridplacement.md#horizontalalignment) of the [ContentPlacement](positioningoverlay.md#contentplacement). [End](../layout/alignment.md#end) becomes [Middle](../layout/alignment.md#middle) and [Middle](../layout/alignment.md#middle) becomes [Start](../layout/alignment.md#start). | 
| [GridRight](#gridright) | Buttons to shift the content right by one grid cell by changing the [HorizontalAlignment](../layout/ninegridplacement.md#horizontalalignment) of the [ContentPlacement](positioningoverlay.md#contentplacement). [Start](../layout/alignment.md#start) becomes [Middle](../layout/alignment.md#middle) and [Middle](../layout/alignment.md#middle) becomes [End](../layout/alignment.md#end). | 
| [GridUp](#gridup) | Buttons to shift the content up by one grid cell by changing the [VerticalAlignment](../layout/ninegridplacement.md#verticalalignment) of the [ContentPlacement](positioningoverlay.md#contentplacement). [End](../layout/alignment.md#end) becomes [Middle](../layout/alignment.md#middle) and [Middle](../layout/alignment.md#middle) becomes [Start](../layout/alignment.md#start). | 
| [Modifier](#modifier) | Modifier key to switch between grid- and fine-positioning modes. | 

## Details

### Constructors

#### GamepadControlScheme()



```cs
public GamepadControlScheme();
```

-----

### Properties

#### GridDown

Buttons to shift the content down by one grid cell by changing the [VerticalAlignment](../layout/ninegridplacement.md#verticalalignment) of the [ContentPlacement](positioningoverlay.md#contentplacement). [Start](../layout/alignment.md#start) becomes [Middle](../layout/alignment.md#middle) and [Middle](../layout/alignment.md#middle) becomes [End](../layout/alignment.md#end).

```cs
public System.Collections.Generic.IReadOnlyList<StardewModdingAPI.SButton> GridDown { get; set; }
```

##### Property Value

[IReadOnlyList](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlylist-1)<SButton>

-----

#### GridLeft

Buttons to shift the content left by one grid cell by changing the [HorizontalAlignment](../layout/ninegridplacement.md#horizontalalignment) of the [ContentPlacement](positioningoverlay.md#contentplacement). [End](../layout/alignment.md#end) becomes [Middle](../layout/alignment.md#middle) and [Middle](../layout/alignment.md#middle) becomes [Start](../layout/alignment.md#start).

```cs
public System.Collections.Generic.IReadOnlyList<StardewModdingAPI.SButton> GridLeft { get; set; }
```

##### Property Value

[IReadOnlyList](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlylist-1)<SButton>

-----

#### GridRight

Buttons to shift the content right by one grid cell by changing the [HorizontalAlignment](../layout/ninegridplacement.md#horizontalalignment) of the [ContentPlacement](positioningoverlay.md#contentplacement). [Start](../layout/alignment.md#start) becomes [Middle](../layout/alignment.md#middle) and [Middle](../layout/alignment.md#middle) becomes [End](../layout/alignment.md#end).

```cs
public System.Collections.Generic.IReadOnlyList<StardewModdingAPI.SButton> GridRight { get; set; }
```

##### Property Value

[IReadOnlyList](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlylist-1)<SButton>

-----

#### GridUp

Buttons to shift the content up by one grid cell by changing the [VerticalAlignment](../layout/ninegridplacement.md#verticalalignment) of the [ContentPlacement](positioningoverlay.md#contentplacement). [End](../layout/alignment.md#end) becomes [Middle](../layout/alignment.md#middle) and [Middle](../layout/alignment.md#middle) becomes [Start](../layout/alignment.md#start).

```cs
public System.Collections.Generic.IReadOnlyList<StardewModdingAPI.SButton> GridUp { get; set; }
```

##### Property Value

[IReadOnlyList](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlylist-1)<SButton>

-----

#### Modifier

Modifier key to switch between grid- and fine-positioning modes.

```cs
public StardewModdingAPI.SButton Modifier { get; set; }
```

##### Property Value

SButton

##### Remarks

If specified, the default motion will be fine, and the modifier key must be held in order to move accross the grid.

-----

