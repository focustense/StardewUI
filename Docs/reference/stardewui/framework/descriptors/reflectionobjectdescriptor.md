---
title: ReflectionObjectDescriptor
description: Object descriptor based on reflection.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class ReflectionObjectDescriptor

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Descriptors](index.md)  
Assembly: StardewUI.dll  

</div>

Object descriptor based on reflection.

```cs
public class ReflectionObjectDescriptor : 
    StardewUI.Framework.Descriptors.IObjectDescriptor
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ ReflectionObjectDescriptor

**Implements**  
[IObjectDescriptor](iobjectdescriptor.md)

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [ReflectionObjectDescriptor(Type, IReadOnlyList&lt;Type&gt;, IReadOnlyDictionary&lt;string, Lazy&lt;IMemberDescriptor&gt;&gt;)](#reflectionobjectdescriptortype-ireadonlylisttype-ireadonlydictionarystring-lazyimemberdescriptor) | Initializes a new [ReflectionObjectDescriptor](reflectionobjectdescriptor.md) with the given target type and members. | 

### Properties

 | Name | Description |
| --- | --- |
| [SupportsChangeNotifications](#supportschangenotifications) | Whether or not objects of this type can notify about data changes; that is, if the type implements [INotifyPropertyChanged](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged). | 
| [TargetType](#targettype) | The type being described, which owns or inherits each of the available members. | 

### Methods

 | Name | Description |
| --- | --- |
| [TryGetEvent(string, IEventDescriptor)](#trygeteventstring-ieventdescriptor) | Attempts to retrieve a named event on the [TargetType](iobjectdescriptor.md#targettype). | 
| [TryGetMethod(string, IMethodDescriptor)](#trygetmethodstring-imethoddescriptor) | Attempts to retrieve a named method of the [TargetType](iobjectdescriptor.md#targettype). | 
| [TryGetProperty(string, IPropertyDescriptor)](#trygetpropertystring-ipropertydescriptor) | Attempts to retrieve a named property of the [TargetType](iobjectdescriptor.md#targettype). | 

## Details

### Constructors

#### ReflectionObjectDescriptor(Type, IReadOnlyList&lt;Type&gt;, IReadOnlyDictionary&lt;string, Lazy&lt;IMemberDescriptor&gt;&gt;)

Initializes a new [ReflectionObjectDescriptor](reflectionobjectdescriptor.md) with the given target type and members.

```cs
protected ReflectionObjectDescriptor(System.Type type, System.Collections.Generic.IReadOnlyList<System.Type> interfaces, System.Collections.Generic.IReadOnlyDictionary<string, Lazy<StardewUI.Framework.Descriptors.IMemberDescriptor>> membersByName);
```

##### Parameters

**`type`** &nbsp; [Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)  
The [TargetType](reflectionobjectdescriptor.md#targettype).

**`interfaces`** &nbsp; [IReadOnlyList](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlylist-1)<[Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)>  
All interfaces implemented by the [TargetType](reflectionobjectdescriptor.md#targettype).

**`membersByName`** &nbsp; [IReadOnlyDictionary](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2)<[string](https://learn.microsoft.com/en-us/dotnet/api/system.string), [Lazy](https://learn.microsoft.com/en-us/dotnet/api/system.lazy-1)<[IMemberDescriptor](imemberdescriptor.md)>>  
Dictionary of member names to the corresponding member descriptors.

-----

### Properties

#### SupportsChangeNotifications

Whether or not objects of this type can notify about data changes; that is, if the type implements [INotifyPropertyChanged](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged).

```cs
public bool SupportsChangeNotifications { get; }
```

##### Property Value

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

-----

#### TargetType

The type being described, which owns or inherits each of the available members.

```cs
public System.Type TargetType { get; }
```

##### Property Value

[Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)

-----

### Methods

#### TryGetEvent(string, IEventDescriptor)

Attempts to retrieve a named event on the [TargetType](iobjectdescriptor.md#targettype).

```cs
public bool TryGetEvent(string name, out StardewUI.Framework.Descriptors.IEventDescriptor event);
```

##### Parameters

**`name`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The event name.

**`event`** &nbsp; [IEventDescriptor](ieventdescriptor.md)  
When this method returns, holds a reference to the [IEventDescriptor](ieventdescriptor.md) whose [Name](imemberdescriptor.md#name) is `name`, or `null` if no event was found with the given name.

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  `true` if the named event was found, otherwise `false`.

-----

#### TryGetMethod(string, IMethodDescriptor)

Attempts to retrieve a named method of the [TargetType](iobjectdescriptor.md#targettype).

```cs
public bool TryGetMethod(string name, out StardewUI.Framework.Descriptors.IMethodDescriptor method);
```

##### Parameters

**`name`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The method name.

**`method`** &nbsp; [IMethodDescriptor](imethoddescriptor.md)  
When this method returns, holds a reference to the [IMethodDescriptor](imethoddescriptor.md) whose [Name](imemberdescriptor.md#name) is `name`, or `null` if no method was found with the given name.

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  `true` if the named method was found, otherwise `false`.

##### Remarks

Overloaded methods are not supported. If different signatures are required, use optional parameters.

-----

#### TryGetProperty(string, IPropertyDescriptor)

Attempts to retrieve a named property of the [TargetType](iobjectdescriptor.md#targettype).

```cs
public bool TryGetProperty(string name, out StardewUI.Framework.Descriptors.IPropertyDescriptor property);
```

##### Parameters

**`name`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The property name.

**`property`** &nbsp; [IPropertyDescriptor](ipropertydescriptor.md)  
When this method returns, holds a reference to the [IPropertyDescriptor](ipropertydescriptor.md) whose [Name](imemberdescriptor.md#name) is `name`, or `null` if no property was found with the given name.

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  `true` if the named property was found, otherwise `false`.

-----

