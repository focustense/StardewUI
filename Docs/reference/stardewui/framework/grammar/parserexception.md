---
title: ParserException
description: The exception that is thrown when a DocumentReader encounters invalid content.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class ParserException

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Grammar](index.md)  
Assembly: StardewUI.dll  

</div>

The exception that is thrown when a [DocumentReader](documentreader.md) encounters invalid content.

```cs
public class ParserException : System.Exception
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ [Exception](https://learn.microsoft.com/en-us/dotnet/api/system.exception) ⇦ ParserException

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [ParserException(string, Int32)](#parserexceptionstring-int) | The exception that is thrown when a [DocumentReader](documentreader.md) encounters invalid content. | 

### Properties

 | Name | Description |
| --- | --- |
| [Data](https://learn.microsoft.com/en-us/dotnet/api/system.exception.data) | <span class="muted" markdown>(Inherited from [Exception](https://learn.microsoft.com/en-us/dotnet/api/system.exception))</span> | 
| [HelpLink](https://learn.microsoft.com/en-us/dotnet/api/system.exception.helplink) | <span class="muted" markdown>(Inherited from [Exception](https://learn.microsoft.com/en-us/dotnet/api/system.exception))</span> | 
| [HResult](https://learn.microsoft.com/en-us/dotnet/api/system.exception.hresult) | <span class="muted" markdown>(Inherited from [Exception](https://learn.microsoft.com/en-us/dotnet/api/system.exception))</span> | 
| [InnerException](https://learn.microsoft.com/en-us/dotnet/api/system.exception.innerexception) | <span class="muted" markdown>(Inherited from [Exception](https://learn.microsoft.com/en-us/dotnet/api/system.exception))</span> | 
| [Message](https://learn.microsoft.com/en-us/dotnet/api/system.exception.message) | <span class="muted" markdown>(Inherited from [Exception](https://learn.microsoft.com/en-us/dotnet/api/system.exception))</span> | 
| [Position](#position) | The position within the markup text where the error was encountered. | 
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

#### ParserException(string, int)

The exception that is thrown when a [DocumentReader](documentreader.md) encounters invalid content.

```cs
public ParserException(string message, int position);
```

##### Parameters

**`message`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The message that describes the error.

**`position`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)  
The position within the markup text where the error was encountered.

-----

### Properties

#### Position

The position within the markup text where the error was encountered.

```cs
public int Position { get; }
```

##### Property Value

[Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)

-----

