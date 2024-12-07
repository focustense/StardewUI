---
title: TraceConfig
description: Configures the tracing behavior for StardewUI.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class TraceConfig

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Diagnostics](index.md)  
Assembly: StardewUI.dll  

</div>

Configures the tracing behavior for StardewUI.

```cs
public class TraceConfig
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ TraceConfig

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [TraceConfig()](#traceconfig) |  | 

### Properties

 | Name | Description |
| --- | --- |
| [EnableHudNotifications](#enablehudnotifications) | Whether to show HUD notifications when tracing is started or stopped. | 
| [OutputDirectory](#outputdirectory) | Directory where traces should be written. | 
| [ToggleHotkeys](#togglehotkeys) | Hotkey(s) used to toggle tracing. | 

## Details

### Constructors

#### TraceConfig()



```cs
public TraceConfig();
```

-----

### Properties

#### EnableHudNotifications

Whether to show HUD notifications when tracing is started or stopped.

```cs
public bool EnableHudNotifications { get; set; }
```

##### Property Value

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

##### Remarks

Notifications are always written to the SMAPI console log, but will not show in-game unless this setting is enabled.

-----

#### OutputDirectory

Directory where traces should be written.

```cs
public string OutputDirectory { get; set; }
```

##### Property Value

[string](https://learn.microsoft.com/en-us/dotnet/api/system.string)

##### Remarks

Unless an absolute path is specified, the directory is relative to Stardew's data directory, i.e. the same directory where `Saves` and `ErrorLogs` are written.

-----

#### ToggleHotkeys

Hotkey(s) used to toggle tracing.

```cs
public StardewModdingAPI.Utilities.KeybindList ToggleHotkeys { get; set; }
```

##### Property Value

KeybindList

-----

