---
title: BackoffNodeDecorator
description: A transparent binding node whose purpose is to throttle failed updates and log any errors.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class BackoffNodeDecorator

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Binding](index.md)  
Assembly: StardewUI.dll  

</div>

A transparent binding node whose purpose is to throttle failed updates and log any errors.

```cs
public class BackoffNodeDecorator : StardewUI.Framework.Binding.IViewNode, 
    System.IDisposable
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ BackoffNodeDecorator

**Implements**  
[IViewNode](iviewnode.md), [IDisposable](https://learn.microsoft.com/en-us/dotnet/api/system.idisposable)

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [BackoffNodeDecorator(IViewNode, BackoffRule)](#backoffnodedecoratoriviewnode-backoffrule) | A transparent binding node whose purpose is to throttle failed updates and log any errors. | 

### Properties

 | Name | Description |
| --- | --- |
| [Children](#children) | The children of this node. | 
| [Context](#context) | The currently-bound context data, used as the source for any [InputBinding](../grammar/attributevaluetype.md#inputbinding), [OneTimeBinding](../grammar/attributevaluetype.md#onetimebinding), [OutputBinding](../grammar/attributevaluetype.md#outputbinding) or [TwoWayBinding](../grammar/attributevaluetype.md#twowaybinding) attributes. | 
| [FloatingElements](#floatingelements) | The floating elements for this node, if any have been created. | 
| [Views](#views) | The views for this node, if any have been created. | 

### Methods

 | Name | Description |
| --- | --- |
| [Dispose()](#dispose) |  | 
| [Print(StringBuilder, Boolean)](#printstringbuilder-bool) | Prints the string representation of this node. | 
| [Reset()](#reset) | Clears any [Views](iviewnode.md#views) associated with this node and resets it to the default state before it was bound. | 
| [Update(TimeSpan)](#updatetimespan) | Performs the regular per-frame update for this node. | 

## Details

### Constructors

#### BackoffNodeDecorator(IViewNode, BackoffRule)

A transparent binding node whose purpose is to throttle failed updates and log any errors.

```cs
public BackoffNodeDecorator(StardewUI.Framework.Binding.IViewNode node, StardewUI.Framework.BackoffRule backoffRule);
```

##### Parameters

**`node`** &nbsp; [IViewNode](iviewnode.md)  
The owned node.

**`backoffRule`** &nbsp; [BackoffRule](../backoffrule.md)  
Configures the backoff duration and scaling when an update fails.

-----

### Properties

#### Children

The children of this node.

```cs
public System.Collections.Generic.IReadOnlyList<StardewUI.Framework.Binding.IViewNode.Child> Children { get; }
```

##### Property Value

[IReadOnlyList](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlylist-1)<[Child](iviewnode.child.md)>

##### Remarks

Node children represent views in potentia. Every DOM node maps to (at least) one [IViewNode](iviewnode.md), but views are created lazily and may not exist for nodes with conditional attributes or other rules.

-----

#### Context

The currently-bound context data, used as the source for any [InputBinding](../grammar/attributevaluetype.md#inputbinding), [OneTimeBinding](../grammar/attributevaluetype.md#onetimebinding), [OutputBinding](../grammar/attributevaluetype.md#outputbinding) or [TwoWayBinding](../grammar/attributevaluetype.md#twowaybinding) attributes.

```cs
public StardewUI.Framework.Binding.BindingContext Context { get; set; }
```

##### Property Value

[BindingContext](bindingcontext.md)

-----

#### FloatingElements

The floating elements for this node, if any have been created.

```cs
public System.Collections.Generic.IReadOnlyList<StardewUI.Layout.FloatingElement> FloatingElements { get; }
```

##### Property Value

[IReadOnlyList](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlylist-1)<[FloatingElement](../../layout/floatingelement.md)>

-----

#### Views

The views for this node, if any have been created.

```cs
public System.Collections.Generic.IReadOnlyList<StardewUI.IView> Views { get; }
```

##### Property Value

[IReadOnlyList](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlylist-1)<[IView](../../iview.md)>

-----

### Methods

#### Dispose()



```cs
public void Dispose();
```

-----

#### Print(StringBuilder, bool)

Prints the string representation of this node.

```cs
public void Print(System.Text.StringBuilder sb, bool includeChildren);
```

##### Parameters

**`sb`** &nbsp; [StringBuilder](https://learn.microsoft.com/en-us/dotnet/api/system.text.stringbuilder)  
The output builder to receive to the node's text.

**`includeChildren`** &nbsp; [Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)  
Whether or not to include the string representation of any/all child nodes between this node's opening and close tags. If this is `false` or there are no children, it will be formatted as a self-closing tag.

-----

#### Reset()

Clears any [Views](iviewnode.md#views) associated with this node and resets it to the default state before it was bound.

```cs
public void Reset();
```

##### Remarks

Propagates the request down to [Children](iviewnode.md#children), but is not required to clear [Children](iviewnode.md#children) and does not affect the [Context](iviewnode.md#context) assignment. 

 This is used to "unbind" the target of a structural node like [ConditionalNode](conditionalnode.md) and in some cases prepare it for subsequent reuse.

-----

#### Update(TimeSpan)

Performs the regular per-frame update for this node.

```cs
public bool Update(System.TimeSpan elapsed);
```

##### Parameters

**`elapsed`** &nbsp; [TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan)  
Time elapsed since last update.

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  `true` if any aspect of the view tree from this level downward was changed, i.e. as a result of a new [Context](iviewnode.md#context), changed context properties, invalidated assets, or the [View](../../view.md) being created for the first time; `false` if no changes were made.

-----

