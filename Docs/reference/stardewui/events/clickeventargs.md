---
title: ClickEventArgs
description: Event arguments for a controller or mouse click.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class ClickEventArgs

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Events](index.md)  
Assembly: StardewUI.dll  

</div>

Event arguments for a controller or mouse click.

```cs
public class ClickEventArgs : StardewUI.Events.PointerEventArgs, 
    StardewUI.Layout.IOffsettable<StardewUI.Events.ClickEventArgs>
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ [EventArgs](https://learn.microsoft.com/en-us/dotnet/api/system.eventargs) ⇦ [BubbleEventArgs](bubbleeventargs.md) ⇦ [PointerEventArgs](pointereventargs.md) ⇦ ClickEventArgs

**Implements**  
[IOffsettable](../layout/ioffsettable-1.md)<[ClickEventArgs](clickeventargs.md)>

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [ClickEventArgs(Vector2, SButton)](#clickeventargsvector2-sbutton) | Event arguments for a controller or mouse click. | 

### Properties

 | Name | Description |
| --- | --- |
| [Button](#button) | The specific button that triggered the click. | 
| [Handled](bubbleeventargs.md#handled) | Whether or not the view receiving the event handled the event. Set to `true` to prevent bubbling.<br><span class="muted" markdown>(Inherited from [BubbleEventArgs](bubbleeventargs.md))</span> | 
| [Position](pointereventargs.md#position) | The position, relative to the view receiving the event, of the pointer when the event occurred.<br><span class="muted" markdown>(Inherited from [PointerEventArgs](pointereventargs.md))</span> | 

### Methods

 | Name | Description |
| --- | --- |
| [IsPrimaryButton()](#isprimarybutton) | Gets whether the pressed [Button](clickeventargs.md#button) is the default for primary actions. | 
| [IsSecondaryButton()](#issecondarybutton) | Gets whether the pressed [Button](clickeventargs.md#button) is the default for secondary (context) actions. | 
| [Offset(Vector2)](#offsetvector2) | Creates a clone of this instance with an offset applied to its position. | 

## Details

### Constructors

#### ClickEventArgs(Vector2, SButton)

Event arguments for a controller or mouse click.

```cs
public ClickEventArgs(Microsoft.Xna.Framework.Vector2 position, StardewModdingAPI.SButton button);
```

##### Parameters

**`position`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)

**`button`** &nbsp; SButton  
The specific button that triggered the click.

-----

### Properties

#### Button

The specific button that triggered the click.

```cs
public StardewModdingAPI.SButton Button { get; }
```

##### Property Value

SButton

-----

### Methods

#### IsPrimaryButton()

Gets whether the pressed [Button](clickeventargs.md#button) is the default for primary actions.

```cs
public bool IsPrimaryButton();
```

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  `true` if the pressed [Button](clickeventargs.md#button) is either MouseLeft or the configured gamepad action button; otherwise, `false`.

-----

#### IsSecondaryButton()

Gets whether the pressed [Button](clickeventargs.md#button) is the default for secondary (context) actions.

```cs
public bool IsSecondaryButton();
```

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  `true` if the pressed [Button](clickeventargs.md#button) is either MouseRight or the configured gamepad tool-use button; otherwise, `false`.

-----

#### Offset(Vector2)

Creates a clone of this instance with an offset applied to its position.

```cs
public StardewUI.Events.ClickEventArgs Offset(Microsoft.Xna.Framework.Vector2 distance);
```

##### Parameters

**`distance`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
The offset distance.

##### Returns

[ClickEventArgs](clickeventargs.md)

-----

