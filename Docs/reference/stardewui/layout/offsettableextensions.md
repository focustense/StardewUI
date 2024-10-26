---
title: OffsettableExtensions
description: Extensions for the IOffsettable&lt;T&gt; interface.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class OffsettableExtensions

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Layout](index.md)  
Assembly: StardewUI.dll  

</div>

Extensions for the [IOffsettable&lt;T&gt;](ioffsettable-1.md) interface.

```cs
public static class OffsettableExtensions
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ OffsettableExtensions

## Members

### Methods

 | Name | Description |
| --- | --- |
| [Clone&lt;T&gt;(T)](#clonett) | Clones an [IOffsettable&lt;T&gt;](ioffsettable-1.md). | 

## Details

### Methods

#### Clone&lt;T&gt;(T)

Clones an [IOffsettable&lt;T&gt;](ioffsettable-1.md).

```cs
public static T Clone<T>(T instance);
```

##### Parameters

**`instance`** &nbsp; T  
The instance to clone.

##### Returns

`T`

  A copy of the `instance`.

##### Remarks

Since every [Offset(Vector2)](ioffsettable-1.md#offsetvector2) is implicitly a clone, we can perform an "explicit" clone by providing a zero offset.

-----

