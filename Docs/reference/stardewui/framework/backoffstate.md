---
title: BackoffState
description: State of an exponential backoff, e.g. as used in a BackoffTracker&lt;T&gt;.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class BackoffState

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework](index.md)  
Assembly: StardewUI.dll  

</div>

State of an exponential backoff, e.g. as used in a `StardewUI.Framework.BackoffTracker<T>`.

```cs
public class BackoffState
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ BackoffState

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [BackoffState(TimeSpan)](#backoffstatetimespan) | State of an exponential backoff, e.g. as used in a `StardewUI.Framework.BackoffTracker<T>`. | 

### Properties

 | Name | Description |
| --- | --- |
| [Duration](#duration) | The most recent duration waited/waiting for a retry. | 
| [Elapsed](#elapsed) | Time elapsed waiting for the current/next retry. | 

## Details

### Constructors

#### BackoffState(TimeSpan)

State of an exponential backoff, e.g. as used in a `StardewUI.Framework.BackoffTracker<T>`.

```cs
public BackoffState(System.TimeSpan initialDuration);
```

##### Parameters

**`initialDuration`** &nbsp; [TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan)  
The initial duration to wait for a retry.

-----

### Properties

#### Duration

The most recent duration waited/waiting for a retry.

```cs
public System.TimeSpan Duration { get; set; }
```

##### Property Value

[TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan)

-----

#### Elapsed

Time elapsed waiting for the current/next retry.

```cs
public System.TimeSpan Elapsed { get; set; }
```

##### Property Value

[TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan)

-----

