---
title: ArgumentExpressionType
description: Defines the possible types of an Argument, which specifies how to resolve its value at runtime.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Enum ArgumentExpressionType

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Grammar](index.md)  
Assembly: StardewUI.dll  

</div>

Defines the possible types of an [Argument](argument.md), which specifies how to resolve its value at runtime.

```cs
public enum ArgumentExpressionType
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ [ValueType](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype) ⇦ [Enum](https://learn.microsoft.com/en-us/dotnet/api/system.enum) ⇦ ArgumentExpressionType

## Fields

 | Name | Value | Description |
| --- | --- | --- |
| <a id="literal">Literal</a> | 0 | The value is the literal string in the markup, i.e. it is the actual string representation of the target data type such as an integer, enumeration or another string. | 
| <a id="contextbinding">ContextBinding</a> | 1 | The current value of some property in the context data. | 
| <a id="eventbinding">EventBinding</a> | 2 | The value of a named property of the [EventArgs](https://learn.microsoft.com/en-us/dotnet/api/system.eventargs) subclass of an associated event, when an argument is being provided to an event handler. | 
| <a id="templatebinding">TemplateBinding</a> | 3 | The expanded value of a template parameter; only valid within a template node. | 

