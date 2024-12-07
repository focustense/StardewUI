---
title: StringConverterFactory
description: Provides conversions from any type to string.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class StringConverterFactory

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Converters](index.md)  
Assembly: StardewUI.dll  

</div>

Provides conversions from any type to [string](https://learn.microsoft.com/en-us/dotnet/api/system.string).

```cs
public class StringConverterFactory : 
    StardewUI.Framework.Converters.IValueConverterFactory
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ StringConverterFactory

**Implements**  
[IValueConverterFactory](ivalueconverterfactory.md)

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [StringConverterFactory()](#stringconverterfactory) |  | 

### Methods

 | Name | Description |
| --- | --- |
| [TryGetConverter&lt;TSource, TDestination&gt;(IValueConverter&lt;TSource, TDestination&gt;)](#trygetconvertertsource-tdestinationivalueconvertertsource-tdestination) | Attempts to obtain a converter from a given source type to a given destination type. | 

## Details

### Constructors

#### StringConverterFactory()



```cs
public StringConverterFactory();
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

