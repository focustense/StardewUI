---
title: IAttribute
description: Attribute of a StarML element.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Interface IAttribute

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Dom](index.md)  
Assembly: StardewUI.dll  

</div>

Attribute of a StarML element.

```cs
public interface IAttribute
```

## Members

### Properties

 | Name | Description |
| --- | --- |
| [ContextRedirect](#contextredirect) | Specifies the redirect to use for a context binding, if applicable and if the [ValueType](iattribute.md#valuetype) is one of the context binding types. | 
| [Name](#name) | The attribute name. | 
| [Type](#type) | The type of the attribute itself, defining how the [Name](iattribute.md#name) should be interpreted. | 
| [Value](#value) | The literal value text. | 
| [ValueType](#valuetype) | The type of the value expression, defining how the [Value](iattribute.md#value) should be interpreted. | 

### Methods

 | Name | Description |
| --- | --- |
| [Print(StringBuilder)](#printstringbuilder) | Prints the textual representation of this node. | 

## Details

### Properties

#### ContextRedirect

Specifies the redirect to use for a context binding, if applicable and if the [ValueType](iattribute.md#valuetype) is one of the context binding types.

```cs
StardewUI.Framework.Dom.ContextRedirect ContextRedirect { get; }
```

##### Property Value

[ContextRedirect](contextredirect.md)

-----

#### Name

The attribute name.

```cs
string Name { get; }
```

##### Property Value

[string](https://learn.microsoft.com/en-us/dotnet/api/system.string)

-----

#### Type

The type of the attribute itself, defining how the [Name](iattribute.md#name) should be interpreted.

```cs
StardewUI.Framework.Grammar.AttributeType Type { get; }
```

##### Property Value

[AttributeType](../grammar/attributetype.md)

-----

#### Value

The literal value text.

```cs
string Value { get; }
```

##### Property Value

[string](https://learn.microsoft.com/en-us/dotnet/api/system.string)

-----

#### ValueType

The type of the value expression, defining how the [Value](iattribute.md#value) should be interpreted.

```cs
StardewUI.Framework.Grammar.AttributeValueType ValueType { get; }
```

##### Property Value

[AttributeValueType](../grammar/attributevaluetype.md)

-----

### Methods

#### Print(StringBuilder)

Prints the textual representation of this node.

```cs
void Print(System.Text.StringBuilder sb);
```

##### Parameters

**`sb`** &nbsp; [StringBuilder](https://learn.microsoft.com/en-us/dotnet/api/system.text.stringbuilder)  
Builder to receive the attribute's text output.

-----

