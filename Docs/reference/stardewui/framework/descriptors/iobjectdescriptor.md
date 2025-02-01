---
title: IObjectDescriptor
description: Describes a type of object that participates in view binding, either as the target or the source.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Interface IObjectDescriptor

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Descriptors](index.md)  
Assembly: StardewUI.dll  

</div>

Describes a type of object that participates in view binding, either as the target or the source.

```cs
public interface IObjectDescriptor
```

## Remarks

The binding target is independent of the actual object instance; it provides methods and data to support interacting with any object of the given [TargetType](iobjectdescriptor.md#targettype).

## Members

### Properties

 | Name | Description |
| --- | --- |
| [MemberNames](#membernames) | Enumerates the names of all members of the object type. | 
| [SupportsChangeNotifications](#supportschangenotifications) | Whether or not objects of this type can notify about data changes; that is, if the type implements [INotifyPropertyChanged](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged). | 
| [TargetType](#targettype) | The type being described, which owns or inherits each of the available members. | 

### Methods

 | Name | Description |
| --- | --- |
| [GetEvent(string)](#geteventstring) | Retrieves a named event on the [TargetType](iobjectdescriptor.md#targettype). | 
| [GetMethod(string)](#getmethodstring) | Retrieves a named method of the [TargetType](iobjectdescriptor.md#targettype). | 
| [GetProperty(string)](#getpropertystring) | Retrieves a named property of the [TargetType](iobjectdescriptor.md#targettype). | 
| [TryGetEvent(string, IEventDescriptor)](#trygeteventstring-ieventdescriptor) | Attempts to retrieve a named event on the [TargetType](iobjectdescriptor.md#targettype). | 
| [TryGetMethod(string, IMethodDescriptor)](#trygetmethodstring-imethoddescriptor) | Attempts to retrieve a named method of the [TargetType](iobjectdescriptor.md#targettype). | 
| [TryGetProperty(string, IPropertyDescriptor)](#trygetpropertystring-ipropertydescriptor) | Attempts to retrieve a named property of the [TargetType](iobjectdescriptor.md#targettype). | 

## Details

### Properties

#### MemberNames

Enumerates the names of all members of the object type.

```cs
System.Collections.Generic.IEnumerable<string> MemberNames { get; }
```

##### Property Value

[IEnumerable](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)<[string](https://learn.microsoft.com/en-us/dotnet/api/system.string)>

-----

#### SupportsChangeNotifications

Whether or not objects of this type can notify about data changes; that is, if the type implements [INotifyPropertyChanged](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged).

```cs
bool SupportsChangeNotifications { get; }
```

##### Property Value

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

-----

#### TargetType

The type being described, which owns or inherits each of the available members.

```cs
System.Type TargetType { get; }
```

##### Property Value

[Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)

-----

### Methods

#### GetEvent(string)

Retrieves a named event on the [TargetType](iobjectdescriptor.md#targettype).

```cs
StardewUI.Framework.Descriptors.IEventDescriptor GetEvent(string name);
```

##### Parameters

**`name`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The event name.

##### Returns

[IEventDescriptor](ieventdescriptor.md)

  The [IEventDescriptor](ieventdescriptor.md) whose [Name](imemberdescriptor.md#name) is `name`.

-----

#### GetMethod(string)

Retrieves a named method of the [TargetType](iobjectdescriptor.md#targettype).

```cs
StardewUI.Framework.Descriptors.IMethodDescriptor GetMethod(string name);
```

##### Parameters

**`name`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The method name.

##### Returns

[IMethodDescriptor](imethoddescriptor.md)

  The [IMethodDescriptor](imethoddescriptor.md) whose [Name](imemberdescriptor.md#name) is `name`.

##### Remarks

Overloaded methods are not supported. If different signatures are required, use optional parameters.

-----

#### GetProperty(string)

Retrieves a named property of the [TargetType](iobjectdescriptor.md#targettype).

```cs
StardewUI.Framework.Descriptors.IPropertyDescriptor GetProperty(string name);
```

##### Parameters

**`name`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The property name.

##### Returns

[IPropertyDescriptor](ipropertydescriptor.md)

  The [IPropertyDescriptor](ipropertydescriptor.md) whose [Name](imemberdescriptor.md#name) is `name`.

-----

#### TryGetEvent(string, IEventDescriptor)

Attempts to retrieve a named event on the [TargetType](iobjectdescriptor.md#targettype).

```cs
bool TryGetEvent(string name, out StardewUI.Framework.Descriptors.IEventDescriptor event);
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
bool TryGetMethod(string name, out StardewUI.Framework.Descriptors.IMethodDescriptor method);
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
bool TryGetProperty(string name, out StardewUI.Framework.Descriptors.IPropertyDescriptor property);
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

