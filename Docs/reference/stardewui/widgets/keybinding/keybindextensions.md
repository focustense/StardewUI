---
title: KeybindExtensions
description: Extensions for SMAPI's keybind types.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class KeybindExtensions

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Widgets.Keybinding](index.md)  
Assembly: StardewUI.dll  

</div>

Extensions for SMAPI's keybind types.

```cs
public static class KeybindExtensions
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ KeybindExtensions

## Members

### Methods

 | Name | Description |
| --- | --- |
| [KeybindEquals(Keybind, Keybind)](#keybindequalskeybind-keybind) | Checks if two keybind instances are equal. | 
| [KeybindEquals(KeybindList, KeybindList)](#keybindequalskeybindlist-keybindlist) | Checks if two keybind lists are equal. | 

## Details

### Methods

#### KeybindEquals(Keybind, Keybind)

Checks if two keybind instances are equal.

```cs
public static bool KeybindEquals(StardewModdingAPI.Utilities.Keybind keybind, StardewModdingAPI.Utilities.Keybind other);
```

##### Parameters

**`keybind`** &nbsp; Keybind  
The first keybind to compare.

**`other`** &nbsp; Keybind  
The second keybind to compare.

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  `true` if `keybind` has the same Buttons as `other` and in the same order, otherwise `false`.

##### Remarks

The comparison is order-sensitive; two keybinds with the same buttons in a different order are considered to be unequal to each other.

-----

#### KeybindEquals(KeybindList, KeybindList)

Checks if two keybind lists are equal.

```cs
public static bool KeybindEquals(StardewModdingAPI.Utilities.KeybindList keybindList, StardewModdingAPI.Utilities.KeybindList other);
```

##### Parameters

**`keybindList`** &nbsp; KeybindList  
The first keybind list to compare.

**`other`** &nbsp; KeybindList  
The second keybind list to compare.

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  `true` if both `keybindList` and `other` contain the exact same keybinds in the same order, otherwise `false`.

##### Remarks

The comparison is order-sensitive; two keybind lists with the same keybinds in a different order, or with any two keybinds having keys in a different order

-----

