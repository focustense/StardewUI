---
title: TextureSpriteConverter
description: Converts a texture to a sprite record, using the texture's entire bounds as the source rectangle.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class TextureSpriteConverter

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Converters](index.md)  
Assembly: StardewUI.dll  

</div>

Converts a texture to a sprite record, using the texture's entire bounds as the source rectangle.

```cs
public class TextureSpriteConverter : 
    StardewUI.Framework.Converters.IValueConverter<Microsoft.Xna.Framework.Graphics.Texture2D, StardewUI.Graphics.Sprite>
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ TextureSpriteConverter

**Implements**  
[IValueConverter](ivalueconverter-2.md)<[Texture2D](https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.Texture2D.html), [Sprite](../../graphics/sprite.md)>

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [TextureSpriteConverter()](#texturespriteconverter) |  | 

### Methods

 | Name | Description |
| --- | --- |
| [Convert(Texture2D)](#converttexture2d) | Converts a value from the source type to the destination type. | 

## Details

### Constructors

#### TextureSpriteConverter()



```cs
public TextureSpriteConverter();
```

-----

### Methods

#### Convert(Texture2D)

Converts a value from the source type to the destination type.

```cs
public StardewUI.Graphics.Sprite Convert(Microsoft.Xna.Framework.Graphics.Texture2D value);
```

##### Parameters

**`value`** &nbsp; [Texture2D](https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.Texture2D.html)  
The value to convert.

##### Returns

[Sprite](../../graphics/sprite.md)

-----

