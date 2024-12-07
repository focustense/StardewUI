---
title: IArgument
description: An argument to a method call, e.g. as used in an IEvent.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Interface IArgument

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Dom](index.md)  
Assembly: StardewUI.dll  

</div>

An argument to a method call, e.g. as used in an [IEvent](ievent.md).

```cs
public interface IArgument
```

## Members

### Properties

 | Name | Description |
| --- | --- |
| [ContextRedirect](#contextredirect) | Specifies the redirect to use for a context binding, if one was specified and if the [Type](iargument.md#type) is [ContextBinding](../grammar/argumentexpressiontype.md#contextbinding). | 
| [Expression](#expression) | The argument value or binding path, not including punctuation such as quotes or prefixes. | 
| [Type](#type) | The type of argument, indicating how it is to be evaluated in any method calls. | 

### Methods

 | Name | Description |
| --- | --- |
| [Print(StringBuilder)](#printstringbuilder) | Prints the textual representation of this argument. | 

## Details

### Properties

#### ContextRedirect

Specifies the redirect to use for a context binding, if one was specified and if the [Type](iargument.md#type) is [ContextBinding](../grammar/argumentexpressiontype.md#contextbinding).

```cs
StardewUI.Framework.Dom.ContextRedirect ContextRedirect { get; }
```

##### Property Value

[ContextRedirect](contextredirect.md)

-----

#### Expression

The argument value or binding path, not including punctuation such as quotes or prefixes.

```cs
string Expression { get; }
```

##### Property Value

[string](https://learn.microsoft.com/en-us/dotnet/api/system.string)

-----

#### Type

The type of argument, indicating how it is to be evaluated in any method calls.

```cs
StardewUI.Framework.Grammar.ArgumentExpressionType Type { get; }
```

##### Property Value

[ArgumentExpressionType](../grammar/argumentexpressiontype.md)

-----

### Methods

#### Print(StringBuilder)

Prints the textual representation of this argument.

```cs
void Print(System.Text.StringBuilder sb);
```

##### Parameters

**`sb`** &nbsp; [StringBuilder](https://learn.microsoft.com/en-us/dotnet/api/system.text.stringbuilder)  
Builder to receive the argument's text output.

-----

