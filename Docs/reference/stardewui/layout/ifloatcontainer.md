---
title: IFloatContainer
description: Provides access to a UI element's floating elements.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Interface IFloatContainer

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Layout](index.md)  
Assembly: StardewUI.dll  

</div>

Provides access to a UI element's floating elements.

```cs
public interface IFloatContainer
```

## Remarks

Every subclass of [View](../view.md) has built-in behavior to hold, track, layout and display floating elements, but some custom [IView](../iview.md) implementations (including [DecoratorView](../widgets/decoratorview.md) subclasses) may not, and must implement the interface themselves if they wish to support floats.

## Members

### Properties

 | Name | Description |
| --- | --- |
| [FloatingElements](#floatingelements) | The floating elements to display relative to this view. | 

## Details

### Properties

#### FloatingElements

The floating elements to display relative to this view.

```cs
System.Collections.Generic.IList<StardewUI.Layout.FloatingElement> FloatingElements { get; set; }
```

##### Property Value

[IList](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ilist-1)<[FloatingElement](floatingelement.md)>

-----

