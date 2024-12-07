---
title: StardewUI.Layout
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# StardewUI.Layout Namespace

## Classes

| Name | Description |
| --- | --- |
| [AlignmentExtensions](alignmentextensions.md) | Common helpers for [Alignment](alignment.md). |
| [Bounds](bounds.md) | A bounding rectangle using floating-point dimensions. |
| [DirtyTracker&lt;T&gt;](dirtytracker-1.md) | Convenience class for tracking properties that have changed, i.e. for layout dirty checking. |
| [DirtyTrackingList&lt;T&gt;](dirtytrackinglist-1.md) | List wrapper that tracks whether changes have been made. |
| [Edges](edges.md) | Describes a set of edge dimensions, e.g. for margin or padding. |
| [FloatingElement](floatingelement.md) | Provides independent layout for an [IView](../iview.md) relative to its parent. |
| [FloatingPosition](floatingposition.md) | Describes the position of a [FloatingElement](floatingelement.md). |
| [NineGridPlacement](ninegridplacement.md) | Model for content placement along a nine-segment grid, i.e. all possible combinations of horizontal and vertical [Alignment](alignment.md). |
| [NineGridPlacement](ninegridplacement.md).[Neighbor](ninegridplacement.neighbor.md) | Represents an adjacent placement; the result of [GetNeighbors(Boolean)](ninegridplacement.md#getneighborsbool). |
| [OffsettableExtensions](offsettableextensions.md) | Extensions for the [IOffsettable&lt;T&gt;](ioffsettable-1.md) interface. |
| [OrientationExtensions](orientationextensions.md) | Helpers for working with [Orientation](orientation.md). |

## Structs

| Name | Description |
| --- | --- |
| [LayoutParameters](layoutparameters.md) | Layout parameters for an [IView](../iview.md). |
| [Length](length.md) | Specifies how to calculate the length of a single dimension (width or height). |

## Interfaces

| Name | Description |
| --- | --- |
| [IOffsettable&lt;T&gt;](ioffsettable-1.md) | Provides a method to clone the current instance with an offset applied. |
| [IPageable](ipageable.md) | Signals that an [IView](../iview.md) implements paging controls. |
| [ITabbable](itabbable.md) | Signals that an [IView](../iview.md) implements tab controls. |

## Enums

| Name | Description |
| --- | --- |
| [Alignment](alignment.md) | Specifies an alignment (horizontal or vertical) for text or other layout. |
| [LengthType](lengthtype.md) | Types of length calculation available for a [Length](length.md). |
| [Orientation](orientation.md) | Available orientation directions for views such as [Lane](../widgets/lane.md). |
| [Visibility](visibility.md) | Controls the visibility of an [IView](../iview.md). |

