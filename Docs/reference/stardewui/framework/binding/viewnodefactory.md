---
title: ViewNodeFactory
description: Default in-game view engine.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class ViewNodeFactory

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Binding](index.md)  
Assembly: StardewUI.dll  

</div>

Default in-game view engine.

```cs
public class ViewNodeFactory : StardewUI.Framework.Binding.IViewNodeFactory
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ ViewNodeFactory

**Implements**  
[IViewNodeFactory](iviewnodefactory.md)

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [ViewNodeFactory(IViewFactory, IValueSourceFactory, IValueConverterFactory, IViewBinder, IAssetCache)](#viewnodefactoryiviewfactory-ivaluesourcefactory-ivalueconverterfactory-iviewbinder-iassetcache) | Default in-game view engine. | 

### Methods

 | Name | Description |
| --- | --- |
| [CreateNode(SNode)](#createnodesnode) | Creates a bound view node, and all descendants, from parsed node data. | 

## Details

### Constructors

#### ViewNodeFactory(IViewFactory, IValueSourceFactory, IValueConverterFactory, IViewBinder, IAssetCache)

Default in-game view engine.

```cs
public ViewNodeFactory(StardewUI.Framework.Binding.IViewFactory viewFactory, StardewUI.Framework.Sources.IValueSourceFactory valueSourceFactory, StardewUI.Framework.Converters.IValueConverterFactory valueConverterFactory, StardewUI.Framework.Binding.IViewBinder viewBinder, StardewUI.Framework.Content.IAssetCache assetCache);
```

##### Parameters

**`viewFactory`** &nbsp; [IViewFactory](iviewfactory.md)  
Factory for creating views, based on their tag names.

**`valueSourceFactory`** &nbsp; [IValueSourceFactory](../sources/ivaluesourcefactory.md)  
The factory responsible for creating [IValueSource&lt;T&gt;](../sources/ivaluesource-1.md) instances from attribute data.

**`valueConverterFactory`** &nbsp; [IValueConverterFactory](../converters/ivalueconverterfactory.md)  
The factory responsible for creating [IValueConverter&lt;TSource, TDestination&gt;](../converters/ivalueconverter-2.md) instances, used to convert bound values to the types required by the target view or structural property.

**`viewBinder`** &nbsp; [IViewBinder](iviewbinder.md)  
Binding service used to create [IViewBinding](iviewbinding.md) instances that detect changes to data or assets and propagate them to the bound [IView](../../iview.md).

**`assetCache`** &nbsp; [IAssetCache](../content/iassetcache.md)  
Cache for obtaining document assets. Used for included views.

-----

### Methods

#### CreateNode(SNode)

Creates a bound view node, and all descendants, from parsed node data.

```cs
public StardewUI.Framework.Binding.IViewNode CreateNode(StardewUI.Framework.Dom.SNode node);
```

##### Parameters

**`node`** &nbsp; [SNode](../dom/snode.md)  
The node data.

##### Returns

[IViewNode](iviewnode.md)

  An [IViewNode](iviewnode.md) providing the [IView](../../iview.md) bound with the node's attributes and children, which automatically applies changes on each [Update(TimeSpan)](iviewnode.md#updatetimespan).

-----

