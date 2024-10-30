---
title: ViewChild
description: Provides information about a view that is the child of another view. Used for interactions.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class ViewChild

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI](index.md)  
Assembly: StardewUI.dll  

</div>

Provides information about a view that is the child of another view. Used for interactions.

```cs
public record ViewChild : StardewUI.Layout.IOffsettable<StardewUI.ViewChild>, 
    IEquatable<StardewUI.ViewChild>
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ ViewChild

**Implements**  
[IOffsettable](layout/ioffsettable-1.md)<[ViewChild](viewchild.md)>, [IEquatable](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1)<[ViewChild](viewchild.md)>

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [ViewChild(IView, Vector2)](#viewchildiview-vector2) | Provides information about a view that is the child of another view. Used for interactions. | 

### Properties

 | Name | Description |
| --- | --- |
| [EqualityContract](#equalitycontract) |  | 
| [Position](#position) | The position of the `View`, relative to the parent. | 
| [View](#view) | The child view. | 

### Methods

 | Name | Description |
| --- | --- |
| [Center()](#center) | Gets the point at the exact center of the view. | 
| [CenterPoint()](#centerpoint) | Gets the nearest whole pixel point at the exact center of the view. | 
| [ContainsPoint(Vector2)](#containspointvector2) | Checks if a given point, relative to the view's parent, is within the bounds of this child. | 
| [FocusSearch(Vector2, Direction)](#focussearchvector2-direction) | Performs a focus search on the referenced view. | 
| [GetActualBounds()](#getactualbounds) | Returns a [Bounds](layout/bounds.md) representing the parent-relative layout bounds of this child. | 
| [GetContentBounds()](#getcontentbounds) | Returns a [Bounds](layout/bounds.md) representing the parent-relative content bounds of this child. | 
| [IsInDirection(Vector2, Direction)](#isindirectionvector2-direction) | Checks if a view can be reached by travelling from a given point in a given direction. | 
| [Offset(Vector2)](#offsetvector2) | Offsets the position by a given distance. | 

## Details

### Constructors

#### ViewChild(IView, Vector2)

Provides information about a view that is the child of another view. Used for interactions.

```cs
public ViewChild(StardewUI.IView View, Microsoft.Xna.Framework.Vector2 Position);
```

##### Parameters

**`View`** &nbsp; [IView](iview.md)  
The child view.

**`Position`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
The position of the `View`, relative to the parent.

-----

### Properties

#### EqualityContract



```cs
protected System.Type EqualityContract { get; }
```

##### Property Value

[Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)

-----

#### Position

The position of the `View`, relative to the parent.

```cs
public Microsoft.Xna.Framework.Vector2 Position { get; set; }
```

##### Property Value

[Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)

-----

#### View

The child view.

```cs
public StardewUI.IView View { get; set; }
```

##### Property Value

[IView](iview.md)

-----

### Methods

#### Center()

Gets the point at the exact center of the view.

```cs
public Microsoft.Xna.Framework.Vector2 Center();
```

##### Returns

[Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)

-----

#### CenterPoint()

Gets the nearest whole pixel point at the exact center of the view.

```cs
public Microsoft.Xna.Framework.Point CenterPoint();
```

##### Returns

[Point](https://docs.monogame.net/api/Microsoft.Xna.Framework.Point.html)

-----

#### ContainsPoint(Vector2)

Checks if a given point, relative to the view's parent, is within the bounds of this child.

```cs
public bool ContainsPoint(Microsoft.Xna.Framework.Vector2 point);
```

##### Parameters

**`point`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
The point to test.

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  `true` if `point` is within the parent-relative bounds of this child; otherwise `false`.

-----

#### FocusSearch(Vector2, Direction)

Performs a focus search on the referenced view.

```cs
public StardewUI.Input.FocusSearchResult FocusSearch(Microsoft.Xna.Framework.Vector2 contentPosition, StardewUI.Direction direction);
```

##### Parameters

**`contentPosition`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
The current position, relative to the parent that owns this child.

**`direction`** &nbsp; [Direction](direction.md)  
The direction of cursor movement.

##### Returns

[FocusSearchResult](input/focussearchresult.md)

  The next focusable view reached by moving in the specified `direction`, or `null` if there are no focusable descendants that are possible to reach in that direction.

##### Remarks

This is equivalent to [FocusSearch(Vector2, Direction)](iview.md#focussearchvector2-direction) but implicitly handles its own [Position](viewchild.md#position), so it can be used recursively without directly adjusting any coordinates.

-----

#### GetActualBounds()

Returns a [Bounds](layout/bounds.md) representing the parent-relative layout bounds of this child.

```cs
public StardewUI.Layout.Bounds GetActualBounds();
```

##### Returns

[Bounds](layout/bounds.md)

##### Remarks

Equivalent to the [ActualBounds](iview.md#actualbounds) offset by this child's [Position](viewchild.md#position).

-----

#### GetContentBounds()

Returns a [Bounds](layout/bounds.md) representing the parent-relative content bounds of this child.

```cs
public StardewUI.Layout.Bounds GetContentBounds();
```

##### Returns

[Bounds](layout/bounds.md)

##### Remarks

Equivalent to the [ContentBounds](iview.md#contentbounds) offset by this child's [Position](viewchild.md#position).

-----

#### IsInDirection(Vector2, Direction)

Checks if a view can be reached by travelling from a given point in a given direction.

```cs
public bool IsInDirection(Microsoft.Xna.Framework.Vector2 origin, StardewUI.Direction direction);
```

##### Parameters

**`origin`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
The origin point.

**`direction`** &nbsp; [Direction](direction.md)  
The direction from `origin`.

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  `true` if the view's boundaries either already contain the `origin` or are in the specified `direction` from the `origin`; otherwise `false`.

-----

#### Offset(Vector2)

Offsets the position by a given distance.

```cs
public StardewUI.ViewChild Offset(Microsoft.Xna.Framework.Vector2 distance);
```

##### Parameters

**`distance`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
The offset distance.

##### Returns

[ViewChild](viewchild.md)

  A copy of the current [ViewChild](viewchild.md) having the same [View](viewchild.md#view) and a [Position](viewchild.md#position) offset by `distance`.

-----

