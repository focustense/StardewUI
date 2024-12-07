---
title: SpriteMapBuilder&lt;T&gt;
description: Builder interface for a SpriteMap&lt;T&gt; using a single texture source.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class SpriteMapBuilder&lt;T&gt;

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Graphics](index.md)  
Assembly: StardewUI.dll  

</div>

Builder interface for a [SpriteMap&lt;T&gt;](spritemap-1.md) using a single texture source.

```cs
public class SpriteMapBuilder<T>
```

### Type Parameters

**`T`**  
Type of key for which to obtain sprites.


**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ SpriteMapBuilder&lt;T&gt;

## Remarks

Works by maintaining a virtual "cursor" which can be moved to capture the next sprite, and adding either one sprite at a time with a specific size, or several with the same size, wrapping around when necessary.

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [SpriteMapBuilder&lt;T&gt;(Texture2D)](#spritemapbuilderttexture2d) | Builder interface for a [SpriteMap&lt;T&gt;](spritemap-1.md) using a single texture source. | 

### Methods

 | Name | Description |
| --- | --- |
| [Add(T, Int32?, Int32?)](#addt-int-int) | Adds a single sprite. | 
| [Add(T, Rectangle)](#addt-rectangle) | Adds a sprite using its specific position and size in the texture. | 
| [Add(IEnumerable&lt;T&gt;)](#addienumerablet) | Adds a sequence of sprites, starting from the current cursor position and using the most recently configured [Size(Int32, Int32)](spritemapbuilder-1.md#sizeint-int) and [Padding(Int32, Int32)](spritemapbuilder-1.md#paddingint-int) to advance the cursor after each element. | 
| [Add(T)](#addt) | Adds a sequence of sprites, starting from the current cursor position and using the most recently configured [Size(Int32, Int32)](spritemapbuilder-1.md#sizeint-int) and [Padding(Int32, Int32)](spritemapbuilder-1.md#paddingint-int) to advance the cursor after each element. | 
| [Build()](#build) | Builds a new [SpriteMap&lt;T&gt;](spritemap-1.md) from the registered sprites. | 
| [Default(T)](#defaultt) | Configures the default sprite for unknown keys to use an existing sprite that has already been registered. | 
| [Default(Sprite)](#defaultsprite) | Configures the default sprite for unknown keys to use a custom sprite. | 
| [MoveBy(Int32, Int32)](#movebyint-int) | Moves the current cursor position by a specified offset. | 
| [MoveBy(Point)](#movebypoint) | Moves the current cursor position by a specified offset. | 
| [MoveTo(Int32, Int32)](#movetoint-int) | Moves the cursor to a specific coordinate. | 
| [MoveTo(Point)](#movetopoint) | Moves the cursor to a specific coordinate. | 
| [Padding(Int32, Int32)](#paddingint-int) | Configures the padding between sprites. | 
| [Padding(Point)](#paddingpoint) | Configures the padding between sprites. | 
| [Size(Int32, Int32)](#sizeint-int) | Configures the pixel size per sprite. | 
| [Size(Point)](#sizepoint) | Configures the pixel size per sprite. | 

## Details

### Constructors

#### SpriteMapBuilder&lt;T&gt;(Texture2D)

Builder interface for a [SpriteMap&lt;T&gt;](spritemap-1.md) using a single texture source.

```cs
public SpriteMapBuilder<T>(Microsoft.Xna.Framework.Graphics.Texture2D texture);
```

##### Parameters

**`texture`** &nbsp; [Texture2D](https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.Texture2D.html)

##### Remarks

Works by maintaining a virtual "cursor" which can be moved to capture the next sprite, and adding either one sprite at a time with a specific size, or several with the same size, wrapping around when necessary.

-----

### Methods

#### Add(T, int?, int?)

Adds a single sprite.

```cs
public StardewUI.Graphics.SpriteMapBuilder<T> Add(T key, int? width, int? height);
```

##### Parameters

**`key`** &nbsp; T  
The key for the sprite.

**`width`** &nbsp; [Nullable](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1)<[Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)>  
Optional override width, otherwise the most recent [Size(Int32, Int32)](spritemapbuilder-1.md#sizeint-int) will be used. Custom widths only apply to this sprite and will **not** affect the size of any subsequent additions.

**`height`** &nbsp; [Nullable](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1)<[Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)>  
Optional override height, otherwise the most recent [Size(Int32, Int32)](spritemapbuilder-1.md#sizeint-int) will be used. Custom heights only apply to this sprite and will **not** affect the size of any subsequent additions.

##### Returns

[SpriteMapBuilder&lt;T&gt;](spritemapbuilder-1.md)

  The current builder instance.

-----

#### Add(T, Rectangle)

Adds a sprite using its specific position and size in the texture.

```cs
public StardewUI.Graphics.SpriteMapBuilder<T> Add(T key, Microsoft.Xna.Framework.Rectangle sourceRect);
```

##### Parameters

**`key`** &nbsp; T  
The key for the sprite.

**`sourceRect`** &nbsp; [Rectangle](https://docs.monogame.net/api/Microsoft.Xna.Framework.Rectangle.html)  
The exact position and size of the sprite.

##### Returns

[SpriteMapBuilder&lt;T&gt;](spritemapbuilder-1.md)

  The current builder instance.

##### Remarks

After adding, moves the cursor to the top-right of the `sourceRect`, unless it would be horizontally out of bounds, in which case it wraps to the beginning (X = 0) of the next row.

-----

#### Add(IEnumerable&lt;T&gt;)

Adds a sequence of sprites, starting from the current cursor position and using the most recently configured [Size(Int32, Int32)](spritemapbuilder-1.md#sizeint-int) and [Padding(Int32, Int32)](spritemapbuilder-1.md#paddingint-int) to advance the cursor after each element.

```cs
public StardewUI.Graphics.SpriteMapBuilder<T> Add(System.Collections.Generic.IEnumerable<T> keys);
```

##### Parameters

**`keys`** &nbsp; [IEnumerable&lt;T&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)  
The keys for the sprites to add, in the same left-to-right order that they appear in the source texture.

##### Returns

[SpriteMapBuilder&lt;T&gt;](spritemapbuilder-1.md)

  The current builder instance.

##### Remarks

Wraps to the beginning of the next row (X = 0) when the end of a row is reached.

-----

#### Add(T)

Adds a sequence of sprites, starting from the current cursor position and using the most recently configured [Size(Int32, Int32)](spritemapbuilder-1.md#sizeint-int) and [Padding(Int32, Int32)](spritemapbuilder-1.md#paddingint-int) to advance the cursor after each element.

```cs
public StardewUI.Graphics.SpriteMapBuilder<T> Add(T keys);
```

##### Parameters

**`keys`** &nbsp; `T`  
The keys for the sprites to add, in the same left-to-right order that they appear in the source texture.

##### Returns

[SpriteMapBuilder&lt;T&gt;](spritemapbuilder-1.md)

  The current builder instance.

##### Remarks

Wraps to the beginning of the next row (X = 0) when the end of a row is reached.

-----

#### Build()

Builds a new [SpriteMap&lt;T&gt;](spritemap-1.md) from the registered sprites.

```cs
public StardewUI.Graphics.SpriteMap<T> Build();
```

##### Returns

[SpriteMap&lt;T&gt;](spritemap-1.md)

-----

#### Default(T)

Configures the default sprite for unknown keys to use an existing sprite that has already been registered.

```cs
public StardewUI.Graphics.SpriteMapBuilder<T> Default(T key);
```

##### Parameters

**`key`** &nbsp; T  
Key of the previously-added sprite to use as default.

##### Returns

[SpriteMapBuilder&lt;T&gt;](spritemapbuilder-1.md)

  The current builder instance.

-----

#### Default(Sprite)

Configures the default sprite for unknown keys to use a custom sprite.

```cs
public StardewUI.Graphics.SpriteMapBuilder<T> Default(StardewUI.Graphics.Sprite sprite);
```

##### Parameters

**`sprite`** &nbsp; [Sprite](sprite.md)  
The sprite to use as default.

##### Returns

[SpriteMapBuilder&lt;T&gt;](spritemapbuilder-1.md)

  The current builder instance.

-----

#### MoveBy(int, int)

Moves the current cursor position by a specified offset.

```cs
public StardewUI.Graphics.SpriteMapBuilder<T> MoveBy(int x, int y);
```

##### Parameters

**`x`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)  
Horizontal offset from current position.

**`y`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)  
Vertical offset from current position.

##### Returns

[SpriteMapBuilder&lt;T&gt;](spritemapbuilder-1.md)

  The current builder instance.

-----

#### MoveBy(Point)

Moves the current cursor position by a specified offset.

```cs
public StardewUI.Graphics.SpriteMapBuilder<T> MoveBy(Microsoft.Xna.Framework.Point p);
```

##### Parameters

**`p`** &nbsp; [Point](https://docs.monogame.net/api/Microsoft.Xna.Framework.Point.html)  
The horizontal (X) and vertical (Y) offsets from the current position.

##### Returns

[SpriteMapBuilder&lt;T&gt;](spritemapbuilder-1.md)

  The current builder instance.

-----

#### MoveTo(int, int)

Moves the cursor to a specific coordinate.

```cs
public StardewUI.Graphics.SpriteMapBuilder<T> MoveTo(int x, int y);
```

##### Parameters

**`x`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)  
The new X coordinate of the cursor.

**`y`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)  
The new Y coordinate of the cursor.

##### Returns

[SpriteMapBuilder&lt;T&gt;](spritemapbuilder-1.md)

  The current builder instance.

##### Remarks

Generally used when dealing with semi-regular spritesheets having distinct areas that are individually uniform but different from each other, e.g. a row of 10x10 placed in an empty area of a 32x32 sheet.

-----

#### MoveTo(Point)

Moves the cursor to a specific coordinate.

```cs
public StardewUI.Graphics.SpriteMapBuilder<T> MoveTo(Microsoft.Xna.Framework.Point p);
```

##### Parameters

**`p`** &nbsp; [Point](https://docs.monogame.net/api/Microsoft.Xna.Framework.Point.html)  
The horizontal (X) and vertical (Y) coordinates for the cursor.

##### Returns

[SpriteMapBuilder&lt;T&gt;](spritemapbuilder-1.md)

  The current builder instance.

##### Remarks

Generally used when dealing with semi-regular spritesheets having distinct areas that are individually uniform but different from each other, e.g. a row of 10x10 placed in an empty area of a 32x32 sheet.

-----

#### Padding(int, int)

Configures the padding between sprites.

```cs
public StardewUI.Graphics.SpriteMapBuilder<T> Padding(int x, int y);
```

##### Parameters

**`x`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)  
Horizontal padding from the right edge of one sprite to the left edge of the next. Added whenever advancing from left to right.

**`y`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)  
Vertical padding from the bottom edge of one sprite to the top edge of the next. Added whenever wrapping from the end of a row to the beginning of the next row.

##### Returns

[SpriteMapBuilder&lt;T&gt;](spritemapbuilder-1.md)

  The current builder instance.

##### Remarks

Applies only to new sprites added afterward; will not affect sprites previously added.

-----

#### Padding(Point)

Configures the padding between sprites.

```cs
public StardewUI.Graphics.SpriteMapBuilder<T> Padding(Microsoft.Xna.Framework.Point p);
```

##### Parameters

**`p`** &nbsp; [Point](https://docs.monogame.net/api/Microsoft.Xna.Framework.Point.html)  
A point containing the horizontal (X) and vertical (Y) padding values. See [Padding(Int32, Int32)](spritemapbuilder-1.md#paddingint-int).

##### Returns

[SpriteMapBuilder&lt;T&gt;](spritemapbuilder-1.md)

  The current builder instance.

##### Remarks

Applies only to new sprites added afterward; will not affect sprites previously added.

-----

#### Size(int, int)

Configures the pixel size per sprite.

```cs
public StardewUI.Graphics.SpriteMapBuilder<T> Size(int width, int height);
```

##### Parameters

**`width`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)  
The pixel width for newly-added sprites.

**`height`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)  
The pixel height for newly-added sprites.

##### Returns

[SpriteMapBuilder&lt;T&gt;](spritemapbuilder-1.md)

  The current builder instance.

##### Remarks

Applies only to new sprites added afterward; will not affect sprites previously added.

-----

#### Size(Point)

Configures the pixel size per sprite.

```cs
public StardewUI.Graphics.SpriteMapBuilder<T> Size(Microsoft.Xna.Framework.Point p);
```

##### Parameters

**`p`** &nbsp; [Point](https://docs.monogame.net/api/Microsoft.Xna.Framework.Point.html)  
A point containing the width (X) and height (Y) for newly-added sprites.

##### Returns

[SpriteMapBuilder&lt;T&gt;](spritemapbuilder-1.md)

  The current builder instance.

##### Remarks

Applies only to new sprites added afterward; will not affect sprites previously added.

-----

