---
title: BaseTypeConverterFactory
description: Allows implicit conversion from a type's ancestor to the destination type, if the source type does not have its own explicitly-defined conversion but a base type does.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class BaseTypeConverterFactory

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Converters](index.md)  
Assembly: StardewUI.dll  

</div>

Allows implicit conversion from a type's ancestor to the destination type, if the source type does not have its own explicitly-defined conversion but a base type does.

```cs
public class BaseTypeConverterFactory : 
    StardewUI.Framework.Converters.IValueConverterFactory
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ BaseTypeConverterFactory

**Implements**  
[IValueConverterFactory](ivalueconverterfactory.md)

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [BaseTypeConverterFactory(IValueConverterFactory)](#basetypeconverterfactoryivalueconverterfactory) | Allows implicit conversion from a type's ancestor to the destination type, if the source type does not have its own explicitly-defined conversion but a base type does. | 

### Methods

 | Name | Description |
| --- | --- |
| [TryGetConverter&lt;TSource, TDestination&gt;(IValueConverter&lt;TSource, TDestination&gt;)](#trygetconvertertsource-tdestinationivalueconvertertsource-tdestination) | Attempts to obtain a converter from a given source type to a given destination type. | 

## Details

### Constructors

#### BaseTypeConverterFactory(IValueConverterFactory)

Allows implicit conversion from a type's ancestor to the destination type, if the source type does not have its own explicitly-defined conversion but a base type does.

```cs
public BaseTypeConverterFactory(StardewUI.Framework.Converters.IValueConverterFactory innerFactory);
```

##### Parameters

**`innerFactory`** &nbsp; [IValueConverterFactory](ivalueconverterfactory.md)  
The converter factory to handle conversion of ancestor types.

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

