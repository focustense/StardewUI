---
title: ValueConverterFactory
description: Standard implementation of IValueConverterFactory that allows registering new converters.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class ValueConverterFactory

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Converters](index.md)  
Assembly: StardewUI.dll  

</div>

Standard implementation of [IValueConverterFactory](ivalueconverterfactory.md) that allows registering new converters.

```cs
public class ValueConverterFactory : 
    StardewUI.Framework.Converters.IValueConverterFactory
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ ValueConverterFactory

**Implements**  
[IValueConverterFactory](ivalueconverterfactory.md)

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [ValueConverterFactory()](#valueconverterfactory) | Initializes a new [ValueConverterFactory](valueconverterfactory.md) instance. | 

### Properties

 | Name | Description |
| --- | --- |
| [Factories](#factories) | The list of factories currently registered. | 

### Methods

 | Name | Description |
| --- | --- |
| [Register(IValueConverterFactory)](#registerivalueconverterfactory) | Registers a delegate factory that may be used to obtain a converter for which there is no explicit registration. | 
| [TryGetConverter(Type, Type, IValueConverter)](#trygetconvertertype-type-ivalueconverter) | Attempts to obtain a converter from a given source type to a given destination type. | 
| [TryGetConverter&lt;TSource, TDestination&gt;(IValueConverter&lt;TSource, TDestination&gt;)](#trygetconvertertsource-tdestinationivalueconvertertsource-tdestination) | Attempts to obtain a converter from a given source type to a given destination type. | 
| [TryRegister&lt;TSource, TDestination&gt;(IValueConverter&lt;TSource, TDestination&gt;)](#tryregistertsource-tdestinationivalueconvertertsource-tdestination) | Attempts to register a new converter. | 
| [TryRegister&lt;TSource, TDestination&gt;(Func&lt;TSource, TDestination&gt;)](#tryregistertsource-tdestinationfunctsource-tdestination) | Attempts to register a new converter. | 

## Details

### Constructors

#### ValueConverterFactory()

Initializes a new [ValueConverterFactory](valueconverterfactory.md) instance.

```cs
public ValueConverterFactory();
```

-----

### Properties

#### Factories

The list of factories currently registered.

```cs
protected System.Collections.Generic.List<StardewUI.Framework.Converters.IValueConverterFactory> Factories { get; }
```

##### Property Value

[List](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1)<[IValueConverterFactory](ivalueconverterfactory.md)>

-----

### Methods

#### Register(IValueConverterFactory)

Registers a delegate factory that may be used to obtain a converter for which there is no explicit registration.

```cs
public void Register(StardewUI.Framework.Converters.IValueConverterFactory factory);
```

##### Parameters

**`factory`** &nbsp; [IValueConverterFactory](ivalueconverterfactory.md)  
The delegate factory.

##### Remarks

Use when a single converter may handle many input or output types, e.g. string-to-enum conversions.

-----

#### TryGetConverter(Type, Type, IValueConverter)

Attempts to obtain a converter from a given source type to a given destination type.

```cs
public bool TryGetConverter(System.Type sourceType, System.Type destinationType, out StardewUI.Framework.Converters.IValueConverter converter);
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

#### TryRegister&lt;TSource, TDestination&gt;(IValueConverter&lt;TSource, TDestination&gt;)

Attempts to register a new converter.

```cs
public bool TryRegister<TSource, TDestination>(StardewUI.Framework.Converters.IValueConverter<TSource, TDestination> converter);
```

##### Parameters

**`converter`** &nbsp; [IValueConverter&lt;TSource, TDestination&gt;](ivalueconverter-2.md)  
The converter that handles this conversion.

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  `true` if the `converter` was registered for the specified types; `false` if there was already a registration or cached converter for those types.

-----

#### TryRegister&lt;TSource, TDestination&gt;(Func&lt;TSource, TDestination&gt;)

Attempts to register a new converter.

```cs
public bool TryRegister<TSource, TDestination>(Func<TSource, TDestination> convert);
```

##### Parameters

**`convert`** &nbsp; [Func&lt;TSource, TDestination&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2)  
Function to convert from `TSource` to `TDestination`.

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  `true` if the conversion function was registered for the specified types; `false` if there was already a registration or cached converter for those types.

-----

