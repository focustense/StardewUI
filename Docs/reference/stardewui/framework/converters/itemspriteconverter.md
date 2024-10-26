---
title: ItemSpriteConverter
description: Converts data from a game item to its corresponding sprite.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class ItemSpriteConverter

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Converters](index.md)  
Assembly: StardewUI.dll  

</div>

Converts data from a game item to its corresponding sprite.

```cs
public class ItemSpriteConverter : 
    StardewUI.Framework.Converters.IValueConverter<StardewValley.ItemTypeDefinitions.ParsedItemData, StardewUI.Graphics.Sprite>
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ ItemSpriteConverter

**Implements**  
[IValueConverter](ivalueconverter-2.md)<ParsedItemData, [Sprite](../../graphics/sprite.md)>

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [ItemSpriteConverter()](#itemspriteconverter) |  | 

### Methods

 | Name | Description |
| --- | --- |
| [Convert(ParsedItemData)](#convertparseditemdata) | Converts a value from the source type to the destination type. | 

## Details

### Constructors

#### ItemSpriteConverter()



```cs
public ItemSpriteConverter();
```

-----

### Methods

#### Convert(ParsedItemData)

Converts a value from the source type to the destination type.

```cs
public StardewUI.Graphics.Sprite Convert(StardewValley.ItemTypeDefinitions.ParsedItemData value);
```

##### Parameters

**`value`** &nbsp; ParsedItemData  
The value to convert.

##### Returns

[Sprite](../../graphics/sprite.md)

-----

