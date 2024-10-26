---
title: SAttribute
description: Record implementation of a StarML IAttribute.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class SAttribute

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Dom](index.md)  
Assembly: StardewUI.dll  

</div>

Record implementation of a StarML [IAttribute](iattribute.md).

```cs
public record SAttribute : StardewUI.Framework.Dom.IAttribute, 
    IEquatable<StardewUI.Framework.Dom.SAttribute>
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ SAttribute

**Implements**  
[IAttribute](iattribute.md), [IEquatable](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1)<[SAttribute](sattribute.md)>

## Remarks

Must be separate from the grammar's [Attribute](../grammar/attribute.md) since `ref struct`s currently are not allowed to implement interfaces.

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [SAttribute(string, string, AttributeType, AttributeValueType, ContextRedirect)](#sattributestring-string-attributetype-attributevaluetype-contextredirect) | Record implementation of a StarML [IAttribute](iattribute.md). | 
| [SAttribute(Attribute)](#sattributeattribute) | Initializes a new [SAttribute](sattribute.md) from the data of a parsed attribute. | 

### Properties

 | Name | Description |
| --- | --- |
| [ContextRedirect](#contextredirect) | Specifies the redirect to use for a context binding, if applicable and if the `ValueType` is one of the context binding types. | 
| [EqualityContract](#equalitycontract) |  | 
| [Name](#name) | The attribute name. | 
| [Type](#type) | The type of the attribute itself, defining how the `Name` should be interpreted. | 
| [Value](#value) | The literal value text. | 
| [ValueType](#valuetype) | The type of the value expression, defining how the `Value` should be interpreted. | 

## Details

### Constructors

#### SAttribute(string, string, AttributeType, AttributeValueType, ContextRedirect)

Record implementation of a StarML [IAttribute](iattribute.md).

```cs
public SAttribute(string Name, string Value, StardewUI.Framework.Grammar.AttributeType Type, StardewUI.Framework.Grammar.AttributeValueType ValueType, StardewUI.Framework.Dom.ContextRedirect ContextRedirect);
```

##### Parameters

**`Name`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The attribute name.

**`Value`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The literal value text.

**`Type`** &nbsp; [AttributeType](../grammar/attributetype.md)  
The type of the attribute itself, defining how the `Name` should be interpreted.

**`ValueType`** &nbsp; [AttributeValueType](../grammar/attributevaluetype.md)  
The type of the value expression, defining how the `Value` should be interpreted.

**`ContextRedirect`** &nbsp; [ContextRedirect](contextredirect.md)  
Specifies the redirect to use for a context binding, if applicable and if the `ValueType` is one of the context binding types.

##### Remarks

Must be separate from the grammar's [Attribute](../grammar/attribute.md) since `ref struct`s currently are not allowed to implement interfaces.

-----

#### SAttribute(Attribute)

Initializes a new [SAttribute](sattribute.md) from the data of a parsed attribute.

```cs
public SAttribute(StardewUI.Framework.Grammar.Attribute attribute);
```

##### Parameters

**`attribute`** &nbsp; [Attribute](../grammar/attribute.md)  
The parsed attribute.

-----

### Properties

#### ContextRedirect

Specifies the redirect to use for a context binding, if applicable and if the `ValueType` is one of the context binding types.

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

#### Name

The attribute name.

```cs
public string Name { get; set; }
```

##### Property Value

[string](https://learn.microsoft.com/en-us/dotnet/api/system.string)

-----

#### Type

The type of the attribute itself, defining how the `Name` should be interpreted.

```cs
public StardewUI.Framework.Grammar.AttributeType Type { get; set; }
```

##### Property Value

[AttributeType](../grammar/attributetype.md)

-----

#### Value

The literal value text.

```cs
public string Value { get; set; }
```

##### Property Value

[string](https://learn.microsoft.com/en-us/dotnet/api/system.string)

-----

#### ValueType

The type of the value expression, defining how the `Value` should be interpreted.

```cs
public StardewUI.Framework.Grammar.AttributeValueType ValueType { get; set; }
```

##### Property Value

[AttributeValueType](../grammar/attributevaluetype.md)

-----

