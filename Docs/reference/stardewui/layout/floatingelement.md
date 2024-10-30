---
title: FloatingElement
description: Provides independent layout for an IView relative to its parent.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class FloatingElement

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Layout](index.md)  
Assembly: StardewUI.dll  

</div>

Provides independent layout for an [IView](../iview.md) relative to its parent.

```cs
public class FloatingElement
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ FloatingElement

## Remarks

Floating elements do not participate in the normal layout ([Measure(Vector2)](../iview.md#measurevector2)) of the view that owns them; they are excluded entirely from the flow, and then provided with their own measurement and position using the final bounds of the parent (i.e. those that result from its non-floating elements). 

 This is primarily useful for annotations, callouts, or elements that are intentionally drawn outside their logical container such as scrollbars or sidebars. Floating views **can** receive focus and clicks, but do not actually capture the cursor like an [Overlay](../overlays/overlay.md) would, and therefore shouldn't be used for modal UI. 

 In general it is preferred to use standard layout controls like [Lane](../widgets/lane.md) over floating elements, but there are specific cases that justify floats, such as the aforementioned scrollbar which should display "outside" the container regardless of how nested the container itself is - i.e. the float must "break out" of the normal flow.

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [FloatingElement(IView, FloatingPosition)](#floatingelementiview-floatingposition) | Provides independent layout for an [IView](../iview.md) relative to its parent. | 

### Properties

 | Name | Description |
| --- | --- |
| [Position](#position) | The element's defined position. | 
| [View](#view) | The view to display within this element. | 

### Methods

 | Name | Description |
| --- | --- |
| [AsViewChild()](#asviewchild) | Creates a [ViewChild](../viewchild.md) with the floating element's view and current position. | 
| [Draw(ISpriteBatch)](#drawispritebatch) | Draws the element at its current position. | 
| [MeasureAndPosition(View, Boolean)](#measureandpositionview-bool) | Measures the view's content and repositions the entire floating element if necessary. | 

## Details

### Constructors

#### FloatingElement(IView, FloatingPosition)

Provides independent layout for an [IView](../iview.md) relative to its parent.

```cs
public FloatingElement(StardewUI.IView view, StardewUI.Layout.FloatingPosition position);
```

##### Parameters

**`view`** &nbsp; [IView](../iview.md)  
The floating view to display/interact with.

**`position`** &nbsp; [FloatingPosition](floatingposition.md)  
Specifies how to position the `view` relative to the parent and its own measured size.

##### Remarks

Floating elements do not participate in the normal layout ([Measure(Vector2)](../iview.md#measurevector2)) of the view that owns them; they are excluded entirely from the flow, and then provided with their own measurement and position using the final bounds of the parent (i.e. those that result from its non-floating elements). 

 This is primarily useful for annotations, callouts, or elements that are intentionally drawn outside their logical container such as scrollbars or sidebars. Floating views **can** receive focus and clicks, but do not actually capture the cursor like an [Overlay](../overlays/overlay.md) would, and therefore shouldn't be used for modal UI. 

 In general it is preferred to use standard layout controls like [Lane](../widgets/lane.md) over floating elements, but there are specific cases that justify floats, such as the aforementioned scrollbar which should display "outside" the container regardless of how nested the container itself is - i.e. the float must "break out" of the normal flow.

-----

### Properties

#### Position

The element's defined position.

```cs
public StardewUI.Layout.FloatingPosition Position { get; }
```

##### Property Value

[FloatingPosition](floatingposition.md)

-----

#### View

The view to display within this element.

```cs
public StardewUI.IView View { get; }
```

##### Property Value

[IView](../iview.md)

-----

### Methods

#### AsViewChild()

Creates a [ViewChild](../viewchild.md) with the floating element's view and current position.

```cs
public StardewUI.ViewChild AsViewChild();
```

##### Returns

[ViewChild](../viewchild.md)

  The current element represented as a [ViewChild](../viewchild.md).

##### Remarks

The result can generally be used as if it were any other non-floating element, e.g. for dispatching clicks, focus searches and other events.

-----

#### Draw(ISpriteBatch)

Draws the element at its current position.

```cs
public void Draw(StardewUI.Graphics.ISpriteBatch spriteBatch);
```

##### Parameters

**`spriteBatch`** &nbsp; [ISpriteBatch](../graphics/ispritebatch.md)  
Sprite batch to hold the drawing output.

-----

#### MeasureAndPosition(View, bool)

Measures the view's content and repositions the entire floating element if necessary.

```cs
public void MeasureAndPosition(StardewUI.View parentView, bool wasParentDirty);
```

##### Parameters

**`parentView`** &nbsp; [View](../view.md)  
The view whose [FloatingElements](../view.md#floatingelements) this element belongs to. Required for repositioning when the layout has changed.

**`wasParentDirty`** &nbsp; [Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)  
Whether this measurement is being done because the parent's layout already changed and therefore a reposition is always required (`true`), or whether to reposition only if the floating element's internal layout has changed (`false`).

-----

