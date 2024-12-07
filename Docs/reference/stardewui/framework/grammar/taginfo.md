---
title: TagInfo
description: Information about a parsed tag in StarML.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Struct TagInfo

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Grammar](index.md)  
Assembly: StardewUI.dll  

</div>

Information about a parsed tag in StarML.

```cs
[System.Obsolete]
public readonly ref struct TagInfo
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ [ValueType](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype) ⇦ TagInfo

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [TagInfo(ReadOnlySpan&lt;Char&gt;, Boolean)](#taginforeadonlyspanchar-bool) | Information about a parsed tag in StarML. | 

### Properties

 | Name | Description |
| --- | --- |
| [IsClosingTag](#isclosingtag) | Whether or not the tag is a closing tag, either in regular `</tag>` form or the end of a self-closing tag (`/>`) after the tag attributes. | 
| [Name](#name) | The tag name. | 

### Methods

 | Name | Description |
| --- | --- |
| [Equals(Object)](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype.equals) | <span class="muted" markdown>(Inherited from [ValueType](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype))</span> | 
| [GetHashCode()](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype.gethashcode) | <span class="muted" markdown>(Inherited from [ValueType](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype))</span> | 
| [ToString()](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype.tostring) | <span class="muted" markdown>(Inherited from [ValueType](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype))</span> | 

## Details

### Constructors

#### TagInfo(ReadOnlySpan&lt;Char&gt;, bool)

Information about a parsed tag in StarML.

```cs
public TagInfo(ReadOnlySpan<System.Char> name, bool isClosingTag);
```

##### Parameters

**`name`** &nbsp; [ReadOnlySpan](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<[Char](https://learn.microsoft.com/en-us/dotnet/api/system.char)>  
The tag name.

**`isClosingTag`** &nbsp; [Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)  
Whether or not the tag is a closing tag, either in regular `</tag>` form or the end of a self-closing tag (`/>`) after the tag attributes.

-----

### Properties

#### IsClosingTag

Whether or not the tag is a closing tag, either in regular `</tag>` form or the end of a self-closing tag (`/>`) after the tag attributes.

```cs
public bool IsClosingTag { get; }
```

##### Property Value

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

-----

#### Name

The tag name.

```cs
public ReadOnlySpan<System.Char> Name { get; }
```

##### Property Value

[ReadOnlySpan](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<[Char](https://learn.microsoft.com/en-us/dotnet/api/system.char)>

-----

