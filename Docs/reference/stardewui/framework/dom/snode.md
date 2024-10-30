---
title: SNode
description: A node in a StarML document, encapsulating the tag, its attributes, and all child nodes.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class SNode

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Dom](index.md)  
Assembly: StardewUI.dll  

</div>

A node in a StarML document, encapsulating the tag, its attributes, and all child nodes.

```cs
public record SNode : IEquatable<StardewUI.Framework.Dom.SNode>
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ SNode

**Implements**  
[IEquatable](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1)<[SNode](snode.md)>

## Remarks

This is also the root of a [Document](document.md) and the visible result of a parser. While there is some memory and performance cost associated with this intermediate representation before assembling the [ViewNode](../binding/viewnode.md), it allows for document assets to be edited (patched) prior to binding.

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [SNode(SElement, IReadOnlyList&lt;SNode&gt;)](#snodeselement-ireadonlylistsnode) | A node in a StarML document, encapsulating the tag, its attributes, and all child nodes. | 

### Properties

 | Name | Description |
| --- | --- |
| [Attributes](#attributes) | Gets the attributes of the associated [Element](snode.md#element). | 
| [ChildNodes](#childnodes) | The children of this node. | 
| [Element](#element) | The element data for this node. | 
| [EqualityContract](#equalitycontract) |  | 
| [Tag](#tag) | Gets the tag of the associated [Element](snode.md#element). | 

## Details

### Constructors

#### SNode(SElement, IReadOnlyList&lt;SNode&gt;)

A node in a StarML document, encapsulating the tag, its attributes, and all child nodes.

```cs
public SNode(StardewUI.Framework.Dom.SElement Element, System.Collections.Generic.IReadOnlyList<StardewUI.Framework.Dom.SNode> ChildNodes);
```

##### Parameters

**`Element`** &nbsp; [SElement](selement.md)  
The element data for this node.

**`ChildNodes`** &nbsp; [IReadOnlyList](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlylist-1)<[SNode](snode.md)>  
The children of this node.

##### Remarks

This is also the root of a [Document](document.md) and the visible result of a parser. While there is some memory and performance cost associated with this intermediate representation before assembling the [ViewNode](../binding/viewnode.md), it allows for document assets to be edited (patched) prior to binding.

-----

### Properties

#### Attributes

Gets the attributes of the associated [Element](snode.md#element).

```cs
public System.Collections.Generic.IReadOnlyList<StardewUI.Framework.Dom.SAttribute> Attributes { get; }
```

##### Property Value

[IReadOnlyList](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlylist-1)<[SAttribute](sattribute.md)>

-----

#### ChildNodes

The children of this node.

```cs
public System.Collections.Generic.IReadOnlyList<StardewUI.Framework.Dom.SNode> ChildNodes { get; set; }
```

##### Property Value

[IReadOnlyList](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlylist-1)<[SNode](snode.md)>

-----

#### Element

The element data for this node.

```cs
public StardewUI.Framework.Dom.SElement Element { get; set; }
```

##### Property Value

[SElement](selement.md)

-----

#### EqualityContract



```cs
protected System.Type EqualityContract { get; }
```

##### Property Value

[Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)

-----

#### Tag

Gets the tag of the associated [Element](snode.md#element).

```cs
public string Tag { get; }
```

##### Property Value

[string](https://learn.microsoft.com/en-us/dotnet/api/system.string)

-----

