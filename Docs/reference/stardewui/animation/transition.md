---
title: Transition
description: Describes the transition behavior of a single property.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class Transition

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Animation](index.md)  
Assembly: StardewUI.dll  

</div>

Describes the transition behavior of a single property.

```cs
public record Transition : IEquatable<StardewUI.Animation.Transition>
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ Transition

**Implements**  
[IEquatable](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1)<[Transition](transition.md)>

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [Transition(TimeSpan, TimeSpan, Easing)](#transitiontimespan-timespan-easing) | Describes the transition behavior of a single property. | 
| [Transition(TimeSpan?, TimeSpan?, Easing)](#transitiontimespan-timespan-easing) | Initializes a new [Transition](transition.md) instance. | 

### Fields

 | Name | Description |
| --- | --- |
| [Default](#default) | A [Transition](transition.md) instance with all values set to their defaults. | 
| [DefaultDelay](#defaultdelay) | Default delay (zero) for transitions not specifying an explicit delay. | 
| [DefaultDuration](#defaultduration) | Default duration (1 second) for transitions not specifying an explicit duration. | 
| [DefaultEasing](#defaulteasing) | Default easing (linear) for transitions not specifying an explicit easing function. | 

### Properties

 | Name | Description |
| --- | --- |
| [Delay](#delay) | Delay during which to hold the current value before transitioning to the new value. | 
| [Duration](#duration) | Duration of the transition. | 
| [Easing](#easing) | Type of easing or acceleration curve for the transition. | 
| [EqualityContract](#equalitycontract) |  | 
| [TotalDuration](#totalduration) | The total duration of the transition, including both the animation itself and any pre-delay. | 

### Methods

 | Name | Description |
| --- | --- |
| [GetPosition(TimeSpan)](#getpositiontimespan) | Computes the interpolation position for this transition, given the time elapsed since the transition was first triggered. | 
| [Parse(string)](#parsestring) | Parses a [Transition](transition.md) value from a string value. | 
| [Parse(ReadOnlySpan&lt;Char&gt;)](#parsereadonlyspanchar) | Parses a [Transition](transition.md) value from a string value. | 
| [TryParse(ReadOnlySpan&lt;Char&gt;, Transition)](#tryparsereadonlyspanchar-transition) | Attempts to parse a [Transition](transition.md) value from a string value. | 

## Details

### Constructors

#### Transition(TimeSpan, TimeSpan, Easing)

Describes the transition behavior of a single property.

```cs
public Transition(System.TimeSpan Duration, System.TimeSpan Delay, StardewUI.Animation.Easing Easing);
```

##### Parameters

**`Duration`** &nbsp; [TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan)  
Duration of the transition.

**`Delay`** &nbsp; [TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan)  
Delay during which to hold the current value before transitioning to the new value.

**`Easing`** &nbsp; [Easing](easing.md)  
Type of easing or acceleration curve for the transition.

-----

#### Transition(TimeSpan?, TimeSpan?, Easing)

Initializes a new [Transition](transition.md) instance.

```cs
public Transition(System.TimeSpan? duration, System.TimeSpan? delay, StardewUI.Animation.Easing easing);
```

##### Parameters

**`duration`** &nbsp; [Nullable](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1)<[TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan)>  
Duration of the transition. If not specified, the transition will use the [DefaultDuration](transition.md#defaultduration).

**`delay`** &nbsp; [Nullable](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1)<[TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan)>  
Delay during which to hold the current value before transitioning to the new value. If not specified, the transition will use the [DefaultDelay](transition.md#defaultdelay).

**`easing`** &nbsp; [Easing](easing.md)  
Type of easing or acceleration curve for the transition. If not specified, the transition will use the [DefaultEasing](transition.md#defaulteasing).

-----

### Fields

#### Default

A [Transition](transition.md) instance with all values set to their defaults.

```cs
public static readonly StardewUI.Animation.Transition Default;
```

##### Field Value

[Transition](transition.md)

-----

#### DefaultDelay

Default delay (zero) for transitions not specifying an explicit delay.

```cs
public static readonly System.TimeSpan DefaultDelay;
```

##### Field Value

[TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan)

-----

#### DefaultDuration

Default duration (1 second) for transitions not specifying an explicit duration.

```cs
public static readonly System.TimeSpan DefaultDuration;
```

##### Field Value

[TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan)

-----

#### DefaultEasing

Default easing (linear) for transitions not specifying an explicit easing function.

```cs
public static readonly StardewUI.Animation.Easing DefaultEasing;
```

##### Field Value

[Easing](easing.md)

-----

### Properties

#### Delay

Delay during which to hold the current value before transitioning to the new value.

```cs
public System.TimeSpan Delay { get; set; }
```

##### Property Value

[TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan)

-----

#### Duration

Duration of the transition.

```cs
public System.TimeSpan Duration { get; set; }
```

##### Property Value

[TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan)

-----

#### Easing

Type of easing or acceleration curve for the transition.

```cs
public StardewUI.Animation.Easing Easing { get; set; }
```

##### Property Value

[Easing](easing.md)

-----

#### EqualityContract



```cs
protected System.Type EqualityContract { get; }
```

##### Property Value

[Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)

-----

#### TotalDuration

The total duration of the transition, including both the animation itself and any pre-delay.

```cs
public System.TimeSpan TotalDuration { get; }
```

##### Property Value

[TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan)

-----

### Methods

#### GetPosition(TimeSpan)

Computes the interpolation position for this transition, given the time elapsed since the transition was first triggered.

```cs
public float GetPosition(System.TimeSpan elapsed);
```

##### Parameters

**`elapsed`** &nbsp; [TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan)  
Time elapsed since the transition was initiated.

##### Returns

[Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)

  The interpolation amount or "y position" at the specified time.

##### Remarks

The value is independent of the type of property being transitioned, or how it is interpolated; the result is intended to be used as the `amount` argument to a [Lerp&lt;T&gt;](lerp-1.md) delegate.

-----

#### Parse(string)

Parses a [Transition](transition.md) value from a string value.

```cs
public static StardewUI.Animation.Transition Parse(string value);
```

##### Parameters

**`value`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The string value to parse.

##### Returns

[Transition](transition.md)

  The parsed transition.

-----

#### Parse(ReadOnlySpan&lt;Char&gt;)

Parses a [Transition](transition.md) value from a string value.

```cs
public static StardewUI.Animation.Transition Parse(ReadOnlySpan<System.Char> value);
```

##### Parameters

**`value`** &nbsp; [ReadOnlySpan](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<[Char](https://learn.microsoft.com/en-us/dotnet/api/system.char)>  
The string value to parse.

##### Returns

[Transition](transition.md)

  The parsed transition.

-----

#### TryParse(ReadOnlySpan&lt;Char&gt;, Transition)

Attempts to parse a [Transition](transition.md) value from a string value.

```cs
public static bool TryParse(ReadOnlySpan<System.Char> value, out StardewUI.Animation.Transition result);
```

##### Parameters

**`value`** &nbsp; [ReadOnlySpan](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<[Char](https://learn.microsoft.com/en-us/dotnet/api/system.char)>  
The string value to parse.

**`result`** &nbsp; [Transition](transition.md)  
The parsed transition, if successful; otherwise `null`.

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  `true` if the `value` was parsed successfully, otherwise `false`.

-----

