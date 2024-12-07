---
title: FullScreenOverlay
description: Base class for an overlay meant to take up the full screen.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class FullScreenOverlay

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Overlays](index.md)  
Assembly: StardewUI.dll  

</div>

Base class for an overlay meant to take up the full screen.

```cs
public class FullScreenOverlay : StardewUI.Overlays.IOverlay
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ FullScreenOverlay

**Implements**  
[IOverlay](ioverlay.md)

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [FullScreenOverlay()](#fullscreenoverlay) | Initializes a new instance of [FullScreenOverlay](fullscreenoverlay.md). | 

### Properties

 | Name | Description |
| --- | --- |
| [CapturingInput](#capturinginput) | Whether the overlay wants to capture all keyboard and gamepad inputs, i.e. prevent them from being dispatched to the parent menu. | 
| [DimmingAmount](#dimmingamount) | Amount to dim whatever is underneath the overlay. | 
| [HorizontalAlignment](#horizontalalignment) | Horizontal alignment of the overlay relative to the [Parent](ioverlay.md#parent) edge. | 
| [HorizontalParentAlignment](#horizontalparentalignment) | Specifies which edge of the [Parent](ioverlay.md#parent) (or screen, if no parent is specified) will be used to align the overlay edge denoted by its [HorizontalAlignment](ioverlay.md#horizontalalignment). | 
| [Parent](#parent) | The parent of this overlay, used for positioning. If not specified, then the overlay will be positioned relative to the entire UI viewport. | 
| [ParentOffset](#parentoffset) | Additional pixel offset to apply to the overlay's position, after alignments. | 
| [VerticalAlignment](#verticalalignment) | Vertical alignment of the overlay relative to the [Parent](ioverlay.md#parent) edge. | 
| [VerticalParentAlignment](#verticalparentalignment) | Specifies which edge of the [Parent](ioverlay.md#parent) (or screen, if no parent is specified) will be used to align the overlay edge denoted by its [VerticalAlignment](ioverlay.md#verticalalignment). | 
| [View](#view) | The view to be displayed/interacted with as an overlay. | 

### Methods

 | Name | Description |
| --- | --- |
| [CreateView()](#createview) | Creates the content view that will be displayed as an overlay. | 
| [OnClose()](#onclose) | Runs when the overlay is removed from the active stack. | 
| [RequireView&lt;TChild&gt;(Func&lt;TChild&gt;)](#requireviewtchildfunctchild) | Ensures that the overlay view is created before attempting to access a child view. | 
| [Update(TimeSpan)](#updatetimespan) | Runs on every game update tick. | 

### Events

 | Name | Description |
| --- | --- |
| [Close](#close) | Event raised when the overlay is closed - i.e. removed from the current context stack. | 

## Details

### Constructors

#### FullScreenOverlay()

Initializes a new instance of [FullScreenOverlay](fullscreenoverlay.md).

```cs
public FullScreenOverlay();
```

-----

### Properties

#### CapturingInput

Whether the overlay wants to capture all keyboard and gamepad inputs, i.e. prevent them from being dispatched to the parent menu.

```cs
public bool CapturingInput { get; protected set; }
```

##### Property Value

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

##### Remarks

This is not necessary to trap focus, which happens automatically; only to capture buttons/keys that would normally have a navigation function, like triggers/shoulders for paging, E/Esc/GamepadB for cancellation, etc. Overlays that enable capturing should provide their own way for the user to escape using keyboard/gamepad, although it is always possible to click the mouse outside the overlay to dismiss it (and implicitly stop the capturing).

-----

#### DimmingAmount

Amount to dim whatever is underneath the overlay.

```cs
public float DimmingAmount { get; set; }
```

##### Property Value

[Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)

##### Remarks

This is an alpha value for a black overlay, so the higher value (between 0 and 1) the darker the content underneath the overlay. These apply individually to each overlay, so multiple stacked overlays will dim not only the underlying main view but also any previous overlays.

-----

#### HorizontalAlignment

Horizontal alignment of the overlay relative to the [Parent](ioverlay.md#parent) edge.

```cs
public StardewUI.Layout.Alignment HorizontalAlignment { get; }
```

##### Property Value

[Alignment](../layout/alignment.md)

##### Remarks

Full-screen overlays should generally stretch to the entire viewport dimensions, but are middle-aligned in case of a discrepancy.

-----

#### HorizontalParentAlignment

Specifies which edge of the [Parent](ioverlay.md#parent) (or screen, if no parent is specified) will be used to align the overlay edge denoted by its [HorizontalAlignment](ioverlay.md#horizontalalignment).

```cs
public StardewUI.Layout.Alignment HorizontalParentAlignment { get; }
```

##### Property Value

[Alignment](../layout/alignment.md)

##### Remarks

Full-screen overlays should generally stretch to the entire viewport dimensions, but are middle-aligned in case of a discrepancy.

-----

#### Parent

The parent of this overlay, used for positioning. If not specified, then the overlay will be positioned relative to the entire UI viewport.

```cs
public StardewUI.IView Parent { get; }
```

##### Property Value

[IView](../iview.md)

##### Remarks

Full-screen overlays always have a `null` parent.

-----

#### ParentOffset

Additional pixel offset to apply to the overlay's position, after alignments.

```cs
public Microsoft.Xna.Framework.Vector2 ParentOffset { get; }
```

##### Property Value

[Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)

-----

#### VerticalAlignment

Vertical alignment of the overlay relative to the [Parent](ioverlay.md#parent) edge.

```cs
public StardewUI.Layout.Alignment VerticalAlignment { get; }
```

##### Property Value

[Alignment](../layout/alignment.md)

##### Remarks

Full-screen overlays should generally stretch to the entire viewport dimensions, but are middle-aligned in case of a discrepancy.

-----

#### VerticalParentAlignment

Specifies which edge of the [Parent](ioverlay.md#parent) (or screen, if no parent is specified) will be used to align the overlay edge denoted by its [VerticalAlignment](ioverlay.md#verticalalignment).

```cs
public StardewUI.Layout.Alignment VerticalParentAlignment { get; }
```

##### Property Value

[Alignment](../layout/alignment.md)

##### Remarks

Full-screen overlays should generally stretch to the entire viewport dimensions, but are middle-aligned in case of a discrepancy.

-----

#### View

The view to be displayed/interacted with as an overlay.

```cs
public StardewUI.IView View { get; }
```

##### Property Value

[IView](../iview.md)

##### Remarks

The view provided in a full-screen overlay is a dimming frame with the content view inside.

-----

### Methods

#### CreateView()

Creates the content view that will be displayed as an overlay.

```cs
protected virtual StardewUI.IView CreateView();
```

##### Returns

[IView](../iview.md)

-----

#### OnClose()

Runs when the overlay is removed from the active stack.

```cs
public void OnClose();
```

-----

#### RequireView&lt;TChild&gt;(Func&lt;TChild&gt;)

Ensures that the overlay view is created before attempting to access a child view.

```cs
protected TChild RequireView<TChild>(Func<TChild> viewSelector);
```

##### Parameters

**`viewSelector`** &nbsp; [Func&lt;TChild&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-1)  
Function to retrieve the inner view.

##### Returns

`TChild`

  The inner view.

##### Remarks

This is syntactic sugar over accessing [View](fullscreenoverlay.md#view) first to force lazy loading.

-----

#### Update(TimeSpan)

Runs on every game update tick.

```cs
public virtual void Update(System.TimeSpan elapsed);
```

##### Parameters

**`elapsed`** &nbsp; [TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan)  
The amount of real time elapsed since the last tick.

-----

### Events

#### Close

Event raised when the overlay is closed - i.e. removed from the current context stack.

```cs
public event EventHandler<System.EventArgs>? Close;
```

##### Event Type

[EventHandler](https://learn.microsoft.com/en-us/dotnet/api/system.eventhandler-1)<[EventArgs](https://learn.microsoft.com/en-us/dotnet/api/system.eventargs)>

-----

