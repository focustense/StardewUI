---
title: IValueConverter&lt;TSource, TDestination&gt;
description: Provides a method to convert between value types.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Interface IValueConverter&lt;TSource, TDestination&gt;

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Converters](index.md)  
Assembly: StardewUI.dll  

</div>

Provides a method to convert between value types.

```cs
public interface IValueConverter<TSource, TDestination>
```

### Type Parameters

**`TSource`**  
The type of value to be converted.

**`TDestination`**  
The converted value type.


## Members

### Methods

 | Name | Description |
| --- | --- |
| [Convert(TSource)](#converttsource) | Converts a value from the source type to the destination type. | 

## Details

### Methods

#### Convert(TSource)

Converts a value from the source type to the destination type.

```cs
TDestination Convert(TSource value);
```

##### Parameters

**`value`** &nbsp; TSource  
The value to convert.

##### Returns

`TDestination`

  The converted value.

-----

