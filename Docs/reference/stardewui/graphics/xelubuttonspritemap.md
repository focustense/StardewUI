---
title: XeluButtonSpriteMap
description: Controller/keyboard sprite map based on Xelu's CC0 pack: https://thoseawesomeguys.com/prompts/
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class XeluButtonSpriteMap

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Graphics](index.md)  
Assembly: StardewUI.dll  

</div>

Controller/keyboard sprite map based on Xelu's CC0 pack: https://thoseawesomeguys.com/prompts/

```cs
public class XeluButtonSpriteMap : StardewUI.Graphics.ButtonSpriteMap
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ [ButtonSpriteMap](buttonspritemap.md) ⇦ XeluButtonSpriteMap

## Remarks

Uses specific sprites (Xbox-based) per gamepad button, with a fallback for unknown buttons. All keyboard keys use the same placeholder border/background sprite with the expectation of having the key name drawn inside, in order to at least be consistent with Stardew's fonts.

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [XeluButtonSpriteMap(Texture2D, Texture2D, Texture2D)](#xelubuttonspritemaptexture2d-texture2d-texture2d) | Controller/keyboard sprite map based on Xelu's CC0 pack: https://thoseawesomeguys.com/prompts/ | 

### Properties

 | Name | Description |
| --- | --- |
| [ControllerBlank](#controllerblank) | A blank controller button upon which the specific button label can be drawn.<br><span class="muted" markdown>(Overrides [ButtonSpriteMap](buttonspritemap.md).`get_ControllerBlank()`)</span> | 
| [KeyboardBlank](#keyboardblank) | A blank keyboard key upon which the specific key name can be drawn.<br><span class="muted" markdown>(Overrides [ButtonSpriteMap](buttonspritemap.md).`get_KeyboardBlank()`)</span> | 
| [KeyboardTheme](#keyboardtheme) | The active theme for keyboard sprites. | 
| [MouseLeft](#mouseleft) | The mouse with left button pressed.<br><span class="muted" markdown>(Overrides [ButtonSpriteMap](buttonspritemap.md).`get_MouseLeft()`)</span> | 
| [MouseMiddle](#mousemiddle) | The mouse with middle button pressed.<br><span class="muted" markdown>(Overrides [ButtonSpriteMap](buttonspritemap.md).`get_MouseMiddle()`)</span> | 
| [MouseRight](#mouseright) | The mouse with right button pressed.<br><span class="muted" markdown>(Overrides [ButtonSpriteMap](buttonspritemap.md).`get_MouseRight()`)</span> | 
| [MouseTheme](#mousetheme) | The active theme for mouse sprites. | 
| [SliceScale](#slicescale) | Scale to apply to nine-slice sprites, specifically keyboard blanks. | 

### Methods

 | Name | Description |
| --- | --- |
| [Get(SButton)](#getsbutton) | Gets the specific sprite for a particular button.<br><span class="muted" markdown>(Overrides [ButtonSpriteMap](buttonspritemap.md).[Get(SButton)](buttonspritemap.md#getsbutton))</span> | 
| [Get(SButton, Boolean)](buttonspritemap.md#getsbutton-boolean) | Gets the sprite corresponding to a particular key.<br><span class="muted" markdown>(Inherited from [ButtonSpriteMap](buttonspritemap.md))</span> | 

## Details

### Constructors

#### XeluButtonSpriteMap(Texture2D, Texture2D, Texture2D)

Controller/keyboard sprite map based on Xelu's CC0 pack: https://thoseawesomeguys.com/prompts/

```cs
public XeluButtonSpriteMap(Microsoft.Xna.Framework.Graphics.Texture2D gamepad, Microsoft.Xna.Framework.Graphics.Texture2D keyboard, Microsoft.Xna.Framework.Graphics.Texture2D mouse);
```

##### Parameters

**`gamepad`** &nbsp; [Texture2D](https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.Texture2D.html)  
Gamepad texture atlas, loaded from the mod's copy of `GamepadButtons.png`.

**`keyboard`** &nbsp; [Texture2D](https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.Texture2D.html)  
Keyboard texture atlas, loaded from the mod's copy of `KeyboardKeys.png`.

**`mouse`** &nbsp; [Texture2D](https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.Texture2D.html)  
Mouse texture atlas, loaded from the mod's copy of `MouseButtons.png`.

##### Remarks

Uses specific sprites (Xbox-based) per gamepad button, with a fallback for unknown buttons. All keyboard keys use the same placeholder border/background sprite with the expectation of having the key name drawn inside, in order to at least be consistent with Stardew's fonts.

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

#### KeyboardTheme

The active theme for keyboard sprites.

```cs
public StardewUI.Graphics.XeluButtonSpriteMap.SpriteTheme KeyboardTheme { get; set; }
```

##### Property Value

[SpriteTheme](xelubuttonspritemap.spritetheme.md)

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

#### MouseTheme

The active theme for mouse sprites.

```cs
public StardewUI.Graphics.XeluButtonSpriteMap.SpriteTheme MouseTheme { get; set; }
```

##### Property Value

[SpriteTheme](xelubuttonspritemap.spritetheme.md)

-----

#### SliceScale

Scale to apply to nine-slice sprites, specifically keyboard blanks.

```cs
public float SliceScale { get; set; }
```

##### Property Value

[Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)

##### Remarks

This setting exists because the sprite assets are much larger than the space available for a typical keybind image in a menu, which - very unusually for Stardew - means they need to be scaled down, not up. However, some UIs (e.g. some overlays) may want to display these sprites at their normal size or larger, and in these cases, should not scale the slices because the borders would look strange or hard to see. 

 In general, considering the base dimensions of 100x100, a comfortable size for menus targeting roughly 48px button height should use roughly 1/3 scale (0.3). Overlays and other UIs intending to render the sprite at full size (or larger) can leave the default of 1.

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

