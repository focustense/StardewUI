---
title: TransformOrigin
description: Describes the origin point to use for a local Transform.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class TransformOrigin

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Graphics](index.md)  
Assembly: StardewUI.dll  

</div>

Describes the origin point to use for a local [Transform](transform.md).

```cs
public record TransformOrigin : IEquatable<StardewUI.Graphics.TransformOrigin>
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ TransformOrigin

**Implements**  
[IEquatable](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1)<[TransformOrigin](transformorigin.md)>

## Remarks

Origin data needs to track two vectors; the relative or percentage position with [X](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html#Microsoft_Xna_Framework_Vector2) and [Y](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html#Microsoft_Xna_Framework_Vector2) between `0` and `1` (e.g. the center of the layout would be `(0.5, 0.5)`) as well as the absolute or pixel position. 

 The relative position is used for individual drawing operations; when drawing a single sprite or text string, the XNA drawing APIs use this exact origin vector. The absolute position, on the other hand, is required for transform propagation in the [GlobalTransform](globaltransform.md), i.e. if the custom-origin transform is applied to a layout view, because it must be converted into a translation matrix.

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [TransformOrigin(Vector2, Vector2)](#transformoriginvector2-vector2) | Describes the origin point to use for a local [Transform](transform.md). | 

### Fields

 | Name | Description |
| --- | --- |
| [Default](#default) | Default origin, with both [Relative](transformorigin.md#relative) and [Absolute](transformorigin.md#absolute) set to [Zero](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html#Microsoft_Xna_Framework_Vector2). | 

### Properties

 | Name | Description |
| --- | --- |
| [Absolute](#absolute) | The pixel position of the exact origin point, relative to the transformed view's top-left corner. | 
| [EqualityContract](#equalitycontract) |  | 
| [Relative](#relative) | The relative position with [X](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html#Microsoft_Xna_Framework_Vector2) and [Y](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html#Microsoft_Xna_Framework_Vector2) values between `0` and `1`, where `(0, 0)` is the top-left, `(0.5, 0.5)` is the middle, and `(1, 1)` is the bottom right. | 

## Details

### Constructors

#### TransformOrigin(Vector2, Vector2)

Describes the origin point to use for a local [Transform](transform.md).

```cs
public TransformOrigin(Microsoft.Xna.Framework.Vector2 Relative, Microsoft.Xna.Framework.Vector2 Absolute);
```

##### Parameters

**`Relative`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
The relative position with [X](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html#Microsoft_Xna_Framework_Vector2) and [Y](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html#Microsoft_Xna_Framework_Vector2) values between `0` and `1`, where `(0, 0)` is the top-left, `(0.5, 0.5)` is the middle, and `(1, 1)` is the bottom right.

**`Absolute`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
The pixel position of the exact origin point, relative to the transformed view's top-left corner.

##### Remarks

Origin data needs to track two vectors; the relative or percentage position with [X](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html#Microsoft_Xna_Framework_Vector2) and [Y](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html#Microsoft_Xna_Framework_Vector2) between `0` and `1` (e.g. the center of the layout would be `(0.5, 0.5)`) as well as the absolute or pixel position. 

 The relative position is used for individual drawing operations; when drawing a single sprite or text string, the XNA drawing APIs use this exact origin vector. The absolute position, on the other hand, is required for transform propagation in the [GlobalTransform](globaltransform.md), i.e. if the custom-origin transform is applied to a layout view, because it must be converted into a translation matrix.

-----

### Fields

#### Default

Default origin, with both [Relative](transformorigin.md#relative) and [Absolute](transformorigin.md#absolute) set to [Zero](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html#Microsoft_Xna_Framework_Vector2).

```cs
public static readonly StardewUI.Graphics.TransformOrigin Default;
```

##### Field Value

[TransformOrigin](transformorigin.md)

-----

### Properties

#### Absolute

The pixel position of the exact origin point, relative to the transformed view's top-left corner.

```cs
public Microsoft.Xna.Framework.Vector2 Absolute { get; set; }
```

##### Property Value

[Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)

-----

#### EqualityContract



```cs
protected System.Type EqualityContract { get; }
```

##### Property Value

[Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)

-----

#### Relative

The relative position with [X](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html#Microsoft_Xna_Framework_Vector2) and [Y](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html#Microsoft_Xna_Framework_Vector2) values between `0` and `1`, where `(0, 0)` is the top-left, `(0.5, 0.5)` is the middle, and `(1, 1)` is the bottom right.

```cs
public Microsoft.Xna.Framework.Vector2 Relative { get; set; }
```

##### Property Value

[Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)

-----

