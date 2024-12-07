---
title: DescriptorFactory
description: Factory for obtaining descriptors, encapsulating both dynamic (reflection) and static (precompiled) descriptors.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class DescriptorFactory

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Descriptors](index.md)  
Assembly: StardewUI.dll  

</div>

Factory for obtaining descriptors, encapsulating both dynamic (reflection) and static (precompiled) descriptors.

```cs
public static class DescriptorFactory
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ DescriptorFactory

## Members

### Methods

 | Name | Description |
| --- | --- |
| [GetObjectDescriptor(Type, Boolean)](#getobjectdescriptortype-bool) | Gets a descriptor for an arbitrary object type; typically used for binding targets. | 
| [GetViewDescriptor(Type)](#getviewdescriptortype) | Gets a descriptor for a type that is assumed to be an [IView](../../iview.md) implementation. | 

## Details

### Methods

#### GetObjectDescriptor(Type, bool)

Gets a descriptor for an arbitrary object type; typically used for binding targets.

```cs
public static StardewUI.Framework.Descriptors.IObjectDescriptor GetObjectDescriptor(System.Type type, bool lazy);
```

##### Parameters

**`type`** &nbsp; [Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)  
The object type.

**`lazy`** &nbsp; [Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)  
Whether to create the descriptor with lazily-initialized members. Lazy descriptors are faster to create, but may have slower initial access times.

##### Returns

[IObjectDescriptor](iobjectdescriptor.md)

-----

#### GetViewDescriptor(Type)

Gets a descriptor for a type that is assumed to be an [IView](../../iview.md) implementation.

```cs
public static StardewUI.Framework.Descriptors.IViewDescriptor GetViewDescriptor(System.Type type);
```

##### Parameters

**`type`** &nbsp; [Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)  
The object type.

##### Returns

[IViewDescriptor](iviewdescriptor.md)

##### Remarks

View descriptors include additional information about view-specific types, such as outlets.

-----

