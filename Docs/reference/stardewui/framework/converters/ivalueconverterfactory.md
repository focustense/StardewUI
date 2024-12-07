---
title: IValueConverterFactory
description: Factory for obtaining instance of IValueConverter&lt;TSource, TDestination&gt;.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Interface IValueConverterFactory

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Converters](index.md)  
Assembly: StardewUI.dll  

</div>

Factory for obtaining instance of [IValueConverter&lt;TSource, TDestination&gt;](ivalueconverter-2.md).

```cs
public interface IValueConverterFactory
```

## Members

### Methods

 | Name | Description |
| --- | --- |
| [GetConverter&lt;TSource, TDestination&gt;()](#getconvertertsource-tdestination) | Gets a converter from a given source type to a given destination type. | 
| [GetRequiredConverter&lt;TSource, TDestination&gt;()](#getrequiredconvertertsource-tdestination) | Gets a converter from a given source type to a given destination type, throwing if the conversion is not supported. | 
| [TryGetConverter&lt;TSource, TDestination&gt;(IValueConverter&lt;TSource, TDestination&gt;)](#trygetconvertertsource-tdestinationivalueconvertertsource-tdestination) | Attempts to obtain a converter from a given source type to a given destination type. | 
| [TryGetConverter(Type, Type, IValueConverter)](#trygetconvertertype-type-ivalueconverter) | Attempts to obtain a converter from a given source type to a given destination type. | 

## Details

### Methods

#### GetConverter&lt;TSource, TDestination&gt;()

Gets a converter from a given source type to a given destination type.

```cs
StardewUI.Framework.Converters.IValueConverter<TSource, TDestination> GetConverter<TSource, TDestination>();
```

##### Returns

[IValueConverter&lt;TSource, TDestination&gt;](ivalueconverter-2.md)

  A converter that converts from `TSource` to `TDestination`, or `null` if the conversion is not supported.

-----

#### GetRequiredConverter&lt;TSource, TDestination&gt;()

Gets a converter from a given source type to a given destination type, throwing if the conversion is not supported.

```cs
StardewUI.Framework.Converters.IValueConverter<TSource, TDestination> GetRequiredConverter<TSource, TDestination>();
```

##### Returns

[IValueConverter&lt;TSource, TDestination&gt;](ivalueconverter-2.md)

  A converter that converts from `TSource` to `TDestination`.

-----

#### TryGetConverter&lt;TSource, TDestination&gt;(IValueConverter&lt;TSource, TDestination&gt;)

Attempts to obtain a converter from a given source type to a given destination type.

```cs
bool TryGetConverter<TSource, TDestination>(out IValueConverter<TSource, TDestination> converter);
```

##### Parameters

**`converter`** &nbsp; [IValueConverter&lt;TSource, TDestination&gt;](ivalueconverter-2.md)  
If the method returns `true`, holds the converter that converts between the specified types; otherwise `null`.

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  `true` if the conversion is supported, otherwise `false`.

-----

#### TryGetConverter(Type, Type, IValueConverter)

Attempts to obtain a converter from a given source type to a given destination type.

```cs
bool TryGetConverter(System.Type sourceType, System.Type destinationType, out StardewUI.Framework.Converters.IValueConverter converter);
```

##### Parameters

**`sourceType`** &nbsp; [Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)  
The type of value to be converted.

**`destinationType`** &nbsp; [Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)  
The converted value type.

**`converter`** &nbsp; [IValueConverter](ivalueconverter.md)  
If the method returns `true`, holds the converter that converts between the specified types; otherwise `null`.

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  `true` if the conversion is supported, otherwise `false`.

-----

