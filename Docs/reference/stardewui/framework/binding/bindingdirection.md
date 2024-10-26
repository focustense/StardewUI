---
title: BindingDirection
description: The direction of data flow in a data binding.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Enum BindingDirection

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Binding](index.md)  
Assembly: StardewUI.dll  

</div>

The direction of data flow in a data binding.

```cs
public enum BindingDirection
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ [ValueType](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype) ⇦ [Enum](https://learn.microsoft.com/en-us/dotnet/api/system.enum) ⇦ BindingDirection

## Fields

 | Name | Value | Description |
| --- | --- | --- |
| <a id="in">In</a> | 0 | An input binding, i.e. the view receives its value from the context. | 
| <a id="out">Out</a> | 1 | An output binding, i.e. the view publishes its value to the context. | 
| <a id="inout">InOut</a> | 2 | A binding that is both input and output, i.e. the view receives its value from the context and also publishes its value to the context, depending on where the most recent change occurred. | 

