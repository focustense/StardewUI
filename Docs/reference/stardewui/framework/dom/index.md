---
title: StardewUI.Framework.Dom
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# StardewUI.Framework.Dom Namespace

## Classes

| Name | Description |
| --- | --- |
| [ContextRedirect](contextredirect.md) | Describes how to redirect the target context of any [IAttribute](iattribute.md) whose [ValueType](iattribute.md#valuetype) is one of the [IsContextBinding(AttributeValueType)](../grammar/attributevaluetypeextensions.md#iscontextbindingattributevaluetype) matching types. |
| [ContextRedirect](contextredirect.md).[Distance](contextredirect.distance.md) | Redirects to an ancestor context by walking up a specified number of levels. |
| [ContextRedirect](contextredirect.md).[Type](contextredirect.type.md) | Redirects to the nearest ancestor matching a specified type. |
| [Document](document.md) | A standalone StarML document. |
| [SArgument](sargument.md) | Record implementation of an [IArgument](iargument.md). |
| [SAttribute](sattribute.md) | Record implementation of a StarML [IAttribute](iattribute.md). |
| [SElement](selement.md) | Record implementation of a StarML [IElement](ielement.md). |
| [SEvent](sevent.md) | An event attribute in a StarML document. |
| [SNode](snode.md) | A node in a StarML document, encapsulating the tag, its attributes, and all child nodes. |
| [TemplateNodeTransformer](templatenodetransformer.md) | Transforms a `template` node based on the structure (attributes, children, etc.) of the instantiating node. |

## Interfaces

| Name | Description |
| --- | --- |
| [IArgument](iargument.md) | An argument to a method call, e.g. as used in an [IEvent](ievent.md). |
| [IAttribute](iattribute.md) | Attribute of a StarML element. |
| [IElement](ielement.md) | Element in a StarML document, including the tag and all enclosed attributes. |
| [IEvent](ievent.md) | Event wire-up in a StarML element. |
| [INodeTransformer](inodetransformer.md) | Provides a method to transform nodes into other nodes. |

