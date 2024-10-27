---
title: ContextRedirect.Distance
description: Redirects to an ancestor context by walking up a specified number of levels.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class ContextRedirect.Distance

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Dom](index.md)  
Assembly: StardewUI.dll  

</div>

Redirects to an ancestor context by walking up a specified number of levels.

```cs
public record ContextRedirect.Distance : StardewUI.Framework.Dom.ContextRedirect, 
    IEquatable<StardewUI.Framework.Dom.ContextRedirect.Distance>
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ [ContextRedirect](contextredirect.md) ⇦ Distance

**Implements**  
[IEquatable](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1)<[Distance](contextredirect.distance.md)>

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [Distance(UInt32)](#distanceuint) | Redirects to an ancestor context by walking up a specified number of levels. | 

### Properties

 | Name | Description |
| --- | --- |
| [Depth](#depth) | Number of parents to traverse. | 
| [EqualityContract](#equalitycontract) | <span class="muted" markdown>(Overrides [ContextRedirect](contextredirect.md).`get_EqualityContract()`)</span> | 

## Details

### Constructors

#### Distance(uint)

Redirects to an ancestor context by walking up a specified number of levels.

```cs
public Distance(uint Depth);
```

##### Parameters

**`Depth`** &nbsp; [UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32)  
Number of parents to traverse.

-----

### Properties

#### Depth

Number of parents to traverse.

```cs
public uint Depth { get; set; }
```

##### Property Value

[UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32)

-----

#### EqualityContract



```cs
protected System.Type EqualityContract { get; }
```

##### Property Value

[Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)

-----

