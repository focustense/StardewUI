---
title: Transform
description: Global transform applied to an ISpriteBatch.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class Transform

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Graphics](index.md)  
Assembly: StardewUI.dll  

</div>

Global transform applied to an [ISpriteBatch](ispritebatch.md).

```cs
public class Transform
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ Transform

## Remarks

Currently only propagates the translation, since this is comparatively trivial to implement, doesn't require any matrix math and it is very rare for UI to have rotation or scale that needs to propagate.

## Members

### Fields

 | Name | Description |
| --- | --- |
| [Default](#default) | Default instance with no transformations applied. | 

### Properties

 | Name | Description |
| --- | --- |
| [Translation](#translation) | The translation vector, i.e. global X/Y origin position. | 

### Methods

 | Name | Description |
| --- | --- |
| [FromTranslation(Vector2)](#fromtranslationvector2) | Creates a [Transform](transform.md) using a specified translation offset. | 
| [Translate(Vector2)](#translatevector2) | Applies a specified translation. | 

## Details

### Fields

#### Default

Default instance with no transformations applied.

```cs
public static readonly StardewUI.Graphics.Transform Default;
```

##### Field Value

[Transform](transform.md)

-----

### Properties

#### Translation

The translation vector, i.e. global X/Y origin position.

```cs
public Microsoft.Xna.Framework.Vector2 Translation { get; set; }
```

##### Property Value

[Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)

-----

### Methods

#### FromTranslation(Vector2)

Creates a [Transform](transform.md) using a specified translation offset.

```cs
public static StardewUI.Graphics.Transform FromTranslation(Microsoft.Xna.Framework.Vector2 translation);
```

##### Parameters

**`translation`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
The translation offset.

##### Returns

[Transform](transform.md)

  A [Transform](transform.md) whose [Translation](transform.md#translation) is equal to `translation`.

-----

#### Translate(Vector2)

Applies a specified translation.

```cs
public StardewUI.Graphics.Transform Translate(Microsoft.Xna.Framework.Vector2 translation);
```

##### Parameters

**`translation`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
The translation vector.

##### Returns

[Transform](transform.md)

  A new [Transform](transform.md) with the specified translation added to any previous transform.

-----

