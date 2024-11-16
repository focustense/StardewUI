---
title: TraceFile
description: Format of a trace file compatible with speedscope.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class TraceFile

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Diagnostics](index.md)  
Assembly: StardewUI.dll  

</div>

Format of a trace file compatible with speedscope.

```cs
public class TraceFile
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ TraceFile

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [TraceFile()](#tracefile) |  | 

### Properties

 | Name | Description |
| --- | --- |
| [CreationDate](#creationdate) | Exact date and time when the trace was started. | 
| [Exporter](#exporter) | The name and version of the exporting mod (i.e. StardewUI). | 
| [Profiles](#profiles) | List of profiles; each profile corresponds to a running thread. | 
| [SchemaUrl](#schemaurl) | JSON schema URL; required by the Speedscope web tool. | 
| [Shared](#shared) | Shared trace data, containing the frames or slice names. | 

### Methods

 | Name | Description |
| --- | --- |
| [CloseFrame(Int32)](#closeframeint) | Appends an event that closes a frame previously opened with [OpenFrame(string)](tracefile.md#openframestring). | 
| [OpenFrame(string)](#openframestring) | Adds a new [TraceFrame](traceframe.md) and [TraceEvent](traceevent.md) to open it, and returns the frame index to be used subsequently with [CloseFrame(Int32)](tracefile.md#closeframeint). | 

## Details

### Constructors

#### TraceFile()



```cs
public TraceFile();
```

-----

### Properties

#### CreationDate

Exact date and time when the trace was started.

```cs
public System.DateTime CreationDate { get; }
```

##### Property Value

[DateTime](https://learn.microsoft.com/en-us/dotnet/api/system.datetime)

-----

#### Exporter

The name and version of the exporting mod (i.e. StardewUI).

```cs
public string Exporter { get; set; }
```

##### Property Value

[string](https://learn.microsoft.com/en-us/dotnet/api/system.string)

-----

#### Profiles

List of profiles; each profile corresponds to a running thread.

```cs
public System.Collections.Generic.List<StardewUI.Framework.Diagnostics.TraceProfile> Profiles { get; }
```

##### Property Value

[List](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1)<[TraceProfile](traceprofile.md)>

-----

#### SchemaUrl

JSON schema URL; required by the Speedscope web tool.

```cs
public string SchemaUrl { get; }
```

##### Property Value

[string](https://learn.microsoft.com/en-us/dotnet/api/system.string)

-----

#### Shared

Shared trace data, containing the frames or slice names.

```cs
public StardewUI.Framework.Diagnostics.TraceShared Shared { get; }
```

##### Property Value

[TraceShared](traceshared.md)

-----

### Methods

#### CloseFrame(int)

Appends an event that closes a frame previously opened with [OpenFrame(string)](tracefile.md#openframestring).

```cs
public void CloseFrame(int frame);
```

##### Parameters

**`frame`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)  
The index of the tracked frame in [Frames](traceshared.md#frames).

-----

#### OpenFrame(string)

Adds a new [TraceFrame](traceframe.md) and [TraceEvent](traceevent.md) to open it, and returns the frame index to be used subsequently with [CloseFrame(Int32)](tracefile.md#closeframeint).

```cs
public int OpenFrame(string name);
```

##### Parameters

**`name`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
Name of the method or operation being traced.

##### Returns

[Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)

  The frame index, to be used with [CloseFrame(Int32)](tracefile.md#closeframeint) when the operation completes.

-----

