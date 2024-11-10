---
title: ConvertedValueSource&lt;TSource, T&gt;
description: A value source that wraps another IValueSource&lt;T&gt; and performs automatic conversion.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class ConvertedValueSource&lt;TSource, T&gt;

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Sources](index.md)  
Assembly: StardewUI.dll  

</div>

A value source that wraps another [IValueSource&lt;T&gt;](ivaluesource-1.md) and performs automatic conversion.

```cs
public class ConvertedValueSource<TSource, T> : 
    StardewUI.Framework.Sources.IValueSource<T>, 
    StardewUI.Framework.Sources.IValueSource
```

### Type Parameters

**`TSource`**  
The original value type, i.e. of the `source`.

**`T`**  
The converted value type.


**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ ConvertedValueSource&lt;TSource, T&gt;

**Implements**  
[IValueSource&lt;T&gt;](ivaluesource-1.md), [IValueSource](ivaluesource.md)

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [ConvertedValueSource&lt;TSource, T&gt;(IValueSource&lt;TSource&gt;, IValueConverter&lt;TSource, T&gt;, IValueConverter&lt;T, TSource&gt;)](#convertedvaluesourcetsource-tivaluesourcetsource-ivalueconvertertsource-t-ivalueconvertert-tsource) | A value source that wraps another [IValueSource&lt;T&gt;](ivaluesource-1.md) and performs automatic conversion. | 

### Properties

 | Name | Description |
| --- | --- |
| [CanRead](#canread) | Whether or not the source can be read from, i.e. if an attempt to **get** the [Value](ivaluesource.md#value) should succeed. | 
| [CanWrite](#canwrite) | Whether or not the source can be written back to, i.e. if an attempt to **set** the [Value](ivaluesource.md#value) should succeed. | 
| [DisplayName](#displayname) | Descriptive name for the property, used primarily for debug views and log/exception messages. | 
| [Value](#value) |  | 
| [ValueType](#valuetype) | The compile-time type of the value tracked by this source; the type parameter for [IValueSource&lt;T&gt;](ivaluesource-1.md). | 

### Methods

 | Name | Description |
| --- | --- |
| [Update(Boolean)](#updatebool) | Checks if the value needs updating, and if so, updates [Value](ivaluesource.md#value) to the latest. | 

## Details

### Constructors

#### ConvertedValueSource&lt;TSource, T&gt;(IValueSource&lt;TSource&gt;, IValueConverter&lt;TSource, T&gt;, IValueConverter&lt;T, TSource&gt;)

A value source that wraps another [IValueSource&lt;T&gt;](ivaluesource-1.md) and performs automatic conversion.

```cs
public ConvertedValueSource<TSource, T>(StardewUI.Framework.Sources.IValueSource<TSource> source, StardewUI.Framework.Converters.IValueConverter<TSource, T> inputConverter, StardewUI.Framework.Converters.IValueConverter<T, TSource> outputConverter);
```

##### Parameters

**`source`** &nbsp; [IValueSource&lt;TSource&gt;](ivaluesource-1.md)  
The original value source.

**`inputConverter`** &nbsp; [IValueConverter&lt;TSource, T&gt;](../converters/ivalueconverter-2.md)  
A converter that converts from `TSource` to `T`. If this is `null`, then this instance's [Value](convertedvaluesource-2.md#value) will always be `null` and [CanRead](convertedvaluesource-2.md#canread) will be `false` regardless of the underlying `source`'s readability.

**`outputConverter`** &nbsp; [IValueConverter&lt;T, TSource&gt;](../converters/ivalueconverter-2.md)  
A converter that converts from `T` to `TSource`. If this is `null`, then this instance cannot accept any assignments to the [Value](convertedvaluesource-2.md#value) property, and [CanWrite](convertedvaluesource-2.md#canwrite) will always be `false` regardless of the underlying `source`'s writability.

-----

### Properties

#### CanRead

Whether or not the source can be read from, i.e. if an attempt to **get** the [Value](ivaluesource.md#value) should succeed.

```cs
public bool CanRead { get; }
```

##### Property Value

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

-----

#### CanWrite

Whether or not the source can be written back to, i.e. if an attempt to **set** the [Value](ivaluesource.md#value) should succeed.

```cs
public bool CanWrite { get; }
```

##### Property Value

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

-----

#### DisplayName

Descriptive name for the property, used primarily for debug views and log/exception messages.

```cs
public string DisplayName { get; }
```

##### Property Value

[string](https://learn.microsoft.com/en-us/dotnet/api/system.string)

-----

#### Value



```cs
public T Value { get; set; }
```

##### Property Value

`T`

-----

#### ValueType

The compile-time type of the value tracked by this source; the type parameter for [IValueSource&lt;T&gt;](ivaluesource-1.md).

```cs
public System.Type ValueType { get; }
```

##### Property Value

[Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)

-----

### Methods

#### Update(bool)

Checks if the value needs updating, and if so, updates [Value](ivaluesource.md#value) to the latest.

```cs
public bool Update(bool force);
```

##### Parameters

**`force`** &nbsp; [Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)  
If `true`, forces the source to update its value even if it isn't considered dirty. This should never be used in a regular binding, but can be useful in sources that are intended for occasional or one-shot use such as event handler arguments.

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  `true` if the [Value](ivaluesource.md#value) was updated; `false` if it already held the most recent value.

##### Remarks

This method is called every frame, for every binding, and providing a correct return value is essential in order to avoid slowdowns due to unnecessary rebinds.

-----

