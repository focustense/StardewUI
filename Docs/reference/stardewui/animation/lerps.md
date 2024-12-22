---
title: Lerps
description: Common registration and lookup for interpolation functions.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class Lerps

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Animation](index.md)  
Assembly: StardewUI.dll  

</div>

Common registration and lookup for interpolation functions.

```cs
public static class Lerps
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ Lerps

## Members

### Methods

 | Name | Description |
| --- | --- |
| [Add&lt;T&gt;(Lerp&lt;T&gt;)](#addtlerpt) | Registers a new interpolation function, if there is not already a function for the same type. | 
| [Get&lt;T&gt;()](#gett) | Retrieves the interpolation function for a given type, if one is defined. | 

## Details

### Methods

#### Add&lt;T&gt;(Lerp&lt;T&gt;)

Registers a new interpolation function, if there is not already a function for the same type.

```cs
public static void Add<T>(StardewUI.Animation.Lerp<T> lerp);
```

##### Parameters

**`lerp`** &nbsp; [Lerp&lt;T&gt;](lerp-1.md)  
Interpolation function for the specified type.

##### Remarks

If an interpolation function is already known for the type `T`, the call is ignored.

-----

#### Get&lt;T&gt;()

Retrieves the interpolation function for a given type, if one is defined.

```cs
public static StardewUI.Animation.Lerp<T> Get<T>();
```

##### Returns

[Lerp&lt;T&gt;](lerp-1.md)

  The interpolation function for type `T`, or `null` if there is no known function for the given type.

-----

