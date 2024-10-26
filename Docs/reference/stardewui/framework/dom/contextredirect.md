---
title: ContextRedirect
description: Describes how to redirect the target context of any IAttribute whose IAttribute.ValueType is one of the AttributeValueTypeExtensions.IsContextBinding matching types.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class ContextRedirect

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Dom](index.md)  
Assembly: StardewUI.dll  

</div>

Describes how to redirect the target context of any [IAttribute](iattribute.md) whose [ValueType](iattribute.md#valuetype) is one of the [IsContextBinding(AttributeValueType)](../grammar/attributevaluetypeextensions.md#iscontextbindingattributevaluetype) matching types.

```cs
public record ContextRedirect : 
    IEquatable<StardewUI.Framework.Dom.ContextRedirect>
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ ContextRedirect

**Implements**  
[IEquatable](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1)<[ContextRedirect](contextredirect.md)>

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [ContextRedirect()](#contextredirect) |  | 

### Properties

 | Name | Description |
| --- | --- |
| [EqualityContract](#equalitycontract) |  | 

### Methods

 | Name | Description |
| --- | --- |
| [FromParts(UInt32, ReadOnlySpan&lt;Char&gt;)](#frompartsuint-readonlyspanchar) | Creates an optional [ContextRedirect](contextredirect.md) using the constituent parts parsed from a grammar element such as an [Attribute](../grammar/attribute.md). | 

## Details

### Constructors

#### ContextRedirect()



```cs
protected ContextRedirect();
```

-----

### Properties

#### EqualityContract



```cs
protected System.Type EqualityContract { get; }
```

##### Property Value

[Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)

-----

### Methods

#### FromParts(uint, ReadOnlySpan&lt;Char&gt;)

Creates an optional [ContextRedirect](contextredirect.md) using the constituent parts parsed from a grammar element such as an [Attribute](../grammar/attribute.md).

```cs
public static StardewUI.Framework.Dom.ContextRedirect FromParts(uint parentDepth, ReadOnlySpan<System.Char> parentType);
```

##### Parameters

**`parentDepth`** &nbsp; [UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32)  
Number of parents to traverse.

**`parentType`** &nbsp; [ReadOnlySpan](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<[Char](https://learn.microsoft.com/en-us/dotnet/api/system.char)>  
The [Name](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.memberinfo.name) of the target ancestor's [Type](https://learn.microsoft.com/en-us/dotnet/api/system.type).

##### Returns

[ContextRedirect](contextredirect.md)

  A new [ContextRedirect](contextredirect.md) that performs the requested redirect, or `null` if the arguments would cause no redirection to occur.

-----

