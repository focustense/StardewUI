---
title: Hsv
description: A color in the HSV space.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class Hsv

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Graphics](index.md)  
Assembly: StardewUI.dll  

</div>

A color in the HSV space.

```cs
public record Hsv : IEquatable<StardewUI.Graphics.Hsv>
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ Hsv

**Implements**  
[IEquatable](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1)<[Hsv](hsv.md)>

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [Hsv(Int32, Single, Single)](#hsvint-float-float) | A color in the HSV space. | 

### Properties

 | Name | Description |
| --- | --- |
| [EqualityContract](#equalitycontract) |  | 
| [Hue](#hue) | The color [hue](https://en.wikipedia.org/wiki/Hue). | 
| [Saturation](#saturation) | The color [saturation](https://en.wikipedia.org/wiki/Colorfulness). | 
| [Value](#value) | The color value or [brightness](https://en.wikipedia.org/wiki/Brightness). | 

### Methods

 | Name | Description |
| --- | --- |
| [FromRgb(Color)](#fromrgbcolor) | Converts an RGB color to its HSV equivalent. | 
| [ToRgb(Single)](#torgbfloat) | Converts this color to its RGB equivalent. | 

## Details

### Constructors

#### Hsv(int, float, float)

A color in the HSV space.

```cs
public Hsv(int Hue, float Saturation, float Value);
```

##### Parameters

**`Hue`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)  
The color [hue](https://en.wikipedia.org/wiki/Hue).

**`Saturation`** &nbsp; [Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)  
The color [saturation](https://en.wikipedia.org/wiki/Colorfulness).

**`Value`** &nbsp; [Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)  
The color value or [brightness](https://en.wikipedia.org/wiki/Brightness).

-----

### Properties

#### EqualityContract



```cs
protected System.Type EqualityContract { get; }
```

##### Property Value

[Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)

-----

#### Hue

The color [hue](https://en.wikipedia.org/wiki/Hue).

```cs
public int Hue { get; set; }
```

##### Property Value

[Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)

-----

#### Saturation

The color [saturation](https://en.wikipedia.org/wiki/Colorfulness).

```cs
public float Saturation { get; set; }
```

##### Property Value

[Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)

-----

#### Value

The color value or [brightness](https://en.wikipedia.org/wiki/Brightness).

```cs
public float Value { get; set; }
```

##### Property Value

[Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)

-----

### Methods

#### FromRgb(Color)

Converts an RGB color to its HSV equivalent.

```cs
public static StardewUI.Graphics.Hsv FromRgb(Microsoft.Xna.Framework.Color color);
```

##### Parameters

**`color`** &nbsp; [Color](https://docs.monogame.net/api/Microsoft.Xna.Framework.Color.html)  
The RGB color.

##### Returns

[Hsv](hsv.md)

  The `color` converted to HSV components.

-----

#### ToRgb(float)

Converts this color to its RGB equivalent.

```cs
public Microsoft.Xna.Framework.Color ToRgb(float alpha);
```

##### Parameters

**`alpha`** &nbsp; [Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)  
Optional alpha component if not 1 (fully opaque).

##### Returns

[Color](https://docs.monogame.net/api/Microsoft.Xna.Framework.Color.html)

  The RGB color.

-----

