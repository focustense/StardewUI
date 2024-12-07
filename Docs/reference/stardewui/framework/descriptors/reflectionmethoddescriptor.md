---
title: ReflectionMethodDescriptor
description: Helper for creating IMethodDescriptor instances using reflection.
search:
    boost: 0.002
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
| [IsSupported(MethodInfo)](#issupportedmethodinfo) | Checks if a method is supported for view binding. | 

## Details

### Methods

#### IsSupported(MethodInfo)

Checks if a method is supported for view binding.

```cs
public static bool IsSupported(System.Reflection.MethodInfo method);
```

##### Parameters

**`method`** &nbsp; [MethodInfo](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.methodinfo)  
The method info.

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  `true` if a `StardewUI.Framework.Descriptors.ReflectionMethodDescriptor<TResult>` can be created for the specified `method`, otherwise `false`.

-----

