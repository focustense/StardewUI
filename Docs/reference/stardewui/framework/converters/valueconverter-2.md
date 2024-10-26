---
title: ValueConverter&lt;TSource, TDestination&gt;
description: Generic delegating converter, accepting a conversion function.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class ValueConverter&lt;TSource, TDestination&gt;

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Converters](index.md)  
Assembly: StardewUI.dll  

</div>

Generic delegating converter, accepting a conversion function.

```cs
public class ValueConverter<TSource, TDestination> : 
    StardewUI.Framework.Converters.IValueConverter<TSource, TDestination>
```

### Type Parameters

**`TSource`**  
The type of value to be converted.

**`TDestination`**  
The converted value type.


**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ ValueConverter&lt;TSource, TDestination&gt;

**Implements**  
[IValueConverter&lt;TSource, TDestination&gt;](ivalueconverter-2.md)

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [ValueConverter&lt;TSource, TDestination&gt;(Func&lt;TSource, TDestination&gt;)](#valueconvertertsource-tdestinationfunctsource-tdestination) | Generic delegating converter, accepting a conversion function. | 

### Methods

 | Name | Description |
| --- | --- |
| [Convert(TSource)](#converttsource) | Converts a value from the source type to the destination type. | 

## Details

### Constructors

#### ValueConverter&lt;TSource, TDestination&gt;(Func&lt;TSource, TDestination&gt;)

Generic delegating converter, accepting a conversion function.

```cs
public ValueConverter<TSource, TDestination>(Func<TSource, TDestination> convert);
```

##### Parameters

**`convert`** &nbsp; [Func&lt;TSource, TDestination&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2)  
Function to convert a `TSource` to a `TDestination`.

-----

### Methods

#### Convert(TSource)

Converts a value from the source type to the destination type.

```cs
public TDestination Convert(TSource value);
```

##### Parameters

**`value`** &nbsp; TSource  
The value to convert.

##### Returns

`TDestination`

-----

