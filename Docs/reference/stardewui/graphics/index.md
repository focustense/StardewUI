---
title: StardewUI.Graphics
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# StardewUI.Graphics Namespace

## Classes

| Name | Description |
| --- | --- |
| [ButtonSpriteMap](buttonspritemap.md) | Base class for a [ISpriteMap&lt;T&gt;](ispritemap-1.md) for controller/keyboard bindings. |
| [NineSlice](nineslice.md) | Draws sprites according to a nine-slice scale. |
| [PropagatedSpriteBatch](propagatedspritebatch.md) | Sprite batch wrapper with transform propagation. |
| [SimpleRotationExtensions](simplerotationextensions.md) | Helper extensions for the [SimpleRotation](simplerotation.md) type. |
| [SliceSettings](slicesettings.md) | Additional nine-slice settings for dealing with certain "unique" structures. |
| [Sprite](sprite.md) | Definition for a scalable sprite. |
| [SpriteMap&lt;T&gt;](spritemap-1.md) | General implementation of an [ISpriteMap&lt;T&gt;](ispritemap-1.md) that can be prepared in a variety of ways. |
| [SpriteMapBuilder&lt;T&gt;](spritemapbuilder-1.md) | Builder interface for a [SpriteMap&lt;T&gt;](spritemap-1.md) using a single texture source. |
| [Transform](transform.md) | Global transform applied to an [ISpriteBatch](ispritebatch.md). |
| [UiSprites](uisprites.md) | Included game sprites that are required for many UI/menu widgets. |
| [XeluButtonSpriteMap](xelubuttonspritemap.md) | Controller/keyboard sprite map based on Xelu's CC0 pack: https://thoseawesomeguys.com/prompts/ |

## Interfaces

| Name | Description |
| --- | --- |
| [ISpriteBatch](ispritebatch.md) | Abstraction for the [SpriteBatch](https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.SpriteBatch.html) providing sprite-drawing methods. |
| [ISpriteMap&lt;T&gt;](ispritemap-1.md) | Provides a single method to obtain a sprite for some key, such as SButton. |

## Enums

| Name | Description |
| --- | --- |
| [SimpleRotation](simplerotation.md) | Types of rotations that are considered to be "simple", i.e. those that only transpose pixels and are therefore fast and non-deforming. |
| [SliceCenterPosition](slicecenterposition.md) | Specifies which side the center position of a [SliceSettings](slicesettings.md) instance is on. |
| [XeluButtonSpriteMap](xelubuttonspritemap.md).[SpriteTheme](xelubuttonspritemap.spritetheme.md) | Available theme variants for certain sprites. |

