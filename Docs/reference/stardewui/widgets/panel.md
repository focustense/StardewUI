---
title: Panel
description: A layout view whose children all overlap the same boundaries.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class Panel

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Widgets](index.md)  
Assembly: StardewUI.dll  

</div>

A layout view whose children all overlap the same boundaries.

```cs
[StardewUI.GenerateDescriptor]
public class Panel : StardewUI.View
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ [View](../view.md) ⇦ Panel

## Remarks

A panel's content size (i.e. if any dimensions are [Content](../layout/lengthtype.md#content)) is always equal to the largest child; alignment applies to each child individually, and children are drawn according to their [ZIndex](../iview.md#zindex) first and then their order in [Children](panel.md#children). 

 Children can be positioned more precisely using their [Margin](../view.md#margin) and [Padding](../view.md#padding) for standard view types, or drawing at non-origin positions for custom [IView](../iview.md) implementations. 

 A common use of panels is to draw overlapping images, in cases where a [Frame](frame.md) doesn't really make sense, e.g. there is no explicit "background" or "border", or if there are more than 2 layers to draw.

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [Panel()](#panel) |  | 

### Properties

 | Name | Description |
| --- | --- |
| [ActualBounds](../view.md#actualbounds) | The bounds of this view relative to the origin (0, 0).<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [BorderSize](../view.md#bordersize) | The layout size (not edge thickness) of the entire drawn area including the border, i.e. the [InnerSize](../view.md#innersize) plus any borders defined in [GetBorderThickness()](../view.md#getborderthickness). Does not include the [Margin](../view.md#margin).<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [Children](#children) | Child views to display in this layout. | 
| [ClipOrigin](../view.md#cliporigin) | Origin position for the [ClipSize](../iview.md#clipsize).<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [ClipSize](../view.md#clipsize) | Size of the clipping rectangle, outside which content will not be displayed.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [ContentBounds](../view.md#contentbounds) | The true bounds of this view's content; i.e. [ActualBounds](../iview.md#actualbounds) excluding margins.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [ContentSize](../view.md#contentsize) | The size of the view's content, which is drawn inside the padding. Subclasses set this in their [OnMeasure(Vector2)](../view.md#onmeasurevector2) method and padding, margins, etc. are handled automatically.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [Draggable](../view.md#draggable) | Whether or not this view should fire drag events such as [DragStart](../view.md#dragstart) and [Drag](../view.md#drag).<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [FloatingBounds](../view.md#floatingbounds) | Contains the bounds of all floating elements in this view tree, including the current view and all descendants.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [FloatingElements](../view.md#floatingelements) | The floating elements to display relative to this view.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [Focusable](../view.md#focusable) | Whether or not the view should be able to receive focus. Applies only to this specific view, not its children.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [HandlesOpacity](../view.md#handlesopacity) | Whether the specific view type handles its own opacity.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [HorizontalContentAlignment](#horizontalcontentalignment) | Specifies how to align each child in [Children](panel.md#children) horizontally within the frame's area. | 
| [InnerSize](../view.md#innersize) | The size allocated to the entire area inside the border, i.e. [ContentSize](../view.md#contentsize) plus any [Padding](../view.md#padding). Does not include border or [Margin](../view.md#margin).<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [IsFocusable](../view.md#isfocusable) | Whether or not the view can receive controller focus, i.e. the stick/d-pad controlled cursor can move to this view. Not generally applicable for mouse controls.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [LastAvailableSize](../view.md#lastavailablesize) | The most recent size used in a [Measure(Vector2)](../view.md#measurevector2) pass. Used for additional dirty checks.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [Layout](../view.md#layout) | Layout settings for this view; determines how its dimensions will be computed.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [LayoutOffset](../view.md#layoutoffset) | Pixel offset of the view's content, which is applied to all pointer events and child queries.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [Margin](../view.md#margin) | Margins (whitespace outside border) for this view.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [Name](../view.md#name) | Simple name for this view, used in log/debug output; does not affect behavior.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [Opacity](../view.md#opacity) | Opacity (alpha level) of the view.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [OuterSize](../view.md#outersize) | The size of the entire area occupied by this view including margins, border and padding.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [Padding](../view.md#padding) | Padding (whitespace inside border) for this view.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [PointerEventsEnabled](../view.md#pointereventsenabled) | Whether this view should receive pointer events like [Click](../view.md#click) or [Drag](../view.md#drag).<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [PointerStyle](../view.md#pointerstyle) | Pointer style to use when this view is hovered.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [ScrollWithChildren](../view.md#scrollwithchildren) | If set to an axis, specifies that when any child of the view is scrolled into view (using [ScrollIntoView(IEnumerable&lt;ViewChild&gt;, Vector2)](../view.md#scrollintoviewienumerableviewchild-vector2)), then this entire view should be scrolled along with it.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [Tags](../view.md#tags) | The user-defined tags for this view.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [Tooltip](../view.md#tooltip) | Localized tooltip to display on hover, if any.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [Transform](../view.md#transform) | Local transformation to apply to this view, including any children and floating elements.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [TransformOrigin](../view.md#transformorigin) | Relative origin position for any [Transform](../iview.md#transform) on this view.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [VerticalContentAlignment](#verticalcontentalignment) | Specifies how to align each child in [Children](panel.md#children) vertically within the frame's area. | 
| [Visibility](../view.md#visibility) | Visibility for this view.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [ZIndex](../view.md#zindex) | Z order for this view within its direct parent. Higher indices draw later (on top).<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 

### Methods

 | Name | Description |
| --- | --- |
| [Align(IView, Alignment, Alignment, string)](#aligniview-alignment-alignment-string) | Creates a [Panel](panel.md) that is used to align some inner content inside a parent, typically another [Panel](panel.md). | 
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
| [GetLocalChildrenAt(Vector2)](../view.md#getlocalchildrenatvector2) | Searches for all views at a given position relative to the content area.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
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
| [OnPropertyChanged(PropertyChangedEventArgs)](../view.md#onpropertychangedpropertychangedeventargs) | Raises the [PropertyChanged](../view.md#propertychanged) event.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [OnPropertyChanged(string)](../view.md#onpropertychangedstring) | Raises the [PropertyChanged](../view.md#propertychanged) event.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [OnUpdate(TimeSpan)](../view.md#onupdatetimespan) | Runs on every update tick.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [OnWheel(WheelEventArgs)](../view.md#onwheelwheeleventargs) | Called when a wheel event is received within this view's bounds.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [ResetDirty()](#resetdirty) | Resets any dirty state associated with this view.<br><span class="muted" markdown>(Overrides [View](../view.md).[ResetDirty()](../view.md#resetdirty))</span> | 
| [ScrollIntoView(IEnumerable&lt;ViewChild&gt;, Vector2)](../view.md#scrollintoviewienumerableviewchild-vector2) | Attempts to scroll the specified target into view, including all of its ancestors, if not fully in view.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
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
| [Wheel](../view.md#wheel) | Event raised when the scroll wheel moves.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 

## Details

### Constructors

#### Panel()



```cs
public Panel();
```

-----

### Properties

#### Children

Child views to display in this layout.

```cs
public System.Collections.Generic.IList<StardewUI.IView> Children { get; set; }
```

##### Property Value

[IList](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ilist-1)<[IView](../iview.md)>

-----

#### HorizontalContentAlignment

Specifies how to align each child in [Children](panel.md#children) horizontally within the frame's area.

```cs
public StardewUI.Layout.Alignment HorizontalContentAlignment { get; set; }
```

##### Property Value

[Alignment](../layout/alignment.md)

-----

#### VerticalContentAlignment

Specifies how to align each child in [Children](panel.md#children) vertically within the frame's area.

```cs
public StardewUI.Layout.Alignment VerticalContentAlignment { get; set; }
```

##### Property Value

[Alignment](../layout/alignment.md)

-----

### Methods

#### Align(IView, Alignment, Alignment, string)

Creates a [Panel](panel.md) that is used to align some inner content inside a parent, typically another [Panel](panel.md).

```cs
public static StardewUI.Widgets.Panel Align(StardewUI.IView content, StardewUI.Layout.Alignment horizontal, StardewUI.Layout.Alignment vertical, string name);
```

##### Parameters

**`content`** &nbsp; [IView](../iview.md)  
The content to align.

**`horizontal`** &nbsp; [Alignment](../layout/alignment.md)  
Horizontal alignment of the content.

**`vertical`** &nbsp; [Alignment](../layout/alignment.md)  
Vertical alignment of the content.

**`name`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
Optional name to give to the panel, for debugging.

##### Returns

[Panel](panel.md)

##### Remarks

The created panel will stretch to fill all available area, and align the `content` view within itself according to the `horizontal` and `vertical` alignments. Several [Align(IView, Alignment, Alignment, string)](panel.md#aligniview-alignment-alignment-string) helpers can be used to align different content/controls to different edges or corners of the same parent [Panel](panel.md).

-----

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

#### ResetDirty()

Resets any dirty state associated with this view.

```cs
protected override void ResetDirty();
```

##### Remarks

This is called at the end of [Measure(Vector2)](../view.md#measurevector2), so that on the next pass, all state appears clean unless it was marked dirty after the last pass completed. The default implementation is a no-op; subclasses should use it to clear any private dirty state, e.g. via [ResetDirty()](../layout/dirtytracker-1.md#resetdirty).

-----

