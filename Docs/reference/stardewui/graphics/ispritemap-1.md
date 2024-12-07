---
title: ISpriteMap&lt;T&gt;
description: Provides a single method to obtain a sprite for some key, such as SButton.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Interface ISpriteMap&lt;T&gt;

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Graphics](index.md)  
Assembly: StardewUI.dll  

</div>

Provides a single method to obtain a sprite for some key, such as SButton.

```cs
public interface ISpriteMap<T>
```

### Type Parameters

**`T`**  
Type of key for which to obtain sprites.


## Members

### Methods

 | Name | Description |
| --- | --- |
| [Get(T, Boolean)](#gett-boolean) | Gets the sprite corresponding to a particular key. | 

## Details

### Methods

#### Get(T, Boolean)

Gets the sprite corresponding to a particular key.

```cs
StardewUI.Graphics.Sprite Get(T key, out System.Boolean isPlaceholder);
```

##### Parameters

**`key`** &nbsp; T  
The key to retrieve.

**`isPlaceholder`** &nbsp; [Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)  
`true` if the returned [Sprite](sprite.md) is not specific to the `key`, but is instead a placeholder (border/background) in which some substitute, typically normal text, must be drawn. `false` if the [Sprite](sprite.md) is a complete self-contained representation of the `key`.

##### Returns

[Sprite](sprite.md)

  The precise or generic sprite for the given `key`.

-----

