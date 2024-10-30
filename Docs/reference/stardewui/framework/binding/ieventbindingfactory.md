---
title: IEventBindingFactory
description: Service for creating IEventBinding instances for a view's events, and subscribing the handlers.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Interface IEventBindingFactory

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Binding](index.md)  
Assembly: StardewUI.dll  

</div>

Service for creating [IEventBinding](ieventbinding.md) instances for a view's events, and subscribing the handlers.

```cs
public interface IEventBindingFactory
```

## Members

### Methods

 | Name | Description |
| --- | --- |
| [TryCreateBinding(IView, IViewDescriptor, IEvent, BindingContext)](#trycreatebindingiview-iviewdescriptor-ievent-bindingcontext) | Attempts to creates a new event binding. | 

## Details

### Methods

#### TryCreateBinding(IView, IViewDescriptor, IEvent, BindingContext)

Attempts to creates a new event binding.

```cs
StardewUI.Framework.Binding.IEventBinding TryCreateBinding(StardewUI.IView view, StardewUI.Framework.Descriptors.IViewDescriptor viewDescriptor, StardewUI.Framework.Dom.IEvent event, StardewUI.Framework.Binding.BindingContext context);
```

##### Parameters

**`view`** &nbsp; [IView](../../iview.md)  
The view to bind to; the target that will raise the bound event.

**`viewDescriptor`** &nbsp; [IViewDescriptor](../descriptors/iviewdescriptor.md)  
Descriptor for the bound view, providing access to its events.

**`event`** &nbsp; [IEvent](../dom/ievent.md)  
The event data.

**`context`** &nbsp; [BindingContext](bindingcontext.md)  
The binding context, including the type descriptor and handler methods.

##### Returns

[IEventBinding](ieventbinding.md)

  The created binding, or `null` if the arguments do not support creating a binding, such as an `event` bound to a `null` value of `context`.

-----

