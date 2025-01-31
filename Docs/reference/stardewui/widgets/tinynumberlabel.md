---
title: TinyNumberLabel
description: Renders a single-line numeric label using custom digit sprites.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class TinyNumberLabel

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Widgets](index.md)  
Assembly: StardewUI.dll  

</div>

Renders a single-line numeric label using custom digit sprites.

```cs
[StardewUI.GenerateDescriptor]
public class TinyNumberLabel : StardewUI.View
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ [View](../view.md) ⇦ TinyNumberLabel

## Remarks

Corresponds to drawTinyDigits(Int32, SpriteBatch, Vector2, Single, Single, Color). 

 For this widget type, [Scale](tinynumberlabel.md#scale)** does** affect layout, and the size of the rendered text is entirely based on the [DigitSprites](tinynumberlabel.md#digitsprites) and cumulative scale which is effectively treated like a font size. If the view's [Layout](../view.md#layout) uses any non-content-based dimensions, it will affect the box size as expected but will not change the rendered text; the text is not scaled to the layout bounds.

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [TinyNumberLabel()](#tinynumberlabel) |  | 

### Properties

 | Name | Description |
| --- | --- |
| [ActualBounds](../view.md#actualbounds) | The bounds of this view relative to the origin (0, 0).<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [BorderSize](../view.md#bordersize) | The layout size (not edge thickness) of the entire drawn area including the border, i.e. the [InnerSize](../view.md#innersize) plus any borders defined in [GetBorderThickness()](../view.md#getborderthickness). Does not include the [Margin](../view.md#margin).<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [ClipOrigin](../view.md#cliporigin) | Origin position for the [ClipSize](../iview.md#clipsize).<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [ClipSize](../view.md#clipsize) | Size of the clipping rectangle, outside which content will not be displayed.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [ContentBounds](../view.md#contentbounds) | The true bounds of this view's content; i.e. [ActualBounds](../iview.md#actualbounds) excluding margins.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [ContentSize](../view.md#contentsize) | The size of the view's content, which is drawn inside the padding. Subclasses set this in their [OnMeasure(Vector2)](../view.md#onmeasurevector2) method and padding, margins, etc. are handled automatically.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [DigitSprites](#digitsprites) | The sprites for each individual digit, with the index corresponding to the digit itself (element 0 for digit '0', element 4 for digit '4', etc.). This must have exactly 10 elements. | 
| [Draggable](../view.md#draggable) | Whether or not this view should fire drag events such as [DragStart](../view.md#dragstart) and [Drag](../view.md#drag).<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [FloatingBounds](../view.md#floatingbounds) | Contains the bounds of all floating elements in this view tree, including the current view and all descendants.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [FloatingElements](../view.md#floatingelements) | The floating elements to display relative to this view.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [Focusable](../view.md#focusable) | Whether or not the view should be able to receive focus. Applies only to this specific view, not its children.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [HandlesOpacity](#handlesopacity) | Whether the specific view type handles its own opacity.<br><span class="muted" markdown>(Overrides [View](../view.md).`get_HandlesOpacity()`)</span> | 
| [InnerSize](../view.md#innersize) | The size allocated to the entire area inside the border, i.e. [ContentSize](../view.md#contentsize) plus any [Padding](../view.md#padding). Does not include border or [Margin](../view.md#margin).<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [IsFocusable](../view.md#isfocusable) | Whether or not the view can receive controller focus, i.e. the stick/d-pad controlled cursor can move to this view. Not generally applicable for mouse controls.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [LastAvailableSize](../view.md#lastavailablesize) | The most recent size used in a [Measure(Vector2)](../view.md#measurevector2) pass. Used for additional dirty checks.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [Layout](../view.md#layout) | Layout settings for this view; determines how its dimensions will be computed.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [LayoutOffset](../view.md#layoutoffset) | Pixel offset of the view's content, which is applied to all pointer events and child queries.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [Margin](../view.md#margin) | Margins (whitespace outside border) for this view.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [Name](../view.md#name) | Simple name for this view, used in log/debug output; does not affect behavior.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [Number](#number) | The number to display. | 
| [Opacity](../view.md#opacity) | Opacity (alpha level) of the view.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [OuterSize](../view.md#outersize) | The size of the entire area occupied by this view including margins, border and padding.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [Padding](../view.md#padding) | Padding (whitespace inside border) for this view.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [PointerEventsEnabled](../view.md#pointereventsenabled) | Whether this view should receive pointer events like [Click](../view.md#click) or [Drag](../view.md#drag).<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [PointerStyle](../view.md#pointerstyle) | Pointer style to use when this view is hovered.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [Scale](#scale) | Scale to draw the digits, relative to their original pixel size. | 
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
| [FindFocusableDescendant(Vector2, Direction)](../view.md#findfocusabledescendantvector2-direction) | Searches for a focusable child within this view that is reachable in the specified `direction`, and returns a result containing the view and search path if found.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [FocusSearch(Vector2, Direction)](../view.md#focussearchvector2-direction) | Finds the next focusable component in a given direction that does _not_ overlap with a current position.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [GetBorderThickness()](../view.md#getborderthickness) | Measures the thickness of each edge of the border, if the view has a border.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [GetChildAt(Vector2, Boolean, Boolean)](../view.md#getchildatvector2-bool-bool) | Finds the child at a given position.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [GetChildPosition(IView)](../view.md#getchildpositioniview) | Computes or retrieves the position of a given direct child.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [GetChildren(Boolean)](../view.md#getchildrenbool) | Gets the current children of this view.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [GetChildrenAt(Vector2)](../view.md#getchildrenatvector2) | Finds all children at a given position.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [GetDefaultFocusChild()](../view.md#getdefaultfocuschild) | Gets the direct child that should contain cursor focus when a menu or overlay containing this view is first opened.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [GetLocalChildren()](../view.md#getlocalchildren) | Gets the view's children with positions relative to the content area.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [GetLocalChildrenAt(Vector2)](../view.md#getlocalchildrenatvector2) | Searches for all views at a given position relative to the content area.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [HasOutOfBoundsContent()](../view.md#hasoutofboundscontent) | Checks if the view has content or elements that are all or partially outside the [ActualBounds](../iview.md#actualbounds).<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
| [HasOwnContent()](../view.md#hasowncontent) | Checks if this view displays its own content, independent of any floating elements or children.<br><span class="muted" markdown>(Inherited from [View](../view.md))</span> | 
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

#### TinyNumberLabel()



```cs
public TinyNumberLabel();
```

-----

### Properties

#### DigitSprites

The sprites for each individual digit, with the index corresponding to the digit itself (element 0 for digit '0', element 4 for digit '4', etc.). This must have exactly 10 elements.

```cs
public System.Collections.Generic.IReadOnlyList<StardewUI.Graphics.Sprite> DigitSprites { get; set; }
```

##### Property Value

[IReadOnlyList](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlylist-1)<[Sprite](../graphics/sprite.md)>

-----

#### HandlesOpacity

Whether the specific view type handles its own opacity.

```cs
protected bool HandlesOpacity { get; }
```

##### Property Value

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

##### Remarks

Subclasses can override this to provide their own, typically better optimized version of opacity; i.e. a basic text or image view could simply multiply its own background/foreground colors without requiring multiple render targets to handle the blending. 

 Any [FloatingElements](../view.md#floatingelements) will still use the default opacity implementation.

-----

#### Number

The number to display.

```cs
public int Number { get; set; }
```

##### Property Value

[Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)

-----

#### Scale

Scale to draw the digits, relative to their original pixel size.

```cs
public float Scale { get; set; }
```

##### Property Value

[Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)

-----

### Methods

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

