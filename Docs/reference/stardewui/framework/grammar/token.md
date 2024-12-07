---
title: Token
description: A token emitted by the StarML Lexer.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Struct Token

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Grammar](index.md)  
Assembly: StardewUI.dll  

</div>

A token emitted by the StarML [Lexer](lexer.md).

```cs
[System.Obsolete]
public readonly ref struct Token
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ [ValueType](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype) ⇦ Token

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [Token(TokenType, ReadOnlySpan&lt;Char&gt;)](#tokentokentype-readonlyspanchar) | A token emitted by the StarML [Lexer](lexer.md). | 

### Properties

 | Name | Description |
| --- | --- |
| [Text](#text) | The token type. | 
| [Type](#type) | The exact text of the token in the original markup. | 

### Methods

 | Name | Description |
| --- | --- |
| [Equals(Object)](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype.equals) | <span class="muted" markdown>(Inherited from [ValueType](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype))</span> | 
| [GetHashCode()](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype.gethashcode) | <span class="muted" markdown>(Inherited from [ValueType](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype))</span> | 
| [ToString()](#tostring) | <span class="muted" markdown>(Overrides [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object).`ToString()`)</span> | 

## Details

### Constructors

#### Token(TokenType, ReadOnlySpan&lt;Char&gt;)

A token emitted by the StarML [Lexer](lexer.md).

```cs
public Token(StardewUI.Framework.Grammar.TokenType type, ReadOnlySpan<System.Char> text);
```

##### Parameters

**`type`** &nbsp; [TokenType](tokentype.md)  
The token type.

**`text`** &nbsp; [ReadOnlySpan](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<[Char](https://learn.microsoft.com/en-us/dotnet/api/system.char)>  
The exact text of the token in the original markup.

-----

### Properties

#### Text

The token type.

```cs
public ReadOnlySpan<System.Char> Text { get; }
```

##### Property Value

[ReadOnlySpan](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<[Char](https://learn.microsoft.com/en-us/dotnet/api/system.char)>

-----

#### Type

The exact text of the token in the original markup.

```cs
public StardewUI.Framework.Grammar.TokenType Type { get; }
```

##### Property Value

[TokenType](tokentype.md)

-----

### Methods

#### ToString()



```cs
public override string ToString();
```

##### Returns

[string](https://learn.microsoft.com/en-us/dotnet/api/system.string)

-----

