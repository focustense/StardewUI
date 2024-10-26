---
title: ViewExtensions
description: Commonly-used extensions for the IView interface and related types.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class ViewExtensions

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI](index.md)  
Assembly: StardewUI.dll  

</div>

Commonly-used extensions for the [IView](iview.md) interface and related types.

```cs
public static class ViewExtensions
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ ViewExtensions

## Members

### Methods

 | Name | Description |
| --- | --- |
| [GetDefaultFocusPath(IView)](#getdefaultfocuspathiview) | Retrieves a path to the default focus child/descendant of a view. | 
| [GetPathToPosition(IView, Vector2)](#getpathtopositioniview-vector2) | Retrieves a path to the view at a given position. | 
| [GetPathToView(IView, IView)](#getpathtoviewiview-iview) | Retrieves the path to a descendant view. | 
| [ResolveChildPath(IView, IEnumerable&lt;IView&gt;)](#resolvechildpathiview-ienumerableiview) | Takes an existing view path and resolves it with child coordinates for the view at each level. | 
| [ToGlobalPositions(IEnumerable&lt;ViewChild&gt;)](#toglobalpositionsienumerableviewchild) | Converts a view path in parent-relative coordinates (e.g. from [GetPathToPosition(IView, Vector2)](viewextensions.md#getpathtopositioniview-vector2) and transforms each element to have an absolute [Position](viewchild.md#position). | 

## Details

### Methods

#### GetDefaultFocusPath(IView)

Retrieves a path to the default focus child/descendant of a view.

```cs
public static System.Collections.Generic.IEnumerable<StardewUI.ViewChild> GetDefaultFocusPath(StardewUI.IView view);
```

##### Parameters

**`view`** &nbsp; [IView](iview.md)  
The view at which to start the search.

##### Returns

[IEnumerable](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)<[ViewChild](viewchild.md)>

  A sequence of [ViewChild](viewchild.md) elements with the [IView](iview.md) and position (relative to parent) at each level, starting with the specified `view` and ending with the lowest-level [IView](iview.md) in the default focus path. If no focusable descendant is found, returns an empty sequence.

-----

#### GetPathToPosition(IView, Vector2)

Retrieves a path to the view at a given position.

```cs
public static System.Collections.Generic.IEnumerable<StardewUI.ViewChild> GetPathToPosition(StardewUI.IView view, Microsoft.Xna.Framework.Vector2 position);
```

##### Parameters

**`view`** &nbsp; [IView](iview.md)  
The view at which to start the search.

**`position`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
The position to search for, in coordinates relative to the `view`.

##### Returns

[IEnumerable](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)<[ViewChild](viewchild.md)>

  A sequence of [ViewChild](viewchild.md) elements with the [IView](iview.md) and position (relative to parent) at each level, starting with the specified `view` and ending with the lowest-level [IView](iview.md) that still overlaps with the specified `position`. If no match is found, returns an empty sequence.

-----

#### GetPathToView(IView, IView)

Retrieves the path to a descendant view.

```cs
public static System.Collections.Generic.IEnumerable<StardewUI.ViewChild> GetPathToView(StardewUI.IView view, StardewUI.IView descendant);
```

##### Parameters

**`view`** &nbsp; [IView](iview.md)  
The view at which to start the search.

**`descendant`** &nbsp; [IView](iview.md)  
The descendant view to search for.

##### Returns

[IEnumerable](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)<[ViewChild](viewchild.md)>

  A sequence of [ViewChild](viewchild.md) elements with the [IView](iview.md) and position (relative to parent) at each level, starting with the specified `view` and ending with the specified `descendant`. If no match is found, returns `null`.

##### Remarks

This method has worst-case O(N) performance, so avoid calling it in tight loops such as draw methods, and cache the result whenever possible.

-----

#### ResolveChildPath(IView, IEnumerable&lt;IView&gt;)

Takes an existing view path and resolves it with child coordinates for the view at each level.

```cs
public static System.Collections.Generic.IEnumerable<StardewUI.ViewChild> ResolveChildPath(StardewUI.IView view, System.Collections.Generic.IEnumerable<StardewUI.IView> path);
```

##### Parameters

**`view`** &nbsp; [IView](iview.md)  
The root view.

**`path`** &nbsp; [IEnumerable](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)<[IView](iview.md)>  
The path from root down to some descendant, such as the path returned by [GetPathToPosition(IView, Vector2)](viewextensions.md#getpathtopositioniview-vector2).

##### Returns

[IEnumerable](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)<[ViewChild](viewchild.md)>

  A sequence of [ViewChild](viewchild.md) elements, starting at the `view`, where each child's [Position](viewchild.md#position) is the child's most current location within its parent.

-----

#### ToGlobalPositions(IEnumerable&lt;ViewChild&gt;)

Converts a view path in parent-relative coordinates (e.g. from [GetPathToPosition(IView, Vector2)](viewextensions.md#getpathtopositioniview-vector2) and transforms each element to have an absolute [Position](viewchild.md#position).

```cs
public static System.Collections.Generic.IEnumerable<StardewUI.ViewChild> ToGlobalPositions(System.Collections.Generic.IEnumerable<StardewUI.ViewChild> path);
```

##### Parameters

**`path`** &nbsp; [IEnumerable](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)<[ViewChild](viewchild.md)>  
The path from root down to leaf view.

##### Returns

[IEnumerable](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)<[ViewChild](viewchild.md)>

  The `path` with positions in global coordinates.

##### Remarks

Since [ViewChild](viewchild.md) does not specify whether the position is local (parent) or global (absolute), it is not possible to validate the incoming sequence and prevent a "double transformation". Callers are responsible for knowing whether or not the input sequence is local or global.

-----

