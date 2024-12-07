---
title: Edges
description: Describes a set of edge dimensions, e.g. for margin or padding.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class Edges

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Layout](index.md)  
Assembly: StardewUI.dll  

</div>

Describes a set of edge dimensions, e.g. for margin or padding.

```cs
[StardewUI.DuckType]
public record Edges : IEquatable<StardewUI.Layout.Edges>
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ Edges

**Implements**  
[IEquatable](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1)<[Edges](edges.md)>

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [Edges(Int32, Int32, Int32, Int32)](#edgesint-int-int-int) | Describes a set of edge dimensions, e.g. for margin or padding. | 
| [Edges(Int32)](#edgesint) | Initializes a new [Edges](edges.md) with all edges set to the same value. | 
| [Edges(Int32, Int32)](#edgesint-int) | Initialies a new [Edges](edges.md) with symmetrical horizontal and vertical values. | 

### Fields

 | Name | Description |
| --- | --- |
| [NONE](#none) | An [Edges](edges.md) instance with all edges set to zero. | 

### Properties

 | Name | Description |
| --- | --- |
| [Bottom](#bottom) | The bottom edge. | 
| [EqualityContract](#equalitycontract) |  | 
| [Horizontal](#horizontal) | Gets the total value for all horizontal edges ([Left](edges.md#left) + [Right](edges.md#right)). | 
| [Left](#left) | The left edge. | 
| [Right](#right) | The right edge. | 
| [Top](#top) | The top edge. | 
| [Total](#total) | The total size occupied by all edges. | 
| [Vertical](#vertical) | Gets the total value for all vertical edges ([Top](edges.md#top) + [Bottom](edges.md#bottom)). | 

### Methods

 | Name | Description |
| --- | --- |
| [Equals(Edges)](#equalsedges) |  | 
| [GetHashCode()](#gethashcode) | <span class="muted" markdown>(Overrides [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object).`GetHashCode()`)</span> | 
| [HorizontalOnly()](#horizontalonly) | Gets a copy of this instance with only the horizontal edges set (vertical edges zeroed out). | 
| [Parse(string)](#parsestring) | Parses an [Edges](edges.md) value from a comma-separated string representation. | 
| [Rotate(SimpleRotation)](#rotatesimplerotation) | Rotates the edges, transposing the individual edge values. | 
| [ToString()](#tostring) | <span class="muted" markdown>(Overrides [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object).`ToString()`)</span> | 
| [VerticalOnly()](#verticalonly) | Gets a copy of this instance with only the vertical edges set (horizontal edges zeroed out). | 

## Details

### Constructors

#### Edges(int, int, int, int)

Describes a set of edge dimensions, e.g. for margin or padding.

```cs
public Edges(int Left, int Top, int Right, int Bottom);
```

##### Parameters

**`Left`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)  
The left edge.

**`Top`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)  
The top edge.

**`Right`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)  
The right edge.

**`Bottom`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)  
The bottom edge.

-----

#### Edges(int)

Initializes a new [Edges](edges.md) with all edges set to the same value.

```cs
public Edges(int all);
```

##### Parameters

**`all`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)  
Common value for all edges.

-----

#### Edges(int, int)

Initialies a new [Edges](edges.md) with symmetrical horizontal and vertical values.

```cs
public Edges(int horizontal, int vertical);
```

##### Parameters

**`horizontal`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)  
Common value for the [Left](edges.md#left) and [Right](edges.md#right) edges.

**`vertical`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)  
Common value for the [Top](edges.md#top) and [Bottom](edges.md#bottom) edges.

-----

### Fields

#### NONE

An [Edges](edges.md) instance with all edges set to zero.

```cs
public static readonly StardewUI.Layout.Edges NONE;
```

##### Field Value

[Edges](edges.md)

-----

### Properties

#### Bottom

The bottom edge.

```cs
public int Bottom { get; set; }
```

##### Property Value

[Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)

-----

#### EqualityContract



```cs
protected System.Type EqualityContract { get; }
```

##### Property Value

[Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)

-----

#### Horizontal

Gets the total value for all horizontal edges ([Left](edges.md#left) + [Right](edges.md#right)).

```cs
public int Horizontal { get; }
```

##### Property Value

[Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)

-----

#### Left

The left edge.

```cs
public int Left { get; set; }
```

##### Property Value

[Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)

-----

#### Right

The right edge.

```cs
public int Right { get; set; }
```

##### Property Value

[Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)

-----

#### Top

The top edge.

```cs
public int Top { get; set; }
```

##### Property Value

[Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)

-----

#### Total

The total size occupied by all edges.

```cs
public Microsoft.Xna.Framework.Vector2 Total { get; }
```

##### Property Value

[Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)

-----

#### Vertical

Gets the total value for all vertical edges ([Top](edges.md#top) + [Bottom](edges.md#bottom)).

```cs
public int Vertical { get; }
```

##### Property Value

[Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)

-----

### Methods

#### Equals(Edges)



```cs
public virtual bool Equals(StardewUI.Layout.Edges other);
```

##### Parameters

**`other`** &nbsp; [Edges](edges.md)

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

##### Remarks

Overrides the default implementation to avoid using reflection on every frame during dirty checks.

-----

#### GetHashCode()



```cs
public override int GetHashCode();
```

##### Returns

[Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)

-----

#### HorizontalOnly()

Gets a copy of this instance with only the horizontal edges set (vertical edges zeroed out).

```cs
public StardewUI.Layout.Edges HorizontalOnly();
```

##### Returns

[Edges](edges.md)

-----

#### Parse(string)

Parses an [Edges](edges.md) value from a comma-separated string representation.

```cs
public static StardewUI.Layout.Edges Parse(string value);
```

##### Parameters

**`value`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The formatted edges to parse.

##### Returns

[Edges](edges.md)

  The parsed [Edges](edges.md).

##### Remarks

The behavior depends on the number of comma-separated tokens in the string, equivalent to the constructor overload with that number of parameters: 

  - A single value will give all edges the same length
  - Two values will set the horizontal (left/right) and vertical (top/bottom) lengths
  - Four values will set each length individually
  - Any other format will throw [FormatException](https://learn.microsoft.com/en-us/dotnet/api/system.formatexception).

-----

#### Rotate(SimpleRotation)

Rotates the edges, transposing the individual edge values.

```cs
public StardewUI.Layout.Edges Rotate(StardewUI.Graphics.SimpleRotation rotation);
```

##### Parameters

**`rotation`** &nbsp; [SimpleRotation](../graphics/simplerotation.md)  
The rotation type (angle).

##### Returns

[Edges](edges.md)

  A rotated copy of this [Edges](edges.md) instance.

-----

#### ToString()



```cs
public override string ToString();
```

##### Returns

[string](https://learn.microsoft.com/en-us/dotnet/api/system.string)

-----

#### VerticalOnly()

Gets a copy of this instance with only the vertical edges set (horizontal edges zeroed out).

```cs
public StardewUI.Layout.Edges VerticalOnly();
```

##### Returns

[Edges](edges.md)

-----

