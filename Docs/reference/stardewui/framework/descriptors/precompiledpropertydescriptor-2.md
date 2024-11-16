---
title: PrecompiledPropertyDescriptor&lt;TTarget, TProperty&gt;
description: Statically-typed implementation of an IPropertyDescriptor&lt;T&gt; with predefined attributes.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class PrecompiledPropertyDescriptor&lt;TTarget, TProperty&gt;

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Descriptors](index.md)  
Assembly: StardewUI.dll  

</div>

Statically-typed implementation of an [IPropertyDescriptor&lt;T&gt;](ipropertydescriptor-1.md) with predefined attributes.

```cs
public class PrecompiledPropertyDescriptor<TTarget, TProperty> : 
    StardewUI.Framework.Descriptors.IPropertyDescriptor<TProperty>, 
    StardewUI.Framework.Descriptors.IPropertyDescriptor, 
    StardewUI.Framework.Descriptors.IMemberDescriptor
```

### Type Parameters

**`TTarget`**  
The property's declaring type.

**`TProperty`**  
The property value type.


**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ PrecompiledPropertyDescriptor&lt;TTarget, TProperty&gt;

**Implements**  
[IPropertyDescriptor&lt;TProperty&gt;](ipropertydescriptor-1.md), [IPropertyDescriptor](ipropertydescriptor.md), [IMemberDescriptor](imemberdescriptor.md)

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [PrecompiledPropertyDescriptor&lt;TTarget, TProperty&gt;(string, Boolean, Boolean, Func&lt;TTarget, TProperty&gt;, Action&lt;TTarget, TProperty&gt;)](#precompiledpropertydescriptorttarget-tpropertystring-bool-bool-functtarget-tproperty-actionttarget-tproperty) | Statically-typed implementation of an [IPropertyDescriptor&lt;T&gt;](ipropertydescriptor-1.md) with predefined attributes. | 

### Properties

 | Name | Description |
| --- | --- |
| [CanRead](#canread) | Whether or not the property is readable, i.e. has a public getter. | 
| [CanWrite](#canwrite) | Whether or not the property is writable, i.e. has a public setter. | 
| [DeclaringType](#declaringtype) | The type on which the member is declared. | 
| [IsAutoProperty](#isautoproperty) | Whether or not the property is likely auto-implemented. | 
| [IsField](#isfield) | Whether or not the underlying member is a field, rather than a real property. | 
| [Name](#name) | The member name. | 
| [ValueType](#valuetype) | The property's value type. | 

### Methods

 | Name | Description |
| --- | --- |
| [GetValue(Object)](#getvalueobject) | Reads the current property value. | 
| [SetValue(Object, TProperty)](#setvalueobject-tproperty) | Writes a new property value. | 

## Details

### Constructors

#### PrecompiledPropertyDescriptor&lt;TTarget, TProperty&gt;(string, bool, bool, Func&lt;TTarget, TProperty&gt;, Action&lt;TTarget, TProperty&gt;)

Statically-typed implementation of an [IPropertyDescriptor&lt;T&gt;](ipropertydescriptor-1.md) with predefined attributes.

```cs
public PrecompiledPropertyDescriptor<TTarget, TProperty>(string name, bool isField, bool isAutoProperty, Func<TTarget, TProperty> getter, Action<TTarget, TProperty> setter);
```

##### Parameters

**`name`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The event name.

**`isField`** &nbsp; [Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)  
Whether or not this descriptor is really for an unwrapped field.

**`isAutoProperty`** &nbsp; [Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)  
Whether or not the property is auto-implemented.

**`getter`** &nbsp; [Func&lt;TTarget, TProperty&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2)  
Function to retrieve the current property value from a target instance.

**`setter`** &nbsp; [Action&lt;TTarget, TProperty&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-2)  
Function to set the current property value on a target instance.

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

#### IsAutoProperty

Whether or not the property is likely auto-implemented.

```cs
public bool IsAutoProperty { get; }
```

##### Property Value

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

##### Remarks

Auto-property detection is heuristic, relying on the method's IL instructions and the name of its backing field. This can often be interpreted as a signal that the property won't receive property-change notifications, since to do so (whether explicitly or via some weaver/source generator) requires an implementation that is different from the auto-generated getter and setter. 

 Caveats: This only works as a negative signal (a value of `false` does not prove that the property _will_ receive notifications, even if the declaring type implements [INotifyPropertyChanged](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged)), and is somewhat fuzzy even as a negative signal; it is theoretically possible for a source generator or IL weaver to leave behind all the markers of an auto property and still emit notifications, although no known libraries actually do so.

-----

#### IsField

Whether or not the underlying member is a field, rather than a real property.

```cs
public bool IsField { get; }
```

##### Property Value

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

##### Remarks

For binding convenience, fields and properties are both called "properties" for descriptors, as the external access pattern is the same; however, mutable fields can never reliably emit property-change notifications regardless of whether the declaring type implements [INotifyPropertyChanged](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged), so this is usually used to emit some warning.

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
public TProperty GetValue(System.Object source);
```

##### Parameters

**`source`** &nbsp; [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object)  
An instance of the property's [DeclaringType](imemberdescriptor.md#declaringtype).

##### Returns

`TProperty`

-----

#### SetValue(Object, TProperty)

Writes a new property value.

```cs
public void SetValue(System.Object target, TProperty value);
```

##### Parameters

**`target`** &nbsp; [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object)  
An instance of the property's [DeclaringType](imemberdescriptor.md#declaringtype).

**`value`** &nbsp; TProperty  
The new property value.

-----

