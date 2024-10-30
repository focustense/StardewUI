---
title: AttributeBindingFactory
description: A general IAttributeBindingFactory implementation using dependency injection for all resolution.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class AttributeBindingFactory

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Binding](index.md)  
Assembly: StardewUI.dll  

</div>

A general [IAttributeBindingFactory](iattributebindingfactory.md) implementation using dependency injection for all resolution.

```cs
public class AttributeBindingFactory : 
    StardewUI.Framework.Binding.IAttributeBindingFactory
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ AttributeBindingFactory

**Implements**  
[IAttributeBindingFactory](iattributebindingfactory.md)

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [AttributeBindingFactory(IValueSourceFactory, IValueConverterFactory)](#attributebindingfactoryivaluesourcefactory-ivalueconverterfactory) | A general [IAttributeBindingFactory](iattributebindingfactory.md) implementation using dependency injection for all resolution. | 

### Methods

 | Name | Description |
| --- | --- |
| [TryCreateBinding(IViewDescriptor, IAttribute, BindingContext)](#trycreatebindingiviewdescriptor-iattribute-bindingcontext) | Attempts to creates a new attribute binding. | 

## Details

### Constructors

#### AttributeBindingFactory(IValueSourceFactory, IValueConverterFactory)

A general [IAttributeBindingFactory](iattributebindingfactory.md) implementation using dependency injection for all resolution.

```cs
public AttributeBindingFactory(StardewUI.Framework.Sources.IValueSourceFactory valueSourceFactory, StardewUI.Framework.Converters.IValueConverterFactory valueConverterFactory);
```

##### Parameters

**`valueSourceFactory`** &nbsp; [IValueSourceFactory](../sources/ivaluesourcefactory.md)  
The factory responsible for creating [IValueSource&lt;T&gt;](../sources/ivaluesource-1.md) instances from attribute data.

**`valueConverterFactory`** &nbsp; [IValueConverterFactory](../converters/ivalueconverterfactory.md)  
The factory responsible for creating [IValueConverter&lt;TSource, TDestination&gt;](../converters/ivalueconverter-2.md) instances, used to convert bound values to the types required by the target view.

-----

### Methods

#### TryCreateBinding(IViewDescriptor, IAttribute, BindingContext)

Attempts to creates a new attribute binding.

```cs
public StardewUI.Framework.Binding.IAttributeBinding TryCreateBinding(StardewUI.Framework.Descriptors.IViewDescriptor viewDescriptor, StardewUI.Framework.Dom.IAttribute attribute, StardewUI.Framework.Binding.BindingContext context);
```

##### Parameters

**`viewDescriptor`** &nbsp; [IViewDescriptor](../descriptors/iviewdescriptor.md)  
Descriptor for the bound view, providing access to its properties.

**`attribute`** &nbsp; [IAttribute](../dom/iattribute.md)  
The attribute data.

**`context`** &nbsp; [BindingContext](bindingcontext.md)  
The binding context, including the bound data and descriptor for the data type.

##### Returns

[IAttributeBinding](iattributebinding.md)

  The created binding, or `null` if the arguments do not support creating a binding, such as an `attribute` bound to a `null` value of `context`.

-----

