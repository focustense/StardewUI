---
title: TraceProfile
description: A single profile in a trace.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class TraceProfile

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Diagnostics](index.md)  
Assembly: StardewUI.dll  

</div>

A single profile in a trace.

```cs
public class TraceProfile
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ TraceProfile

## Remarks

For speedscope purposes, this is always an "EventedProfile". StardewUI does not use sampled profiles.

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [TraceProfile(string, Int64)](#traceprofilestring-long) | A single profile in a trace. | 

### Properties

 | Name | Description |
| --- | --- |
| [EndValue](#endvalue) | The timestamp when tracing ended, in the specified [Unit](traceprofile.md#unit). | 
| [Events](#events) | The events recorded for this profile. | 
| [Name](#name) | Name of the profile. Used to identify the thread. | 
| [StartValue](#startvalue) | The timestamp when tracing was started, in the specified [Unit](traceprofile.md#unit). | 
| [Type](#type) | Discriminator for the profile type. In StardewUI, this is always `evented`. | 
| [Unit](#unit) | Unit of measurement for all time values. | 

## Details

### Constructors

#### TraceProfile(string, long)

A single profile in a trace.

```cs
public TraceProfile(string name, long startValue);
```

##### Parameters

**`name`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The [Name](traceprofile.md#name) of the profile, used to identify the thread.

**`startValue`** &nbsp; [Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64)  
The timestamp when tracing was started, in the specified [Unit](traceprofile.md#unit) (default: microseconds).

##### Remarks

For speedscope purposes, this is always an "EventedProfile". StardewUI does not use sampled profiles.

-----

### Properties

#### EndValue

The timestamp when tracing ended, in the specified [Unit](traceprofile.md#unit).

```cs
public long EndValue { get; set; }
```

##### Property Value

[Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64)

-----

#### Events

The events recorded for this profile.

```cs
public System.Collections.Generic.List<StardewUI.Framework.Diagnostics.TraceEvent> Events { get; }
```

##### Property Value

[List](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1)<[TraceEvent](traceevent.md)>

-----

#### Name

Name of the profile. Used to identify the thread.

```cs
public string Name { get; set; }
```

##### Property Value

[string](https://learn.microsoft.com/en-us/dotnet/api/system.string)

-----

#### StartValue

The timestamp when tracing was started, in the specified [Unit](traceprofile.md#unit).

```cs
public long StartValue { get; set; }
```

##### Property Value

[Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64)

-----

#### Type

Discriminator for the profile type. In StardewUI, this is always `evented`.

```cs
public string Type { get; }
```

##### Property Value

[string](https://learn.microsoft.com/en-us/dotnet/api/system.string)

-----

#### Unit

Unit of measurement for all time values.

```cs
public string Unit { get; }
```

##### Property Value

[string](https://learn.microsoft.com/en-us/dotnet/api/system.string)

##### Remarks

StardewUI's traces are always measured in `microseconds`.

-----

