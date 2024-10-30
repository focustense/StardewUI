---
title: AlignmentExtensions
description: Common helpers for Alignment.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class AlignmentExtensions

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Layout](index.md)  
Assembly: StardewUI.dll  

</div>

Common helpers for [Alignment](alignment.md).

```cs
public static class AlignmentExtensions
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ AlignmentExtensions

## Members

### Methods

 | Name | Description |
| --- | --- |
| [Align(Alignment, Single, Single)](#alignalignment-float-float) | Applies an alignment to an axis starting at position 0. | 

## Details

### Methods

#### Align(Alignment, float, float)

Applies an alignment to an axis starting at position 0.

```cs
public static float Align(StardewUI.Layout.Alignment alignment, float contentLength, float axisLength);
```

##### Parameters

**`alignment`** &nbsp; [Alignment](alignment.md)  
The alignment type.

**`contentLength`** &nbsp; [Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)  
The length (width or height) of the content to align.

**`axisLength`** &nbsp; [Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)  
The total space (width or height) available for the content.

##### Returns

[Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)

-----

