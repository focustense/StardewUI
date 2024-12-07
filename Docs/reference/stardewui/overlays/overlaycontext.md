---
title: OverlayContext
description: The context of an overlay, e.g. the active overlays for a particular menu or other non-overlay UI.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class OverlayContext

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Overlays](index.md)  
Assembly: StardewUI.dll  

</div>

The context of an overlay, e.g. the active overlays for a particular menu or other non-overlay UI.

```cs
public class OverlayContext
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ OverlayContext

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [OverlayContext()](#overlaycontext) |  | 

### Properties

 | Name | Description |
| --- | --- |
| [Current](#current) | The ambient context for the UI root that is currently being displayed or handling events. | 
| [Front](#front) | Gets the overlay at the front of the stack. | 

### Methods

 | Name | Description |
| --- | --- |
| [BackToFront()](#backtofront) | Iterates the stack from the back/bottom/least-recent overlay to the front/top/most-recent. | 
| [FrontToBack()](#fronttoback) | Iterates the stack from the front/top/most-recent overlay to the back/bottom/least-recent. | 
| [Pop()](#pop) | Removes the front-most overlay. | 
| [Push(IOverlay)](#pushioverlay) | Pushes an overlay to the front. | 
| [Remove(IOverlay)](#removeioverlay) | Removes a specific overlay from the stack, regardless of its position. | 

### Events

 | Name | Description |
| --- | --- |
| [Pushed](#pushed) | Event raised when an overlay is pushed to the front. | 

## Details

### Constructors

#### OverlayContext()



```cs
public OverlayContext();
```

-----

### Properties

#### Current

The ambient context for the UI root that is currently being displayed or handling events.

```cs
public static StardewUI.Overlays.OverlayContext Current { get; set; }
```

##### Property Value

[OverlayContext](overlaycontext.md)

-----

#### Front

Gets the overlay at the front of the stack.

```cs
public StardewUI.Overlays.IOverlay Front { get; }
```

##### Property Value

[IOverlay](ioverlay.md)

-----

### Methods

#### BackToFront()

Iterates the stack from the back/bottom/least-recent overlay to the front/top/most-recent.

```cs
public System.Collections.Generic.IEnumerable<StardewUI.Overlays.IOverlay> BackToFront();
```

##### Returns

[IEnumerable](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)<[IOverlay](ioverlay.md)>

-----

#### FrontToBack()

Iterates the stack from the front/top/most-recent overlay to the back/bottom/least-recent.

```cs
public System.Collections.Generic.IEnumerable<StardewUI.Overlays.IOverlay> FrontToBack();
```

##### Returns

[IEnumerable](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)<[IOverlay](ioverlay.md)>

-----

#### Pop()

Removes the front-most overlay.

```cs
public StardewUI.Overlays.IOverlay Pop();
```

##### Returns

[IOverlay](ioverlay.md)

  The overlay previously at the front, or `null` if no overlays were active.

-----

#### Push(IOverlay)

Pushes an overlay to the front.

```cs
public void Push(StardewUI.Overlays.IOverlay overlay);
```

##### Parameters

**`overlay`** &nbsp; [IOverlay](ioverlay.md)  
The overlay to display on top of the current UI and any other overlays.

##### Remarks

If the specified `overlay` is already in the stack, then it will be moved from its previous position to the front.

-----

#### Remove(IOverlay)

Removes a specific overlay from the stack, regardless of its position.

```cs
public bool Remove(StardewUI.Overlays.IOverlay overlay);
```

##### Parameters

**`overlay`** &nbsp; [IOverlay](ioverlay.md)  
The overlay to remove.

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  `true` if the `overlay` was removed; `false` if it was not active.

##### Remarks

This is most often invoked by an overlay needing to dismiss itself, e.g. an overlay with an "OK" or "Close" button.

-----

### Events

#### Pushed

Event raised when an overlay is pushed to the front.

```cs
public event EventHandler<System.EventArgs>? Pushed;
```

##### Event Type

[EventHandler](https://learn.microsoft.com/en-us/dotnet/api/system.eventhandler-1)<[EventArgs](https://learn.microsoft.com/en-us/dotnet/api/system.eventargs)>

##### Remarks

This can either be a new overlay, or an overlay that was farther back and brought forward. After this event, the affected overlay will always be the [Front](overlaycontext.md#front).

-----

