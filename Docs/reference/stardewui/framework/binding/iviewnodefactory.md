---
title: IViewNodeFactory
description: High-level abstraction for translating node trees into bound view trees.
search:
    boost: 0.002
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
| [CreateNode(Document)](#createnodedocument) | Creates a bound view node, and all descendants, from the root of a parsed [Document](../dom/document.md). | 
| [CreateNode(SNode, IReadOnlyList&lt;INodeTransformer&gt;, IResolutionScope)](#createnodesnode-ireadonlylistinodetransformer-iresolutionscope) | Creates a bound view node, and all descendants, from parsed node data. | 

## Details

### Methods

#### CreateNode(Document)

Creates a bound view node, and all descendants, from the root of a parsed [Document](../dom/document.md).

```cs
StardewUI.Framework.Binding.IViewNode CreateNode(StardewUI.Framework.Dom.Document document);
```

##### Parameters

**`document`** &nbsp; [Document](../dom/document.md)  
The markup document.

##### Returns

[IViewNode](iviewnode.md)

  An [IViewNode](iviewnode.md) providing the [IView](../../iview.md) bound with the node's attributes and children, which automatically applies changes on each [Update(TimeSpan)](iviewnode.md#updatetimespan).

##### Remarks

This method automatically infers the correct [IResolutionScope](../content/iresolutionscope.md), so it does not require an explicit scope to be given.

-----

#### CreateNode(SNode, IReadOnlyList&lt;INodeTransformer&gt;, IResolutionScope)

Creates a bound view node, and all descendants, from parsed node data.

```cs
StardewUI.Framework.Binding.IViewNode CreateNode(StardewUI.Framework.Dom.SNode node, System.Collections.Generic.IReadOnlyList<StardewUI.Framework.Dom.INodeTransformer> nodeTransformers, StardewUI.Framework.Content.IResolutionScope resolutionScope);
```

##### Parameters

**`node`** &nbsp; [SNode](../dom/snode.md)  
The node data.

**`nodeTransformers`** &nbsp; [IReadOnlyList](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlylist-1)<[INodeTransformer](../dom/inodetransformer.md)>  
Transformers to run on each document node before using it to create a runtime (bound) view node.

**`resolutionScope`** &nbsp; [IResolutionScope](../content/iresolutionscope.md)  
Scope for resolving externalized attributes, such as translation keys.

##### Returns

[IViewNode](iviewnode.md)

  An [IViewNode](iviewnode.md) providing the [IView](../../iview.md) bound with the node's attributes and children, which automatically applies changes on each [Update(TimeSpan)](iviewnode.md#updatetimespan).

-----

