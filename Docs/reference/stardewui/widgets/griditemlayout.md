---
title: GridItemLayout
description: Describes the layout of all items in a Grid.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class GridItemLayout

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Widgets](index.md)  
Assembly: StardewUI.dll  

</div>

Describes the layout of all items in a [Grid](grid.md).

```cs
public record GridItemLayout : IEquatable<StardewUI.Widgets.GridItemLayout>
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ GridItemLayout

**Implements**  
[IEquatable](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1)<[GridItemLayout](griditemlayout.md)>

## Members

### Properties

 | Name | Description |
| --- | --- |
| [EqualityContract](#equalitycontract) |  | 

### Methods

 | Name | Description |
| --- | --- |
| [GetItemCountAndLength(Single, Single)](#getitemcountandlengthfloat-float) | Computes the length (along the grid's [PrimaryOrientation](grid.md#primaryorientation) axis) of a single item, and the number of items that can fit before wrapping. | 
| [Parse(string)](#parsestring) | Converts the string representation of an item layout to an equivalent [GridItemLayout](griditemlayout.md). | 

## Details

### Properties

#### EqualityContract



```cs
protected System.Type EqualityContract { get; }
```

##### Property Value

[Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)

-----

### Methods

#### GetItemCountAndLength(float, float)

Computes the length (along the grid's [PrimaryOrientation](grid.md#primaryorientation) axis) of a single item, and the number of items that can fit before wrapping.

```cs
public virtual ValueTuple<System.Single, System.Int32> GetItemCountAndLength(float available, float spacing);
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

#### Parse(string)

Converts the string representation of an item layout to an equivalent [GridItemLayout](griditemlayout.md).

```cs
public static StardewUI.Widgets.GridItemLayout Parse(string value);
```

##### Parameters

**`value`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
String containing the item layout to convert.

##### Returns

[GridItemLayout](griditemlayout.md)

  The converted layout information.

-----

