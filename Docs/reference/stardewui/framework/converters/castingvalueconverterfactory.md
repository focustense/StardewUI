---
title: CastingValueConverterFactory
description: Factory that automatically implements casting conversions, where the source type can be assigned directly to the destination type.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class CastingValueConverterFactory

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Converters](index.md)  
Assembly: StardewUI.dll  

</div>

Factory that automatically implements casting conversions, where the source type can be assigned directly to the destination type.

```cs
public class CastingValueConverterFactory : 
    StardewUI.Framework.Converters.IValueConverterFactory
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ CastingValueConverterFactory

**Implements**  
[IValueConverterFactory](ivalueconverterfactory.md)

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [CastingValueConverterFactory()](#castingvalueconverterfactory) |  | 

### Methods

 | Name | Description |
| --- | --- |
| [TryGetConverter&lt;TSource, TDestination&gt;(IValueConverter&lt;TSource, TDestination&gt;)](#trygetconvertertsource-tdestinationivalueconvertertsource-tdestination) | Attempts to obtain a converter from a given source type to a given destination type. | 

## Details

### Constructors

#### CastingValueConverterFactory()



```cs
public CastingValueConverterFactory();
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

