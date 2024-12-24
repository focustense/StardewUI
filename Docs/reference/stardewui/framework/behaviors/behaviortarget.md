---
title: BehaviorTarget
description: Encapsulates the target of an IViewBehavior.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class BehaviorTarget

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Behaviors](index.md)  
Assembly: StardewUI.dll  

</div>

Encapsulates the target of an [IViewBehavior](iviewbehavior.md).

```cs
public record BehaviorTarget : 
    IEquatable<StardewUI.Framework.Behaviors.BehaviorTarget>
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ BehaviorTarget

**Implements**  
[IEquatable](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1)<[BehaviorTarget](behaviortarget.md)>

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [BehaviorTarget(IView, IViewState)](#behaviortargetiview-iviewstate) | Encapsulates the target of an [IViewBehavior](iviewbehavior.md). | 

### Properties

 | Name | Description |
| --- | --- |
| [EqualityContract](#equalitycontract) |  | 
| [View](#view) | The view that will receive the behavior. | 
| [ViewState](#viewstate) | State overrides for the `View`. | 

## Details

### Constructors

#### BehaviorTarget(IView, IViewState)

Encapsulates the target of an [IViewBehavior](iviewbehavior.md).

```cs
public BehaviorTarget(StardewUI.IView View, StardewUI.Framework.Behaviors.IViewState ViewState);
```

##### Parameters

**`View`** &nbsp; [IView](../../iview.md)  
The view that will receive the behavior.

**`ViewState`** &nbsp; [IViewState](iviewstate.md)  
State overrides for the `View`.

-----

### Properties

#### EqualityContract



```cs
protected System.Type EqualityContract { get; }
```

##### Property Value

[Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)

-----

#### View

The view that will receive the behavior.

```cs
public StardewUI.IView View { get; set; }
```

##### Property Value

[IView](../../iview.md)

-----

#### ViewState

State overrides for the `View`.

```cs
public StardewUI.Framework.Behaviors.IViewState ViewState { get; set; }
```

##### Property Value

[IViewState](iviewstate.md)

-----

