---
title: ITraceWriter
description: Abstract output writer for performance traces.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Interface ITraceWriter

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Diagnostics](index.md)  
Assembly: StardewUI.dll  

</div>

Abstract output writer for performance traces.

```cs
public interface ITraceWriter
```

## Remarks

This is an internal helper meant for use by the [Trace](trace.md) utility and should not be used directly by mods.

## Members

### Properties

 | Name | Description |
| --- | --- |
| [IsTracing](#istracing) | Whether or not a trace has been started, and not yet ended. | 

### Methods

 | Name | Description |
| --- | --- |
| [BeginSlice(string)](#beginslicestring) | Begins tracking a new operation (slice). | 
| [BeginTrace()](#begintrace) | Starts a new trace. | 
| [EndTrace()](#endtrace) | Ends the current trace and writes all recorded data to a new trace file in the output directory. | 

## Details

### Properties

#### IsTracing

Whether or not a trace has been started, and not yet ended.

```cs
bool IsTracing { get; }
```

##### Property Value

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

-----

### Methods

#### BeginSlice(string)

Begins tracking a new operation (slice).

```cs
System.IDisposable BeginSlice(string name);
```

##### Parameters

**`name`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The name that should appear in the trace log/visualization.

##### Returns

[IDisposable](https://learn.microsoft.com/en-us/dotnet/api/system.idisposable)

  A disposable instance which, when disposed, stops tracking this operation and records the duration it took, for subsequent writing to the trace file.

##### Remarks

Slices must be disposed in the opposite order in which they are created, otherwise the final trace may be considered invalid.

-----

#### BeginTrace()

Starts a new trace.

```cs
void BeginTrace();
```

-----

#### EndTrace()

Ends the current trace and writes all recorded data to a new trace file in the output directory.

```cs
void EndTrace();
```

-----

