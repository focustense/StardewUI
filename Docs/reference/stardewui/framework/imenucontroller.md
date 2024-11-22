---
title: IMenuController
description: Wrapper for a mod-managed IClickableMenu that allows further customization of menu-level properties not accessible to StarML or data binding.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Interface IMenuController

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework](index.md)  
Assembly: StardewUI.dll  

</div>

Wrapper for a mod-managed IClickableMenu that allows further customization of menu-level properties not accessible to StarML or data binding.

```cs
public interface IMenuController : System.IDisposable
```

**Implements**  
[IDisposable](https://learn.microsoft.com/en-us/dotnet/api/system.idisposable)

## Members

### Properties

 | Name | Description |
| --- | --- |
| [CanClose](#canclose) | Gets or sets a function that returns whether or not the menu can be closed. | 
| [CloseAction](#closeaction) | Gets or sets an action that **replaces** the default menu-close behavior. | 
| [CloseButtonOffset](#closebuttonoffset) | Offset from the menu view's top-right edge to draw the close button. | 
| [DimmingAmount](#dimmingamount) | How much the menu should dim the entire screen underneath. | 
| [Menu](#menu) | Gets the menu, which can be opened using activeClickableMenu, or as a child menu. | 
| [PositionSelector](#positionselector) | Gets or sets a function that returns the top-left position of the menu. | 

### Methods

 | Name | Description |
| --- | --- |
| [EnableCloseButton(Texture2D, Rectangle?, Single)](#enableclosebuttontexture2d-rectangle-float) | Configures the menu to display a close button on the upper-right side. | 
| [SetGutters(Int32, Int32, Int32, Int32)](#setguttersint-int-int-int) | Configures the menu's gutter widths/heights. | 

### Events

 | Name | Description |
| --- | --- |
| [Closed](#closed) | Event raised after the menu has been closed. | 
| [Closing](#closing) | Event raised when the menu is about to close. | 

## Details

### Properties

#### CanClose

Gets or sets a function that returns whether or not the menu can be closed.

```cs
Func<System.Boolean> CanClose { get; set; }
```

##### Property Value

[Func](https://learn.microsoft.com/en-us/dotnet/api/system.func-1)<[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)>

##### Remarks

This is equivalent to implementing readyToClose().

-----

#### CloseAction

Gets or sets an action that **replaces** the default menu-close behavior.

```cs
System.Action CloseAction { get; set; }
```

##### Property Value

[Action](https://learn.microsoft.com/en-us/dotnet/api/system.action)

##### Remarks

Most users should leave this property unset. It is intended for use in unusual contexts, such as replacing the mod settings in a Generic Mod Config Menu integration. Setting any non-null value to this property will suppress the default behavior of exitThisMenu(Boolean) entirely, so the caller is responsible for handling all possible scenarios (e.g. child of another menu, or sub-menu of the title menu).

-----

#### CloseButtonOffset

Offset from the menu view's top-right edge to draw the close button.

```cs
Microsoft.Xna.Framework.Vector2 CloseButtonOffset { get; set; }
```

##### Property Value

[Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)

##### Remarks

Only applies when [EnableCloseButton(Texture2D, Rectangle?, Single)](imenucontroller.md#enableclosebuttontexture2d-rectangle-float) has been called at least once.

-----

#### DimmingAmount

How much the menu should dim the entire screen underneath.

```cs
float DimmingAmount { get; set; }
```

##### Property Value

[Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)

##### Remarks

The default dimming is appropriate for most menus, but if the menu is being drawn as a delegate of some other macro-menu, then it can be lowered or removed (set to `0`) entirely.

-----

#### Menu

Gets the menu, which can be opened using activeClickableMenu, or as a child menu.

```cs
StardewValley.Menus.IClickableMenu Menu { get; }
```

##### Property Value

IClickableMenu

-----

#### PositionSelector

Gets or sets a function that returns the top-left position of the menu.

```cs
Func<Microsoft.Xna.Framework.Point> PositionSelector { get; set; }
```

##### Property Value

[Func](https://learn.microsoft.com/en-us/dotnet/api/system.func-1)<[Point](https://docs.monogame.net/api/Microsoft.Xna.Framework.Point.html)>

##### Remarks

Setting any non-null value will disable the auto-centering functionality, and is equivalent to setting the xPositionOnScreen and yPositionOnScreen fields.

-----

### Methods

#### EnableCloseButton(Texture2D, Rectangle?, float)

Configures the menu to display a close button on the upper-right side.

```cs
void EnableCloseButton(Microsoft.Xna.Framework.Graphics.Texture2D texture, Microsoft.Xna.Framework.Rectangle? sourceRect, float scale);
```

##### Parameters

**`texture`** &nbsp; [Texture2D](https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.Texture2D.html)  
The source image/tile sheet containing the button image.

**`sourceRect`** &nbsp; [Nullable](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1)<[Rectangle](https://docs.monogame.net/api/Microsoft.Xna.Framework.Rectangle.html)>  
The location within the `texture` where the image is located, or `null` to draw the entire `texture`.

**`scale`** &nbsp; [Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)  
Scale to apply, if the destination size should be different from the size of the `sourceRect`.

##### Remarks

If no `texture` is specified, then all other parameters are ignored and the default close button sprite is drawn. Otherwise, a custom sprite will be drawn using the specified parameters.

-----

#### SetGutters(int, int, int, int)

Configures the menu's gutter widths/heights.

```cs
void SetGutters(int left, int top, int right, int bottom);
```

##### Parameters

**`left`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)  
The gutter width on the left side of the viewport.

**`top`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)  
The gutter height at the top of the viewport.

**`right`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)  
The gutter width on the right side of the viewport. The default value of `-1` specifies that the `left` value should be mirrored on the right.

**`bottom`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)  
The gutter height at the bottom of the viewport. The default value of `-1` specifies that the `top` value should be mirrored on the bottom.

##### Remarks

Gutters are areas of the screen that the menu should not occupy. These are typically used with a menu whose root view uses [Stretch()](../layout/length.md#stretch) for one of its [Layout](../iview.md#layout) dimensions, and allows limiting the max width/height relative to the viewport size. 

 The historical reason for gutters is [overscan](https://en.wikipedia.org/wiki/Overscan), however they are still commonly used for aesthetic reasons.

-----

### Events

#### Closed

Event raised after the menu has been closed.

```cs
event System.Action? Closed;
```

##### Event Type

[Action](https://learn.microsoft.com/en-us/dotnet/api/system.action)

-----

#### Closing

Event raised when the menu is about to close.

```cs
event System.Action? Closing;
```

##### Event Type

[Action](https://learn.microsoft.com/en-us/dotnet/api/system.action)

##### Remarks

This has the same lifecycle as cleanupBeforeExit().

-----

