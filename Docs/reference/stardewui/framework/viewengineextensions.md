---
title: ViewEngineExtensions
description: Extensions for the IViewEngine interface.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class ViewEngineExtensions

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework](index.md)  
Assembly: StardewUI.dll  

</div>

Extensions for the [IViewEngine](iviewengine.md) interface.

```cs
public static class ViewEngineExtensions
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ ViewEngineExtensions

## Members

### Methods

 | Name | Description |
| --- | --- |
| [EnableHotReloadingWithSourceSync(IViewEngine, string)](#enablehotreloadingwithsourcesynciviewengine-string) | Starts monitoring this mod's directory for changes to assets managed by any of the [IViewEngine](iviewengine.md)'s `Register` methods, e.g. views and sprites, and attempts to set up an additional sync from the mod's project (source) directory to the deployed mod directory so that hot reloads can be initiated from the IDE. | 

## Details

### Methods

#### EnableHotReloadingWithSourceSync(IViewEngine, string)

Starts monitoring this mod's directory for changes to assets managed by any of the [IViewEngine](iviewengine.md)'s `Register` methods, e.g. views and sprites, and attempts to set up an additional sync from the mod's project (source) directory to the deployed mod directory so that hot reloads can be initiated from the IDE.

```cs
public static void EnableHotReloadingWithSourceSync(StardewUI.Framework.IViewEngine viewEngine, string callerFilePath);
```

##### Parameters

**`viewEngine`** &nbsp; [IViewEngine](iviewengine.md)  
The view engine API.

**`callerFilePath`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
Do not pass in this argument, so that [CallerFilePathAttribute](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.callerfilepathattribute) can provide the correct value on build.

##### Remarks

Callers should normally omit the `callerFilePath` parameter in their call; this will cause it to be replaced at compile time with the actual file path of the caller, and used to automatically detect the project path. 

 If detection/sync fails due to an unusual project structure, consider providing an exact path directly to [EnableHotReloading(string)](iviewengine.md#enablehotreloadingstring) instead of using this extension. 

 Hot reload may impact game performance and should normally only be used during development and/or in debug mode.

-----

