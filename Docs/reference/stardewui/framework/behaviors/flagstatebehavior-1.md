---
title: FlagStateBehavior&lt;TValue&gt;
description: Behavior that applies a property override when a named view state flag is detected.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class FlagStateBehavior&lt;TValue&gt;

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Behaviors](index.md)  
Assembly: StardewUI.dll  

</div>

Behavior that applies a property override when a named view state flag is detected.

```cs
public class FlagStateBehavior<TValue> : 
    StardewUI.Framework.Behaviors.ViewBehavior<TView, TData>
```

### Type Parameters

**`TValue`**  
Value type for the overridden property.


**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ [ViewBehavior&lt;TView, TData&gt;](viewbehavior-2.md) ⇦ FlagStateBehavior&lt;TValue&gt;

## Remarks

The flag is generally added or removed by [ConditionalFlagBehavior](conditionalflagbehavior.md) as part of a two-step approach to creating conditional attributes.

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [FlagStateBehavior&lt;TValue&gt;(string, string)](#flagstatebehaviortvaluestring-string) | Behavior that applies a property override when a named view state flag is detected. | 

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
| [OnNewData(TValue)](#onnewdatatvalue) | <span class="muted" markdown>(Overrides [ViewBehavior&lt;TView, TData&gt;](viewbehavior-2.md).[OnNewData(TData)](viewbehavior-2.md#onnewdatatdata))</span> | 
| [PreUpdate(TimeSpan)](viewbehavior-2.md#preupdatetimespan) | Runs on every update tick, before any bindings or views update.<br><span class="muted" markdown>(Inherited from [ViewBehavior&lt;TView, TData&gt;](viewbehavior-2.md))</span> | 
| [Update(TimeSpan)](viewbehavior-2.md#updatetimespan) | Runs on every update tick.<br><span class="muted" markdown>(Inherited from [ViewBehavior&lt;TView, TData&gt;](viewbehavior-2.md))</span> | 

## Details

### Constructors

#### FlagStateBehavior&lt;TValue&gt;(string, string)

Behavior that applies a property override when a named view state flag is detected.

```cs
public FlagStateBehavior<TValue>(string flagName, string propertyName);
```

##### Parameters

**`flagName`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
Name of the flag to watch.

**`propertyName`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
Name of the overridden property.

##### Remarks

The flag is generally added or removed by [ConditionalFlagBehavior](conditionalflagbehavior.md) as part of a two-step approach to creating conditional attributes.

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

#### OnNewData(TValue)



```cs
protected override void OnNewData(TValue previousData);
```

##### Parameters

**`previousData`** &nbsp; TValue

-----

