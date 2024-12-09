---
title: GridItemLayout.Length
description: A GridItemLayout specifying that each item is to have the same fixed length (width or height, depending on the grid's Orientation) and to wrap to the next row/column afterward.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class GridItemLayout.Length

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Widgets](index.md)  
Assembly: StardewUI.dll  

</div>

A [GridItemLayout](griditemlayout.md) specifying that each item is to have the same fixed length (width or height, depending on the grid's [Orientation](../layout/orientation.md)) and to wrap to the next row/column afterward.

```cs
public record GridItemLayout.Length : StardewUI.Widgets.GridItemLayout, 
    IEquatable<StardewUI.Widgets.GridItemLayout.Length>
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ [GridItemLayout](griditemlayout.md) ⇦ Length

**Implements**  
[IEquatable](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1)<[Length](griditemlayout.length.md)>

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [Length(Single, Boolean)](#lengthfloat-bool) | A [GridItemLayout](griditemlayout.md) specifying that each item is to have the same fixed length (width or height, depending on the grid's [Orientation](../layout/orientation.md)) and to wrap to the next row/column afterward. | 

### Properties

 | Name | Description |
| --- | --- |
| [EqualityContract](#equalitycontract) | <span class="muted" markdown>(Overrides [GridItemLayout](griditemlayout.md).`get_EqualityContract()`)</span> | 
| [Expandable](#expandable) | Whether the length of cells should expand (uniformly) to fill the grid's primary layout size, i.e. when that size is not an exact multiple of the total size of all cells that can fit. | 
| [Px](#px) | The length, in pixels, of each item along the grid's orientation axis. | 

### Methods

 | Name | Description |
| --- | --- |
| [GetItemCountAndLength(Single, Single)](#getitemcountandlengthfloat-float) | Computes the length (along the grid's [PrimaryOrientation](grid.md#primaryorientation) axis) of a single item, and the number of items that can fit before wrapping.<br><span class="muted" markdown>(Overrides [GridItemLayout](griditemlayout.md).[GetItemCountAndLength(Single, Single)](griditemlayout.md#getitemcountandlengthfloat-float))</span> | 

## Details

### Constructors

#### Length(float, bool)

A [GridItemLayout](griditemlayout.md) specifying that each item is to have the same fixed length (width or height, depending on the grid's [Orientation](../layout/orientation.md)) and to wrap to the next row/column afterward.

```cs
public Length(float Px, bool Expandable);
```

##### Parameters

**`Px`** &nbsp; [Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)  
The length, in pixels, of each item along the grid's orientation axis.

**`Expandable`** &nbsp; [Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)  
Whether the length of cells should expand (uniformly) to fill the grid's primary layout size, i.e. when that size is not an exact multiple of the total size of all cells that can fit.

-----

### Properties

#### EqualityContract



```cs
protected System.Type EqualityContract { get; }
```

##### Property Value

[Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)

-----

#### Expandable

Whether the length of cells should expand (uniformly) to fill the grid's primary layout size, i.e. when that size is not an exact multiple of the total size of all cells that can fit.

```cs
public bool Expandable { get; set; }
```

##### Property Value

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

-----

#### Px

The length, in pixels, of each item along the grid's orientation axis.

```cs
public float Px { get; set; }
```

##### Property Value

[Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)

-----

### Methods

#### GetItemCountAndLength(float, float)

Computes the length (along the grid's [PrimaryOrientation](grid.md#primaryorientation) axis) of a single item, and the number of items that can fit before wrapping.

```cs
public override ValueTuple<System.Single, System.Int32> GetItemCountAndLength(float available, float spacing);
```

##### Parameters

**`available`** &nbsp; [Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)  
The length available along the same axis.

**`spacing`** &nbsp; [Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)  
Spacing between items, to adjust count-based layouts.

##### Returns

[ValueTuple](https://learn.microsoft.com/en-us/dotnet/api/system.valuetuple-2)<[Single](https://learn.microsoft.com/en-us/dotnet/api/system.single), [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)>

  The length to apply to each item.

-----

