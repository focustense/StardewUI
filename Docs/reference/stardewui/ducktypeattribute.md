---
title: DuckTypeAttribute
description: Specifies that a type is eligible for duck-type conversions in data bindings.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class DuckTypeAttribute

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI](index.md)  
Assembly: StardewUI.dll  

</div>

Specifies that a type is eligible for duck-type conversions in data bindings.

```cs
[System.AttributeUsage]
public class DuckTypeAttribute : System.Attribute
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ [Attribute](https://learn.microsoft.com/en-us/dotnet/api/system.attribute) ⇦ DuckTypeAttribute

## Remarks

This attribute is not used by the core library, only the data binding framework. When a type is decorated with it, values of normally non-convertible types, such as user-defined types in a separate mod, can become eligible for conversion to the decorated type and have converters generated at runtime, as long as the external type's properties are sufficient to satisfy one of the decorated type's constructors; or, in the case of default constructors, when the external type can contribute at least one property value. 

 Duck type conversions always match using the combined property type and (case-insensitive) name. The name of the decorated type's property or constructor argument must match the name of the property on the source type, unless [DuckPropertyAttribute](duckpropertyattribute.md) is specified, in which case it must match the specified name(s).

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [DuckTypeAttribute()](#ducktypeattribute) |  | 

### Properties

 | Name | Description |
| --- | --- |
| [TypeId](https://learn.microsoft.com/en-us/dotnet/api/system.attribute.typeid) | <span class="muted" markdown>(Inherited from [Attribute](https://learn.microsoft.com/en-us/dotnet/api/system.attribute))</span> | 

### Methods

 | Name | Description |
| --- | --- |
| [Equals(Object)](https://learn.microsoft.com/en-us/dotnet/api/system.attribute.equals) | <span class="muted" markdown>(Inherited from [Attribute](https://learn.microsoft.com/en-us/dotnet/api/system.attribute))</span> | 
| [GetHashCode()](https://learn.microsoft.com/en-us/dotnet/api/system.attribute.gethashcode) | <span class="muted" markdown>(Inherited from [Attribute](https://learn.microsoft.com/en-us/dotnet/api/system.attribute))</span> | 
| [IsDefaultAttribute()](https://learn.microsoft.com/en-us/dotnet/api/system.attribute.isdefaultattribute) | <span class="muted" markdown>(Inherited from [Attribute](https://learn.microsoft.com/en-us/dotnet/api/system.attribute))</span> | 
| [Match(Object)](https://learn.microsoft.com/en-us/dotnet/api/system.attribute.match) | <span class="muted" markdown>(Inherited from [Attribute](https://learn.microsoft.com/en-us/dotnet/api/system.attribute))</span> | 

## Details

### Constructors

#### DuckTypeAttribute()



```cs
public DuckTypeAttribute();
```

-----

