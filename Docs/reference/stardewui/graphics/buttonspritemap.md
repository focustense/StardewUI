---
title: ButtonSpriteMap
description: Base class for a ISpriteMap`1 for controller/keyboard bindings.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class ButtonSpriteMap

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Graphics](index.md)  
Assembly: StardewUI.dll  

</div>

Base class for a [ISpriteMap&lt;T&gt;](ispritemap-1.md) for controller/keyboard bindings.

```cs
public class ButtonSpriteMap : 
    StardewUI.Graphics.ISpriteMap<StardewModdingAPI.SButton>
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ ButtonSpriteMap

**Implements**  
[ISpriteMap](ispritemap-1.md)<SButton>

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [ButtonSpriteMap()](#buttonspritemap) |  | 

### Properties

 | Name | Description |
| --- | --- |
| [ControllerBlank](#controllerblank) | A blank controller button upon which the specific button label can be drawn. | 
| [KeyboardBlank](#keyboardblank) | A blank keyboard key upon which the specific key name can be drawn. | 
| [MouseLeft](#mouseleft) | The mouse with left button pressed. | 
| [MouseMiddle](#mousemiddle) | The mouse with middle button pressed. | 
| [MouseRight](#mouseright) | The mouse with right button pressed. | 

### Methods

 | Name | Description |
| --- | --- |
| [Get(SButton, Boolean)](#getsbutton-boolean) | Gets the sprite corresponding to a particular key. | 
| [Get(SButton)](#getsbutton) | Gets the specific sprite for a particular button. | 

## Details

### Constructors

#### ButtonSpriteMap()



```cs
protected ButtonSpriteMap();
```

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

#### Get(SButton, Boolean)

Gets the sprite corresponding to a particular key.

```cs
public StardewUI.Graphics.Sprite Get(StardewModdingAPI.SButton key, out System.Boolean isPlaceholder);
```

##### Parameters

**`key`** &nbsp; SButton  
The key to retrieve.

**`isPlaceholder`** &nbsp; [Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)  
`true` if the returned [Sprite](sprite.md) is not specific to the `key`, but is instead a placeholder (border/background) in which some substitute, typically normal text, must be drawn. `false` if the [Sprite](sprite.md) is a complete self-contained representation of the `key`.

##### Returns

[Sprite](sprite.md)

-----

#### Get(SButton)

Gets the specific sprite for a particular button.

```cs
protected virtual StardewUI.Graphics.Sprite Get(StardewModdingAPI.SButton button);
```

##### Parameters

**`button`** &nbsp; SButton  
The button for which to retrieve a sprite.

##### Returns

[Sprite](sprite.md)

  The precise [Sprite](sprite.md) representing the given `button`, or `null` if the button does not have a special sprite and could/should use a generic background + text.

-----

