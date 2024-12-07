---
title: TraceShared
description: Data shared between profiles.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class TraceShared

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Diagnostics](index.md)  
Assembly: StardewUI.dll  

</div>

Data shared between profiles.

```cs
public class TraceShared
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ TraceShared

## Remarks

StardewUI only uses a single profile, but this structure is required by speedscope.

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [TraceShared()](#traceshared) |  | 

### Properties

 | Name | Description |
| --- | --- |
| [Frames](#frames) | The captured frames, or slices. | 

## Details

### Constructors

#### TraceShared()



```cs
public TraceShared();
```

-----

### Properties

#### Frames

The captured frames, or slices.

```cs
public System.Collections.Generic.List<StardewUI.Framework.Diagnostics.TraceFrame> Frames { get; }
```

##### Property Value

[List](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1)<[TraceFrame](traceframe.md)>

-----

