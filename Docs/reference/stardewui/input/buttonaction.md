---
title: ButtonAction
description: The actions that a given button can trigger in a UI context. For details see ButtonResolver.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Enum ButtonAction

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Input](index.md)  
Assembly: StardewUI.dll  

</div>

The actions that a given button can trigger in a UI context. For details see [ButtonResolver](buttonresolver.md).

```cs
public enum ButtonAction
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ [ValueType](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype) ⇦ [Enum](https://learn.microsoft.com/en-us/dotnet/api/system.enum) ⇦ ButtonAction

## Fields

 | Name | Value | Description |
| --- | --- | --- |
| <a id="none">None</a> | 0 | The button has no standard UI behavior. | 
| <a id="primary">Primary</a> | 1 | Used for primary interaction, or "left click". | 
| <a id="secondary">Secondary</a> | 2 | Used for secondary interaction, or "right click". | 
| <a id="cancel">Cancel</a> | 3 | Cancels out of the current menu, overlay, etc. | 

