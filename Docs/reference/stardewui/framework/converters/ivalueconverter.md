---
title: IValueConverter
description: Provides a method to convert between arbitrary types.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Interface IValueConverter

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Converters](index.md)  
Assembly: StardewUI.dll  

</div>

Provides a method to convert between arbitrary types.

```cs
public interface IValueConverter
```

## Remarks

This is a non-generic version of the [IValueConverter&lt;TSource, TDestination&gt;](ivalueconverter-2.md) that should normally only be used by framework code. Avoid implementing this directly; instead prefer the generic version, which implicitly implements this interface.

## Members

### Properties

 | Name | Description |
| --- | --- |
| [DestinationType](#destinationtype) | The type of object that this converts to; the result type of the [Convert(Object)](ivalueconverter.md#convertobject) method. | 
| [SourceType](#sourcetype) | The type of object this converts from; the `value` argument to the [Convert(Object)](ivalueconverter.md#convertobject) method. | 

### Methods

 | Name | Description |
| --- | --- |
| [Convert(Object)](#convertobject) | Converts a value from the [SourceType](ivalueconverter.md#sourcetype) to the [DestinationType](ivalueconverter.md#destinationtype). | 

## Details

### Properties

#### DestinationType

The type of object that this converts to; the result type of the [Convert(Object)](ivalueconverter.md#convertobject) method.

```cs
System.Type DestinationType { get; }
```

##### Property Value

[Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)

-----

#### SourceType

The type of object this converts from; the `value` argument to the [Convert(Object)](ivalueconverter.md#convertobject) method.

```cs
System.Type SourceType { get; }
```

##### Property Value

[Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)

-----

### Methods

#### Convert(Object)

Converts a value from the [SourceType](ivalueconverter.md#sourcetype) to the [DestinationType](ivalueconverter.md#destinationtype).

```cs
System.Object Convert(System.Object value);
```

##### Parameters

**`value`** &nbsp; [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object)  
The value to convert.

##### Returns

[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object)

  The converted value.

-----

