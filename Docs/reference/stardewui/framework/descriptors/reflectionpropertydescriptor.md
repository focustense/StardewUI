---
title: ReflectionPropertyDescriptor
description: Helper for creating ReflectionPropertyDescriptor`2 with types not known at compile time.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class ReflectionPropertyDescriptor

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Descriptors](index.md)  
Assembly: StardewUI.dll  

</div>

Helper for creating [ReflectionPropertyDescriptor&lt;T, TValue&gt;](reflectionpropertydescriptor-2.md) with types not known at compile time.

```cs
public static class ReflectionPropertyDescriptor
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ ReflectionPropertyDescriptor

## Members

### Methods

 | Name | Description |
| --- | --- |
| [FromPropertyInfo(PropertyInfo)](#frompropertyinfopropertyinfo) | Creates a binding property from reflected property. | 

## Details

### Methods

#### FromPropertyInfo(PropertyInfo)

Creates a binding property from reflected property.

```cs
public static StardewUI.Framework.Descriptors.IPropertyDescriptor FromPropertyInfo(System.Reflection.PropertyInfo propertyInfo);
```

##### Parameters

**`propertyInfo`** &nbsp; [PropertyInfo](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.propertyinfo)  
The reflected property info.

##### Returns

[IPropertyDescriptor](ipropertydescriptor.md)

  A binding property of type [ReflectionPropertyDescriptor&lt;T, TValue&gt;](reflectionpropertydescriptor-2.md), whose generic arguments are the property's [DeclaringType](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.memberinfo.declaringtype) and [PropertyType](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.propertyinfo.propertytype), respectively.

-----

