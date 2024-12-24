---
title: VisibilityStateBehavior&lt;TValue&gt;
description: Behavior that applies a property override when a view enters or leaves the visible state.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class VisibilityStateBehavior&lt;TValue&gt;

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Behaviors](index.md)  
Assembly: StardewUI.dll  

</div>

Behavior that applies a property override when a view enters or leaves the visible state.

```cs
public class VisibilityStateBehavior<TValue> : 
    StardewUI.Framework.Behaviors.ViewBehavior<TView, TData>
```

### Type Parameters

**`TValue`**  
Value type for the overridden property.


**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ [ViewBehavior&lt;TView, TData&gt;](viewbehavior-2.md) ⇦ VisibilityStateBehavior&lt;TValue&gt;

## Remarks

The override is added when the view's [Visibility](../../iview.md#visibility) becomes `StardewUI.Layout.Visibility.#ctor` and removed when it becomes [Hidden](../../layout/visibility.md#hidden). In addition, the state corresponding to the view's initial visibility is applied as soon as the behavior as initialized, allowing for transitions to occur when the view is first created even if the visibility never explicitly changes.

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [VisibilityStateBehavior&lt;TValue&gt;(string)](#visibilitystatebehaviortvaluestring) | Behavior that applies a property override when a view enters or leaves the visible state. | 

### Properties

 | Name | Description |
| --- | --- |
| [Data](viewbehavior-2.md#data) | The assigned or bound data.<br><span class="muted" markdown>(Inherited from [ViewBehavior&lt;TView, TData&gt;](viewbehavior-2.md))</span> | 
| [View](viewbehavior-2.md#view) | The currently-attached view.<br><span class="muted" markdown>(Inherited from [ViewBehavior&lt;TView, TData&gt;](viewbehavior-2.md))</span> | 
| [ViewState](viewbehavior-2.md#viewstate) | State overrides for the [View](viewbehavior-2.md#view).<br><span class="muted" markdown>(Inherited from [ViewBehavior&lt;TView, TData&gt;](viewbehavior-2.md))</span> | 

### Methods

 | Name | Description |
| --- | --- |
| [CanUpdate()](viewbehavior-2.md#canupdate) | Checks whether the behavior is allowed to [Update(TimeSpan)](iviewbehavior.md#updatetimespan).<br><span class="muted" markdown>(Inherited from [ViewBehavior&lt;TView, TData&gt;](viewbehavior-2.md))</span> | 
| [Dispose()](viewbehavior-2.md#dispose) | <span class="muted" markdown>(Inherited from [ViewBehavior&lt;TView, TData&gt;](viewbehavior-2.md))</span> | 
| [Initialize(BehaviorTarget)](viewbehavior-2.md#initializebehaviortarget) | Initializes the target (view, state overrides, etc.) for the behavior.<br><span class="muted" markdown>(Inherited from [ViewBehavior&lt;TView, TData&gt;](viewbehavior-2.md))</span> | 
| [OnDispose()](#ondispose) | Runs when the behavior is being disposed.<br><span class="muted" markdown>(Overrides [ViewBehavior&lt;TView, TData&gt;](viewbehavior-2.md).[OnDispose()](viewbehavior-2.md#ondispose))</span> | 
| [OnInitialize()](#oninitialize) | Runs after the behavior has been initialized.<br><span class="muted" markdown>(Overrides [ViewBehavior&lt;TView, TData&gt;](viewbehavior-2.md).[OnInitialize()](viewbehavior-2.md#oninitialize))</span> | 
| [OnNewData(TValue)](#onnewdatatvalue) | <span class="muted" markdown>(Overrides [ViewBehavior&lt;TView, TData&gt;](viewbehavior-2.md).[OnNewData(TData)](viewbehavior-2.md#onnewdatatdata))</span> | 
| [Update(TimeSpan)](#updatetimespan) | Runs on every update tick.<br><span class="muted" markdown>(Overrides [ViewBehavior&lt;TView, TData&gt;](viewbehavior-2.md).[Update(TimeSpan)](viewbehavior-2.md#updatetimespan))</span> | 

## Details

### Constructors

#### VisibilityStateBehavior&lt;TValue&gt;(string)

Behavior that applies a property override when a view enters or leaves the visible state.

```cs
public VisibilityStateBehavior<TValue>(string propertyName);
```

##### Parameters

**`propertyName`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
Name of the overridden property.

##### Remarks

The override is added when the view's [Visibility](../../iview.md#visibility) becomes `StardewUI.Layout.Visibility.#ctor` and removed when it becomes [Hidden](../../layout/visibility.md#hidden). In addition, the state corresponding to the view's initial visibility is applied as soon as the behavior as initialized, allowing for transitions to occur when the view is first created even if the visibility never explicitly changes.

-----

### Methods

#### OnDispose()

Runs when the behavior is being disposed.

```cs
protected override void OnDispose();
```

##### Remarks

The default implementation does nothing. Overriding this allows subclasses to perform their own cleanup, if required by the specific feature.

-----

#### OnInitialize()

Runs after the behavior has been initialized.

```cs
protected override void OnInitialize();
```

##### Remarks

Setup code should go in this method to ensure that the values of [View](viewbehavior-2.md#view) and [ViewState](viewbehavior-2.md#viewstate) are actually assigned. If code runs in the behavior's constructor, these are not guaranteed to be populated.

-----

#### OnNewData(TValue)



```cs
protected override void OnNewData(TValue previousData);
```

##### Parameters

**`previousData`** &nbsp; TValue

-----

#### Update(TimeSpan)

Runs on every update tick.

```cs
public override void Update(System.TimeSpan elapsed);
```

##### Parameters

**`elapsed`** &nbsp; [TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan)  
Time elapsed since the last update.

-----

