---
title: ReflectionMethodDescriptor
description: Helper for creating IMethodDescriptor instances using reflection.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class ReflectionMethodDescriptor

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Descriptors](index.md)  
Assembly: StardewUI.dll  

</div>

Helper for creating [IMethodDescriptor](imethoddescriptor.md) instances using reflection.

```cs
public static class ReflectionMethodDescriptor
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ ReflectionMethodDescriptor

## Members

### Methods

 | Name | Description |
| --- | --- |
| [FromMethodInfo(MethodInfo)](#frommethodinfomethodinfo) | Creates or retrieves a descriptor for a given method. | 

## Details

### Methods

#### FromMethodInfo(MethodInfo)

Creates or retrieves a descriptor for a given method.

```cs
public static StardewUI.Framework.Descriptors.IMethodDescriptor FromMethodInfo(System.Reflection.MethodInfo method);
```

##### Parameters

**`method`** &nbsp; [MethodInfo](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.methodinfo)  
The method info.

##### Returns

[IMethodDescriptor](imethoddescriptor.md)

  The descriptor for the specified `method`.

-----

