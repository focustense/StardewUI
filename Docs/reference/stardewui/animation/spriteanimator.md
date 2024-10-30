---
title: SpriteAnimator
description: Animates the sprite of an Image, using equal duration for all frames in a list.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class SpriteAnimator

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Animation](index.md)  
Assembly: StardewUI.dll  

</div>

Animates the sprite of an [Image](../widgets/image.md), using equal duration for all frames in a list.

```cs
public class SpriteAnimator
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ SpriteAnimator

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [SpriteAnimator(Image)](#spriteanimatorimage) | Initializes a new instance of [SpriteAnimator](spriteanimator.md) that animates the sprite on a specified image. | 

### Properties

 | Name | Description |
| --- | --- |
| [FrameDuration](#frameduration) | Duration of each frame. | 
| [Frames](#frames) | Frames to animate through. | 
| [Paused](#paused) | Whether or not to pause animation. If `true`, the animator will hold at the current position and not progress until set to `false` again. | 
| [StartDelay](#startdelay) | Delay before advancing from the first frame to the next frames. | 

### Methods

 | Name | Description |
| --- | --- |
| [Reset()](#reset) | Resets the animation to the first frame, and waits any [StartDelay](spriteanimator.md#startdelay) required again. | 
| [Tick(TimeSpan)](#ticktimespan) | Advances the animation. | 

## Details

### Constructors

#### SpriteAnimator(Image)

Initializes a new instance of [SpriteAnimator](spriteanimator.md) that animates the sprite on a specified image.

```cs
public SpriteAnimator(StardewUI.Widgets.Image image);
```

##### Parameters

**`image`** &nbsp; [Image](../widgets/image.md)  
The image to animate.

-----

### Properties

#### FrameDuration

Duration of each frame.

```cs
public System.TimeSpan FrameDuration { get; set; }
```

##### Property Value

[TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan)

-----

#### Frames

Frames to animate through.

```cs
public System.Collections.Generic.IReadOnlyList<StardewUI.Graphics.Sprite> Frames { get; set; }
```

##### Property Value

[IReadOnlyList](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlylist-1)<[Sprite](../graphics/sprite.md)>

-----

#### Paused

Whether or not to pause animation. If `true`, the animator will hold at the current position and not progress until set to `false` again.

```cs
public bool Paused { get; set; }
```

##### Property Value

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

-----

#### StartDelay

Delay before advancing from the first frame to the next frames.

```cs
public System.TimeSpan StartDelay { get; set; }
```

##### Property Value

[TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan)

##### Remarks

Repeats on every loop, but only applies to the first frame of each loop.

-----

### Methods

#### Reset()

Resets the animation to the first frame, and waits any [StartDelay](spriteanimator.md#startdelay) required again.

```cs
public void Reset();
```

-----

#### Tick(TimeSpan)

Advances the animation.

```cs
public void Tick(System.TimeSpan elapsed);
```

##### Parameters

**`elapsed`** &nbsp; [TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan)  
The time elapsed since the previous tick.

-----

