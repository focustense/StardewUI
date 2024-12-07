---
title: ContextUpdateTracker
description: Tracks context instances that already had updates dispatched this frame, to prevent duplication.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class ContextUpdateTracker

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Binding](index.md)  
Assembly: StardewUI.dll  

</div>

Tracks context instances that already had updates dispatched this frame, to prevent duplication.

```cs
public class ContextUpdateTracker
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ ContextUpdateTracker

## Remarks

Used by the [ContextUpdatingNodeDecorator](contextupdatingnodedecorator.md).

## Members

### Fields

 | Name | Description |
| --- | --- |
| [Instance](#instance) | Global instance for entire framework. | 

### Methods

 | Name | Description |
| --- | --- |
| [Reset()](#reset) | Resets all state; to be called at the beginning of each frame. | 
| [TrackUpdate(Object)](#trackupdateobject) | Tracks the update of a context instance so that [WasAlreadyUpdated(Object)](contextupdatetracker.md#wasalreadyupdatedobject) returns `true` for the given `contextData` until [Reset()](contextupdatetracker.md#reset) is called. | 
| [WasAlreadyUpdated(Object)](#wasalreadyupdatedobject) | Checks if a context instance already received an update tick dispatch for the current frame. | 

## Details

### Fields

#### Instance

Global instance for entire framework.

```cs
public static readonly StardewUI.Framework.Binding.ContextUpdateTracker Instance;
```

##### Field Value

[ContextUpdateTracker](contextupdatetracker.md)

##### Remarks

Entire view trees may share the same context data; this is entirely up to the client(s). Therefore, the same tracker instance must be shared by all nodes in order to prevent duplication. 

 The instance is expected to be reset in the `ModEntry` or similar entry point.

-----

### Methods

#### Reset()

Resets all state; to be called at the beginning of each frame.

```cs
public void Reset();
```

-----

#### TrackUpdate(Object)

Tracks the update of a context instance so that [WasAlreadyUpdated(Object)](contextupdatetracker.md#wasalreadyupdatedobject) returns `true` for the given `contextData` until [Reset()](contextupdatetracker.md#reset) is called.

```cs
public void TrackUpdate(System.Object contextData);
```

##### Parameters

**`contextData`** &nbsp; [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object)  
The context that was updated.

-----

#### WasAlreadyUpdated(Object)

Checks if a context instance already received an update tick dispatch for the current frame.

```cs
public bool WasAlreadyUpdated(System.Object contextData);
```

##### Parameters

**`contextData`** &nbsp; [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object)  
The context that may have been previously updated.

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  `true` if the `contextData` already received an update tick, otherwise `false`.

-----

