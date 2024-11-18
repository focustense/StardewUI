---
title: INodeTransformer
description: Provides a method to transform nodes into other nodes.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Interface INodeTransformer

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Dom](index.md)  
Assembly: StardewUI.dll  

</div>

Provides a method to transform nodes into other nodes.

```cs
public interface INodeTransformer
```

## Remarks

Transformers are a form of preprocessing that apply before a view is bound; they operate on the parsed DOM content but not the runtime/bound nodes.

## Members

### Methods

 | Name | Description |
| --- | --- |
| [Transform(SNode)](#transformsnode) | Transforms a node. | 

## Details

### Methods

#### Transform(SNode)

Transforms a node.

```cs
System.Collections.Generic.IReadOnlyList<StardewUI.Framework.Dom.SNode> Transform(StardewUI.Framework.Dom.SNode source);
```

##### Parameters

**`source`** &nbsp; [SNode](snode.md)  
The node to transform.

##### Returns

[IReadOnlyList](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlylist-1)<[SNode](snode.md)>

  The transformed nodes, if any transform was applied, or a single-element list with the original `source` if the transformation is not applicable to this node.

-----

