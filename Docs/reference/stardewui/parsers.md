---
title: Parsers
description: Utilities for parsing third-party types, generally related to MonoGame/XNA.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class Parsers

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI](index.md)  
Assembly: StardewUI.dll  

</div>

Utilities for parsing third-party types, generally related to MonoGame/XNA.

```cs
public static class Parsers
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ Parsers

## Members

### Methods

 | Name | Description |
| --- | --- |
| [ParseColor(string)](#parsecolorstring) | Parses a named or hex color as a [Color](https://docs.monogame.net/api/Microsoft.Xna.Framework.Color.html). | 
| [ParseRectangle(string)](#parserectanglestring) | Parses a [Rectangle](https://docs.monogame.net/api/Microsoft.Xna.Framework.Rectangle.html) value from its comma-separated string representation. | 
| [ParseVector2(string)](#parsevector2string) | Parses a [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html) from a comma-separated value pair. | 
| [ParseVector2(ReadOnlySpan&lt;Char&gt;)](#parsevector2readonlyspanchar) | Parses a [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html) from a comma-separated value pair. | 
| [TryParseColor(string, Color)](#tryparsecolorstring-color) | Attempts to parse a named or hex color as a [Color](https://docs.monogame.net/api/Microsoft.Xna.Framework.Color.html). | 
| [TryParseVector2(ReadOnlySpan&lt;Char&gt;, Vector2)](#tryparsevector2readonlyspanchar-vector2) | Attempts to parse a [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html) from a comma-separated value pair. | 

## Details

### Methods

#### ParseColor(string)

Parses a named or hex color as a [Color](https://docs.monogame.net/api/Microsoft.Xna.Framework.Color.html).

```cs
public static Microsoft.Xna.Framework.Color ParseColor(string value);
```

##### Parameters

**`value`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
A string containing a named or hex color.a

##### Returns

[Color](https://docs.monogame.net/api/Microsoft.Xna.Framework.Color.html)

  The parsed color.

##### Remarks

Supports hex strings of the form `#rgb`, `#rrggbb`, or `#rrggbbaa`, as well as any of the MonoGame named color strings like `LimeGreen`.

-----

#### ParseRectangle(string)

Parses a [Rectangle](https://docs.monogame.net/api/Microsoft.Xna.Framework.Rectangle.html) value from its comma-separated string representation.

```cs
public static Microsoft.Xna.Framework.Rectangle ParseRectangle(string value);
```

##### Parameters

**`value`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
String representation of a [Rectangle](https://docs.monogame.net/api/Microsoft.Xna.Framework.Rectangle.html), having 4 comma-separated integer values.

##### Returns

[Rectangle](https://docs.monogame.net/api/Microsoft.Xna.Framework.Rectangle.html)

  The parsed [Rectangle](https://docs.monogame.net/api/Microsoft.Xna.Framework.Rectangle.html).

##### Remarks

This is equivalent to [Convert](https://learn.microsoft.com/en-us/dotnet/api/system.convert) but does not require an instance.

-----

#### ParseVector2(string)

Parses a [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html) from a comma-separated value pair.

```cs
public static Microsoft.Xna.Framework.Vector2 ParseVector2(string value);
```

##### Parameters

**`value`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The string value.

##### Returns

[Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)

  The parsed [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html).

-----

#### ParseVector2(ReadOnlySpan&lt;Char&gt;)

Parses a [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html) from a comma-separated value pair.

```cs
public static Microsoft.Xna.Framework.Vector2 ParseVector2(ReadOnlySpan<System.Char> value);
```

##### Parameters

**`value`** &nbsp; [ReadOnlySpan](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<[Char](https://learn.microsoft.com/en-us/dotnet/api/system.char)>  
The string value.

##### Returns

[Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)

  The parsed [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html).

-----

#### TryParseColor(string, Color)

Attempts to parse a named or hex color as a [Color](https://docs.monogame.net/api/Microsoft.Xna.Framework.Color.html).

```cs
public static bool TryParseColor(string value, out Microsoft.Xna.Framework.Color color);
```

##### Parameters

**`value`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
A string containing a named or hex color.a

**`color`** &nbsp; [Color](https://docs.monogame.net/api/Microsoft.Xna.Framework.Color.html)  
The result if successful, otherwise a default [Color](https://docs.monogame.net/api/Microsoft.Xna.Framework.Color.html).

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  `true` if the `value` was successfully parsed into `color`; `false` if the parsing was unsuccessful.

##### Remarks

Supports hex strings of the form `#rgb`, `#rrggbb`, or `#rrggbbaa`, as well as any of the MonoGame named color strings like `LimeGreen`.

-----

#### TryParseVector2(ReadOnlySpan&lt;Char&gt;, Vector2)

Attempts to parse a [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html) from a comma-separated value pair.

```cs
public static bool TryParseVector2(ReadOnlySpan<System.Char> value, out Microsoft.Xna.Framework.Vector2 result);
```

##### Parameters

**`value`** &nbsp; [ReadOnlySpan](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<[Char](https://learn.microsoft.com/en-us/dotnet/api/system.char)>  
The string value.

**`result`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
The result if successful, otherwise a default [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  `true` if the `value` was successfully parsed into `result`; `false` if the parsing was unsuccessful.

-----

