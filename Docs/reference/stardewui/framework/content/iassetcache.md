---
title: IAssetCache
description: Cache used for asset-based view bindings.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Interface IAssetCache

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Content](index.md)  
Assembly: StardewUI.dll  

</div>

Cache used for asset-based view bindings.

```cs
public interface IAssetCache
```

## Remarks

Similar to SMAPI's content helpers, but instead of providing just the current asset at the time of the request, returns entry objects with an expired flag for effective (and performant) use in [Update(Boolean)](../sources/ivaluesource.md#updatebool).

## Members

### Methods

 | Name | Description |
| --- | --- |
| [Get&lt;T&gt;(string)](#gettstring) | Retrieves the current entry for a given asset name. | 

## Details

### Methods

#### Get&lt;T&gt;(string)

Retrieves the current entry for a given asset name.

```cs
StardewUI.Framework.Content.IAssetCacheEntry<T> Get<T>(string name);
```

##### Parameters

**`name`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
Name of the asset.

##### Returns

[IAssetCacheEntry&lt;T&gt;](iassetcacheentry-1.md)

  A cache entry object that contains the most current asset data, and an expired flag to detect if the asset is no longer valid in the future.

##### Remarks

If the asset was invalidated by SMAPI and has not yet been reloaded, then this will trigger a reload.

-----

