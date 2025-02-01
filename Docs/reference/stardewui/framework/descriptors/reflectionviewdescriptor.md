---
title: ReflectionViewDescriptor
description: View descriptor based on reflection.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class ReflectionViewDescriptor

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Descriptors](index.md)  
Assembly: StardewUI.dll  

</div>

View descriptor based on reflection.

```cs
public class ReflectionViewDescriptor : 
    StardewUI.Framework.Descriptors.IViewDescriptor, 
    StardewUI.Framework.Descriptors.IObjectDescriptor
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ ReflectionViewDescriptor

**Implements**  
[IViewDescriptor](iviewdescriptor.md), [IObjectDescriptor](iobjectdescriptor.md)

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
| [TryGetChildrenProperty(string, IPropertyDescriptor)](#trygetchildrenpropertystring-ipropertydescriptor) | Attempts to retrieve the property of the [TargetType](iobjectdescriptor.md#targettype) that holds the view's children/content. | 
| [TryGetEvent(string, IEventDescriptor)](#trygeteventstring-ieventdescriptor) | Attempts to retrieve a named event on the [TargetType](iobjectdescriptor.md#targettype). | 
| [TryGetMethod(string, IMethodDescriptor)](#trygetmethodstring-imethoddescriptor) | Attempts to retrieve a named method of the [TargetType](iobjectdescriptor.md#targettype). | 
| [TryGetProperty(string, IPropertyDescriptor)](#trygetpropertystring-ipropertydescriptor) | Attempts to retrieve a named property of the [TargetType](iobjectdescriptor.md#targettype). | 

## Details

### Properties

#### MemberNames

Enumerates the names of all members of the object type.

```cs
public System.Collections.Generic.IEnumerable<string> MemberNames { get; }
```

##### Property Value

[IEnumerable](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)<[string](https://learn.microsoft.com/en-us/dotnet/api/system.string)>

-----

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

#### TryGetChildrenProperty(string, IPropertyDescriptor)

Attempts to retrieve the property of the [TargetType](iobjectdescriptor.md#targettype) that holds the view's children/content.

```cs
public bool TryGetChildrenProperty(string outletName, out StardewUI.Framework.Descriptors.IPropertyDescriptor property);
```

##### Parameters

**`outletName`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The name of the specific outlet, if targeting a non-default outlet on a view with multiple outlets. Corresponds to [Name](../../widgets/outletattribute.md#name).

**`property`** &nbsp; [IPropertyDescriptor](ipropertydescriptor.md)  
When this method returns, holds a reference to the [IPropertyDescriptor](ipropertydescriptor.md) that holds the view's children/content, or `null` if no such property is available.

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  `true` if a children/content property was found, otherwise `false`.

-----

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

