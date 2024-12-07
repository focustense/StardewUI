---
title: AnyCastConverterFactory
description: Factory supporting conversions to and from IAnyCast.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class AnyCastConverterFactory

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Converters](index.md)  
Assembly: StardewUI.dll  

</div>

Factory supporting conversions to and from [IAnyCast](../ianycast.md).

```cs
public class AnyCastConverterFactory : 
    StardewUI.Framework.Converters.IValueConverterFactory
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ AnyCastConverterFactory

**Implements**  
[IValueConverterFactory](ivalueconverterfactory.md)

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [AnyCastConverterFactory()](#anycastconverterfactory) |  | 

### Methods

 | Name | Description |
| --- | --- |
| [TryGetConverter&lt;TSource, TDestination&gt;(IValueConverter&lt;TSource, TDestination&gt;)](#trygetconvertertsource-tdestinationivalueconvertertsource-tdestination) | Attempts to obtain a converter from a given source type to a given destination type. | 

## Details

### Constructors

#### AnyCastConverterFactory()



```cs
public AnyCastConverterFactory();
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

