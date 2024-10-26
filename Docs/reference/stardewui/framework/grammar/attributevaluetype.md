---
title: AttributeValueType
description: Types allowed for the value of an Attribute.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Enum AttributeValueType

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Grammar](index.md)  
Assembly: StardewUI.dll  

</div>

Types allowed for the value of an [Attribute](attribute.md).

```cs
public enum AttributeValueType
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ [ValueType](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype) ⇦ [Enum](https://learn.microsoft.com/en-us/dotnet/api/system.enum) ⇦ AttributeValueType

## Fields

 | Name | Value | Description |
| --- | --- | --- |
| <a id="literal">Literal</a> | 0 | The value is the literal string in the markup, i.e. it is the actual string representation of the target data type such as an integer, enumeration or another string. | 
| <a id="assetbinding">AssetBinding</a> | 1 | A read-only binding which obtains the value from a named game asset. | 
| <a id="inputbinding">InputBinding</a> | 2 | A one-way data binding which obtains the value from the context data and assigns it to the view. | 
| <a id="onetimebinding">OneTimeBinding</a> | 3 | A special type of [InputBinding](attributevaluetype.md#inputbinding) that only reads the value a single time, and does not update subsequently afterward. | 
| <a id="outputbinding">OutputBinding</a> | 4 | A one-way data binding which obtains the value from the view and assigns it to the context data. | 
| <a id="twowaybinding">TwoWayBinding</a> | 5 | A two-way data binding which both assigns the context data's value to the view, and the view's value to the context data, depending on which one was most recently changed. | 

