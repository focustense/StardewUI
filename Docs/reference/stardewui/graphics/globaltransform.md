---
title: GlobalTransform
description: Global transform applied to an ISpriteBatch.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class GlobalTransform

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Graphics](index.md)  
Assembly: StardewUI.dll  

</div>

Global transform applied to an [ISpriteBatch](ispritebatch.md).

```cs
public record GlobalTransform : IEquatable<StardewUI.Graphics.GlobalTransform>
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ GlobalTransform

**Implements**  
[IEquatable](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1)<[GlobalTransform](globaltransform.md)>

## Remarks

Because the [SpriteBatch](https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.SpriteBatch.html) in MonoGame/XNA has scale and rotation parameters for individual draw methods (whether texture or text), which are presumably more optimized than computing a new global transform matrix and restarting the sprite batch, the global transform maintains a current local transform and only "merges" it into the transform matrix once the accumulated local transform can no longer be represented in one [Transform](transform.md). 

 These "local" transforms actually represent the model matrix, while the accumulated global transform is the view matrix, so the relationship between them is quirky; see [CanMergeLocally(Transform)](transform.md#canmergelocallytransform) for more details on the process. 

 Global transforms are always "around" the viewport origin (0, 0). To use a different origin relative to the current view, first translated by the negated origin position, then apply regular transforms, then translate by the (positive) origin position again.

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [GlobalTransform(Matrix, Transform, TransformOrigin)](#globaltransformmatrix-transform-transformorigin) | Global transform applied to an [ISpriteBatch](ispritebatch.md). | 

### Fields

 | Name | Description |
| --- | --- |
| [Default](#default) | The default instance, which applies no transformation. | 

### Properties

 | Name | Description |
| --- | --- |
| [EqualityContract](#equalitycontract) |  | 
| [Local](#local) | Local transform to apply after the `Matrix` takes effect. | 
| [LocalOrigin](#localorigin) | Origin for the [Local](globaltransform.md#local) transform. | 
| [Matrix](#matrix) | The cumulative transformation matrix. | 

### Methods

 | Name | Description |
| --- | --- |
| [Apply(Transform, TransformOrigin, Boolean)](#applytransform-transformorigin-boolean) | Applies a local transformation and returns the new, accumulated global transform. | 
| [Collapse()](#collapse) | Merges the [Local](globaltransform.md#local) component into the global [Matrix](globaltransform.md#matrix). | 

## Details

### Constructors

#### GlobalTransform(Matrix, Transform, TransformOrigin)

Global transform applied to an [ISpriteBatch](ispritebatch.md).

```cs
public GlobalTransform(Microsoft.Xna.Framework.Matrix Matrix, StardewUI.Graphics.Transform Local, StardewUI.Graphics.TransformOrigin LocalOrigin);
```

##### Parameters

**`Matrix`** &nbsp; [Matrix](https://docs.monogame.net/api/Microsoft.Xna.Framework.Matrix.html)  
The cumulative transformation matrix.

**`Local`** &nbsp; [Transform](transform.md)  
Local transform to apply after the `Matrix` takes effect.

**`LocalOrigin`** &nbsp; [TransformOrigin](transformorigin.md)  
Origin for the [Local](globaltransform.md#local) transform.

##### Remarks

Because the [SpriteBatch](https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.SpriteBatch.html) in MonoGame/XNA has scale and rotation parameters for individual draw methods (whether texture or text), which are presumably more optimized than computing a new global transform matrix and restarting the sprite batch, the global transform maintains a current local transform and only "merges" it into the transform matrix once the accumulated local transform can no longer be represented in one [Transform](transform.md). 

 These "local" transforms actually represent the model matrix, while the accumulated global transform is the view matrix, so the relationship between them is quirky; see [CanMergeLocally(Transform)](transform.md#canmergelocallytransform) for more details on the process. 

 Global transforms are always "around" the viewport origin (0, 0). To use a different origin relative to the current view, first translated by the negated origin position, then apply regular transforms, then translate by the (positive) origin position again.

-----

### Fields

#### Default

The default instance, which applies no transformation.

```cs
public static readonly StardewUI.Graphics.GlobalTransform Default;
```

##### Field Value

[GlobalTransform](globaltransform.md)

-----

### Properties

#### EqualityContract



```cs
protected System.Type EqualityContract { get; }
```

##### Property Value

[Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)

-----

#### Local

Local transform to apply after the `Matrix` takes effect.

```cs
public StardewUI.Graphics.Transform Local { get; set; }
```

##### Property Value

[Transform](transform.md)

-----

#### LocalOrigin

Origin for the [Local](globaltransform.md#local) transform.

```cs
public StardewUI.Graphics.TransformOrigin LocalOrigin { get; set; }
```

##### Property Value

[TransformOrigin](transformorigin.md)

-----

#### Matrix

The cumulative transformation matrix.

```cs
public Microsoft.Xna.Framework.Matrix Matrix { get; set; }
```

##### Property Value

[Matrix](https://docs.monogame.net/api/Microsoft.Xna.Framework.Matrix.html)

-----

### Methods

#### Apply(Transform, TransformOrigin, Boolean)

Applies a local transformation and returns the new, accumulated global transform.

```cs
public StardewUI.Graphics.GlobalTransform Apply(StardewUI.Graphics.Transform transform, StardewUI.Graphics.TransformOrigin origin, out System.Boolean isNewMatrix);
```

##### Parameters

**`transform`** &nbsp; [Transform](transform.md)  
The local transform to apply.

**`origin`** &nbsp; [TransformOrigin](transformorigin.md)  
Origin position for the `transform`.

**`isNewMatrix`** &nbsp; [Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)  
Whether the newly-created [GlobalTransform](globaltransform.md) has a different [Matrix](globaltransform.md#matrix) from the current instance.

##### Returns

[GlobalTransform](globaltransform.md)

  A new [GlobalTransform](globaltransform.md) that combines the accumulated transformation of this instance with the specified `transform`.

-----

#### Collapse()

Merges the [Local](globaltransform.md#local) component into the global [Matrix](globaltransform.md#matrix).

```cs
public StardewUI.Graphics.GlobalTransform Collapse();
```

##### Returns

[GlobalTransform](globaltransform.md)

  A new [GlobalTransform](globaltransform.md) whose [Matrix](globaltransform.md#matrix) is the combined [Matrix](globaltransform.md#matrix) and [Local](globaltransform.md#local) components of this instance, and whose [Local](globaltransform.md#local) transform is reset to the [Default](transform.md#default).

##### Remarks

For use when the [Local](globaltransform.md#local) transform cannot be combined with the model transform of a specific drawing operation.

-----

