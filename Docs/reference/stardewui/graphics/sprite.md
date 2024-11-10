---
title: Sprite
description: Definition for a scalable sprite.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class Sprite

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Graphics](index.md)  
Assembly: StardewUI.dll  

</div>

Definition for a scalable sprite.

```cs
[StardewUI.DuckType]
public record Sprite : IEquatable<StardewUI.Graphics.Sprite>
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ Sprite

**Implements**  
[IEquatable](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1)<[Sprite](sprite.md)>

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [Sprite(Texture2D, Rectangle?, Edges, SliceSettings)](#spritetexture2d-rectangle-edges-slicesettings) | Definition for a scalable sprite. | 

### Properties

 | Name | Description |
| --- | --- |
| [EqualityContract](#equalitycontract) |  | 
| [FixedEdges](#fixededges) | The thickness of each "fixed" edge to use with 9-patch/9-slice scaling. Specifying these values can prevent corner distortion for images that have been designed for such scaling. See [Nine-Slice Scaling](https://en.wikipedia.org/wiki/9-slice_scaling) for a detailed explanation. | 
| [Size](#size) | The size (width/height) of the sprite, in pixels. | 
| [SliceSettings](#slicesettings) | Additional settings for the scaling and slicing behavior. | 
| [SourceRect](#sourcerect) | The inner area of the `Texture` in which the specific image is located, or `null` to draw the entire texture. | 
| [Texture](#texture) | The texture containing the sprite's pixel data. | 

## Details

### Constructors

#### Sprite(Texture2D, Rectangle?, Edges, SliceSettings)

Definition for a scalable sprite.

```cs
public Sprite(Microsoft.Xna.Framework.Graphics.Texture2D Texture, Microsoft.Xna.Framework.Rectangle? SourceRect, StardewUI.Layout.Edges FixedEdges, StardewUI.Graphics.SliceSettings SliceSettings);
```

##### Parameters

**`Texture`** &nbsp; [Texture2D](https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.Texture2D.html)  
The texture containing the sprite's pixel data.

**`SourceRect`** &nbsp; [Nullable](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1)<[Rectangle](https://docs.monogame.net/api/Microsoft.Xna.Framework.Rectangle.html)>  
The inner area of the `Texture` in which the specific image is located, or `null` to draw the entire texture.

**`FixedEdges`** &nbsp; [Edges](../layout/edges.md)  
The thickness of each "fixed" edge to use with 9-patch/9-slice scaling. Specifying these values can prevent corner distortion for images that have been designed for such scaling. See [Nine-Slice Scaling](https://en.wikipedia.org/wiki/9-slice_scaling) for a detailed explanation.

**`SliceSettings`** &nbsp; [SliceSettings](slicesettings.md)  
Additional settings for the scaling and slicing behavior.

-----

### Properties

#### EqualityContract



```cs
protected System.Type EqualityContract { get; }
```

##### Property Value

[Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)

-----

#### FixedEdges

The thickness of each "fixed" edge to use with 9-patch/9-slice scaling. Specifying these values can prevent corner distortion for images that have been designed for such scaling. See [Nine-Slice Scaling](https://en.wikipedia.org/wiki/9-slice_scaling) for a detailed explanation.

```cs
public StardewUI.Layout.Edges FixedEdges { get; set; }
```

##### Property Value

[Edges](../layout/edges.md)

-----

#### Size

The size (width/height) of the sprite, in pixels.

```cs
public Microsoft.Xna.Framework.Point Size { get; }
```

##### Property Value

[Point](https://docs.monogame.net/api/Microsoft.Xna.Framework.Point.html)

-----

#### SliceSettings

Additional settings for the scaling and slicing behavior.

```cs
public StardewUI.Graphics.SliceSettings SliceSettings { get; set; }
```

##### Property Value

[SliceSettings](slicesettings.md)

-----

#### SourceRect

The inner area of the `Texture` in which the specific image is located, or `null` to draw the entire texture.

```cs
public Microsoft.Xna.Framework.Rectangle? SourceRect { get; set; }
```

##### Property Value

[Nullable](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1)<[Rectangle](https://docs.monogame.net/api/Microsoft.Xna.Framework.Rectangle.html)>

-----

#### Texture

The texture containing the sprite's pixel data.

```cs
public Microsoft.Xna.Framework.Graphics.Texture2D Texture { get; set; }
```

##### Property Value

[Texture2D](https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.Texture2D.html)

-----

