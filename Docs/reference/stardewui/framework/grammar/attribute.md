---
title: Attribute
description: A complete attribute assignment parsed from StarML.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Struct Attribute

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Grammar](index.md)  
Assembly: StardewUI.dll  

</div>

A complete attribute assignment parsed from StarML.

```cs
[System.Obsolete]
public readonly ref struct Attribute
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ [ValueType](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype) ⇦ Attribute

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [Attribute(ReadOnlySpan&lt;Char&gt;, AttributeType, Boolean, AttributeValueType, ReadOnlySpan&lt;Char&gt;, UInt32, ReadOnlySpan&lt;Char&gt;)](#attributereadonlyspanchar-attributetype-bool-attributevaluetype-readonlyspanchar-uint-readonlyspanchar) | A complete attribute assignment parsed from StarML. | 

### Properties

 | Name | Description |
| --- | --- |
| [IsNegated](#isnegated) | Whether the attribute has a negation (`!`) operator before assignment. | 
| [Name](#name) | The attribute name. | 
| [ParentDepth](#parentdepth) | The depth to walk - i.e. number of parents to traverse - to find the context on which to evaluate a context binding. Exclusive with [ParentType](attribute.md#parenttype) and only valid if the [ValueType](attribute.md#valuetype) is a type that matches [IsContextBinding(AttributeValueType)](attributevaluetypeextensions.md#iscontextbindingattributevaluetype). | 
| [ParentType](#parenttype) | The type name of the parent to walk up to for a context redirect. Exclusive with [ParentDepth](attribute.md#parentdepth) and only valid if the [ValueType](attribute.md#valuetype) is a type that matches [IsContextBinding(AttributeValueType)](attributevaluetypeextensions.md#iscontextbindingattributevaluetype). | 
| [Type](#type) | The type of the attribute itself, i.e. how its [Name](attribute.md#name) should be interpreted. | 
| [Value](#value) | The literal value text. | 
| [ValueType](#valuetype) | The type of the value expression, defining how the [Value](attribute.md#value) should be interpreted. | 

### Methods

 | Name | Description |
| --- | --- |
| [Equals(Object)](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype.equals) | <span class="muted" markdown>(Inherited from [ValueType](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype))</span> | 
| [GetHashCode()](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype.gethashcode) | <span class="muted" markdown>(Inherited from [ValueType](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype))</span> | 
| [ToString()](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype.tostring) | <span class="muted" markdown>(Inherited from [ValueType](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype))</span> | 

## Details

### Constructors

#### Attribute(ReadOnlySpan&lt;Char&gt;, AttributeType, bool, AttributeValueType, ReadOnlySpan&lt;Char&gt;, uint, ReadOnlySpan&lt;Char&gt;)

A complete attribute assignment parsed from StarML.

```cs
public Attribute(ReadOnlySpan<System.Char> name, StardewUI.Framework.Grammar.AttributeType type, bool isNegated, StardewUI.Framework.Grammar.AttributeValueType valueType, ReadOnlySpan<System.Char> value, uint parentDepth, ReadOnlySpan<System.Char> parentType);
```

##### Parameters

**`name`** &nbsp; [ReadOnlySpan](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<[Char](https://learn.microsoft.com/en-us/dotnet/api/system.char)>  
The attribute name.

**`type`** &nbsp; [AttributeType](attributetype.md)  
The type of the attribute itself, i.e. how its `name` should be interpreted.

**`isNegated`** &nbsp; [Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)  
Whether the attribute has a negation (`!`) operator before assignment.

**`valueType`** &nbsp; [AttributeValueType](attributevaluetype.md)  
The type of the value expression, defining how the `value` should be interpreted.

**`value`** &nbsp; [ReadOnlySpan](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<[Char](https://learn.microsoft.com/en-us/dotnet/api/system.char)>  
The literal value text.

**`parentDepth`** &nbsp; [UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32)  
The depth to walk - i.e. number of parents to traverse - to find the context on which to evaluate a context binding. Only valid if the `valueType` is a type that matches [IsContextBinding(AttributeValueType)](attributevaluetypeextensions.md#iscontextbindingattributevaluetype).

**`parentType`** &nbsp; [ReadOnlySpan](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<[Char](https://learn.microsoft.com/en-us/dotnet/api/system.char)>  
The type name of the parent to search for, to find the the context on which to evaluate a context binding. Exclusive with `parentDepth` and only valid if the `valueType` is a type that matches [IsContextBinding(AttributeValueType)](attributevaluetypeextensions.md#iscontextbindingattributevaluetype).

-----

### Properties

#### IsNegated

Whether the attribute has a negation (`!`) operator before assignment.

```cs
public bool IsNegated { get; }
```

##### Property Value

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

##### Remarks

Negation behavior is specific to the exact attribute and is not supported for many/most attributes.

-----

#### Name

The attribute name.

```cs
public ReadOnlySpan<System.Char> Name { get; }
```

##### Property Value

[ReadOnlySpan](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<[Char](https://learn.microsoft.com/en-us/dotnet/api/system.char)>

-----

#### ParentDepth

The depth to walk - i.e. number of parents to traverse - to find the context on which to evaluate a context binding. Exclusive with [ParentType](attribute.md#parenttype) and only valid if the [ValueType](attribute.md#valuetype) is a type that matches [IsContextBinding(AttributeValueType)](attributevaluetypeextensions.md#iscontextbindingattributevaluetype).

```cs
public uint ParentDepth { get; }
```

##### Property Value

[UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32)

-----

#### ParentType

The type name of the parent to walk up to for a context redirect. Exclusive with [ParentDepth](attribute.md#parentdepth) and only valid if the [ValueType](attribute.md#valuetype) is a type that matches [IsContextBinding(AttributeValueType)](attributevaluetypeextensions.md#iscontextbindingattributevaluetype).

```cs
public ReadOnlySpan<System.Char> ParentType { get; }
```

##### Property Value

[ReadOnlySpan](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<[Char](https://learn.microsoft.com/en-us/dotnet/api/system.char)>

-----

#### Type

The type of the attribute itself, i.e. how its [Name](attribute.md#name) should be interpreted.

```cs
public StardewUI.Framework.Grammar.AttributeType Type { get; }
```

##### Property Value

[AttributeType](attributetype.md)

-----

#### Value

The literal value text.

```cs
public ReadOnlySpan<System.Char> Value { get; }
```

##### Property Value

[ReadOnlySpan](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<[Char](https://learn.microsoft.com/en-us/dotnet/api/system.char)>

-----

#### ValueType

The type of the value expression, defining how the [Value](attribute.md#value) should be interpreted.

```cs
public StardewUI.Framework.Grammar.AttributeValueType ValueType { get; }
```

##### Property Value

[AttributeValueType](attributevaluetype.md)

-----

