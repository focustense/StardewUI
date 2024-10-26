---
title: Overlay
description: A basic overlay with immutable properties.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class Overlay

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Overlays](index.md)  
Assembly: StardewUI.dll  

</div>

A basic overlay with immutable properties.

```cs
public class Overlay : StardewUI.Overlays.IOverlay
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ Overlay

**Implements**  
[IOverlay](ioverlay.md)

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [Overlay(IView, IView, Alignment, Alignment, Alignment, Alignment, Vector2)](#overlayiview-iview-alignment-alignment-alignment-alignment-vector2) | A basic overlay with immutable properties. | 

### Properties

 | Name | Description |
| --- | --- |
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
| [OnClose(Action)](#oncloseaction) | Registers an action to be run when the overlay is closed. | 
| [Pop()](#pop) | Removes the front-most overlay. | 
| [Push(IOverlay)](#pushioverlay) |  | 
| [Remove(IOverlay)](#removeioverlay) |  | 

### Events

 | Name | Description |
| --- | --- |
| [Close](#close) | Raised when the overlay is removed from the active stack. | 

## Details

### Constructors

#### Overlay(IView, IView, Alignment, Alignment, Alignment, Alignment, Vector2)

A basic overlay with immutable properties.

```cs
public Overlay(StardewUI.IView view, StardewUI.IView parent, StardewUI.Layout.Alignment horizontalAlignment, StardewUI.Layout.Alignment horizontalParentAlignment, StardewUI.Layout.Alignment verticalAlignment, StardewUI.Layout.Alignment verticalParentAlignment, Microsoft.Xna.Framework.Vector2 parentOffset);
```

##### Parameters

**`view`** &nbsp; [IView](../iview.md)  
The [View](ioverlay.md#view).

**`parent`** &nbsp; [IView](../iview.md)  
The [Parent](ioverlay.md#parent).

**`horizontalAlignment`** &nbsp; [Alignment](../layout/alignment.md)  
The [HorizontalAlignment](ioverlay.md#horizontalalignment).

**`horizontalParentAlignment`** &nbsp; [Alignment](../layout/alignment.md)  
The [HorizontalParentAlignment](ioverlay.md#horizontalparentalignment).

**`verticalAlignment`** &nbsp; [Alignment](../layout/alignment.md)  
The [VerticalAlignment](ioverlay.md#verticalalignment).

**`verticalParentAlignment`** &nbsp; [Alignment](../layout/alignment.md)  
The [VerticalParentAlignment](ioverlay.md#verticalparentalignment).

**`parentOffset`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
The [ParentOffset](ioverlay.md#parentoffset).

-----

### Properties

#### HorizontalAlignment

Horizontal alignment of the overlay relative to the [Parent](ioverlay.md#parent) edge.

```cs
public StardewUI.Layout.Alignment HorizontalAlignment { get; }
```

##### Property Value

[Alignment](../layout/alignment.md)

##### Remarks

Specifies which edge of the overlay is used for positioning, regardless of which parent edge it is aligning to. For example, a [HorizontalAlignment](ioverlay.md#horizontalalignment) of [Start](../layout/alignment.md#start) and a [HorizontalParentAlignment](ioverlay.md#horizontalparentalignment) of [End](../layout/alignment.md#end) means that the overlay's left edge will be aligned to the parent's right edge; similarly, if both are set to [Start](../layout/alignment.md#start), then the overlay's left edge is aligned to the parent's _left_ edge.

-----

#### HorizontalParentAlignment

Specifies which edge of the [Parent](ioverlay.md#parent) (or screen, if no parent is specified) will be used to align the overlay edge denoted by its [HorizontalAlignment](ioverlay.md#horizontalalignment).

```cs
public StardewUI.Layout.Alignment HorizontalParentAlignment { get; }
```

##### Property Value

[Alignment](../layout/alignment.md)

##### Remarks

For example, a [HorizontalAlignment](ioverlay.md#horizontalalignment) of [Start](../layout/alignment.md#start) and a [HorizontalParentAlignment](ioverlay.md#horizontalparentalignment) of [End](../layout/alignment.md#end) means that the overlay's left edge will be aligned to the parent's right edge; similarly, if both are set to [Start](../layout/alignment.md#start), then the overlay's left edge is aligned to the parent's _left_ edge.

-----

#### Parent

The parent of this overlay, used for positioning. If not specified, then the overlay will be positioned relative to the entire UI viewport.

```cs
public StardewUI.IView Parent { get; }
```

##### Property Value

[IView](../iview.md)

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

Specifies which edge of the overlay is used for positioning, regardless of which parent edge it is aligning to. For example, a [VerticalAlignment](ioverlay.md#verticalalignment) of [Start](../layout/alignment.md#start) and a [VerticalParentAlignment](ioverlay.md#verticalparentalignment) of [End](../layout/alignment.md#end) means that the overlay's top edge will be aligned to the parent's bottom edge; similarly, if both are set to [Start](../layout/alignment.md#start), then the overlay's top edge is aligned to the parent's _top_ edge.

-----

#### VerticalParentAlignment

Specifies which edge of the [Parent](ioverlay.md#parent) (or screen, if no parent is specified) will be used to align the overlay edge denoted by its [VerticalAlignment](ioverlay.md#verticalalignment).

```cs
public StardewUI.Layout.Alignment VerticalParentAlignment { get; }
```

##### Property Value

[Alignment](../layout/alignment.md)

##### Remarks

For example, a [VerticalAlignment](ioverlay.md#verticalalignment) of [Start](../layout/alignment.md#start) and a [VerticalParentAlignment](ioverlay.md#verticalparentalignment) of [End](../layout/alignment.md#end) means that the overlay's top edge will be aligned to the parent's bottom edge; similarly, if both are set to [Start](../layout/alignment.md#start), then the overlay's top edge is aligned to the parent's _top_ edge.

-----

#### View

The view to be displayed/interacted with as an overlay.

```cs
public StardewUI.IView View { get; }
```

##### Property Value

[IView](../iview.md)

-----

### Methods

#### OnClose()

Runs when the overlay is removed from the active stack.

```cs
public void OnClose();
```

-----

#### OnClose(Action)

Registers an action to be run when the overlay is closed.

```cs
public StardewUI.Overlays.Overlay OnClose(System.Action onClose);
```

##### Parameters

**`onClose`** &nbsp; [Action](https://learn.microsoft.com/en-us/dotnet/api/system.action)  
The action to run on close.

##### Returns

[Overlay](overlay.md)

  The current [Overlay](overlay.md) instance.

##### Remarks

Typically chained to the constructor when creating a new overlay.

-----

#### Pop()

Removes the front-most overlay.

```cs
public static StardewUI.Overlays.IOverlay Pop();
```

##### Returns

[IOverlay](ioverlay.md)

##### Remarks

Applies to the ambient [OverlayContext](overlaycontext.md), and is ignored if no context is available.

-----

#### Push(IOverlay)



```cs
public static void Push(StardewUI.Overlays.IOverlay overlay);
```

##### Parameters

**`overlay`** &nbsp; [IOverlay](ioverlay.md)

##### Remarks

Applies to the ambient [OverlayContext](overlaycontext.md), and is ignored if no context is available.

-----

#### Remove(IOverlay)



```cs
public static bool Remove(StardewUI.Overlays.IOverlay overlay);
```

##### Parameters

**`overlay`** &nbsp; [IOverlay](ioverlay.md)

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

##### Remarks

Applies to the ambient [OverlayContext](overlaycontext.md), and is ignored if no context is available.

-----

### Events

#### Close

Raised when the overlay is removed from the active stack.

```cs
public event EventHandler<System.EventArgs>? Close;
```

##### Event Type

[EventHandler](https://learn.microsoft.com/en-us/dotnet/api/system.eventhandler-1)<[EventArgs](https://learn.microsoft.com/en-us/dotnet/api/system.eventargs)>

-----

