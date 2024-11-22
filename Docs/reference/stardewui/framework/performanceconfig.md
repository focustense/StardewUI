---
title: PerformanceConfig
description: Configuration sub-settings providing control over performance tweaks.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class PerformanceConfig

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework](index.md)  
Assembly: StardewUI.dll  

</div>

Configuration sub-settings providing control over performance tweaks.

```cs
public class PerformanceConfig
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ PerformanceConfig

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [PerformanceConfig()](#performanceconfig) |  | 

### Properties

 | Name | Description |
| --- | --- |
| [EnableParallelDescriptors](#enableparalleldescriptors) | Whether to process member descriptors of a view or model in parallel. | 
| [EnableReflectionWarmup](#enablereflectionwarmup) | Whether to warm up StardewUI's reflection cache on a background thread during game start. | 

## Details

### Constructors

#### PerformanceConfig()



```cs
public PerformanceConfig();
```

-----

### Properties

#### EnableParallelDescriptors

Whether to process member descriptors of a view or model in parallel.

```cs
public bool EnableParallelDescriptors { get; set; }
```

##### Property Value

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

##### Remarks

Parallel processing will often make first-time loads slower rather than faster, due to scheduling overhead and some synchronization. This may be beneficial if types with hundreds of fields/properties are involved.

-----

#### EnableReflectionWarmup

Whether to warm up StardewUI's reflection cache on a background thread during game start.

```cs
public bool EnableReflectionWarmup { get; set; }
```

##### Property Value

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

##### Remarks

This will usually improve first-time loads of menus by 10-20%, and tends to have no cost/imperceptible cost on app startup, since it uses only one background thread (no parallelism) and other startup tasks tend to run on main thread only.

-----

