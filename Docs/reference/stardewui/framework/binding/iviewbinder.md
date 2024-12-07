---
title: IViewBinder
description: Service for creating view bindings and their dependencies.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Interface IViewBinder

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Binding](index.md)  
Assembly: StardewUI.dll  

</div>

Service for creating view bindings and their dependencies.

```cs
public interface IViewBinder
```

## Members

### Methods

 | Name | Description |
| --- | --- |
| [Bind(IView, IElement, BindingContext, IResolutionScope)](#bindiview-ielement-bindingcontext-iresolutionscope) | Creates a view binding. | 
| [GetDescriptor(IView)](#getdescriptoriview) | Retrieves the descriptor for a view, which provides information about its properties. | 

## Details

### Methods

#### Bind(IView, IElement, BindingContext, IResolutionScope)

Creates a view binding.

```cs
StardewUI.Framework.Binding.IViewBinding Bind(StardewUI.IView view, StardewUI.Framework.Dom.IElement element, StardewUI.Framework.Binding.BindingContext context, StardewUI.Framework.Content.IResolutionScope resolutionScope);
```

##### Parameters

**`view`** &nbsp; [IView](../../iview.md)  
The view that will be bound.

**`element`** &nbsp; [IElement](../dom/ielement.md)  
The element data providing the literal or binding attributes.

**`context`** &nbsp; [BindingContext](bindingcontext.md)  
The binding context/data, for any non-asset bindings using bindings whose [AttributeValueType](../grammar/attributevaluetype.md) is one of the recognized [IsContextBinding(AttributeValueType)](../grammar/attributevaluetypeextensions.md#iscontextbindingattributevaluetype) types.

**`resolutionScope`** &nbsp; [IResolutionScope](../content/iresolutionscope.md)  
Scope for resolving externalized attributes, such as translation keys.

##### Returns

[IViewBinding](iviewbinding.md)

  A view binding that can be used to propagate changes in the `context` or any dependent assets to the `view`.

-----

#### GetDescriptor(IView)

Retrieves the descriptor for a view, which provides information about its properties.

```cs
StardewUI.Framework.Descriptors.IViewDescriptor GetDescriptor(StardewUI.IView view);
```

##### Parameters

**`view`** &nbsp; [IView](../../iview.md)  
The view instance.

##### Returns

[IViewDescriptor](../descriptors/iviewdescriptor.md)

  The descriptor for the `view`.

##### Remarks

Descriptors participate in view binding but may also be used for other purposes, such as updating child lists.

-----

