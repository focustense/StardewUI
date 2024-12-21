---
title: ISourceResolver
description: Provides a method to connect a parsed Document back to the mod that provided it.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Interface ISourceResolver

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Content](index.md)  
Assembly: StardewUI.dll  

</div>

Provides a method to connect a parsed [Document](../dom/document.md) back to the mod that provided it.

```cs
public interface ISourceResolver
```

## Members

### Methods

 | Name | Description |
| --- | --- |
| [TryGetProvidingModId(Document, String)](#trygetprovidingmodiddocument-string) | Attempts to determine which mod is the originator of some markup document. | 

## Details

### Methods

#### TryGetProvidingModId(Document, String)

Attempts to determine which mod is the originator of some markup document.

```cs
bool TryGetProvidingModId(StardewUI.Framework.Dom.Document document, out System.String modId);
```

##### Parameters

**`document`** &nbsp; [Document](../dom/document.md)  
The markup document.

**`modId`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
Holds the `StardewModdingAPI.IManifest.UniqueID` of the originating mod, if the method returns `true`, otherwise `null`.

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  `true` if the `document` source was successfully resolved, otherwise `false`.

-----

