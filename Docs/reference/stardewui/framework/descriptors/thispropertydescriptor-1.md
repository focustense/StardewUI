---
title: ThisPropertyDescriptor&lt;T&gt;
description: Special descriptor used for "this" references in argument/attribute bindings, allowing them to reference the current context instead of a property on it.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class ThisPropertyDescriptor&lt;T&gt;

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Descriptors](index.md)  
Assembly: StardewUI.dll  

</div>

Special descriptor used for "this" references in argument/attribute bindings, allowing them to reference the current context instead of a property on it.

```cs
public class ThisPropertyDescriptor<T> : 
    StardewUI.Framework.Descriptors.IPropertyDescriptor<T>, 
    StardewUI.Framework.Descriptors.IPropertyDescriptor, 
    StardewUI.Framework.Descriptors.IMemberDescriptor
```

### Type Parameters

**`T`**  
The object type.


**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ ThisPropertyDescriptor&lt;T&gt;

**Implements**  
[IPropertyDescriptor&lt;T&gt;](ipropertydescriptor-1.md), [IPropertyDescriptor](ipropertydescriptor.md), [IMemberDescriptor](imemberdescriptor.md)

## Members

### Fields

 | Name | Description |
| --- | --- |
| [Instance](#instance) | Gets the singleton descriptor instance for the current object/property type. | 

### Properties

 | Name | Description |
| --- | --- |
| [CanRead](#canread) | Whether or not the property is readable, i.e. has a public getter. | 
| [CanWrite](#canwrite) | Whether or not the property is writable, i.e. has a public setter. | 
| [DeclaringType](#declaringtype) | The type on which the member is declared. | 
| [Name](#name) | The member name. | 
| [ValueType](#valuetype) | The property's value type. | 

### Methods

 | Name | Description |
| --- | --- |
| [GetValue(Object)](#getvalueobject) | Reads the current property value. | 
| [SetValue(Object, T)](#setvalueobject-t) | Writes a new property value. | 

## Details

### Fields

#### Instance

Gets the singleton descriptor instance for the current object/property type.

```cs
public static readonly StardewUI.Framework.Descriptors.ThisPropertyDescriptor`1 Instance;
```

##### Field Value

[ThisPropertyDescriptor&lt;T&gt;](thispropertydescriptor-1.md)

-----

### Properties

#### CanRead

Whether or not the property is readable, i.e. has a public getter.

```cs
public bool CanRead { get; }
```

##### Property Value

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

-----

#### CanWrite

Whether or not the property is writable, i.e. has a public setter.

```cs
public bool CanWrite { get; }
```

##### Property Value

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

-----

#### DeclaringType

The type on which the member is declared.

```cs
public System.Type DeclaringType { get; }
```

##### Property Value

[Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)

-----

#### Name

The member name.

```cs
public string Name { get; }
```

##### Property Value

[string](https://learn.microsoft.com/en-us/dotnet/api/system.string)

-----

#### ValueType

The property's value type.

```cs
public System.Type ValueType { get; }
```

##### Property Value

[Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)

-----

### Methods

#### GetValue(Object)

Reads the current property value.

```cs
public T GetValue(System.Object source);
```

##### Parameters

**`source`** &nbsp; [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object)  
An instance of the property's [DeclaringType](imemberdescriptor.md#declaringtype).

##### Returns

`T`

-----

#### SetValue(Object, T)

Writes a new property value.

```cs
public void SetValue(System.Object target, T value);
```

##### Parameters

**`target`** &nbsp; [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object)  
An instance of the property's [DeclaringType](imemberdescriptor.md#declaringtype).

**`value`** &nbsp; T  
The new property value.

-----

