---
title: ButtonEventArgs
description: Event arguments for an event relating to a button (or key) of some input device.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class ButtonEventArgs

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Events](index.md)  
Assembly: StardewUI.dll  

</div>

Event arguments for an event relating to a button (or key) of some input device.

```cs
public class ButtonEventArgs : StardewUI.Events.PointerEventArgs, 
    StardewUI.Layout.IOffsettable<StardewUI.Events.ButtonEventArgs>
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ [EventArgs](https://learn.microsoft.com/en-us/dotnet/api/system.eventargs) ⇦ [BubbleEventArgs](bubbleeventargs.md) ⇦ [PointerEventArgs](pointereventargs.md) ⇦ ButtonEventArgs

**Implements**  
[IOffsettable](../layout/ioffsettable-1.md)<[ButtonEventArgs](buttoneventargs.md)>

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [ButtonEventArgs(Vector2, SButton)](#buttoneventargsvector2-sbutton) | Event arguments for an event relating to a button (or key) of some input device. | 

### Properties

 | Name | Description |
| --- | --- |
| [Button](#button) | The button that triggered the event. | 
| [Handled](bubbleeventargs.md#handled) | Whether or not the view receiving the event handled the event. Set to `true` to prevent bubbling.<br><span class="muted" markdown>(Inherited from [BubbleEventArgs](bubbleeventargs.md))</span> | 
| [Position](pointereventargs.md#position) | The position, relative to the view receiving the event, of the pointer when the event occurred.<br><span class="muted" markdown>(Inherited from [PointerEventArgs](pointereventargs.md))</span> | 

### Methods

 | Name | Description |
| --- | --- |
| [Offset(Vector2)](#offsetvector2) | Creates a clone of this instance with an offset applied to its position. | 

## Details

### Constructors

#### ButtonEventArgs(Vector2, SButton)

Event arguments for an event relating to a button (or key) of some input device.

```cs
public ButtonEventArgs(Microsoft.Xna.Framework.Vector2 position, StardewModdingAPI.SButton button);
```

##### Parameters

**`position`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
The position of the mouse cursor when the button was pressed.

**`button`** &nbsp; SButton  
The button that triggered the event.

-----

### Properties

#### Button

The button that triggered the event.

```cs
public StardewModdingAPI.SButton Button { get; }
```

##### Property Value

SButton

-----

### Methods

#### Offset(Vector2)

Creates a clone of this instance with an offset applied to its position.

```cs
public StardewUI.Events.ButtonEventArgs Offset(Microsoft.Xna.Framework.Vector2 distance);
```

##### Parameters

**`distance`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
The offset distance.

##### Returns

[ButtonEventArgs](buttoneventargs.md)

-----

