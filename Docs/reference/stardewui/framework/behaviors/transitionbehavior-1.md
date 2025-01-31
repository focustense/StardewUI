---
title: TransitionBehavior&lt;TValue&gt;
description: Behavior that applies gradual transitions (AKA tweens) to view properties.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class TransitionBehavior&lt;TValue&gt;

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Behaviors](index.md)  
Assembly: StardewUI.dll  

</div>

Behavior that applies gradual transitions (AKA tweens) to view properties.

```cs
public class TransitionBehavior<TValue> : 
    StardewUI.Framework.Behaviors.ViewBehavior<TView, TData>
```

### Type Parameters

**`TValue`**  
Value type for the transitioned property.


**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ [ViewBehavior&lt;TView, TData&gt;](viewbehavior-2.md) ⇦ TransitionBehavior&lt;TValue&gt;

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [TransitionBehavior&lt;TValue&gt;(string, Lerp&lt;TValue&gt;)](#transitionbehaviortvaluestring-lerptvalue) | Behavior that applies gradual transitions (AKA tweens) to view properties. | 

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
| [OnAttached()](#onattached) | Runs after the behavior is attached to a target.<br><span class="muted" markdown>(Overrides [ViewBehavior&lt;TView, TData&gt;](viewbehavior-2.md).[OnAttached()](viewbehavior-2.md#onattached))</span> | 
| [OnDetached(IView)](#ondetachediview) | Runs when the behavior is detached from a target.<br><span class="muted" markdown>(Overrides [ViewBehavior&lt;TView, TData&gt;](viewbehavior-2.md).[OnDetached(IView)](viewbehavior-2.md#ondetachediview))</span> | 
| [OnDispose()](viewbehavior-2.md#ondispose) | Runs when the behavior is being disposed.<br><span class="muted" markdown>(Inherited from [ViewBehavior&lt;TView, TData&gt;](viewbehavior-2.md))</span> | 
| [OnNewData(TData)](viewbehavior-2.md#onnewdatatdata) | <span class="muted" markdown>(Inherited from [ViewBehavior&lt;TView, TData&gt;](viewbehavior-2.md))</span> | 
| [PreUpdate(TimeSpan)](#preupdatetimespan) | Runs on every update tick, before any bindings or views update.<br><span class="muted" markdown>(Overrides [ViewBehavior&lt;TView, TData&gt;](viewbehavior-2.md).[PreUpdate(TimeSpan)](viewbehavior-2.md#preupdatetimespan))</span> | 
| [Update(TimeSpan)](#updatetimespan) | Runs on every update tick.<br><span class="muted" markdown>(Overrides [ViewBehavior&lt;TView, TData&gt;](viewbehavior-2.md).[Update(TimeSpan)](viewbehavior-2.md#updatetimespan))</span> | 

## Details

### Constructors

#### TransitionBehavior&lt;TValue&gt;(string, Lerp&lt;TValue&gt;)

Behavior that applies gradual transitions (AKA tweens) to view properties.

```cs
public TransitionBehavior<TValue>(string propertyName, StardewUI.Animation.Lerp<TValue> lerp);
```

##### Parameters

**`propertyName`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
Name of the overridden property.

**`lerp`** &nbsp; [Lerp&lt;TValue&gt;](../../animation/lerp-1.md)  
Interpolation function for the transitioned property type.

-----

### Methods

#### OnAttached()

Runs after the behavior is attached to a target.

```cs
protected override void OnAttached();
```

##### Remarks

Setup code should go in this method to ensure that the values of [View](viewbehavior-2.md#view) and [ViewState](viewbehavior-2.md#viewstate) are actually assigned. If code runs in the behavior's constructor, these are not guaranteed to be populated.

-----

#### OnDetached(IView)

Runs when the behavior is detached from a target.

```cs
protected override void OnDetached(StardewUI.IView view);
```

##### Parameters

**`view`** &nbsp; [IView](../../iview.md)  
The view that was previously attached.

##### Remarks

Behaviors may receive new views as part of a "rebind", if the old view is destroyed and recreated, for example as the result of a conditional binding changing states. 

`OnDetached` is always immediately followed by [OnAttached()](viewbehavior-2.md#onattached). A behavior cannot remain in a detached state; however, overriding this method gives behaviors the opportunity to clean up state from the old view (e.g. remove event handlers) before the new one is attached. 

 Also runs when the behavior is disposed, so detach logic does not need to be duplicated in [OnDispose()](viewbehavior-2.md#ondispose).

-----

#### PreUpdate(TimeSpan)

Runs on every update tick, before any bindings or views update.

```cs
public override void PreUpdate(System.TimeSpan elapsed);
```

##### Parameters

**`elapsed`** &nbsp; [TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan)

##### Remarks

Typically used to read information about the underlying view as it existed at the beginning of the frame, e.g. to handle a transition.

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

