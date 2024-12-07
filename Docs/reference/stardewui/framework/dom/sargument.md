---
title: SArgument
description: Record implementation of an IArgument.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class SArgument

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Dom](index.md)  
Assembly: StardewUI.dll  

</div>

Record implementation of an [IArgument](iargument.md).

```cs
public record SArgument : StardewUI.Framework.Dom.IArgument, 
    IEquatable<StardewUI.Framework.Dom.SArgument>
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ SArgument

**Implements**  
[IArgument](iargument.md), [IEquatable](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1)<[SArgument](sargument.md)>

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [SArgument(ArgumentExpressionType, string, ContextRedirect)](#sargumentargumentexpressiontype-string-contextredirect) | Record implementation of an [IArgument](iargument.md). | 
| [SArgument(Argument)](#sargumentargument) | Initializes a new [SArgument](sargument.md) from the data of a parsed argument. | 

### Properties

 | Name | Description |
| --- | --- |
| [ContextRedirect](#contextredirect) | Specifies the redirect to use for a context binding, if one was specified and if the [Type](sargument.md#type) is [ContextBinding](../grammar/argumentexpressiontype.md#contextbinding). | 
| [EqualityContract](#equalitycontract) |  | 
| [Expression](#expression) | The argument value or binding path, not including punctuation such as quotes or prefixes. | 
| [Type](#type) | The type of argument, indicating how it is to be evaluated in any method calls. | 

## Details

### Constructors

#### SArgument(ArgumentExpressionType, string, ContextRedirect)

Record implementation of an [IArgument](iargument.md).

```cs
public SArgument(StardewUI.Framework.Grammar.ArgumentExpressionType Type, string Expression, StardewUI.Framework.Dom.ContextRedirect ContextRedirect);
```

##### Parameters

**`Type`** &nbsp; [ArgumentExpressionType](../grammar/argumentexpressiontype.md)  
The type of argument, indicating how it is to be evaluated in any method calls.

**`Expression`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The argument value or binding path, not including punctuation such as quotes or prefixes.

**`ContextRedirect`** &nbsp; [ContextRedirect](contextredirect.md)  
Specifies the redirect to use for a context binding, if one was specified and if the [Type](sargument.md#type) is [ContextBinding](../grammar/argumentexpressiontype.md#contextbinding).

-----

#### SArgument(Argument)

Initializes a new [SArgument](sargument.md) from the data of a parsed argument.

```cs
public SArgument(StardewUI.Framework.Grammar.Argument argument);
```

##### Parameters

**`argument`** &nbsp; [Argument](../grammar/argument.md)  
The parsed argument.

-----

### Properties

#### ContextRedirect

Specifies the redirect to use for a context binding, if one was specified and if the [Type](sargument.md#type) is [ContextBinding](../grammar/argumentexpressiontype.md#contextbinding).

```cs
public StardewUI.Framework.Dom.ContextRedirect ContextRedirect { get; set; }
```

##### Property Value

[ContextRedirect](contextredirect.md)

-----

#### EqualityContract



```cs
protected System.Type EqualityContract { get; }
```

##### Property Value

[Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)

-----

#### Expression

The argument value or binding path, not including punctuation such as quotes or prefixes.

```cs
public string Expression { get; set; }
```

##### Property Value

[string](https://learn.microsoft.com/en-us/dotnet/api/system.string)

-----

#### Type

The type of argument, indicating how it is to be evaluated in any method calls.

```cs
public StardewUI.Framework.Grammar.ArgumentExpressionType Type { get; set; }
```

##### Property Value

[ArgumentExpressionType](../grammar/argumentexpressiontype.md)

-----

