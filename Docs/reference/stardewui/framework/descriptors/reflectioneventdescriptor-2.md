---
title: ReflectionEventDescriptor&lt;TTarget, THandler&gt;
description: Reflection-based implementation of an event descriptor.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class ReflectionEventDescriptor&lt;TTarget, THandler&gt;

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Descriptors](index.md)  
Assembly: StardewUI.dll  

</div>

Reflection-based implementation of an event descriptor.

```cs
public class ReflectionEventDescriptor<TTarget, THandler> : 
    StardewUI.Framework.Descriptors.IEventDescriptor, 
    StardewUI.Framework.Descriptors.IMemberDescriptor
```

### Type Parameters

**`TTarget`**  
The type that declares the event.

**`THandler`**  
The event handler (delegate) type.


**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ ReflectionEventDescriptor&lt;TTarget, THandler&gt;

**Implements**  
[IEventDescriptor](ieventdescriptor.md), [IMemberDescriptor](imemberdescriptor.md)

## Members

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
| [Equals(Object)](#equalsobject) | <span class="muted" markdown>(Overrides [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object).`Equals(Object)`)</span> | 
| [GetHashCode()](#gethashcode) | <span class="muted" markdown>(Overrides [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object).`GetHashCode()`)</span> | 
| [Remove(Object, Delegate)](#removeobject-delegate) | Removes an event handler. | 

## Details

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

