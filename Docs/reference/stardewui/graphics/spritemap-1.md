---
title: SpriteMap&lt;T&gt;
description: General implementation of an ISpriteMap&lt;T&gt; that can be prepared in a variety of ways.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class SpriteMap&lt;T&gt;

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Graphics](index.md)  
Assembly: StardewUI.dll  

</div>

General implementation of an [ISpriteMap&lt;T&gt;](ispritemap-1.md) that can be prepared in a variety of ways.

```cs
public class SpriteMap<T> : StardewUI.Graphics.ISpriteMap<T>
```

### Type Parameters

**`T`**  
Type of key for which to obtain sprites.


**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ SpriteMap&lt;T&gt;

**Implements**  
[ISpriteMap&lt;T&gt;](ispritemap-1.md)

## Remarks

Can be constructed directly, but it is normally recommended to use [SpriteMapBuilder&lt;T&gt;](spritemapbuilder-1.md). Applies basic placeholder logic that considers only the `defaultSprite` to be a placeholder.

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [SpriteMap&lt;T&gt;(IReadOnlyDictionary&lt;T, Sprite&gt;, Sprite)](#spritemaptireadonlydictionaryt-sprite-sprite) | General implementation of an [ISpriteMap&lt;T&gt;](ispritemap-1.md) that can be prepared in a variety of ways. | 

### Methods

 | Name | Description |
| --- | --- |
| [Get(T, Boolean)](#gett-boolean) | Gets the sprite corresponding to a particular key. | 

## Details

### Constructors

#### SpriteMap&lt;T&gt;(IReadOnlyDictionary&lt;T, Sprite&gt;, Sprite)

General implementation of an [ISpriteMap&lt;T&gt;](ispritemap-1.md) that can be prepared in a variety of ways.

```cs
public SpriteMap<T>(System.Collections.Generic.IReadOnlyDictionary<T, StardewUI.Graphics.Sprite> sprites, StardewUI.Graphics.Sprite defaultSprite);
```

##### Parameters

**`sprites`** &nbsp; [IReadOnlyDictionary&lt;T, Sprite&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2)  
Map of keys to sprites.

**`defaultSprite`** &nbsp; [Sprite](sprite.md)  
Default sprite to show when looking up a key without a corresponding sprite.

##### Remarks

Can be constructed directly, but it is normally recommended to use [SpriteMapBuilder&lt;T&gt;](spritemapbuilder-1.md). Applies basic placeholder logic that considers only the `defaultSprite` to be a placeholder.

-----

### Methods

#### Get(T, Boolean)

Gets the sprite corresponding to a particular key.

```cs
public StardewUI.Graphics.Sprite Get(T key, out System.Boolean isPlaceholder);
```

##### Parameters

**`key`** &nbsp; T  
The key to retrieve.

**`isPlaceholder`** &nbsp; [Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)  
`true` if the returned [Sprite](sprite.md) is not specific to the `key`, but is instead a placeholder (border/background) in which some substitute, typically normal text, must be drawn. `false` if the [Sprite](sprite.md) is a complete self-contained representation of the `key`.

##### Returns

[Sprite](sprite.md)

-----

