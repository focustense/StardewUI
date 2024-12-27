---
title: Parsers
description: Utilities for parsing third-party types, generally related MonoGame/XNA.
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

Utilities for parsing third-party types, generally related MonoGame/XNA.

```cs
public static class Parsers
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ Parsers

## Members

### Methods

 | Name | Description |
| --- | --- |
| [ParseVector2(string)](#parsevector2string) | Parses a [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html) from a comma-separated value pair. | 
| [ParseVector2(ReadOnlySpan&lt;Char&gt;)](#parsevector2readonlyspanchar) | Parses a [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html) from a comma-separated value pair. | 
| [TryParseVector2(ReadOnlySpan&lt;Char&gt;, Vector2)](#tryparsevector2readonlyspanchar-vector2) | Attempts to parse a [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html) from a comma-separated value pair. | 

## Details

### Methods

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

