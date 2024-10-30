---
title: SimpleRotationExtensions
description: Helper extensions for the SimpleRotation type.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class SimpleRotationExtensions

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Graphics](index.md)  
Assembly: StardewUI.dll  

</div>

Helper extensions for the [SimpleRotation](simplerotation.md) type.

```cs
public static class SimpleRotationExtensions
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ SimpleRotationExtensions

## Members

### Methods

 | Name | Description |
| --- | --- |
| [Angle(SimpleRotation)](#anglesimplerotation) | Gets the angle of a rotation, in radians. | 
| [IsQuarter(SimpleRotation)](#isquartersimplerotation) | Gets whether a rotation is a quarter turn. | 

## Details

### Methods

#### Angle(SimpleRotation)

Gets the angle of a rotation, in radians.

```cs
public static float Angle(StardewUI.Graphics.SimpleRotation rotation);
```

##### Parameters

**`rotation`** &nbsp; [SimpleRotation](simplerotation.md)  
The rotation type.

##### Returns

[Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)

  The angle of the rotation, in radians.

-----

#### IsQuarter(SimpleRotation)

Gets whether a rotation is a quarter turn.

```cs
public static bool IsQuarter(StardewUI.Graphics.SimpleRotation rotation);
```

##### Parameters

**`rotation`** &nbsp; [SimpleRotation](simplerotation.md)  
The rotation type.

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  `true` if the current instance is one of [QuarterClockwise](simplerotation.md#quarterclockwise) or [QuarterCounterclockwise](simplerotation.md#quartercounterclockwise); otherwise `false`.

##### Remarks

Often used to check whether to invert X/Y values in measurements.

-----

