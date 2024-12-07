---
title: LayoutConverter
description: String converter for the LayoutParameters type.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class LayoutConverter

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Converters](index.md)  
Assembly: StardewUI.dll  

</div>

String converter for the [LayoutParameters](../../layout/layoutparameters.md) type.

```cs
public class LayoutConverter : 
    StardewUI.Framework.Converters.IValueConverter<string, StardewUI.Layout.LayoutParameters>, 
    StardewUI.Framework.Converters.IValueConverter
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ LayoutConverter

**Implements**  
[IValueConverter](ivalueconverter-2.md)<[string](https://learn.microsoft.com/en-us/dotnet/api/system.string), [LayoutParameters](../../layout/layoutparameters.md)>, [IValueConverter](ivalueconverter.md)

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [LayoutConverter()](#layoutconverter) |  | 

### Methods

 | Name | Description |
| --- | --- |
| [Convert(string)](#convertstring) | Converts a value from the source type to the destination type. | 

## Details

### Constructors

#### LayoutConverter()



```cs
public LayoutConverter();
```

-----

### Methods

#### Convert(string)

Converts a value from the source type to the destination type.

```cs
public StardewUI.Layout.LayoutParameters Convert(string value);
```

##### Parameters

**`value`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The value to convert.

##### Returns

[LayoutParameters](../../layout/layoutparameters.md)

-----

