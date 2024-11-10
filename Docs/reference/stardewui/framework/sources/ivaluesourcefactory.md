---
title: IValueSourceFactory
description: Provides methods to look up runtime value types and build appropriate sources based on their binding information.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Interface IValueSourceFactory

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Sources](index.md)  
Assembly: StardewUI.dll  

</div>

Provides methods to look up runtime value types and build appropriate sources based on their binding information.

```cs
public interface IValueSourceFactory
```

## Members

### Methods

 | Name | Description |
| --- | --- |
| [GetValueSource(Type, IArgument, BindingContext)](#getvaluesourcetype-iargument-bindingcontext) | Creates a value source that supplies values of a given type according to the specified argument binding. | 
| [GetValueSource(Type, IAttribute, BindingContext, IResolutionScope)](#getvaluesourcetype-iattribute-bindingcontext-iresolutionscope) | Creates a value source that supplies values of a given type according to the specified binding attribute. | 
| [GetValueSource&lt;T&gt;(IArgument, BindingContext)](#getvaluesourcetiargument-bindingcontext) | Creates a value source that supplies values according to the specified argument binding. | 
| [GetValueSource&lt;T&gt;(IAttribute, BindingContext, IResolutionScope)](#getvaluesourcetiattribute-bindingcontext-iresolutionscope) | Creates a value source that supplies values according to the specified binding attribute. | 
| [GetValueType(IArgument, BindingContext)](#getvaluetypeiargument-bindingcontext) | Determines the type of value that will be supplied by a given argument binding, and with the specified context. | 
| [GetValueType(IAttribute, IPropertyDescriptor, BindingContext)](#getvaluetypeiattribute-ipropertydescriptor-bindingcontext) | Determines the type of value that will be supplied by a given attribute binding, and with the specified context. | 

## Details

### Methods

#### GetValueSource(Type, IArgument, BindingContext)

Creates a value source that supplies values of a given type according to the specified argument binding.

```cs
StardewUI.Framework.Sources.IValueSource GetValueSource(System.Type type, StardewUI.Framework.Dom.IArgument argument, StardewUI.Framework.Binding.BindingContext context);
```

##### Parameters

**`type`** &nbsp; [Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)  
The type of value to obtain; can be determined using [GetValueType(IArgument, BindingContext)](ivaluesourcefactory.md#getvaluetypeiargument-bindingcontext).

**`argument`** &nbsp; [IArgument](../dom/iargument.md)  
The parsed markup argument containing the binding info.

**`context`** &nbsp; [BindingContext](../binding/bindingcontext.md)  
The binding context to use for any contextual bindings (those with [ContextBinding](../grammar/argumentexpressiontype.md#contextbinding)).

##### Returns

[IValueSource](ivaluesource.md)

-----

#### GetValueSource(Type, IAttribute, BindingContext, IResolutionScope)

Creates a value source that supplies values of a given type according to the specified binding attribute.

```cs
StardewUI.Framework.Sources.IValueSource GetValueSource(System.Type type, StardewUI.Framework.Dom.IAttribute attribute, StardewUI.Framework.Binding.BindingContext context, StardewUI.Framework.Content.IResolutionScope scope);
```

##### Parameters

**`type`** &nbsp; [Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)  
The type of value to obtain; can be determined using [GetValueType(IArgument, BindingContext)](ivaluesourcefactory.md#getvaluetypeiargument-bindingcontext).

**`attribute`** &nbsp; [IAttribute](../dom/iattribute.md)  
The parsed markup attribute containing the binding info.

**`context`** &nbsp; [BindingContext](../binding/bindingcontext.md)  
The binding context to use for any contextual bindings (those with [ContextBinding](../grammar/argumentexpressiontype.md#contextbinding)).

**`scope`** &nbsp; [IResolutionScope](../content/iresolutionscope.md)  
Scope for resolving externalized attributes, such as translation keys.

##### Returns

[IValueSource](ivaluesource.md)

-----

#### GetValueSource&lt;T&gt;(IArgument, BindingContext)

Creates a value source that supplies values according to the specified argument binding.

```cs
StardewUI.Framework.Sources.IValueSource<T> GetValueSource<T>(StardewUI.Framework.Dom.IArgument argument, StardewUI.Framework.Binding.BindingContext context);
```

##### Parameters

**`argument`** &nbsp; [IArgument](../dom/iargument.md)  
The parsed markup argument containing the binding info.

**`context`** &nbsp; [BindingContext](../binding/bindingcontext.md)  
The binding context to use for any contextual bindings (those with [InputBinding](../grammar/attributevaluetype.md#inputbinding), [OneTimeBinding](../grammar/attributevaluetype.md#onetimebinding), [OutputBinding](../grammar/attributevaluetype.md#outputbinding) or [TwoWayBinding](../grammar/attributevaluetype.md#twowaybinding)).

##### Returns

[IValueSource&lt;T&gt;](ivaluesource-1.md)

-----

#### GetValueSource&lt;T&gt;(IAttribute, BindingContext, IResolutionScope)

Creates a value source that supplies values according to the specified binding attribute.

```cs
StardewUI.Framework.Sources.IValueSource<T> GetValueSource<T>(StardewUI.Framework.Dom.IAttribute attribute, StardewUI.Framework.Binding.BindingContext context, StardewUI.Framework.Content.IResolutionScope scope);
```

##### Parameters

**`attribute`** &nbsp; [IAttribute](../dom/iattribute.md)  
The parsed markup attribute containing the binding info.

**`context`** &nbsp; [BindingContext](../binding/bindingcontext.md)  
The binding context to use for any contextual bindings (those with [InputBinding](../grammar/attributevaluetype.md#inputbinding), [OneTimeBinding](../grammar/attributevaluetype.md#onetimebinding), [OutputBinding](../grammar/attributevaluetype.md#outputbinding) or [TwoWayBinding](../grammar/attributevaluetype.md#twowaybinding)).

**`scope`** &nbsp; [IResolutionScope](../content/iresolutionscope.md)  
Scope for resolving externalized attributes, such as translation keys.

##### Returns

[IValueSource&lt;T&gt;](ivaluesource-1.md)

-----

#### GetValueType(IArgument, BindingContext)

Determines the type of value that will be supplied by a given argument binding, and with the specified context.

```cs
System.Type GetValueType(StardewUI.Framework.Dom.IArgument argument, StardewUI.Framework.Binding.BindingContext context);
```

##### Parameters

**`argument`** &nbsp; [IArgument](../dom/iargument.md)  
The parsed markup argument containing the binding info.

**`context`** &nbsp; [BindingContext](../binding/bindingcontext.md)  
The binding context to use for any contextual bindings (those with [ContextBinding](../grammar/argumentexpressiontype.md#contextbinding)).

##### Returns

[Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)

##### Remarks

This provides the type argument that must be supplied to [GetValueSource(Type, IArgument, BindingContext)](ivaluesourcefactory.md#getvaluesourcetype-iargument-bindingcontext).

-----

#### GetValueType(IAttribute, IPropertyDescriptor, BindingContext)

Determines the type of value that will be supplied by a given attribute binding, and with the specified context.

```cs
System.Type GetValueType(StardewUI.Framework.Dom.IAttribute attribute, StardewUI.Framework.Descriptors.IPropertyDescriptor property, StardewUI.Framework.Binding.BindingContext context);
```

##### Parameters

**`attribute`** &nbsp; [IAttribute](../dom/iattribute.md)  
The parsed markup attribute containing the binding info.

**`property`** &nbsp; [IPropertyDescriptor](../descriptors/ipropertydescriptor.md)  
Binding metadata for the destination property; used when the source does not encode any independent type information. If not specified, some attribute values may be unsupported.

**`context`** &nbsp; [BindingContext](../binding/bindingcontext.md)  
The binding context to use for any contextual bindings (those with [InputBinding](../grammar/attributevaluetype.md#inputbinding), [OneTimeBinding](../grammar/attributevaluetype.md#onetimebinding), [OutputBinding](../grammar/attributevaluetype.md#outputbinding) or [TwoWayBinding](../grammar/attributevaluetype.md#twowaybinding)).

##### Returns

[Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)

##### Remarks

This provides the type argument that must be supplied to [GetValueSource(Type, IArgument, BindingContext)](ivaluesourcefactory.md#getvaluesourcetype-iargument-bindingcontext).

-----

