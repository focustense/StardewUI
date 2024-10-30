---
title: ModConfig
description: Configuration settings for StardewUI.Framework.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class ModConfig

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework](index.md)  
Assembly: StardewUI.dll  

</div>

Configuration settings for StardewUI.Framework.

```cs
public class ModConfig
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ ModConfig

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [ModConfig()](#modconfig) |  | 

### Properties

 | Name | Description |
| --- | --- |
| [Tracing](#tracing) | Settings related to performance tracing. | 

## Details

### Constructors

#### ModConfig()



```cs
public ModConfig();
```

-----

### Properties

#### Tracing

Settings related to performance tracing.

```cs
public StardewUI.Framework.Diagnostics.TraceConfig Tracing { get; set; }
```

##### Property Value

[TraceConfig](diagnostics/traceconfig.md)

-----

