---
title: IViewNodeFactory
description: High-level abstraction for translating node trees into bound view trees.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Interface IViewNodeFactory

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Binding](index.md)  
Assembly: StardewUI.dll  

</div>

High-level abstraction for translating node trees into bound view trees.

```cs
public interface IViewNodeFactory
```

## Members

### Methods

 | Name | Description |
| --- | --- |
| [CreateNode(SNode)](#createnodesnode) | Creates a bound view node, and all descendants, from parsed node data. | 

## Details

### Methods

#### CreateNode(SNode)

Creates a bound view node, and all descendants, from parsed node data.

```cs
StardewUI.Framework.Binding.IViewNode CreateNode(StardewUI.Framework.Dom.SNode node);
```

##### Parameters

**`node`** &nbsp; [SNode](../dom/snode.md)  
The node data.

##### Returns

[IViewNode](iviewnode.md)

  An [IViewNode](iviewnode.md) providing the [IView](../../iview.md) bound with the node's attributes and children, which automatically applies changes on each [Update(TimeSpan)](iviewnode.md#updatetimespan).

-----

