---
title: TransformConverter
description: String converter for the Transform type.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class TransformConverter

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Converters](index.md)  
Assembly: StardewUI.dll  

</div>

String converter for the [Transform](../../graphics/transform.md) type.

```cs
public class TransformConverter : 
    StardewUI.Framework.Converters.IValueConverter<string, StardewUI.Graphics.Transform>, 
    StardewUI.Framework.Converters.IValueConverter
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ TransformConverter

**Implements**  
[IValueConverter](ivalueconverter-2.md)<[string](https://learn.microsoft.com/en-us/dotnet/api/system.string), [Transform](../../graphics/transform.md)>, [IValueConverter](ivalueconverter.md)

## Remarks

Valid strings must be a semicolon-separated list of one of the valid transform properties, followed by a colon, followed by the property value. 

 Valid property names include: `translate`, `translateX`, `translateY`, `rotate`, `scale`, `scaleX` and `scaleY`. 

 The value following each property should be a number, except for `translate` and `scale` which should be a [Vector2](https://docs.monogame.net/api/Microsoft.Xna.Framework.Vector2.html) compatible string such as `2, -4` instead. Rotation values are interpreted as instead of radians for improved readability.

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [TransformConverter()](#transformconverter) |  | 

### Methods

 | Name | Description |
| --- | --- |
| [Convert(string)](#convertstring) | Converts a value from the source type to the destination type. | 

## Details

### Constructors

#### TransformConverter()



```cs
public TransformConverter();
```

-----

### Methods

#### Convert(string)

Converts a value from the source type to the destination type.

```cs
public StardewUI.Graphics.Transform Convert(string value);
```

##### Parameters

**`value`** &nbsp; [string](https://learn.microsoft.com/en-us/dotnet/api/system.string)  
The value to convert.

##### Returns

[Transform](../../graphics/transform.md)

-----

