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
| [IsSupported(EventInfo)](#issupportedeventinfo) | Checks if an event is supported for view binding. | 

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

#### IsSupported(EventInfo)

Checks if an event is supported for view binding.

```cs
public static bool IsSupported(System.Reflection.EventInfo eventInfo);
```

##### Parameters

**`eventInfo`** &nbsp; [EventInfo](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.eventinfo)  
The event info.

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  `true` if a [ReflectionEventDescriptor&lt;TTarget, THandler&gt;](reflectioneventdescriptor-2.md) can be created for the specified `eventInfo`, otherwise `false`.

-----

