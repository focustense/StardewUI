---
title: PrecompiledMethodDescriptor&lt;TTarget, TReturn&gt;
description: Statically-typed implementation of an IMethodDescriptor&lt;T&gt; with predefined attributes.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class PrecompiledMethodDescriptor&lt;TTarget, TReturn&gt;

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Descriptors](index.md)  
Assembly: StardewUI.dll  

</div>

Statically-typed implementation of an [IMethodDescriptor&lt;T&gt;](imethoddescriptor-1.md) with predefined attributes.

```cs
public class PrecompiledMethodDescriptor<TTarget, TReturn> : 
    StardewUI.Framework.Descriptors.IMethodDescriptor<TReturn>, 
    StardewUI.Framework.Descriptors.IMethodDescriptor, 
    StardewUI.Framework.Descriptors.IMemberDescriptor
```

### Type Parameters

**`TTarget`**  
The method's declaring type.

**`TReturn`**  
The type of the method's return value.


**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ PrecompiledMethodDescriptor&lt;TTarget, TReturn&gt;

**Implements**  
[IMethodDescriptor&lt;TReturn&gt;](imethoddescriptor-1.md), [IMethodDescriptor](imethoddescriptor.md), [IMemberDescriptor](imemberdescriptor.md)

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [PrecompiledMethodDescriptor&lt;TTarget, TReturn&gt;(string, Type, Object, Func&lt;TTarget, Object, Object&gt;)](#precompiledmethoddescriptorttarget-treturnstring-type-object-functtarget-object-object) | Statically-typed implementation of an [IMethodDescriptor&lt;T&gt;](imethoddescriptor-1.md) with predefined attributes. | 

### Properties

 | Name | Description |
| --- | --- |
| [ArgumentTypes](#argumenttypes) | The exact types expected for the method's arguments. | 
| [DeclaringType](#declaringtype) | The type on which the member is declared. | 
| [Name](#name) | The member name. | 
| [OptionalArgumentCount](#optionalargumentcount) | The number of optional arguments at the end of the argument list. | 
| [ReturnType](#returntype) | The method's return type. | 

### Methods

 | Name | Description |
| --- | --- |
| [Invoke(Object, Object)](#invokeobject-object) | Invokes the underlying method. | 

## Details

### Constructors

#### PrecompiledMethodDescriptor&lt;TTarget, TReturn&gt;(string, Type, Object, Func&lt;TTarget, Object, Object&gt;)

Statically-typed implementation of an [IMethodDescriptor&lt;T&gt;](imethoddescriptor-1.md) with predefined attributes.

```cs
public PrecompiledMethodDescriptor<TTarget, TReturn>(string name, System.Type argumentTypes, System.Object defaultValues, Func<TTarget, System.Object, System.Object> invoke);
```

##### Parameters

**`name`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The method name.

**`argumentTypes`** &nbsp; [Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)  
Types of all method parameters, including optional parameters.

**`defaultValues`** &nbsp; [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object)  
Default values for all optional parameters at the end of the argument list.

**`invoke`** &nbsp; [Func&lt;TTarget, Object, Object&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-3)  
Function to invoke the method on a given target with a specified argument list.

-----

### Properties

#### ArgumentTypes

The exact types expected for the method's arguments.

```cs
public ReadOnlySpan<System.Type> ArgumentTypes { get; }
```

##### Property Value

[ReadOnlySpan](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<[Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)>

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

#### OptionalArgumentCount

The number of optional arguments at the end of the argument list.

```cs
public int OptionalArgumentCount { get; }
```

##### Property Value

[Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)

##### Remarks

Optional arguments can be provided with [Missing](https://learn.microsoft.com/en-us/dotnet/api/system.type.missing) in order to ignore them in the invocation.

-----

#### ReturnType

The method's return type.

```cs
public System.Type ReturnType { get; }
```

##### Property Value

[Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)

-----

### Methods

#### Invoke(Object, Object)

Invokes the underlying method.

```cs
public TReturn Invoke(System.Object target, System.Object arguments);
```

##### Parameters

**`target`** &nbsp; [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object)  
The object instance on which to invoke the method.

**`arguments`** &nbsp; [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object)  
The arguments to provide to the method.

##### Returns

`TReturn`

-----

