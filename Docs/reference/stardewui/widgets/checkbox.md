---
title: CheckBox
description: A togglable checkbox.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class CheckBox

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Widgets](index.md)  
Assembly: StardewUI.dll  

</div>

A togglable checkbox.

```cs
[StardewUI.GenerateDescriptor]
public class CheckBox : StardewUI.Widgets.ComponentView<T>
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ [DecoratorView&lt;T&gt;](decoratorview-1.md) ⇦ [ComponentView&lt;T&gt;](componentview-1.md) ⇦ CheckBox

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [CheckBox()](#checkbox) |  | 

### Properties

 | Name | Description |
| --- | --- |
| [ActualBounds](decoratorview-1.md#actualbounds) | The bounds of this view relative to the origin (0, 0).<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [CheckedSprite](#checkedsprite) | Sprite to display when the box is checked, if not using the default. | 
| [ClipOrigin](decoratorview-1.md#cliporigin) | Origin position for the [ClipSize](../iview.md#clipsize).<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [ClipSize](decoratorview-1.md#clipsize) | Size of the clipping rectangle, outside which content will not be displayed.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [ContentBounds](decoratorview-1.md#contentbounds) | The true bounds of this view's content; i.e. [ActualBounds](../iview.md#actualbounds) excluding margins.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [FloatingBounds](decoratorview-1.md#floatingbounds) | Contains the bounds of all floating elements in this view tree, including the current view and all descendants.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [IsChecked](#ischecked) | Whether or not the box is checked. | 
| [IsFocusable](decoratorview-1.md#isfocusable) | Whether or not the view can receive controller focus, i.e. the stick/d-pad controlled cursor can move to this view. Not generally applicable for mouse controls.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [LabelColor](#labelcolor) | Color with which to render any [LabelText](checkbox.md#labeltext). | 
| [LabelText](#labeltext) | Optional label text to be displayed to the right of the checkbox image. | 
| [Layout](decoratorview-1.md#layout) | The current layout parameters, which determine how [Measure(Vector2)](../iview.md#measurevector2) will behave.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [Margin](#margin) | Margins (whitespace outside border) for this view. | 
| [Name](decoratorview-1.md#name) | Simple name for this view, used in log/debug output; does not affect behavior.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [Opacity](#opacity) | Opacity (alpha level) of the view.<br><span class="muted" markdown>(Overrides [DecoratorView&lt;T&gt;](decoratorview-1.md).`get_Opacity()`)</span> | 
| [OuterSize](decoratorview-1.md#outersize) | The true computed layout size resulting from a single [Measure(Vector2)](../iview.md#measurevector2) pass.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [PointerEventsEnabled](decoratorview-1.md#pointereventsenabled) | Whether this view should receive pointer events like [Click](../iview.md#click) or [Drag](../iview.md#drag).<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [PointerStyle](decoratorview-1.md#pointerstyle) | Pointer style to use when this view is hovered.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [ScrollWithChildren](decoratorview-1.md#scrollwithchildren) | If set to an axis, specifies that when any child of the view is scrolled into view (using [ScrollIntoView(IEnumerable&lt;ViewChild&gt;, Vector2)](../iview.md#scrollintoviewienumerableviewchild-vector2)), then this entire view should be scrolled along with it.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [Tags](decoratorview-1.md#tags) | The user-defined tags for this view.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [Tooltip](decoratorview-1.md#tooltip) | Tooltip data to display on hover, if any.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [Transform](decoratorview-1.md#transform) | Local transformation to apply to this view, including any children and floating elements.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [TransformOrigin](decoratorview-1.md#transformorigin) | Relative origin position for any [Transform](../iview.md#transform) on this view.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [UncheckedSprite](#uncheckedsprite) | Sprite to display when the box is unchecked, if not using the default. | 
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
| [GetChildAt(Vector2, Boolean, Boolean)](decoratorview-1.md#getchildatvector2-bool-bool) | Finds the child at a given position.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [GetChildPosition(IView)](decoratorview-1.md#getchildpositioniview) | Computes or retrieves the position of a given direct child.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [GetChildren(Boolean)](decoratorview-1.md#getchildrenbool) | Gets the current children of this view.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [GetChildrenAt(Vector2)](decoratorview-1.md#getchildrenatvector2) | Finds all children at a given position.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [GetDefaultFocusChild()](decoratorview-1.md#getdefaultfocuschild) | Gets the direct child that should contain cursor focus when a menu or overlay containing this view is first opened.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [HasOutOfBoundsContent()](decoratorview-1.md#hasoutofboundscontent) | Checks if the view has content or elements that are all or partially outside the [ActualBounds](../iview.md#actualbounds).<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [IsDirty()](decoratorview-1.md#isdirty) | Checks whether or not the view is dirty - i.e. requires a new layout with a full [Measure(Vector2)](../iview.md#measurevector2).<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [IsVisible(Vector2?)](decoratorview-1.md#isvisiblevector2) | Checks if the view is effectively visible, i.e. if it has anything to draw.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [Measure(Vector2)](decoratorview-1.md#measurevector2) | Performs layout on this view, updating its [OuterSize](../iview.md#outersize), [ActualBounds](../iview.md#actualbounds) and [ContentBounds](../iview.md#contentbounds), and arranging any children in their respective positions.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [OnButtonPress(ButtonEventArgs)](decoratorview-1.md#onbuttonpressbuttoneventargs) | Called when a button press is received while this view is in the focus path.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [OnButtonRepeat(ButtonEventArgs)](decoratorview-1.md#onbuttonrepeatbuttoneventargs) | Called when a button press is first received, and at recurring intervals thereafter, for as long as the button is held and this view remains in the focus path.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [OnClick(ClickEventArgs)](decoratorview-1.md#onclickclickeventargs) | Called when a click is received within this view's bounds.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [OnDrag(PointerEventArgs)](decoratorview-1.md#ondragpointereventargs) | Called when the view is being dragged (mouse moved while left button held).<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [OnDrop(PointerEventArgs)](decoratorview-1.md#ondroppointereventargs) | Called when the mouse button is released after at least one [OnDrag(PointerEventArgs)](../iview.md#ondragpointereventargs).<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [OnLayout()](decoratorview-1.md#onlayout) | Runs whenever layout occurs as a result of the UI elements changing.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [OnPointerMove(PointerMoveEventArgs)](decoratorview-1.md#onpointermovepointermoveeventargs) | Called when a pointer movement related to this view occurs.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [OnPropertyChanged(PropertyChangedEventArgs)](decoratorview-1.md#onpropertychangedpropertychangedeventargs) | Raises the [PropertyChanged](decoratorview-1.md#propertychanged) event.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [OnPropertyChanged(string)](decoratorview-1.md#onpropertychangedstring) | Raises the [PropertyChanged](decoratorview-1.md#propertychanged) event.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [OnUpdate(TimeSpan)](decoratorview-1.md#onupdatetimespan) | Runs on every update tick.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [OnWheel(WheelEventArgs)](decoratorview-1.md#onwheelwheeleventargs) | Called when a wheel event is received within this view's bounds.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [RegisterDecoratedProperty&lt;TValue&gt;(DecoratedProperty&lt;T, TValue&gt;)](decoratorview-1.md#registerdecoratedpropertytvaluedecoratedpropertyt-tvalue) | Registers a [DecoratedProperty&lt;T, TValue&gt;](decoratorview-1.decoratedproperty-1.md).<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [ScrollIntoView(IEnumerable&lt;ViewChild&gt;, Vector2)](decoratorview-1.md#scrollintoviewienumerableviewchild-vector2) | Attempts to scroll the specified target into view, including all of its ancestors, if not fully in view.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 

### Events

 | Name | Description |
| --- | --- |
| [ButtonPress](decoratorview-1.md#buttonpress) | Event raised when any button on any input device is pressed.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [ButtonRepeat](decoratorview-1.md#buttonrepeat) | Event raised when a button is being held while the view is in focus, and has been held long enough since the initial [ButtonPress](../iview.md#buttonpress) or the previous `ButtonRepeat` to trigger a repeated press.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [Change](#change) | Event raised when the checked state changes. | 
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

#### CheckBox()



```cs
public CheckBox();
```

-----

### Properties

#### CheckedSprite

Sprite to display when the box is checked, if not using the default.

```cs
public StardewUI.Graphics.Sprite CheckedSprite { get; set; }
```

##### Property Value

[Sprite](../graphics/sprite.md)

-----

#### IsChecked

Whether or not the box is checked.

```cs
public bool IsChecked { get; set; }
```

##### Property Value

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

-----

#### LabelColor

Color with which to render any [LabelText](checkbox.md#labeltext).

```cs
public Microsoft.Xna.Framework.Color LabelColor { get; set; }
```

##### Property Value

[Color](https://docs.monogame.net/api/Microsoft.Xna.Framework.Color.html)

-----

#### LabelText

Optional label text to be displayed to the right of the checkbox image.

```cs
public string LabelText { get; set; }
```

##### Property Value

[string](https://learn.microsoft.com/en-us/dotnet/api/system.string)

##### Remarks

The label text is clickable as part of the checkbox, but does not receive focus.

-----

#### Margin

Margins (whitespace outside border) for this view.

```cs
public StardewUI.Layout.Edges Margin { get; set; }
```

##### Property Value

[Edges](../layout/edges.md)

-----

#### Opacity

Opacity (alpha level) of the view.

```cs
public float Opacity { get; set; }
```

##### Property Value

[Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)

##### Remarks

Affects this view and all descendants; used to control opacity of an entire control or layout area.

-----

#### UncheckedSprite

Sprite to display when the box is unchecked, if not using the default.

```cs
public StardewUI.Graphics.Sprite UncheckedSprite { get; set; }
```

##### Property Value

[Sprite](../graphics/sprite.md)

-----

### Methods

#### CreateView()

Creates and returns the root view.

```cs
protected override StardewUI.Widgets.Lane CreateView();
```

##### Returns

[Lane](lane.md)

-----

### Events

#### Change

Event raised when the checked state changes.

```cs
public event EventHandler<System.EventArgs>? Change;
```

##### Event Type

[EventHandler](https://learn.microsoft.com/en-us/dotnet/api/system.eventhandler-1)<[EventArgs](https://learn.microsoft.com/en-us/dotnet/api/system.eventargs)>

-----

