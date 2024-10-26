---
title: EventBindingFactory
description: Reflection-based implementation of an IEventBindingFactory.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class EventBindingFactory

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Binding](index.md)  
Assembly: StardewUI.dll  

</div>

Reflection-based implementation of an [IEventBindingFactory](ieventbindingfactory.md).

```cs
public class EventBindingFactory : 
    StardewUI.Framework.Binding.IEventBindingFactory
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ EventBindingFactory

**Implements**  
[IEventBindingFactory](ieventbindingfactory.md)

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [EventBindingFactory(IValueSourceFactory, IValueConverterFactory)](#eventbindingfactoryivaluesourcefactory-ivalueconverterfactory) | Reflection-based implementation of an [IEventBindingFactory](ieventbindingfactory.md). | 

### Methods

 | Name | Description |
| --- | --- |
| [TryCreateBinding(IView, IViewDescriptor, IEvent, BindingContext)](#trycreatebindingiview-iviewdescriptor-ievent-bindingcontext) | Attempts to creates a new event binding. | 

## Details

### Constructors

#### EventBindingFactory(IValueSourceFactory, IValueConverterFactory)

Reflection-based implementation of an [IEventBindingFactory](ieventbindingfactory.md).

```cs
public EventBindingFactory(StardewUI.Framework.Sources.IValueSourceFactory valueSourceFactory, StardewUI.Framework.Converters.IValueConverterFactory valueConverterFactory);
```

##### Parameters

**`valueSourceFactory`** &nbsp; [IValueSourceFactory](../sources/ivaluesourcefactory.md)

**`valueConverterFactory`** &nbsp; [IValueConverterFactory](../converters/ivalueconverterfactory.md)

-----

### Methods

#### TryCreateBinding(IView, IViewDescriptor, IEvent, BindingContext)

Attempts to creates a new event binding.

```cs
public StardewUI.Framework.Binding.IEventBinding TryCreateBinding(StardewUI.IView view, StardewUI.Framework.Descriptors.IViewDescriptor viewDescriptor, StardewUI.Framework.Dom.IEvent event, StardewUI.Framework.Binding.BindingContext context);
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

