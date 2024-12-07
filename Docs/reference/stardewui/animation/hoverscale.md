---
title: HoverScale
description: Standalone scaling behavior that can be attached to any Image, causing it to scale up to a specified amount when hovered by the pointer.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class HoverScale

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Animation](index.md)  
Assembly: StardewUI.dll  

</div>

Standalone scaling behavior that can be attached to any [Image](../widgets/image.md), causing it to scale up to a specified amount when hovered by the pointer.

```cs
public class HoverScale
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ HoverScale

## Members

### Methods

 | Name | Description |
| --- | --- |
| [Attach(Image, Single, TimeSpan?)](#attachimage-float-timespan) | Attaches a new hover behavior to an image. | 

## Details

### Methods

#### Attach(Image, float, TimeSpan?)

Attaches a new hover behavior to an image.

```cs
public static void Attach(StardewUI.Widgets.Image image, float maxScale, System.TimeSpan? duration);
```

##### Parameters

**`image`** &nbsp; [Image](../widgets/image.md)  
The image that will receive the hover behavior.

**`maxScale`** &nbsp; [Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)  
Target scale at the end of the animation; generally a number > 1.

**`duration`** &nbsp; [Nullable](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1)<[TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan)>  
Duration of the animation; if not specified, defaults to 80 ms.

-----

