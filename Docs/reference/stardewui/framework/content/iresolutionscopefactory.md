---
title: IResolutionScopeFactory
description: Factory for creating IResolutionScope instances.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Interface IResolutionScopeFactory

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Content](index.md)  
Assembly: StardewUI.dll  

</div>

Factory for creating [IResolutionScope](iresolutionscope.md) instances.

```cs
public interface IResolutionScopeFactory
```

## Members

### Methods

 | Name | Description |
| --- | --- |
| [CreateForDocument(Document)](#createfordocumentdocument) | Obtains a resolution scope for a loaded document. | 

## Details

### Methods

#### CreateForDocument(Document)

Obtains a resolution scope for a loaded document.

```cs
StardewUI.Framework.Content.IResolutionScope CreateForDocument(StardewUI.Framework.Dom.Document document);
```

##### Parameters

**`document`** &nbsp; [Document](../dom/document.md)  
The UI document in which tokens may be resolved.

##### Returns

[IResolutionScope](iresolutionscope.md)

  An [IResolutionScope](iresolutionscope.md) for the specified `document`, based on the best-known information about the document's source.

-----

