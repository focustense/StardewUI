---
title: IBehaviorFactory
description: Factory for creating IViewBehavior instances from markup data.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Interface IBehaviorFactory

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Behaviors](index.md)  
Assembly: StardewUI.dll  

</div>

Factory for creating [IViewBehavior](iviewbehavior.md) instances from markup data.

```cs
public interface IBehaviorFactory
```

## Members

### Methods

 | Name | Description |
| --- | --- |
| [CreateBehavior(Type, string, string)](#createbehaviortype-string-string) | Creates a new behavior. | 
| [SupportsName(string)](#supportsnamestring) | Checks if the factory can create behaviors with a specified name. | 

## Details

### Methods

#### CreateBehavior(Type, string, string)

Creates a new behavior.

```cs
StardewUI.Framework.Behaviors.IViewBehavior CreateBehavior(System.Type viewType, string name, string argument);
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
bool SupportsName(string name);
```

##### Parameters

**`name`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The behavior name.

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  `true` if this factory should handle the specified `name`, otherwise `false`.

-----

