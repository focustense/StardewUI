---
title: DuckTypeEnumConverterFactory
description: Factory that automatically implements duck-typing conversions between enum types that share the same names.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class DuckTypeEnumConverterFactory

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Converters](index.md)  
Assembly: StardewUI.dll  

</div>

Factory that automatically implements duck-typing conversions between enum types that share the same names.

```cs
public class DuckTypeEnumConverterFactory : 
    StardewUI.Framework.Converters.IValueConverterFactory
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ DuckTypeEnumConverterFactory

**Implements**  
[IValueConverterFactory](ivalueconverterfactory.md)

## Remarks

Enum values do not need to be identical; matching is performed on the (case-insensitive) name.

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [DuckTypeEnumConverterFactory()](#ducktypeenumconverterfactory) |  | 

### Methods

 | Name | Description |
| --- | --- |
| [TryGetConverter&lt;TSource, TDestination&gt;(IValueConverter&lt;TSource, TDestination&gt;)](#trygetconvertertsource-tdestinationivalueconvertertsource-tdestination) | Attempts to obtain a converter from a given source type to a given destination type. | 

## Details

### Constructors

#### DuckTypeEnumConverterFactory()



```cs
public DuckTypeEnumConverterFactory();
```

-----

### Methods

#### TryGetConverter&lt;TSource, TDestination&gt;(IValueConverter&lt;TSource, TDestination&gt;)

Attempts to obtain a converter from a given source type to a given destination type.

```cs
public bool TryGetConverter<TSource, TDestination>(out IValueConverter<TSource, TDestination> converter);
```

##### Parameters

**`converter`** &nbsp; [IValueConverter&lt;TSource, TDestination&gt;](ivalueconverter-2.md)  
If the method returns `true`, holds the converter that converts between the specified types; otherwise `null`.

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  `true` if the conversion is supported, otherwise `false`.

-----

