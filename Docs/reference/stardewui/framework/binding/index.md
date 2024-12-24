---
title: StardewUI.Framework.Binding
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# StardewUI.Framework.Binding Namespace

## Classes

| Name | Description |
| --- | --- |
| [AttributeBindingFactory](attributebindingfactory.md) | A general [IAttributeBindingFactory](iattributebindingfactory.md) implementation using dependency injection for all resolution. |
| [BackoffNodeDecorator](backoffnodedecorator.md) | A transparent binding node whose purpose is to throttle failed updates and log any errors. |
| [BinaryCondition](binarycondition.md) | A condition based on the comparison of two values. |
| [BindingContext](bindingcontext.md) | Context, or scope, of a bound view, providing the backing data and tools for accessing its properties. |
| [BindingDirectionExtensions](bindingdirectionextensions.md) | Extension methods for the [BindingDirection](bindingdirection.md) enum. |
| [BindingException](bindingexception.md) | The exception that is thrown when an unrecoverable error happens during data binding for a view. |
| [BoundViewDefaults](boundviewdefaults.md) | View defaults that provide the current data-bound values for any bound attributes/properties and fall back to the original ("blank") view defaults for unbound properties. |
| [ConditionalNode](conditionalnode.md) | A structural node that only passes through its child node when some condition passes. |
| [ConditionExtensions](conditionextensions.md) | Extensions for the [ICondition](icondition.md) interface. |
| [ContextUpdateTracker](contextupdatetracker.md) | Tracks context instances that already had updates dispatched this frame, to prevent duplication. |
| [ContextUpdatingNodeDecorator](contextupdatingnodedecorator.md) | A transparent binding node that propagates [Update(TimeSpan)](iviewnode.md#updatetimespan) ticks to an eligible context. |
| [EventBindingFactory](eventbindingfactory.md) | Reflection-based implementation of an [IEventBindingFactory](ieventbindingfactory.md). |
| [IncludedViewNode](includedviewnode.md) | Quasi-structural node that loads its content from a shared game asset. |
| [IViewNode](iviewnode.md).[Child](iviewnode.child.md) | Child of an [IViewNode](iviewnode.md), specifying the node data and the view outlet in which it should appear. |
| [NegatedCondition](negatedcondition.md) | Wrapper for an [ICondition](icondition.md) that negates its outcome. |
| [ReflectionViewBinder](reflectionviewbinder.md) | An [IViewBinder](iviewbinder.md) implementation using reflected view descriptors. |
| [RepeaterNode](repeaternode.md) | A structural node that accepts a collection ([IEnumerable&lt;T&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)) valued attribute and repeats its inner elements with each child bound to a collection element, in the same order as the collection. |
| [UnaryCondition](unarycondition.md) | A condition based on a single value that is convertible to a [Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean). |
| [ViewBehaviors](viewbehaviors.md) | Wrapper for the entire set of behaviors attached to a single node/view. |
| [ViewFactory](viewfactory.md) | A view factory based on per-tag delegates. Can be used as a base class for other view factories. |
| [ViewNode](viewnode.md) | Internal structure of a view node, encapsulating dependencies required for data binding and lazy creation/updates. |
| [ViewNodeFactory](viewnodefactory.md) | Default in-game view engine. |

## Interfaces

| Name | Description |
| --- | --- |
| [IAttributeBinding](iattributebinding.md) | Binding instance for a single attribute on a single view. |
| [IAttributeBindingFactory](iattributebindingfactory.md) | Service for creating [IAttributeBinding](iattributebinding.md) instances for the individual attributes of a bound view. |
| [ICondition](icondition.md) | A condition used in a [ConditionalNode](conditionalnode.md). |
| [IEventBinding](ieventbinding.md) | Binding instance for a single event on a single view. |
| [IEventBindingFactory](ieventbindingfactory.md) | Service for creating [IEventBinding](ieventbinding.md) instances for a view's events, and subscribing the handlers. |
| [IViewBinder](iviewbinder.md) | Service for creating view bindings and their dependencies. |
| [IViewBinding](iviewbinding.md) | Represents the binding state of an entire view; provides a single method to perform a once-per-frame update. |
| [IViewFactory](iviewfactory.md) | Factory for creating views from tags. |
| [IViewNode](iviewnode.md) | Encapsulates a single bound node in a view tree. |
| [IViewNodeFactory](iviewnodefactory.md) | High-level abstraction for translating node trees into bound view trees. |

## Enums

| Name | Description |
| --- | --- |
| [BindingDirection](bindingdirection.md) | The direction of data flow in a data binding. |

