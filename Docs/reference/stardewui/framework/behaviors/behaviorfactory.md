---
title: BehaviorFactory
description: A behavior factory based on per-name delegates. Can be used as a base class for other factories.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class BehaviorFactory

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Behaviors](index.md)  
Assembly: StardewUI.dll  

</div>

A behavior factory based on per-name delegates. Can be used as a base class for other factories.

```cs
public class BehaviorFactory : StardewUI.Framework.Behaviors.IBehaviorFactory
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ BehaviorFactory

**Implements**  
[IBehaviorFactory](ibehaviorfactory.md)

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [BehaviorFactory()](#behaviorfactory) |  | 

### Methods

 | Name | Description |
| --- | --- |
| [Add(IBehaviorFactory)](#addibehaviorfactory) | Adds a new delegate factory which this factory will be allowed to use as a fallback for any behavior names not handled directly. | 
| [CreateBehavior(Type, string, string)](#createbehaviortype-string-string) | Creates a new behavior. | 
| [Register&lt;TBehavior&gt;(string)](#registertbehaviorstring) | Registers a behavior for a given name using the behavior's default parameterless constructor. | 
| [Register(string, Func&lt;string, IViewBehavior&gt;)](#registerstring-funcstring-iviewbehavior) | Registers a behavior for a given name using a delegate function. | 
| [SupportsName(string)](#supportsnamestring) | Checks if the factory can create behaviors with a specified name. | 

## Details

### Constructors

#### BehaviorFactory()



```cs
public BehaviorFactory();
```

-----

### Methods

#### Add(IBehaviorFactory)

Adds a new delegate factory which this factory will be allowed to use as a fallback for any behavior names not handled directly.

```cs
public void Add(StardewUI.Framework.Behaviors.IBehaviorFactory factory);
```

##### Parameters

**`factory`** &nbsp; [IBehaviorFactory](ibehaviorfactory.md)  
The delegate factory.

-----

#### CreateBehavior(Type, string, string)

Creates a new behavior.

```cs
public StardewUI.Framework.Behaviors.IViewBehavior CreateBehavior(System.Type viewType, string name, string argument);
```

##### Parameters

**`viewType`** &nbsp; [Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)  
The specific type of [IView](../../iview.md) that will receive the behavior.

**`name`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The behavior name that specifies the type of behavior.

**`argument`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
Additional argument provided in the markup, distinct from the behavior's [DataType](iviewbehavior.md#datatype). Enables prefixed behaviors such as `tween:opacity`

##### Returns

[IViewBehavior](iviewbehavior.md)

  A new behavior of a type corresponding to the `name`.

-----

#### Register&lt;TBehavior&gt;(string)

Registers a behavior for a given name using the behavior's default parameterless constructor.

```cs
public void Register<TBehavior>(string name);
```

##### Parameters

**`name`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The markup name used to create to the `TBehavior` type.

##### Remarks

Used for behaviors that do not take arguments, only data.

-----

#### Register(string, Func&lt;string, IViewBehavior&gt;)

Registers a behavior for a given name using a delegate function.

```cs
public void Register(string name, Func<string, StardewUI.Framework.Behaviors.IViewBehavior> factory);
```

##### Parameters

**`name`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The markup name used to create this type of behavior.

**`factory`** &nbsp; [Func](https://learn.microsoft.com/en-us/dotnet/api/system.func-2)<[string](https://learn.microsoft.com/en-us/dotnet/api/system.string), [IViewBehavior](iviewbehavior.md)>  
Delegate function that accepts the construction argument (if any) and creates the corresponding behavior.

-----

#### SupportsName(string)

Checks if the factory can create behaviors with a specified name.

```cs
public bool SupportsName(string name);
```

##### Parameters

**`name`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The behavior name.

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  `true` if this factory should handle the specified `name`, otherwise `false`.

-----

