---
title: LengthType
description: Types of length calculation available for a Length.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Enum LengthType

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Layout](index.md)  
Assembly: StardewUI.dll  

</div>

Types of length calculation available for a [Length](length.md).

```cs
public enum LengthType
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ [ValueType](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype) ⇦ [Enum](https://learn.microsoft.com/en-us/dotnet/api/system.enum) ⇦ LengthType

## Remarks

For all types, content may overflow or be clipped if the available size is not large enough.

## Fields

 | Name | Value | Description |
| --- | --- | --- |
| <a id="content">Content</a> | 0 | Ignore the specified [Value](length.md#value) and use a value just high enough to fit all content. | 
| <a id="px">Px</a> | 1 | Use the exact [Value](length.md#value) specified, in pixels. | 
| <a id="percent">Percent</a> | 2 | Use the specified [Value](length.md#value) as a percentage of the available width/height. | 
| <a id="stretch">Stretch</a> | 3 | Ignore the specified [Value](length.md#value) and stretch to the full available width/height. | 

