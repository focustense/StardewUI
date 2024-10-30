---
title: ReflectionEventDescriptor
description: Helper for creating IEventDescriptor instances using reflection.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class ReflectionEventDescriptor

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Descriptors](index.md)  
Assembly: StardewUI.dll  

</div>

Helper for creating [IEventDescriptor](ieventdescriptor.md) instances using reflection.

```cs
public static class ReflectionEventDescriptor
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ ReflectionEventDescriptor

## Members

### Methods

 | Name | Description |
| --- | --- |
| [FromEventInfo(EventInfo)](#fromeventinfoeventinfo) | Creates or retrieves a descriptor for a given event. | 

## Details

### Methods

#### FromEventInfo(EventInfo)

Creates or retrieves a descriptor for a given event.

```cs
public static StardewUI.Framework.Descriptors.IEventDescriptor FromEventInfo(System.Reflection.EventInfo eventInfo);
```

##### Parameters

**`eventInfo`** &nbsp; [EventInfo](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.eventinfo)  
The event info.

##### Returns

[IEventDescriptor](ieventdescriptor.md)

  The descriptor for the specified `eventInfo`.

-----

