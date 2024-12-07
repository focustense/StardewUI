---
title: PrecompiledEventDescriptor&lt;TTarget, THandler&gt;
description: Statically-typed implementation of an IEventDescriptor with predefined attributes.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class PrecompiledEventDescriptor&lt;TTarget, THandler&gt;

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Descriptors](index.md)  
Assembly: StardewUI.dll  

</div>

Statically-typed implementation of an [IEventDescriptor](ieventdescriptor.md) with predefined attributes.

```cs
public class PrecompiledEventDescriptor<TTarget, THandler> : 
    StardewUI.Framework.Descriptors.IEventDescriptor, 
    StardewUI.Framework.Descriptors.IMemberDescriptor
```

### Type Parameters

**`TTarget`**  
The event's declaring type.

**`THandler`**  
The delegate type of event handlers.


**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ PrecompiledEventDescriptor&lt;TTarget, THandler&gt;

**Implements**  
[IEventDescriptor](ieventdescriptor.md), [IMemberDescriptor](imemberdescriptor.md)

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [PrecompiledEventDescriptor&lt;TTarget, THandler&gt;(string, Action&lt;TTarget, THandler&gt;, Action&lt;TTarget, THandler&gt;, Type)](#precompiledeventdescriptorttarget-thandlerstring-actionttarget-thandler-actionttarget-thandler-type) | Statically-typed implementation of an [IEventDescriptor](ieventdescriptor.md) with predefined attributes. | 

### Properties

 | Name | Description |
| --- | --- |
| [ArgsTypeDescriptor](#argstypedescriptor) | Descriptor for the type of event object (arguments), generally a subtype of [EventArgs](https://learn.microsoft.com/en-us/dotnet/api/system.eventargs). | 
| [DeclaringType](#declaringtype) | The type on which the member is declared. | 
| [DelegateParameterCount](#delegateparametercount) | Number of parameters that the `Invoke` method of the [DelegateType](ieventdescriptor.md#delegatetype) accepts. | 
| [DelegateType](#delegatetype) | The type (subtype of [Delegate](https://learn.microsoft.com/en-us/dotnet/api/system.delegate)) that can be added/removed from the event handlers. | 
| [Name](#name) | The member name. | 

### Methods

 | Name | Description |
| --- | --- |
| [Add(Object, Delegate)](#addobject-delegate) | Adds an event handler. | 
| [Remove(Object, Delegate)](#removeobject-delegate) | Removes an event handler. | 

## Details

### Constructors

#### PrecompiledEventDescriptor&lt;TTarget, THandler&gt;(string, Action&lt;TTarget, THandler&gt;, Action&lt;TTarget, THandler&gt;, Type)

Statically-typed implementation of an [IEventDescriptor](ieventdescriptor.md) with predefined attributes.

```cs
public PrecompiledEventDescriptor<TTarget, THandler>(string name, Action<TTarget, THandler> add, Action<TTarget, THandler> remove, System.Type argsType);
```

##### Parameters

**`name`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The event name.

**`add`** &nbsp; [Action&lt;TTarget, THandler&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-2)  
Function to add a new event handler to an instance of the target type.

**`remove`** &nbsp; [Action&lt;TTarget, THandler&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-2)  
Function to remove an existing event handler from an instance of the target type.

**`argsType`** &nbsp; [Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)  
Type of the argument parameter in the `THandler` delegate.

-----

### Properties

#### ArgsTypeDescriptor

Descriptor for the type of event object (arguments), generally a subtype of [EventArgs](https://learn.microsoft.com/en-us/dotnet/api/system.eventargs).

```cs
public StardewUI.Framework.Descriptors.IObjectDescriptor ArgsTypeDescriptor { get; }
```

##### Property Value

[IObjectDescriptor](iobjectdescriptor.md)

-----

#### DeclaringType

The type on which the member is declared.

```cs
public System.Type DeclaringType { get; }
```

##### Property Value

[Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)

-----

#### DelegateParameterCount

Number of parameters that the `Invoke` method of the [DelegateType](ieventdescriptor.md#delegatetype) accepts.

```cs
public int DelegateParameterCount { get; }
```

##### Property Value

[Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)

##### Remarks

For precompiled descriptors, this is assumed to always be exactly 2 (sender and args).

-----

#### DelegateType

The type (subtype of [Delegate](https://learn.microsoft.com/en-us/dotnet/api/system.delegate)) that can be added/removed from the event handlers.

```cs
public System.Type DelegateType { get; }
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

### Methods

#### Add(Object, Delegate)

Adds an event handler.

```cs
public void Add(System.Object target, System.Delegate handler);
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
public void Remove(System.Object target, System.Delegate handler);
```

##### Parameters

**`target`** &nbsp; [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object)  
The instance of the [DeclaringType](imemberdescriptor.md#declaringtype) on which to unsubscribe from events.

**`handler`** &nbsp; [Delegate](https://learn.microsoft.com/en-us/dotnet/api/system.delegate)  
The handler that was previously registered, i.e. via [Add(Object, Delegate)](ieventdescriptor.md#addobject-delegate).

-----

