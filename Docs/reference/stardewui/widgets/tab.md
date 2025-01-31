---
title: Tab
description: A view with tab appearance, used to navigate sections of a larger complex menu.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class Tab

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Widgets](index.md)  
Assembly: StardewUI.dll  

</div>

A view with tab appearance, used to navigate sections of a larger complex menu.

```cs
[StardewUI.GenerateDescriptor]
public class Tab : StardewUI.Widgets.ComponentView<T>
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ [DecoratorView&lt;T&gt;](decoratorview-1.md) ⇦ [ComponentView&lt;T&gt;](componentview-1.md) ⇦ Tab

## Remarks

Tabs activate when clicked; multiple tabs can be assigned to the same [Group](tab.md#group), each with a unique [GroupKey](tab.md#groupkey), in order to deactivate other tabs when any one tab is activated.

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [Tab()](#tab) |  | 

### Properties

 | Name | Description |
| --- | --- |
| [Active](#active) | Whether or not the tab is considered active (selected). | 
| [ActiveOffset](#activeoffset) | The drawing offset to apply when the tab is [Active](tab.md#active). | 
| [ActualBounds](decoratorview-1.md#actualbounds) | The bounds of this view relative to the origin (0, 0).<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [Background](#background) | Background image to draw behind the tab's [Content](tab.md#content), which provides the tab appearance. | 
| [BackgroundRotation](#backgroundrotation) | Rotation of the [Background](tab.md#background); does not apply to [Content](tab.md#content). | 
| [ClipOrigin](decoratorview-1.md#cliporigin) | Origin position for the [ClipSize](../iview.md#clipsize).<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [ClipSize](decoratorview-1.md#clipsize) | Size of the clipping rectangle, outside which content will not be displayed.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [Content](#content) | Content to draw inside the tab's border. | 
| [ContentBounds](decoratorview-1.md#contentbounds) | The true bounds of this view's content; i.e. [ActualBounds](../iview.md#actualbounds) excluding margins.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [ContentMargin](#contentmargin) | Margin to apply to the frame containing the [Content](tab.md#content), i.e. distance between the tab's visual border and the inner image, text, etc. | 
| [FloatingBounds](decoratorview-1.md#floatingbounds) | Contains the bounds of all floating elements in this view tree, including the current view and all descendants.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [Group](#group) | The selection group, if any, to which this tab belongs. | 
| [GroupKey](#groupkey) | The unique key per [Group](tab.md#group) that identifies this tab. | 
| [IsFocusable](decoratorview-1.md#isfocusable) | Whether or not the view can receive controller focus, i.e. the stick/d-pad controlled cursor can move to this view. Not generally applicable for mouse controls.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [Layout](decoratorview-1.md#layout) | The current layout parameters, which determine how [Measure(Vector2)](../iview.md#measurevector2) will behave.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [Name](decoratorview-1.md#name) | Simple name for this view, used in log/debug output; does not affect behavior.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [Opacity](decoratorview-1.md#opacity) | Opacity (alpha level) of the view.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [OuterSize](decoratorview-1.md#outersize) | The true computed layout size resulting from a single [Measure(Vector2)](../iview.md#measurevector2) pass.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [PointerEventsEnabled](decoratorview-1.md#pointereventsenabled) | Whether this view should receive pointer events like [Click](../iview.md#click) or [Drag](../iview.md#drag).<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [PointerStyle](decoratorview-1.md#pointerstyle) | Pointer style to use when this view is hovered.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [ScrollWithChildren](decoratorview-1.md#scrollwithchildren) | If set to an axis, specifies that when any child of the view is scrolled into view (using [ScrollIntoView(IEnumerable&lt;ViewChild&gt;, Vector2)](../iview.md#scrollintoviewienumerableviewchild-vector2)), then this entire view should be scrolled along with it.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [Tags](decoratorview-1.md#tags) | The user-defined tags for this view.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [Tooltip](decoratorview-1.md#tooltip) | Tooltip data to display on hover, if any.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [Transform](decoratorview-1.md#transform) | Local transformation to apply to this view, including any children and floating elements.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [TransformOrigin](decoratorview-1.md#transformorigin) | Relative origin position for any [Transform](../iview.md#transform) on this view.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [View](componentview-1.md#view) | <span class="muted" markdown>(Inherited from [ComponentView&lt;T&gt;](componentview-1.md))</span> | 
| [Visibility](decoratorview-1.md#visibility) | Drawing visibility for this view.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [ZIndex](decoratorview-1.md#zindex) | Z order for this view within its direct parent. Higher indices draw later (on top).<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 

### Methods

 | Name | Description |
| --- | --- |
| [ContainsPoint(Vector2)](decoratorview-1.md#containspointvector2) | Checks if a given point, relative to the view's origin, is within its bounds.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [CreateView()](#createview) | Creates and returns the root view.<br><span class="muted" markdown>(Overrides [ComponentView&lt;T&gt;](componentview-1.md).[CreateView()](componentview-1.md#createview))</span> | 
| [Dispose()](#dispose) | <span class="muted" markdown>(Overrides [DecoratorView&lt;T&gt;](decoratorview-1.md).[Dispose()](decoratorview-1.md#dispose))</span> | 
| [Draw(ISpriteBatch)](#drawispritebatch) | Draws the content for this view.<br><span class="muted" markdown>(Overrides [DecoratorView&lt;T&gt;](decoratorview-1.md).[Draw(ISpriteBatch)](decoratorview-1.md#drawispritebatch))</span> | 
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
| [OnClick(ClickEventArgs)](#onclickclickeventargs) | Called when a click is received within this view's bounds.<br><span class="muted" markdown>(Overrides [DecoratorView&lt;T&gt;](decoratorview-1.md).[OnClick(ClickEventArgs)](decoratorview-1.md#onclickclickeventargs))</span> | 
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
| [Activate](#activate) | Event raised when [Active](tab.md#active) becomes `true`. | 
| [ButtonPress](decoratorview-1.md#buttonpress) | Event raised when any button on any input device is pressed.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [ButtonRepeat](decoratorview-1.md#buttonrepeat) | Event raised when a button is being held while the view is in focus, and has been held long enough since the initial [ButtonPress](../iview.md#buttonpress) or the previous `ButtonRepeat` to trigger a repeated press.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [Click](decoratorview-1.md#click) | Event raised when the view receives a click initiated from any button.<br><span class="muted" markdown>(Inherited from [DecoratorView&lt;T&gt;](decoratorview-1.md))</span> | 
| [Deactivate](#deactivate) | Event raised when [Active](tab.md#active) becomes `false`. | 
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

#### Tab()



```cs
public Tab();
```

-----

### Properties

#### Active

Whether or not the tab is considered active (selected).

```cs
public bool Active { get; set; }
```

##### Property Value

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

##### Remarks

Active tabs have an offset appearance, normally used to indicate their "pressed" status in relation to other, neighboring tabs. The offset can be adjusted with [ActiveOffset](tab.md#activeoffset).

-----

#### ActiveOffset

The drawing offset to apply when the tab is [Active](tab.md#active).

```cs
public Microsoft.Xna.Framework.Vector2 ActiveOffset { get; set; }
```

##### Property Value

[Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)

-----

#### Background

Background image to draw behind the tab's [Content](tab.md#content), which provides the tab appearance.

```cs
public StardewUI.Graphics.Sprite Background { get; set; }
```

##### Property Value

[Sprite](../graphics/sprite.md)

-----

#### BackgroundRotation

Rotation of the [Background](tab.md#background); does not apply to [Content](tab.md#content).

```cs
public StardewUI.Graphics.SimpleRotation? BackgroundRotation { get; set; }
```

##### Property Value

[Nullable](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1)<[SimpleRotation](../graphics/simplerotation.md)>

-----

#### Content

Content to draw inside the tab's border.

```cs
public StardewUI.IView Content { get; set; }
```

##### Property Value

[IView](../iview.md)

-----

#### ContentMargin

Margin to apply to the frame containing the [Content](tab.md#content), i.e. distance between the tab's visual border and the inner image, text, etc.

```cs
public StardewUI.Layout.Edges ContentMargin { get; set; }
```

##### Property Value

[Edges](../layout/edges.md)

##### Remarks

When using the default [Background](tab.md#background), this is automatically set up to match its border size. If a different background is used, the margin may need to be adjusted.

-----

#### Group

The selection group, if any, to which this tab belongs.

```cs
public StardewUI.Widgets.SelectionGroup Group { get; set; }
```

##### Property Value

[SelectionGroup](selectiongroup.md)

##### Remarks

Assigning multiple tabs to the same group guarantees that only one can be [Active](tab.md#active) at a time. If this tab becomes active, any previously-active tab will become inactive. 

 To participate in the group, a non-empty [GroupKey](tab.md#groupkey) must also be specified.

-----

#### GroupKey

The unique key per [Group](tab.md#group) that identifies this tab.

```cs
public string GroupKey { get; set; }
```

##### Property Value

[string](https://learn.microsoft.com/en-us/dotnet/api/system.string)

-----

### Methods

#### CreateView()

Creates and returns the root view.

```cs
protected override StardewUI.Widgets.Panel CreateView();
```

##### Returns

[Panel](panel.md)

-----

#### Dispose()



```cs
public override void Dispose();
```

-----

#### Draw(ISpriteBatch)

Draws the content for this view.

```cs
public override void Draw(StardewUI.Graphics.ISpriteBatch b);
```

##### Parameters

**`b`** &nbsp; [ISpriteBatch](../graphics/ispritebatch.md)  
Sprite batch to hold the drawing output.

##### Remarks

No positional argument is included because [ISpriteBatch](../graphics/ispritebatch.md) handles its own transformations; the top-left coordinates of this view are always (0, 0).

-----

#### OnClick(ClickEventArgs)

Called when a click is received within this view's bounds.

```cs
public override void OnClick(StardewUI.Events.ClickEventArgs e);
```

##### Parameters

**`e`** &nbsp; [ClickEventArgs](../events/clickeventargs.md)  
The event data.

-----

### Events

#### Activate

Event raised when [Active](tab.md#active) becomes `true`.

```cs
public event System.EventHandler? Activate;
```

##### Event Type

[EventHandler](https://learn.microsoft.com/en-us/dotnet/api/system.eventhandler)

-----

#### Deactivate

Event raised when [Active](tab.md#active) becomes `false`.

```cs
public event System.EventHandler? Deactivate;
```

##### Event Type

[EventHandler](https://learn.microsoft.com/en-us/dotnet/api/system.eventhandler)

-----

