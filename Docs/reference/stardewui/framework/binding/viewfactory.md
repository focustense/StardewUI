---
title: ViewFactory
description: A view factory based on per-tag delegates. Can be used as a base class for other view factories.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class ViewFactory

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Binding](index.md)  
Assembly: StardewUI.dll  

</div>

A view factory based on per-tag delegates. Can be used as a base class for other view factories.

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
| [Register&lt;TView&gt;(string)](#registertviewstring) | Registers a view for a given tag using the view's default parameterless constructor. | 
| [Register(string, Func&lt;IView&gt;)](#registerstring-funciview) | Registers a view for a given tag using a delegate function. | 
| [SupportsTag(string)](#supportstagstring) | Checks if the factory can create views corresponding to a specific tag. | 

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
public virtual StardewUI.IView CreateView(string tagName);
```

##### Parameters

**`tagName`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The markup tag that specifies the type of view.

##### Returns

[IView](../../iview.md)

  A new view of a type corresponding to the `tagName`.

-----

#### Register&lt;TView&gt;(string)

Registers a view for a given tag using the view's default parameterless constructor.

```cs
public void Register<TView>(string tagName);
```

##### Parameters

**`tagName`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The markup tag corresponding to the `TView` type.

-----

#### Register(string, Func&lt;IView&gt;)

Registers a view for a given tag using a delegate function.

```cs
public void Register(string tagName, Func<StardewUI.IView> tagFactory);
```

##### Parameters

**`tagName`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The markup tag to handle.

**`tagFactory`** &nbsp; [Func](https://learn.microsoft.com/en-us/dotnet/api/system.func-1)<[IView](../../iview.md)>  
Delegate function to create the view corresponding to the `tagName`.

-----

#### SupportsTag(string)

Checks if the factory can create views corresponding to a specific tag.

```cs
public virtual bool SupportsTag(string tagName);
```

##### Parameters

**`tagName`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The markup tag that specifies the type of view.

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  `true` if this factory should handle the specified `tagName`, otherwise `false`.

-----

