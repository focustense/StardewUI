---
title: ColorConverter
description: String converter for the XNA Color type.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class ColorConverter

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Converters](index.md)  
Assembly: StardewUI.dll  

</div>

String converter for the XNA [Color](https://docs.monogame.net/api/Microsoft.Xna.Framework.Color.html) type.

```cs
public class ColorConverter : 
    StardewUI.Framework.Converters.IValueConverter<string, Microsoft.Xna.Framework.Color>, 
    StardewUI.Framework.Converters.IValueConverter
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ ColorConverter

**Implements**  
[IValueConverter](ivalueconverter-2.md)<[string](https://learn.microsoft.com/en-us/dotnet/api/system.string), [Color](https://docs.monogame.net/api/Microsoft.Xna.Framework.Color.html)>, [IValueConverter](ivalueconverter.md)

## Remarks

Supports hex strings of the form `#rgb`, `#rrggbb`, or `#rrggbbaa`, as well as any of the XNA named color strings like `LimeGreen`.

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [ColorConverter()](#colorconverter) |  | 

### Methods

 | Name | Description |
| --- | --- |
| [Convert(string)](#convertstring) | Converts a value from the source type to the destination type. | 

## Details

### Constructors

#### ColorConverter()



```cs
public ColorConverter();
```

-----

### Methods

#### Convert(string)

Converts a value from the source type to the destination type.

```cs
public Microsoft.Xna.Framework.Color Convert(string value);
```

##### Parameters

**`value`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The value to convert.

##### Returns

[Color](https://docs.monogame.net/api/Microsoft.Xna.Framework.Color.html)

-----

