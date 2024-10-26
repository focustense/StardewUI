---
title: Length
description: Specifies how to calculate the length of a single dimension (width or height).
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Struct Length

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Layout](index.md)  
Assembly: StardewUI.dll  

</div>

Specifies how to calculate the length of a single dimension (width or height).

```cs
public readonly struct Length : IEquatable<StardewUI.Layout.Length>
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ [ValueType](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype) ⇦ Length

**Implements**  
[IEquatable](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1)<[Length](length.md)>

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [Length(LengthType, Single)](#lengthlengthtype-float) | Specifies how to calculate the length of a single dimension (width or height). | 

### Properties

 | Name | Description |
| --- | --- |
| [Type](#type) | Specifies how to interpret the [Value](length.md#value). | 
| [Value](#value) | The dimension value, with behavior determined by [Type](length.md#type). | 

### Methods

 | Name | Description |
| --- | --- |
| [Content()](#content) | Creates a new [Length](length.md) having [Content](lengthtype.md#content). | 
| [Percent(Single)](#percentfloat) | Creates a new [Length](length.md) having [Percent](lengthtype.md#percent) and the specified percent size. | 
| [Px(Single)](#pxfloat) | Creates a new [Length](length.md) having [Px](lengthtype.md#px) and the specified pixel size. | 
| [Resolve(Single, Func&lt;Single&gt;)](#resolvefloat-funcsingle) | Resolves an actual (pixel) length. | 
| [Stretch()](#stretch) | Creates a new [Length](length.md) having [Stretch](lengthtype.md#stretch). | 

## Details

### Constructors

#### Length(LengthType, float)

Specifies how to calculate the length of a single dimension (width or height).

```cs
public Length(StardewUI.Layout.LengthType Type, float Value);
```

##### Parameters

**`Type`** &nbsp; [LengthType](lengthtype.md)  
Specifies how to interpret the [Value](length.md#value).

**`Value`** &nbsp; [Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)  
The dimension value, with behavior determined by [Type](length.md#type).

-----

### Properties

#### Type

Specifies how to interpret the [Value](length.md#value).

```cs
public StardewUI.Layout.LengthType Type { get; set; }
```

##### Property Value

[LengthType](lengthtype.md)

-----

#### Value

The dimension value, with behavior determined by [Type](length.md#type).

```cs
public float Value { get; set; }
```

##### Property Value

[Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)

-----

### Methods

#### Content()

Creates a new [Length](length.md) having [Content](lengthtype.md#content).

```cs
public static StardewUI.Layout.Length Content();
```

##### Returns

[Length](length.md)

-----

#### Percent(float)

Creates a new [Length](length.md) having [Percent](lengthtype.md#percent) and the specified percent size.

```cs
public static StardewUI.Layout.Length Percent(float value);
```

##### Parameters

**`value`** &nbsp; [Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)  
The length in 100-based percent units (e.g. `50.0` is 50%).

##### Returns

[Length](length.md)

-----

#### Px(float)

Creates a new [Length](length.md) having [Px](lengthtype.md#px) and the specified pixel size.

```cs
public static StardewUI.Layout.Length Px(float value);
```

##### Parameters

**`value`** &nbsp; [Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)  
The length in pixels.

##### Returns

[Length](length.md)

-----

#### Resolve(float, Func&lt;Single&gt;)

Resolves an actual (pixel) length.

```cs
public float Resolve(float availableLength, Func<System.Single> getContentLength);
```

##### Parameters

**`availableLength`** &nbsp; [Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)  
The remaining space available.

**`getContentLength`** &nbsp; [Func](https://learn.microsoft.com/en-us/dotnet/api/system.func-1)<[Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)>  
A function to get the length of inner content. Will not be called unless the [LengthType](lengthtype.md) requires it.

##### Returns

[Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)

##### Remarks

This is a convenience method for common layout scenarios, where content length is relatively simple to compute. Its use is optional; complex widgets can use any means they prefer to compute [ContentSize](../view.md#contentsize). 

 The result is intentionally not constrained to `availableLength`, which is only used for the [Stretch](lengthtype.md#stretch) method. This allows callers to check if the bounds were exceeded (e.g. to render a scroll bar, ellipsis, etc.) before clamping it.

-----

#### Stretch()

Creates a new [Length](length.md) having [Stretch](lengthtype.md#stretch).

```cs
public static StardewUI.Layout.Length Stretch();
```

##### Returns

[Length](length.md)

-----

