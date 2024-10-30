---
title: LayoutParameters
description: Layout parameters for an IView.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Struct LayoutParameters

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Layout](index.md)  
Assembly: StardewUI.dll  

</div>

Layout parameters for an [IView](../iview.md).

```cs
public readonly struct LayoutParameters : 
    IEquatable<StardewUI.Layout.LayoutParameters>
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ [ValueType](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype) ⇦ LayoutParameters

**Implements**  
[IEquatable](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1)<[LayoutParameters](layoutparameters.md)>

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [LayoutParameters()](#layoutparameters) | Initializes a new [LayoutParameters](layoutparameters.md) with default layout settings. | 

### Properties

 | Name | Description |
| --- | --- |
| [Height](#height) | The vertical height/layout method. | 
| [MaxHeight](#maxheight) | Maximum height allowed. | 
| [MaxWidth](#maxwidth) | Maximum width allowed. | 
| [MinHeight](#minheight) | Minimum height to occupy. | 
| [MinWidth](#minwidth) | Minimum width to occupy. | 
| [Width](#width) | The horizontal width/layout method. | 

### Methods

 | Name | Description |
| --- | --- |
| [AutoRow()](#autorow) | Creates a [LayoutParameters](layoutparameters.md) that stretches to the available horizontal width, fits the content height, and has no other constraints. Typically used for rows in a vertical layout. | 
| [Equals(Object)](#equalsobject) | <span class="muted" markdown>(Overrides [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object).`Equals(Object)`)</span> | 
| [Equals(LayoutParameters)](#equalslayoutparameters) |  | 
| [Fill()](#fill) | Creates a [LayoutParameters](layoutparameters.md) that stretches to the full available width and height. | 
| [FitContent()](#fitcontent) | Creates a [LayoutParameters](layoutparameters.md) that tracks content width and height, and has no other constraints. | 
| [FixedSize(Point)](#fixedsizepoint) | Creates a [LayoutParameters](layoutparameters.md) with fixed dimensions, and no other constraints. | 
| [FixedSize(Single, Single)](#fixedsizefloat-float) | Creates a [LayoutParameters](layoutparameters.md) with fixed dimensions, and no other constraints. | 
| [GetHashCode()](#gethashcode) | <span class="muted" markdown>(Overrides [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object).`GetHashCode()`)</span> | 
| [GetLimits(Vector2)](#getlimitsvector2) | Determines the effective content size limits. | 
| [Resolve(Vector2, Func&lt;Vector2&gt;)](#resolvevector2-funcvector2) | Resolves the actual size for the current [LayoutParameters](layoutparameters.md). | 
| [ToString()](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype.tostring) | <span class="muted" markdown>(Inherited from [ValueType](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype))</span> | 

## Details

### Constructors

#### LayoutParameters()

Initializes a new [LayoutParameters](layoutparameters.md) with default layout settings.

```cs
public LayoutParameters();
```

-----

### Properties

#### Height

The vertical height/layout method.

```cs
public StardewUI.Layout.Length Height { get; set; }
```

##### Property Value

[Length](length.md)

-----

#### MaxHeight

Maximum height allowed.

```cs
public float? MaxHeight { get; set; }
```

##### Property Value

[Nullable](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1)<[Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)>

##### Remarks

If specified, the [Y](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html#Microsoft_Xna_Framework_Vector2) component of a view's content size should never exceed this value, regardless of how the [Height](layoutparameters.md#height) is configured.

-----

#### MaxWidth

Maximum width allowed.

```cs
public float? MaxWidth { get; set; }
```

##### Property Value

[Nullable](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1)<[Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)>

##### Remarks

If specified, the [X](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html#Microsoft_Xna_Framework_Vector2) component of a view's content size should never exceed this value, regardless of how the [Width](layoutparameters.md#width) is configured.

-----

#### MinHeight

Minimum height to occupy.

```cs
public float? MinHeight { get; set; }
```

##### Property Value

[Nullable](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1)<[Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)>

##### Remarks

If specified, the [Y](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html#Microsoft_Xna_Framework_Vector2) component of a view's content size will always be at least this value, regardless of how the [Height](layoutparameters.md#height) is configured. Typically, minimum sizes are only used with [Content](lengthtype.md#content) if there might be very little content. If a [MaxHeight](layoutparameters.md#maxheight) is also specified and is smaller than the `MinHeight`, then `MaxHeight` takes precedence.

-----

#### MinWidth

Minimum width to occupy.

```cs
public float? MinWidth { get; set; }
```

##### Property Value

[Nullable](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1)<[Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)>

##### Remarks

If specified, the [X](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html#Microsoft_Xna_Framework_Vector2) component of a view's content size will always be at least this value, regardless of how the [Width](layoutparameters.md#width) is configured. Typically, minimum sizes are only used with [Content](lengthtype.md#content) if there might be very little content. If a [MaxWidth](layoutparameters.md#maxwidth) is also specified and is smaller than the `MinWidth`, then `MaxWidth` takes precedence.

-----

#### Width

The horizontal width/layout method.

```cs
public StardewUI.Layout.Length Width { get; set; }
```

##### Property Value

[Length](length.md)

-----

### Methods

#### AutoRow()

Creates a [LayoutParameters](layoutparameters.md) that stretches to the available horizontal width, fits the content height, and has no other constraints. Typically used for rows in a vertical layout.

```cs
public static StardewUI.Layout.LayoutParameters AutoRow();
```

##### Returns

[LayoutParameters](layoutparameters.md)

-----

#### Equals(Object)



```cs
public override bool Equals(System.Object obj);
```

##### Parameters

**`obj`** &nbsp; [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object)

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

##### Remarks

Overrides the default implementation to avoid using reflection on every frame during dirty checks.

-----

#### Equals(LayoutParameters)



```cs
public bool Equals(StardewUI.Layout.LayoutParameters other);
```

##### Parameters

**`other`** &nbsp; [LayoutParameters](layoutparameters.md)

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

-----

#### Fill()

Creates a [LayoutParameters](layoutparameters.md) that stretches to the full available width and height.

```cs
public static StardewUI.Layout.LayoutParameters Fill();
```

##### Returns

[LayoutParameters](layoutparameters.md)

-----

#### FitContent()

Creates a [LayoutParameters](layoutparameters.md) that tracks content width and height, and has no other constraints.

```cs
public static StardewUI.Layout.LayoutParameters FitContent();
```

##### Returns

[LayoutParameters](layoutparameters.md)

-----

#### FixedSize(Point)

Creates a [LayoutParameters](layoutparameters.md) with fixed dimensions, and no other constraints.

```cs
public static StardewUI.Layout.LayoutParameters FixedSize(Microsoft.Xna.Framework.Point size);
```

##### Parameters

**`size`** &nbsp; [Point](https://docs.monogame.net/api/Microsoft.Xna.Framework.Point.html)  
The layout size, in pixels.

##### Returns

[LayoutParameters](layoutparameters.md)

-----

#### FixedSize(float, float)

Creates a [LayoutParameters](layoutparameters.md) with fixed dimensions, and no other constraints.

```cs
public static StardewUI.Layout.LayoutParameters FixedSize(float width, float height);
```

##### Parameters

**`width`** &nbsp; [Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)  
The layout width, in pixels.

**`height`** &nbsp; [Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)  
The layout height, in pixels.

##### Returns

[LayoutParameters](layoutparameters.md)

-----

#### GetHashCode()



```cs
public override int GetHashCode();
```

##### Returns

[Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)

-----

#### GetLimits(Vector2)

Determines the effective content size limits.

```cs
public Microsoft.Xna.Framework.Vector2 GetLimits(Microsoft.Xna.Framework.Vector2 availableSize);
```

##### Parameters

**`availableSize`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
The available size in the container/parent.

##### Returns

[Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)

  The size (equal to or smaller than `availableSize`) that can be allocated to content.

##### Remarks

Limits are not the same as the actual size coming from a [Resolve(Vector2, Func&lt;Vector2&gt;)](layoutparameters.md#resolvevector2-funcvector2); they provide a maximum width and/or height in the event that one or both dimensions are set to [Content](lengthtype.md#content). In these cases, the caller usually wants the "constraints" - e.g. a text block with fixed width but variable height needs to know that width before it can determine the actual height. 

 Implementations of [View](../view.md) will typically obtain the limits in their [OnMeasure(Vector2)](../view.md#onmeasurevector2)  method in order to perform internal/child layout, and determine the content size for [Resolve(Vector2, Func&lt;Vector2&gt;)](layoutparameters.md#resolvevector2-funcvector2).

-----

#### Resolve(Vector2, Func&lt;Vector2&gt;)

Resolves the actual size for the current [LayoutParameters](layoutparameters.md).

```cs
public Microsoft.Xna.Framework.Vector2 Resolve(Microsoft.Xna.Framework.Vector2 availableSize, Func<Microsoft.Xna.Framework.Vector2> getContentSize);
```

##### Parameters

**`availableSize`** &nbsp; [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)  
The available size in the container/parent.

**`getContentSize`** &nbsp; [Func](https://learn.microsoft.com/en-us/dotnet/api/system.func-1)<[Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)>  
Function to compute the inner content size based on limits obtained from [GetLimits(Vector2)](layoutparameters.md#getlimitsvector2); will only be invoked if it is required for the current layout configuration, i.e. if one or both dimensions are set to fit content.

##### Returns

[Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html)

-----

