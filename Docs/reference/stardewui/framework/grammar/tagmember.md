---
title: TagMember
description: The type of tag member read, resulting from a call to DocumentReader.NextMember.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Enum TagMember

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Grammar](index.md)  
Assembly: StardewUI.dll  

</div>

The type of tag member read, resulting from a call to [NextMember()](documentreader.md#nextmember).

```cs
public enum TagMember
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ [ValueType](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype) ⇦ [Enum](https://learn.microsoft.com/en-us/dotnet/api/system.enum) ⇦ TagMember

## Fields

 | Name | Value | Description |
| --- | --- | --- |
| <a id="none">None</a> | 0 | No member was read, i.e. the reader reached the end of the tag. | 
| <a id="attribute">Attribute</a> | 1 | A regular attribute, which binds or writes to a property of the target view. | 
| <a id="event">Event</a> | 2 | An event attribute, which attaches an event handler to the target view. | 

