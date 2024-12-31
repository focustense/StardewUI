---
title: StardewUI.Framework.Converters
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# StardewUI.Framework.Converters Namespace

## Classes

| Name | Description |
| --- | --- |
| [AnyCastConverterFactory](anycastconverterfactory.md) | Factory supporting conversions to and from [IAnyCast](../ianycast.md). |
| [CastingValueConverterFactory](castingvalueconverterfactory.md) | Factory that automatically implements casting conversions, where the source type can be assigned directly to the destination type. |
| [ColorConverter](colorconverter.md) | String converter for the XNA [Color](https://docs.monogame.net/api/Microsoft.Xna.Framework.Color.html) type. |
| [DuckTypeClassConverterFactory](ducktypeclassconverterfactory.md) | Factory that creates duck-typing converters for `class` and `struct` types. |
| [DuckTypeEnumConverterFactory](ducktypeenumconverterfactory.md) | Factory that automatically implements duck-typing conversions between enum types that share the same names. |
| [EnumNameConverterFactory](enumnameconverterfactory.md) | Factory that automatically implements string-to-enum conversions based on the case-insensitive enum names. |
| [IdentityValueConverterFactory](identityvalueconverterfactory.md) | Factory that automatically implements identity conversions, where the source and destination type are the same. |
| [ItemSpriteConverter](itemspriteconverter.md) | Converts data from a game item to its corresponding sprite. |
| [LayoutConverter](layoutconverter.md) | String converter for the [LayoutParameters](../../layout/layoutparameters.md) type. |
| [NamedFontConverter](namedfontconverter.md) | Converter for fonts that are already built into the game, i.e. found on Game1. |
| [NullableConverterFactory](nullableconverterfactory.md) | Factory that implements automatic conversion between nullable and non-nullable types. |
| [PointConverter](pointconverter.md) | String converter for the XNA [Point](https://docs.monogame.net/api/Microsoft.Xna.Framework.Point.html) type. |
| [StringConverterFactory](stringconverterfactory.md) | Provides conversions from any type to [string](https://learn.microsoft.com/en-us/dotnet/api/system.string). |
| [TextureRectSpriteConverter](texturerectspriteconverter.md) | Converts a tuple with a texture and source rectangle (within the texture) to a sprite record. |
| [TextureSpriteConverter](texturespriteconverter.md) | Converts a texture to a sprite record, using the texture's entire bounds as the source rectangle. |
| [TransformConverter](transformconverter.md) | String converter for the [Transform](../../graphics/transform.md) type. |
| [ValueConverter&lt;TSource, TDestination&gt;](valueconverter-2.md) | Generic delegating converter, accepting a conversion function. |
| [ValueConverterFactory](valueconverterfactory.md) | Standard implementation of [IValueConverterFactory](ivalueconverterfactory.md) that allows registering new converters. |

## Interfaces

| Name | Description |
| --- | --- |
| [IValueConverter](ivalueconverter.md) | Provides a method to convert between arbitrary types. |
| [IValueConverter&lt;TSource, TDestination&gt;](ivalueconverter-2.md) | Provides a method to convert between value types. |
| [IValueConverterFactory](ivalueconverterfactory.md) | Factory for obtaining instance of [IValueConverter&lt;TSource, TDestination&gt;](ivalueconverter-2.md). |

