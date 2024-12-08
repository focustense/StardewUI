---
title: ConvertedValueSource
description: Helpers for creating instances of the generic ConvertedValueSource&lt;TSource, T&gt; when some of the types are unknown at compile time.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class ConvertedValueSource

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Sources](index.md)  
Assembly: StardewUI.dll  

</div>

Helpers for creating instances of the generic [ConvertedValueSource&lt;TSource, T&gt;](convertedvaluesource-2.md) when some of the types are unknown at compile time.

```cs
public static class ConvertedValueSource
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ ConvertedValueSource

## Members

### Methods

 | Name | Description |
| --- | --- |
| [Create(IValueSource, Type, IValueConverterFactory, Boolean)](#createivaluesource-type-ivalueconverterfactory-bool) | Creates a converted source with a specified output type, using an original source with unknown value type. | 
| [Create&lt;T&gt;(IValueSource, IValueConverterFactory, Boolean)](#createtivaluesource-ivalueconverterfactory-bool) | Creates a converted source with a specified output type, using an original source with unknown value type. | 

## Details

### Methods

#### Create(IValueSource, Type, IValueConverterFactory, bool)

Creates a converted source with a specified output type, using an original source with unknown value type.

```cs
public static StardewUI.Framework.Sources.IValueSource Create(StardewUI.Framework.Sources.IValueSource original, System.Type destinationType, StardewUI.Framework.Converters.IValueConverterFactory converterFactory, bool twoWay);
```

##### Parameters

**`original`** &nbsp; [IValueSource](ivaluesource.md)  
The original value source.

**`destinationType`** &nbsp; [Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)  
The type to convert to.

**`converterFactory`** &nbsp; [IValueConverterFactory](../converters/ivalueconverterfactory.md)  
Factory for creating instances of [IValueConverter&lt;TSource, TDestination&gt;](../converters/ivalueconverter-2.md).

**`twoWay`** &nbsp; [Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)  
Whether the resulting [IValueSource](ivaluesource.md) should be able to convert in the reverse direction, i.e. for two-way bindings, by setting [Value](ivaluesource.md#value).

##### Returns

[IValueSource](ivaluesource.md)

-----

#### Create&lt;T&gt;(IValueSource, IValueConverterFactory, bool)

Creates a converted source with a specified output type, using an original source with unknown value type.

```cs
public static StardewUI.Framework.Sources.IValueSource<T> Create<T>(StardewUI.Framework.Sources.IValueSource original, StardewUI.Framework.Converters.IValueConverterFactory converterFactory, bool twoWay);
```

##### Parameters

**`original`** &nbsp; [IValueSource](ivaluesource.md)  
The original value source.

**`converterFactory`** &nbsp; [IValueConverterFactory](../converters/ivalueconverterfactory.md)  
Factory for creating instances of [IValueConverter&lt;TSource, TDestination&gt;](../converters/ivalueconverter-2.md).

**`twoWay`** &nbsp; [Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)  
Whether the resulting [IValueSource](ivaluesource.md) should be able to convert in the reverse direction, i.e. for two-way bindings, by setting [Value](ivaluesource.md#value).

##### Returns

[IValueSource&lt;T&gt;](ivaluesource-1.md)

-----

