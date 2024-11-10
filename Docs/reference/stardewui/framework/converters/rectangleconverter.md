---
title: RectangleConverter
description: String converter for the XNA Rectangle type.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class RectangleConverter

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Converters](index.md)  
Assembly: StardewUI.dll  

</div>

String converter for the XNA [Rectangle](https://docs.monogame.net/api/Microsoft.Xna.Framework.Rectangle.html) type.

```cs
public class RectangleConverter : 
    StardewUI.Framework.Converters.IValueConverter<string, Microsoft.Xna.Framework.Rectangle>, 
    StardewUI.Framework.Converters.IValueConverter
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ RectangleConverter

**Implements**  
[IValueConverter](ivalueconverter-2.md)<[string](https://learn.microsoft.com/en-us/dotnet/api/system.string), [Rectangle](https://docs.monogame.net/api/Microsoft.Xna.Framework.Rectangle.html)>, [IValueConverter](ivalueconverter.md)

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [RectangleConverter()](#rectangleconverter) |  | 

### Methods

 | Name | Description |
| --- | --- |
| [Convert(string)](#convertstring) | Converts a value from the source type to the destination type. | 
| [Parse(string)](#parsestring) | Parses a [Rectangle](https://docs.monogame.net/api/Microsoft.Xna.Framework.Rectangle.html) value from its comma-separated string representation. | 

## Details

### Constructors

#### RectangleConverter()



```cs
public RectangleConverter();
```

-----

### Methods

#### Convert(string)

Converts a value from the source type to the destination type.

```cs
public Microsoft.Xna.Framework.Rectangle Convert(string value);
```

##### Parameters

**`value`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The value to convert.

##### Returns

[Rectangle](https://docs.monogame.net/api/Microsoft.Xna.Framework.Rectangle.html)

-----

#### Parse(string)

Parses a [Rectangle](https://docs.monogame.net/api/Microsoft.Xna.Framework.Rectangle.html) value from its comma-separated string representation.

```cs
public static Microsoft.Xna.Framework.Rectangle Parse(string value);
```

##### Parameters

**`value`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
String representation of a [Rectangle](https://docs.monogame.net/api/Microsoft.Xna.Framework.Rectangle.html), having 4 comma-separated integer values.

##### Returns

[Rectangle](https://docs.monogame.net/api/Microsoft.Xna.Framework.Rectangle.html)

  The parsed [Rectangle](https://docs.monogame.net/api/Microsoft.Xna.Framework.Rectangle.html).

##### Remarks

This is equivalent to [Convert(string)](rectangleconverter.md#convertstring) but does not require an instance.

-----

