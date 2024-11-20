---
title: ScrollableFrameView
description: Layout widget for a sectioned menu including a scrollable content area.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class ScrollableFrameView

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Widgets](index.md)  
Assembly: StardewUI.dll  

</div>

Layout widget for a sectioned menu including a scrollable content area.

```cs
[StardewUI.GenerateDescriptor]
public class ScrollableFrameView : StardewUI.Widgets.ComponentView
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ [DecoratorView&lt;T&gt;](decoratorview-1.md) ⇦ [ComponentView&lt;T&gt;](componentview-1.md) ⇦ [ComponentView](componentview.md) ⇦ ScrollableFrameView

## Remarks

The primary content is always kept centered, with optional title, footer and sidebar (e.g. for navigation) available to decorate the view. Many if not most menus can be fully represented with this layout, as long as they do not have built-in subnavigation such as top-level tabs.

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [ScrollableFrameView()](#scrollableframeview) |  | 

### Properties

 | Name | Description |
| --- | --- |
| [ActualBounds](decoratorview-1.md#actualbounds) | The bounds of this view relative to the origin (0, 0).<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [Content](#content) | The primary content, which displays inside the menu frame and is clipped/scrollable. | 
| [ContentBounds](decoratorview-1.md#contentbounds) | The true bounds of this view's content; i.e. [ActualBounds](../iview.md#actualbounds) excluding margins.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [ContentLayout](#contentlayout) | Layout parameters to apply to the actual [ScrollContainer](scrollcontainer.md) containing the [Content](scrollableframeview.md#content). | 
| [FloatingBounds](decoratorview-1.md#floatingbounds) | Contains the bounds of all floating elements in this view tree, including the current view and all descendants.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [Footer](#footer) | Optional footer to display below the [Content](scrollableframeview.md#content). | 
| [FrameLayout](#framelayout) | Layout parameters to apply to the frame surrounding the [Content](scrollableframeview.md#content). | 
| [IsFocusable](decoratorview-1.md#isfocusable) | Whether or not the view can receive controller focus, i.e. the stick/d-pad controlled cursor can move to this view. Not generally applicable for mouse controls.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [Layout](decoratorview-1.md#layout) | The current layout parameters, which determine how [Measure(Vector2)](../iview.md#measurevector2) will behave.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [Name](decoratorview-1.md#name) | Simple name for this view, used in log/debug output; does not affect behavior.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [Opacity](decoratorview-1.md#opacity) | Opacity (alpha level) of the view.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [OuterSize](decoratorview-1.md#outersize) | The true computed layout size resulting from a single [Measure(Vector2)](../iview.md#measurevector2) pass.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [PointerEventsEnabled](decoratorview-1.md#pointereventsenabled) | Whether this view should receive pointer events like [Click](../iview.md#click) or [Drag](../iview.md#drag).<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [ScrollWithChildren](decoratorview-1.md#scrollwithchildren) | If set to an axis, specifies that when any child of the view is scrolled into view (using [ScrollIntoView(IEnumerable&lt;ViewChild&gt;, Vector2)](../iview.md#scrollintoviewienumerableviewchild-vector2)), then this entire view should be scrolled along with it.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [Sidebar](#sidebar) | Optional content to display to the left of the [Content](scrollableframeview.md#content) frame. | 
| [SidebarWidth](#sidebarwidth) | Maximum width of the sidebar area. | 
| [Tags](decoratorview-1.md#tags) | The user-defined tags for this view.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [Title](#title) | Title to display above the [Content](scrollableframeview.md#content). | 
| [Tooltip](decoratorview-1.md#tooltip) | Tooltip data to display on hover, if any.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [View](componentview-1.md#view) | <span class="muted" markdown>(Inherited from [ComponentView&lt;T&gt;](componentview-1.md))</span> | 
| [Visibility](decoratorview-1.md#visibility) | Drawing visibility for this view.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [ZIndex](decoratorview-1.md#zindex) | Z order for this view within its direct parent. Higher indices draw later (on top).<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 

### Methods

 | Name | Description |
| --- | --- |
| [ContainsPoint(Vector2)](decoratorview-1.md#containspointvector2) | Checks if a given point, relative to the view's origin, is within its bounds.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [CreateView()](#createview) | Creates and returns the root view.<br><span class="muted" markdown>(Overrides [ComponentView&lt;T&gt;](componentview-1.md).[CreateView()](componentview-1.md#createview))</span> | 
| [Dispose()](decoratorview-1.md#dispose) | <span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [Draw(ISpriteBatch)](decoratorview-1.md#drawispritebatch) | Draws the content for this view.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [FocusSearch(Vector2, Direction)](decoratorview-1.md#focussearchvector2-direction) | Finds the next focusable component in a given direction that does _not_ overlap with a current position.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [GetChildAt(Vector2)](decoratorview-1.md#getchildatvector2) | Finds the child at a given position.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [GetChildPosition(IView)](decoratorview-1.md#getchildpositioniview) | Computes or retrieves the position of a given direct child.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [GetChildren()](decoratorview-1.md#getchildren) | Gets the current children of this view.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [GetChildrenAt(Vector2)](decoratorview-1.md#getchildrenatvector2) | Finds all children at a given position.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [GetDefaultFocusChild()](decoratorview-1.md#getdefaultfocuschild) | Gets the direct child that should contain cursor focus when a menu or overlay containing this view is first opened.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [HasOutOfBoundsContent()](decoratorview-1.md#hasoutofboundscontent) | Checks if the view has content or elements that are all or partially outside the [ActualBounds](../iview.md#actualbounds).<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [IsDirty()](decoratorview-1.md#isdirty) | Checks whether or not the view is dirty - i.e. requires a new layout with a full [Measure(Vector2)](../iview.md#measurevector2).<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [Measure(Vector2)](#measurevector2) | Performs layout on this view, updating its [OuterSize](../iview.md#outersize), [ActualBounds](../iview.md#actualbounds) and [ContentBounds](../iview.md#contentbounds), and arranging any children in their respective positions.<br><span class="muted" markdown>(Overrides [DecoratorView&lt;T&gt;](decoratorview-1.md).[Measure(Vector2)](decoratorview-1.md#measurevector2))</span> | 
| [OnButtonPress(ButtonEventArgs)](decoratorview-1.md#onbuttonpressbuttoneventargs) | Called when a button press is received while this view is in the focus path.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [OnClick(ClickEventArgs)](decoratorview-1.md#onclickclickeventargs) | Called when a click is received within this view's bounds.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [OnDrag(PointerEventArgs)](decoratorview-1.md#ondragpointereventargs) | Called when the view is being dragged (mouse moved while left button held).<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [OnDrop(PointerEventArgs)](decoratorview-1.md#ondroppointereventargs) | Called when the mouse button is released after at least one [OnDrag(PointerEventArgs)](../iview.md#ondragpointereventargs).<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [OnLayout()](decoratorview-1.md#onlayout) | Runs whenever layout occurs as a result of the UI elements changing.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [OnPointerMove(PointerMoveEventArgs)](decoratorview-1.md#onpointermovepointermoveeventargs) | Called when a pointer movement related to this view occurs.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [OnPropertyChanged(PropertyChangedEventArgs)](decoratorview-1.md#onpropertychangedpropertychangedeventargs) | Raises the [PropertyChanged](decoratorview-1.md#propertychanged) event.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [OnPropertyChanged(string)](decoratorview-1.md#onpropertychangedstring) | Raises the [PropertyChanged](decoratorview-1.md#propertychanged) event.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [OnUpdate(TimeSpan)](decoratorview-1.md#onupdatetimespan) | Runs on every update tick.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [OnWheel(WheelEventArgs)](#onwheelwheeleventargs) | Called when a wheel event is received within this view's bounds.<br><span class="muted" markdown>(Overrides [DecoratorView&lt;T&gt;](decoratorview-1.md).[OnWheel(WheelEventArgs)](decoratorview-1.md#onwheelwheeleventargs))</span> | 
| [RegisterDecoratedProperty&lt;TValue&gt;(DecoratedProperty&lt;T, TValue&gt;)](decoratorview-1.md#registerdecoratedpropertytvaluedecoratedpropertyt-tvalue) | Registers a [DecoratedProperty&lt;T, TValue&gt;](decoratorview-1.decoratedproperty-1.md).<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [ScrollIntoView(IEnumerable&lt;ViewChild&gt;, Vector2)](decoratorview-1.md#scrollintoviewienumerableviewchild-vector2) | Attempts to scroll the specified target into view, including all of its ancestors, if not fully in view.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 

### Events

 | Name | Description |
| --- | --- |
| [ButtonPress](decoratorview-1.md#buttonpress) | Event raised when any button on any input device is pressed.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [Click](decoratorview-1.md#click) | Event raised when the view receives a click initiated from any button.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [Drag](decoratorview-1.md#drag) | Event raised when the view is being dragged using the mouse.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [DragEnd](decoratorview-1.md#dragend) | Event raised when mouse dragging is stopped, i.e. when the button is released. Always raised after the last [Drag](../iview.md#drag), and only once per drag operation.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [DragStart](decoratorview-1.md#dragstart) | Event raised when mouse dragging is first activated. Always raised before the first [Drag](../iview.md#drag), and only once per drag operation.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [LeftClick](decoratorview-1.md#leftclick) | Event raised when the view receives a click initiated from the left mouse button, or the controller's action button (A).<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [PointerEnter](decoratorview-1.md#pointerenter) | Event raised when the pointer enters the view.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [PointerLeave](decoratorview-1.md#pointerleave) | Event raised when the pointer exits the view.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [PointerMove](decoratorview-1.md#pointermove) | Event raised when the pointer moves within the view.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [PropertyChanged](decoratorview-1.md#propertychanged) | <span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [RightClick](decoratorview-1.md#rightclick) | Event raised when the view receives a click initiated from the right mouse button, or the controller's tool-use button (X).<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [Wheel](decoratorview-1.md#wheel) | Event raised when the scroll wheel moves.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 

## Details

### Constructors

#### ScrollableFrameView()



```cs
public ScrollableFrameView();
```

-----

### Properties

#### Content

The primary content, which displays inside the menu frame and is clipped/scrollable.

```cs
public StardewUI.IView Content { get; set; }
```

##### Property Value

[IView](../iview.md)

-----

#### ContentLayout

Layout parameters to apply to the actual [ScrollContainer](scrollcontainer.md) containing the [Content](scrollableframeview.md#content).

```cs
public StardewUI.Layout.LayoutParameters ContentLayout { get; set; }
```

##### Property Value

[LayoutParameters](../layout/layoutparameters.md)

##### Remarks

The scroll container sits between the [Content](scrollableframeview.md#content) and the outer frame. By default it is set to stretch to the outer layout dimensions, but can be modified to e.g. fit width to content.

-----

#### Footer

Optional footer to display below the [Content](scrollableframeview.md#content).

```cs
public StardewUI.IView Footer { get; set; }
```

##### Property Value

[IView](../iview.md)

##### Remarks

Footer layout can be any arbitrary size and will not push up the [Content](scrollableframeview.md#content). However, footers wider than the `Content` may cause problems.

-----

#### FrameLayout

Layout parameters to apply to the frame surrounding the [Content](scrollableframeview.md#content).

```cs
public StardewUI.Layout.LayoutParameters FrameLayout { get; set; }
```

##### Property Value

[LayoutParameters](../layout/layoutparameters.md)

##### Remarks

Determines the size of the scrollable area and should generally be one of the [FixedSize(Point)](../layout/layoutparameters.md#fixedsizepoint) overloads, or at least have a fixed [Height](../layout/layoutparameters.md#height).

-----

#### Sidebar

Optional content to display to the left of the [Content](scrollableframeview.md#content) frame.

```cs
public StardewUI.IView Sidebar { get; set; }
```

##### Property Value

[IView](../iview.md)

##### Remarks

Typically used for navigation or other contextual info. Max width is constrained to [SidebarWidth](scrollableframeview.md#sidebarwidth) regardless of layout.

-----

#### SidebarWidth

Maximum width of the sidebar area.

```cs
public int SidebarWidth { get; set; }
```

##### Property Value

[Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)

##### Remarks

To keep the primary content centered, the same dimension must be applied to the scrollbar's container, so the width must be specified ahead of time. This acts as a maximum width; the actual sidebar does not have to fill this space (it will be right-aligned in that case), but larger views may clip or overflow.

-----

#### Title

Title to display above the [Content](scrollableframeview.md#content).

```cs
public string Title { get; set; }
```

##### Property Value

[string](https://learn.microsoft.com/en-us/dotnet/api/system.string)

##### Remarks

All titles are displayed as a [Banner](banner.md).

-----

### Methods

#### CreateView()

Creates and returns the root view.

```cs
protected override StardewUI.IView CreateView();
```

##### Returns

[IView](../iview.md)

-----

#### Measure(Vector2)

Performs layout on this view, updating its [OuterSize](../iview.md#outersize), [ActualBounds](../iview.md#actualbounds) and [ContentBounds](../iview.md#contentbounds), and arranging any children in their respective positions.

```cs
public override bool Measure(Microsoft.Xna.Framework.Vector2 availableSize);
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

#### OnWheel(WheelEventArgs)

Called when a wheel event is received within this view's bounds.

```cs
public override void OnWheel(StardewUI.Events.WheelEventArgs e);
```

##### Parameters

**`e`** &nbsp; [WheelEventArgs](../events/wheeleventargs.md)  
The event data.

-----

