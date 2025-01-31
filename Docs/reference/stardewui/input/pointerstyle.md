---
title: PointerStyle
description: The vanilla pointer styles that can be drawn as a mouse cursor.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Enum PointerStyle

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Input](index.md)  
Assembly: StardewUI.dll  

</div>

The vanilla pointer styles that can be drawn as a mouse cursor.

```cs
public enum PointerStyle
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ [ValueType](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype) ⇦ [Enum](https://learn.microsoft.com/en-us/dotnet/api/system.enum) ⇦ PointerStyle

## Remarks

These are the values supported and expected in vanilla menus via drawMouse(SpriteBatch, Boolean, Int32). Unlike [Cursor](../graphics/cursor.md), the available options are limited to standard 16x16 rectangles on the main Cursors tilesheet.

## Fields

 | Name | Value | Description |
| --- | --- | --- |
| <a id="default">Default</a> | -1 | The default pointer for the given input device. Automatically switches between [Arrow](pointerstyle.md#arrow) and [Finger](pointerstyle.md#finger) depending on whether gamepad controls are active. | 
| <a id="arrow">Arrow</a> | 0 | The default arrow cursor, used for mouse input when no special circumstances are present. | 
| <a id="hourglass">Hourglass</a> | 1 | Wait cursor, used e.g. when loading a save game. | 
| <a id="hand">Hand</a> | 2 | An open hand. Often used to indicate picking up items, opening doors or chests, petting animals, etc. In a UI, can also be used to indicate panning or dragging. | 
| <a id="gift">Gift</a> | 3 | Arrow tip with a wrapped gift box. Used to indicate gifting an NPC. | 
| <a id="dialogue">Dialogue</a> | 4 | Arrow tip with a speech bubble. Used to indicate the initiation of dialogue with an NPC. | 
| <a id="search">Search</a> | 5 | Small magnifying glass denoting some type of search function. | 
| <a id="harvest">Harvest</a> | 6 | Standard arrow pointer next to a plus, used for harvesting. The same icon is also used for stamina. | 
| <a id="health">Health</a> | 7 | Standard arrow pointer next to a heart. | 
| <a id="finger">Finger</a> | 44 | Closed hand with pointed index finger, used for gamepad focus. | 
| <a id="controllera">ControllerA</a> | 45 | The "A" button on a gamepad controller, without an explicit pointer. | 
| <a id="controllerx">ControllerX</a> | 46 | The "X" button on a gamepad controller, without an explicit pointer. | 
| <a id="controllerb">ControllerB</a> | 47 | The "B" button on a gamepad controller, without an explicit pointer. | 
| <a id="controllery">ControllerY</a> | 48 | The "Y" button on a gamepad controller, without an explicit pointer. | 

