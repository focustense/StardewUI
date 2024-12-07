---
title: StardewUI.Framework.Grammar
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# StardewUI.Framework.Grammar Namespace

## Classes

| Name | Description |
| --- | --- |
| [AttributeValueTypeExtensions](attributevaluetypeextensions.md) | Extensions for the [AttributeValueType](attributevaluetype.md) enum. |
| [LexerException](lexerexception.md) | The exception that is thrown when a [Lexer](lexer.md) fails to process the markup it is given. |
| [ParserException](parserexception.md) | The exception that is thrown when a [DocumentReader](documentreader.md) encounters invalid content. |

## Structs

| Name | Description |
| --- | --- |
| [Argument](argument.md) | A complete method argument parsed from StarML. |
| [Attribute](attribute.md) | A complete attribute assignment parsed from StarML. |
| [DocumentReader](documentreader.md) | Reads elements and associated attributes from a StarML document (content string). |
| [Event](event.md) | An event handler parsed from StarML. |
| [Lexer](lexer.md) | Consumes raw StarML content as a token stream. |
| [TagInfo](taginfo.md) | Information about a parsed tag in StarML. |
| [Token](token.md) | A token emitted by the StarML [Lexer](lexer.md). |

## Enums

| Name | Description |
| --- | --- |
| [ArgumentExpressionType](argumentexpressiontype.md) | Defines the possible types of an [Argument](argument.md), which specifies how to resolve its value at runtime. |
| [AttributeType](attributetype.md) | The different types of an [Attribute](attribute.md), independent of its value. |
| [AttributeValueType](attributevaluetype.md) | Types allowed for the value of an [Attribute](attribute.md). |
| [TagMember](tagmember.md) | The type of tag member read, resulting from a call to [NextMember()](documentreader.md#nextmember). |
| [TokenType](tokentype.md) | Types of tokens allowed in StarML. |

