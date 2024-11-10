---
title: Vector2Converter
description: String converter for the XNA Vector2 type.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class Vector2Converter

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Converters](index.md)  
Assembly: StardewUI.dll  

</div>

String converter for the XNA [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html) type.

```cs
public class Vector2Converter : 
    StardewUI.Framework.Converters.IValueConverter<string, Microsoft.Xna.Framework.Vector2>, 
    StardewUI.Framework.Converters.IValueConverter
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ Vector2Converter

**Implements**  
[IValueConverter](ivalueconverter-2.md)<[string](https://learn.microsoft.com/en-us/dotnet/api/system.string), [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)>, [IValueConverter](ivalueconverter.md)

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [Vector2Converter()](#vector2converter) |  | 

### Methods

 | Name | Description |
| --- | --- |
| [Convert(string)](#convertstring) | Converts a value from the source type to the destination type. | 

## Details

### Constructors

#### Vector2Converter()



```cs
public Vector2Converter();
```

-----

### Methods

#### Convert(string)

Converts a value from the source type to the destination type.

```cs
public Microsoft.Xna.Framework.Vector2 Convert(string value);
```

##### Parameters

**`value`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The value to convert.

##### Returns

[Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)

-----

