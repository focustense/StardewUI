---
title: Transform
description: Global transform applied to an ISpriteBatch.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class Transform

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Graphics](index.md)  
Assembly: StardewUI.dll  

</div>

Global transform applied to an [ISpriteBatch](ispritebatch.md).

```cs
public record Transform : IEquatable<StardewUI.Graphics.Transform>
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ Transform

**Implements**  
[IEquatable](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1)<[Transform](transform.md)>

## Remarks

The order of the different translation parameters reflects the actual order in which the transformations will be applied in a [GlobalTransform](globaltransform.md). Scaling before rotation prevents unexpected skewing, and rotating before translation keeps the coordinate system intact. 

 To deliberately apply the individual operations in a different order, use separate [Transform](transform.md) instances applied in sequence, or simply compute the [Matrix](https://docs.monogame.net/api/Microsoft.Xna.Framework.Matrix.html) directly.

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [Transform(Vector2, Single, Vector2)](#transformvector2-float-vector2) | Global transform applied to an [ISpriteBatch](ispritebatch.md). | 

### Fields

 | Name | Description |
| --- | --- |
| [Default](#default) | Default instance with no transformations applied. | 

### Properties

 | Name | Description |
| --- | --- |
| [EqualityContract](#equalitycontract) |  | 
| [HasNonUniformScale](#hasnonuniformscale) | Whether the current instance has a non-uniform [Scale](transform.md#scale). | 
| [HasRotation](#hasrotation) | Whether the current instance has a non-zero [Rotation](transform.md#rotation). | 
| [HasScale](#hasscale) | Whether the current instance has non-unity [Scale](transform.md#scale), regardless of uniformity. | 
| [HasTranslation](#hastranslation) | Whether the current instance has a non-zero [Translation](transform.md#translation). | 
| [IsOriginRelative](#isoriginrelative) | Whether the current transform is affected by transform origin. | 
| [Rotation](#rotation) | 2D rotation (always along Z axis) to apply, in radians. | 
| [Scale](#scale) | The scale at which to draw. [One](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html#Microsoft_Xna_Framework_Vector2) is unity scale (i.e. no scaling). | 
| [Translation](#translation) | Translation offset for drawn content. | 

### Methods

 | Name | Description |
| --- | --- |
| [CanMergeLocally(Transform)](#canmergelocallytransform) | Checks if a subsequent transform can be merged into this one while preserving the result as a simple local transform, i.e. not requiring the use of a transformation matrix. | 
| [CanMergeLocally(Vector2, Single, Vector2)](#canmergelocallyvector2-float-vector2) | Checks if a subsequent transform, represented by its individual components, can be merged into this one while preserving the result as a simple local transform, i.e. not requiring the use of a transformation matrix. | 
| [FromRotation(Single)](#fromrotationfloat) | Creates a new [Transform](transform.md) that applies a specific 2D rotation. | 
| [FromScale(Vector2)](#fromscalevector2) | Creates a [Transform](transform.md) using a specified scale. | 
| [FromTranslation(Vector2)](#fromtranslationvector2) | Creates a [Transform](transform.md) using a specified translation offset. | 
| [IsRectangular()](#isrectangular) | Checks if this transform represents a single rectangular area of the parent, i.e. not rotated or skewed. | 
| [ToMatrix()](#tomatrix) | Creates a transformation matrix from the properties of this transform. | 

## Details

### Constructors

#### Transform(Vector2, float, Vector2)

Global transform applied to an [ISpriteBatch](ispritebatch.md).

```cs
public Transform(Microsoft.Xna.Framework.Vector2 Scale, float Rotation, Microsoft.Xna.Framework.Vector2 Translation);
```

##### Parameters

**`Scale`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
The scale at which to draw. [One](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html#Microsoft_Xna_Framework_Vector2) is unity scale (i.e. no scaling).

**`Rotation`** &nbsp; [Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)  
2D rotation (always along Z axis) to apply, in radians.

**`Translation`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
Translation offset for drawn content.

##### Remarks

The order of the different translation parameters reflects the actual order in which the transformations will be applied in a [GlobalTransform](globaltransform.md). Scaling before rotation prevents unexpected skewing, and rotating before translation keeps the coordinate system intact. 

 To deliberately apply the individual operations in a different order, use separate [Transform](transform.md) instances applied in sequence, or simply compute the [Matrix](https://docs.monogame.net/api/Microsoft.Xna.Framework.Matrix.html) directly.

-----

### Fields

#### Default

Default instance with no transformations applied.

```cs
public static readonly StardewUI.Graphics.Transform Default;
```

##### Field Value

[Transform](transform.md)

-----

### Properties

#### EqualityContract



```cs
protected System.Type EqualityContract { get; }
```

##### Property Value

[Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)

-----

#### HasNonUniformScale

Whether the current instance has a non-uniform [Scale](transform.md#scale).

```cs
public bool HasNonUniformScale { get; }
```

##### Property Value

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

##### Remarks

Non-uniform scale sometimes needs to be treated differently from uniform scale because the former is not commutative with rotation.

-----

#### HasRotation

Whether the current instance has a non-zero [Rotation](transform.md#rotation).

```cs
public bool HasRotation { get; }
```

##### Property Value

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

-----

#### HasScale

Whether the current instance has non-unity [Scale](transform.md#scale), regardless of uniformity.

```cs
public bool HasScale { get; }
```

##### Property Value

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

-----

#### HasTranslation

Whether the current instance has a non-zero [Translation](transform.md#translation).

```cs
public bool HasTranslation { get; }
```

##### Property Value

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

-----

#### IsOriginRelative

Whether the current transform is affected by transform origin.

```cs
public bool IsOriginRelative { get; }
```

##### Property Value

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

##### Remarks

Some types of transformations, specifically translation, have outcomes independent of the transformation origin and should therefore not attempt to use it or pass it on to global transforms.

-----

#### Rotation

2D rotation (always along Z axis) to apply, in radians.

```cs
public float Rotation { get; set; }
```

##### Property Value

[Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)

-----

#### Scale

The scale at which to draw. [One](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html#Microsoft_Xna_Framework_Vector2) is unity scale (i.e. no scaling).

```cs
public Microsoft.Xna.Framework.Vector2 Scale { get; set; }
```

##### Property Value

[Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)

-----

#### Translation

Translation offset for drawn content.

```cs
public Microsoft.Xna.Framework.Vector2 Translation { get; set; }
```

##### Property Value

[Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)

-----

### Methods

#### CanMergeLocally(Transform)

Checks if a subsequent transform can be merged into this one while preserving the result as a simple local transform, i.e. not requiring the use of a transformation matrix.

```cs
public bool CanMergeLocally(StardewUI.Graphics.Transform next);
```

##### Parameters

**`next`** &nbsp; [Transform](transform.md)  
The local transformation to apply after the current instance.

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  `true` if the cumulative sequence of transformations can continue to be represented as a simple local [Transform](transform.md); `false` if the combination requires converting [ToMatrix()](transform.md#tomatrix) and subsequent inclusion into a new [GlobalTransform](globaltransform.md).

##### Remarks

Local transforms can be merged if they are either: 

  -  Mathematically commutative with the existing properties, such as uniform scaling, or additional translation on a local transform that is _only_ translation; or 
  -  Following the same transformation order that applies during the various [Draw(Texture2D, Vector2, Rectangle?, Color, Single, Vector2, Vector2, SpriteEffects, Single)](https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.SpriteBatch.html#Microsoft_Xna_Framework_Graphics_SpriteBatch) and [DrawString(SpriteFont, string, Vector2, Color)](https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.SpriteBatch.html#Microsoft_Xna_Framework_Graphics_SpriteBatch) methods, i.e. [Scale](transform.md#scale) followed by [Rotation](transform.md#rotation) followed by [Translation](transform.md#translation). For example, if the current instance has [Scale](transform.md#scale) and [Rotation](transform.md#rotation), and the `next` transform has `Rotation` only, then the rotations can be trivially summed.

-----

#### CanMergeLocally(Vector2, float, Vector2)

Checks if a subsequent transform, represented by its individual components, can be merged into this one while preserving the result as a simple local transform, i.e. not requiring the use of a transformation matrix.

```cs
public bool CanMergeLocally(Microsoft.Xna.Framework.Vector2 scale, float rotation, Microsoft.Xna.Framework.Vector2 translation);
```

##### Parameters

**`scale`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
The [Scale](transform.md#scale) component of the next transform.

**`rotation`** &nbsp; [Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)  
The [Rotation](transform.md#rotation) component of the next transform.

**`translation`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
The [Translation](transform.md#translation) component of the next transform.

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

-----

#### FromRotation(float)

Creates a new [Transform](transform.md) that applies a specific 2D rotation.

```cs
public static StardewUI.Graphics.Transform FromRotation(float angle);
```

##### Parameters

**`angle`** &nbsp; [Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)  
The rotation angle, in radians.

##### Returns

[Transform](transform.md)

  A [Transform](transform.md) whose [Rotation](transform.md#rotation) is equal to the specified `angle`.

-----

#### FromScale(Vector2)

Creates a [Transform](transform.md) using a specified scale.

```cs
public static StardewUI.Graphics.Transform FromScale(Microsoft.Xna.Framework.Vector2 scale);
```

##### Parameters

**`scale`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
The scale to apply.

##### Returns

[Transform](transform.md)

  A [Transform](transform.md) whose [Scale](transform.md#scale) is equal to the specified `scale`.

-----

#### FromTranslation(Vector2)

Creates a [Transform](transform.md) using a specified translation offset.

```cs
public static StardewUI.Graphics.Transform FromTranslation(Microsoft.Xna.Framework.Vector2 translation);
```

##### Parameters

**`translation`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
The translation offset.

##### Returns

[Transform](transform.md)

  A [Transform](transform.md) whose [Translation](transform.md#translation) is equal to the specified `translation`.

-----

#### IsRectangular()

Checks if this transform represents a single rectangular area of the parent, i.e. not rotated or skewed.

```cs
public bool IsRectangular();
```

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

-----

#### ToMatrix()

Creates a transformation matrix from the properties of this transform.

```cs
public Microsoft.Xna.Framework.Matrix ToMatrix();
```

##### Returns

[Matrix](https://docs.monogame.net/api/Microsoft.Xna.Framework.Matrix.html)

  A transformation matrix equivalent to this transform.

##### Remarks

The created matrix, when used with [Begin(SpriteSortMode, BlendState, SamplerState, DepthStencilState, RasterizerState, Effect, Matrix?)](https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.SpriteBatch.html#Microsoft_Xna_Framework_Graphics_SpriteBatch), will have the same effect as if the current [Scale](transform.md#scale), [Rotation](transform.md#rotation) and [Translation](transform.md#translation) were to be provided directly as arguments to the sprite or text drawing method(s).

-----

