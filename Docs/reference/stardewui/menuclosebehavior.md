---
title: MenuCloseBehavior
description: Available behaviors for closing a ViewMenu&lt;T&gt;.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Enum MenuCloseBehavior

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI](index.md)  
Assembly: StardewUI.dll  

</div>

Available behaviors for closing a [ViewMenu&lt;T&gt;](viewmenu-1.md).

```cs
public enum MenuCloseBehavior
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ [ValueType](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype) ⇦ [Enum](https://learn.microsoft.com/en-us/dotnet/api/system.enum) ⇦ MenuCloseBehavior

## Fields

 | Name | Value | Description |
| --- | --- | --- |
| <a id="default">Default</a> | 0 | Use the game's default closing logic. | 
| <a id="custom">Custom</a> | 1 | Block the game's default closing logic, but allow the menu to be closed explicitly via its [Close()](viewmenu-1.md#close) method. | 
| <a id="disabled">Disabled</a> | 2 | Do not allow the menu to be closed by any means. | 

