---
title: BackoffRule
description: Defines the rules for exponential backoff.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class BackoffRule

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework](index.md)  
Assembly: StardewUI.dll  

</div>

Defines the rules for exponential backoff.

```cs
public class BackoffRule
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ BackoffRule

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [BackoffRule(TimeSpan, TimeSpan, Single)](#backoffruletimespan-timespan-float) | Defines the rules for exponential backoff. | 

### Fields

 | Name | Description |
| --- | --- |
| [Default](#default) | Standard backoff rule deemed suitable for most types of UI retries. | 

### Properties

 | Name | Description |
| --- | --- |
| [InitialDuration](#initialduration) | Gets the duration to wait before the first retry attempt. | 

### Methods

 | Name | Description |
| --- | --- |
| [GetNextDuration(TimeSpan)](#getnextdurationtimespan) | Gets the duration to wait before the next retry attempt. | 

## Details

### Constructors

#### BackoffRule(TimeSpan, TimeSpan, float)

Defines the rules for exponential backoff.

```cs
public BackoffRule(System.TimeSpan initialDuration, System.TimeSpan maxDuration, float multiplier);
```

##### Parameters

**`initialDuration`** &nbsp; [TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan)  
Duration to wait before the first retry attempt.

**`maxDuration`** &nbsp; [TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan)  
Maximum duration to wait before any retry attempt; i.e. no matter how many retries have already occurred for a given key, it will not extend the duration any longer than this.

**`multiplier`** &nbsp; [Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)  
Amount to multiply the current duration on each subsequent retry, starting from `initialDuration` and going no higher than `maxDuration`.

-----

### Fields

#### Default

Standard backoff rule deemed suitable for most types of UI retries.

```cs
public static readonly StardewUI.Framework.BackoffRule Default;
```

##### Field Value

[BackoffRule](backoffrule.md)

##### Remarks

Uses an initial delay of 50 ms, maximum delay of 5 s, and multiplier of 4.

-----

### Properties

#### InitialDuration

Gets the duration to wait before the first retry attempt.

```cs
public System.TimeSpan InitialDuration { get; }
```

##### Property Value

[TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan)

-----

### Methods

#### GetNextDuration(TimeSpan)

Gets the duration to wait before the next retry attempt.

```cs
public System.TimeSpan GetNextDuration(System.TimeSpan previousDuration);
```

##### Parameters

**`previousDuration`** &nbsp; [TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan)  
The wait duration that was used on the previous attempt.

##### Returns

[TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan)

-----

