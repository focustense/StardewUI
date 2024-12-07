---
title: ContextPropertyValueSource&lt;T&gt;
description: Value source that obtains its value from a context (or "model") property.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class ContextPropertyValueSource&lt;T&gt;

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Sources](index.md)  
Assembly: StardewUI.dll  

</div>

Value source that obtains its value from a context (or "model") property.

```cs
public class ContextPropertyValueSource<T> : 
    StardewUI.Framework.Sources.IValueSource<T>, 
    StardewUI.Framework.Sources.IValueSource, System.IDisposable
```

### Type Parameters

**`T`**  
The return type of the context property.


**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ ContextPropertyValueSource&lt;T&gt;

**Implements**  
[IValueSource&lt;T&gt;](ivaluesource-1.md), [IValueSource](ivaluesource.md), [IDisposable](https://learn.microsoft.com/en-us/dotnet/api/system.idisposable)

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [ContextPropertyValueSource&lt;T&gt;(BindingContext, string, Boolean)](#contextpropertyvaluesourcetbindingcontext-string-bool) | Initializes a new instance of [ContextPropertyValueSource&lt;T&gt;](contextpropertyvaluesource-1.md) using the specified context and property name. | 

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
| [Dispose()](#dispose) |  | 
| [Update(Boolean)](#updatebool) | Checks if the value needs updating, and if so, updates [Value](ivaluesource.md#value) to the latest. | 

## Details

### Constructors

#### ContextPropertyValueSource&lt;T&gt;(BindingContext, string, bool)

Initializes a new instance of [ContextPropertyValueSource&lt;T&gt;](contextpropertyvaluesource-1.md) using the specified context and property name.

```cs
public ContextPropertyValueSource<T>(StardewUI.Framework.Binding.BindingContext context, string propertyName, bool allowUpdates);
```

##### Parameters

**`context`** &nbsp; [BindingContext](../binding/bindingcontext.md)  
Context used for the data binding.

**`propertyName`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
Property to read on the [Data](../binding/bindingcontext.md#data) of the supplied `context` when updating.

**`allowUpdates`** &nbsp; [Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)  
Whether or not to allow [Update(Boolean)](contextpropertyvaluesource-1.md#updatebool) to read a new value. `false` prevents all updates and makes the source read only one time.

##### Remarks

If the [Data](../binding/bindingcontext.md#data) of the supplied `context` implements [INotifyPropertyChanged](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged), then [Update(Boolean)](contextpropertyvaluesource-1.md#updatebool) and [Value](contextpropertyvaluesource-1.md#value) will respond to changes to the given `propertyName`. Otherwise, the source is "static" and will never change its value or return `true` from [Update(Boolean)](contextpropertyvaluesource-1.md#updatebool).

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

#### Dispose()



```cs
public void Dispose();
```

-----

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

