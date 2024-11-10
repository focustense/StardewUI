---
title: GridItemLayoutConverter
description: String converter for a GridItemLayout.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class GridItemLayoutConverter

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Converters](index.md)  
Assembly: StardewUI.dll  

</div>

String converter for a [GridItemLayout](../../widgets/griditemlayout.md).

```cs
public class GridItemLayoutConverter : 
    StardewUI.Framework.Converters.IValueConverter<string, StardewUI.Widgets.GridItemLayout>, 
    StardewUI.Framework.Converters.IValueConverter
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ GridItemLayoutConverter

**Implements**  
[IValueConverter](ivalueconverter-2.md)<[string](https://learn.microsoft.com/en-us/dotnet/api/system.string), [GridItemLayout](../../widgets/griditemlayout.md)>, [IValueConverter](ivalueconverter.md)

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [GridItemLayoutConverter()](#griditemlayoutconverter) |  | 

### Methods

 | Name | Description |
| --- | --- |
| [Convert(string)](#convertstring) | Converts a value from the source type to the destination type. | 

## Details

### Constructors

#### GridItemLayoutConverter()



```cs
public GridItemLayoutConverter();
```

-----

### Methods

#### Convert(string)

Converts a value from the source type to the destination type.

```cs
public StardewUI.Widgets.GridItemLayout Convert(string value);
```

##### Parameters

**`value`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The value to convert.

##### Returns

[GridItemLayout](../../widgets/griditemlayout.md)

-----

