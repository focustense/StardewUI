---
title: ReflectionPropertyDescriptor&lt;T, TValue&gt;
description: Binding property based on reflection.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class ReflectionPropertyDescriptor&lt;T, TValue&gt;

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Descriptors](index.md)  
Assembly: StardewUI.dll  

</div>

Binding property based on reflection.

```cs
public class ReflectionPropertyDescriptor<T, TValue> : 
    StardewUI.Framework.Descriptors.IPropertyDescriptor<TValue>, 
    StardewUI.Framework.Descriptors.IPropertyDescriptor, 
    StardewUI.Framework.Descriptors.IMemberDescriptor
```

### Type Parameters

**`T`**  
The type on which the property is declared.

**`TValue`**  
The property's value type.


**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ ReflectionPropertyDescriptor&lt;T, TValue&gt;

**Implements**  
[IPropertyDescriptor&lt;TValue&gt;](ipropertydescriptor-1.md), [IPropertyDescriptor](ipropertydescriptor.md), [IMemberDescriptor](imemberdescriptor.md)

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [ReflectionPropertyDescriptor&lt;T, TValue&gt;(PropertyInfo)](#reflectionpropertydescriptort-tvaluepropertyinfo) | Initializes a new [ReflectionPropertyDescriptor](reflectionpropertydescriptor.md) from reflected property info. | 

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
| [Equals(Object)](#equalsobject) | <span class="muted" markdown>(Overrides [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object).`Equals(Object)`)</span> | 
| [GetHashCode()](#gethashcode) | <span class="muted" markdown>(Overrides [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object).`GetHashCode()`)</span> | 
| [GetValue(Object)](#getvalueobject) | Reads the current property value. | 
| [SetValue(Object, TValue)](#setvalueobject-tvalue) | Writes a new property value. | 

## Details

### Constructors

#### ReflectionPropertyDescriptor&lt;T, TValue&gt;(PropertyInfo)

Initializes a new [ReflectionPropertyDescriptor](reflectionpropertydescriptor.md) from reflected property info.

```cs
public ReflectionPropertyDescriptor<T, TValue>(System.Reflection.PropertyInfo propertyInfo);
```

##### Parameters

**`propertyInfo`** &nbsp; [PropertyInfo](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.propertyinfo)  
The reflected property info.

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

#### Equals(Object)



```cs
public override bool Equals(System.Object obj);
```

##### Parameters

**`obj`** &nbsp; [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object)

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

-----

#### GetHashCode()



```cs
public override int GetHashCode();
```

##### Returns

[Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)

-----

#### GetValue(Object)

Reads the current property value.

```cs
public TValue GetValue(System.Object source);
```

##### Parameters

**`source`** &nbsp; [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object)  
An instance of the property's [DeclaringType](imemberdescriptor.md#declaringtype).

##### Returns

`TValue`

-----

#### SetValue(Object, TValue)

Writes a new property value.

```cs
public void SetValue(System.Object target, TValue value);
```

##### Parameters

**`target`** &nbsp; [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object)  
An instance of the property's [DeclaringType](imemberdescriptor.md#declaringtype).

**`value`** &nbsp; TValue  
The new property value.

-----

