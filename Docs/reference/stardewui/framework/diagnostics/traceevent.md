---
title: TraceEvent
description: Defines a single trace event.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class TraceEvent

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Diagnostics](index.md)  
Assembly: StardewUI.dll  

</div>

Defines a single trace event.

```cs
public record TraceEvent : 
    IEquatable<StardewUI.Framework.Diagnostics.TraceEvent>
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ TraceEvent

**Implements**  
[IEquatable](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1)<[TraceEvent](traceevent.md)>

## Remarks

StardewUI uses only evented profiles, so the data is either for an `OpenFrameEvent` or `CloseFrameEvent`.

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [TraceEvent(Char, Int64, Int32)](#traceeventchar-long-int) | Defines a single trace event. | 

### Properties

 | Name | Description |
| --- | --- |
| [At](#at) | Time when the event was logged, in the profile's [Unit](traceprofile.md#unit) (i.e. in microseconds for any StardewUI event). | 
| [EqualityContract](#equalitycontract) |  | 
| [Frame](#frame) | Index into the [Frames](traceshared.md#frames) identifying which frame this event refers to. Used to correlate open and close events. | 
| [Type](#type) | Discriminator for the event type; `'O'` for open frame or `'C'` for close. | 

## Details

### Constructors

#### TraceEvent(char, long, int)

Defines a single trace event.

```cs
public TraceEvent(char Type, long At, int Frame);
```

##### Parameters

**`Type`** &nbsp; [Char](https://learn.microsoft.com/en-us/dotnet/api/system.char)  
Discriminator for the event type; `'O'` for open frame or `'C'` for close.

**`At`** &nbsp; [Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64)  
Time when the event was logged, in the profile's [Unit](traceprofile.md#unit) (i.e. in microseconds for any StardewUI event).

**`Frame`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)  
Index into the [Frames](traceshared.md#frames) identifying which frame this event refers to. Used to correlate open and close events.

##### Remarks

StardewUI uses only evented profiles, so the data is either for an `OpenFrameEvent` or `CloseFrameEvent`.

-----

### Properties

#### At

Time when the event was logged, in the profile's [Unit](traceprofile.md#unit) (i.e. in microseconds for any StardewUI event).

```cs
public long At { get; set; }
```

##### Property Value

[Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64)

-----

#### EqualityContract



```cs
protected System.Type EqualityContract { get; }
```

##### Property Value

[Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)

-----

#### Frame

Index into the [Frames](traceshared.md#frames) identifying which frame this event refers to. Used to correlate open and close events.

```cs
public int Frame { get; set; }
```

##### Property Value

[Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)

-----

#### Type

Discriminator for the event type; `'O'` for open frame or `'C'` for close.

```cs
public char Type { get; set; }
```

##### Property Value

[Char](https://learn.microsoft.com/en-us/dotnet/api/system.char)

-----

