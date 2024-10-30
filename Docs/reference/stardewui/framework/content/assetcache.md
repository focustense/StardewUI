---
title: AssetCache
description: Standard in-game implementation of the asset cache based on SMAPI's helpers and events.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class AssetCache

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Content](index.md)  
Assembly: StardewUI.dll  

</div>

Standard in-game implementation of the asset cache based on SMAPI's helpers and events.

```cs
public class AssetCache : StardewUI.Framework.Content.IAssetCache
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ AssetCache

**Implements**  
[IAssetCache](iassetcache.md)

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [AssetCache(IGameContentHelper, IContentEvents)](#assetcacheigamecontenthelper-icontentevents) | Initializes a new instance of [AssetCache](assetcache.md). | 

### Methods

 | Name | Description |
| --- | --- |
| [Get&lt;T&gt;(string)](#gettstring) | Retrieves the current entry for a given asset name. | 

## Details

### Constructors

#### AssetCache(IGameContentHelper, IContentEvents)

Initializes a new instance of [AssetCache](assetcache.md).

```cs
public AssetCache(StardewModdingAPI.IGameContentHelper content, StardewModdingAPI.Events.IContentEvents events);
```

##### Parameters

**`content`** &nbsp; IGameContentHelper  
SMAPI content helper, used to load the assets.

**`events`** &nbsp; IContentEvents  
SMAPI content events, used to detect invalidation.

-----

### Methods

#### Get&lt;T&gt;(string)

Retrieves the current entry for a given asset name.

```cs
public StardewUI.Framework.Content.IAssetCacheEntry<T> Get<T>(string name);
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

