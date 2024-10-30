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

