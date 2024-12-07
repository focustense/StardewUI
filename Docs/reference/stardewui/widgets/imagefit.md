---
title: ImageFit
description: Specifies how an image should be scaled to fit the content area when the available size is different from the image size, and especially when it has a different aspect ratio.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Enum ImageFit

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Widgets](index.md)  
Assembly: StardewUI.dll  

</div>

Specifies how an image should be scaled to fit the content area when the available size is different from the image size, and especially when it has a different aspect ratio.

```cs
public enum ImageFit
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ [ValueType](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype) ⇦ [Enum](https://learn.microsoft.com/en-us/dotnet/api/system.enum) ⇦ ImageFit

## Fields

 | Name | Value | Description |
| --- | --- | --- |
| <a id="none">None</a> | 0 | Don't scale the image, i.e. draw it at its original size regardless of the eventual layout size. | 
| <a id="contain">Contain</a> | 1 | Force uniform scaling, and make both dimensions small enough to fit in the content area. | 
| <a id="cover">Cover</a> | 2 | Force uniform scaling, and make both dimensions large enough to completely cover the content area (i.e. clip whatever parts are outside the bounds). | 
| <a id="stretch">Stretch</a> | 3 | Allow non-uniform scaling, and scale the image to exactly match the content area. | 

