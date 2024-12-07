---
title: Lerp&lt;T&gt;
description: Performs linear interpolation between two values.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Delegate Lerp&lt;T&gt;

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Animation](index.md)  
Assembly: StardewUI.dll  

</div>

Performs linear interpolation between two values.

```cs
public T Lerp<T>(T value1, T value2, float amount);
```

### Type Parameters

**`T`**  
The type of value.


### Parameters

**`value1`** &nbsp; T  
The first, or "start" value to use at `amount` = `0.0`.

**`value2`** &nbsp; T  
The second, or "end" value to use at `amount` = `1.0`.

**`amount`** &nbsp; [Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)  
The interpolation amount between `0.0` and `1.0`.

