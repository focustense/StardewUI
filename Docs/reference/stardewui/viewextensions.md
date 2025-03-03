---
title: ViewExtensions
description: Commonly-used extensions for the IView interface and related types.
search:
    boost: 0.002
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
| [FocusablePath(IEnumerable&lt;ViewChild&gt;)](#focusablepathienumerableviewchild) | Returns the focusable component of the path to a view, typically a cursor target. | 
| [GetDefaultFocusPath(IView)](#getdefaultfocuspathiview) | Retrieves a path to the default focus child/descendant of a view. | 
| [GetPathToPosition(IView, Vector2, Boolean, Boolean)](#getpathtopositioniview-vector2-bool-bool) | Retrieves a path to the view at a given position. | 
| [GetPathToView(IView, IView)](#getpathtoviewiview-iview) | Retrieves the path to a descendant view. | 
| [ResolveChildPath(IView, IEnumerable&lt;IView&gt;)](#resolvechildpathiview-ienumerableiview) | Takes an existing view path and resolves it with child coordinates for the view at each level. | 
| [ToGlobalPositions(IEnumerable&lt;ViewChild&gt;)](#toglobalpositionsienumerableviewchild) | Converts a view path in parent-relative coordinates (e.g. from [GetPathToPosition(IView, Vector2, Boolean, Boolean)](viewextensions.md#getpathtopositioniview-vector2-bool-bool) and transforms each element to have an absolute [Position](viewchild.md#position). | 
| [ZOrder(IEnumerable&lt;ViewChild&gt;, Boolean)](#zorderienumerableviewchild-bool) | Sorts a sequence of children in ascending z-order. | 
| [ZOrderDescending(IEnumerable&lt;ViewChild&gt;, Boolean)](#zorderdescendingienumerableviewchild-bool) | Sorts a sequence of children in descending z-order. | 

## Details

### Methods

#### FocusablePath(IEnumerable&lt;ViewChild&gt;)

Returns the focusable component of the path to a view, typically a cursor target.

```cs
public static System.Collections.Generic.IEnumerable<StardewUI.ViewChild> FocusablePath(System.Collections.Generic.IEnumerable<StardewUI.ViewChild> path);
```

##### Parameters

**`path`** &nbsp; [IEnumerable](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)<[ViewChild](viewchild.md)>  
The view path.

##### Returns

[IEnumerable](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)<[ViewChild](viewchild.md)>

  The sequence of `path` elements ending with the last view for which [IsFocusable](iview.md#isfocusable) is `true`. If there are no focusable views in the path, returns an empty sequence.

-----

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

#### GetPathToPosition(IView, Vector2, bool, bool)

Retrieves a path to the view at a given position.

```cs
public static System.Collections.Generic.IEnumerable<StardewUI.ViewChild> GetPathToPosition(StardewUI.IView view, Microsoft.Xna.Framework.Vector2 position, bool preferFocusable, bool requirePointerEvents);
```

##### Parameters

**`view`** &nbsp; [IView](iview.md)  
The view at which to start the search.

**`position`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
The position to search for, in coordinates relative to the `view`.

**`preferFocusable`** &nbsp; [Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)  
`true` to prioritize a focusable child over a non-focusable child with a higher z-index in case of overlap; `false` to always use the topmost child.

**`requirePointerEvents`** &nbsp; [Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)  
Whether to exclude views whose [PointerEventsEnabled](iview.md#pointereventsenabled) is currently `false`. This short-circuits the pathing; if any ancestor of a view has pointer events disabled then it cannot be part of the path.

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
The path from root down to some descendant, such as the path returned by [GetPathToPosition(IView, Vector2, Boolean, Boolean)](viewextensions.md#getpathtopositioniview-vector2-bool-bool).

##### Returns

[IEnumerable](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)<[ViewChild](viewchild.md)>

  A sequence of [ViewChild](viewchild.md) elements, starting at the `view`, where each child's [Position](viewchild.md#position) is the child's most current location within its parent.

-----

#### ToGlobalPositions(IEnumerable&lt;ViewChild&gt;)

Converts a view path in parent-relative coordinates (e.g. from [GetPathToPosition(IView, Vector2, Boolean, Boolean)](viewextensions.md#getpathtopositioniview-vector2-bool-bool) and transforms each element to have an absolute [Position](viewchild.md#position).

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

#### ZOrder(IEnumerable&lt;ViewChild&gt;, bool)

Sorts a sequence of children in ascending z-order.

```cs
public static System.Collections.Generic.IEnumerable<StardewUI.ViewChild> ZOrder(System.Collections.Generic.IEnumerable<StardewUI.ViewChild> children, bool focusPriority);
```

##### Parameters

**`children`** &nbsp; [IEnumerable](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)<[ViewChild](viewchild.md)>  
The view children.

**`focusPriority`** &nbsp; [Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)  
`true` to sort focusable children first regardless of z-index; `false` to ignore [IsFocusable](iview.md#isfocusable).

##### Returns

[IEnumerable](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)<[ViewChild](viewchild.md)>

  The `children` ordered by the view's [ZIndex](iview.md#zindex) and original sequence order.

##### Remarks

Order is preserved between views with the same [ZIndex](iview.md#zindex), so the resulting sequence will have a primary order of z-index (lower indices first) and a secondary order of original sequence position. This is the correct order for drawing views.

-----

#### ZOrderDescending(IEnumerable&lt;ViewChild&gt;, bool)

Sorts a sequence of children in descending z-order.

```cs
public static System.Collections.Generic.IEnumerable<StardewUI.ViewChild> ZOrderDescending(System.Collections.Generic.IEnumerable<StardewUI.ViewChild> children, bool focusPriority);
```

##### Parameters

**`children`** &nbsp; [IEnumerable](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)<[ViewChild](viewchild.md)>  
The view children.

**`focusPriority`** &nbsp; [Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)  
`true` to sort focusable children first regardless of z-index; `false` to ignore [IsFocusable](iview.md#isfocusable).

##### Returns

[IEnumerable](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)<[ViewChild](viewchild.md)>

##### Remarks

The resulting sequence will have an order such that views with higher [ZIndex](iview.md#zindex) appear first, and views with the same z-index will appear in the _reverse_ order of the original sequence. This is the correct order for handling cursor events and any other actions that need to operate on the "topmost" view first.

-----

