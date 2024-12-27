---
title: ViewBehavior&lt;TView, TData&gt;
description: Base class for a behavior extension, which enables self-contained, stateful behaviors to be "attached" to an arbitrary view without having to extend the view itself.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class ViewBehavior&lt;TView, TData&gt;

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Behaviors](index.md)  
Assembly: StardewUI.dll  

</div>

Base class for a behavior extension, which enables self-contained, stateful behaviors to be "attached" to an arbitrary view without having to extend the view itself.

```cs
public class ViewBehavior<TView, TData> : 
    StardewUI.Framework.Behaviors.IViewBehavior, System.IDisposable
```

### Type Parameters

**`TView`**  
Base type for all views that support this behavior.

**`TData`**  
Type of data provided to this behavior as an argument/binding.


**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ ViewBehavior&lt;TView, TData&gt;

**Implements**  
[IViewBehavior](iviewbehavior.md), [IDisposable](https://learn.microsoft.com/en-us/dotnet/api/system.idisposable)

## Remarks

Behaviors receive the [View](viewbehavior-2.md#view) which is decorated by the behavior, and some arbitrary [Data](viewbehavior-2.md#data) obtained from the attribute value or binding. They then become part of the UI's update loop, via their [Update(TimeSpan)](viewbehavior-2.md#updatetimespan) method running every tick.

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [ViewBehavior&lt;TView, TData&gt;()](#viewbehaviortview-tdata) |  | 

### Properties

 | Name | Description |
| --- | --- |
| [Data](#data) | The assigned or bound data. | 
| [View](#view) | The currently-attached view. | 
| [ViewState](#viewstate) | State overrides for the [View](viewbehavior-2.md#view). | 

### Methods

 | Name | Description |
| --- | --- |
| [CanUpdate()](#canupdate) | Checks whether the behavior is allowed to [Update(TimeSpan)](iviewbehavior.md#updatetimespan). | 
| [Dispose()](#dispose) |  | 
| [Initialize(BehaviorTarget)](#initializebehaviortarget) | Initializes the target (view, state overrides, etc.) for the behavior. | 
| [OnAttached()](#onattached) | Runs after the behavior is attached to a target. | 
| [OnDetached(IView)](#ondetachediview) | Runs when the behavior is detached from a target. | 
| [OnDispose()](#ondispose) | Runs when the behavior is being disposed. | 
| [OnNewData(TData)](#onnewdatatdata) | Runs when the [Data](viewbehavior-2.md#data) of this behavior is changed. | 
| [PreUpdate(TimeSpan)](#preupdatetimespan) | Runs on every update tick, before any bindings or views update. | 
| [Update(TimeSpan)](#updatetimespan) | Runs on every update tick. | 

## Details

### Constructors

#### ViewBehavior&lt;TView, TData&gt;()



```cs
protected ViewBehavior<TView, TData>();
```

-----

### Properties

#### Data

The assigned or bound data.

```cs
public TData Data { get; set; }
```

##### Property Value

`TData`

-----

#### View

The currently-attached view.

```cs
public TView View { get; }
```

##### Property Value

`TView`

-----

#### ViewState

State overrides for the [View](viewbehavior-2.md#view).

```cs
public StardewUI.Framework.Behaviors.IViewState ViewState { get; }
```

##### Property Value

[IViewState](iviewstate.md)

-----

### Methods

#### CanUpdate()

Checks whether the behavior is allowed to [Update(TimeSpan)](iviewbehavior.md#updatetimespan).

```cs
public bool CanUpdate();
```

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  `true` to continue running [Update(TimeSpan)](iviewbehavior.md#updatetimespan) ticks, `false` to skip updates.

##### Remarks

Implementations can override this in order to selectively disable updates. Typically, updates will be disabled when the behavior cannot run due to not having an attached view or data.

-----

#### Dispose()



```cs
public void Dispose();
```

-----

#### Initialize(BehaviorTarget)

Initializes the target (view, state overrides, etc.) for the behavior.

```cs
public void Initialize(StardewUI.Framework.Behaviors.BehaviorTarget target);
```

##### Parameters

**`target`** &nbsp; [BehaviorTarget](behaviortarget.md)  
The target of the behavior.

##### Remarks

The framework guarantees that [Update(TimeSpan)](iviewbehavior.md#updatetimespan) will never be called before `Initialize`, so views may be implemented with default parameterless constructors and perform initialization in this method.

-----

#### OnAttached()

Runs after the behavior is attached to a target.

```cs
protected virtual void OnAttached();
```

##### Remarks

Setup code should go in this method to ensure that the values of [View](viewbehavior-2.md#view) and [ViewState](viewbehavior-2.md#viewstate) are actually assigned. If code runs in the behavior's constructor, these are not guaranteed to be populated.

-----

#### OnDetached(IView)

Runs when the behavior is detached from a target.

```cs
protected virtual void OnDetached(StardewUI.IView view);
```

##### Parameters

**`view`** &nbsp; [IView](../../iview.md)  
The view that was previously attached.

##### Remarks

Behaviors may receive new views as part of a "rebind", if the old view is destroyed and recreated, for example as the result of a conditional binding changing states. 

`OnDetached` is always immediately followed by [OnAttached()](viewbehavior-2.md#onattached). A behavior cannot remain in a detached state; however, overriding this method gives behaviors the opportunity to clean up state from the old view (e.g. remove event handlers) before the new one is attached. 

 Also runs when the behavior is disposed, so detach logic does not need to be duplicated in [OnDispose()](viewbehavior-2.md#ondispose).

-----

#### OnDispose()

Runs when the behavior is being disposed.

```cs
protected virtual void OnDispose();
```

##### Remarks

The default implementation does nothing. Overriding this allows subclasses to perform their own cleanup, if required by the specific feature.

-----

#### OnNewData(TData)

Runs when the [Data](viewbehavior-2.md#data) of this behavior is changed.

```cs
protected virtual void OnNewData(TData previousData);
```

##### Parameters

**`previousData`** &nbsp; TData

##### Remarks

At the time this method runs, [Data](viewbehavior-2.md#data) has already been assigned to the new value. After the method completes, the `previousData` will no longer be accessible to this behavior.

-----

#### PreUpdate(TimeSpan)

Runs on every update tick, before any bindings or views update.

```cs
public virtual void PreUpdate(System.TimeSpan elapsed);
```

##### Parameters

**`elapsed`** &nbsp; [TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan)

##### Remarks

Typically used to read information about the underlying view as it existed at the beginning of the frame, e.g. to handle a transition.

-----

#### Update(TimeSpan)

Runs on every update tick.

```cs
public virtual void Update(System.TimeSpan elapsed);
```

##### Parameters

**`elapsed`** &nbsp; [TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan)  
Time elapsed since the last update.

-----

