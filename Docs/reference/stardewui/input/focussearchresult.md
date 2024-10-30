---
title: FocusSearchResult
description: The result of a IView.FocusSearch. Identifies the specific view/position found, as well as the path to that view from the search root.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class FocusSearchResult

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Input](index.md)  
Assembly: StardewUI.dll  

</div>

The result of a [FocusSearch(Vector2, Direction)](../iview.md#focussearchvector2-direction). Identifies the specific view/position found, as well as the path to that view from the search root.

```cs
public record FocusSearchResult : IEquatable<StardewUI.Input.FocusSearchResult>
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ FocusSearchResult

**Implements**  
[IEquatable](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1)<[FocusSearchResult](focussearchresult.md)>

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [FocusSearchResult(ViewChild, IEnumerable&lt;ViewChild&gt;)](#focussearchresultviewchild-ienumerableviewchild) | The result of a [FocusSearch(Vector2, Direction)](../iview.md#focussearchvector2-direction). Identifies the specific view/position found, as well as the path to that view from the search root. | 

### Properties

 | Name | Description |
| --- | --- |
| [EqualityContract](#equalitycontract) |  | 
| [Path](#path) | The path from root to [Target](focussearchresult.md#target), in top-down order; each element's [Position](../viewchild.md#position) is relative to the parent, **not** the search root as `Target` is. | 
| [Target](#target) | The specific view that can/will be focused, with a [Position](../viewchild.md#position) relative to the search root. | 

### Methods

 | Name | Description |
| --- | --- |
| [AsChild(IView, Vector2)](#aschildiview-vector2) | Returns a transformed [FocusSearchResult](focussearchresult.md) that adds a view (generally the caller) to the beginning of the [Path](focussearchresult.md#path), and applies its content offset to either the first element of the current [Path](focussearchresult.md#path) (if non-empty) or the [Target](focussearchresult.md#target) (if the path is empty). | 
| [Offset(Vector2)](#offsetvector2) | Applies a local offset to a search result. | 

## Details

### Constructors

#### FocusSearchResult(ViewChild, IEnumerable&lt;ViewChild&gt;)

The result of a [FocusSearch(Vector2, Direction)](../iview.md#focussearchvector2-direction). Identifies the specific view/position found, as well as the path to that view from the search root.

```cs
public FocusSearchResult(StardewUI.ViewChild Target, System.Collections.Generic.IEnumerable<StardewUI.ViewChild> Path);
```

##### Parameters

**`Target`** &nbsp; [ViewChild](../viewchild.md)  
The specific view that can/will be focused, with a [Position](../viewchild.md#position) relative to the search root.

**`Path`** &nbsp; [IEnumerable](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)<[ViewChild](../viewchild.md)>  
The path from root to [Target](focussearchresult.md#target), in top-down order; each element's [Position](../viewchild.md#position) is relative to the parent, **not** the search root as `Target` is.

-----

### Properties

#### EqualityContract



```cs
protected System.Type EqualityContract { get; }
```

##### Property Value

[Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)

-----

#### Path

The path from root to [Target](focussearchresult.md#target), in top-down order; each element's [Position](../viewchild.md#position) is relative to the parent, **not** the search root as `Target` is.

```cs
public System.Collections.Generic.IEnumerable<StardewUI.ViewChild> Path { get; set; }
```

##### Property Value

[IEnumerable](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)<[ViewChild](../viewchild.md)>

-----

#### Target

The specific view that can/will be focused, with a [Position](../viewchild.md#position) relative to the search root.

```cs
public StardewUI.ViewChild Target { get; set; }
```

##### Property Value

[ViewChild](../viewchild.md)

-----

### Methods

#### AsChild(IView, Vector2)

Returns a transformed [FocusSearchResult](focussearchresult.md) that adds a view (generally the caller) to the beginning of the [Path](focussearchresult.md#path), and applies its content offset to either the first element of the current [Path](focussearchresult.md#path) (if non-empty) or the [Target](focussearchresult.md#target) (if the path is empty).

```cs
public StardewUI.Input.FocusSearchResult AsChild(StardewUI.IView parent, Microsoft.Xna.Framework.Vector2 position);
```

##### Parameters

**`parent`** &nbsp; [IView](../iview.md)  
The parent that contains the current result.

**`position`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
The content offset relative to the `parent`.

##### Returns

[FocusSearchResult](focussearchresult.md)

##### Remarks

Used to propagate results correctly up the view hierarchy in a focus search. This is called by [FocusSearch(Vector2, Direction)](../view.md#focussearchvector2-direction) and should not be called in overrides of [FindFocusableDescendant(Vector2, Direction)](../view.md#findfocusabledescendantvector2-direction).

-----

#### Offset(Vector2)

Applies a local offset to a search result.

```cs
public StardewUI.Input.FocusSearchResult Offset(Microsoft.Xna.Framework.Vector2 distance);
```

##### Parameters

**`distance`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
The distance to offset the [Target](focussearchresult.md#target) and first element of [Path](focussearchresult.md#path).

##### Returns

[FocusSearchResult](focussearchresult.md)

  A new [FocusSearchResult](focussearchresult.md) with the `distance` offset applied.

##### Remarks

Used to propagate the child position into a search result produced by that child. For example, view A is a layout with positioned child view C, which yields a search result targeting view Z in terms of its (C's) local coordinates. Applying the offset will adjust either the first element of the [Path](focussearchresult.md#path), if non-empty, or the [Target](focussearchresult.md#target) itself if [Path](focussearchresult.md#path) is empty. No other elements of the [Path](focussearchresult.md#path) will be modified, as each element is already positioned relative to its parent preceding it in the list.

-----

