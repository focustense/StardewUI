---
title: Document
description: A standalone StarML document.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class Document

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Dom](index.md)  
Assembly: StardewUI.dll  

</div>

A standalone StarML document.

```cs
public record Document : IEquatable<StardewUI.Framework.Dom.Document>
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ Document

**Implements**  
[IEquatable](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1)<[Document](document.md)>

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [Document(SNode, IReadOnlyList&lt;SNode&gt;)](#documentsnode-ireadonlylistsnode) | A standalone StarML document. | 

### Properties

 | Name | Description |
| --- | --- |
| [EqualityContract](#equalitycontract) |  | 
| [Root](#root) | The primary content node. | 
| [Templates](#templates) | List of template nodes for inline expansion. | 

### Methods

 | Name | Description |
| --- | --- |
| [Parse(ReadOnlySpan&lt;Char&gt;)](#parsereadonlyspanchar) | Parses a [Document](document.md) from its original markup text. | 

## Details

### Constructors

#### Document(SNode, IReadOnlyList&lt;SNode&gt;)

A standalone StarML document.

```cs
public Document(StardewUI.Framework.Dom.SNode Root, System.Collections.Generic.IReadOnlyList<StardewUI.Framework.Dom.SNode> Templates);
```

##### Parameters

**`Root`** &nbsp; [SNode](snode.md)  
The primary content node.

**`Templates`** &nbsp; [IReadOnlyList](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlylist-1)<[SNode](snode.md)>  
List of template nodes for inline expansion.

-----

### Properties

#### EqualityContract



```cs
protected System.Type EqualityContract { get; }
```

##### Property Value

[Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)

-----

#### Root

The primary content node.

```cs
public StardewUI.Framework.Dom.SNode Root { get; set; }
```

##### Property Value

[SNode](snode.md)

-----

#### Templates

List of template nodes for inline expansion.

```cs
public System.Collections.Generic.IReadOnlyList<StardewUI.Framework.Dom.SNode> Templates { get; set; }
```

##### Property Value

[IReadOnlyList](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlylist-1)<[SNode](snode.md)>

-----

### Methods

#### Parse(ReadOnlySpan&lt;Char&gt;)

Parses a [Document](document.md) from its original markup text.

```cs
public static StardewUI.Framework.Dom.Document Parse(ReadOnlySpan<System.Char> text);
```

##### Parameters

**`text`** &nbsp; [ReadOnlySpan](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<[Char](https://learn.microsoft.com/en-us/dotnet/api/system.char)>  
The StarML markup text.

##### Returns

[Document](document.md)

  The parsed document as a DOM tree.

-----

