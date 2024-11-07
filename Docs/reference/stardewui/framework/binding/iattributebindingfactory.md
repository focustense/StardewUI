---
title: IAttributeBindingFactory
description: Service for creating IAttributeBinding instances for the individual attributes of a bound view.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Interface IAttributeBindingFactory

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Binding](index.md)  
Assembly: StardewUI.dll  

</div>

Service for creating [IAttributeBinding](iattributebinding.md) instances for the individual attributes of a bound view.

```cs
public interface IAttributeBindingFactory
```

## Members

### Methods

 | Name | Description |
| --- | --- |
| [TryCreateBinding(IViewDescriptor, IAttribute, BindingContext, IResolutionScope)](#trycreatebindingiviewdescriptor-iattribute-bindingcontext-iresolutionscope) | Attempts to creates a new attribute binding. | 

## Details

### Methods

#### TryCreateBinding(IViewDescriptor, IAttribute, BindingContext, IResolutionScope)

Attempts to creates a new attribute binding.

```cs
StardewUI.Framework.Binding.IAttributeBinding TryCreateBinding(StardewUI.Framework.Descriptors.IViewDescriptor viewDescriptor, StardewUI.Framework.Dom.IAttribute attribute, StardewUI.Framework.Binding.BindingContext context, StardewUI.Framework.Content.IResolutionScope resolutionScope);
```

##### Parameters

**`viewDescriptor`** &nbsp; [IViewDescriptor](../descriptors/iviewdescriptor.md)  
Descriptor for the bound view, providing access to its properties.

**`attribute`** &nbsp; [IAttribute](../dom/iattribute.md)  
The attribute data.

**`context`** &nbsp; [BindingContext](bindingcontext.md)  
The binding context, including the bound data and descriptor for the data type.

**`resolutionScope`** &nbsp; [IResolutionScope](../content/iresolutionscope.md)  
Scope for resolving externalized attributes, such as translation keys.

##### Returns

[IAttributeBinding](iattributebinding.md)

  The created binding, or `null` if the arguments do not support creating a binding, such as an `attribute` bound to a `null` value of `context`.

-----

