---
title: IPropertyDescriptor&lt;T&gt;
description: Describes a single property on a bindable object (i.e. a view) and provides methods to read or write the value.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Interface IPropertyDescriptor&lt;T&gt;

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Descriptors](index.md)  
Assembly: StardewUI.dll  

</div>

Describes a single property on a bindable object (i.e. a view) and provides methods to read or write the value.

```cs
public interface IPropertyDescriptor<T> : 
    StardewUI.Framework.Descriptors.IPropertyDescriptor, 
    StardewUI.Framework.Descriptors.IMemberDescriptor
```

### Type Parameters

**`T`**  
The property type.


**Implements**  
[IPropertyDescriptor](ipropertydescriptor.md), [IMemberDescriptor](imemberdescriptor.md)

## Remarks

The read and write methods take [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) as a receiver because they are intended to be invoked from a non-generic context.

## Members

### Methods

 | Name | Description |
| --- | --- |
| [GetValue(Object)](#getvalueobject) | Reads the current property value. | 
| [SetValue(Object, T)](#setvalueobject-t) | Writes a new property value. | 

## Details

### Methods

#### GetValue(Object)

Reads the current property value.

```cs
T GetValue(System.Object source);
```

##### Parameters

**`source`** &nbsp; [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object)  
An instance of the property's [DeclaringType](imemberdescriptor.md#declaringtype).

##### Returns

`T`

  The current property value.

-----

#### SetValue(Object, T)

Writes a new property value.

```cs
void SetValue(System.Object target, T value);
```

##### Parameters

**`target`** &nbsp; [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object)  
An instance of the property's [DeclaringType](imemberdescriptor.md#declaringtype).

**`value`** &nbsp; T  
The new property value.

-----

