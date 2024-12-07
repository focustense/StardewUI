---
title: GridItemLayout.Count
description: A GridItemLayout specifying the maximum divisions - rows or columns, depending on the grid's Orientation; items will be sized distributed uniformly along that axis.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class GridItemLayout.Count

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Widgets](index.md)  
Assembly: StardewUI.dll  

</div>

A [GridItemLayout](griditemlayout.md) specifying the maximum divisions - rows or columns, depending on the grid's [Orientation](../layout/orientation.md); items will be sized distributed uniformly along that axis.

```cs
public record GridItemLayout.Count : StardewUI.Widgets.GridItemLayout, 
    IEquatable<StardewUI.Widgets.GridItemLayout.Count>
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ [GridItemLayout](griditemlayout.md) ⇦ Count

**Implements**  
[IEquatable](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1)<[Count](griditemlayout.count.md)>

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [Count(Int32)](#countint) | A [GridItemLayout](griditemlayout.md) specifying the maximum divisions - rows or columns, depending on the grid's [Orientation](../layout/orientation.md); items will be sized distributed uniformly along that axis. | 

### Properties

 | Name | Description |
| --- | --- |
| [EqualityContract](#equalitycontract) | <span class="muted" markdown>(Overrides [GridItemLayout](griditemlayout.md).`get_EqualityContract()`)</span> | 
| [ItemCount](#itemcount) | Maximum number of cell divisions along the primary orientation axis. | 

### Methods

 | Name | Description |
| --- | --- |
| [GetItemCountAndLength(Single, Single)](#getitemcountandlengthfloat-float) | Computes the length (along the grid's [PrimaryOrientation](grid.md#primaryorientation) axis) of a single item, and the number of items that can fit before wrapping.<br><span class="muted" markdown>(Overrides [GridItemLayout](griditemlayout.md).[GetItemCountAndLength(Single, Single)](griditemlayout.md#getitemcountandlengthfloat-float))</span> | 

## Details

### Constructors

#### Count(int)

A [GridItemLayout](griditemlayout.md) specifying the maximum divisions - rows or columns, depending on the grid's [Orientation](../layout/orientation.md); items will be sized distributed uniformly along that axis.

```cs
public Count(int ItemCount);
```

##### Parameters

**`ItemCount`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)  
Maximum number of cell divisions along the primary orientation axis.

-----

### Properties

#### EqualityContract



```cs
protected System.Type EqualityContract { get; }
```

##### Property Value

[Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)

-----

#### ItemCount

Maximum number of cell divisions along the primary orientation axis.

```cs
public int ItemCount { get; set; }
```

##### Property Value

[Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)

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

