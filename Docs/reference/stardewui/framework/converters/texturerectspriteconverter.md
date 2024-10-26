---
title: TextureRectSpriteConverter
description: Converts a tuple with a texture and source rectangle (within the texture) to a sprite record.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class TextureRectSpriteConverter

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Converters](index.md)  
Assembly: StardewUI.dll  

</div>

Converts a tuple with a texture and source rectangle (within the texture) to a sprite record.

```cs
public class TextureRectSpriteConverter : 
    StardewUI.Framework.Converters.IValueConverter<Tuple<Microsoft.Xna.Framework.Graphics.Texture2D, Microsoft.Xna.Framework.Rectangle>, StardewUI.Graphics.Sprite>
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ TextureRectSpriteConverter

**Implements**  
[IValueConverter](ivalueconverter-2.md)<[Tuple](https://learn.microsoft.com/en-us/dotnet/api/system.tuple-2)<[Texture2D](https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.Texture2D.html), [Rectangle](https://docs.monogame.net/api/Microsoft.Xna.Framework.Rectangle.html)>, [Sprite](../../graphics/sprite.md)>

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [TextureRectSpriteConverter()](#texturerectspriteconverter) |  | 

### Methods

 | Name | Description |
| --- | --- |
| [Convert(Tuple&lt;Texture2D, Rectangle&gt;)](#converttupletexture2d-rectangle) | Converts a value from the source type to the destination type. | 

## Details

### Constructors

#### TextureRectSpriteConverter()



```cs
public TextureRectSpriteConverter();
```

-----

### Methods

#### Convert(Tuple&lt;Texture2D, Rectangle&gt;)

Converts a value from the source type to the destination type.

```cs
public StardewUI.Graphics.Sprite Convert(Tuple<Microsoft.Xna.Framework.Graphics.Texture2D, Microsoft.Xna.Framework.Rectangle> value);
```

##### Parameters

**`value`** &nbsp; [Tuple](https://learn.microsoft.com/en-us/dotnet/api/system.tuple-2)<[Texture2D](https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.Texture2D.html), [Rectangle](https://docs.monogame.net/api/Microsoft.Xna.Framework.Rectangle.html)>  
The value to convert.

##### Returns

[Sprite](../../graphics/sprite.md)

-----

