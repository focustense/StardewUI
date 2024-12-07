---
title: KeybindType
description: Specifies the exact type of keybind supported by a widget using a KeybindList.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Enum KeybindType

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Widgets.Keybinding](index.md)  
Assembly: StardewUI.dll  

</div>

Specifies the exact type of keybind supported by a widget using a KeybindList.

```cs
public enum KeybindType
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ [ValueType](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype) ⇦ [Enum](https://learn.microsoft.com/en-us/dotnet/api/system.enum) ⇦ KeybindType

## Fields

 | Name | Value | Description |
| --- | --- | --- |
| <a id="singlebutton">SingleButton</a> | 0 | The binding is a single SButton and does not support key combinations. | 
| <a id="singlekeybind">SingleKeybind</a> | 1 | The binding is a single Keybind, supporting exactly one combination of keys that must all be pressed. | 
| <a id="multiplekeybinds">MultipleKeybinds</a> | 2 | The binding is a real KeybindList, which can handle any number of individual Keybinds each with their own key combination. | 

