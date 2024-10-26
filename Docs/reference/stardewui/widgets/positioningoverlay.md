---
title: PositioningOverlay
description: An overlay that can be used to edit the position of some arbitrary content.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class PositioningOverlay

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Widgets](index.md)  
Assembly: StardewUI.dll  

</div>

An overlay that can be used to edit the position of some arbitrary content.

```cs
public class PositioningOverlay : StardewUI.Overlays.FullScreenOverlay
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ [FullScreenOverlay](../overlays/fullscreenoverlay.md) ⇦ PositioningOverlay

## Remarks

Note that the widget only provides a means to visually/interactively obtain a new position, similar to e.g. obtaining a text string from a modal input query. It is up to the caller to persist the resulting [ContentPlacement](positioningoverlay.md#contentplacement) to configuration and determine how to actually position the content in its usual context (e.g. game HUD).

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [PositioningOverlay(ISpriteMap&lt;SButton&gt;, ISpriteMap&lt;Direction&gt;)](#positioningoverlayispritemapsbutton-ispritemapdirection) | An overlay that can be used to edit the position of some arbitrary content. | 

### Properties

 | Name | Description |
| --- | --- |
| [CapturingInput](../overlays/fullscreenoverlay.md#capturinginput) | Whether the overlay wants to capture all keyboard and gamepad inputs, i.e. prevent them from being dispatched to the parent menu.<br><span class="muted" markdown>(Inherited from [FullScreenOverlay](../overlays/fullscreenoverlay.md))</span> | 
| [Content](#content) | The content that is being positioned. | 
| [ContentPlacement](#contentplacement) | Current placement of the [Content](positioningoverlay.md#content) within the viewport. | 
| [DimmingAmount](../overlays/fullscreenoverlay.md#dimmingamount) | Amount to dim whatever is underneath the overlay.<br><span class="muted" markdown>(Inherited from [FullScreenOverlay](../overlays/fullscreenoverlay.md))</span> | 
| [GamepadControls](#gamepadcontrols) | The control scheme to use when positioning with a gamepad. | 
| [HorizontalAlignment](../overlays/fullscreenoverlay.md#horizontalalignment) | Horizontal alignment of the overlay relative to the [Parent](../overlays/ioverlay.md#parent) edge.<br><span class="muted" markdown>(Inherited from [FullScreenOverlay](../overlays/fullscreenoverlay.md))</span> | 
| [HorizontalParentAlignment](../overlays/fullscreenoverlay.md#horizontalparentalignment) | Specifies which edge of the [Parent](../overlays/ioverlay.md#parent) (or screen, if no parent is specified) will be used to align the overlay edge denoted by its [HorizontalAlignment](../overlays/ioverlay.md#horizontalalignment).<br><span class="muted" markdown>(Inherited from [FullScreenOverlay](../overlays/fullscreenoverlay.md))</span> | 
| [KeyboardControls](#keyboardcontrols) | The control scheme to use when positioning with keyboard/mouse. | 
| [Parent](../overlays/fullscreenoverlay.md#parent) | The parent of this overlay, used for positioning. If not specified, then the overlay will be positioned relative to the entire UI viewport.<br><span class="muted" markdown>(Inherited from [FullScreenOverlay](../overlays/fullscreenoverlay.md))</span> | 
| [ParentOffset](../overlays/fullscreenoverlay.md#parentoffset) | Additional pixel offset to apply to the overlay's position, after alignments.<br><span class="muted" markdown>(Inherited from [FullScreenOverlay](../overlays/fullscreenoverlay.md))</span> | 
| [VerticalAlignment](../overlays/fullscreenoverlay.md#verticalalignment) | Vertical alignment of the overlay relative to the [Parent](../overlays/ioverlay.md#parent) edge.<br><span class="muted" markdown>(Inherited from [FullScreenOverlay](../overlays/fullscreenoverlay.md))</span> | 
| [VerticalParentAlignment](../overlays/fullscreenoverlay.md#verticalparentalignment) | Specifies which edge of the [Parent](../overlays/ioverlay.md#parent) (or screen, if no parent is specified) will be used to align the overlay edge denoted by its [VerticalAlignment](../overlays/ioverlay.md#verticalalignment).<br><span class="muted" markdown>(Inherited from [FullScreenOverlay](../overlays/fullscreenoverlay.md))</span> | 
| [View](../overlays/fullscreenoverlay.md#view) | The view to be displayed/interacted with as an overlay.<br><span class="muted" markdown>(Inherited from [FullScreenOverlay](../overlays/fullscreenoverlay.md))</span> | 

### Methods

 | Name | Description |
| --- | --- |
| [CreateView()](#createview) | Creates the content view that will be displayed as an overlay.<br><span class="muted" markdown>(Overrides [FullScreenOverlay](../overlays/fullscreenoverlay.md).[CreateView()](../overlays/fullscreenoverlay.md#createview))</span> | 
| [OnClose()](../overlays/fullscreenoverlay.md#onclose) | Runs when the overlay is removed from the active stack.<br><span class="muted" markdown>(Inherited from [FullScreenOverlay](../overlays/fullscreenoverlay.md))</span> | 
| [RequireView&lt;TChild&gt;(Func&lt;TChild&gt;)](../overlays/fullscreenoverlay.md#requireviewtchildfunctchild) | Ensures that the overlay view is created before attempting to access a child view.<br><span class="muted" markdown>(Inherited from [FullScreenOverlay](../overlays/fullscreenoverlay.md))</span> | 
| [Update(TimeSpan)](../overlays/fullscreenoverlay.md#updatetimespan) | Runs on every game update tick.<br><span class="muted" markdown>(Inherited from [FullScreenOverlay](../overlays/fullscreenoverlay.md))</span> | 

### Events

 | Name | Description |
| --- | --- |
| [Close](../overlays/fullscreenoverlay.md#close) | Event raised when the overlay is closed - i.e. removed from the current context stack.<br><span class="muted" markdown>(Inherited from [FullScreenOverlay](../overlays/fullscreenoverlay.md))</span> | 

## Details

### Constructors

#### PositioningOverlay(ISpriteMap&lt;SButton&gt;, ISpriteMap&lt;Direction&gt;)

An overlay that can be used to edit the position of some arbitrary content.

```cs
public PositioningOverlay(StardewUI.Graphics.ISpriteMap<StardewModdingAPI.SButton> buttonSpriteMap, StardewUI.Graphics.ISpriteMap<StardewUI.Direction> directionSpriteMap);
```

##### Parameters

**`buttonSpriteMap`** &nbsp; [ISpriteMap](../graphics/ispritemap-1.md)<SButton>  
Map of buttons to button prompt sprites.

**`directionSpriteMap`** &nbsp; [ISpriteMap](../graphics/ispritemap-1.md)<[Direction](../direction.md)>  
Map of directions to directional arrow sprites; used to indicate dragging.

##### Remarks

Note that the widget only provides a means to visually/interactively obtain a new position, similar to e.g. obtaining a text string from a modal input query. It is up to the caller to persist the resulting [ContentPlacement](positioningoverlay.md#contentplacement) to configuration and determine how to actually position the content in its usual context (e.g. game HUD).

-----

### Properties

#### Content

The content that is being positioned.

```cs
public StardewUI.IView Content { get; set; }
```

##### Property Value

[IView](../iview.md)

##### Remarks

This is normally a "representative" version of the real content, as the true HUD widget or other element may not exist or have its properties known at configuration time.

-----

#### ContentPlacement

Current placement of the [Content](positioningoverlay.md#content) within the viewport.

```cs
public StardewUI.Layout.NineGridPlacement ContentPlacement { get; set; }
```

##### Property Value

[NineGridPlacement](../layout/ninegridplacement.md)

-----

#### GamepadControls

The control scheme to use when positioning with a gamepad.

```cs
public StardewUI.Widgets.PositioningOverlay.GamepadControlScheme GamepadControls { get; set; }
```

##### Property Value

[GamepadControlScheme](positioningoverlay.gamepadcontrolscheme.md)

-----

#### KeyboardControls

The control scheme to use when positioning with keyboard/mouse.

```cs
public StardewUI.Widgets.PositioningOverlay.ControlScheme KeyboardControls { get; set; }
```

##### Property Value

[ControlScheme](positioningoverlay.controlscheme.md)

-----

### Methods

#### CreateView()

Creates the content view that will be displayed as an overlay.

```cs
protected override StardewUI.IView CreateView();
```

##### Returns

[IView](../iview.md)

-----

