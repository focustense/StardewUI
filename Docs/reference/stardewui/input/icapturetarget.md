---
title: ICaptureTarget
description: Denotes a view or other UI element that can be the active IKeyboardSubscriber. Allows view hosts to provide deterministic release, e.g. when the mouse is clicked outside the target.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Interface ICaptureTarget

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Input](index.md)  
Assembly: StardewUI.dll  

</div>

Denotes a view or other UI element that can be the active IKeyboardSubscriber. Allows view hosts to provide deterministic release, e.g. when the mouse is clicked outside the target.

```cs
public interface ICaptureTarget
```

## Remarks

This is primarily intended to work by checking if the Subscriber implements this interface, and if it's [CapturingView](icapturetarget.md#capturingview) belongs to the current click/focus tree. To work correctly, both of these conditions must be met.

## Members

### Properties

 | Name | Description |
| --- | --- |
| [CapturingView](#capturingview) | The view that initiated the capturing. May be the same object as the [ICaptureTarget](icapturetarget.md), or may be the "owner" of a hidden TextBox or other IKeyboardSubscriber. | 

### Methods

 | Name | Description |
| --- | --- |
| [ReleaseCapture()](#releasecapture) | Stops input capturing from this target. | 
| [Update(TimeSpan)](#updatetimespan) | Runs on every game update tick while capturing. | 

## Details

### Properties

#### CapturingView

The view that initiated the capturing. May be the same object as the [ICaptureTarget](icapturetarget.md), or may be the "owner" of a hidden TextBox or other IKeyboardSubscriber.

```cs
StardewUI.IView CapturingView { get; }
```

##### Property Value

[IView](../iview.md)

-----

### Methods

#### ReleaseCapture()

Stops input capturing from this target.

```cs
void ReleaseCapture();
```

-----

#### Update(TimeSpan)

Runs on every game update tick while capturing.

```cs
void Update(System.TimeSpan elapsed);
```

##### Parameters

**`elapsed`** &nbsp; [TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan)  
The amount of real time elapsed since the last tick.

-----

