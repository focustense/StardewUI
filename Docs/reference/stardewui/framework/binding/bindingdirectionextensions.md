---
title: BindingDirectionExtensions
description: Extension methods for the BindingDirection enum.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class BindingDirectionExtensions

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Binding](index.md)  
Assembly: StardewUI.dll  

</div>

Extension methods for the [BindingDirection](bindingdirection.md) enum.

```cs
public static class BindingDirectionExtensions
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ BindingDirectionExtensions

## Members

### Methods

 | Name | Description |
| --- | --- |
| [IsIn(BindingDirection)](#isinbindingdirection) | Gets whether or not a direction includes an input binding, i.e. is either [In](bindingdirection.md#in) or [InOut](bindingdirection.md#inout). | 
| [IsOut(BindingDirection)](#isoutbindingdirection) | Gets whether or not a direction includes an output binding, i.e. is either [Out](bindingdirection.md#out) or [InOut](bindingdirection.md#inout). | 

## Details

### Methods

#### IsIn(BindingDirection)

Gets whether or not a direction includes an input binding, i.e. is either [In](bindingdirection.md#in) or [InOut](bindingdirection.md#inout).

```cs
public static bool IsIn(StardewUI.Framework.Binding.BindingDirection direction);
```

##### Parameters

**`direction`** &nbsp; [BindingDirection](bindingdirection.md)  
The binding direction.

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

-----

#### IsOut(BindingDirection)

Gets whether or not a direction includes an output binding, i.e. is either [Out](bindingdirection.md#out) or [InOut](bindingdirection.md#inout).

```cs
public static bool IsOut(StardewUI.Framework.Binding.BindingDirection direction);
```

##### Parameters

**`direction`** &nbsp; [BindingDirection](bindingdirection.md)  
The binding direction.

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

-----

