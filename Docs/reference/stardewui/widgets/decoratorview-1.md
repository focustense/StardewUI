---
title: DecoratorView&lt;T&gt;
description: A view that owns and delegates to an inner view.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class DecoratorView&lt;T&gt;

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Widgets](index.md)  
Assembly: StardewUI.dll  

</div>

A view that owns and delegates to an inner view.

```cs
public class DecoratorView<T> : StardewUI.IView, 
    System.ComponentModel.INotifyPropertyChanged, System.IDisposable
```

### Type Parameters

**`T`**  
The specific type of view that the decorator owns.


**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ DecoratorView&lt;T&gt;

**Implements**  
[IView](../iview.md), [INotifyPropertyChanged](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged), [IDisposable](https://learn.microsoft.com/en-us/dotnet/api/system.idisposable)

## Remarks

Decorator views, while not abstract, are used as a base type for other composite views, and primarily intended for framework use. Custom widgets should normally use [ComponentView&lt;T&gt;](componentview-1.md) instead, which incorporates lazy loading and other conveniences for minimalistic implementations. 

 The inner view is considered to be owned by the decorator; it will be assigned any values that were assigned to the decorator itself, such as [Layout](../iview.md#layout), and if it implements [IDisposable](https://learn.microsoft.com/en-us/dotnet/api/system.idisposable), then it will be disposed along with the decorator.

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [DecoratorView&lt;T&gt;()](#decoratorviewt) | Initializes a new [DecoratorView&lt;T&gt;](decoratorview-1.md) instance. | 

### Properties

 | Name | Description |
| --- | --- |
| [ActualBounds](#actualbounds) | The bounds of this view relative to the origin (0, 0). | 
| [ContentBounds](#contentbounds) | The true bounds of this view's content; i.e. [ActualBounds](../iview.md#actualbounds) excluding margins. | 
| [FloatingBounds](#floatingbounds) | Contains the bounds of all floating elements in this view tree, including the current view and all descendants. | 
| [IsFocusable](#isfocusable) | Whether or not the view can receive controller focus, i.e. the stick/d-pad controlled cursor can move to this view. Not generally applicable for mouse controls. | 
| [Layout](#layout) | The current layout parameters, which determine how [Measure(Vector2)](../iview.md#measurevector2) will behave. | 
| [Name](#name) | Simple name for this view, used in log/debug output; does not affect behavior. | 
| [OuterSize](#outersize) | The true computed layout size resulting from a single [Measure(Vector2)](../iview.md#measurevector2) pass. | 
| [PointerEventsEnabled](#pointereventsenabled) | Whether this view should receive pointer events like [Click](../iview.md#click) or [Drag](../iview.md#drag). | 
| [ScrollWithChildren](#scrollwithchildren) | If set to an axis, specifies that when any child of the view is scrolled into view (using [ScrollIntoView(IEnumerable&lt;ViewChild&gt;, Vector2)](../iview.md#scrollintoviewienumerableviewchild-vector2)), then this entire view should be scrolled along with it. | 
| [Tags](#tags) | The user-defined tags for this view. | 
| [Tooltip](#tooltip) | Localized tooltip to display on hover, if any. | 
| [View](#view) | The inner view that is decorated by this view. | 
| [Visibility](#visibility) | Drawing visibility for this view. | 
| [ZIndex](#zindex) | Z order for this view within its direct parent. Higher indices draw later (on top). | 

### Methods

 | Name | Description |
| --- | --- |
| [ContainsPoint(Vector2)](#containspointvector2) | Checks if a given point, relative to the view's origin, is within its bounds. | 
| [Dispose()](#dispose) |  | 
| [Draw(ISpriteBatch)](#drawispritebatch) | Draws the content for this view. | 
| [FocusSearch(Vector2, Direction)](#focussearchvector2-direction) | Finds the next focusable component in a given direction that does _not_ overlap with a current position. | 
| [GetChildAt(Vector2)](#getchildatvector2) | Finds the child at a given position. | 
| [GetChildPosition(IView)](#getchildpositioniview) | Computes or retrieves the position of a given direct child. | 
| [GetChildren()](#getchildren) | Gets the current children of this view. | 
| [GetChildrenAt(Vector2)](#getchildrenatvector2) | Finds all children at a given position. | 
| [GetDefaultFocusChild()](#getdefaultfocuschild) | Gets the direct child that should contain cursor focus when a menu or overlay containing this view is first opened. | 
| [HasOutOfBoundsContent()](#hasoutofboundscontent) | Checks if the view has content or elements that are all or partially outside the [ActualBounds](../iview.md#actualbounds). | 
| [IsDirty()](#isdirty) | Checks whether or not the view is dirty - i.e. requires a new layout with a full [Measure(Vector2)](../iview.md#measurevector2). | 
| [Measure(Vector2)](#measurevector2) | Performs layout on this view, updating its [OuterSize](../iview.md#outersize), [ActualBounds](../iview.md#actualbounds) and [ContentBounds](../iview.md#contentbounds), and arranging any children in their respective positions. | 
| [OnButtonPress(ButtonEventArgs)](#onbuttonpressbuttoneventargs) | Called when a button press is received while this view is in the focus path. | 
| [OnClick(ClickEventArgs)](#onclickclickeventargs) | Called when a click is received within this view's bounds. | 
| [OnDrag(PointerEventArgs)](#ondragpointereventargs) | Called when the view is being dragged (mouse moved while left button held). | 
| [OnDrop(PointerEventArgs)](#ondroppointereventargs) | Called when the mouse button is released after at least one [OnDrag(PointerEventArgs)](../iview.md#ondragpointereventargs). | 
| [OnLayout()](#onlayout) | Runs whenever layout occurs as a result of the UI elements changing. | 
| [OnPointerMove(PointerMoveEventArgs)](#onpointermovepointermoveeventargs) | Called when a pointer movement related to this view occurs. | 
| [OnPropertyChanged(PropertyChangedEventArgs)](#onpropertychangedpropertychangedeventargs) | Raises the [PropertyChanged](decoratorview-1.md#propertychanged) event. | 
| [OnPropertyChanged(string)](#onpropertychangedstring) | Raises the [PropertyChanged](decoratorview-1.md#propertychanged) event. | 
| [OnUpdate(TimeSpan)](#onupdatetimespan) | Runs on every update tick. | 
| [OnWheel(WheelEventArgs)](#onwheelwheeleventargs) | Called when a wheel event is received within this view's bounds. | 
| [RegisterDecoratedProperty&lt;TValue&gt;(DecoratedProperty&lt;T, TValue&gt;)](#registerdecoratedpropertytvaluedecoratedpropertyt-tvalue) | Registers a [DecoratedProperty&lt;T, TValue&gt;](decoratorview-1.decoratedproperty-1.md). | 
| [ScrollIntoView(IEnumerable&lt;ViewChild&gt;, Vector2)](#scrollintoviewienumerableviewchild-vector2) | Attempts to scroll the specified target into view, including all of its ancestors, if not fully in view. | 

### Events

 | Name | Description |
| --- | --- |
| [ButtonPress](#buttonpress) | Event raised when any button on any input device is pressed. | 
| [Click](#click) | Event raised when the view receives a click initiated from any button. | 
| [Drag](#drag) | Event raised when the view is being dragged using the mouse. | 
| [DragEnd](#dragend) | Event raised when mouse dragging is stopped, i.e. when the button is released. Always raised after the last [Drag](../iview.md#drag), and only once per drag operation. | 
| [DragStart](#dragstart) | Event raised when mouse dragging is first activated. Always raised before the first [Drag](../iview.md#drag), and only once per drag operation. | 
| [LeftClick](#leftclick) | Event raised when the view receives a click initiated from the left mouse button, or the controller's action button (A). | 
| [PointerEnter](#pointerenter) | Event raised when the pointer enters the view. | 
| [PointerLeave](#pointerleave) | Event raised when the pointer exits the view. | 
| [PropertyChanged](#propertychanged) |  | 
| [RightClick](#rightclick) | Event raised when the view receives a click initiated from the right mouse button, or the controller's tool-use button (X). | 
| [Wheel](#wheel) | Event raised when the scroll wheel moves. | 

## Details

### Constructors

#### DecoratorView&lt;T&gt;()

Initializes a new [DecoratorView&lt;T&gt;](decoratorview-1.md) instance.

```cs
public DecoratorView<T>();
```

-----

### Properties

#### ActualBounds

The bounds of this view relative to the origin (0, 0).

```cs
public StardewUI.Layout.Bounds ActualBounds { get; }
```

##### Property Value

[Bounds](../layout/bounds.md)

##### Remarks

Typically, a view's bounds is the rectangle from (0, 0) having size of [OuterSize](../iview.md#outersize), but there may be a difference especially in the case of negative margins. The various sizes affect layout flow and can even be negative - for example, in a left-to-right layout, a view with left margin -100, right margin 20 and inner width 30 (no padding) has an X size of -50, indicating that it actually (correctly) causes adjacent views to be pulled left along with it. However, `ActualBounds` always has a positive [Size](../layout/bounds.md#size), and if an implicit content offset is being applied (e.g. because of negative margins) then it will be reflected in [Position](../layout/bounds.md#position) and not affect the [Size](../layout/bounds.md#size); the previous example would have position X = -100 and size X = 50 (30 content + 20 right margin). 

 In terms of usage, [OuterSize](../iview.md#outersize) is generally used for the layout itself ([Measure(Vector2)](../iview.md#measurevector2) and [OnMeasure(Vector2)](../view.md#onmeasurevector2) of parent views) whereas [ActualBounds](../iview.md#actualbounds) is preferred for click and focus targeting.

-----

#### ContentBounds

The true bounds of this view's content; i.e. [ActualBounds](../iview.md#actualbounds) excluding margins.

```cs
public StardewUI.Layout.Bounds ContentBounds { get; }
```

##### Property Value

[Bounds](../layout/bounds.md)

-----

#### FloatingBounds

Contains the bounds of all floating elements in this view tree, including the current view and all descendants.

```cs
public System.Collections.Generic.IEnumerable<StardewUI.Layout.Bounds> FloatingBounds { get; }
```

##### Property Value

[IEnumerable](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)<[Bounds](../layout/bounds.md)>

-----

#### IsFocusable

Whether or not the view can receive controller focus, i.e. the stick/d-pad controlled cursor can move to this view. Not generally applicable for mouse controls.

```cs
public bool IsFocusable { get; }
```

##### Property Value

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

##### Remarks

In other game UI code this is more typically referred to as "snap", since there is no true input focus. However, focus is the more general term and better explains what is happening with e.g. a text box.

-----

#### Layout

The current layout parameters, which determine how [Measure(Vector2)](../iview.md#measurevector2) will behave.

```cs
public StardewUI.Layout.LayoutParameters Layout { get; set; }
```

##### Property Value

[LayoutParameters](../layout/layoutparameters.md)

-----

#### Name

Simple name for this view, used in log/debug output; does not affect behavior.

```cs
public string Name { get; set; }
```

##### Property Value

[string](https://learn.microsoft.com/en-us/dotnet/api/system.string)

-----

#### OuterSize

The true computed layout size resulting from a single [Measure(Vector2)](../iview.md#measurevector2) pass.

```cs
public Microsoft.Xna.Framework.Vector2 OuterSize { get; }
```

##### Property Value

[Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)

-----

#### PointerEventsEnabled

Whether this view should receive pointer events like [Click](../iview.md#click) or [Drag](../iview.md#drag).

```cs
public bool PointerEventsEnabled { get; set; }
```

##### Property Value

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

##### Remarks

By default, all views receive pointer events; this may be disabled for views that intentionally overlap other views but shouldn't block their input, such as local non-modal overlays.

-----

#### ScrollWithChildren

If set to an axis, specifies that when any child of the view is scrolled into view (using [ScrollIntoView(IEnumerable&lt;ViewChild&gt;, Vector2)](../iview.md#scrollintoviewienumerableviewchild-vector2)), then this entire view should be scrolled along with it.

```cs
public StardewUI.Layout.Orientation? ScrollWithChildren { get; set; }
```

##### Property Value

[Nullable](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1)<[Orientation](../layout/orientation.md)>

##### Remarks

For example, if the view lays out children horizontally, and some children may occupy only a very small amount of space near the top while others are much taller vertically or positioned near the bottom, it might be desirable to configure this with [Vertical](../layout/orientation.md#vertical), so that the entire "row" is positioned within the scrollable viewport. 

 In other words, "if any part of me is made visible via scrolling, then all of me should be visible".

-----

#### Tags

The user-defined tags for this view.

```cs
public StardewUI.Tags Tags { get; }
```

##### Property Value

[Tags](../tags.md)

-----

#### Tooltip

Localized tooltip to display on hover, if any.

```cs
public string Tooltip { get; set; }
```

##### Property Value

[string](https://learn.microsoft.com/en-us/dotnet/api/system.string)

-----

#### View

The inner view that is decorated by this view.

```cs
protected T View { get; set; }
```

##### Property Value

`T`

-----

#### Visibility

Drawing visibility for this view.

```cs
public StardewUI.Layout.Visibility Visibility { get; set; }
```

##### Property Value

[Visibility](../layout/visibility.md)

-----

#### ZIndex

Z order for this view within its direct parent. Higher indices draw later (on top).

```cs
public int ZIndex { get; set; }
```

##### Property Value

[Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)

-----

### Methods

#### ContainsPoint(Vector2)

Checks if a given point, relative to the view's origin, is within its bounds.

```cs
public virtual bool ContainsPoint(Microsoft.Xna.Framework.Vector2 point);
```

##### Parameters

**`point`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
The point to test.

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  `true` if `point` is within the view bounds; otherwise `false`.

-----

#### Dispose()



```cs
public virtual void Dispose();
```

-----

#### Draw(ISpriteBatch)

Draws the content for this view.

```cs
public virtual void Draw(StardewUI.Graphics.ISpriteBatch b);
```

##### Parameters

**`b`** &nbsp; [ISpriteBatch](../graphics/ispritebatch.md)  
Sprite batch to hold the drawing output.

##### Remarks

No positional argument is included because [ISpriteBatch](../graphics/ispritebatch.md) handles its own transformations; the top-left coordinates of this view are always (0, 0).

-----

#### FocusSearch(Vector2, Direction)

Finds the next focusable component in a given direction that does _not_ overlap with a current position.

```cs
public virtual StardewUI.Input.FocusSearchResult FocusSearch(Microsoft.Xna.Framework.Vector2 position, StardewUI.Direction direction);
```

##### Parameters

**`position`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
The current cursor position, relative to this view. May have dimensions that are negative or outside the view bounds, indicating that the cursor is not currently within the view.

**`direction`** &nbsp; [Direction](../direction.md)  
The direction of cursor movement.

##### Returns

[FocusSearchResult](../input/focussearchresult.md)

  The next focusable view reached by moving in the specified `direction`, or `null` if there are no focusable descendants that are possible to reach in that direction.

##### Remarks

If `position` is out of bounds, it does not necessarily mean that the view should return `null`; the expected result depends on the `direction` also. The base case is when the focus position is already in bounds, and in this case a view should return whichever view can be reached by moving from the edge of that view along a straight line in the specified `direction`. However, focus search is recursive and the result should reflect the "best" candidate for focus if the cursor were to move _into_ this view's bounds. For example, in a 1D horizontal layout the rules might be: 

  - If the `direction` is [East](../direction.md#east), and the position's X value is negative, then the result should the leftmost focusable child, regardless of Y value.
  - If the direction is [South](../direction.md#south), and the X position is within the view's horizontal bounds, and the Y value is negative or greater than the view's height, then result should be whichever child intersects with that X position.
  - If the direction is [West](../direction.md#west) and the X position is negative, or the direction is [East](../direction.md#east) and the X position is greater than the view's width, then the result should be `null` as there is literally nothing the view knows about in that direction.

 There are no strict rules for how a view performs focus search, but in general it is assumed that a view implementation understands its own layout and can accommodate accordingly; for example, a grid would follow essentially the same rules as our "list" example above, with additional considerations for navigating rows. "Ragged" 2D layouts might have complex rules requiring explicit neighbors, and therefore are typically easier to implement as nested lanes.

-----

#### GetChildAt(Vector2)

Finds the child at a given position.

```cs
public virtual StardewUI.ViewChild GetChildAt(Microsoft.Xna.Framework.Vector2 position);
```

##### Parameters

**`position`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
The search position, relative to the view's top-left coordinate.

##### Returns

[ViewChild](../viewchild.md)

  The view at `position`, or `null` if there is no match.

##### Remarks

If multiple children overlap the same position, then this returns the topmost child.

-----

#### GetChildPosition(IView)

Computes or retrieves the position of a given direct child.

```cs
public virtual Microsoft.Xna.Framework.Vector2? GetChildPosition(StardewUI.IView childView);
```

##### Parameters

**`childView`** &nbsp; [IView](../iview.md)  
The child of this view.

##### Returns

[Nullable](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1)<[Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)>

  The local coordinates of the `childView`, or `null` if the `childView` is not a current or direct child.

##### Remarks

Implementation of this may be O(N) and therefore it should not be called every frame; it is intended for use in directional movement and other user-initiated events.

-----

#### GetChildren()

Gets the current children of this view.

```cs
public virtual System.Collections.Generic.IEnumerable<StardewUI.ViewChild> GetChildren();
```

##### Returns

[IEnumerable](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)<[ViewChild](../viewchild.md)>

-----

#### GetChildrenAt(Vector2)

Finds all children at a given position.

```cs
public virtual System.Collections.Generic.IEnumerable<StardewUI.ViewChild> GetChildrenAt(Microsoft.Xna.Framework.Vector2 position);
```

##### Parameters

**`position`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
The search position, relative to the view's top-left coordinate.

##### Returns

[IEnumerable](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)<[ViewChild](../viewchild.md)>

  A sequence of views at the specified `position`, in front-to-back (reverse [ZIndex](../iview.md#zindex)) order.

-----

#### GetDefaultFocusChild()

Gets the direct child that should contain cursor focus when a menu or overlay containing this view is first opened.

```cs
public virtual StardewUI.ViewChild GetDefaultFocusChild();
```

##### Returns

[ViewChild](../viewchild.md)

  The child view and position where initial focus should be, either directly or indirectly as a result of a descendant receiving focus. If this [IView](../iview.md) is itself [IsFocusable](../iview.md#isfocusable), then the result may be a [ViewChild](../viewchild.md) referencing this view.

-----

#### HasOutOfBoundsContent()

Checks if the view has content or elements that are all or partially outside the [ActualBounds](../iview.md#actualbounds).

```cs
public virtual bool HasOutOfBoundsContent();
```

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

##### Remarks

This may be the case for e.g. floating elements, and covers not only the view's immediate content/children but also that of any descendants.

-----

#### IsDirty()

Checks whether or not the view is dirty - i.e. requires a new layout with a full [Measure(Vector2)](../iview.md#measurevector2).

```cs
public virtual bool IsDirty();
```

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  `true` if the view must be measured again; otherwise `false`.

##### Remarks

Typically, a view will be considered dirty if and only if one of the following are true: 

  - The [Layout](../iview.md#layout) has changed
  - The content has changed in a way that could affect layout, e.g. the text has changed in a [Content](../layout/lengthtype.md#content) configuration
  - The `availableSize` is not the same as the previously-seen value (see remarks in [Measure(Vector2)](../iview.md#measurevector2))

 A correct implementation is important for performance, as full layout can be very expensive to run on every frame.

-----

#### Measure(Vector2)

Performs layout on this view, updating its [OuterSize](../iview.md#outersize), [ActualBounds](../iview.md#actualbounds) and [ContentBounds](../iview.md#contentbounds), and arranging any children in their respective positions.

```cs
public virtual bool Measure(Microsoft.Xna.Framework.Vector2 availableSize);
```

##### Parameters

**`availableSize`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
The width/height that is still available in the container/parent.

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  Whether or not any layout was performed as a result of this pass. Callers may use this to propagate layout back up the tree, or perform expensive follow-up actions.

##### Remarks

Most views should save the value of `availableSize` for use in [IsDirty()](../iview.md#isdirty) checks.

-----

#### OnButtonPress(ButtonEventArgs)

Called when a button press is received while this view is in the focus path.

```cs
public virtual void OnButtonPress(StardewUI.Events.ButtonEventArgs e);
```

##### Parameters

**`e`** &nbsp; [ButtonEventArgs](../events/buttoneventargs.md)  
The event data.

-----

#### OnClick(ClickEventArgs)

Called when a click is received within this view's bounds.

```cs
public virtual void OnClick(StardewUI.Events.ClickEventArgs e);
```

##### Parameters

**`e`** &nbsp; [ClickEventArgs](../events/clickeventargs.md)  
The event data.

-----

#### OnDrag(PointerEventArgs)

Called when the view is being dragged (mouse moved while left button held).

```cs
public virtual void OnDrag(StardewUI.Events.PointerEventArgs e);
```

##### Parameters

**`e`** &nbsp; [PointerEventArgs](../events/pointereventargs.md)  
The event data.

-----

#### OnDrop(PointerEventArgs)

Called when the mouse button is released after at least one [OnDrag(PointerEventArgs)](../iview.md#ondragpointereventargs).

```cs
public virtual void OnDrop(StardewUI.Events.PointerEventArgs e);
```

##### Parameters

**`e`** &nbsp; [PointerEventArgs](../events/pointereventargs.md)  
The event data.

-----

#### OnLayout()

Runs whenever layout occurs as a result of the UI elements changing.

```cs
protected virtual void OnLayout();
```

-----

#### OnPointerMove(PointerMoveEventArgs)

Called when a pointer movement related to this view occurs.

```cs
public virtual void OnPointerMove(StardewUI.Events.PointerMoveEventArgs e);
```

##### Parameters

**`e`** &nbsp; [PointerMoveEventArgs](../events/pointermoveeventargs.md)  
The event data.

##### Remarks

This can either be the pointer entering the view, leaving the view, or moving within the view. The method is used to trigger events such as [PointerEnter](../view.md#pointerenter) and [PointerLeave](../view.md#pointerleave).

-----

#### OnPropertyChanged(PropertyChangedEventArgs)

Raises the [PropertyChanged](decoratorview-1.md#propertychanged) event.

```cs
protected virtual void OnPropertyChanged(System.ComponentModel.PropertyChangedEventArgs args);
```

##### Parameters

**`args`** &nbsp; [PropertyChangedEventArgs](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.propertychangedeventargs)  
The event arguments.

-----

#### OnPropertyChanged(string)

Raises the [PropertyChanged](decoratorview-1.md#propertychanged) event.

```cs
protected virtual void OnPropertyChanged(string propertyName);
```

##### Parameters

**`propertyName`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The name of the property that was changed.

-----

#### OnUpdate(TimeSpan)

Runs on every update tick.

```cs
public virtual void OnUpdate(System.TimeSpan elapsed);
```

##### Parameters

**`elapsed`** &nbsp; [TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan)  
Time elapsed since last game tick.

##### Remarks

Provided as an escape hatch for very unusual scenarios like responding to flips in the game's gamepadControls state. 

**Override this at your own extreme peril.** Frequently performing any layout-affecting logic in this function can negate the performance benefits of a retained-mode UI and cause the UI to become sluggish or even completely unresponsive.  Do not use it for animation; use [Animator](../animation/animator.md) instead.

-----

#### OnWheel(WheelEventArgs)

Called when a wheel event is received within this view's bounds.

```cs
public virtual void OnWheel(StardewUI.Events.WheelEventArgs e);
```

##### Parameters

**`e`** &nbsp; [WheelEventArgs](../events/wheeleventargs.md)  
The event data.

-----

#### RegisterDecoratedProperty&lt;TValue&gt;(DecoratedProperty&lt;T, TValue&gt;)

Registers a [DecoratedProperty&lt;T, TValue&gt;](decoratorview-1.decoratedproperty-1.md).

```cs
protected void RegisterDecoratedProperty<TValue>(StardewUI.Widgets.DecoratorView<T>.DecoratedProperty<T, TValue> property);
```

##### Parameters

**`property`** &nbsp; [DecoratedProperty&lt;T, TValue&gt;](decoratorview-1.decoratedproperty-1.md)  
The property to register.

##### Remarks

All [DecoratedProperty&lt;T, TValue&gt;](decoratorview-1.decoratedproperty-1.md) fields must be registered in the constructor or before they are read from or written to.

-----

#### ScrollIntoView(IEnumerable&lt;ViewChild&gt;, Vector2)

Attempts to scroll the specified target into view, including all of its ancestors, if not fully in view.

```cs
public virtual bool ScrollIntoView(System.Collections.Generic.IEnumerable<StardewUI.ViewChild> path, out Microsoft.Xna.Framework.Vector2 distance);
```

##### Parameters

**`path`** &nbsp; [IEnumerable](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)<[ViewChild](../viewchild.md)>  
The path to the view that should be visible, starting from (and not including) this view; each element has the local position within its own parent, so the algorithm can run recursively. This is a slice of the same path returned in a [FocusSearchResult](../input/focussearchresult.md).

**`distance`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
The total distance that was scrolled, including distance scrolled by descendants.

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  Whether or not the scroll was successful; `false` prevents the request from bubbling.

-----

### Events

#### ButtonPress

Event raised when any button on any input device is pressed.

```cs
public event EventHandler<StardewUI.Events.ButtonEventArgs>? ButtonPress;
```

##### Event Type

[EventHandler](https://learn.microsoft.com/en-us/dotnet/api/system.eventhandler-1)<[ButtonEventArgs](../events/buttoneventargs.md)>

##### Remarks

Only the views in the current focus path should receive these events.

-----

#### Click

Event raised when the view receives a click initiated from any button.

```cs
public event EventHandler<StardewUI.Events.ClickEventArgs>? Click;
```

##### Event Type

[EventHandler](https://learn.microsoft.com/en-us/dotnet/api/system.eventhandler-1)<[ClickEventArgs](../events/clickeventargs.md)>

-----

#### Drag

Event raised when the view is being dragged using the mouse.

```cs
public event EventHandler<StardewUI.Events.PointerEventArgs>? Drag;
```

##### Event Type

[EventHandler](https://learn.microsoft.com/en-us/dotnet/api/system.eventhandler-1)<[PointerEventArgs](../events/pointereventargs.md)>

-----

#### DragEnd

Event raised when mouse dragging is stopped, i.e. when the button is released. Always raised after the last [Drag](../iview.md#drag), and only once per drag operation.

```cs
public event EventHandler<StardewUI.Events.PointerEventArgs>? DragEnd;
```

##### Event Type

[EventHandler](https://learn.microsoft.com/en-us/dotnet/api/system.eventhandler-1)<[PointerEventArgs](../events/pointereventargs.md)>

-----

#### DragStart

Event raised when mouse dragging is first activated. Always raised before the first [Drag](../iview.md#drag), and only once per drag operation.

```cs
public event EventHandler<StardewUI.Events.PointerEventArgs>? DragStart;
```

##### Event Type

[EventHandler](https://learn.microsoft.com/en-us/dotnet/api/system.eventhandler-1)<[PointerEventArgs](../events/pointereventargs.md)>

-----

#### LeftClick

Event raised when the view receives a click initiated from the left mouse button, or the controller's action button (A).

```cs
public event EventHandler<StardewUI.Events.ClickEventArgs>? LeftClick;
```

##### Event Type

[EventHandler](https://learn.microsoft.com/en-us/dotnet/api/system.eventhandler-1)<[ClickEventArgs](../events/clickeventargs.md)>

##### Remarks

Using this event is a shortcut for handling [Click](../iview.md#click) and checking for [IsPrimaryButton()](../events/clickeventargs.md#isprimarybutton).

-----

#### PointerEnter

Event raised when the pointer enters the view.

```cs
public event EventHandler<StardewUI.Events.PointerEventArgs>? PointerEnter;
```

##### Event Type

[EventHandler](https://learn.microsoft.com/en-us/dotnet/api/system.eventhandler-1)<[PointerEventArgs](../events/pointereventargs.md)>

-----

#### PointerLeave

Event raised when the pointer exits the view.

```cs
public event EventHandler<StardewUI.Events.PointerEventArgs>? PointerLeave;
```

##### Event Type

[EventHandler](https://learn.microsoft.com/en-us/dotnet/api/system.eventhandler-1)<[PointerEventArgs](../events/pointereventargs.md)>

-----

#### PropertyChanged



```cs
public event System.ComponentModel.PropertyChangedEventHandler? PropertyChanged;
```

##### Event Type

[PropertyChangedEventHandler](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.propertychangedeventhandler)

-----

#### RightClick

Event raised when the view receives a click initiated from the right mouse button, or the controller's tool-use button (X).

```cs
public event EventHandler<StardewUI.Events.ClickEventArgs>? RightClick;
```

##### Event Type

[EventHandler](https://learn.microsoft.com/en-us/dotnet/api/system.eventhandler-1)<[ClickEventArgs](../events/clickeventargs.md)>

##### Remarks

Using this event is a shortcut for handling [Click](../iview.md#click) and checking for [IsSecondaryButton()](../events/clickeventargs.md#issecondarybutton).

-----

#### Wheel

Event raised when the scroll wheel moves.

```cs
public event EventHandler<StardewUI.Events.WheelEventArgs>? Wheel;
```

##### Event Type

[EventHandler](https://learn.microsoft.com/en-us/dotnet/api/system.eventhandler-1)<[WheelEventArgs](../events/wheeleventargs.md)>

-----

