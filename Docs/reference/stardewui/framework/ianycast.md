---
title: IAnyCast
description: A marker interface that, when used in place of Object, forces the framework to attempt an explicit conversion/cast to the expected destination type.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Interface IAnyCast

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework](index.md)  
Assembly: StardewUI.dll  

</div>

A marker interface that, when used in place of [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object), forces the framework to attempt an explicit conversion/cast to the expected destination type.

```cs
public interface IAnyCast
```

## Remarks

Casts are normally only permitted when the source type is actually assignable to the destination type. `IAnyCast` is used to indicate that the source is intentionally ambiguous or boxed, and the real type cannot be known until the conversion is attempted, at which point it is assumed to be assignment-compatible or have an explicit conversion operator.

## Members

### Properties

 | Name | Description |
| --- | --- |
| [Value](#value) | The boxed value. | 

## Details

### Properties

#### Value

The boxed value.

```cs
System.Object Value { get; }
```

##### Property Value

[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object)

-----

