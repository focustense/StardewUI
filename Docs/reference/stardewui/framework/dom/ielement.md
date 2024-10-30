---
title: IElement
description: Element in a StarML document, including the tag and all enclosed attributes.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Interface IElement

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Dom](index.md)  
Assembly: StardewUI.dll  

</div>

Element in a StarML document, including the tag and all enclosed attributes.

```cs
public interface IElement
```

## Members

### Properties

 | Name | Description |
| --- | --- |
| [Attributes](#attributes) | The parsed list of attributes applied to this instance of the tag. | 
| [Events](#events) | The parsed list of events applied to this instance of the tag. | 
| [Tag](#tag) | The parsed tag name. | 

### Methods

 | Name | Description |
| --- | --- |
| [Print(StringBuilder, Boolean)](#printstringbuilder-bool) | Prints the textual representation of this element. | 
| [PrintClosingTag(StringBuilder)](#printclosingtagstringbuilder) | Prints the closing tag for this element. | 

## Details

### Properties

#### Attributes

The parsed list of attributes applied to this instance of the tag.

```cs
System.Collections.Generic.IReadOnlyList<StardewUI.Framework.Dom.IAttribute> Attributes { get; }
```

##### Property Value

[IReadOnlyList](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlylist-1)<[IAttribute](iattribute.md)>

-----

#### Events

The parsed list of events applied to this instance of the tag.

```cs
System.Collections.Generic.IReadOnlyList<StardewUI.Framework.Dom.IEvent> Events { get; }
```

##### Property Value

[IReadOnlyList](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlylist-1)<[IEvent](ievent.md)>

-----

#### Tag

The parsed tag name.

```cs
string Tag { get; }
```

##### Property Value

[string](https://learn.microsoft.com/en-us/dotnet/api/system.string)

-----

### Methods

#### Print(StringBuilder, bool)

Prints the textual representation of this element.

```cs
void Print(System.Text.StringBuilder sb, bool asSelfClosing);
```

##### Parameters

**`sb`** &nbsp; [StringBuilder](https://learn.microsoft.com/en-us/dotnet/api/system.text.stringbuilder)  
Builder to receive the element's text output.

**`asSelfClosing`** &nbsp; [Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)  
Whether to print the element as a self-closing tag, i.e. whether to include a `/` character before the closing `>`.

-----

#### PrintClosingTag(StringBuilder)

Prints the closing tag for this element.

```cs
void PrintClosingTag(System.Text.StringBuilder sb);
```

##### Parameters

**`sb`** &nbsp; [StringBuilder](https://learn.microsoft.com/en-us/dotnet/api/system.text.stringbuilder)  
Builder to receive the element's text output.

-----

