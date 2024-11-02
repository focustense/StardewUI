---
title: View
description: Base class for typical widgets wanting to implement IView.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class View

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI](index.md)  
Assembly: StardewUI.dll  

</div>

Base class for typical widgets wanting to implement [IView](iview.md).

```cs
public class View : StardewUI.IView, 
    System.ComponentModel.INotifyPropertyChanged
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ View

**Implements**  
[IView](iview.md), [INotifyPropertyChanged](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged)

## Remarks

Use of this class isn't required, but provides some useful behaviors so that view types don't need to keep re-implementing them, such as a standard [Measure(Vector2)](view.md#measurevector2) implementation that skips unnecessary layouts.

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [View()](#view) | Initializes a new instance of [View](view.md). | 

### Properties

 | Name | Description |
| --- | --- |
| [ActualBounds](#actualbounds) | The bounds of this view relative to the origin (0, 0). | 
| [BorderSize](#bordersize) | The layout size (not edge thickness) of the entire drawn area including the border, i.e. the [InnerSize](view.md#innersize) plus any borders defined in [GetBorderThickness()](view.md#getborderthickness). Does not include the [Margin](view.md#margin). | 
| [ContentBounds](#contentbounds) | The true bounds of this view's content; i.e. [ActualBounds](iview.md#actualbounds) excluding margins. | 
| [ContentSize](#contentsize) | The size of the view's content, which is drawn inside the padding. Subclasses set this in their [OnMeasure(Vector2)](view.md#onmeasurevector2) method and padding, margins, etc. are handled automatically. | 
| [Draggable](#draggable) | Whether or not this view should fire drag events such as [DragStart](view.md#dragstart) and [Drag](view.md#drag). | 
| [FloatingElements](#floatingelements) | The floating elements to display relative to this view. | 
| [Focusable](#focusable) | Whether or not the view should be able to receive focus. Applies only to this specific view, not its children. | 
| [InnerSize](#innersize) | The size allocated to the entire area inside the border, i.e. [ContentSize](view.md#contentsize) plus any [Padding](view.md#padding). Does not include border or [Margin](view.md#margin). | 
| [IsFocusable](#isfocusable) | Whether or not the view can receive controller focus, i.e. the stick/d-pad controlled cursor can move to this view. Not generally applicable for mouse controls. | 
| [LastAvailableSize](#lastavailablesize) | The most recent size used in a [Measure(Vector2)](view.md#measurevector2) pass. Used for additional dirty checks. | 
| [Layout](#layout) | Layout settings for this view; determines how its dimensions will be computed. | 
| [Margin](#margin) | Margins (whitespace outside border) for this view. | 
| [Name](#name) | Simple name for this view, used in log/debug output; does not affect behavior. | 
| [OuterSize](#outersize) | The size of the entire area occupied by this view including margins, border and padding. | 
| [Padding](#padding) | Padding (whitespace inside border) for this view. | 
| [PointerEventsEnabled](#pointereventsenabled) | Whether this view should receive pointer events like [Click](view.md#click) or [Drag](view.md#drag). | 
| [ScrollWithChildren](#scrollwithchildren) | If set to an axis, specifies that when any child of the view is scrolled into view (using [ScrollIntoView(IEnumerable&lt;ViewChild&gt;, Vector2)](view.md#scrollintoviewienumerableviewchild-vector2)), then this entire view should be scrolled along with it. | 
| [Tags](#tags) | The user-defined tags for this view. | 
| [Tooltip](#tooltip) | Localized tooltip to display on hover, if any. | 
| [Visibility](#visibility) | Visibility for this view. | 
| [ZIndex](#zindex) | Z order for this view within its direct parent. Higher indices draw later (on top). | 

### Methods

 | Name | Description |
| --- | --- |
| [ContainsPoint(Vector2)](#containspointvector2) | Checks if a given point, relative to the view's origin, is within its bounds. | 
| [Draw(ISpriteBatch)](#drawispritebatch) | Draws the content for this view. | 
| [FindFocusableDescendant(Vector2, Direction)](#findfocusabledescendantvector2-direction) | Searches for a focusable child within this view that is reachable in the specified `direction`, and returns a result containing the view and search path if found. | 
| [FocusSearch(Vector2, Direction)](#focussearchvector2-direction) | Finds the next focusable component in a given direction that does _not_ overlap with a current position. | 
| [GetBorderThickness()](#getborderthickness) | Measures the thickness of each edge of the border, if the view has a border. | 
| [GetChildAt(Vector2)](#getchildatvector2) | Finds the child at a given position. | 
| [GetChildPosition(IView)](#getchildpositioniview) | Computes or retrieves the position of a given direct child. | 
| [GetChildren()](#getchildren) | Gets the current children of this view. | 
| [GetChildrenAt(Vector2)](#getchildrenatvector2) | Finds all children at a given position. | 
| [GetDefaultFocusChild()](#getdefaultfocuschild) | Gets the direct child that should contain cursor focus when a menu or overlay containing this view is first opened. | 
| [GetLocalChildren()](#getlocalchildren) | Gets the view's children with positions relative to the content area. | 
| [GetLocalChildrenAt(Vector2)](#getlocalchildrenatvector2) | Searches for all views at a given position relative to the content area. | 
| [HasOutOfBoundsContent()](#hasoutofboundscontent) | Checks if the view has content or elements that are all or partially outside the [ActualBounds](iview.md#actualbounds). | 
| [IsContentDirty()](#iscontentdirty) | Checks whether or not the internal content/layout has changed. | 
| [IsDirty()](#isdirty) | Checks whether or not the view is dirty - i.e. requires a new layout with a full [Measure(Vector2)](iview.md#measurevector2). | 
| [LogFocusSearch(string)](#logfocussearchstring) | Outputs a debug log entry with the current view type, name and specified message. | 
| [Measure(Vector2)](#measurevector2) | Performs layout on this view, updating its [OuterSize](iview.md#outersize), [ActualBounds](iview.md#actualbounds) and [ContentBounds](iview.md#contentbounds), and arranging any children in their respective positions. | 
| [OnButtonPress(ButtonEventArgs)](#onbuttonpressbuttoneventargs) | Called when a button press is received while this view is in the focus path. | 
| [OnClick(ClickEventArgs)](#onclickclickeventargs) | Called when a click is received within this view's bounds. | 
| [OnDrag(PointerEventArgs)](#ondragpointereventargs) | Called when the view is being dragged (mouse moved while left button held). | 
| [OnDrawBorder(ISpriteBatch)](#ondrawborderispritebatch) | Draws the view's border, if it has one. | 
| [OnDrawContent(ISpriteBatch)](#ondrawcontentispritebatch) | Draws the inner content of this view. | 
| [OnDrop(PointerEventArgs)](#ondroppointereventargs) | Called when the mouse button is released after at least one [OnDrag(PointerEventArgs)](iview.md#ondragpointereventargs). | 
| [OnMeasure(Vector2)](#onmeasurevector2) | Performs the internal layout. | 
| [OnPointerMove(PointerMoveEventArgs)](#onpointermovepointermoveeventargs) | Called when a pointer movement related to this view occurs. | 
| [OnPropertyChanged(PropertyChangedEventArgs)](#onpropertychangedpropertychangedeventargs) | Raises the [PropertyChanged](view.md#propertychanged) event. | 
| [OnPropertyChanged(string)](#onpropertychangedstring) | Raises the [PropertyChanged](view.md#propertychanged) event. | 
| [OnUpdate(TimeSpan)](#onupdatetimespan) | Runs on every update tick. | 
| [OnWheel(WheelEventArgs)](#onwheelwheeleventargs) | Called when a wheel event is received within this view's bounds. | 
| [ResetDirty()](#resetdirty) | Resets any dirty state associated with this view. | 
| [ScrollIntoView(IEnumerable&lt;ViewChild&gt;, Vector2)](#scrollintoviewienumerableviewchild-vector2) | Attempts to scroll the specified target into view, including all of its ancestors, if not fully in view. | 
| [ToString()](#tostring) | <span class="muted" markdown>(Overrides [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object).`ToString()`)</span> | 

### Events

 | Name | Description |
| --- | --- |
| [ButtonPress](#buttonpress) | Event raised when any button on any input device is pressed. | 
| [Click](#click) | Event raised when the view receives a click. | 
| [Drag](#drag) | Event raised when the view is being dragged using the mouse. | 
| [DragEnd](#dragend) | Event raised when mouse dragging is stopped, i.e. when the button is released. Always raised after the last [Drag](view.md#drag), and only once per drag operation. | 
| [DragStart](#dragstart) | Event raised when mouse dragging is first activated. Always raised before the first [Drag](view.md#drag), and only once per drag operation. | 
| [LeftClick](#leftclick) | Event raised when the view receives a click initiated from the left mouse button, or the controller's action button (A). | 
| [PointerEnter](#pointerenter) | Event raised when the pointer enters the view. | 
| [PointerLeave](#pointerleave) | Event raised when the pointer exits the view. | 
| [PropertyChanged](#propertychanged) |  | 
| [RightClick](#rightclick) | Event raised when the view receives a click initiated from the right mouse button, or the controller's tool-use button (X). | 
| [Wheel](#wheel) | Event raised when the scroll wheel moves. | 

## Details

### Constructors

#### View()

Initializes a new instance of [View](view.md).

```cs
public View();
```

##### Remarks

The view's [Name](view.md#name) will default to the simple name of its most derived [Type](https://learn.microsoft.com/en-us/dotnet/api/system.type).

-----

### Properties

#### ActualBounds

The bounds of this view relative to the origin (0, 0).

```cs
public StardewUI.Layout.Bounds ActualBounds { get; }
```

##### Property Value

[Bounds](layout/bounds.md)

##### Remarks

Typically, a view's bounds is the rectangle from (0, 0) having size of [OuterSize](iview.md#outersize), but there may be a difference especially in the case of negative margins. The various sizes affect layout flow and can even be negative - for example, in a left-to-right layout, a view with left margin -100, right margin 20 and inner width 30 (no padding) has an X size of -50, indicating that it actually (correctly) causes adjacent views to be pulled left along with it. However, `ActualBounds` always has a positive [Size](layout/bounds.md#size), and if an implicit content offset is being applied (e.g. because of negative margins) then it will be reflected in [Position](layout/bounds.md#position) and not affect the [Size](layout/bounds.md#size); the previous example would have position X = -100 and size X = 50 (30 content + 20 right margin). 

 In terms of usage, [OuterSize](iview.md#outersize) is generally used for the layout itself ([Measure(Vector2)](iview.md#measurevector2) and [OnMeasure(Vector2)](view.md#onmeasurevector2) of parent views) whereas [ActualBounds](iview.md#actualbounds) is preferred for click and focus targeting.

-----

#### BorderSize

The layout size (not edge thickness) of the entire drawn area including the border, i.e. the [InnerSize](view.md#innersize) plus any borders defined in [GetBorderThickness()](view.md#getborderthickness). Does not include the [Margin](view.md#margin).

```cs
public Microsoft.Xna.Framework.Vector2 BorderSize { get; }
```

##### Property Value

[Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)

-----

#### ContentBounds

The true bounds of this view's content; i.e. [ActualBounds](iview.md#actualbounds) excluding margins.

```cs
public StardewUI.Layout.Bounds ContentBounds { get; }
```

##### Property Value

[Bounds](layout/bounds.md)

-----

#### ContentSize

The size of the view's content, which is drawn inside the padding. Subclasses set this in their [OnMeasure(Vector2)](view.md#onmeasurevector2) method and padding, margins, etc. are handled automatically.

```cs
public Microsoft.Xna.Framework.Vector2 ContentSize { get; protected set; }
```

##### Property Value

[Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)

-----

#### Draggable

Whether or not this view should fire drag events such as [DragStart](view.md#dragstart) and [Drag](view.md#drag).

```cs
public bool Draggable { get; set; }
```

##### Property Value

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

-----

#### FloatingElements

The floating elements to display relative to this view.

```cs
public System.Collections.Generic.IList<StardewUI.Layout.FloatingElement> FloatingElements { get; set; }
```

##### Property Value

[IList](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ilist-1)<[FloatingElement](layout/floatingelement.md)>

-----

#### Focusable

Whether or not the view should be able to receive focus. Applies only to this specific view, not its children.

```cs
public bool Focusable { get; set; }
```

##### Property Value

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

##### Remarks

All views are non-focusable by default and must have their focus enabled explicitly. Subclasses may choose to override the default value if they should always be focusable.

-----

#### InnerSize

The size allocated to the entire area inside the border, i.e. [ContentSize](view.md#contentsize) plus any [Padding](view.md#padding). Does not include border or [Margin](view.md#margin).

```cs
public Microsoft.Xna.Framework.Vector2 InnerSize { get; }
```

##### Property Value

[Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)

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

#### LastAvailableSize

The most recent size used in a [Measure(Vector2)](view.md#measurevector2) pass. Used for additional dirty checks.

```cs
protected Microsoft.Xna.Framework.Vector2 LastAvailableSize { get; private set; }
```

##### Property Value

[Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)

-----

#### Layout

Layout settings for this view; determines how its dimensions will be computed.

```cs
public StardewUI.Layout.LayoutParameters Layout { get; set; }
```

##### Property Value

[LayoutParameters](layout/layoutparameters.md)

-----

#### Margin

Margins (whitespace outside border) for this view.

```cs
public StardewUI.Layout.Edges Margin { get; set; }
```

##### Property Value

[Edges](layout/edges.md)

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

The size of the entire area occupied by this view including margins, border and padding.

```cs
public Microsoft.Xna.Framework.Vector2 OuterSize { get; }
```

##### Property Value

[Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)

-----

#### Padding

Padding (whitespace inside border) for this view.

```cs
public StardewUI.Layout.Edges Padding { get; set; }
```

##### Property Value

[Edges](layout/edges.md)

-----

#### PointerEventsEnabled

Whether this view should receive pointer events like [Click](view.md#click) or [Drag](view.md#drag).

```cs
public bool PointerEventsEnabled { get; set; }
```

##### Property Value

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

##### Remarks

By default, all views receive pointer events; this may be disabled for views that intentionally overlap other views but shouldn't block their input, such as local non-modal overlays.

-----

#### ScrollWithChildren

If set to an axis, specifies that when any child of the view is scrolled into view (using [ScrollIntoView(IEnumerable&lt;ViewChild&gt;, Vector2)](view.md#scrollintoviewienumerableviewchild-vector2)), then this entire view should be scrolled along with it.

```cs
public StardewUI.Layout.Orientation? ScrollWithChildren { get; set; }
```

##### Property Value

[Nullable](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1)<[Orientation](layout/orientation.md)>

##### Remarks

For example, if the view lays out children horizontally, and some children may occupy only a very small amount of space near the top while others are much taller vertically or positioned near the bottom, it might be desirable to configure this with [Vertical](layout/orientation.md#vertical), so that the entire "row" is positioned within the scrollable viewport. 

 In other words, "if any part of me is made visible via scrolling, then all of me should be visible".

-----

#### Tags

The user-defined tags for this view.

```cs
public StardewUI.Tags Tags { get; set; }
```

##### Property Value

[Tags](tags.md)

-----

#### Tooltip

Localized tooltip to display on hover, if any.

```cs
public string Tooltip { get; set; }
```

##### Property Value

[string](https://learn.microsoft.com/en-us/dotnet/api/system.string)

-----

#### Visibility

Visibility for this view.

```cs
public StardewUI.Layout.Visibility Visibility { get; set; }
```

##### Property Value

[Visibility](layout/visibility.md)

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
public bool ContainsPoint(Microsoft.Xna.Framework.Vector2 point);
```

##### Parameters

**`point`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
The point to test.

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  `true` if `point` is within the view bounds; otherwise `false`.

-----

#### Draw(ISpriteBatch)

Draws the content for this view.

```cs
public void Draw(StardewUI.Graphics.ISpriteBatch b);
```

##### Parameters

**`b`** &nbsp; [ISpriteBatch](graphics/ispritebatch.md)  
Sprite batch to hold the drawing output.

##### Remarks

Drawing always happens after the measure pass, so [ContentSize](view.md#contentsize) should be known and stable at this time, as long as the implementation itself is stable.

-----

#### FindFocusableDescendant(Vector2, Direction)

Searches for a focusable child within this view that is reachable in the specified `direction`, and returns a result containing the view and search path if found.

```cs
protected virtual StardewUI.Input.FocusSearchResult FindFocusableDescendant(Microsoft.Xna.Framework.Vector2 contentPosition, StardewUI.Direction direction);
```

##### Parameters

**`contentPosition`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
The search position, relative to where this view's content starts (after applying margin, borders and padding).

**`direction`** &nbsp; [Direction](direction.md)  
The search direction.

##### Returns

[FocusSearchResult](input/focussearchresult.md)

##### Remarks

This is the same as [FocusSearch(Vector2, Direction)](view.md#focussearchvector2-direction) but in pre-transformed content coordinates, and does not require checking for "self-focus" as [FocusSearch(Vector2, Direction)](view.md#focussearchvector2-direction) already does this. The default implementation simply returns `null` as most views do not have children; subclasses with children must override this.

-----

#### FocusSearch(Vector2, Direction)

Finds the next focusable component in a given direction that does _not_ overlap with a current position.

```cs
public StardewUI.Input.FocusSearchResult FocusSearch(Microsoft.Xna.Framework.Vector2 position, StardewUI.Direction direction);
```

##### Parameters

**`position`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
The current cursor position, relative to this view. May have dimensions that are negative or outside the view bounds, indicating that the cursor is not currently within the view.

**`direction`** &nbsp; [Direction](direction.md)  
The direction of cursor movement.

##### Returns

[FocusSearchResult](input/focussearchresult.md)

##### Remarks

This will first call [FindFocusableDescendant(Vector2, Direction)](view.md#findfocusabledescendantvector2-direction) to see if the specific view type wants to implement its own focus search. If there is no focusable descendant, then this will return a reference to the current view if [IsFocusable](view.md#isfocusable) is `true` and the position is _not_ already within the view's bounds - meaning, any focusable view can accept focus from any direction, but will not consider itself a result if it is already focused (since we are trying to "move" focus).

-----

#### GetBorderThickness()

Measures the thickness of each edge of the border, if the view has a border.

```cs
protected virtual StardewUI.Layout.Edges GetBorderThickness();
```

##### Returns

[Edges](layout/edges.md)

  The border edge thicknesses.

##### Remarks

Used only by views that will implement a border via [OnDrawBorder(ISpriteBatch)](view.md#ondrawborderispritebatch). The border thickness is considered during layout, and generally treated as additional [Padding](view.md#padding) for the purposes of setting allowed content size. 

 Borders usually have a static size, but if the thickness can change, then implementations must account for it in their dirty checking ([IsContentDirty()](view.md#iscontentdirty)).

-----

#### GetChildAt(Vector2)

Finds the child at a given position.

```cs
public StardewUI.ViewChild GetChildAt(Microsoft.Xna.Framework.Vector2 position);
```

##### Parameters

**`position`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
The search position, relative to the view's top-left coordinate.

##### Returns

[ViewChild](viewchild.md)

  The view at `position`, or `null` if there is no match.

##### Remarks

If multiple children overlap the same position, then this returns the topmost child.

-----

#### GetChildPosition(IView)

Computes or retrieves the position of a given direct child.

```cs
public Microsoft.Xna.Framework.Vector2? GetChildPosition(StardewUI.IView childView);
```

##### Parameters

**`childView`** &nbsp; [IView](iview.md)  
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
public System.Collections.Generic.IEnumerable<StardewUI.ViewChild> GetChildren();
```

##### Returns

[IEnumerable](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)<[ViewChild](viewchild.md)>

-----

#### GetChildrenAt(Vector2)

Finds all children at a given position.

```cs
public System.Collections.Generic.IEnumerable<StardewUI.ViewChild> GetChildrenAt(Microsoft.Xna.Framework.Vector2 position);
```

##### Parameters

**`position`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
The search position, relative to the view's top-left coordinate.

##### Returns

[IEnumerable](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)<[ViewChild](viewchild.md)>

  A sequence of views at the specified `position`, in front-to-back (reverse [ZIndex](iview.md#zindex)) order.

-----

#### GetDefaultFocusChild()

Gets the direct child that should contain cursor focus when a menu or overlay containing this view is first opened.

```cs
public virtual StardewUI.ViewChild GetDefaultFocusChild();
```

##### Returns

[ViewChild](viewchild.md)

  The child view and position where initial focus should be, either directly or indirectly as a result of a descendant receiving focus. If this [IView](iview.md) is itself [IsFocusable](iview.md#isfocusable), then the result may be a [ViewChild](viewchild.md) referencing this view.

-----

#### GetLocalChildren()

Gets the view's children with positions relative to the content area.

```cs
protected virtual System.Collections.Generic.IEnumerable<StardewUI.ViewChild> GetLocalChildren();
```

##### Returns

[IEnumerable](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)<[ViewChild](viewchild.md)>

##### Remarks

This has the same signature as [GetChildren()](view.md#getchildren) but assumes that coordinates are in the same space as those used in [OnDrawContent(ISpriteBatch)](view.md#ondrawcontentispritebatch), i.e. not accounting for margin/border/padding. These coordinates are automatically adjusted in the [GetChildren()](view.md#getchildren) to be relative to the entire view. 

 The default implementation returns an empty sequence. Composite views must override this method in order for user interactions to behave correctly.

-----

#### GetLocalChildrenAt(Vector2)

Searches for all views at a given position relative to the content area.

```cs
protected virtual System.Collections.Generic.IEnumerable<StardewUI.ViewChild> GetLocalChildrenAt(Microsoft.Xna.Framework.Vector2 contentPosition);
```

##### Parameters

**`contentPosition`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
The search position, relative to where this view's content starts (after applying margin, borders and padding).

##### Returns

[IEnumerable](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)<[ViewChild](viewchild.md)>

  The views at the specified `contentPosition`, sorted in reverse order of their [ZIndex](iview.md#zindex).

##### Remarks

The default implementation performs a linear search on all children and returns all whose bounds overlap the specified `contentPosition`. Views can override this to provide optimized implementations for their layout, or handle overlapping views.

-----

#### HasOutOfBoundsContent()

Checks if the view has content or elements that are all or partially outside the [ActualBounds](iview.md#actualbounds).

```cs
public bool HasOutOfBoundsContent();
```

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

##### Remarks

This may be the case for e.g. floating elements, and covers not only the view's immediate content/children but also that of any descendants.

-----

#### IsContentDirty()

Checks whether or not the internal content/layout has changed.

```cs
protected virtual bool IsContentDirty();
```

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  `true` if content has changed; otherwise `false`.

##### Remarks

The base implementation of [IsDirty()](view.md#isdirty) only checks if the base layout attributes have changed, i.e. [Layout](view.md#layout), [Margin](view.md#margin), [Padding](view.md#padding), etc. It does not know about content/data in any subclasses; those that accept content parameters (like text) will typically use [DirtyTracker&lt;T&gt;](layout/dirtytracker-1.md) to hold that content and should implement this method to check their [IsDirty](layout/dirtytracker-1.md#isdirty) states.

-----

#### IsDirty()

Checks whether or not the view is dirty - i.e. requires a new layout with a full [Measure(Vector2)](iview.md#measurevector2).

```cs
public bool IsDirty();
```

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  `true` if the view must be measured again; otherwise `false`.

##### Remarks

Typically, a view will be considered dirty if and only if one of the following are true: 

  - The [Layout](iview.md#layout) has changed
  - The content has changed in a way that could affect layout, e.g. the text has changed in a [Content](layout/lengthtype.md#content) configuration
  - The `availableSize` is not the same as the previously-seen value (see remarks in [Measure(Vector2)](iview.md#measurevector2))

 A correct implementation is important for performance, as full layout can be very expensive to run on every frame.

-----

#### LogFocusSearch(string)

Outputs a debug log entry with the current view type, name and specified message.

```cs
protected void LogFocusSearch(string message);
```

##### Parameters

**`message`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The message to log in addition to the view type and name.

##### Remarks

Used primarily for debugging focus searches and requires the `DEBUG_FOCUS_SEARCH` compiler flag.

-----

#### Measure(Vector2)

Performs layout on this view, updating its [OuterSize](iview.md#outersize), [ActualBounds](iview.md#actualbounds) and [ContentBounds](iview.md#contentbounds), and arranging any children in their respective positions.

```cs
public bool Measure(Microsoft.Xna.Framework.Vector2 availableSize);
```

##### Parameters

**`availableSize`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
The width/height that is still available in the container/parent.

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  Whether or not any layout was performed as a result of this pass. Callers may use this to propagate layout back up the tree, or perform expensive follow-up actions.

##### Remarks

Most views should save the value of `availableSize` for use in [IsDirty()](iview.md#isdirty) checks.

-----

#### OnButtonPress(ButtonEventArgs)

Called when a button press is received while this view is in the focus path.

```cs
public virtual void OnButtonPress(StardewUI.Events.ButtonEventArgs e);
```

##### Parameters

**`e`** &nbsp; [ButtonEventArgs](events/buttoneventargs.md)  
The event data.

-----

#### OnClick(ClickEventArgs)

Called when a click is received within this view's bounds.

```cs
public virtual void OnClick(StardewUI.Events.ClickEventArgs e);
```

##### Parameters

**`e`** &nbsp; [ClickEventArgs](events/clickeventargs.md)  
The event data.

-----

#### OnDrag(PointerEventArgs)

Called when the view is being dragged (mouse moved while left button held).

```cs
public virtual void OnDrag(StardewUI.Events.PointerEventArgs e);
```

##### Parameters

**`e`** &nbsp; [PointerEventArgs](events/pointereventargs.md)  
The event data.

-----

#### OnDrawBorder(ISpriteBatch)

Draws the view's border, if it has one.

```cs
protected virtual void OnDrawBorder(StardewUI.Graphics.ISpriteBatch b);
```

##### Parameters

**`b`** &nbsp; [ISpriteBatch](graphics/ispritebatch.md)  
Sprite batch to hold the drawing output.

##### Remarks

This is called from [Draw(ISpriteBatch)](view.md#drawispritebatch) after applying [Margin](view.md#margin) but before [Padding](view.md#padding).

-----

#### OnDrawContent(ISpriteBatch)

Draws the inner content of this view.

```cs
protected virtual void OnDrawContent(StardewUI.Graphics.ISpriteBatch b);
```

##### Parameters

**`b`** &nbsp; [ISpriteBatch](graphics/ispritebatch.md)  
Sprite batch to hold the drawing output.

##### Remarks

This is called from [Draw(ISpriteBatch)](view.md#drawispritebatch) after applying both [Margin](view.md#margin) and [Padding](view.md#padding).

-----

#### OnDrop(PointerEventArgs)

Called when the mouse button is released after at least one [OnDrag(PointerEventArgs)](iview.md#ondragpointereventargs).

```cs
public virtual void OnDrop(StardewUI.Events.PointerEventArgs e);
```

##### Parameters

**`e`** &nbsp; [PointerEventArgs](events/pointereventargs.md)  
The event data.

-----

#### OnMeasure(Vector2)

Performs the internal layout.

```cs
protected virtual void OnMeasure(Microsoft.Xna.Framework.Vector2 availableSize);
```

##### Parameters

**`availableSize`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
Size available in the container, after applying padding, margin and borders.

##### Remarks

This is called from [Measure(Vector2)](view.md#measurevector2) only when the layout is dirty (layout parameters or content changed) and a new layout is actually required. Subclasses must implement this and set [ContentSize](view.md#contentsize) once layout is complete. Typically, [Resolve(Vector2, Func&lt;Vector2&gt;)](layout/layoutparameters.md#resolvevector2-funcvector2) should be used in order to ensure that the original [LayoutParameters](layout/layoutparameters.md) are respected (e.g. if the actual content size is smaller than the configured size). 

 The `availableSize` provided to the method is pre-adjusted for [Margin](view.md#margin), [Padding](view.md#padding), and any border determined by [GetBorderThickness()](view.md#getborderthickness).

-----

#### OnPointerMove(PointerMoveEventArgs)

Called when a pointer movement related to this view occurs.

```cs
public virtual void OnPointerMove(StardewUI.Events.PointerMoveEventArgs e);
```

##### Parameters

**`e`** &nbsp; [PointerMoveEventArgs](events/pointermoveeventargs.md)  
The event data.

##### Remarks

This can either be the pointer entering the view, leaving the view, or moving within the view. The method is used to trigger events such as [PointerEnter](view.md#pointerenter) and [PointerLeave](view.md#pointerleave).

-----

#### OnPropertyChanged(PropertyChangedEventArgs)

Raises the [PropertyChanged](view.md#propertychanged) event.

```cs
protected virtual void OnPropertyChanged(System.ComponentModel.PropertyChangedEventArgs args);
```

##### Parameters

**`args`** &nbsp; [PropertyChangedEventArgs](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.propertychangedeventargs)  
The event arguments.

-----

#### OnPropertyChanged(string)

Raises the [PropertyChanged](view.md#propertychanged) event.

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

When overriding [OnUpdate(TimeSpan)](view.md#onupdatetimespan), be sure to call `base.OnUpdate()` to ensure that any view children also receive their updates.

-----

#### OnWheel(WheelEventArgs)

Called when a wheel event is received within this view's bounds.

```cs
public virtual void OnWheel(StardewUI.Events.WheelEventArgs e);
```

##### Parameters

**`e`** &nbsp; [WheelEventArgs](events/wheeleventargs.md)  
The event data.

-----

#### ResetDirty()

Resets any dirty state associated with this view.

```cs
protected virtual void ResetDirty();
```

##### Remarks

This is called at the end of [Measure(Vector2)](view.md#measurevector2), so that on the next pass, all state appears clean unless it was marked dirty after the last pass completed. The default implementation is a no-op; subclasses should use it to clear any private dirty state, e.g. via [ResetDirty()](layout/dirtytracker-1.md#resetdirty).

-----

#### ScrollIntoView(IEnumerable&lt;ViewChild&gt;, Vector2)

Attempts to scroll the specified target into view, including all of its ancestors, if not fully in view.

```cs
public virtual bool ScrollIntoView(System.Collections.Generic.IEnumerable<StardewUI.ViewChild> path, out Microsoft.Xna.Framework.Vector2 distance);
```

##### Parameters

**`path`** &nbsp; [IEnumerable](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)<[ViewChild](viewchild.md)>  
The path to the view that should be visible, starting from (and not including) this view; each element has the local position within its own parent, so the algorithm can run recursively. This is a slice of the same path returned in a [FocusSearchResult](input/focussearchresult.md).

**`distance`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
The total distance that was scrolled, including distance scrolled by descendants.

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

##### Remarks

The default implementation does no scrolling of its own, only passes the request down to the child and aborts if the child returns `true`. Scrollable views must override this to provide scrolling behavior.

-----

#### ToString()



```cs
public override string ToString();
```

##### Returns

[string](https://learn.microsoft.com/en-us/dotnet/api/system.string)

-----

### Events

#### ButtonPress

Event raised when any button on any input device is pressed.

```cs
public event EventHandler<StardewUI.Events.ButtonEventArgs>? ButtonPress;
```

##### Event Type

[EventHandler](https://learn.microsoft.com/en-us/dotnet/api/system.eventhandler-1)<[ButtonEventArgs](events/buttoneventargs.md)>

##### Remarks

Only the views in the current focus path should receive these events.

-----

#### Click

Event raised when the view receives a click.

```cs
public event EventHandler<StardewUI.Events.ClickEventArgs>? Click;
```

##### Event Type

[EventHandler](https://learn.microsoft.com/en-us/dotnet/api/system.eventhandler-1)<[ClickEventArgs](events/clickeventargs.md)>

-----

#### Drag

Event raised when the view is being dragged using the mouse.

```cs
public event EventHandler<StardewUI.Events.PointerEventArgs>? Drag;
```

##### Event Type

[EventHandler](https://learn.microsoft.com/en-us/dotnet/api/system.eventhandler-1)<[PointerEventArgs](events/pointereventargs.md)>

-----

#### DragEnd

Event raised when mouse dragging is stopped, i.e. when the button is released. Always raised after the last [Drag](view.md#drag), and only once per drag operation.

```cs
public event EventHandler<StardewUI.Events.PointerEventArgs>? DragEnd;
```

##### Event Type

[EventHandler](https://learn.microsoft.com/en-us/dotnet/api/system.eventhandler-1)<[PointerEventArgs](events/pointereventargs.md)>

-----

#### DragStart

Event raised when mouse dragging is first activated. Always raised before the first [Drag](view.md#drag), and only once per drag operation.

```cs
public event EventHandler<StardewUI.Events.PointerEventArgs>? DragStart;
```

##### Event Type

[EventHandler](https://learn.microsoft.com/en-us/dotnet/api/system.eventhandler-1)<[PointerEventArgs](events/pointereventargs.md)>

-----

#### LeftClick

Event raised when the view receives a click initiated from the left mouse button, or the controller's action button (A).

```cs
public event EventHandler<StardewUI.Events.ClickEventArgs>? LeftClick;
```

##### Event Type

[EventHandler](https://learn.microsoft.com/en-us/dotnet/api/system.eventhandler-1)<[ClickEventArgs](events/clickeventargs.md)>

-----

#### PointerEnter

Event raised when the pointer enters the view.

```cs
public event EventHandler<StardewUI.Events.PointerEventArgs>? PointerEnter;
```

##### Event Type

[EventHandler](https://learn.microsoft.com/en-us/dotnet/api/system.eventhandler-1)<[PointerEventArgs](events/pointereventargs.md)>

-----

#### PointerLeave

Event raised when the pointer exits the view.

```cs
public event EventHandler<StardewUI.Events.PointerEventArgs>? PointerLeave;
```

##### Event Type

[EventHandler](https://learn.microsoft.com/en-us/dotnet/api/system.eventhandler-1)<[PointerEventArgs](events/pointereventargs.md)>

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

[EventHandler](https://learn.microsoft.com/en-us/dotnet/api/system.eventhandler-1)<[ClickEventArgs](events/clickeventargs.md)>

-----

#### Wheel

Event raised when the scroll wheel moves.

```cs
public event EventHandler<StardewUI.Events.WheelEventArgs>? Wheel;
```

##### Event Type

[EventHandler](https://learn.microsoft.com/en-us/dotnet/api/system.eventhandler-1)<[WheelEventArgs](events/wheeleventargs.md)>

-----

