---
title: ViewFactory
description: View factory for built-in view types.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class ViewFactory

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Binding](index.md)  
Assembly: StardewUI.dll  

</div>

View factory for built-in view types.

```cs
public class ViewFactory : StardewUI.Framework.Binding.IViewFactory
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ ViewFactory

**Implements**  
[IViewFactory](iviewfactory.md)

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [ViewFactory()](#viewfactory) |  | 

### Methods

 | Name | Description |
| --- | --- |
| [CreateView(string)](#createviewstring) | Creates a new view. | 

## Details

### Constructors

#### ViewFactory()



```cs
public ViewFactory();
```

-----

### Methods

#### CreateView(string)

Creates a new view.

```cs
public StardewUI.IView CreateView(string tagName);
```

##### Parameters

**`tagName`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The markup tag that specifies the type of view.

##### Returns

[IView](../../iview.md)

  A new view of a type corresponding to the `tagName`.

-----

