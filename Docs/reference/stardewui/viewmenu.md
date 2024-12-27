---
title: ViewMenu
description: Generic menu implementation based on a root IView.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class ViewMenu

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI](index.md)  
Assembly: StardewUI.dll  

</div>

Generic menu implementation based on a root [IView](iview.md).

```cs
public class ViewMenu : StardewValley.Menus.IClickableMenu, System.IDisposable
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ IClickableMenu ⇦ ViewMenu

**Implements**  
[IDisposable](https://learn.microsoft.com/en-us/dotnet/api/system.idisposable)

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [ViewMenu(Edges, Boolean)](#viewmenuedges-bool) | Initializes a new instance of [ViewMenu](viewmenu.md). | 

### Fields

 | Name | Description |
| --- | --- |
| _childMenu | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| _dependencies | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| _parentMenu | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| allClickableComponents | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| behaviorBeforeCleanup | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| closeSound | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| currentlySnappedComponent | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| destroy | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| exitFunction | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| height | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| upperRightCloseButton | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| width | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| xPositionOnScreen | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| yPositionOnScreen | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 

### Properties

 | Name | Description |
| --- | --- |
| [CloseButtonOffset](#closebuttonoffset) | Offset from the menu view's top-right edge to draw the close button, if a [CloseButtonSprite](viewmenu.md#closebuttonsprite) is also specified. | 
| [CloseButtonSprite](#closebuttonsprite) | The sprite to draw for the close button shown on the upper right. If no value is specified, then no close button will be drawn. The default behavior is to not show any close button. | 
| [DimmingAmount](#dimmingamount) | Amount of dimming between 0 and 1; i.e. opacity of the background underlay. | 
| [Gutter](#gutter) | Gets or sets the menu's gutter edges, which constrain the portion of the viewport in which any part of the menu may be drawn. | 
| Position | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| [View](#view) | The view to display with this menu. | 

### Methods

 | Name | Description |
| --- | --- |
| _ShouldAutoSnapPrioritizeAlignedElements() | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| actionOnRegionChange(Int32, Int32) | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| AddDependency() | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| [applyMovementKey(Int32)](#applymovementkeyint) | Initiates a focus search in the specified direction.<br><span class="muted" markdown>(Overrides IClickableMenu.applyMovementKey(Int32))</span> | 
| applyMovementKey(Keys) | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| [areGamePadControlsImplemented()](#aregamepadcontrolsimplemented) | Returns whether or not the menu wants **exclusive** gamepad controls.<br><span class="muted" markdown>(Overrides IClickableMenu.areGamePadControlsImplemented())</span> | 
| automaticSnapBehavior(Int32, Int32, Int32) | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| [BuildTooltip(IEnumerable&lt;ViewChild&gt;)](#buildtooltipienumerableviewchild) | Builds/formats a tooltip given the sequence of views from root to the lowest-level hovered child. | 
| cleanupBeforeExit() | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| clickAway() | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| [Close()](#close) | Closes this menu, either by removing it from the parent if it is a child menu, or removing it as the game's active menu if it is standalone. | 
| [CreateView()](#createview) | Creates the view. | 
| [CustomClose()](#customclose) | When overridden in a derived class, provides an alternative method to close the menu instead of the default logic in exitThisMenu(Boolean). | 
| customSnapBehavior(Int32, Int32, Int32) | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| [Dispose()](#dispose) |  | 
| [draw(SpriteBatch)](#drawspritebatch) | Draws the current menu content.<br><span class="muted" markdown>(Overrides IClickableMenu.draw(SpriteBatch))</span> | 
| draw(SpriteBatch, Int32, Int32, Int32) | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| drawBackground(SpriteBatch) | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| drawBorderLabel(SpriteBatch, string, SpriteFont, Int32, Int32) | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| drawHorizontalPartition(SpriteBatch, Int32, Boolean, Int32, Int32, Int32) | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| drawMouse(SpriteBatch, Boolean, Int32) | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| drawVerticalIntersectingPartition(SpriteBatch, Int32, Int32, Int32, Int32, Int32) | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| drawVerticalPartition(SpriteBatch, Int32, Boolean, Int32, Int32, Int32, Int32) | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| drawVerticalUpperIntersectingPartition(SpriteBatch, Int32, Int32, Int32, Int32, Int32) | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| emergencyShutDown() | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| exitThisMenu(Boolean) | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| exitThisMenuNoSound() | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| gamePadButtonHeld(Buttons) | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| gameWindowSizeChanged(Rectangle, Rectangle) | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| GetChildMenu() | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| [GetCloseBehavior()](#getclosebehavior) | Gets the current close behavior for the menu. | 
| getComponentWithID(Int32) | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| getCurrentlySnappedComponent() | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| [GetOriginPosition(Point, Point)](#getoriginpositionpoint-point) | Computes the origin (top left) position of the menu for a given viewport and offset. | 
| GetParentMenu() | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| HasDependencies() | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| initialize(Int32, Int32, Int32, Int32, Boolean) | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| initializeUpperRightCloseButton() | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| IsActive() | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| IsAutomaticSnapValid(Int32, ClickableComponent, ClickableComponent) | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| isWithinBounds(Int32, Int32) | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| [leftClickHeld(Int32, Int32)](#leftclickheldint-int) | Invoked on every frame in which a mouse button is down, regardless of the state in the previous frame.<br><span class="muted" markdown>(Overrides IClickableMenu.leftClickHeld(Int32, Int32))</span> | 
| moveCursorInDirection(Int32) | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| noSnappedComponentFound(Int32, Int32, Int32) | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| [OnClosed(EventArgs)](#onclosedeventargs) | Invokes the [Closed](viewmenu.md#closed) event handler. | 
| [Open(MenuActivationMode)](#openmenuactivationmode) | Opens this menu, i.e. makes it active if it is not already active. | 
| overrideSnappyMenuCursorMovementBan() | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| [performHoverAction(Int32, Int32)](#performhoveractionint-int) | Invoked on every frame with the mouse's current coordinates.<br><span class="muted" markdown>(Overrides IClickableMenu.performHoverAction(Int32, Int32))</span> | 
| [populateClickableComponentList()](#populateclickablecomponentlist) | <span class="muted" markdown>(Overrides IClickableMenu.populateClickableComponentList())</span> | 
| [readyToClose()](#readytoclose) | Checks if the menu is allowed to be closed by the game's default input handling.<br><span class="muted" markdown>(Overrides IClickableMenu.readyToClose())</span> | 
| [receiveGamePadButton(Buttons)](#receivegamepadbuttonbuttons) | Invoked whenever a controller button is newly pressed.<br><span class="muted" markdown>(Overrides IClickableMenu.receiveGamePadButton(Buttons))</span> | 
| [receiveKeyPress(Keys)](#receivekeypresskeys) | Invoked whenever a keyboard key is newly pressed.<br><span class="muted" markdown>(Overrides IClickableMenu.receiveKeyPress(Keys))</span> | 
| [receiveLeftClick(Int32, Int32, Boolean)](#receiveleftclickint-int-bool) | Invoked whenever the left mouse button is newly pressed.<br><span class="muted" markdown>(Overrides IClickableMenu.receiveLeftClick(Int32, Int32, Boolean))</span> | 
| [receiveRightClick(Int32, Int32, Boolean)](#receiverightclickint-int-bool) | Invoked whenever the right mouse button is newly pressed.<br><span class="muted" markdown>(Overrides IClickableMenu.receiveRightClick(Int32, Int32, Boolean))</span> | 
| [receiveScrollWheelAction(Int32)](#receivescrollwheelactionint) | Invoked whenever the mouse wheel is used. Only works with vertical scrolls.<br><span class="muted" markdown>(Overrides IClickableMenu.receiveScrollWheelAction(Int32))</span> | 
| [releaseLeftClick(Int32, Int32)](#releaseleftclickint-int) | Invoked whenever the left mouse button is just released, after being pressed/held on the last frame.<br><span class="muted" markdown>(Overrides IClickableMenu.releaseLeftClick(Int32, Int32))</span> | 
| RemoveDependency() | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| SetChildMenu(IClickableMenu) | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| setCurrentlySnappedComponentTo(Int32) | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| setUpForGamePadMode() | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| shouldClampGamePadCursor() | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| [shouldDrawCloseButton()](#shoulddrawclosebutton) | Returns whether or not to draw a button on the upper right that closes the menu when clicked.<br><span class="muted" markdown>(Overrides IClickableMenu.shouldDrawCloseButton())</span> | 
| showWithoutTransparencyIfOptionIsSet() | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| snapCursorToCurrentSnappedComponent() | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| snapToDefaultClickableComponent() | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| [update(GameTime)](#updategametime) | Runs on every update tick.<br><span class="muted" markdown>(Overrides IClickableMenu.update(GameTime))</span> | 

### Events

 | Name | Description |
| --- | --- |
| [Closed](#closed) | Event raised when the menu is closed. | 

## Details

### Constructors

#### ViewMenu(Edges, bool)

Initializes a new instance of [ViewMenu](viewmenu.md).

```cs
public ViewMenu(StardewUI.Layout.Edges gutter, bool forceDefaultFocus);
```

##### Parameters

**`gutter`** &nbsp; [Edges](layout/edges.md)  
Gutter edges, in which no content should be drawn. Used for overscan, or general aesthetics.

**`forceDefaultFocus`** &nbsp; [Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)  
Whether to always focus (snap the cursor to) the default element, even if the menu was triggered by keyboard/mouse.

-----

### Properties

#### CloseButtonOffset

Offset from the menu view's top-right edge to draw the close button, if a [CloseButtonSprite](viewmenu.md#closebuttonsprite) is also specified.

```cs
public Microsoft.Xna.Framework.Vector2 CloseButtonOffset { get; set; }
```

##### Property Value

[Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)

-----

#### CloseButtonSprite

The sprite to draw for the close button shown on the upper right. If no value is specified, then no close button will be drawn. The default behavior is to not show any close button.

```cs
public StardewUI.Graphics.Sprite CloseButtonSprite { get; set; }
```

##### Property Value

[Sprite](graphics/sprite.md)

-----

#### DimmingAmount

Amount of dimming between 0 and 1; i.e. opacity of the background underlay.

```cs
public float DimmingAmount { get; set; }
```

##### Property Value

[Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)

##### Remarks

Underlay is only drawn when game options do not force clear backgrounds.

-----

#### Gutter

Gets or sets the menu's gutter edges, which constrain the portion of the viewport in which any part of the menu may be drawn.

```cs
protected StardewUI.Layout.Edges Gutter { get; set; }
```

##### Property Value

[Edges](layout/edges.md)

##### Remarks

Gutters effectively shrink the viewport for both measurement (size calculation) and layout (centering) by clipping the screen edges.

-----

#### View

The view to display with this menu.

```cs
public StardewUI.IView View { get; }
```

##### Property Value

[IView](iview.md)

-----

### Methods

#### applyMovementKey(int)

Initiates a focus search in the specified direction.

```cs
public override void applyMovementKey(int directionValue);
```

##### Parameters

**`directionValue`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)  
An integer value corresponding to the direction; one of 0 (up), 1 (right), 2 (down) or 3 (left).

-----

#### areGamePadControlsImplemented()

Returns whether or not the menu wants **exclusive** gamepad controls.

```cs
public override bool areGamePadControlsImplemented();
```

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  Always `false`.

##### Remarks

This implementation always returns `false`. Contrary to what the name in Stardew's code implies, this setting is not required for [receiveGamePadButton(Buttons)](viewmenu.md#receivegamepadbuttonbuttons) to work; instead, when enabled, it suppresses the game's default mapping of button presses to clicks, and would therefore require reimplementing key-repeat and other basic behaviors. There is no reason to enable it here.

-----

#### BuildTooltip(IEnumerable&lt;ViewChild&gt;)

Builds/formats a tooltip given the sequence of views from root to the lowest-level hovered child.

```cs
protected virtual StardewUI.Data.TooltipData BuildTooltip(System.Collections.Generic.IEnumerable<StardewUI.ViewChild> path);
```

##### Parameters

**`path`** &nbsp; [IEnumerable](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)<[ViewChild](viewchild.md)>  
Sequence of all elements, and their relative positions, that the mouse coordinates are currently within.

##### Returns

[TooltipData](data/tooltipdata.md)

  The tooltip string to display, or `null` to not show any tooltip.

##### Remarks

The default implementation reads the value of the _last_ (lowest-level) view with a non-null [Tooltip](iview.md#tooltip), and breaks [Text](data/tooltipdata.md#text) and [Title](data/tooltipdata.md#title) lines longer than 640px, which is the default vanilla tooltip width.

-----

#### Close()

Closes this menu, either by removing it from the parent if it is a child menu, or removing it as the game's active menu if it is standalone.

```cs
public void Close();
```

-----

#### CreateView()

Creates the view.

```cs
protected virtual StardewUI.IView CreateView();
```

##### Returns

[IView](iview.md)

  The created view.

##### Remarks

Subclasses will generally create an entire tree in this method and store references to any views that might require content updates.

-----

#### CustomClose()

When overridden in a derived class, provides an alternative method to close the menu instead of the default logic in exitThisMenu(Boolean).

```cs
protected virtual void CustomClose();
```

##### Remarks

The method will only be called when the menu is closed (either programmatically or via the UI) while [GetCloseBehavior()](viewmenu.md#getclosebehavior) is returning [Custom](menuclosebehavior.md#custom).

-----

#### Dispose()



```cs
public void Dispose();
```

-----

#### draw(SpriteBatch)

Draws the current menu content.

```cs
public override void draw(Microsoft.Xna.Framework.Graphics.SpriteBatch b);
```

##### Parameters

**`b`** &nbsp; [SpriteBatch](https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.SpriteBatch.html)  
The target batch.

-----

#### GetCloseBehavior()

Gets the current close behavior for the menu.

```cs
protected virtual StardewUI.MenuCloseBehavior GetCloseBehavior();
```

##### Returns

[MenuCloseBehavior](menuclosebehavior.md)

##### Remarks

The default implementation always returns [Default](menuclosebehavior.md#default). Subclasses may override this in order to use [CustomClose()](viewmenu.md#customclose), or disable closure entirely.

-----

#### GetOriginPosition(Point, Point)

Computes the origin (top left) position of the menu for a given viewport and offset.

```cs
protected virtual Microsoft.Xna.Framework.Point GetOriginPosition(Microsoft.Xna.Framework.Point viewportSize, Microsoft.Xna.Framework.Point gutterOffset);
```

##### Parameters

**`viewportSize`** &nbsp; [Point](https://docs.monogame.net/api/Microsoft.Xna.Framework.Point.html)  
The available size of the viewport in which the menu is to be displayed.

**`gutterOffset`** &nbsp; [Point](https://docs.monogame.net/api/Microsoft.Xna.Framework.Point.html)  
The offset implied by any asymmetrical [Gutter](viewmenu.md#gutter) setting; for example, a gutter whose [Left](layout/edges.md#left) edge is `100` px and whose [Right](layout/edges.md#right) edge is only `50` px would have an X offset of `25` px (half the difference, because centered).

##### Returns

[Point](https://docs.monogame.net/api/Microsoft.Xna.Framework.Point.html)

  The origin (top left) position for the menu's root view.

-----

#### leftClickHeld(int, int)

Invoked on every frame in which a mouse button is down, regardless of the state in the previous frame.

```cs
public override void leftClickHeld(int x, int y);
```

##### Parameters

**`x`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)  
The mouse's current X position on screen.

**`y`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)  
The mouse's current Y position on screen.

-----

#### OnClosed(EventArgs)

Invokes the [Closed](viewmenu.md#closed) event handler.

```cs
protected virtual void OnClosed(System.EventArgs e);
```

##### Parameters

**`e`** &nbsp; [EventArgs](https://learn.microsoft.com/en-us/dotnet/api/system.eventargs)  
The event arguments.

-----

#### Open(MenuActivationMode)

Opens this menu, i.e. makes it active if it is not already active.

```cs
public void Open(StardewUI.MenuActivationMode activationMode);
```

##### Parameters

**`activationMode`** &nbsp; [MenuActivationMode](menuactivationmode.md)  
The activation behavior which determines which (if any) other active menu this one can replace. Ignored when the game's title menu is open.

-----

#### performHoverAction(int, int)

Invoked on every frame with the mouse's current coordinates.

```cs
public override void performHoverAction(int x, int y);
```

##### Parameters

**`x`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)  
The mouse's current X position on screen.

**`y`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)  
The mouse's current Y position on screen.

##### Remarks

Essentially the same as [update(GameTime)](viewmenu.md#updategametime) but slightly more convenient for mouse hover/movement effects because of the arguments provided.

-----

#### populateClickableComponentList()



```cs
public override void populateClickableComponentList();
```

##### Remarks

Always a no-op for menus in StardewUI.

-----

#### readyToClose()

Checks if the menu is allowed to be closed by the game's default input handling.

```cs
public override bool readyToClose();
```

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

-----

#### receiveGamePadButton(Buttons)

Invoked whenever a controller button is newly pressed.

```cs
public override void receiveGamePadButton(Microsoft.Xna.Framework.Input.Buttons b);
```

##### Parameters

**`b`** &nbsp; [Buttons](https://docs.monogame.net/api/Microsoft.Xna.Framework.Input.Buttons.html)  
The button that was pressed.

-----

#### receiveKeyPress(Keys)

Invoked whenever a keyboard key is newly pressed.

```cs
public override void receiveKeyPress(Microsoft.Xna.Framework.Input.Keys key);
```

##### Parameters

**`key`** &nbsp; [Keys](https://docs.monogame.net/api/Microsoft.Xna.Framework.Input.Keys.html)  
The key that was pressed.

-----

#### receiveLeftClick(int, int, bool)

Invoked whenever the left mouse button is newly pressed.

```cs
public override void receiveLeftClick(int x, int y, bool playSound);
```

##### Parameters

**`x`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)  
The mouse's current X position on screen.

**`y`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)  
The mouse's current Y position on screen.

**`playSound`** &nbsp; [Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)  
Currently not used.

-----

#### receiveRightClick(int, int, bool)

Invoked whenever the right mouse button is newly pressed.

```cs
public override void receiveRightClick(int x, int y, bool playSound);
```

##### Parameters

**`x`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)  
The mouse's current X position on screen.

**`y`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)  
The mouse's current Y position on screen.

**`playSound`** &nbsp; [Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)  
Currently not used.

-----

#### receiveScrollWheelAction(int)

Invoked whenever the mouse wheel is used. Only works with vertical scrolls.

```cs
public override void receiveScrollWheelAction(int value);
```

##### Parameters

**`value`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)  
A value indicating the desired vertical scroll direction; negative values indicate "down" and positive values indicate "up".

-----

#### releaseLeftClick(int, int)

Invoked whenever the left mouse button is just released, after being pressed/held on the last frame.

```cs
public override void releaseLeftClick(int x, int y);
```

##### Parameters

**`x`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)  
The mouse's current X position on screen.

**`y`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)  
The mouse's current Y position on screen.

-----

#### shouldDrawCloseButton()

Returns whether or not to draw a button on the upper right that closes the menu when clicked.

```cs
public override bool shouldDrawCloseButton();
```

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

##### Remarks

Regardless of this value, a close button will never be drawn unless [CloseButtonSprite](viewmenu.md#closebuttonsprite) is set.

-----

#### update(GameTime)

Runs on every update tick.

```cs
public override void update(Microsoft.Xna.Framework.GameTime time);
```

##### Parameters

**`time`** &nbsp; [GameTime](https://docs.monogame.net/api/Microsoft.Xna.Framework.GameTime.html)  
The current [GameTime](https://docs.monogame.net/api/Microsoft.Xna.Framework.GameTime.html) including the time elapsed since last update tick.

-----

### Events

#### Closed

Event raised when the menu is closed.

```cs
public event EventHandler<System.EventArgs>? Closed;
```

##### Event Type

[EventHandler](https://learn.microsoft.com/en-us/dotnet/api/system.eventhandler-1)<[EventArgs](https://learn.microsoft.com/en-us/dotnet/api/system.eventargs)>

-----

