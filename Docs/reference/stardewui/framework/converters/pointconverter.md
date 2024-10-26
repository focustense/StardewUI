---
title: PointConverter
description: String converter for the XNA Point type.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class PointConverter

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Converters](index.md)  
Assembly: StardewUI.dll  

</div>

String converter for the XNA [Point](https://docs.monogame.net/api/Microsoft.Xna.Framework.Point.html) type.

```cs
public class PointConverter : 
    StardewUI.Framework.Converters.IValueConverter<string, Microsoft.Xna.Framework.Point>
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ PointConverter

**Implements**  
[IValueConverter](ivalueconverter-2.md)<[string](https://learn.microsoft.com/en-us/dotnet/api/system.string), [Point](https://docs.monogame.net/api/Microsoft.Xna.Framework.Point.html)>

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [PointConverter()](#pointconverter) |  | 

### Methods

 | Name | Description |
| --- | --- |
| [Convert(string)](#convertstring) | Converts a value from the source type to the destination type. | 

## Details

### Constructors

#### PointConverter()



```cs
public PointConverter();
```

-----

### Methods

#### Convert(string)

Converts a value from the source type to the destination type.

```cs
public Microsoft.Xna.Framework.Point Convert(string value);
```

##### Parameters

**`value`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The value to convert.

##### Returns

[Point](https://docs.monogame.net/api/Microsoft.Xna.Framework.Point.html)

-----

