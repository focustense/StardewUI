---
title: IViewFactory
description: Factory for creating views from tags.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Interface IViewFactory

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Binding](index.md)  
Assembly: StardewUI.dll  

</div>

Factory for creating views from tags.

```cs
public interface IViewFactory
```

## Remarks

This is a simple, low-level abstraction that simply maps tags to view types. It does not perform any reflection or participate in view binding.

## Members

### Methods

 | Name | Description |
| --- | --- |
| [CreateView(string)](#createviewstring) | Creates a new view. | 
| [SupportsTag(string)](#supportstagstring) | Checks if the factory can create views corresponding to a specific tag. | 

## Details

### Methods

#### CreateView(string)

Creates a new view.

```cs
StardewUI.IView CreateView(string tagName);
```

##### Parameters

**`tagName`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The markup tag that specifies the type of view.

##### Returns

[IView](../../iview.md)

  A new view of a type corresponding to the `tagName`.

-----

#### SupportsTag(string)

Checks if the factory can create views corresponding to a specific tag.

```cs
bool SupportsTag(string tagName);
```

##### Parameters

**`tagName`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The markup tag that specifies the type of view.

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  `true` if this factory should handle the specified `tagName`, otherwise `false`.

-----

