---
title: PointerEventArgs
description: Base class for any event involving the cursor/pointer, e.g. clicks.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class PointerEventArgs

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Events](index.md)  
Assembly: StardewUI.dll  

</div>

Base class for any event involving the cursor/pointer, e.g. clicks.

```cs
public class PointerEventArgs : StardewUI.Events.BubbleEventArgs, 
    StardewUI.Layout.IOffsettable<StardewUI.Events.PointerEventArgs>
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ [EventArgs](https://learn.microsoft.com/en-us/dotnet/api/system.eventargs) ⇦ [BubbleEventArgs](bubbleeventargs.md) ⇦ PointerEventArgs

**Implements**  
[IOffsettable](../layout/ioffsettable-1.md)<[PointerEventArgs](pointereventargs.md)>

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [PointerEventArgs(Vector2)](#pointereventargsvector2) | Base class for any event involving the cursor/pointer, e.g. clicks. | 

### Properties

 | Name | Description |
| --- | --- |
| [Handled](bubbleeventargs.md#handled) | Whether or not the view receiving the event handled the event. Set to `true` to prevent bubbling.<br><span class="muted" markdown>(Inherited from [BubbleEventArgs](bubbleeventargs.md))</span> | 
| [Position](#position) | The position, relative to the view receiving the event, of the pointer when the event occurred. | 

### Methods

 | Name | Description |
| --- | --- |
| [Offset(Vector2)](#offsetvector2) | Creates a clone of this instance with an offset applied to its position. | 

## Details

### Constructors

#### PointerEventArgs(Vector2)

Base class for any event involving the cursor/pointer, e.g. clicks.

```cs
public PointerEventArgs(Microsoft.Xna.Framework.Vector2 position);
```

##### Parameters

**`position`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
The position, relative to the view receiving the event, of the pointer when the event occurred.

-----

### Properties

#### Position

The position, relative to the view receiving the event, of the pointer when the event occurred.

```cs
public Microsoft.Xna.Framework.Vector2 Position { get; }
```

##### Property Value

[Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)

-----

### Methods

#### Offset(Vector2)

Creates a clone of this instance with an offset applied to its position.

```cs
public StardewUI.Events.PointerEventArgs Offset(Microsoft.Xna.Framework.Vector2 distance);
```

##### Parameters

**`distance`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
The offset distance.

##### Returns

[PointerEventArgs](pointereventargs.md)

-----

