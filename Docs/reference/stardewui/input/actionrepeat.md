---
title: ActionRepeat
description: Configures the repeat rate of an action used in an ActionState&lt;T&gt;.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class ActionRepeat

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Input](index.md)  
Assembly: StardewUI.dll  

</div>

Configures the repeat rate of an action used in an [ActionState&lt;T&gt;](actionstate-1.md).

```cs
[StardewUI.DuckType]
public record ActionRepeat : IEquatable<StardewUI.Input.ActionRepeat>
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ ActionRepeat

**Implements**  
[IEquatable](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1)<[ActionRepeat](actionrepeat.md)>

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [ActionRepeat(TimeSpan, TimeSpan?)](#actionrepeattimespan-timespan) | Configures the repeat rate of an action used in an [ActionState&lt;T&gt;](actionstate-1.md). | 

### Fields

 | Name | Description |
| --- | --- |
| [Continuous](#continuous) | Configures an action to repeat continuously, i.e. to run again on every frame as long as the trigger keys are still held. | 
| [Default](#default) | Default repetition setting suitable for most UI scenarios. | 
| [None](#none) | Configures an action to never repeat, no matter how long the trigger keys are held. | 

### Properties

 | Name | Description |
| --- | --- |
| [EqualityContract](#equalitycontract) |  | 
| [InitialDelay](#initialdelay) | Initial delay after the first press, before any repetitions are allowed. | 
| [RepeatInterval](#repeatinterval) | The interval between repetitions of the action, while the key is held. | 

## Details

### Constructors

#### ActionRepeat(TimeSpan, TimeSpan?)

Configures the repeat rate of an action used in an [ActionState&lt;T&gt;](actionstate-1.md).

```cs
public ActionRepeat(System.TimeSpan RepeatInterval, System.TimeSpan? InitialDelay);
```

##### Parameters

**`RepeatInterval`** &nbsp; [TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan)  
The interval between repetitions of the action, while the key is held.

**`InitialDelay`** &nbsp; [Nullable](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1)<[TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan)>  
Initial delay after the first press, before any repetitions are allowed.

-----

### Fields

#### Continuous

Configures an action to repeat continuously, i.e. to run again on every frame as long as the trigger keys are still held.

```cs
public static readonly StardewUI.Input.ActionRepeat Continuous;
```

##### Field Value

[ActionRepeat](actionrepeat.md)

-----

#### Default

Default repetition setting suitable for most UI scenarios.

```cs
public static readonly StardewUI.Input.ActionRepeat Default;
```

##### Field Value

[ActionRepeat](actionrepeat.md)

##### Remarks

Not perfectly consistent (nor intended to be consistent) with vanilla game settings, which are all over the place depending on which key/button is being considered.

-----

#### None

Configures an action to never repeat, no matter how long the trigger keys are held.

```cs
public static readonly StardewUI.Input.ActionRepeat None;
```

##### Field Value

[ActionRepeat](actionrepeat.md)

-----

### Properties

#### EqualityContract



```cs
protected System.Type EqualityContract { get; }
```

##### Property Value

[Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)

-----

#### InitialDelay

Initial delay after the first press, before any repetitions are allowed.

```cs
public System.TimeSpan? InitialDelay { get; set; }
```

##### Property Value

[Nullable](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1)<[TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan)>

-----

#### RepeatInterval

The interval between repetitions of the action, while the key is held.

```cs
public System.TimeSpan RepeatInterval { get; set; }
```

##### Property Value

[TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan)

-----

