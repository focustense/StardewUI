---
title: BindingException
description: The exception that is thrown when an unrecoverable error happens during data binding for a view.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class BindingException

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Binding](index.md)  
Assembly: StardewUI.dll  

</div>

The exception that is thrown when an unrecoverable error happens during data binding for a view.

```cs
public class BindingException : System.Exception
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ [Exception](https://learn.microsoft.com/en-us/dotnet/api/system.exception) ⇦ BindingException

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [BindingException()](#bindingexception) | Initializes a new instance of the [BindingException](bindingexception.md) class. | 
| [BindingException(string)](#bindingexceptionstring) | Initializes a new instance of the [BindingException](bindingexception.md) class with a specified error message. | 
| [BindingException(string, SNode)](#bindingexceptionstring-snode) | Initializes a new instance of the [BindingException](bindingexception.md) class with a specified error message and a reference to the failed node. | 
| [BindingException(string, Exception)](#bindingexceptionstring-exception) | Initializes a new instance of the [BindingException](bindingexception.md) class with a specified error message and a reference to the inner exception that is the cause of this exception. | 
| [BindingException(string, SNode, Exception)](#bindingexceptionstring-snode-exception) | Initializes a new instance of the [BindingException](bindingexception.md) class with a specified error message and references to the failed node and inner exception that are the cause of this exception. | 

### Properties

 | Name | Description |
| --- | --- |
| [Data](https://learn.microsoft.com/en-us/dotnet/api/system.exception.data) | <span class="muted" markdown>(Inherited from [Exception](https://learn.microsoft.com/en-us/dotnet/api/system.exception))</span> | 
| [HelpLink](https://learn.microsoft.com/en-us/dotnet/api/system.exception.helplink) | <span class="muted" markdown>(Inherited from [Exception](https://learn.microsoft.com/en-us/dotnet/api/system.exception))</span> | 
| [HResult](https://learn.microsoft.com/en-us/dotnet/api/system.exception.hresult) | <span class="muted" markdown>(Inherited from [Exception](https://learn.microsoft.com/en-us/dotnet/api/system.exception))</span> | 
| [InnerException](https://learn.microsoft.com/en-us/dotnet/api/system.exception.innerexception) | <span class="muted" markdown>(Inherited from [Exception](https://learn.microsoft.com/en-us/dotnet/api/system.exception))</span> | 
| [Message](https://learn.microsoft.com/en-us/dotnet/api/system.exception.message) | <span class="muted" markdown>(Inherited from [Exception](https://learn.microsoft.com/en-us/dotnet/api/system.exception))</span> | 
| [Node](#node) | The specific node that failed to bind, if known. | 
| [Source](https://learn.microsoft.com/en-us/dotnet/api/system.exception.source) | <span class="muted" markdown>(Inherited from [Exception](https://learn.microsoft.com/en-us/dotnet/api/system.exception))</span> | 
| [StackTrace](https://learn.microsoft.com/en-us/dotnet/api/system.exception.stacktrace) | <span class="muted" markdown>(Inherited from [Exception](https://learn.microsoft.com/en-us/dotnet/api/system.exception))</span> | 
| [TargetSite](https://learn.microsoft.com/en-us/dotnet/api/system.exception.targetsite) | <span class="muted" markdown>(Inherited from [Exception](https://learn.microsoft.com/en-us/dotnet/api/system.exception))</span> | 

### Methods

 | Name | Description |
| --- | --- |
| [GetBaseException()](https://learn.microsoft.com/en-us/dotnet/api/system.exception.getbaseexception) | <span class="muted" markdown>(Inherited from [Exception](https://learn.microsoft.com/en-us/dotnet/api/system.exception))</span> | 
| [GetObjectData(SerializationInfo, StreamingContext)](https://learn.microsoft.com/en-us/dotnet/api/system.exception.getobjectdata) | <span class="muted" markdown>(Inherited from [Exception](https://learn.microsoft.com/en-us/dotnet/api/system.exception))</span> | 
| [GetType()](https://learn.microsoft.com/en-us/dotnet/api/system.exception.gettype) | <span class="muted" markdown>(Inherited from [Exception](https://learn.microsoft.com/en-us/dotnet/api/system.exception))</span> | 
| [ToString()](https://learn.microsoft.com/en-us/dotnet/api/system.exception.tostring) | <span class="muted" markdown>(Inherited from [Exception](https://learn.microsoft.com/en-us/dotnet/api/system.exception))</span> | 

### Events

 | Name | Description |
| --- | --- |
| [SerializeObjectState](https://learn.microsoft.com/en-us/dotnet/api/system.exception.serializeobjectstate) | <span class="muted" markdown>(Inherited from [Exception](https://learn.microsoft.com/en-us/dotnet/api/system.exception))</span> | 

## Details

### Constructors

#### BindingException()

Initializes a new instance of the [BindingException](bindingexception.md) class.

```cs
public BindingException();
```

-----

#### BindingException(string)

Initializes a new instance of the [BindingException](bindingexception.md) class with a specified error message.

```cs
public BindingException(string message);
```

##### Parameters

**`message`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The message that describes the error.

-----

#### BindingException(string, SNode)

Initializes a new instance of the [BindingException](bindingexception.md) class with a specified error message and a reference to the failed node.

```cs
public BindingException(string message, StardewUI.Framework.Dom.SNode node);
```

##### Parameters

**`message`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The message that describes the error.

**`node`** &nbsp; [SNode](../dom/snode.md)  
The specific node that failed to bind.

-----

#### BindingException(string, Exception)

Initializes a new instance of the [BindingException](bindingexception.md) class with a specified error message and a reference to the inner exception that is the cause of this exception.

```cs
public BindingException(string message, System.Exception innerException);
```

##### Parameters

**`message`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The error message that explains the reason for the exception.

**`innerException`** &nbsp; [Exception](https://learn.microsoft.com/en-us/dotnet/api/system.exception)  
The exception that is the cause of the current exception, or `null` if not specified.

-----

#### BindingException(string, SNode, Exception)

Initializes a new instance of the [BindingException](bindingexception.md) class with a specified error message and references to the failed node and inner exception that are the cause of this exception.

```cs
public BindingException(string message, StardewUI.Framework.Dom.SNode node, System.Exception innerException);
```

##### Parameters

**`message`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The error message that explains the reason for the exception.

**`node`** &nbsp; [SNode](../dom/snode.md)  
The specific node that failed to bind.

**`innerException`** &nbsp; [Exception](https://learn.microsoft.com/en-us/dotnet/api/system.exception)  
The exception that is the cause of the current exception, or `null` if not specified.

-----

### Properties

#### Node

The specific node that failed to bind, if known.

```cs
public StardewUI.Framework.Dom.SNode Node { get; }
```

##### Property Value

[SNode](../dom/snode.md)

-----

