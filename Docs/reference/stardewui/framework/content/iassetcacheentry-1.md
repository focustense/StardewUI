---
title: IAssetCacheEntry&lt;T&gt;
description: Entry retrieved from an IAssetCache.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Interface IAssetCacheEntry&lt;T&gt;

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Content](index.md)  
Assembly: StardewUI.dll  

</div>

Entry retrieved from an [IAssetCache](iassetcache.md).

```cs
public interface IAssetCacheEntry<T>
```

### Type Parameters

**`T`**  
Type of cached asset.


## Members

### Properties

 | Name | Description |
| --- | --- |
| [Asset](#asset) | The cached asset. | 
| [IsValid](#isvalid) | Whether or not the [Asset](iassetcacheentry-1.md#asset) is valid and can be accessed. | 

## Details

### Properties

#### Asset

The cached asset.

```cs
T Asset { get; }
```

##### Property Value

`T`

-----

#### IsValid

Whether or not the [Asset](iassetcacheentry-1.md#asset) is valid and can be accessed.

```cs
bool IsValid { get; }
```

##### Property Value

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

##### Remarks

Invalid assets either failed to load or have been invalidated at the source and may be disposed (if [IDisposable](https://learn.microsoft.com/en-us/dotnet/api/system.idisposable)) or otherwise unusable. Consumers of the cache entry **must not** attempt to read or use the [Asset](iassetcacheentry-1.md#asset) property of an invalid asset.

-----

