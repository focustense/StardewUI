---
title: Bounds
description: A bounding rectangle using floating-point dimensions.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class Bounds

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Layout](index.md)  
Assembly: StardewUI.dll  

</div>

A bounding rectangle using floating-point dimensions.

```cs
[StardewUI.DuckType]
public record Bounds : StardewUI.Layout.IOffsettable<StardewUI.Layout.Bounds>, 
    IEquatable<StardewUI.Layout.Bounds>
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ Bounds

**Implements**  
[IOffsettable](ioffsettable-1.md)<[Bounds](bounds.md)>, [IEquatable](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1)<[Bounds](bounds.md)>

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [Bounds(Vector2, Vector2)](#boundsvector2-vector2) | A bounding rectangle using floating-point dimensions. | 

### Fields

 | Name | Description |
| --- | --- |
| [Empty](#empty) | Empty bounds, used for invalid results. | 

### Properties

 | Name | Description |
| --- | --- |
| [Bottom](#bottom) | The Y value at the bottom edge of the bounding rectangle. | 
| [EqualityContract](#equalitycontract) |  | 
| [Left](#left) | The X value at the left edge of the bounding rectangle. | 
| [Position](#position) | The top-left position. | 
| [Right](#right) | The X value at the right edge of the bounding rectangle. | 
| [Size](#size) | The width and height. | 
| [Top](#top) | The Y value at the top edge of the bounding rectangle. | 

### Methods

 | Name | Description |
| --- | --- |
| [Center()](#center) | Gets the point at the center of the bounding rectangle. | 
| [ContainsBounds(Bounds)](#containsboundsbounds) | Checks if an entire bounding rectangle is fully within these bounds. | 
| [ContainsPoint(Vector2)](#containspointvector2) | Checks if a given point is within the bounds. | 
| [Intersection(Bounds)](#intersectionbounds) | Computes the intersection of this [Bounds](bounds.md) with another instance. | 
| [IntersectsWith(Bounds)](#intersectswithbounds) | Checks if this [Bounds](bounds.md) intersects with another instance, without computing the intersection. | 
| [Offset(Vector2)](#offsetvector2) | Offsets a [Bounds](bounds.md) by a given distance. | 
| [Truncate()](#truncate) | Converts the bounds to an integer [Rectangle](https://docs.monogame.net/api/Microsoft.Xna.Framework.Rectangle.html), truncating any fractional values. | 
| [Union(Bounds)](#unionbounds) | Computes the union of this [Bounds](bounds.md) with another instance. | 

## Details

### Constructors

#### Bounds(Vector2, Vector2)

A bounding rectangle using floating-point dimensions.

```cs
public Bounds(Microsoft.Xna.Framework.Vector2 Position, Microsoft.Xna.Framework.Vector2 Size);
```

##### Parameters

**`Position`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
The top-left position.

**`Size`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
The width and height.

-----

### Fields

#### Empty

Empty bounds, used for invalid results.

```cs
public static readonly StardewUI.Layout.Bounds Empty;
```

##### Field Value

[Bounds](bounds.md)

-----

### Properties

#### Bottom

The Y value at the bottom edge of the bounding rectangle.

```cs
public float Bottom { get; }
```

##### Property Value

[Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)

-----

#### EqualityContract



```cs
protected System.Type EqualityContract { get; }
```

##### Property Value

[Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)

-----

#### Left

The X value at the left edge of the bounding rectangle.

```cs
public float Left { get; }
```

##### Property Value

[Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)

-----

#### Position

The top-left position.

```cs
public Microsoft.Xna.Framework.Vector2 Position { get; set; }
```

##### Property Value

[Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)

-----

#### Right

The X value at the right edge of the bounding rectangle.

```cs
public float Right { get; }
```

##### Property Value

[Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)

-----

#### Size

The width and height.

```cs
public Microsoft.Xna.Framework.Vector2 Size { get; set; }
```

##### Property Value

[Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)

-----

#### Top

The Y value at the top edge of the bounding rectangle.

```cs
public float Top { get; }
```

##### Property Value

[Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)

-----

### Methods

#### Center()

Gets the point at the center of the bounding rectangle.

```cs
public Microsoft.Xna.Framework.Vector2 Center();
```

##### Returns

[Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)

-----

#### ContainsBounds(Bounds)

Checks if an entire bounding rectangle is fully within these bounds.

```cs
public bool ContainsBounds(StardewUI.Layout.Bounds bounds);
```

##### Parameters

**`bounds`** &nbsp; [Bounds](bounds.md)  
The other bounds.

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  `true` if `bounds` are a subset of the current instance; `false` if the two bounds do not overlap or only overlap partially.

-----

#### ContainsPoint(Vector2)

Checks if a given point is within the bounds.

```cs
public bool ContainsPoint(Microsoft.Xna.Framework.Vector2 point);
```

##### Parameters

**`point`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
The point to check.

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  `true` if `point` is inside these bounds; otherwise `false`.

-----

#### Intersection(Bounds)

Computes the intersection of this [Bounds](bounds.md) with another instance.

```cs
public StardewUI.Layout.Bounds Intersection(StardewUI.Layout.Bounds other);
```

##### Parameters

**`other`** &nbsp; [Bounds](bounds.md)  
The other bounds to intersect with.

##### Returns

[Bounds](bounds.md)

  A new [Bounds](bounds.md) whose area is the intersection of this instance and `other`, or [Empty](bounds.md#empty) if they do not overlap.

-----

#### IntersectsWith(Bounds)

Checks if this [Bounds](bounds.md) intersects with another instance, without computing the intersection.

```cs
public bool IntersectsWith(StardewUI.Layout.Bounds other);
```

##### Parameters

**`other`** &nbsp; [Bounds](bounds.md)  
The other bounds to check for intersection.

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  True if this [Bounds](bounds.md) and the `other` bounds have any intersecting area, otherwise `false`.

-----

#### Offset(Vector2)

Offsets a [Bounds](bounds.md) by a given distance.

```cs
public StardewUI.Layout.Bounds Offset(Microsoft.Xna.Framework.Vector2 distance);
```

##### Parameters

**`distance`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
The offset distance.

##### Returns

[Bounds](bounds.md)

  A new [Bounds](bounds.md) with the same size as this instance and a [Position](bounds.md#position) offset by the specified `distance`.

-----

#### Truncate()

Converts the bounds to an integer [Rectangle](https://docs.monogame.net/api/Microsoft.Xna.Framework.Rectangle.html), truncating any fractional values.

```cs
public Microsoft.Xna.Framework.Rectangle Truncate();
```

##### Returns

[Rectangle](https://docs.monogame.net/api/Microsoft.Xna.Framework.Rectangle.html)

##### Remarks

Truncating is the same behavior used in [ToPoint()](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html#Microsoft_Xna_Framework_Vector2), making this consistent with the equivalent component-by-component translation to XNA.

-----

#### Union(Bounds)

Computes the union of this [Bounds](bounds.md) with another instance.

```cs
public StardewUI.Layout.Bounds Union(StardewUI.Layout.Bounds other);
```

##### Parameters

**`other`** &nbsp; [Bounds](bounds.md)  
The other bounds to add to the union.

##### Returns

[Bounds](bounds.md)

  A new [Bounds](bounds.md) whose set is the union of this instance and `other`; i.e. is exactly large enough to contain both bounds.

-----

