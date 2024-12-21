---
title: ViewNode
description: Internal structure of a view node, encapsulating dependencies required for data binding and lazy creation/updates.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class ViewNode

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Binding](index.md)  
Assembly: StardewUI.dll  

</div>

Internal structure of a view node, encapsulating dependencies required for data binding and lazy creation/updates.

```cs
public class ViewNode : StardewUI.Framework.Binding.IViewNode, 
    System.IDisposable
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ ViewNode

**Implements**  
[IViewNode](iviewnode.md), [IDisposable](https://learn.microsoft.com/en-us/dotnet/api/system.idisposable)

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [ViewNode(IValueSourceFactory, IValueConverterFactory, IViewFactory, IViewBinder, SElement, IResolutionScope, ViewBehaviors, IAttribute, IAttribute)](#viewnodeivaluesourcefactory-ivalueconverterfactory-iviewfactory-iviewbinder-selement-iresolutionscope-viewbehaviors-iattribute-iattribute) | Internal structure of a view node, encapsulating dependencies required for data binding and lazy creation/updates. | 

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
| [ToString()](#tostring) | <span class="muted" markdown>(Overrides [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object).`ToString()`)</span> | 
| [Update(TimeSpan)](#updatetimespan) | Performs the regular per-frame update for this node. | 

## Details

### Constructors

#### ViewNode(IValueSourceFactory, IValueConverterFactory, IViewFactory, IViewBinder, SElement, IResolutionScope, ViewBehaviors, IAttribute, IAttribute)

Internal structure of a view node, encapsulating dependencies required for data binding and lazy creation/updates.

```cs
public ViewNode(StardewUI.Framework.Sources.IValueSourceFactory valueSourceFactory, StardewUI.Framework.Converters.IValueConverterFactory valueConverterFactory, StardewUI.Framework.Binding.IViewFactory viewFactory, StardewUI.Framework.Binding.IViewBinder viewBinder, StardewUI.Framework.Dom.SElement element, StardewUI.Framework.Content.IResolutionScope resolutionScope, StardewUI.Framework.Binding.ViewBehaviors behaviors, StardewUI.Framework.Dom.IAttribute contextAttribute, StardewUI.Framework.Dom.IAttribute floatAttribute);
```

##### Parameters

**`valueSourceFactory`** &nbsp; [IValueSourceFactory](../sources/ivaluesourcefactory.md)  
The factory responsible for creating [IValueSource&lt;T&gt;](../sources/ivaluesource-1.md) instances from attribute data.

**`valueConverterFactory`** &nbsp; [IValueConverterFactory](../converters/ivalueconverterfactory.md)  
The factory responsible for creating [IValueConverter&lt;TSource, TDestination&gt;](../converters/ivalueconverter-2.md) instances, used to convert bound values to the types required by the target view.

**`viewFactory`** &nbsp; [IViewFactory](iviewfactory.md)  
Factory for creating views, based on their tag names.

**`viewBinder`** &nbsp; [IViewBinder](iviewbinder.md)  
Binding service used to create [IViewBinding](iviewbinding.md) instances that detect changes to data or assets and propagate them to the bound [IView](../../iview.md).

**`element`** &nbsp; [SElement](../dom/selement.md)  
Element data for this node.

**`resolutionScope`** &nbsp; [IResolutionScope](../content/iresolutionscope.md)  
Scope for resolving externalized attributes, such as translation keys.

**`behaviors`** &nbsp; [ViewBehaviors](viewbehaviors.md)  
Behavior extensions for this node.

**`contextAttribute`** &nbsp; [IAttribute](../dom/iattribute.md)  
Optional attribute specifying how to resolve the context for child nodes based on this node's assigned [Context](viewnode.md#context).

**`floatAttribute`** &nbsp; [IAttribute](../dom/iattribute.md)  
Optional attribute designating that the node should be a [FloatingElement](../../layout/floatingelement.md) and specifying how to resolve its [Position](../../layout/floatingelement.md#position).

-----

### Properties

#### Children

The children of this node.

```cs
public System.Collections.Generic.IReadOnlyList<StardewUI.Framework.Binding.IViewNode.Child> Children { get; set; }
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

#### ToString()



```cs
public override string ToString();
```

##### Returns

[string](https://learn.microsoft.com/en-us/dotnet/api/system.string)

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

