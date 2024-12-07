---
title: IOverlay
description: Definition of an overlay - i.e. a UI element that displays over all other UI.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Interface IOverlay

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Overlays](index.md)  
Assembly: StardewUI.dll  

</div>

Definition of an overlay - i.e. a UI element that displays over all other UI.

```cs
public interface IOverlay
```

## Members

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
| [OnClose()](#onclose) | Runs when the overlay is removed from the active stack. | 
| [Update(TimeSpan)](#updatetimespan) | Runs on every game update tick. | 

### Events

 | Name | Description |
| --- | --- |
| [Close](#close) | Event raised when the overlay is closed - i.e. removed from the current context stack. | 

## Details

### Properties

#### CapturingInput

Whether the overlay wants to capture all keyboard and gamepad inputs, i.e. prevent them from being dispatched to the parent menu.

```cs
bool CapturingInput { get; }
```

##### Property Value

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

##### Remarks

This is not necessary to trap focus, which happens automatically; only to capture buttons/keys that would normally have a navigation function, like triggers/shoulders for paging, E/Esc/GamepadB for cancellation, etc. Overlays that enable capturing should provide their own way for the user to escape using keyboard/gamepad, although it is always possible to click the mouse outside the overlay to dismiss it (and implicitly stop the capturing).

-----

#### DimmingAmount

Amount to dim whatever is underneath the overlay.

```cs
float DimmingAmount { get; }
```

##### Property Value

[Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)

##### Remarks

This is an alpha value for a black overlay, so the higher value (between 0 and 1) the darker the content underneath the overlay. These apply individually to each overlay, so multiple stacked overlays will dim not only the underlying main view but also any previous overlays.

-----

#### HorizontalAlignment

Horizontal alignment of the overlay relative to the [Parent](ioverlay.md#parent) edge.

```cs
StardewUI.Layout.Alignment HorizontalAlignment { get; }
```

##### Property Value

[Alignment](../layout/alignment.md)

##### Remarks

Specifies which edge of the overlay is used for positioning, regardless of which parent edge it is aligning to. For example, a [HorizontalAlignment](ioverlay.md#horizontalalignment) of [Start](../layout/alignment.md#start) and a [HorizontalParentAlignment](ioverlay.md#horizontalparentalignment) of [End](../layout/alignment.md#end) means that the overlay's left edge will be aligned to the parent's right edge; similarly, if both are set to [Start](../layout/alignment.md#start), then the overlay's left edge is aligned to the parent's _left_ edge.

-----

#### HorizontalParentAlignment

Specifies which edge of the [Parent](ioverlay.md#parent) (or screen, if no parent is specified) will be used to align the overlay edge denoted by its [HorizontalAlignment](ioverlay.md#horizontalalignment).

```cs
StardewUI.Layout.Alignment HorizontalParentAlignment { get; }
```

##### Property Value

[Alignment](../layout/alignment.md)

##### Remarks

For example, a [HorizontalAlignment](ioverlay.md#horizontalalignment) of [Start](../layout/alignment.md#start) and a [HorizontalParentAlignment](ioverlay.md#horizontalparentalignment) of [End](../layout/alignment.md#end) means that the overlay's left edge will be aligned to the parent's right edge; similarly, if both are set to [Start](../layout/alignment.md#start), then the overlay's left edge is aligned to the parent's _left_ edge.

-----

#### Parent

The parent of this overlay, used for positioning. If not specified, then the overlay will be positioned relative to the entire UI viewport.

```cs
StardewUI.IView Parent { get; }
```

##### Property Value

[IView](../iview.md)

-----

#### ParentOffset

Additional pixel offset to apply to the overlay's position, after alignments.

```cs
Microsoft.Xna.Framework.Vector2 ParentOffset { get; }
```

##### Property Value

[Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)

-----

#### VerticalAlignment

Vertical alignment of the overlay relative to the [Parent](ioverlay.md#parent) edge.

```cs
StardewUI.Layout.Alignment VerticalAlignment { get; }
```

##### Property Value

[Alignment](../layout/alignment.md)

##### Remarks

Specifies which edge of the overlay is used for positioning, regardless of which parent edge it is aligning to. For example, a [VerticalAlignment](ioverlay.md#verticalalignment) of [Start](../layout/alignment.md#start) and a [VerticalParentAlignment](ioverlay.md#verticalparentalignment) of [End](../layout/alignment.md#end) means that the overlay's top edge will be aligned to the parent's bottom edge; similarly, if both are set to [Start](../layout/alignment.md#start), then the overlay's top edge is aligned to the parent's _top_ edge.

-----

#### VerticalParentAlignment

Specifies which edge of the [Parent](ioverlay.md#parent) (or screen, if no parent is specified) will be used to align the overlay edge denoted by its [VerticalAlignment](ioverlay.md#verticalalignment).

```cs
StardewUI.Layout.Alignment VerticalParentAlignment { get; }
```

##### Property Value

[Alignment](../layout/alignment.md)

##### Remarks

For example, a [VerticalAlignment](ioverlay.md#verticalalignment) of [Start](../layout/alignment.md#start) and a [VerticalParentAlignment](ioverlay.md#verticalparentalignment) of [End](../layout/alignment.md#end) means that the overlay's top edge will be aligned to the parent's bottom edge; similarly, if both are set to [Start](../layout/alignment.md#start), then the overlay's top edge is aligned to the parent's _top_ edge.

-----

#### View

The view to be displayed/interacted with as an overlay.

```cs
StardewUI.IView View { get; }
```

##### Property Value

[IView](../iview.md)

-----

### Methods

#### OnClose()

Runs when the overlay is removed from the active stack.

```cs
void OnClose();
```

-----

#### Update(TimeSpan)

Runs on every game update tick.

```cs
void Update(System.TimeSpan elapsed);
```

##### Parameters

**`elapsed`** &nbsp; [TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan)  
The amount of real time elapsed since the last tick.

-----

### Events

#### Close

Event raised when the overlay is closed - i.e. removed from the current context stack.

```cs
event EventHandler<System.EventArgs>? Close;
```

##### Event Type

[EventHandler](https://learn.microsoft.com/en-us/dotnet/api/system.eventhandler-1)<[EventArgs](https://learn.microsoft.com/en-us/dotnet/api/system.eventargs)>

-----

