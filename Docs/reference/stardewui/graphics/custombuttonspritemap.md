---
title: CustomButtonSpriteMap
description: Controller/keyboard/mouse sprite map using custom/configured asset data.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class CustomButtonSpriteMap

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Graphics](index.md)  
Assembly: StardewUI.dll  

</div>

Controller/keyboard/mouse sprite map using custom/configured asset data.

```cs
public class CustomButtonSpriteMap : StardewUI.Graphics.ButtonSpriteMap
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ [ButtonSpriteMap](buttonspritemap.md) ⇦ CustomButtonSpriteMap

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [CustomButtonSpriteMap(IGameContentHelper, ButtonSpriteMapData)](#custombuttonspritemapigamecontenthelper-buttonspritemapdata) | Controller/keyboard/mouse sprite map using custom/configured asset data. | 

### Properties

 | Name | Description |
| --- | --- |
| [ControllerBlank](#controllerblank) | A blank controller button upon which the specific button label can be drawn.<br><span class="muted" markdown>(Overrides [ButtonSpriteMap](buttonspritemap.md).`get_ControllerBlank()`)</span> | 
| [KeyboardBlank](#keyboardblank) | A blank keyboard key upon which the specific key name can be drawn.<br><span class="muted" markdown>(Overrides [ButtonSpriteMap](buttonspritemap.md).`get_KeyboardBlank()`)</span> | 
| [MouseLeft](#mouseleft) | The mouse with left button pressed.<br><span class="muted" markdown>(Overrides [ButtonSpriteMap](buttonspritemap.md).`get_MouseLeft()`)</span> | 
| [MouseMiddle](#mousemiddle) | The mouse with middle button pressed.<br><span class="muted" markdown>(Overrides [ButtonSpriteMap](buttonspritemap.md).`get_MouseMiddle()`)</span> | 
| [MouseRight](#mouseright) | The mouse with right button pressed.<br><span class="muted" markdown>(Overrides [ButtonSpriteMap](buttonspritemap.md).`get_MouseRight()`)</span> | 

### Methods

 | Name | Description |
| --- | --- |
| [Get(SButton)](#getsbutton) | Gets the specific sprite for a particular button.<br><span class="muted" markdown>(Overrides [ButtonSpriteMap](buttonspritemap.md).[Get(SButton)](buttonspritemap.md#getsbutton))</span> | 
| [Get(SButton, Boolean)](buttonspritemap.md#getsbutton-boolean) | Gets the sprite corresponding to a particular key.<br><span class="muted" markdown>(Inherited from [ButtonSpriteMap](buttonspritemap.md))</span> | 

## Details

### Constructors

#### CustomButtonSpriteMap(IGameContentHelper, ButtonSpriteMapData)

Controller/keyboard/mouse sprite map using custom/configured asset data.

```cs
public CustomButtonSpriteMap(StardewModdingAPI.IGameContentHelper content, StardewUI.Data.ButtonSpriteMapData data);
```

##### Parameters

**`content`** &nbsp; IGameContentHelper  
Helper for retrieving main game assets.

**`data`** &nbsp; [ButtonSpriteMapData](../data/buttonspritemapdata.md)  
Configuration data for this map.

-----

### Properties

#### ControllerBlank

A blank controller button upon which the specific button label can be drawn.

```cs
protected StardewUI.Graphics.Sprite ControllerBlank { get; }
```

##### Property Value

[Sprite](sprite.md)

##### Remarks

If the sprite specifies non-zero [FixedEdges](sprite.md#fixededges) then they will be added to the label's margin.

-----

#### KeyboardBlank

A blank keyboard key upon which the specific key name can be drawn.

```cs
protected StardewUI.Graphics.Sprite KeyboardBlank { get; }
```

##### Property Value

[Sprite](sprite.md)

##### Remarks

If the sprite specifies non-zero [FixedEdges](sprite.md#fixededges) then they will be added to the label's margin.

-----

#### MouseLeft

The mouse with left button pressed.

```cs
protected StardewUI.Graphics.Sprite MouseLeft { get; }
```

##### Property Value

[Sprite](sprite.md)

-----

#### MouseMiddle

The mouse with middle button pressed.

```cs
protected StardewUI.Graphics.Sprite MouseMiddle { get; }
```

##### Property Value

[Sprite](sprite.md)

-----

#### MouseRight

The mouse with right button pressed.

```cs
protected StardewUI.Graphics.Sprite MouseRight { get; }
```

##### Property Value

[Sprite](sprite.md)

-----

### Methods

#### Get(SButton)

Gets the specific sprite for a particular button.

```cs
protected override StardewUI.Graphics.Sprite Get(StardewModdingAPI.SButton button);
```

##### Parameters

**`button`** &nbsp; SButton  
The button for which to retrieve a sprite.

##### Returns

[Sprite](sprite.md)

  The precise [Sprite](sprite.md) representing the given `button`, or `null` if the button does not have a special sprite and could/should use a generic background + text.

-----

