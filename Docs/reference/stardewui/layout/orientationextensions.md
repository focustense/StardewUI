---
title: OrientationExtensions
description: Helpers for working with Orientation.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class OrientationExtensions

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Layout](index.md)  
Assembly: StardewUI.dll  

</div>

Helpers for working with [Orientation](orientation.md).

```cs
public static class OrientationExtensions
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ OrientationExtensions

## Members

### Methods

 | Name | Description |
| --- | --- |
| [CreateVector(Orientation, Single)](#createvectororientation-float) | Creates a new [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html) with the oriented dimension set to a specified length and the other dimension set to zero. | 
| [Get(Orientation, Vector2)](#getorientation-vector2) | Gets the component of a vector along the orientation's axis. | 
| [Length(Orientation, LayoutParameters)](#lengthorientation-layoutparameters) | Gets the dimension setting of a layout along the orientation's axis. | 
| [Set(Orientation, Vector2, Single)](#setorientation-vector2-float) | Sets the component of a vector corresponding to the orientation's axis. | 
| [Swap(Orientation)](#swaporientation) | Gets the opposite/perpendicular orientation to a given orientation. | 
| [Update(Orientation, Vector2, Func&lt;Single, Single&gt;)](#updateorientation-vector2-funcsingle-single) |  | 

## Details

### Methods

#### CreateVector(Orientation, float)

Creates a new [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html) with the oriented dimension set to a specified length and the other dimension set to zero.

```cs
public static Microsoft.Xna.Framework.Vector2 CreateVector(StardewUI.Layout.Orientation orientation, float length);
```

##### Parameters

**`orientation`** &nbsp; [Orientation](orientation.md)  
The orientation.

**`length`** &nbsp; [Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)  
The length along the orientation axis.

##### Returns

[Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)

  A new [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html) whose length along the `orientation` axis is `length`.

-----

#### Get(Orientation, Vector2)

Gets the component of a vector along the orientation's axis.

```cs
public static float Get(StardewUI.Layout.Orientation orientation, Microsoft.Xna.Framework.Vector2 vec);
```

##### Parameters

**`orientation`** &nbsp; [Orientation](orientation.md)  
The orientation.

**`vec`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
Any vector value.

##### Returns

[Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)

  The vector's [X](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html#Microsoft_Xna_Framework_Vector2) component if [Horizontal](orientation.md#horizontal), or [Y](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html#Microsoft_Xna_Framework_Vector2) if [Vertical](orientation.md#vertical).

-----

#### Length(Orientation, LayoutParameters)

Gets the dimension setting of a layout along the orientation's axis.

```cs
public static StardewUI.Layout.Length Length(StardewUI.Layout.Orientation orientation, StardewUI.Layout.LayoutParameters layout);
```

##### Parameters

**`orientation`** &nbsp; [Orientation](orientation.md)  
The orientation.

**`layout`** &nbsp; [LayoutParameters](layoutparameters.md)  
Layout parameters to extract from.

##### Returns

[Length](length.md)

  The [Width](layoutparameters.md#width) of the specified `layout` if the orientation is [Horizontal](orientation.md#horizontal); [Height](layoutparameters.md#height) if [Vertical](orientation.md#vertical).

-----

#### Set(Orientation, Vector2, float)

Sets the component of a vector corresponding to the orientation's axis.

```cs
public static void Set(StardewUI.Layout.Orientation orientation, Microsoft.Xna.Framework.Vector2 vec, float value);
```

##### Parameters

**`orientation`** &nbsp; [Orientation](orientation.md)  
The orientation.

**`vec`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
Any vector value.

**`value`** &nbsp; [Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)  
The new value for the specified axis.

-----

#### Swap(Orientation)

Gets the opposite/perpendicular orientation to a given orientation.

```cs
public static StardewUI.Layout.Orientation Swap(StardewUI.Layout.Orientation orientation);
```

##### Parameters

**`orientation`** &nbsp; [Orientation](orientation.md)  
The orientation.

##### Returns

[Orientation](orientation.md)

-----

#### Update(Orientation, Vector2, Func&lt;Single, Single&gt;)



```cs
public static float Update(StardewUI.Layout.Orientation orientation, Microsoft.Xna.Framework.Vector2 vec, Func<System.Single, System.Single> select);
```

##### Parameters

**`orientation`** &nbsp; [Orientation](orientation.md)  
The orientation.

**`vec`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
Any vector value.

**`select`** &nbsp; [Func](https://learn.microsoft.com/en-us/dotnet/api/system.func-2)<[Single](https://learn.microsoft.com/en-us/dotnet/api/system.single), [Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)>  
A function that takes the previous value and returns the updated value.

##### Returns

[Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)

  The value after update.

-----

