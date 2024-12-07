---
title: DuckPropertyAttribute
description: Specifies a property name to use for duck-type conversions, if different from the member name.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class DuckPropertyAttribute

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI](index.md)  
Assembly: StardewUI.dll  

</div>

Specifies a property name to use for duck-type conversions, if different from the member name.

```cs
[System.AttributeUsage]
public class DuckPropertyAttribute : System.Attribute
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ [Attribute](https://learn.microsoft.com/en-us/dotnet/api/system.attribute) ⇦ DuckPropertyAttribute

## Remarks

This attribute is applied to the type being converted _from_, unlike [DuckTypeAttribute](ducktypeattribute.md) which applies to the target type. It is used to match a target property with a different name, or to match multiple target properties with a single source field. 

 Multiple copies of the attribute can be used to match multiple target properties, with `targetTypeName` being used to optionally filter which type conversions it will apply to, if the data type might be used in more than one kind of conversion.

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [DuckPropertyAttribute(string, string)](#duckpropertyattributestring-string) | Specifies a property name to use for duck-type conversions, if different from the member name. | 

### Properties

 | Name | Description |
| --- | --- |
| [TargetPropertyName](#targetpropertyname) | The name of the property to match on the target type. | 
| [TargetTypeName](#targettypename) | Name of the conversion target type (i.e. type with [DuckTypeAttribute](ducktypeattribute.md)) to which this rename applies, not including the namespace or generic arguments. If not set, the property will be available under the specified [TargetPropertyName](duckpropertyattribute.md#targetpropertyname) for all conversions. | 
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

#### DuckPropertyAttribute(string, string)

Specifies a property name to use for duck-type conversions, if different from the member name.

```cs
public DuckPropertyAttribute(string targetPropertyName, string targetTypeName);
```

##### Parameters

**`targetPropertyName`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The name of the property to match on the target type.

**`targetTypeName`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
Name of the conversion target type (i.e. type with [DuckTypeAttribute](ducktypeattribute.md)) to which this rename applies, not including the namespace or generic arguments. If not set, the property will be available under the specified `targetPropertyName` for all conversions.

##### Remarks

This attribute is applied to the type being converted _from_, unlike [DuckTypeAttribute](ducktypeattribute.md) which applies to the target type. It is used to match a target property with a different name, or to match multiple target properties with a single source field. 

 Multiple copies of the attribute can be used to match multiple target properties, with `targetTypeName` being used to optionally filter which type conversions it will apply to, if the data type might be used in more than one kind of conversion.

-----

### Properties

#### TargetPropertyName

The name of the property to match on the target type.

```cs
public string TargetPropertyName { get; }
```

##### Property Value

[string](https://learn.microsoft.com/en-us/dotnet/api/system.string)

-----

#### TargetTypeName

Name of the conversion target type (i.e. type with [DuckTypeAttribute](ducktypeattribute.md)) to which this rename applies, not including the namespace or generic arguments. If not set, the property will be available under the specified [TargetPropertyName](duckpropertyattribute.md#targetpropertyname) for all conversions.

```cs
public string TargetTypeName { get; }
```

##### Property Value

[string](https://learn.microsoft.com/en-us/dotnet/api/system.string)

-----

