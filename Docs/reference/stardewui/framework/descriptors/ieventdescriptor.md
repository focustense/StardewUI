---
title: IEventDescriptor
description: Describes a single event on some type.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Interface IEventDescriptor

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Descriptors](index.md)  
Assembly: StardewUI.dll  

</div>

Describes a single event on some type.

```cs
public interface IEventDescriptor : 
    StardewUI.Framework.Descriptors.IMemberDescriptor
```

**Implements**  
[IMemberDescriptor](imemberdescriptor.md)

## Members

### Properties

 | Name | Description |
| --- | --- |
| [ArgsTypeDescriptor](#argstypedescriptor) | Descriptor for the type of event object (arguments), generally a subtype of [EventArgs](https://learn.microsoft.com/en-us/dotnet/api/system.eventargs). | 
| [DelegateParameterCount](#delegateparametercount) | Number of parameters that the `Invoke` method of the [DelegateType](ieventdescriptor.md#delegatetype) accepts. | 
| [DelegateType](#delegatetype) | The type (subtype of [Delegate](https://learn.microsoft.com/en-us/dotnet/api/system.delegate)) that can be added/removed from the event handlers. | 

### Methods

 | Name | Description |
| --- | --- |
| [Add(Object, Delegate)](#addobject-delegate) | Adds an event handler. | 
| [Remove(Object, Delegate)](#removeobject-delegate) | Removes an event handler. | 

## Details

### Properties

#### ArgsTypeDescriptor

Descriptor for the type of event object (arguments), generally a subtype of [EventArgs](https://learn.microsoft.com/en-us/dotnet/api/system.eventargs).

```cs
StardewUI.Framework.Descriptors.IObjectDescriptor ArgsTypeDescriptor { get; }
```

##### Property Value

[IObjectDescriptor](iobjectdescriptor.md)

-----

#### DelegateParameterCount

Number of parameters that the `Invoke` method of the [DelegateType](ieventdescriptor.md#delegatetype) accepts.

```cs
int DelegateParameterCount { get; }
```

##### Property Value

[Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)

-----

#### DelegateType

The type (subtype of [Delegate](https://learn.microsoft.com/en-us/dotnet/api/system.delegate)) that can be added/removed from the event handlers.

```cs
System.Type DelegateType { get; }
```

##### Property Value

[Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)

-----

### Methods

#### Add(Object, Delegate)

Adds an event handler.

```cs
void Add(System.Object target, System.Delegate handler);
```

##### Parameters

**`target`** &nbsp; [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object)  
The instance of the [DeclaringType](imemberdescriptor.md#declaringtype) on which to subscribe to events.

**`handler`** &nbsp; [Delegate](https://learn.microsoft.com/en-us/dotnet/api/system.delegate)  
The handler to run when the event is raised; must be assignable to the [DelegateType](ieventdescriptor.md#delegatetype).

-----

#### Remove(Object, Delegate)

Removes an event handler.

```cs
void Remove(System.Object target, System.Delegate handler);
```

##### Parameters

**`target`** &nbsp; [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object)  
The instance of the [DeclaringType](imemberdescriptor.md#declaringtype) on which to unsubscribe from events.

**`handler`** &nbsp; [Delegate](https://learn.microsoft.com/en-us/dotnet/api/system.delegate)  
The handler that was previously registered, i.e. via [Add(Object, Delegate)](ieventdescriptor.md#addobject-delegate).

-----

