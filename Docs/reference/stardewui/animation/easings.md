---
title: Easings
description: Common registration and lookup for easing functions.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class Easings

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Animation](index.md)  
Assembly: StardewUI.dll  

</div>

Common registration and lookup for easing functions.

```cs
public static class Easings
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ Easings

## Members

### Methods

 | Name | Description |
| --- | --- |
| [Add(string, Easing)](#addstring-easing) | Registers a new easing, if one with the same name is not already registered. | 
| [CubicBezier(Single, Single, Single, Single)](#cubicbezierfloat-float-float-float) | Creates a Cubic Bézier (AKA key spline) easing from two control points. | 
| [Named(string)](#namedstring) | Retrieves an easing function, given its registered name. | 
| [Parse(string)](#parsestring) | Parses an [Easing](easing.md) value from a string value. | 
| [Parse(ReadOnlySpan&lt;Char&gt;)](#parsereadonlyspanchar) | Parses an [Easing](easing.md) value from a string value. | 
| [TryParse(ReadOnlySpan&lt;Char&gt;, Easing)](#tryparsereadonlyspanchar-easing) | Attempts to parse an [Easing](easing.md) value from a string value. | 

## Details

### Methods

#### Add(string, Easing)

Registers a new easing, if one with the same name is not already registered.

```cs
public static void Add(string name, StardewUI.Animation.Easing easing);
```

##### Parameters

**`name`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The name of the easing function.

**`easing`** &nbsp; [Easing](easing.md)  
The easing function.

##### Remarks

If an easing with the specified `name` already exists, the call is ignored.

-----

#### CubicBezier(float, float, float, float)

Creates a Cubic Bézier (AKA key spline) easing from two control points.

```cs
public static StardewUI.Animation.Easing CubicBezier(float x1, float y1, float x2, float y2);
```

##### Parameters

**`x1`** &nbsp; [Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)  
The X value of the first control point.

**`y1`** &nbsp; [Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)  
The Y value of the first control point.

**`x2`** &nbsp; [Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)  
The X value of the second control point.

**`y2`** &nbsp; [Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)  
The Y value of the second control point.

##### Returns

[Easing](easing.md)

  The easing function described by the control points.

-----

#### Named(string)

Retrieves an easing function, given its registered name.

```cs
public static StardewUI.Animation.Easing Named(string name);
```

##### Parameters

**`name`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
An easing function name, such as `EaseOutCubic`.

##### Returns

[Easing](easing.md)

  The easing function registered with the specified `name`, or `null` if no such function was registered.

-----

#### Parse(string)

Parses an [Easing](easing.md) value from a string value.

```cs
public static StardewUI.Animation.Easing Parse(string value);
```

##### Parameters

**`value`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The string value to parse.

##### Returns

[Easing](easing.md)

  The parsed easing function.

-----

#### Parse(ReadOnlySpan&lt;Char&gt;)

Parses an [Easing](easing.md) value from a string value.

```cs
public static StardewUI.Animation.Easing Parse(ReadOnlySpan<System.Char> value);
```

##### Parameters

**`value`** &nbsp; [ReadOnlySpan](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<[Char](https://learn.microsoft.com/en-us/dotnet/api/system.char)>  
The string value to parse.

##### Returns

[Easing](easing.md)

  The parsed easing function.

-----

#### TryParse(ReadOnlySpan&lt;Char&gt;, Easing)

Attempts to parse an [Easing](easing.md) value from a string value.

```cs
public static bool TryParse(ReadOnlySpan<System.Char> value, out StardewUI.Animation.Easing result);
```

##### Parameters

**`value`** &nbsp; [ReadOnlySpan](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<[Char](https://learn.microsoft.com/en-us/dotnet/api/system.char)>  
The string value to parse.

**`result`** &nbsp; [Easing](easing.md)  
The parsed easing function, if successful; otherwise `null`.

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  `true` if the `value` was parsed successfully, otherwise `false`.

##### Remarks

Works with both named easings, and known easing functions like `CubicBezier`.

-----

