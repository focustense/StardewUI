---
title: ReflectionViewBinder
description: An IViewBinder implementation using reflected view descriptors.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class ReflectionViewBinder

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Binding](index.md)  
Assembly: StardewUI.dll  

</div>

An [IViewBinder](iviewbinder.md) implementation using reflected view descriptors.

```cs
public class ReflectionViewBinder : StardewUI.Framework.Binding.IViewBinder
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ ReflectionViewBinder

**Implements**  
[IViewBinder](iviewbinder.md)

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [ReflectionViewBinder(IAttributeBindingFactory, IEventBindingFactory)](#reflectionviewbinderiattributebindingfactory-ieventbindingfactory) | An [IViewBinder](iviewbinder.md) implementation using reflected view descriptors. | 

### Methods

 | Name | Description |
| --- | --- |
| [Bind(IView, IElement, BindingContext, IResolutionScope)](#bindiview-ielement-bindingcontext-iresolutionscope) | Creates a view binding. | 
| [GetDescriptor(IView)](#getdescriptoriview) | Retrieves the descriptor for a view, which provides information about its properties. | 

## Details

### Constructors

#### ReflectionViewBinder(IAttributeBindingFactory, IEventBindingFactory)

An [IViewBinder](iviewbinder.md) implementation using reflected view descriptors.

```cs
public ReflectionViewBinder(StardewUI.Framework.Binding.IAttributeBindingFactory attributeBindingFactory, StardewUI.Framework.Binding.IEventBindingFactory eventBindingFactory);
```

##### Parameters

**`attributeBindingFactory`** &nbsp; [IAttributeBindingFactory](iattributebindingfactory.md)  
Factory for creating the [IAttributeBinding](iattributebinding.md) instances used to bind individual attributes of the view.

**`eventBindingFactory`** &nbsp; [IEventBindingFactory](ieventbindingfactory.md)  
Factory for creating the [IEventBinding](ieventbinding.md) instances used to bind events raised by the view.

-----

### Methods

#### Bind(IView, IElement, BindingContext, IResolutionScope)

Creates a view binding.

```cs
public StardewUI.Framework.Binding.IViewBinding Bind(StardewUI.IView view, StardewUI.Framework.Dom.IElement element, StardewUI.Framework.Binding.BindingContext context, StardewUI.Framework.Content.IResolutionScope resolutionScope);
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
public StardewUI.Framework.Descriptors.IViewDescriptor GetDescriptor(StardewUI.IView view);
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

