---
title: TokenType
description: Types of tokens allowed in StarML.
search:
    boost: 0.002
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
| <a id="attributemodifier">AttributeModifier</a> | 7 | Modifier token designating the type of an attribute; the `*` character (structural) or `+` (behavior). | 
| <a id="literal">Literal</a> | 8 | A string of literal text, as found within a quoted or bound attribute. | 
| <a id="assignment">Assignment</a> | 9 | The `=` character, as used in an attribute syntax such as `attr="value"`. | 
| <a id="negation">Negation</a> | 10 | The `!` character, sometimes used to negate the value of an attribute, e.g. for conditional attributes. | 
| <a id="quote">Quote</a> | 11 | Double quote character (`"`) used to start or terminate a [Literal](tokentype.md#literal) string. | 
| <a id="bindingstart">BindingStart</a> | 12 | A pair of opening braces (`{{`), used to start a binding expression for an attribute value. | 
| <a id="bindingend">BindingEnd</a> | 13 | A pair of closing braces (`}}`), used to end a binding expression for an attribute value. | 
| <a id="bindingmodifier">BindingModifier</a> | 14 | An explicit binding modifier; one of `@` (asset), `#` (translation), `&` (template), `<` (input only), `>` (output only) or `<>` (two-way). | 
| <a id="contextparent">ContextParent</a> | 15 | A caret (`^`) used in a binding expression, indicating a walk up to the parent context. | 
| <a id="contextancestor">ContextAncestor</a> | 16 | A tilde (`~`) used in a binding expression, indicating traversal up to a parent with a named type. | 
| <a id="pipe">Pipe</a> | 17 | The pipe (`|`) character, which is used to start and end event bindings. | 
| <a id="argumentliststart">ArgumentListStart</a> | 18 | The left parenthesis (`(`) character, used to start an argument list. | 
| <a id="argumentlistend">ArgumentListEnd</a> | 19 | The right parenthesis (`(`) character, used to end an argument list. | 
| <a id="argumentprefix">ArgumentPrefix</a> | 20 | Prefix character for an argument, e.g. `$` to refer to an event property. | 
| <a id="argumentseparator">ArgumentSeparator</a> | 21 | The comma (`,`) character, used to separator arguments in an argument list. | 
| <a id="commentstart">CommentStart</a> | 22 | Beginning of a comment block (`<!--`). | 
| <a id="commentend">CommentEnd</a> | 23 | End of a comment block (`-->`). | 

