---
title: UI
description: Entry point for Stardew UI. Must be called from Mod.Entry.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class UI

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI](index.md)  
Assembly: StardewUI.dll  

</div>

Entry point for Stardew UI. Must be called from Entry(IModHelper).

```cs
public static class UI
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ UI

## Members

### Methods

 | Name | Description |
| --- | --- |
| [Initialize(IModHelper, IMonitor)](#initializeimodhelper-imonitor) | Initialize the framework. | 
| [RegisterAddon(IAddon)](#registeraddoniaddon) | Registers a UI add-on (mod extension). | 

## Details

### Methods

#### Initialize(IModHelper, IMonitor)

Initialize the framework.

```cs
public static void Initialize(StardewModdingAPI.IModHelper helper, StardewModdingAPI.IMonitor monitor);
```

##### Parameters

**`helper`** &nbsp; IModHelper  
Helper for the calling mod.

**`monitor`** &nbsp; IMonitor  
SMAPI logging helper.

-----

#### RegisterAddon(IAddon)

Registers a UI add-on (mod extension).

```cs
public static void RegisterAddon(StardewUI.Framework.Addons.IAddon addon);
```

##### Parameters

**`addon`** &nbsp; [IAddon](framework/addons/iaddon.md)  
The add-on definition.

##### Remarks

Add-ons are resolved in the game's GameLaunched event. Therefore, mods providing add-ons must register them as early as possible, typically in their Entry(IModHelper) method, but if that is too early, then in a `GameLaunched` handler of their own with a high EventPriority. 

 Types provided by add-ons (views, converters, etc.) will not actually be used until a UI is created, so add-ons may employ lazy/deferred loading if they need to postpone some critical operations until after the game is fully loaded, other APIs are initialized, etc.

-----

