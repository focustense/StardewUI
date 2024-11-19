---
title: MenuActivationMode
description: Available behaviors for opening a ViewMenu&lt;T&gt;.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Enum MenuActivationMode

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI](index.md)  
Assembly: StardewUI.dll  

</div>

Available behaviors for opening a [ViewMenu&lt;T&gt;](viewmenu-1.md).

```cs
public enum MenuActivationMode
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ [ValueType](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype) ⇦ [Enum](https://learn.microsoft.com/en-us/dotnet/api/system.enum) ⇦ MenuActivationMode

## Fields

 | Name | Value | Description |
| --- | --- | --- |
| <a id="standalone">Standalone</a> | 0 | Opens the menu as standalone, replacing any previously-open menu and all of its descendants. | 
| <a id="child">Child</a> | 1 | Opens the menu as a child of the frontmost game menu that is already active. | 
| <a id="sibling">Sibling</a> | 2 | Replaces the frontmost game menu that is already active, but keeps all of its ancestors. | 

