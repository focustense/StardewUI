---
title: Cursor
description: Defines a cursor that can be drawn instead of or in addition to the regular mouse pointer.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class Cursor

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Graphics](index.md)  
Assembly: StardewUI.dll  

</div>

Defines a cursor that can be drawn instead of or in addition to the regular mouse pointer.

```cs
public record Cursor : IEquatable<StardewUI.Graphics.Cursor>
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ Cursor

**Implements**  
[IEquatable](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1)<[Cursor](cursor.md)>

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [Cursor(Sprite, Point?, Point?, Color?)](#cursorsprite-point-point-color) | Defines a cursor that can be drawn instead of or in addition to the regular mouse pointer. | 

### Fields

 | Name | Description |
| --- | --- |
| [DefaultOffset](#defaultoffset) | The default offset to apply when [Offset](cursor.md#offset) is not specified. | 
| [DefaultTint](#defaulttint) | The default color to tint with when [Tint](cursor.md#tint) is not specified. | 

### Properties

 | Name | Description |
| --- | --- |
| [EqualityContract](#equalitycontract) |  | 
| [Offset](#offset) | Offset from the exact mouse position where the sprite should be drawn. This always refers to the top-left corner of the sprite. If not specified, uses [DefaultOffset](cursor.md#defaultoffset). | 
| [Size](#size) | Size with which to draw the [Sprite](cursor.md#sprite), if different from the size of the sprite's [SourceRect](sprite.md#sourcerect). | 
| [Sprite](#sprite) | The sprite to draw. | 
| [Tint](#tint) | Tint color for the sprite. If not specified, uses [DefaultTint](cursor.md#defaulttint). | 

## Details

### Constructors

#### Cursor(Sprite, Point?, Point?, Color?)

Defines a cursor that can be drawn instead of or in addition to the regular mouse pointer.

```cs
public Cursor(StardewUI.Graphics.Sprite Sprite, Microsoft.Xna.Framework.Point? Size, Microsoft.Xna.Framework.Point? Offset, Microsoft.Xna.Framework.Color? Tint);
```

##### Parameters

**`Sprite`** &nbsp; [Sprite](sprite.md)  
The sprite to draw.

**`Size`** &nbsp; [Nullable](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1)<[Point](https://docs.monogame.net/api/Microsoft.Xna.Framework.Point.html)>  
Size with which to draw the [Sprite](cursor.md#sprite), if different from the size of the sprite's [SourceRect](sprite.md#sourcerect).

**`Offset`** &nbsp; [Nullable](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1)<[Point](https://docs.monogame.net/api/Microsoft.Xna.Framework.Point.html)>  
Offset from the exact mouse position where the sprite should be drawn. This always refers to the top-left corner of the sprite. If not specified, uses [DefaultOffset](cursor.md#defaultoffset).

**`Tint`** &nbsp; [Nullable](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1)<[Color](https://docs.monogame.net/api/Microsoft.Xna.Framework.Color.html)>  
Tint color for the sprite. If not specified, uses [DefaultTint](cursor.md#defaulttint).

-----

### Fields

#### DefaultOffset

The default offset to apply when [Offset](cursor.md#offset) is not specified.

```cs
public static readonly Microsoft.Xna.Framework.Point DefaultOffset;
```

##### Field Value

[Point](https://docs.monogame.net/api/Microsoft.Xna.Framework.Point.html)

-----

#### DefaultTint

The default color to tint with when [Tint](cursor.md#tint) is not specified.

```cs
public static readonly Microsoft.Xna.Framework.Color DefaultTint;
```

##### Field Value

[Color](https://docs.monogame.net/api/Microsoft.Xna.Framework.Color.html)

-----

### Properties

#### EqualityContract



```cs
protected System.Type EqualityContract { get; }
```

##### Property Value

[Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)

-----

#### Offset

Offset from the exact mouse position where the sprite should be drawn. This always refers to the top-left corner of the sprite. If not specified, uses [DefaultOffset](cursor.md#defaultoffset).

```cs
public Microsoft.Xna.Framework.Point? Offset { get; set; }
```

##### Property Value

[Nullable](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1)<[Point](https://docs.monogame.net/api/Microsoft.Xna.Framework.Point.html)>

-----

#### Size

Size with which to draw the [Sprite](cursor.md#sprite), if different from the size of the sprite's [SourceRect](sprite.md#sourcerect).

```cs
public Microsoft.Xna.Framework.Point? Size { get; set; }
```

##### Property Value

[Nullable](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1)<[Point](https://docs.monogame.net/api/Microsoft.Xna.Framework.Point.html)>

-----

#### Sprite

The sprite to draw.

```cs
public StardewUI.Graphics.Sprite Sprite { get; set; }
```

##### Property Value

[Sprite](sprite.md)

-----

#### Tint

Tint color for the sprite. If not specified, uses [DefaultTint](cursor.md#defaulttint).

```cs
public Microsoft.Xna.Framework.Color? Tint { get; set; }
```

##### Property Value

[Nullable](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1)<[Color](https://docs.monogame.net/api/Microsoft.Xna.Framework.Color.html)>

-----

