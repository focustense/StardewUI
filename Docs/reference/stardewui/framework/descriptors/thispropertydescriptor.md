---
title: ThisPropertyDescriptor
description: Helper for obtaining a ThisPropertyDescriptor`1 using a type known only at runtime.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class ThisPropertyDescriptor

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Descriptors](index.md)  
Assembly: StardewUI.dll  

</div>

Helper for obtaining a [ThisPropertyDescriptor&lt;T&gt;](thispropertydescriptor-1.md) using a type known only at runtime.

```cs
public static class ThisPropertyDescriptor
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ ThisPropertyDescriptor

## Members

### Methods

 | Name | Description |
| --- | --- |
| [ForTypeUncached(Type)](#fortypeuncachedtype) | Gets the [ThisPropertyDescriptor&lt;T&gt;](thispropertydescriptor-1.md) corresponding to a specified type. | 

## Details

### Methods

#### ForTypeUncached(Type)

Gets the [ThisPropertyDescriptor&lt;T&gt;](thispropertydescriptor-1.md) corresponding to a specified type.

```cs
public static StardewUI.Framework.Descriptors.IPropertyDescriptor ForTypeUncached(System.Type type);
```

##### Parameters

**`type`** &nbsp; [Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)  
The object type.

##### Returns

[IPropertyDescriptor](ipropertydescriptor.md)

  The [ThisPropertyDescriptor&lt;T&gt;](thispropertydescriptor-1.md) instance for the given `type`.

-----

