---
title: IMethodDescriptor&lt;T&gt;
description: Describes a single method on some type, and provides a wrapper method to invoke it.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Interface IMethodDescriptor&lt;T&gt;

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Descriptors](index.md)  
Assembly: StardewUI.dll  

</div>

Describes a single method on some type, and provides a wrapper method to invoke it.

```cs
public interface IMethodDescriptor<T> : 
    StardewUI.Framework.Descriptors.IMethodDescriptor, 
    StardewUI.Framework.Descriptors.IMemberDescriptor
```

### Type Parameters

**`T`**  
The return type of the described method.


**Implements**  
[IMethodDescriptor](imethoddescriptor.md), [IMemberDescriptor](imemberdescriptor.md)

## Members

### Methods

 | Name | Description |
| --- | --- |
| [Invoke(Object, Object)](#invokeobject-object) | Invokes the underlying method. | 

## Details

### Methods

#### Invoke(Object, Object)

Invokes the underlying method.

```cs
T Invoke(System.Object target, System.Object arguments);
```

##### Parameters

**`target`** &nbsp; [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object)  
The object instance on which to invoke the method.

**`arguments`** &nbsp; [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object)  
The arguments to provide to the method.

##### Returns

`T`

  The return value.

-----

