---
title: Trace
description: Provides methods to toggle tracing and write to the current trace.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class Trace

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Diagnostics](index.md)  
Assembly: StardewUI.dll  

</div>

Provides methods to toggle tracing and write to the current trace.

```cs
public static class Trace
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ Trace

## Members

### Properties

 | Name | Description |
| --- | --- |
| [IsTracing](#istracing) | Gets or sets whether tracing is active. | 

### Methods

 | Name | Description |
| --- | --- |
| [Begin(string)](#beginstring) | Begins tracking a new operation (slice). | 
| [Begin(Func&lt;string&gt;, string)](#beginfuncstring-string) | Begins tracking a new operation (slice). | 
| [Begin(Object, string)](#beginobject-string) | Begins tracking a new operation (slice). | 

## Details

### Properties

#### IsTracing

Gets or sets whether tracing is active.

```cs
public static bool IsTracing { get; set; }
```

##### Property Value

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

##### Remarks

While inactive, all calls to any of the [Begin(string)](trace.md#beginstring) overloads are ignored and return `null`. If tracing is active, then setting this to `false` will cause the trace file to be written automatically.

-----

### Methods

#### Begin(string)

Begins tracking a new operation (slice).

```cs
public static System.IDisposable Begin(string name);
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

#### Begin(Func&lt;string&gt;, string)

Begins tracking a new operation (slice).

```cs
public static System.IDisposable Begin(Func<string> callerName, string memberName);
```

##### Parameters

**`callerName`** &nbsp; [Func](https://learn.microsoft.com/en-us/dotnet/api/system.func-1)<[string](https://learn.microsoft.com/en-us/dotnet/api/system.string)>  
Reference to the name (e.g. type name) of the object performing the traced operation.

**`memberName`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
Name of the member (method or property) about to begin execution.

##### Returns

[IDisposable](https://learn.microsoft.com/en-us/dotnet/api/system.idisposable)

  A disposable instance which, when disposed, stops tracking this operation and records the duration it took, for subsequent writing to the trace file.

##### Remarks

Slices must be disposed in the opposite order in which they are created, otherwise the final trace may be considered invalid.

-----

#### Begin(Object, string)

Begins tracking a new operation (slice).

```cs
public static System.IDisposable Begin(System.Object caller, string memberName);
```

##### Parameters

**`caller`** &nbsp; [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object)  
Reference to the object performing the traced operation.

**`memberName`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
Name of the member (method or property) about to begin execution.

##### Returns

[IDisposable](https://learn.microsoft.com/en-us/dotnet/api/system.idisposable)

  A disposable instance which, when disposed, stops tracking this operation and records the duration it took, for subsequent writing to the trace file.

##### Remarks

Slices must be disposed in the opposite order in which they are created, otherwise the final trace may be considered invalid.

-----

