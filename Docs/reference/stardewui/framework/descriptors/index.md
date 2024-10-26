---
title: StardewUI.Framework.Descriptors
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# StardewUI.Framework.Descriptors Namespace

## Classes

| Name | Description |
| --- | --- |
| [DescriptorException](descriptorexception.md) | The exception that is thrown when an error occurs while reading or building the metadata for a bound view or one of its data sources. |
| [ExpressionFieldDescriptor&lt;T, TValue&gt;](expressionfielddescriptor-2.md) | Implementation of a field descriptor using a compiled expression tree. |
| [LazyExpressionFieldDescriptor](lazyexpressionfielddescriptor.md) | Helper for creating [LazyExpressionFieldDescriptor&lt;TValue&gt;](lazyexpressionfielddescriptor-1.md) with types not known at compile time. |
| [LazyExpressionFieldDescriptor&lt;TValue&gt;](lazyexpressionfielddescriptor-1.md) | Implementation of a field descriptor that supports a transition between two inner descriptor types. |
| [ReflectionEventDescriptor](reflectioneventdescriptor.md) | Helper for creating [IEventDescriptor](ieventdescriptor.md) instances using reflection. |
| [ReflectionEventDescriptor&lt;TTarget, THandler&gt;](reflectioneventdescriptor-2.md) | Reflection-based implementation of an event descriptor. |
| [ReflectionFieldDescriptor&lt;TValue&gt;](reflectionfielddescriptor-1.md) | Implementation of a field descriptor based on reflection. |
| [ReflectionMethodDescriptor](reflectionmethoddescriptor.md) | Helper for creating [IMethodDescriptor](imethoddescriptor.md) instances using reflection. |
| [ReflectionObjectDescriptor](reflectionobjectdescriptor.md) | Object descriptor based on reflection. |
| [ReflectionPropertyDescriptor](reflectionpropertydescriptor.md) | Helper for creating [ReflectionPropertyDescriptor&lt;T, TValue&gt;](reflectionpropertydescriptor-2.md) with types not known at compile time. |
| [ReflectionPropertyDescriptor&lt;T, TValue&gt;](reflectionpropertydescriptor-2.md) | Binding property based on reflection. |
| [ReflectionViewDescriptor](reflectionviewdescriptor.md) | View descriptor based on reflection. |
| [ThisPropertyDescriptor](thispropertydescriptor.md) | Helper for obtaining a [ThisPropertyDescriptor&lt;T&gt;](thispropertydescriptor-1.md) using a type known only at runtime. |
| [ThisPropertyDescriptor&lt;T&gt;](thispropertydescriptor-1.md) | Special descriptor used for "this" references in argument/attribute bindings, allowing them to reference the current context instead of a property on it. |

## Interfaces

| Name | Description |
| --- | --- |
| [IEventDescriptor](ieventdescriptor.md) | Describes a single event on some type. |
| [IMemberDescriptor](imemberdescriptor.md) | Describes a single member (property, method, or event) of a bindable object, such as a view. |
| [IMethodDescriptor](imethoddescriptor.md) | Describes a single method on some type. |
| [IMethodDescriptor&lt;T&gt;](imethoddescriptor-1.md) | Describes a single method on some type, and provides a wrapper method to invoke it. |
| [IObjectDescriptor](iobjectdescriptor.md) | Describes a type of object that participates in view binding, either as the target or the source. |
| [IPropertyDescriptor](ipropertydescriptor.md) | Describes a single property on a bindable object (i.e. a view). |
| [IPropertyDescriptor&lt;T&gt;](ipropertydescriptor-1.md) | Describes a single property on a bindable object (i.e. a view) and provides methods to read or write the value. |
| [IViewDescriptor](iviewdescriptor.md) | Describes a type of view that can be used in a view binding. |

