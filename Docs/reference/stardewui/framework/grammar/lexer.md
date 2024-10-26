---
title: Lexer
description: Consumes raw StarML content as a token stream.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Struct Lexer

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Grammar](index.md)  
Assembly: StardewUI.dll  

</div>

Consumes raw StarML content as a token stream.

```cs
[System.Obsolete]
public ref struct Lexer
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ [ValueType](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype) ⇦ Lexer

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [Lexer(ReadOnlySpan&lt;Char&gt;)](#lexerreadonlyspanchar) | Consumes raw StarML content as a token stream. | 

### Properties

 | Name | Description |
| --- | --- |
| [Current](#current) | The most recent token that was read, if the previous call to [MoveNext()](lexer.md#movenext) was successful; otherwise, an empty token. | 
| [Eof](#eof) | Whether the lexer is at the end of the content, either at the very end or with only trailing whitespace. | 
| [Position](#position) | The current position in the markup text, i.e. the position at the _end_ the [Current](lexer.md#current) token. | 

### Methods

 | Name | Description |
| --- | --- |
| [Equals(Object)](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype.equals) | <span class="muted" markdown>(Inherited from [ValueType](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype))</span> | 
| [GetEnumerator()](#getenumerator) | Returns a reference to this [Lexer](lexer.md). | 
| [GetHashCode()](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype.gethashcode) | <span class="muted" markdown>(Inherited from [ValueType](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype))</span> | 
| [MoveNext()](#movenext) | Reads the next token into [Current](lexer.md#current) and advances the [Position](lexer.md#position). | 
| [ReadOptionalToken(TokenType)](#readoptionaltokentokentype) | Attempts to read the next token and, if successful, validates that it has a specific type. | 
| [ReadRequiredToken(TokenType)](#readrequiredtokentokentype) | Reads the next token and validates that it has a specific type. | 
| [ToString()](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype.tostring) | <span class="muted" markdown>(Inherited from [ValueType](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype))</span> | 

## Details

### Constructors

#### Lexer(ReadOnlySpan&lt;Char&gt;)

Consumes raw StarML content as a token stream.

```cs
public Lexer(ReadOnlySpan<System.Char> text);
```

##### Parameters

**`text`** &nbsp; [ReadOnlySpan](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<[Char](https://learn.microsoft.com/en-us/dotnet/api/system.char)>  
The markup text.

-----

### Properties

#### Current

The most recent token that was read, if the previous call to [MoveNext()](lexer.md#movenext) was successful; otherwise, an empty token.

```cs
public StardewUI.Framework.Grammar.Token Current { get; private set; }
```

##### Property Value

[Token](token.md)

-----

#### Eof

Whether the lexer is at the end of the content, either at the very end or with only trailing whitespace.

```cs
public bool Eof { get; }
```

##### Property Value

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

-----

#### Position

The current position in the markup text, i.e. the position at the _end_ the [Current](lexer.md#current) token.

```cs
public int Position { get; }
```

##### Property Value

[Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)

-----

### Methods

#### GetEnumerator()

Returns a reference to this [Lexer](lexer.md).

```cs
public StardewUI.Framework.Grammar.Lexer GetEnumerator();
```

##### Returns

[Lexer](lexer.md)

##### Remarks

Implementing this, along with [Current](lexer.md#current) and [MoveNext()](lexer.md#movenext), allows it to be used in a `foreach` loop without having to implement [IEnumerable&lt;T&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1), which is not allowed on a `ref struct`.

-----

#### MoveNext()

Reads the next token into [Current](lexer.md#current) and advances the [Position](lexer.md#position).

```cs
public bool MoveNext();
```

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  `true` if a token was read; `false` if the end of the content was reached.

-----

#### ReadOptionalToken(TokenType)

Attempts to read the next token and, if successful, validates that it has a specific type.

```cs
public bool ReadOptionalToken(StardewUI.Framework.Grammar.TokenType expectedTypes);
```

##### Parameters

**`expectedTypes`** &nbsp; [TokenType](tokentype.md)  
The [TokenType](tokentype.md)s allowed for the next token.

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  `true` if a token was read and was one of the `expectedTypes`; `false` if the end of the content was reached.

-----

#### ReadRequiredToken(TokenType)

Reads the next token and validates that it has a specific type.

```cs
public void ReadRequiredToken(StardewUI.Framework.Grammar.TokenType expectedTypes);
```

##### Parameters

**`expectedTypes`** &nbsp; [TokenType](tokentype.md)  
The [TokenType](tokentype.md)s allowed for the next token.

-----

