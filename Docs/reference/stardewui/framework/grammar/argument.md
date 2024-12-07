---
title: Argument
description: A complete method argument parsed from StarML.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Struct Argument

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Grammar](index.md)  
Assembly: StardewUI.dll  

</div>

A complete method argument parsed from StarML.

```cs
[System.Obsolete]
public readonly ref struct Argument
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ [ValueType](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype) ⇦ Argument

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [Argument(ArgumentExpressionType, ReadOnlySpan&lt;Char&gt;, UInt32, ReadOnlySpan&lt;Char&gt;)](#argumentargumentexpressiontype-readonlyspanchar-uint-readonlyspanchar) | A complete method argument parsed from StarML. | 

### Properties

 | Name | Description |
| --- | --- |
| [Expression](#expression) | The literal expression text. | 
| [ExpressionType](#expressiontype) | The type describing how [Expression](argument.md#expression) should be interpreted. | 
| [ParentDepth](#parentdepth) | The depth to walk - i.e. number of parents to traverse - to find the context on which to evaluate the [Expression](argument.md#expression) when the [ExpressionType](argument.md#expressiontype) is [ContextBinding](argumentexpressiontype.md#contextbinding). | 
| [ParentType](#parenttype) | The type name of the parent to walk up to for a context redirect. Exclusive with [ParentDepth](argument.md#parentdepth) and only valid if the [ExpressionType](argument.md#expressiontype) is [ContextBinding](argumentexpressiontype.md#contextbinding). | 

### Methods

 | Name | Description |
| --- | --- |
| [Equals(Object)](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype.equals) | <span class="muted" markdown>(Inherited from [ValueType](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype))</span> | 
| [GetHashCode()](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype.gethashcode) | <span class="muted" markdown>(Inherited from [ValueType](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype))</span> | 
| [ToString()](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype.tostring) | <span class="muted" markdown>(Inherited from [ValueType](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype))</span> | 

## Details

### Constructors

#### Argument(ArgumentExpressionType, ReadOnlySpan&lt;Char&gt;, uint, ReadOnlySpan&lt;Char&gt;)

A complete method argument parsed from StarML.

```cs
public Argument(StardewUI.Framework.Grammar.ArgumentExpressionType expressionType, ReadOnlySpan<System.Char> expression, uint parentDepth, ReadOnlySpan<System.Char> parentType);
```

##### Parameters

**`expressionType`** &nbsp; [ArgumentExpressionType](argumentexpressiontype.md)  
The type describing how `expression` should be interpreted.

**`expression`** &nbsp; [ReadOnlySpan](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<[Char](https://learn.microsoft.com/en-us/dotnet/api/system.char)>  
The literal expression text.

**`parentDepth`** &nbsp; [UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32)  
The depth to walk - i.e. number of parents to traverse - to find the context on which to evaluate the `expression` when the `expressionType` is [ContextBinding](argumentexpressiontype.md#contextbinding).

**`parentType`** &nbsp; [ReadOnlySpan](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<[Char](https://learn.microsoft.com/en-us/dotnet/api/system.char)>  
The type name of the parent to walk up to for a context redirect. Exclusive with `parentDepth` and only valid if the `expressionType` is [ContextBinding](argumentexpressiontype.md#contextbinding).

-----

### Properties

#### Expression

The literal expression text.

```cs
public ReadOnlySpan<System.Char> Expression { get; }
```

##### Property Value

[ReadOnlySpan](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<[Char](https://learn.microsoft.com/en-us/dotnet/api/system.char)>

-----

#### ExpressionType

The type describing how [Expression](argument.md#expression) should be interpreted.

```cs
public StardewUI.Framework.Grammar.ArgumentExpressionType ExpressionType { get; }
```

##### Property Value

[ArgumentExpressionType](argumentexpressiontype.md)

-----

#### ParentDepth

The depth to walk - i.e. number of parents to traverse - to find the context on which to evaluate the [Expression](argument.md#expression) when the [ExpressionType](argument.md#expressiontype) is [ContextBinding](argumentexpressiontype.md#contextbinding).

```cs
public uint ParentDepth { get; }
```

##### Property Value

[UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32)

-----

#### ParentType

The type name of the parent to walk up to for a context redirect. Exclusive with [ParentDepth](argument.md#parentdepth) and only valid if the [ExpressionType](argument.md#expressiontype) is [ContextBinding](argumentexpressiontype.md#contextbinding).

```cs
public ReadOnlySpan<System.Char> ParentType { get; }
```

##### Property Value

[ReadOnlySpan](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<[Char](https://learn.microsoft.com/en-us/dotnet/api/system.char)>

-----

