---
title: WheelEventArgs
description: Event arguments for mouse wheel/scroll wheel actions.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class WheelEventArgs

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Events](index.md)  
Assembly: StardewUI.dll  

</div>

Event arguments for mouse wheel/scroll wheel actions.

```cs
public class WheelEventArgs : StardewUI.Events.PointerEventArgs, 
    StardewUI.Layout.IOffsettable<StardewUI.Events.WheelEventArgs>
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ [EventArgs](https://learn.microsoft.com/en-us/dotnet/api/system.eventargs) ⇦ [BubbleEventArgs](bubbleeventargs.md) ⇦ [PointerEventArgs](pointereventargs.md) ⇦ WheelEventArgs

**Implements**  
[IOffsettable](../layout/ioffsettable-1.md)<[WheelEventArgs](wheeleventargs.md)>

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [WheelEventArgs(Vector2, Direction)](#wheeleventargsvector2-direction) | Event arguments for mouse wheel/scroll wheel actions. | 

### Properties

 | Name | Description |
| --- | --- |
| [Direction](#direction) | Direction of the wheel movement. | 
| [Handled](bubbleeventargs.md#handled) | Whether or not the view receiving the event handled the event. Set to `true` to prevent bubbling.<br><span class="muted" markdown>(Inherited from [BubbleEventArgs](bubbleeventargs.md))</span> | 
| [Position](pointereventargs.md#position) | The position, relative to the view receiving the event, of the pointer when the event occurred.<br><span class="muted" markdown>(Inherited from [PointerEventArgs](pointereventargs.md))</span> | 

### Methods

 | Name | Description |
| --- | --- |
| [Offset(Vector2)](#offsetvector2) | Creates a clone of this instance with an offset applied to its position. | 

## Details

### Constructors

#### WheelEventArgs(Vector2, Direction)

Event arguments for mouse wheel/scroll wheel actions.

```cs
public WheelEventArgs(Microsoft.Xna.Framework.Vector2 position, StardewUI.Direction direction);
```

##### Parameters

**`position`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)

**`direction`** &nbsp; [Direction](../direction.md)  
Direction of the wheel movement.

-----

### Properties

#### Direction

Direction of the wheel movement.

```cs
public StardewUI.Direction Direction { get; }
```

##### Property Value

[Direction](../direction.md)

-----

### Methods

#### Offset(Vector2)

Creates a clone of this instance with an offset applied to its position.

```cs
public StardewUI.Events.WheelEventArgs Offset(Microsoft.Xna.Framework.Vector2 distance);
```

##### Parameters

**`distance`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
The offset distance.

##### Returns

[WheelEventArgs](wheeleventargs.md)

-----

