---
title: BindingContext
description: Context, or scope, of a bound view, providing the backing data and tools for accessing its properties.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class BindingContext

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Binding](index.md)  
Assembly: StardewUI.dll  

</div>

Context, or scope, of a bound view, providing the backing data and tools for accessing its properties.

```cs
public record BindingContext : 
    IEquatable<StardewUI.Framework.Binding.BindingContext>
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ BindingContext

**Implements**  
[IEquatable](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1)<[BindingContext](bindingcontext.md)>

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [BindingContext(IObjectDescriptor, Object, BindingContext)](#bindingcontextiobjectdescriptor-object-bindingcontext) | Context, or scope, of a bound view, providing the backing data and tools for accessing its properties. | 

### Properties

 | Name | Description |
| --- | --- |
| [Data](#data) | The bound data. | 
| [Descriptor](#descriptor) | Descriptor of the [Data](bindingcontext.md#data) type, used to read current values. | 
| [EqualityContract](#equalitycontract) |  | 
| [Parent](#parent) | The parent context from which this context was derived, if any. | 

### Methods

 | Name | Description |
| --- | --- |
| [Create(Object, BindingContext)](#createobject-bindingcontext) | Creates a [BindingContext](bindingcontext.md) from the specified data, automatically building a new descriptor if the data type has not been previously seen. | 
| [Redirect(ContextRedirect)](#redirectcontextredirect) | Resolves a redirected context, using this context as the starting point. | 

## Details

### Constructors

#### BindingContext(IObjectDescriptor, Object, BindingContext)

Context, or scope, of a bound view, providing the backing data and tools for accessing its properties.

```cs
public BindingContext(StardewUI.Framework.Descriptors.IObjectDescriptor Descriptor, System.Object Data, StardewUI.Framework.Binding.BindingContext Parent);
```

##### Parameters

**`Descriptor`** &nbsp; [IObjectDescriptor](../descriptors/iobjectdescriptor.md)  
Descriptor of the [Data](bindingcontext.md#data) type, used to read current values.

**`Data`** &nbsp; [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object)  
The bound data.

**`Parent`** &nbsp; [BindingContext](bindingcontext.md)  
The parent context from which this context was derived, if any.

-----

### Properties

#### Data

The bound data.

```cs
public System.Object Data { get; set; }
```

##### Property Value

[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object)

-----

#### Descriptor

Descriptor of the [Data](bindingcontext.md#data) type, used to read current values.

```cs
public StardewUI.Framework.Descriptors.IObjectDescriptor Descriptor { get; set; }
```

##### Property Value

[IObjectDescriptor](../descriptors/iobjectdescriptor.md)

-----

#### EqualityContract



```cs
protected System.Type EqualityContract { get; }
```

##### Property Value

[Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)

-----

#### Parent

The parent context from which this context was derived, if any.

```cs
public StardewUI.Framework.Binding.BindingContext Parent { get; set; }
```

##### Property Value

[BindingContext](bindingcontext.md)

-----

### Methods

#### Create(Object, BindingContext)

Creates a [BindingContext](bindingcontext.md) from the specified data, automatically building a new descriptor if the data type has not been previously seen.

```cs
public static StardewUI.Framework.Binding.BindingContext Create(System.Object data, StardewUI.Framework.Binding.BindingContext parent);
```

##### Parameters

**`data`** &nbsp; [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object)  
The bound data.

**`parent`** &nbsp; [BindingContext](bindingcontext.md)  
The parent context from which this context was derived, if any.

##### Returns

[BindingContext](bindingcontext.md)

  A new [BindingContext](bindingcontext.md) whose [Data](bindingcontext.md#data) is the specified `data` and whose [Descriptor](bindingcontext.md#descriptor) is the descriptor of `data`'s runtime type.

-----

#### Redirect(ContextRedirect)

Resolves a redirected context, using this context as the starting point.

```cs
public StardewUI.Framework.Binding.BindingContext Redirect(StardewUI.Framework.Dom.ContextRedirect redirect);
```

##### Parameters

**`redirect`** &nbsp; [ContextRedirect](../dom/contextredirect.md)  
The redirect data.

##### Returns

[BindingContext](bindingcontext.md)

  The resolved [BindingContext](bindingcontext.md), or `null` if the `redirect` does not resolve to a valid context.

-----

