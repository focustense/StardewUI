---
title: IOffsettable&lt;T&gt;
description: Provides a method to clone the current instance with an offset applied.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Interface IOffsettable&lt;T&gt;

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Layout](index.md)  
Assembly: StardewUI.dll  

</div>

Provides a method to clone the current instance with an offset applied.

```cs
public interface IOffsettable<T>
```

### Type Parameters

**`T`**  
The output type; should be the same as the implementing class.


## Members

### Methods

 | Name | Description |
| --- | --- |
| [Offset(Vector2)](#offsetvector2) | Creates a clone of this instance with an offset applied to its position. | 

## Details

### Methods

#### Offset(Vector2)

Creates a clone of this instance with an offset applied to its position.

```cs
T Offset(Microsoft.Xna.Framework.Vector2 distance);
```

##### Parameters

**`distance`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
The offset distance.

##### Returns

`T`

-----

