---
title: IViewBinding
description: Represents the binding state of an entire view; provides a single method to perform a once-per-frame update.
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

### Methods

 | Name | Description |
| --- | --- |
| [Update()](#update) | Updates the view, including all bound attributes. | 

## Details

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

