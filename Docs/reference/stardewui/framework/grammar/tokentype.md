---
title: TokenType
description: Types of tokens allowed in StarML.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Enum TokenType

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Grammar](index.md)  
Assembly: StardewUI.dll  

</div>

Types of tokens allowed in StarML.

```cs
public enum TokenType
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ [ValueType](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype) ⇦ [Enum](https://learn.microsoft.com/en-us/dotnet/api/system.enum) ⇦ TokenType

## Fields

 | Name | Value | Description |
| --- | --- | --- |
| <a id="unknown">Unknown</a> | 0 | Unknown token; used when a lexer has not been initialized, or has reached the end of its content. | 
| <a id="openingtagstart">OpeningTagStart</a> | 1 | Start of an opening tag, i.e. the `<` character without a subsequent `/`. | 
| <a id="closingtagstart">ClosingTagStart</a> | 2 | Start of a closing tag, i.e. the `</` character sequence. | 
| <a id="tagend">TagEnd</a> | 3 | End of a regular opening or closing tag, i.e. the `>` character. | 
| <a id="selfclosingtagend">SelfClosingTagEnd</a> | 4 | End of a self-closing tag, i.e. the `/>` character sequence. | 
| <a id="name">Name</a> | 5 | A valid name, i.e. of an element (tag) or attribute. | 
| <a id="nameseparator">NameSeparator</a> | 6 | The dot (`.`) character used to separate components of a name. | 
| <a id="attributemodifier">AttributeModifier</a> | 7 | Modifier token designating the type of an attribute; the `*` character (structural). | 
| <a id="literal">Literal</a> | 8 | A string of literal text, as found within a quoted or bound attribute. | 
| <a id="assignment">Assignment</a> | 9 | The `=` character, as used in an attribute syntax such as `attr="value"`. | 
| <a id="quote">Quote</a> | 10 | Double quote character (`"`) used to start or terminate a [Literal](tokentype.md#literal) string. | 
| <a id="bindingstart">BindingStart</a> | 11 | A pair of opening braces (`{{`), used to start a binding expression for an attribute value. | 
| <a id="bindingend">BindingEnd</a> | 12 | A pair of closing braces (`}}`), used to end a binding expression for an attribute value. | 
| <a id="bindingmodifier">BindingModifier</a> | 13 | An explicit binding modifier; one of `@` (asset), `#` (translation), `&` (template), `<` (input only), `>` (output only) or `<>` (two-way). | 
| <a id="contextparent">ContextParent</a> | 14 | A caret (`^`) used in a binding expression, indicating a walk up to the parent context. | 
| <a id="contextancestor">ContextAncestor</a> | 15 | A tilde (`~`) used in a binding expression, indicating traversal up to a parent with a named type. | 
| <a id="pipe">Pipe</a> | 16 | The pipe (`|`) character, which is used to start and end event bindings. | 
| <a id="argumentliststart">ArgumentListStart</a> | 17 | The left parenthesis (`(`) character, used to start an argument list. | 
| <a id="argumentlistend">ArgumentListEnd</a> | 18 | The right parenthesis (`(`) character, used to end an argument list. | 
| <a id="argumentprefix">ArgumentPrefix</a> | 19 | Prefix character for an argument, e.g. `$` to refer to an event property. | 
| <a id="argumentseparator">ArgumentSeparator</a> | 20 | The comma (`,`) character, used to separator arguments in an argument list. | 

