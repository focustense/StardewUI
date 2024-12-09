---
title: NineSlice
description: Draws sprites according to a nine-slice scale.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class NineSlice

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Graphics](index.md)  
Assembly: StardewUI.dll  

</div>

Draws sprites according to a [nine-slice scale](https://en.wikipedia.org/wiki/9-slice_scaling).

```cs
public class NineSlice
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ NineSlice

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [NineSlice(Sprite)](#nineslicesprite) | Draws sprites according to a [nine-slice scale](https://en.wikipedia.org/wiki/9-slice_scaling). | 

### Properties

 | Name | Description |
| --- | --- |
| [Sprite](#sprite) | The source sprite. | 

### Methods

 | Name | Description |
| --- | --- |
| [Draw(ISpriteBatch, Color?, SpriteEffects)](#drawispritebatch-color-spriteeffects) | Draws the sprite to an [ISpriteBatch](ispritebatch.md), applying 9-slice scaling if specified. | 
| [Layout(Rectangle, SimpleRotation?)](#layoutrectangle-simplerotation) | Prepares the layout for next [Draw(ISpriteBatch, Color?, SpriteEffects)](nineslice.md#drawispritebatch-color-spriteeffects). | 

## Details

### Constructors

#### NineSlice(Sprite)

Draws sprites according to a [nine-slice scale](https://en.wikipedia.org/wiki/9-slice_scaling).

```cs
public NineSlice(StardewUI.Graphics.Sprite sprite);
```

##### Parameters

**`sprite`** &nbsp; [Sprite](sprite.md)  
The source sprite.

-----

### Properties

#### Sprite

The source sprite.

```cs
public StardewUI.Graphics.Sprite Sprite { get; set; }
```

##### Property Value

[Sprite](sprite.md)

-----

### Methods

#### Draw(ISpriteBatch, Color?, SpriteEffects)

Draws the sprite to an [ISpriteBatch](ispritebatch.md), applying 9-slice scaling if specified.

```cs
public void Draw(StardewUI.Graphics.ISpriteBatch b, Microsoft.Xna.Framework.Color? tint, Microsoft.Xna.Framework.Graphics.SpriteEffects effects);
```

##### Parameters

**`b`** &nbsp; [ISpriteBatch](ispritebatch.md)  
Output sprite batch.

**`tint`** &nbsp; [Nullable](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1)<[Color](https://docs.monogame.net/api/Microsoft.Xna.Framework.Color.html)>  
Optional tint multiplier color.

**`effects`** &nbsp; [SpriteEffects](https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.SpriteEffects.html)  
Sprite effect to apply, e.g. for drawing flipped.

-----

#### Layout(Rectangle, SimpleRotation?)

Prepares the layout for next [Draw(ISpriteBatch, Color?, SpriteEffects)](nineslice.md#drawispritebatch-color-spriteeffects).

```cs
public void Layout(Microsoft.Xna.Framework.Rectangle destinationRect, StardewUI.Graphics.SimpleRotation? rotation);
```

##### Parameters

**`destinationRect`** &nbsp; [Rectangle](https://docs.monogame.net/api/Microsoft.Xna.Framework.Rectangle.html)  
The rectangular area that the drawn sprite should fill.

**`rotation`** &nbsp; [Nullable](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1)<[SimpleRotation](simplerotation.md)>  
Rotation to apply to the source sprite, if any.

-----

