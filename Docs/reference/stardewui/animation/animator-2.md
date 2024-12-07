---
title: Animator&lt;T, V&gt;
description: Animates a single property of a single class.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class Animator&lt;T, V&gt;

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Animation](index.md)  
Assembly: StardewUI.dll  

</div>

Animates a single property of a single class.

```cs
public class Animator<T, V>
```

### Type Parameters

**`T`**  
The target class that will receive the animation.

**`V`**  
The type of value belonging to `T` that should be animated.


**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ Animator&lt;T, V&gt;

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [Animator&lt;T, V&gt;(T, Func&lt;T, V&gt;, Lerp&lt;V&gt;, Action&lt;T, V&gt;)](#animatort-vt-funct-v-lerpv-actiont-v) | Initializes a new [Animator&lt;T, V&gt;](animator-2.md). | 

### Properties

 | Name | Description |
| --- | --- |
| [AutoReverse](#autoreverse) | Whether to automatically start playing in reverse after reaching the end. | 
| [CurrentAnimation](#currentanimation) | The current animation, if any, started by [Start(Animation&lt;V&gt;)](animator-2.md#startanimationv) or any `Start` overloads. | 
| [IsReversing](#isreversing) | Gets whether or not the animator is currently animating in [Reverse()](animator-2.md#reverse). | 
| [Loop](#loop) | Whether or not the animation should automatically loop back to the beginning when finished. | 
| [Paused](#paused) | Whether or not to pause animation. If `true`, the animator will hold at the current position and not progress until set to `false` again. Does not affect the [CurrentAnimation](animator-2.md#currentanimation). | 

### Methods

 | Name | Description |
| --- | --- |
| [Forward()](#forward) | Causes the animator to animate in the forward direction toward animation's [EndValue](animation-1.md#endvalue). | 
| [Reset()](#reset) | Jumps to the first frame of the current animation, or the last frame if [IsReversing](animator-2.md#isreversing) is `true`. | 
| [Reverse()](#reverse) | Reverses the current animation, so that it gradually returns to the animation's [StartValue](animation-1.md#startvalue). | 
| [Start(Animation&lt;V&gt;)](#startanimationv) | Starts a new animation. | 
| [Start(V, V, TimeSpan?)](#startv-v-timespan) | Starts a new animation using the specified start/end values and duration. | 
| [Start(V, TimeSpan)](#startv-timespan) | Starts a new animation that begins at the current value and ends at the specified value after the specified duration. | 
| [Stop()](#stop) | Completely stops animating, removing the [CurrentAnimation](animator-2.md#currentanimation) and resetting animation state such as [Reverse()](animator-2.md#reverse) and [Paused](animator-2.md#paused). | 
| [Tick(TimeSpan)](#ticktimespan) | Continues animating in the current direction. | 

## Details

### Constructors

#### Animator&lt;T, V&gt;(T, Func&lt;T, V&gt;, Lerp&lt;V&gt;, Action&lt;T, V&gt;)

Initializes a new [Animator&lt;T, V&gt;](animator-2.md).

```cs
public Animator<T, V>(T target, Func<T, V> getValue, StardewUI.Animation.Lerp<V> lerpValue, Action<T, V> setValue);
```

##### Parameters

**`target`** &nbsp; T  
The object whose property will be animated.

**`getValue`** &nbsp; [Func&lt;T, V&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2)  
Function to get the current value. Used for animations that don't explicit specify a start value, e.g. when using the [Start(Animation&lt;V&gt;)](animator-2.md#startanimationv) overload.

**`lerpValue`** &nbsp; [Lerp&lt;V&gt;](lerp-1.md)  
Function to linearly interpolate between the start and end values.

**`setValue`** &nbsp; [Action&lt;T, V&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-2)  
Delegate to set the value on the `target`.

-----

### Properties

#### AutoReverse

Whether to automatically start playing in reverse after reaching the end.

```cs
public bool AutoReverse { get; set; }
```

##### Property Value

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

-----

#### CurrentAnimation

The current animation, if any, started by [Start(Animation&lt;V&gt;)](animator-2.md#startanimationv) or any `Start` overloads.

```cs
public StardewUI.Animation.Animation<V> CurrentAnimation { get; private set; }
```

##### Property Value

[Animation&lt;V&gt;](animation-1.md)

-----

#### IsReversing

Gets whether or not the animator is currently animating in [Reverse()](animator-2.md#reverse).

```cs
public bool IsReversing { get; private set; }
```

##### Property Value

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

-----

#### Loop

Whether or not the animation should automatically loop back to the beginning when finished.

```cs
public bool Loop { get; set; }
```

##### Property Value

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

-----

#### Paused

Whether or not to pause animation. If `true`, the animator will hold at the current position and not progress until set to `false` again. Does not affect the [CurrentAnimation](animator-2.md#currentanimation).

```cs
public bool Paused { get; set; }
```

##### Property Value

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

-----

### Methods

#### Forward()

Causes the animator to animate in the forward direction toward animation's [EndValue](animation-1.md#endvalue).

```cs
public void Forward();
```

##### Remarks

Does not restart the animation; if the animator is not reversed, then calling this has no effect.

-----

#### Reset()

Jumps to the first frame of the current animation, or the last frame if [IsReversing](animator-2.md#isreversing) is `true`.

```cs
public void Reset();
```

##### Remarks

Has no effect unless [CurrentAnimation](animator-2.md#currentanimation) has been set by a previous call to one of the [Start(Animation&lt;V&gt;)](animator-2.md#startanimationv) overloads.

-----

#### Reverse()

Reverses the current animation, so that it gradually returns to the animation's [StartValue](animation-1.md#startvalue).

```cs
public void Reverse();
```

##### Remarks

Calling [Reverse()](animator-2.md#reverse) is different from starting a new animation with reversed start and end values; specifically, it will follow the timeline/curve backward from the current progress. If only 1/4 second of a 1-second animation elapsed in the forward direction, then the reverse animation will also only take 1/4 second.

-----

#### Start(Animation&lt;V&gt;)

Starts a new animation.

```cs
public void Start(StardewUI.Animation.Animation<V> animation);
```

##### Parameters

**`animation`** &nbsp; [Animation&lt;V&gt;](animation-1.md)  
The animation settings.

-----

#### Start(V, V, TimeSpan?)

Starts a new animation using the specified start/end values and duration.

```cs
public void Start(V startValue, V endValue, System.TimeSpan? duration);
```

##### Parameters

**`startValue`** &nbsp; V  
The initial value of the animation property. This will take effect immediately, even if it is far away from the current value; i.e. it may cause "jumps".

**`endValue`** &nbsp; V  
The final value to be reached once the `duration` ends.

**`duration`** &nbsp; [Nullable](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1)<[TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan)>  
Duration of the animation; defaults to 1 second if not specified.

-----

#### Start(V, TimeSpan)

Starts a new animation that begins at the current value and ends at the specified value after the specified duration.

```cs
public void Start(V endValue, System.TimeSpan duration);
```

##### Parameters

**`endValue`** &nbsp; V  
The final value to be reached once the `duration` ends.

**`duration`** &nbsp; [TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan)  
Duration of the animation; defaults to 1 second if not specified.

-----

#### Stop()

Completely stops animating, removing the [CurrentAnimation](animator-2.md#currentanimation) and resetting animation state such as [Reverse()](animator-2.md#reverse) and [Paused](animator-2.md#paused).

```cs
public void Stop();
```

##### Remarks

This tries to put the animator in the same state it was in when first created. To preserve the current animation but pause progress and be able to resume later, set [Paused](animator-2.md#paused) instead. 

 Calling this does **not** reset the animated object to the animation's starting value. To do this, call [Reset()](animator-2.md#reset) before calling [Stop()](animator-2.md#stop) (not after, as [Reset()](animator-2.md#reset) has no effect once the [CurrentAnimation](animator-2.md#currentanimation) is cleared).

-----

#### Tick(TimeSpan)

Continues animating in the current direction.

```cs
public void Tick(System.TimeSpan elapsed);
```

##### Parameters

**`elapsed`** &nbsp; [TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan)  
Time elapsed since last tick.

-----

