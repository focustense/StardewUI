---
title: IViewBinding
description: Represents the binding state of an entire view; provides a single method to perform a once-per-frame update.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Interface IViewBinding

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Binding](index.md)  
Assembly: StardewUI.dll  

</div>

Represents the binding state of an entire view; provides a single method to perform a once-per-frame update.

```cs
public interface IViewBinding : System.IDisposable
```

**Implements**  
[IDisposable](https://learn.microsoft.com/en-us/dotnet/api/system.idisposable)

## Members

### Properties

 | Name | Description |
| --- | --- |
| [Attributes](#attributes) | The specific attributes bound for the attached view. | 

### Methods

 | Name | Description |
| --- | --- |
| [Update()](#update) | Updates the view, including all bound attributes. | 

## Details

### Properties

#### Attributes

The specific attributes bound for the attached view.

```cs
System.Collections.Generic.IReadOnlyList<StardewUI.Framework.Binding.IAttributeBinding> Attributes { get; }
```

##### Property Value

[IReadOnlyList](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlylist-1)<[IAttributeBinding](iattributebinding.md)>

##### Remarks

Per-attribute updates are encapsulated in the [Update()](iviewbinding.md#update) method, so this is normally only needed for inspecting the state of bindings, e.g. to build a [BoundViewDefaults](boundviewdefaults.md) instance.

-----

### Methods

#### Update()

Updates the view, including all bound attributes.

```cs
bool Update();
```

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  `true` if any updates were performed; `false` if there was no update due to having no underlying changes in the bound data or assets.

-----

