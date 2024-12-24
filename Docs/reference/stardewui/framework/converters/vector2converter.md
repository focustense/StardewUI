---
title: Vector2Converter
description: String converter for the XNA Vector2 type.
search:
    boost: 0.002
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
| [Parse(ReadOnlySpan&lt;Char&gt;)](#parsereadonlyspanchar) | Parses a [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html) from a comma-separated value pair. | 

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

#### Parse(ReadOnlySpan&lt;Char&gt;)

Parses a [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html) from a comma-separated value pair.

```cs
public static Microsoft.Xna.Framework.Vector2 Parse(ReadOnlySpan<System.Char> value);
```

##### Parameters

**`value`** &nbsp; [ReadOnlySpan](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<[Char](https://learn.microsoft.com/en-us/dotnet/api/system.char)>  
The string value.

##### Returns

[Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)

  The parsed [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html).

-----

