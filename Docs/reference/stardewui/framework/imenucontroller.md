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
| [Menu](#menu) | Gets the menu, which can be opened using activeClickableMenu, or as a child menu. | 
| [OnClosing](#onclosing) | Gets or sets the action to be invoked before the menu closes. | 
| [PositionSelector](#positionselector) | Gets or sets a function that returns the top-left position of the menu. | 

### Methods

 | Name | Description |
| --- | --- |
| [SetGutters(Int32, Int32, Int32, Int32)](#setguttersint-int-int-int) | Configures the menu's gutter widths/heights. | 

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

#### Menu

Gets the menu, which can be opened using activeClickableMenu, or as a child menu.

```cs
StardewValley.Menus.IClickableMenu Menu { get; }
```

##### Property Value

IClickableMenu

-----

#### OnClosing

Gets or sets the action to be invoked before the menu closes.

```cs
System.Action OnClosing { get; set; }
```

##### Property Value

[Action](https://learn.microsoft.com/en-us/dotnet/api/system.action)

##### Remarks

This is equivalent to implementing cleanupBeforeExit().

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

