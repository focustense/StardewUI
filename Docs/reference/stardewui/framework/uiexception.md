---
title: UIException
description: Base class for all exceptions specific to StardewUI.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class UIException

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework](index.md)  
Assembly: StardewUI.dll  

</div>

Base class for all exceptions specific to StardewUI.

```cs
public class UIException : System.Exception
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ [Exception](https://learn.microsoft.com/en-us/dotnet/api/system.exception) ⇦ UIException

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [UIException()](#uiexception) | Initializes a new instance of the [UIException](uiexception.md) class. | 
| [UIException(string)](#uiexceptionstring) | Initializes a new instance of the [UIException](uiexception.md) class with a specified error message. | 
| [UIException(string, Exception)](#uiexceptionstring-exception) | Initializes a new instance of the [UIException](uiexception.md) class with a specified error message and a reference to the inner exception that is the cause of this exception. | 

### Properties

 | Name | Description |
| --- | --- |
| [Data](https://learn.microsoft.com/en-us/dotnet/api/system.exception.data) | <span class="muted" markdown>(Inherited from [Exception](https://learn.microsoft.com/en-us/dotnet/api/system.exception))</span> | 
| [HelpLink](https://learn.microsoft.com/en-us/dotnet/api/system.exception.helplink) | <span class="muted" markdown>(Inherited from [Exception](https://learn.microsoft.com/en-us/dotnet/api/system.exception))</span> | 
| [HResult](https://learn.microsoft.com/en-us/dotnet/api/system.exception.hresult) | <span class="muted" markdown>(Inherited from [Exception](https://learn.microsoft.com/en-us/dotnet/api/system.exception))</span> | 
| [InnerException](https://learn.microsoft.com/en-us/dotnet/api/system.exception.innerexception) | <span class="muted" markdown>(Inherited from [Exception](https://learn.microsoft.com/en-us/dotnet/api/system.exception))</span> | 
| [Message](https://learn.microsoft.com/en-us/dotnet/api/system.exception.message) | <span class="muted" markdown>(Inherited from [Exception](https://learn.microsoft.com/en-us/dotnet/api/system.exception))</span> | 
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

#### UIException()

Initializes a new instance of the [UIException](uiexception.md) class.

```cs
public UIException();
```

-----

#### UIException(string)

Initializes a new instance of the [UIException](uiexception.md) class with a specified error message.

```cs
public UIException(string message);
```

##### Parameters

**`message`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The message that describes the error.

-----

#### UIException(string, Exception)

Initializes a new instance of the [UIException](uiexception.md) class with a specified error message and a reference to the inner exception that is the cause of this exception.

```cs
public UIException(string message, System.Exception innerException);
```

##### Parameters

**`message`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The error message that explains the reason for the exception.

**`innerException`** &nbsp; [Exception](https://learn.microsoft.com/en-us/dotnet/api/system.exception)  
The exception that is the cause of the current exception, or `null` if not specified.

-----

