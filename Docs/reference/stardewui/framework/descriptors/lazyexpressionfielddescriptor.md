---
title: LazyExpressionFieldDescriptor
description: Helper for creating LazyExpressionFieldDescriptor`1 with types not known at compile time.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class LazyExpressionFieldDescriptor

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Descriptors](index.md)  
Assembly: StardewUI.dll  

</div>

Helper for creating [LazyExpressionFieldDescriptor&lt;TValue&gt;](lazyexpressionfielddescriptor-1.md) with types not known at compile time.

```cs
public static class LazyExpressionFieldDescriptor
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ LazyExpressionFieldDescriptor

## Members

### Methods

 | Name | Description |
| --- | --- |
| [FromFieldInfo(FieldInfo)](#fromfieldinfofieldinfo) | Creates a binding field from reflected field info. | 

## Details

### Methods

#### FromFieldInfo(FieldInfo)

Creates a binding field from reflected field info.

```cs
public static StardewUI.Framework.Descriptors.IPropertyDescriptor FromFieldInfo(System.Reflection.FieldInfo fieldInfo);
```

##### Parameters

**`fieldInfo`** &nbsp; [FieldInfo](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.fieldinfo)  
The reflected field info.

##### Returns

[IPropertyDescriptor](ipropertydescriptor.md)

  A binding field of type [LazyExpressionFieldDescriptor&lt;TValue&gt;](lazyexpressionfielddescriptor-1.md), whose generic argument is the field's [FieldType](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.fieldinfo.fieldtype).

-----

