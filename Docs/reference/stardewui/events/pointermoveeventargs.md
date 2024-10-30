---
title: PointerMoveEventArgs
description: Event arguments for pointer movement relative to some view.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class PointerMoveEventArgs

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Events](index.md)  
Assembly: StardewUI.dll  

</div>

Event arguments for pointer movement relative to some view.

```cs
public class PointerMoveEventArgs : StardewUI.Events.PointerEventArgs, 
    StardewUI.Layout.IOffsettable<StardewUI.Events.PointerMoveEventArgs>
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ [EventArgs](https://learn.microsoft.com/en-us/dotnet/api/system.eventargs) ⇦ [BubbleEventArgs](bubbleeventargs.md) ⇦ [PointerEventArgs](pointereventargs.md) ⇦ PointerMoveEventArgs

**Implements**  
[IOffsettable](../layout/ioffsettable-1.md)<[PointerMoveEventArgs](pointermoveeventargs.md)>

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [PointerMoveEventArgs(Vector2, Vector2)](#pointermoveeventargsvector2-vector2) | Event arguments for pointer movement relative to some view. | 

### Properties

 | Name | Description |
| --- | --- |
| [Handled](bubbleeventargs.md#handled) | Whether or not the view receiving the event handled the event. Set to `true` to prevent bubbling.<br><span class="muted" markdown>(Inherited from [BubbleEventArgs](bubbleeventargs.md))</span> | 
| [Position](pointereventargs.md#position) | The position, relative to the view receiving the event, of the pointer when the event occurred.<br><span class="muted" markdown>(Inherited from [PointerEventArgs](pointereventargs.md))</span> | 
| [PreviousPosition](#previousposition) | The previously-tracked position of the pointer. | 

### Methods

 | Name | Description |
| --- | --- |
| [Offset(Vector2)](#offsetvector2) | Creates a clone of this instance with an offset applied to its position. | 

## Details

### Constructors

#### PointerMoveEventArgs(Vector2, Vector2)

Event arguments for pointer movement relative to some view.

```cs
public PointerMoveEventArgs(Microsoft.Xna.Framework.Vector2 previousPosition, Microsoft.Xna.Framework.Vector2 position);
```

##### Parameters

**`previousPosition`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
The previously-tracked position of the pointer.

**`position`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
The new pointer position.

-----

### Properties

#### PreviousPosition

The previously-tracked position of the pointer.

```cs
public Microsoft.Xna.Framework.Vector2 PreviousPosition { get; }
```

##### Property Value

[Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)

-----

### Methods

#### Offset(Vector2)

Creates a clone of this instance with an offset applied to its position.

```cs
public StardewUI.Events.PointerMoveEventArgs Offset(Microsoft.Xna.Framework.Vector2 distance);
```

##### Parameters

**`distance`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
The offset distance.

##### Returns

[PointerMoveEventArgs](pointermoveeventargs.md)

-----

