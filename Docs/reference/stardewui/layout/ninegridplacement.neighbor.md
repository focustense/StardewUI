---
title: NineGridPlacement.Neighbor
description: Represents an adjacent placement; the result of NineGridPlacement.GetNeighbors.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class NineGridPlacement.Neighbor

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Layout](index.md)  
Assembly: StardewUI.dll  

</div>

Represents an adjacent placement; the result of [GetNeighbors(Boolean)](ninegridplacement.md#getneighborsbool).

```cs
public record NineGridPlacement.Neighbor : 
    IEquatable<StardewUI.Layout.NineGridPlacement.Neighbor>
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ Neighbor

**Implements**  
[IEquatable](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1)<[Neighbor](ninegridplacement.neighbor.md)>

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [Neighbor(Direction, NineGridPlacement)](#neighbordirection-ninegridplacement) | Represents an adjacent placement; the result of [GetNeighbors(Boolean)](ninegridplacement.md#getneighborsbool). | 

### Properties

 | Name | Description |
| --- | --- |
| [Direction](#direction) | The direction of traversal for this neighbor. | 
| [EqualityContract](#equalitycontract) |  | 
| [Placement](#placement) | The neighboring placement. | 

## Details

### Constructors

#### Neighbor(Direction, NineGridPlacement)

Represents an adjacent placement; the result of [GetNeighbors(Boolean)](ninegridplacement.md#getneighborsbool).

```cs
public Neighbor(StardewUI.Direction Direction, StardewUI.Layout.NineGridPlacement Placement);
```

##### Parameters

**`Direction`** &nbsp; [Direction](../direction.md)  
The direction of traversal for this neighbor.

**`Placement`** &nbsp; [NineGridPlacement](ninegridplacement.md)  
The neighboring placement.

-----

### Properties

#### Direction

The direction of traversal for this neighbor.

```cs
public StardewUI.Direction Direction { get; set; }
```

##### Property Value

[Direction](../direction.md)

-----

#### EqualityContract



```cs
protected System.Type EqualityContract { get; }
```

##### Property Value

[Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)

-----

#### Placement

The neighboring placement.

```cs
public StardewUI.Layout.NineGridPlacement Placement { get; set; }
```

##### Property Value

[NineGridPlacement](ninegridplacement.md)

-----

