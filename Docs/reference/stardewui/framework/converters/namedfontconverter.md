---
title: NamedFontConverter
description: Converter for fonts that are already built into the game, i.e. found on Game1.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class NamedFontConverter

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Converters](index.md)  
Assembly: StardewUI.dll  

</div>

Converter for fonts that are already built into the game, i.e. found on Game1.

```cs
public class NamedFontConverter : 
    StardewUI.Framework.Converters.IValueConverter<string, Microsoft.Xna.Framework.Graphics.SpriteFont>
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ NamedFontConverter

**Implements**  
[IValueConverter](ivalueconverter-2.md)<[string](https://learn.microsoft.com/en-us/dotnet/api/system.string), [SpriteFont](https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.SpriteFont.html)>

## Remarks

Does not account for fonts added as separate assets, which require bound attributes and not literals.

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [NamedFontConverter()](#namedfontconverter) |  | 

### Methods

 | Name | Description |
| --- | --- |
| [Convert(string)](#convertstring) | Converts a value from the source type to the destination type. | 

## Details

### Constructors

#### NamedFontConverter()



```cs
public NamedFontConverter();
```

-----

### Methods

#### Convert(string)

Converts a value from the source type to the destination type.

```cs
public Microsoft.Xna.Framework.Graphics.SpriteFont Convert(string value);
```

##### Parameters

**`value`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The value to convert.

##### Returns

[SpriteFont](https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.SpriteFont.html)

-----

