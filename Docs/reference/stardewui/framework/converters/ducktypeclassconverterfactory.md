---
title: DuckTypeClassConverterFactory
description: Factory that creates duck-typing converters for class and struct types.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class DuckTypeClassConverterFactory

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Converters](index.md)  
Assembly: StardewUI.dll  

</div>

Factory that creates duck-typing converters for `class` and `struct` types.

```cs
public class DuckTypeClassConverterFactory : 
    StardewUI.Framework.Converters.IValueConverterFactory
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ DuckTypeClassConverterFactory

**Implements**  
[IValueConverterFactory](ivalueconverterfactory.md)

## Remarks

For the conversion to be allowed: 

  - The `TDestination` type must be annotated with [DuckTypeAttribute](../../ducktypeattribute.md).
  - The destination type must have either a default constructor, or a constructor that can be completely satisfied by properties/fields of the `TSource` type.
  - If the best or only constructor match is the default/parameterless constructor, at least one writable property on the target type must be satisfied by a property/field on the source type.

 Additionally, source types may use fields or properties, but only constructor arguments and properties will be considered on the destination type.

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [DuckTypeClassConverterFactory(IValueConverterFactory)](#ducktypeclassconverterfactoryivalueconverterfactory) | Factory that creates duck-typing converters for `class` and `struct` types. | 

### Properties

 | Name | Description |
| --- | --- |
| [EnableDebugOutput](#enabledebugoutput) | Whether or not to print MSIL output for generated conversion methods. | 

### Methods

 | Name | Description |
| --- | --- |
| [TryGetConverter&lt;TSource, TDestination&gt;(IValueConverter&lt;TSource, TDestination&gt;)](#trygetconvertertsource-tdestinationivalueconvertertsource-tdestination) | Attempts to obtain a converter from a given source type to a given destination type. | 

## Details

### Constructors

#### DuckTypeClassConverterFactory(IValueConverterFactory)

Factory that creates duck-typing converters for `class` and `struct` types.

```cs
public DuckTypeClassConverterFactory(StardewUI.Framework.Converters.IValueConverterFactory innerFactory);
```

##### Parameters

**`innerFactory`** &nbsp; [IValueConverterFactory](ivalueconverterfactory.md)  
The converter factory to handle conversion of individual properties/arguments.

##### Remarks

For the conversion to be allowed: 

  - The `TDestination` type must be annotated with [DuckTypeAttribute](../../ducktypeattribute.md).
  - The destination type must have either a default constructor, or a constructor that can be completely satisfied by properties/fields of the `TSource` type.
  - If the best or only constructor match is the default/parameterless constructor, at least one writable property on the target type must be satisfied by a property/field on the source type.

 Additionally, source types may use fields or properties, but only constructor arguments and properties will be considered on the destination type.

-----

### Properties

#### EnableDebugOutput

Whether or not to print MSIL output for generated conversion methods.

```cs
public bool EnableDebugOutput { get; set; }
```

##### Property Value

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

##### Remarks

Use for troubleshooting misbehaving converters or AVE crashes.

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

