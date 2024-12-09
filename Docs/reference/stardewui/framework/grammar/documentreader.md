---
title: DocumentReader
description: Reads elements and associated attributes from a StarML document (content string).
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Struct DocumentReader

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Grammar](index.md)  
Assembly: StardewUI.dll  

</div>

Reads elements and associated attributes from a StarML document (content string).

```cs
[System.Obsolete]
public ref struct DocumentReader
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ [ValueType](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype) ⇦ DocumentReader

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [DocumentReader(Lexer)](#documentreaderlexer) | Reads elements and associated attributes from a StarML document (content string). | 
| [DocumentReader(ReadOnlySpan&lt;Char&gt;)](#documentreaderreadonlyspanchar) | Initializes a new [DocumentReader](documentreader.md) from the specified text, creating an implicit lexer. | 

### Properties

 | Name | Description |
| --- | --- |
| [Argument](#argument) | The argument that was just read, if the previous [NextArgument()](documentreader.md#nextargument) returned `true`; otherwise, an empty argument. | 
| [Attribute](#attribute) | The attribute that was just read, if the previous [NextMember()](documentreader.md#nextmember) returned [Attribute](tagmember.md#attribute); otherwise, an empty attribute. | 
| [Eof](#eof) | Whether the end of the document has been reached. | 
| [Event](#event) | The event that was just read, if the previous [NextMember()](documentreader.md#nextmember) returned [Event](tagmember.md#event); otherwise, an empty event. | 
| [Position](#position) | The current position in the document content. | 
| [Tag](#tag) | The tag that was just read, if the previous [NextTag()](documentreader.md#nexttag) returned `true`; otherwise, an empty tag. | 

### Methods

 | Name | Description |
| --- | --- |
| [Equals(Object)](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype.equals) | <span class="muted" markdown>(Inherited from [ValueType](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype))</span> | 
| [GetHashCode()](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype.gethashcode) | <span class="muted" markdown>(Inherited from [ValueType](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype))</span> | 
| [NextArgument()](#nextargument) | Reads the next [Argument](documentreader.md#argument), if the current scope is within an argument list. | 
| [NextMember()](#nextmember) | Reads the next [Attribute](documentreader.md#attribute). Only valid in a tag scope, i.e. after a call to [NextTag()](documentreader.md#nexttag) returns `true`. | 
| [NextTag()](#nexttag) | Reads the next [Tag](documentreader.md#tag), discarding any remaining attributes for the current tag. | 
| [ToString()](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype.tostring) | <span class="muted" markdown>(Inherited from [ValueType](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype))</span> | 

## Details

### Constructors

#### DocumentReader(Lexer)

Reads elements and associated attributes from a StarML document (content string).

```cs
public DocumentReader(StardewUI.Framework.Grammar.Lexer lexer);
```

##### Parameters

**`lexer`** &nbsp; [Lexer](lexer.md)  
The lexer that reads syntax tokens from the document.

-----

#### DocumentReader(ReadOnlySpan&lt;Char&gt;)

Initializes a new [DocumentReader](documentreader.md) from the specified text, creating an implicit lexer.

```cs
public DocumentReader(ReadOnlySpan<System.Char> text);
```

##### Parameters

**`text`** &nbsp; [ReadOnlySpan](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<[Char](https://learn.microsoft.com/en-us/dotnet/api/system.char)>  
The markup text.

-----

### Properties

#### Argument

The argument that was just read, if the previous [NextArgument()](documentreader.md#nextargument) returned `true`; otherwise, an empty argument.

```cs
public StardewUI.Framework.Grammar.Argument Argument { get; private set; }
```

##### Property Value

[Argument](argument.md)

-----

#### Attribute

The attribute that was just read, if the previous [NextMember()](documentreader.md#nextmember) returned [Attribute](tagmember.md#attribute); otherwise, an empty attribute.

```cs
public StardewUI.Framework.Grammar.Attribute Attribute { get; private set; }
```

##### Property Value

[Attribute](attribute.md)

-----

#### Eof

Whether the end of the document has been reached.

```cs
public bool Eof { get; }
```

##### Property Value

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

-----

#### Event

The event that was just read, if the previous [NextMember()](documentreader.md#nextmember) returned [Event](tagmember.md#event); otherwise, an empty event.

```cs
public StardewUI.Framework.Grammar.Event Event { get; private set; }
```

##### Property Value

[Event](event.md)

-----

#### Position

The current position in the document content.

```cs
public int Position { get; }
```

##### Property Value

[Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)

-----

#### Tag

The tag that was just read, if the previous [NextTag()](documentreader.md#nexttag) returned `true`; otherwise, an empty tag.

```cs
public StardewUI.Framework.Grammar.TagInfo Tag { get; private set; }
```

##### Property Value

[TagInfo](taginfo.md)

##### Remarks

The tag remains valid as attributes are read; i.e. `ReadNextAttribute(AttributeType, ReadOnlySpan<Char>, Boolean)` will never change this value.

-----

### Methods

#### NextArgument()

Reads the next [Argument](documentreader.md#argument), if the current scope is within an argument list.

```cs
public bool NextArgument();
```

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  `true` if an argument was read; `false` if there are no more arguments in the list or if the current position was not within an argument list.

-----

#### NextMember()

Reads the next [Attribute](documentreader.md#attribute). Only valid in a tag scope, i.e. after a call to [NextTag()](documentreader.md#nexttag) returns `true`.

```cs
public StardewUI.Framework.Grammar.TagMember NextMember();
```

##### Returns

[TagMember](tagmember.md)

  `true` if an attribute was read; `false` if there are no more attributes to read for the current element.

-----

#### NextTag()

Reads the next [Tag](documentreader.md#tag), discarding any remaining attributes for the current tag.

```cs
public bool NextTag();
```

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  `true` if an attribute was read; `false` if the end of the document was reached.

-----

