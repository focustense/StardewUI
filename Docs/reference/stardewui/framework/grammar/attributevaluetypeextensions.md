---
title: AttributeValueTypeExtensions
description: Extensions for the AttributeValueType enum.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class AttributeValueTypeExtensions

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Grammar](index.md)  
Assembly: StardewUI.dll  

</div>

Extensions for the [AttributeValueType](attributevaluetype.md) enum.

```cs
public static class AttributeValueTypeExtensions
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ AttributeValueTypeExtensions

## Members

### Methods

 | Name | Description |
| --- | --- |
| [IsContextBinding(AttributeValueType)](#iscontextbindingattributevaluetype) | Tests if a given `valueType` is any type of context binding, regardless of its direction. | 

## Details

### Methods

#### IsContextBinding(AttributeValueType)

Tests if a given `valueType` is any type of context binding, regardless of its direction.

```cs
public static bool IsContextBinding(StardewUI.Framework.Grammar.AttributeValueType valueType);
```

##### Parameters

**`valueType`** &nbsp; [AttributeValueType](attributevaluetype.md)  
The value type.

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  `true` if the attribute binds to a context property; `false` if it is some other type of attribute such as [Literal](attributevaluetype.md#literal) or [AssetBinding](attributevaluetype.md#assetbinding).

-----

