---
title: StateBehaviorFactory
description: "Factory for creating behaviors that apply single-property overrides on state transitions, such as hover:transform."
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class StateBehaviorFactory

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Behaviors](index.md)  
Assembly: StardewUI.dll  

</div>

Factory for creating behaviors that apply single-property overrides on state transitions, such as `hover:transform`.

```cs
public class StateBehaviorFactory : 
    StardewUI.Framework.Behaviors.IBehaviorFactory
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ StateBehaviorFactory

**Implements**  
[IBehaviorFactory](ibehaviorfactory.md)

## Remarks

Also handles transitions, which follow a similar creation mechanism.

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [StateBehaviorFactory()](#statebehaviorfactory) |  | 

### Methods

 | Name | Description |
| --- | --- |
| [CreateBehavior(Type, string, string)](#createbehaviortype-string-string) | Creates a new behavior. | 
| [SupportsName(string)](#supportsnamestring) | Checks if the factory can create behaviors with a specified name. | 

## Details

### Constructors

#### StateBehaviorFactory()



```cs
public StateBehaviorFactory();
```

-----

### Methods

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

