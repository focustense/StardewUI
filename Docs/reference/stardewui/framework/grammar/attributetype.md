---
title: AttributeType
description: The different types of an Attribute, independent of its value.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Enum AttributeType

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Grammar](index.md)  
Assembly: StardewUI.dll  

</div>

The different types of an [Attribute](attribute.md), independent of its value.

```cs
public enum AttributeType
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ [ValueType](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype) ⇦ [Enum](https://learn.microsoft.com/en-us/dotnet/api/system.enum) ⇦ AttributeType

## Fields

 | Name | Value | Description |
| --- | --- | --- |
| <a id="property">Property</a> | 0 | Sets or binds a property on the target view. | 
| <a id="structural">Structural</a> | 1 | Affects the structure or hierarchy of the view tree, e.g. by making a node conditional or repeated. | 
| <a id="behavior">Behavior</a> | 2 | Invokes a standard or custom [IViewBehavior](../behaviors/iviewbehavior.md) that attaches to the view. | 

