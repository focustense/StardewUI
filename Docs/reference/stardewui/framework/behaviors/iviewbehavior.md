---
title: IViewBehavior
description: Provides methods for attaching arbitrary data-dependent behavior to a view.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Interface IViewBehavior

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Behaviors](index.md)  
Assembly: StardewUI.dll  

</div>

Provides methods for attaching arbitrary data-dependent behavior to a view.

```cs
public interface IViewBehavior : System.IDisposable
```

**Implements**  
[IDisposable](https://learn.microsoft.com/en-us/dotnet/api/system.idisposable)

## Remarks

Add-ons should normally not use this interface directly, and instead derive from [ViewBehavior&lt;TView, TData&gt;](viewbehavior-2.md) for type safety.

## Members

### Properties

 | Name | Description |
| --- | --- |
| [DataType](#datatype) | The type of data that the behavior accepts in [SetData(Object)](iviewbehavior.md#setdataobject). | 

### Methods

 | Name | Description |
| --- | --- |
| [CanUpdate()](#canupdate) | Checks whether the behavior is allowed to [Update(TimeSpan)](iviewbehavior.md#updatetimespan). | 
| [Initialize(BehaviorTarget)](#initializebehaviortarget) | Initializes the target (view, state overrides, etc.) for the behavior. | 
| [PreUpdate(TimeSpan)](#preupdatetimespan) | Runs on every update tick, before any bindings or views update. | 
| [SetData(Object)](#setdataobject) | Updates the behavior's current data. | 
| [Update(TimeSpan)](#updatetimespan) | Runs on every update tick. | 

## Details

### Properties

#### DataType

The type of data that the behavior accepts in [SetData(Object)](iviewbehavior.md#setdataobject).

```cs
System.Type DataType { get; }
```

##### Property Value

[Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)

-----

### Methods

#### CanUpdate()

Checks whether the behavior is allowed to [Update(TimeSpan)](iviewbehavior.md#updatetimespan).

```cs
bool CanUpdate();
```

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  `true` to continue running [Update(TimeSpan)](iviewbehavior.md#updatetimespan) ticks, `false` to skip updates.

##### Remarks

Implementations can override this in order to selectively disable updates. Typically, updates will be disabled when the behavior cannot run due to not having an attached view or data.

-----

#### Initialize(BehaviorTarget)

Initializes the target (view, state overrides, etc.) for the behavior.

```cs
void Initialize(StardewUI.Framework.Behaviors.BehaviorTarget target);
```

##### Parameters

**`target`** &nbsp; [BehaviorTarget](behaviortarget.md)  
The target of the behavior.

##### Remarks

The framework guarantees that [Update(TimeSpan)](iviewbehavior.md#updatetimespan) will never be called before `Initialize`, so views may be implemented with default parameterless constructors and perform initialization in this method.

-----

#### PreUpdate(TimeSpan)

Runs on every update tick, before any bindings or views update.

```cs
void PreUpdate(System.TimeSpan elapsed);
```

##### Parameters

**`elapsed`** &nbsp; [TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan)

##### Remarks

Typically used to read information about the underlying view as it existed at the beginning of the frame, e.g. to handle a transition.

-----

#### SetData(Object)

Updates the behavior's current data.

```cs
void SetData(System.Object data);
```

##### Parameters

**`data`** &nbsp; [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object)  
The new data.

-----

#### Update(TimeSpan)

Runs on every update tick.

```cs
void Update(System.TimeSpan elapsed);
```

##### Parameters

**`elapsed`** &nbsp; [TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan)  
Time elapsed since the last update.

-----

