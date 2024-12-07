---
title: SliceSettings
description: Additional nine-slice settings for dealing with certain "unique" structures.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class SliceSettings

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Graphics](index.md)  
Assembly: StardewUI.dll  

</div>

Additional nine-slice settings for dealing with certain "unique" structures.

```cs
[StardewUI.DuckType]
public record SliceSettings : IEquatable<StardewUI.Graphics.SliceSettings>
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ SliceSettings

**Implements**  
[IEquatable](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1)<[SliceSettings](slicesettings.md)>

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [SliceSettings(Int32?, SliceCenterPosition, Int32?, SliceCenterPosition, Single, Boolean)](#slicesettingsint-slicecenterposition-int-slicecenterposition-float-bool) | Additional nine-slice settings for dealing with certain "unique" structures. | 

### Properties

 | Name | Description |
| --- | --- |
| [CenterX](#centerx) | The X position to use for the horizontal center slices, or `null` to start where the left fixed edge ends. | 
| [CenterXPosition](#centerxposition) | Specifies whether the [CenterX](slicesettings.md#centerx) should be understood as the start position or the end position of the horizontal center slice. | 
| [CenterY](#centery) | The Y position to use for the vertical center slices, or `null` to start where the top fixed edge ends. | 
| [CenterYPosition](#centeryposition) | Specifies whether the [CenterY](slicesettings.md#centery) should be understood as the start position or the end position of the vertical center slice. | 
| [EdgesOnly](#edgesonly) | If `true`, then only the outer 8 edge segments should be drawn, and the 9th (horizontal and vertical middle, i.e. "background") segment will be ignored. | 
| [EqualityContract](#equalitycontract) |  | 
| [Scale](#scale) | Scale to apply to the slices themselves; for example, if a 16x16 source draws to a 64x64 target, and a scale of 2 is used, then a 2x3 border slice would draw as 16x24 (normal 8x16, multiplied by 2). | 

### Methods

 | Name | Description |
| --- | --- |
| [WithScale(Single)](#withscalefloat) | Creates a copy of this [SliceSettings](slicesettings.md) with a different scale. | 

## Details

### Constructors

#### SliceSettings(int?, SliceCenterPosition, int?, SliceCenterPosition, float, bool)

Additional nine-slice settings for dealing with certain "unique" structures.

```cs
public SliceSettings(int? CenterX, StardewUI.Graphics.SliceCenterPosition CenterXPosition, int? CenterY, StardewUI.Graphics.SliceCenterPosition CenterYPosition, float Scale, bool EdgesOnly);
```

##### Parameters

**`CenterX`** &nbsp; [Nullable](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1)<[Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)>  
The X position to use for the horizontal center slices, or `null` to start where the left fixed edge ends.

**`CenterXPosition`** &nbsp; [SliceCenterPosition](slicecenterposition.md)  
Specifies whether the [CenterX](slicesettings.md#centerx) should be understood as the start position or the end position of the horizontal center slice.

**`CenterY`** &nbsp; [Nullable](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1)<[Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)>  
The Y position to use for the vertical center slices, or `null` to start where the top fixed edge ends.

**`CenterYPosition`** &nbsp; [SliceCenterPosition](slicecenterposition.md)  
Specifies whether the [CenterY](slicesettings.md#centery) should be understood as the start position or the end position of the vertical center slice.

**`Scale`** &nbsp; [Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)  
Scale to apply to the slices themselves; for example, if a 16x16 source draws to a 64x64 target, and a scale of 2 is used, then a 2x3 border slice would draw as 16x24 (normal 8x16, multiplied by 2).

**`EdgesOnly`** &nbsp; [Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)  
If `true`, then only the outer 8 edge segments should be drawn, and the 9th (horizontal and vertical middle, i.e. "background") segment will be ignored.

-----

### Properties

#### CenterX

The X position to use for the horizontal center slices, or `null` to start where the left fixed edge ends.

```cs
public int? CenterX { get; set; }
```

##### Property Value

[Nullable](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1)<[Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)>

-----

#### CenterXPosition

Specifies whether the [CenterX](slicesettings.md#centerx) should be understood as the start position or the end position of the horizontal center slice.

```cs
public StardewUI.Graphics.SliceCenterPosition CenterXPosition { get; set; }
```

##### Property Value

[SliceCenterPosition](slicecenterposition.md)

-----

#### CenterY

The Y position to use for the vertical center slices, or `null` to start where the top fixed edge ends.

```cs
public int? CenterY { get; set; }
```

##### Property Value

[Nullable](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1)<[Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)>

-----

#### CenterYPosition

Specifies whether the [CenterY](slicesettings.md#centery) should be understood as the start position or the end position of the vertical center slice.

```cs
public StardewUI.Graphics.SliceCenterPosition CenterYPosition { get; set; }
```

##### Property Value

[SliceCenterPosition](slicecenterposition.md)

-----

#### EdgesOnly

If `true`, then only the outer 8 edge segments should be drawn, and the 9th (horizontal and vertical middle, i.e. "background") segment will be ignored.

```cs
public bool EdgesOnly { get; set; }
```

##### Property Value

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

-----

#### EqualityContract



```cs
protected System.Type EqualityContract { get; }
```

##### Property Value

[Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)

-----

#### Scale

Scale to apply to the slices themselves; for example, if a 16x16 source draws to a 64x64 target, and a scale of 2 is used, then a 2x3 border slice would draw as 16x24 (normal 8x16, multiplied by 2).

```cs
public float Scale { get; set; }
```

##### Property Value

[Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)

-----

### Methods

#### WithScale(float)

Creates a copy of this [SliceSettings](slicesettings.md) with a different scale.

```cs
public StardewUI.Graphics.SliceSettings WithScale(float newScale);
```

##### Parameters

**`newScale`** &nbsp; [Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)  
The scale to use.

##### Returns

[SliceSettings](slicesettings.md)

  A copy of this [SliceSettings](slicesettings.md) with its [Scale](slicesettings.md#scale) set to `newScale`.

-----

