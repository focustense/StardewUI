---
title: ConditionalFlagBehavior
description: Updates a view state flag with a boolean value corresponding to the behavior's data.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class ConditionalFlagBehavior

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Behaviors](index.md)  
Assembly: StardewUI.dll  

</div>

Updates a view state flag with a boolean value corresponding to the behavior's data.

```cs
public class ConditionalFlagBehavior : 
    StardewUI.Framework.Behaviors.ViewBehavior<TView, TData>
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ [ViewBehavior&lt;TView, TData&gt;](viewbehavior-2.md) ⇦ ConditionalFlagBehavior

## Remarks

Essentially enables arbitrary state names to be linked with context properties that are boolean-valued or convertible to boolean, primarily as a bridge for the [FlagStateBehavior&lt;TValue&gt;](flagstatebehavior-1.md) which in turn allows property changes to be associated with the state. In other words, part one of the two-part process used to create conditional attributes.

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [ConditionalFlagBehavior(string)](#conditionalflagbehaviorstring) | Updates a view state flag with a boolean value corresponding to the behavior's data. | 

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
| [OnDetached(IView)](viewbehavior-2.md#ondetachediview) | Runs when the behavior is detached from a target.<br><span class="muted" markdown>(Inherited from [ViewBehavior&lt;TView, TData&gt;](viewbehavior-2.md))</span> | 
| [OnDispose()](viewbehavior-2.md#ondispose) | Runs when the behavior is being disposed.<br><span class="muted" markdown>(Inherited from [ViewBehavior&lt;TView, TData&gt;](viewbehavior-2.md))</span> | 
| [OnNewData(Boolean)](#onnewdatabool) | <span class="muted" markdown>(Overrides [ViewBehavior&lt;TView, TData&gt;](viewbehavior-2.md).[OnNewData(TData)](viewbehavior-2.md#onnewdatatdata))</span> | 
| [PreUpdate(TimeSpan)](viewbehavior-2.md#preupdatetimespan) | Runs on every update tick, before any bindings or views update.<br><span class="muted" markdown>(Inherited from [ViewBehavior&lt;TView, TData&gt;](viewbehavior-2.md))</span> | 
| [Update(TimeSpan)](viewbehavior-2.md#updatetimespan) | Runs on every update tick.<br><span class="muted" markdown>(Inherited from [ViewBehavior&lt;TView, TData&gt;](viewbehavior-2.md))</span> | 

## Details

### Constructors

#### ConditionalFlagBehavior(string)

Updates a view state flag with a boolean value corresponding to the behavior's data.

```cs
public ConditionalFlagBehavior(string flagName);
```

##### Parameters

**`flagName`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
Name of the flag to set when [Data](viewbehavior-2.md#data) is `true`.

##### Remarks

Essentially enables arbitrary state names to be linked with context properties that are boolean-valued or convertible to boolean, primarily as a bridge for the [FlagStateBehavior&lt;TValue&gt;](flagstatebehavior-1.md) which in turn allows property changes to be associated with the state. In other words, part one of the two-part process used to create conditional attributes.

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

#### OnNewData(bool)



```cs
protected override void OnNewData(bool previousData);
```

##### Parameters

**`previousData`** &nbsp; [Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

-----

