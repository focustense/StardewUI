---
title: TemplateNodeTransformer
description: Transforms a template node based on the structure (attributes, children, etc.) of the instantiating node.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class TemplateNodeTransformer

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Dom](index.md)  
Assembly: StardewUI.dll  

</div>

Transforms a `template` node based on the structure (attributes, children, etc.) of the instantiating node.

```cs
public class TemplateNodeTransformer : StardewUI.Framework.Dom.INodeTransformer
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ TemplateNodeTransformer

**Implements**  
[INodeTransformer](inodetransformer.md)

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [TemplateNodeTransformer(SNode)](#templatenodetransformersnode) | Transforms a `template` node based on the structure (attributes, children, etc.) of the instantiating node. | 

### Methods

 | Name | Description |
| --- | --- |
| [Transform(SNode)](#transformsnode) | Transforms a node. | 

## Details

### Constructors

#### TemplateNodeTransformer(SNode)

Transforms a `template` node based on the structure (attributes, children, etc.) of the instantiating node.

```cs
public TemplateNodeTransformer(StardewUI.Framework.Dom.SNode template);
```

##### Parameters

**`template`** &nbsp; [SNode](snode.md)  
The template node.

-----

### Methods

#### Transform(SNode)

Transforms a node.

```cs
public System.Collections.Generic.IReadOnlyList<StardewUI.Framework.Dom.SNode> Transform(StardewUI.Framework.Dom.SNode source);
```

##### Parameters

**`source`** &nbsp; [SNode](snode.md)  
The node to transform.

##### Returns

[IReadOnlyList](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlylist-1)<[SNode](snode.md)>

  The transformed nodes, if any transform was applied, or a single-element list with the original `source` if the transformation is not applicable to this node.

-----

