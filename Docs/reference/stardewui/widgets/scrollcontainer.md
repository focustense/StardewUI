---
title: ScrollContainer
description: Renders inner content clipped to a boundary and with a modifiable scroll offset.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class ScrollContainer

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Widgets](index.md)  
Assembly: StardewUI.dll  

</div>

Renders inner content clipped to a boundary and with a modifiable scroll offset.

```cs
[StardewUI.GenerateDescriptor]
public class ScrollContainer : StardewUI.View
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ [View](../view.md) ⇦ ScrollContainer

## Remarks

Does not provide its own scroll bar; scrolling UI and behavior can be controlled via adding a [Scrollbar](scrollbar.md) to any other part of the UI. 

 While nothing prevents a [ScrollContainer](scrollcontainer.md) from being set up with the [Orientation](scrollcontainer.md#orientation) dimension set to use [Content](../layout/lengthtype.md#content), in general the container will only work correctly when the scrolled dimension is constrained ([Px](../layout/lengthtype.md#px) or [Stretch](../layout/lengthtype.md#stretch)). Scrolling behavior is enabled by providing an infinite available length to the [Content](scrollcontainer.md#content) view for layout, while constraining its own size. 

 Scrolling is not virtual. Regardless of the difference in size between scroll container and content, the full content will always be drawn on every frame, and simply clipped to the available area. This may therefore not be suitable for extremely long lists or other unbounded content.

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [ScrollContainer()](#scrollcontainer) |  | 

### Properties

 | Name | Description |
| --- | --- |
| [ActualBounds](../view.md#actualbounds) | The bounds of this view relative to the origin (0, 0).<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [BorderSize](../view.md#bordersize) | The layout size (not edge thickness) of the entire drawn area including the border, i.e. the [InnerSize](../view.md#innersize) plus any borders defined in [GetBorderThickness()](../view.md#getborderthickness). Does not include the [Margin](../view.md#margin).<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [ClipOrigin](../view.md#cliporigin) | Origin position for the [ClipSize](../iview.md#clipsize).<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [ClipSize](../view.md#clipsize) | Size of the clipping rectangle, outside which content will not be displayed.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [Content](#content) | The inner content view which will be scrolled. | 
| [ContentBounds](../view.md#contentbounds) | The true bounds of this view's content; i.e. [ActualBounds](../iview.md#actualbounds) excluding margins.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [ContentSize](../view.md#contentsize) | The size of the view's content, which is drawn inside the padding. Subclasses set this in their [OnMeasure(Vector2)](../view.md#onmeasurevector2) method and padding, margins, etc. are handled automatically.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [ContentViewSize](#contentviewsize) | The size of the current content view, or [Zero](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html#Microsoft_Xna_Framework_Vector2) if there is no content. | 
| [Draggable](../view.md#draggable) | Whether or not this view should fire drag events such as [DragStart](../view.md#dragstart) and [Drag](../view.md#drag).<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [FloatingBounds](../view.md#floatingbounds) | Contains the bounds of all floating elements in this view tree, including the current view and all descendants.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [FloatingElements](../view.md#floatingelements) | The floating elements to display relative to this view.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [Focusable](../view.md#focusable) | Whether or not the view should be able to receive focus. Applies only to this specific view, not its children.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [HandlesOpacity](../view.md#handlesopacity) | Whether the specific view type handles its own opacity.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [InnerSize](../view.md#innersize) | The size allocated to the entire area inside the border, i.e. [ContentSize](../view.md#contentsize) plus any [Padding](../view.md#padding). Does not include border or [Margin](../view.md#margin).<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [IsFocusable](../view.md#isfocusable) | Whether or not the view can receive controller focus, i.e. the stick/d-pad controlled cursor can move to this view. Not generally applicable for mouse controls.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [LastAvailableSize](../view.md#lastavailablesize) | The most recent size used in a [Measure(Vector2)](../view.md#measurevector2) pass. Used for additional dirty checks.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [Layout](../view.md#layout) | Layout settings for this view; determines how its dimensions will be computed.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [LayoutOffset](#layoutoffset) | Pixel offset of the view's content, which is applied to all pointer events and child queries.<br><span class="muted" markdown>(Overrides [View](../view.md).`get_LayoutOffset()`)</span> | 
| [Margin](../view.md#margin) | Margins (whitespace outside border) for this view.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [Name](../view.md#name) | Simple name for this view, used in log/debug output; does not affect behavior.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [Opacity](../view.md#opacity) | Opacity (alpha level) of the view.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [Orientation](#orientation) | The orientation, i.e. the direction of scrolling. | 
| [OuterSize](../view.md#outersize) | The size of the entire area occupied by this view including margins, border and padding.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [Padding](../view.md#padding) | Padding (whitespace inside border) for this view.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [Peeking](#peeking) | The amount of "peeking" to add when scrolling a component into view; adds extra space before/after the visible element so that all or part of the previous/next element is also visible. | 
| [PointerEventsEnabled](../view.md#pointereventsenabled) | Whether this view should receive pointer events like [Click](../view.md#click) or [Drag](../view.md#drag).<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [PointerStyle](../view.md#pointerstyle) | Pointer style to use when this view is hovered.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [ScrollOffset](#scrolloffset) | The current scroll position along the [Orientation](scrollcontainer.md#orientation) axis. | 
| [ScrollSize](#scrollsize) | The maximum amount by which the container can be scrolled without exceeding the inner content bounds. | 
| [ScrollStep](#scrollstep) | Default scroll distance when calling [ScrollForward()](scrollcontainer.md#scrollforward) or [ScrollBackward()](scrollcontainer.md#scrollbackward). Does not prevent directly setting the scroll position via [ScrollOffset](scrollcontainer.md#scrolloffset). | 
| [ScrollWithChildren](../view.md#scrollwithchildren) | If set to an axis, specifies that when any child of the view is scrolled into view (using [ScrollIntoView(IEnumerable&lt;ViewChild&gt;, Vector2)](../view.md#scrollintoviewienumerableviewchild-vector2)), then this entire view should be scrolled along with it.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [Tags](../view.md#tags) | The user-defined tags for this view.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [Tooltip](../view.md#tooltip) | Localized tooltip to display on hover, if any.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [Transform](../view.md#transform) | Local transformation to apply to this view, including any children and floating elements.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [TransformOrigin](../view.md#transformorigin) | Relative origin position for any [Transform](../iview.md#transform) on this view.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [Visibility](../view.md#visibility) | Visibility for this view.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [ZIndex](../view.md#zindex) | Z order for this view within its direct parent. Higher indices draw later (on top).<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 

### Methods

 | Name | Description |
| --- | --- |
| [ContainsPoint(Vector2)](../view.md#containspointvector2) | Checks if a given point, relative to the view's origin, is within its bounds.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [Dispose()](../view.md#dispose) | <span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [Draw(ISpriteBatch)](../view.md#drawispritebatch) | Draws the content for this view.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [FindFocusableDescendant(Vector2, Direction)](#findfocusabledescendantvector2-direction) | Searches for a focusable child within this view that is reachable in the specified `direction`, and returns a result containing the view and search path if found.<br><span class="muted" markdown>(Overrides [View](../view.md).[FindFocusableDescendant(Vector2, Direction)](../view.md#findfocusabledescendantvector2-direction))</span> | 
| [FocusSearch(Vector2, Direction)](../view.md#focussearchvector2-direction) | Finds the next focusable component in a given direction that does _not_ overlap with a current position.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [GetBorderThickness()](../view.md#getborderthickness) | Measures the thickness of each edge of the border, if the view has a border.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [GetChildAt(Vector2, Boolean, Boolean)](../view.md#getchildatvector2-bool-bool) | Finds the child at a given position.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [GetChildPosition(IView)](../view.md#getchildpositioniview) | Computes or retrieves the position of a given direct child.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [GetChildren(Boolean)](../view.md#getchildrenbool) | Gets the current children of this view.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [GetChildrenAt(Vector2)](../view.md#getchildrenatvector2) | Finds all children at a given position.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [GetDefaultFocusChild()](../view.md#getdefaultfocuschild) | Gets the direct child that should contain cursor focus when a menu or overlay containing this view is first opened.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [GetLocalChildren()](#getlocalchildren) | Gets the view's children with positions relative to the content area.<br><span class="muted" markdown>(Overrides [View](../view.md).[GetLocalChildren()](../view.md#getlocalchildren))</span> | 
| [GetLocalChildrenAt(Vector2)](#getlocalchildrenatvector2) | Searches for all views at a given position relative to the content area.<br><span class="muted" markdown>(Overrides [View](../view.md).[GetLocalChildrenAt(Vector2)](../view.md#getlocalchildrenatvector2))</span> | 
| [HasOutOfBoundsContent()](../view.md#hasoutofboundscontent) | Checks if the view has content or elements that are all or partially outside the [ActualBounds](../iview.md#actualbounds).<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [HasOwnContent()](#hasowncontent) | Checks if this view displays its own content, independent of any floating elements or children.<br><span class="muted" markdown>(Overrides [View](../view.md).[HasOwnContent()](../view.md#hasowncontent))</span> | 
| [IsContentDirty()](#iscontentdirty) | Checks whether or not the internal content/layout has changed.<br><span class="muted" markdown>(Overrides [View](../view.md).[IsContentDirty()](../view.md#iscontentdirty))</span> | 
| [IsDirty()](../view.md#isdirty) | Checks whether or not the view is dirty - i.e. requires a new layout with a full [Measure(Vector2)](../iview.md#measurevector2).<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [IsVisible(Vector2?)](../view.md#isvisiblevector2) | Checks if the view is effectively visible, i.e. if it has anything to draw.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [LogFocusSearch(string)](../view.md#logfocussearchstring) | Outputs a debug log entry with the current view type, name and specified message.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [Measure(Vector2)](../view.md#measurevector2) | Performs layout on this view, updating its [OuterSize](../iview.md#outersize), [ActualBounds](../iview.md#actualbounds) and [ContentBounds](../iview.md#contentbounds), and arranging any children in their respective positions.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [OnButtonPress(ButtonEventArgs)](../view.md#onbuttonpressbuttoneventargs) | Called when a button press is received while this view is in the focus path.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [OnButtonRepeat(ButtonEventArgs)](../view.md#onbuttonrepeatbuttoneventargs) | Called when a button press is first received, and at recurring intervals thereafter, for as long as the button is held and this view remains in the focus path.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [OnClick(ClickEventArgs)](../view.md#onclickclickeventargs) | Called when a click is received within this view's bounds.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [OnDispose()](../view.md#ondispose) | Performs additional cleanup when [Dispose()](../view.md#dispose) is called.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [OnDrag(PointerEventArgs)](../view.md#ondragpointereventargs) | Called when the view is being dragged (mouse moved while left button held).<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [OnDrawBorder(ISpriteBatch)](../view.md#ondrawborderispritebatch) | Draws the view's border, if it has one.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [OnDrawContent(ISpriteBatch)](#ondrawcontentispritebatch) | Draws the inner content of this view.<br><span class="muted" markdown>(Overrides [View](../view.md).[OnDrawContent(ISpriteBatch)](../view.md#ondrawcontentispritebatch))</span> | 
| [OnDrop(PointerEventArgs)](../view.md#ondroppointereventargs) | Called when the mouse button is released after at least one [OnDrag(PointerEventArgs)](../iview.md#ondragpointereventargs).<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [OnMeasure(Vector2)](#onmeasurevector2) | Performs the internal layout.<br><span class="muted" markdown>(Overrides [View](../view.md).[OnMeasure(Vector2)](../view.md#onmeasurevector2))</span> | 
| [OnPointerMove(PointerMoveEventArgs)](../view.md#onpointermovepointermoveeventargs) | Called when a pointer movement related to this view occurs.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [OnPropertyChanged(PropertyChangedEventArgs)](#onpropertychangedpropertychangedeventargs) | Raises the [PropertyChanged](../view.md#propertychanged) event.<br><span class="muted" markdown>(Overrides [View](../view.md).[OnPropertyChanged(PropertyChangedEventArgs)](../view.md#onpropertychangedpropertychangedeventargs))</span> | 
| [OnPropertyChanged(string)](../view.md#onpropertychangedstring) | Raises the [PropertyChanged](../view.md#propertychanged) event.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [OnUpdate(TimeSpan)](../view.md#onupdatetimespan) | Runs on every update tick.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [OnWheel(WheelEventArgs)](../view.md#onwheelwheeleventargs) | Called when a wheel event is received within this view's bounds.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [ResetDirty()](#resetdirty) | Resets any dirty state associated with this view.<br><span class="muted" markdown>(Overrides [View](../view.md).[ResetDirty()](../view.md#resetdirty))</span> | 
| [ScrollBackward()](#scrollbackward) | Scrolls backward (up or left) by the distance configured in [ScrollStep](scrollcontainer.md#scrollstep). | 
| [ScrollForward()](#scrollforward) | Scrolls forward (down or right) by the distance configured in [ScrollStep](scrollcontainer.md#scrollstep). | 
| [ScrollIntoView(IEnumerable&lt;ViewChild&gt;, Vector2)](#scrollintoviewienumerableviewchild-vector2) | Attempts to scroll the specified target into view, including all of its ancestors, if not fully in view.<br><span class="muted" markdown>(Overrides [View](../view.md).[ScrollIntoView(IEnumerable&lt;ViewChild&gt;, Vector2)](../view.md#scrollintoviewienumerableviewchild-vector2))</span> | 
| [ToString()](../view.md#tostring) | <span class="muted" markdown>(Inherited from [View](../view.md))</span> | 

### Events

 | Name | Description |
| --- | --- |
| [ButtonPress](../view.md#buttonpress) | Event raised when any button on any input device is pressed.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [ButtonRepeat](../view.md#buttonrepeat) | Event raised when a button is being held while the view is in focus, and has been held long enough since the initial [ButtonPress](../view.md#buttonpress) or the previous `ButtonRepeat` to trigger a repeated press.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [Click](../view.md#click) | Event raised when the view receives a click.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [Drag](../view.md#drag) | Event raised when the view is being dragged using the mouse.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [DragEnd](../view.md#dragend) | Event raised when mouse dragging is stopped, i.e. when the button is released. Always raised after the last [Drag](../view.md#drag), and only once per drag operation.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [DragStart](../view.md#dragstart) | Event raised when mouse dragging is first activated. Always raised before the first [Drag](../view.md#drag), and only once per drag operation.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [LeftClick](../view.md#leftclick) | Event raised when the view receives a click initiated from the left mouse button, or the controller's action button (A).<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [PointerEnter](../view.md#pointerenter) | Event raised when the pointer enters the view.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [PointerLeave](../view.md#pointerleave) | Event raised when the pointer exits the view.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [PointerMove](../view.md#pointermove) | Event raised when the pointer moves within the view.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [PropertyChanged](../view.md#propertychanged) | <span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [RightClick](../view.md#rightclick) | Event raised when the view receives a click initiated from the right mouse button, or the controller's tool-use button (X).<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [ScrollChanged](#scrollchanged) | Event raised when any aspect of the scrolling changes. | 
| [Wheel](../view.md#wheel) | Event raised when the scroll wheel moves.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 

## Details

### Constructors

#### ScrollContainer()



```cs
public ScrollContainer();
```

-----

### Properties

#### Content

The inner content view which will be scrolled.

```cs
public StardewUI.IView Content { get; set; }
```

##### Property Value

[IView](../iview.md)

-----

#### ContentViewSize

The size of the current content view, or [Zero](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html#Microsoft_Xna_Framework_Vector2) if there is no content.

```cs
protected Microsoft.Xna.Framework.Vector2 ContentViewSize { get; }
```

##### Property Value

[Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)

-----

#### LayoutOffset

Pixel offset of the view's content, which is applied to all pointer events and child queries.

```cs
protected Microsoft.Xna.Framework.Vector2 LayoutOffset { get; }
```

##### Property Value

[Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)

##### Remarks

A non-zero offset means that the nominal positions of any view children (e.g. as obtained from [GetChildren(Boolean)](../view.md#getchildrenbool)) are different from their actual drawing positions on screen, for example in the case of a [ScrollContainer](scrollcontainer.md) that is not at the default scroll position. 

 If a view will internally shift content in this way without affecting layout, it should update the [LayoutOffset](../view.md#layoutoffset) property to ensure correctness of pointer events and coordinate-related queries such as [GetLocalChildrenAt(Vector2)](../view.md#getlocalchildrenatvector2), **instead of** attempting to correct for that offset locally.

-----

#### Orientation

The orientation, i.e. the direction of scrolling.

```cs
public StardewUI.Layout.Orientation Orientation { get; set; }
```

##### Property Value

[Orientation](../layout/orientation.md)

##### Remarks

A single [ScrollContainer](scrollcontainer.md) can only scroll in one direction. If content needs to scroll both horizontally and vertically, a nested [ScrollContainer](scrollcontainer.md) can be used.

-----

#### Peeking

The amount of "peeking" to add when scrolling a component into view; adds extra space before/after the visible element so that all or part of the previous/next element is also visible.

```cs
public float Peeking { get; set; }
```

##### Property Value

[Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)

##### Remarks

Nonzero values help with discoverability, making it clear that there is more content.

-----

#### ScrollOffset

The current scroll position along the [Orientation](scrollcontainer.md#orientation) axis.

```cs
public float ScrollOffset { get; set; }
```

##### Property Value

[Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)

-----

#### ScrollSize

The maximum amount by which the container can be scrolled without exceeding the inner content bounds.

```cs
public float ScrollSize { get; }
```

##### Property Value

[Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)

-----

#### ScrollStep

Default scroll distance when calling [ScrollForward()](scrollcontainer.md#scrollforward) or [ScrollBackward()](scrollcontainer.md#scrollbackward). Does not prevent directly setting the scroll position via [ScrollOffset](scrollcontainer.md#scrolloffset).

```cs
public float ScrollStep { get; set; }
```

##### Property Value

[Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)

-----

### Methods

#### FindFocusableDescendant(Vector2, Direction)

Searches for a focusable child within this view that is reachable in the specified `direction`, and returns a result containing the view and search path if found.

```cs
protected override StardewUI.Input.FocusSearchResult FindFocusableDescendant(Microsoft.Xna.Framework.Vector2 contentPosition, StardewUI.Direction direction);
```

##### Parameters

**`contentPosition`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
The search position, relative to where this view's content starts (after applying margin, borders and padding).

**`direction`** &nbsp; [Direction](../direction.md)  
The search direction.

##### Returns

[FocusSearchResult](../input/focussearchresult.md)

##### Remarks

This is the same as [FocusSearch(Vector2, Direction)](../view.md#focussearchvector2-direction) but in pre-transformed content coordinates, and does not require checking for "self-focus" as [FocusSearch(Vector2, Direction)](../view.md#focussearchvector2-direction) already does this. The default implementation simply returns `null` as most views do not have children; subclasses with children must override this.

-----

#### GetLocalChildren()

Gets the view's children with positions relative to the content area.

```cs
protected override System.Collections.Generic.IEnumerable<StardewUI.ViewChild> GetLocalChildren();
```

##### Returns

[IEnumerable](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)<[ViewChild](../viewchild.md)>

##### Remarks

This has the same signature as [GetChildren(Boolean)](../view.md#getchildrenbool) but assumes that coordinates are in the same space as those used in [OnDrawContent(ISpriteBatch)](../view.md#ondrawcontentispritebatch), i.e. not accounting for margin/border/padding. These coordinates are automatically adjusted in the [GetChildren(Boolean)](../view.md#getchildrenbool) to be relative to the entire view. 

 The default implementation returns an empty sequence. Composite views must override this method in order for user interactions to behave correctly.

-----

#### GetLocalChildrenAt(Vector2)

Searches for all views at a given position relative to the content area.

```cs
protected override System.Collections.Generic.IEnumerable<StardewUI.ViewChild> GetLocalChildrenAt(Microsoft.Xna.Framework.Vector2 contentPosition);
```

##### Parameters

**`contentPosition`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
The search position, relative to where this view's content starts (after applying margin, borders and padding).

##### Returns

[IEnumerable](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)<[ViewChild](../viewchild.md)>

  The views at the specified `contentPosition`, in original layout order.

##### Remarks

The default implementation performs a linear search on all children and returns all whose bounds overlap the specified `contentPosition`. Views can override this to provide optimized implementations for their layout, or handle overlapping views.

-----

#### HasOwnContent()

Checks if this view displays its own content, independent of any floating elements or children.

```cs
protected override bool HasOwnContent();
```

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

##### Remarks

This is used by [IsVisible(Vector2?)](../view.md#isvisiblevector2) to determine whether children need to be searched. If a view provides its own content, e.g. a label or image displaying text or a sprite, or a frame displaying a background/border, then the entire view's bounds are understood to have visible content. Otherwise, the view is only considered visible as a whole if at least one child is visible, and is only visible at any given point if there is an intersecting child at that point.

-----

#### IsContentDirty()

Checks whether or not the internal content/layout has changed.

```cs
protected override bool IsContentDirty();
```

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  `true` if content has changed; otherwise `false`.

##### Remarks

The base implementation of [IsDirty()](../view.md#isdirty) only checks if the base layout attributes have changed, i.e. [Layout](../view.md#layout), [Margin](../view.md#margin), [Padding](../view.md#padding), etc. It does not know about content/data in any subclasses; those that accept content parameters (like text) will typically use [DirtyTracker&lt;T&gt;](../layout/dirtytracker-1.md) to hold that content and should implement this method to check their [IsDirty](../layout/dirtytracker-1.md#isdirty) states.

-----

#### OnDrawContent(ISpriteBatch)

Draws the inner content of this view.

```cs
protected override void OnDrawContent(StardewUI.Graphics.ISpriteBatch b);
```

##### Parameters

**`b`** &nbsp; [ISpriteBatch](../graphics/ispritebatch.md)  
Sprite batch to hold the drawing output.

##### Remarks

This is called from [Draw(ISpriteBatch)](../view.md#drawispritebatch) after applying both [Margin](../view.md#margin) and [Padding](../view.md#padding).

-----

#### OnMeasure(Vector2)

Performs the internal layout.

```cs
protected override void OnMeasure(Microsoft.Xna.Framework.Vector2 availableSize);
```

##### Parameters

**`availableSize`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
Size available in the container, after applying padding, margin and borders.

##### Remarks

This is called from [Measure(Vector2)](../view.md#measurevector2) only when the layout is dirty (layout parameters or content changed) and a new layout is actually required. Subclasses must implement this and set [ContentSize](../view.md#contentsize) once layout is complete. Typically, [Resolve(Vector2, Func&lt;Vector2&gt;)](../layout/layoutparameters.md#resolvevector2-funcvector2) should be used in order to ensure that the original [LayoutParameters](../layout/layoutparameters.md) are respected (e.g. if the actual content size is smaller than the configured size). 

 The `availableSize` provided to the method is pre-adjusted for [Margin](../view.md#margin), [Padding](../view.md#padding), and any border determined by [GetBorderThickness()](../view.md#getborderthickness).

-----

#### OnPropertyChanged(PropertyChangedEventArgs)

Raises the [PropertyChanged](../view.md#propertychanged) event.

```cs
protected override void OnPropertyChanged(System.ComponentModel.PropertyChangedEventArgs args);
```

##### Parameters

**`args`** &nbsp; [PropertyChangedEventArgs](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.propertychangedeventargs)  
The event arguments.

-----

#### ResetDirty()

Resets any dirty state associated with this view.

```cs
protected override void ResetDirty();
```

##### Remarks

This is called at the end of [Measure(Vector2)](../view.md#measurevector2), so that on the next pass, all state appears clean unless it was marked dirty after the last pass completed. The default implementation is a no-op; subclasses should use it to clear any private dirty state, e.g. via [ResetDirty()](../layout/dirtytracker-1.md#resetdirty).

-----

#### ScrollBackward()

Scrolls backward (up or left) by the distance configured in [ScrollStep](scrollcontainer.md#scrollstep).

```cs
public bool ScrollBackward();
```

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

-----

#### ScrollForward()

Scrolls forward (down or right) by the distance configured in [ScrollStep](scrollcontainer.md#scrollstep).

```cs
public bool ScrollForward();
```

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

-----

#### ScrollIntoView(IEnumerable&lt;ViewChild&gt;, Vector2)

Attempts to scroll the specified target into view, including all of its ancestors, if not fully in view.

```cs
public override bool ScrollIntoView(System.Collections.Generic.IEnumerable<StardewUI.ViewChild> path, out Microsoft.Xna.Framework.Vector2 distance);
```

##### Parameters

**`path`** &nbsp; [IEnumerable](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)<[ViewChild](../viewchild.md)>  
The path to the view that should be visible, starting from (and not including) this view; each element has the local position within its own parent, so the algorithm can run recursively. This is a slice of the same path returned in a [FocusSearchResult](../input/focussearchresult.md).

**`distance`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
The total distance that was scrolled, including distance scrolled by descendants.

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  Whether or not the scroll was successful; `false` prevents the request from bubbling.

##### Remarks

The default implementation does no scrolling of its own, only passes the request down to the child and aborts if the child returns `true`. Scrollable views must override this to provide scrolling behavior.

-----

### Events

#### ScrollChanged

Event raised when any aspect of the scrolling changes.

```cs
public event System.EventHandler? ScrollChanged;
```

##### Event Type

[EventHandler](https://learn.microsoft.com/en-us/dotnet/api/system.eventhandler)

##### Remarks

This tracks changes to the [ScrollOffset](scrollcontainer.md#scrolloffset) but also the [ScrollSize](scrollcontainer.md#scrollsize), even if the offset has not changed. [ScrollStep](scrollcontainer.md#scrollstep) is not included.

-----

