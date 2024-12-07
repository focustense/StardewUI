---
title: IViewNode.Child
description: Child of an IViewNode, specifying the node data and the view outlet in which it should appear.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class IViewNode.Child

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Binding](index.md)  
Assembly: StardewUI.dll  

</div>

Child of an [IViewNode](iviewnode.md), specifying the node data and the view outlet in which it should appear.

```cs
public record IViewNode.Child : System.IDisposable, 
    IEquatable<StardewUI.Framework.Binding.IViewNode.Child>
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ Child

**Implements**  
[IDisposable](https://learn.microsoft.com/en-us/dotnet/api/system.idisposable), [IEquatable](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1)<[Child](iviewnode.child.md)>

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [Child(IViewNode, string)](#childiviewnode-string) | Child of an [IViewNode](iviewnode.md), specifying the node data and the view outlet in which it should appear. | 

### Properties

 | Name | Description |
| --- | --- |
| [EqualityContract](#equalitycontract) |  | 
| [Node](#node) | The child node. | 
| [OutletName](#outletname) | The outlet in which the `Node` should be inserted. | 

### Methods

 | Name | Description |
| --- | --- |
| [Dispose()](#dispose) |  | 

## Details

### Constructors

#### Child(IViewNode, string)

Child of an [IViewNode](iviewnode.md), specifying the node data and the view outlet in which it should appear.

```cs
public Child(StardewUI.Framework.Binding.IViewNode Node, string OutletName);
```

##### Parameters

**`Node`** &nbsp; [IViewNode](iviewnode.md)  
The child node.

**`OutletName`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The outlet in which the `Node` should be inserted.

-----

### Properties

#### EqualityContract



```cs
protected System.Type EqualityContract { get; }
```

##### Property Value

[Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)

-----

#### Node

The child node.

```cs
public StardewUI.Framework.Binding.IViewNode Node { get; set; }
```

##### Property Value

[IViewNode](iviewnode.md)

-----

#### OutletName

The outlet in which the `Node` should be inserted.

```cs
public string OutletName { get; set; }
```

##### Property Value

[string](https://learn.microsoft.com/en-us/dotnet/api/system.string)

-----

### Methods

#### Dispose()



```cs
public void Dispose();
```

-----

