---
title: NineGridPlacement
description: Model for content placement along a nine-segment grid, i.e. all possible combinations of horizontal and vertical Alignment.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class NineGridPlacement

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Layout](index.md)  
Assembly: StardewUI.dll  

</div>

Model for content placement along a nine-segment grid, i.e. all possible combinations of horizontal and vertical [Alignment](alignment.md).

```cs
[StardewUI.DuckType]
public record NineGridPlacement : IEquatable<StardewUI.Layout.NineGridPlacement>
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ NineGridPlacement

**Implements**  
[IEquatable](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1)<[NineGridPlacement](ninegridplacement.md)>

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [NineGridPlacement(Alignment, Alignment, Point)](#ninegridplacementalignment-alignment-point) | Model for content placement along a nine-segment grid, i.e. all possible combinations of horizontal and vertical [Alignment](alignment.md). | 

### Fields

 | Name | Description |
| --- | --- |
| [StandardPlacements](#standardplacements) | All the standard placements with no [Offset](ninegridplacement.md#offset), arranged from bottom-left to top-right. | 

### Properties

 | Name | Description |
| --- | --- |
| [EqualityContract](#equalitycontract) |  | 
| [HorizontalAlignment](#horizontalalignment) | Content alignment along the horizontal axis. | 
| [Offset](#offset) | Absolute axis-independent pixel offset. | 
| [VerticalAlignment](#verticalalignment) | Content alignment along the vertical axis. | 

### Methods

 | Name | Description |
| --- | --- |
| [AtPosition(Vector2, Vector2, Alignment, Alignment)](#atpositionvector2-vector2-alignment-alignment) | Gets the [NineGridPlacement](ninegridplacement.md) for an alignment pair that resolves to a specified exact position. | 
| [EqualsIgnoringOffset(NineGridPlacement)](#equalsignoringoffsetninegridplacement) | Checks if another [NineGridPlacement](ninegridplacement.md) has the same alignments as this one, regardless of offset. | 
| [GetMargin()](#getmargin) | Calculates what margin should be applied to the content container in order to achieve the [Offset](ninegridplacement.md#offset). | 
| [GetNeighbors(Boolean)](#getneighborsbool) | Gets the [NineGridPlacement](ninegridplacement.md)s that neighbor the current placement, i.e. are reachable in a single [Snap(Direction, Boolean)](ninegridplacement.md#snapdirection-bool). | 
| [GetPosition(Vector2, Vector2)](#getpositionvector2-vector2) | Computes the position of some content within its container bounds. | 
| [IsMiddle()](#ismiddle) | Checks if this placement is aligned to the exact center of the container, not counting [Offset](ninegridplacement.md#offset). | 
| [Nudge(Direction, Int32)](#nudgedirection-int) | Keeps the same alignments, but pushes the content farther in a specific direction. | 
| [Parse(string)](#parsestring) | Parses a [NineGridPlacement](ninegridplacement.md) from its string representation. | 
| [Snap(Direction, Boolean)](#snapdirection-bool) | Snaps to an adjacent grid cell. | 
| [TryParse(string, NineGridPlacement)](#tryparsestring-ninegridplacement) | Attempts to parse a [NineGridPlacement](ninegridplacement.md) from its string representation. | 

## Details

### Constructors

#### NineGridPlacement(Alignment, Alignment, Point)

Model for content placement along a nine-segment grid, i.e. all possible combinations of horizontal and vertical [Alignment](alignment.md).

```cs
public NineGridPlacement(StardewUI.Layout.Alignment HorizontalAlignment, StardewUI.Layout.Alignment VerticalAlignment, Microsoft.Xna.Framework.Point Offset);
```

##### Parameters

**`HorizontalAlignment`** &nbsp; [Alignment](alignment.md)  
Content alignment along the horizontal axis.

**`VerticalAlignment`** &nbsp; [Alignment](alignment.md)  
Content alignment along the vertical axis.

**`Offset`** &nbsp; [Point](https://docs.monogame.net/api/Microsoft.Xna.Framework.Point.html)  
Absolute axis-independent pixel offset.

-----

### Fields

#### StandardPlacements

All the standard placements with no [Offset](ninegridplacement.md#offset), arranged from bottom-left to top-right.

```cs
public static readonly System.Collections.Immutable.IImmutableList<StardewUI.Layout.NineGridPlacement> StandardPlacements;
```

##### Field Value

[IImmutableList](https://learn.microsoft.com/en-us/dotnet/api/system.collections.immutable.iimmutablelist-1)<[NineGridPlacement](ninegridplacement.md)>

-----

### Properties

#### EqualityContract



```cs
protected System.Type EqualityContract { get; }
```

##### Property Value

[Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)

-----

#### HorizontalAlignment

Content alignment along the horizontal axis.

```cs
public StardewUI.Layout.Alignment HorizontalAlignment { get; set; }
```

##### Property Value

[Alignment](alignment.md)

-----

#### Offset

Absolute axis-independent pixel offset.

```cs
public Microsoft.Xna.Framework.Point Offset { get; set; }
```

##### Property Value

[Point](https://docs.monogame.net/api/Microsoft.Xna.Framework.Point.html)

-----

#### VerticalAlignment

Content alignment along the vertical axis.

```cs
public StardewUI.Layout.Alignment VerticalAlignment { get; set; }
```

##### Property Value

[Alignment](alignment.md)

-----

### Methods

#### AtPosition(Vector2, Vector2, Alignment, Alignment)

Gets the [NineGridPlacement](ninegridplacement.md) for an alignment pair that resolves to a specified exact position.

```cs
public static StardewUI.Layout.NineGridPlacement AtPosition(Microsoft.Xna.Framework.Vector2 position, Microsoft.Xna.Framework.Vector2 size, StardewUI.Layout.Alignment horizontalAlignment, StardewUI.Layout.Alignment verticalAlignment);
```

##### Parameters

**`position`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
The target position on screen or within the container.

**`size`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
The size of the viewport or container.

**`horizontalAlignment`** &nbsp; [Alignment](alignment.md)  
The desired horizontal alignment.

**`verticalAlignment`** &nbsp; [Alignment](alignment.md)  
The desired vertical alignment.

##### Returns

[NineGridPlacement](ninegridplacement.md)

  A [NineGridPlacement](ninegridplacement.md) whose [HorizontalAlignment](ninegridplacement.md#horizontalalignment) and [VerticalAlignment](ninegridplacement.md#verticalalignment) match the `horizontalAlignment` and `verticalAlignment`, respectively, and whose [GetPosition(Vector2, Vector2)](ninegridplacement.md#getpositionvector2-vector2) will resolve to exactly the specified `position`.

-----

#### EqualsIgnoringOffset(NineGridPlacement)

Checks if another [NineGridPlacement](ninegridplacement.md) has the same alignments as this one, regardless of offset.

```cs
public bool EqualsIgnoringOffset(StardewUI.Layout.NineGridPlacement other);
```

##### Parameters

**`other`** &nbsp; [NineGridPlacement](ninegridplacement.md)  
The instance to compare.

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  `true` if the `other` instance has the same alignments, otherwise `false`.

-----

#### GetMargin()

Calculates what margin should be applied to the content container in order to achieve the [Offset](ninegridplacement.md#offset).

```cs
public StardewUI.Layout.Edges GetMargin();
```

##### Returns

[Edges](edges.md)

  The margin required to apply the current [Offset](ninegridplacement.md#offset) to a layout container whose content alignment matches the current [HorizontalAlignment](ninegridplacement.md#horizontalalignment) and [VerticalAlignment](ninegridplacement.md#verticalalignment).

##### Remarks

Based on the model of a [Panel](../widgets/panel.md) or [Frame](../widgets/frame.md) whose layout is set to [Fill()](layoutparameters.md#fill) its container and who will adopt the [HorizontalAlignment](ninegridplacement.md#horizontalalignment) and [VerticalAlignment](ninegridplacement.md#verticalalignment) of this placement as its own [HorizontalContentAlignment](../widgets/panel.md#horizontalcontentalignment) and [VerticalContentAlignment](../widgets/panel.md#verticalcontentalignment) (or equivalent for other view types). 

 Depending on the particular alignments, this can apply either positive or negative margin to either the start or end axis (or both).

-----

#### GetNeighbors(bool)

Gets the [NineGridPlacement](ninegridplacement.md)s that neighbor the current placement, i.e. are reachable in a single [Snap(Direction, Boolean)](ninegridplacement.md#snapdirection-bool).

```cs
public System.Collections.Generic.IEnumerable<StardewUI.Layout.NineGridPlacement.Neighbor> GetNeighbors(bool avoidMiddle);
```

##### Parameters

**`avoidMiddle`** &nbsp; [Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)  
Whether to avoid the exact center, i.e. having both [HorizontalAlignment](ninegridplacement.md#horizontalalignment) and [VerticalAlignment](ninegridplacement.md#verticalalignment) be [Middle](alignment.md#middle). This is often used for positioning HUD elements which typically are not useful to show in the middle of the screen, and the positioning UI may want to use that space for button prompts instead.

##### Returns

[IEnumerable](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)<[Neighbor](ninegridplacement.neighbor.md)>

-----

#### GetPosition(Vector2, Vector2)

Computes the position of some content within its container bounds.

```cs
public Microsoft.Xna.Framework.Vector2 GetPosition(Microsoft.Xna.Framework.Vector2 contentSize, Microsoft.Xna.Framework.Vector2 containerSize);
```

##### Parameters

**`contentSize`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
Size of the content to be positioned.

**`containerSize`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
Size of the container in which the content will be positioned.

##### Returns

[Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)

  The aligned content position, relative to the container.

-----

#### IsMiddle()

Checks if this placement is aligned to the exact center of the container, not counting [Offset](ninegridplacement.md#offset).

```cs
public bool IsMiddle();
```

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

-----

#### Nudge(Direction, int)

Keeps the same alignments, but pushes the content farther in a specific direction.

```cs
public StardewUI.Layout.NineGridPlacement Nudge(StardewUI.Direction direction, int distance);
```

##### Parameters

**`direction`** &nbsp; [Direction](../direction.md)  
Direction of the additional offset.

**`distance`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)  
Pixel distance to offset in the specified `direction`.

##### Returns

[NineGridPlacement](ninegridplacement.md)

  A new [NineGridPlacement](ninegridplacement.md) whose alignments are the same as the current instance and whose [Offset](ninegridplacement.md#offset) represents a move from the current offset in the specified `direction` with the specified `distance`.

-----

#### Parse(string)

Parses a [NineGridPlacement](ninegridplacement.md) from its string representation.

```cs
public static StardewUI.Layout.NineGridPlacement Parse(string value);
```

##### Parameters

**`value`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The string value to parse.

##### Returns

[NineGridPlacement](ninegridplacement.md)

  The parsed placement.

-----

#### Snap(Direction, bool)

Snaps to an adjacent grid cell.

```cs
public StardewUI.Layout.NineGridPlacement Snap(StardewUI.Direction direction, bool avoidMiddle);
```

##### Parameters

**`direction`** &nbsp; [Direction](../direction.md)  
Direction in which to move.

**`avoidMiddle`** &nbsp; [Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)  
Whether to avoid the exact center, i.e. having both [HorizontalAlignment](ninegridplacement.md#horizontalalignment) and [VerticalAlignment](ninegridplacement.md#verticalalignment) be [Middle](alignment.md#middle). This is often used for positioning HUD elements which typically are not useful to show in the middle of the screen, and the positioning UI may want to use that space for button prompts instead.

##### Returns

[NineGridPlacement](ninegridplacement.md)

  A new [NineGridPlacement](ninegridplacement.md) representing the adjacent cell in the specified `direction`, or `null` if there is no adjacent cell (e.g. trying to snap [West](../direction.md#west) from a placement that is already at the horizontal [Start](alignment.md#start)).

##### Remarks

Causes the [Offset](ninegridplacement.md#offset) to be reset for the newly-created placement.

-----

#### TryParse(string, NineGridPlacement)

Attempts to parse a [NineGridPlacement](ninegridplacement.md) from its string representation.

```cs
public static bool TryParse(string value, out StardewUI.Layout.NineGridPlacement result);
```

##### Parameters

**`value`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The string value to parse.

**`result`** &nbsp; [NineGridPlacement](ninegridplacement.md)  
If the method returns `true`, holds the parsed placement; otherwise `null`.

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  `true` if the `value` was successfully parsed; `false` if the input was not in a valid format.

-----

