---
title: DirectionExtensions
description: Helpers for working with Direction.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class DirectionExtensions

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI](index.md)  
Assembly: StardewUI.dll  

</div>

Helpers for working with [Direction](direction.md).

```cs
public static class DirectionExtensions
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ DirectionExtensions

## Members

### Methods

 | Name | Description |
| --- | --- |
| [GetOrientation(Direction)](#getorientationdirection) | Gets the orientation axis associated with a given `direction`, i.e. whether it flows horizontally or vertically. | 
| [IsHorizontal(Direction)](#ishorizontaldirection) | Returns `true` if the specified `direction` is along the horizontal (width) axis, otherwise `false`. | 
| [IsVertical(Direction)](#isverticaldirection) | Returns `true` if the specified `direction` is along the vertical (height) axis, otherwise `false`. | 

## Details

### Methods

#### GetOrientation(Direction)

Gets the orientation axis associated with a given `direction`, i.e. whether it flows horizontally or vertically.

```cs
public static StardewUI.Layout.Orientation GetOrientation(StardewUI.Direction direction);
```

##### Parameters

**`direction`** &nbsp; [Direction](direction.md)

##### Returns

[Orientation](layout/orientation.md)

-----

#### IsHorizontal(Direction)

Returns `true` if the specified `direction` is along the horizontal (width) axis, otherwise `false`.

```cs
public static bool IsHorizontal(StardewUI.Direction direction);
```

##### Parameters

**`direction`** &nbsp; [Direction](direction.md)

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

-----

#### IsVertical(Direction)

Returns `true` if the specified `direction` is along the vertical (height) axis, otherwise `false`.

```cs
public static bool IsVertical(StardewUI.Direction direction);
```

##### Parameters

**`direction`** &nbsp; [Direction](direction.md)

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

-----

