---
title: IValueSource&lt;T&gt;
description: Abstract representation of the source of any value, generally as used in a data binding.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Interface IValueSource&lt;T&gt;

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Sources](index.md)  
Assembly: StardewUI.dll  

</div>

Abstract representation of the source of any value, generally as used in a data binding.

```cs
public interface IValueSource<T> : StardewUI.Framework.Sources.IValueSource
```

### Type Parameters

**`T`**  
Type of value supplied.


**Implements**  
[IValueSource](ivaluesource.md)

## Members

### Properties

 | Name | Description |
| --- | --- |
| [Value](#value) | Gets the current value obtained from the most recent [Update(Boolean)](ivaluesource.md#updatebool), or writes a new value when set. | 

## Details

### Properties

#### Value

Gets the current value obtained from the most recent [Update(Boolean)](ivaluesource.md#updatebool), or writes a new value when set.

```cs
T Value { get; set; }
```

##### Property Value

`T`

-----

