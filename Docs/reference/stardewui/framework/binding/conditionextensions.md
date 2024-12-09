---
title: ConditionExtensions
description: Extensions for the ICondition interface.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class ConditionExtensions

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Binding](index.md)  
Assembly: StardewUI.dll  

</div>

Extensions for the [ICondition](icondition.md) interface.

```cs
public static class ConditionExtensions
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ ConditionExtensions

## Members

### Methods

 | Name | Description |
| --- | --- |
| [NegateIf(ICondition, Boolean)](#negateificondition-bool) | Checks a negation flag, and returns a negated version of the `condition` if set. | 

## Details

### Methods

#### NegateIf(ICondition, bool)

Checks a negation flag, and returns a negated version of the `condition` if set.

```cs
public static StardewUI.Framework.Binding.ICondition NegateIf(StardewUI.Framework.Binding.ICondition condition, bool isNegated);
```

##### Parameters

**`condition`** &nbsp; [ICondition](icondition.md)  
The original condition.

**`isNegated`** &nbsp; [Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)  
Whether or not to negate the `condition`.

##### Returns

[ICondition](icondition.md)

  A negated version of the `condition`, if `isNegated` is `true`; otherwise, the original `condition`.

-----

