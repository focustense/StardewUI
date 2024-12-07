---
title: ButtonResolver
description: Helper for resolving button state reported by vanilla menu code.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class ButtonResolver

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Input](index.md)  
Assembly: StardewUI.dll  

</div>

Helper for resolving button state reported by vanilla menu code.

```cs
public static class ButtonResolver
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ ButtonResolver

## Remarks

Stardew's menu system is quite obnoxious about trying to "simplify" button handling and doesn't provide a lot of escape hatches. In addition, the buttons it considers to be the same are not equivalent to the way normal gameplay operates; for example, gamepad controls use A as the action button which is the same as a right-click when interacting with the game world; however, in menus A is the same as a left-click while X is right click. 

 Going through this class can help identify the correct "function" of a button in a UI context as well as identify which real button was actually pressed, the better to work with input suppressions and similar concerns.

## Members

### Methods

 | Name | Description |
| --- | --- |
| [GetActionButtons(ButtonAction)](#getactionbuttonsbuttonaction) | Gets all buttons that can be resolved to a specific action. | 
| [GetButtonAction(SButton)](#getbuttonactionsbutton) | Determines the action that should be performed by a button. | 
| [GetPressedButton(SButton)](#getpressedbuttonsbutton) | Attempts to determine the actual physically pressed key for a "representative" or logical button reported by the underlying menu system that may not actually be down. | 

## Details

### Methods

#### GetActionButtons(ButtonAction)

Gets all buttons that can be resolved to a specific action.

```cs
public static System.Collections.Generic.IEnumerable<StardewModdingAPI.SButton> GetActionButtons(StardewUI.Input.ButtonAction action);
```

##### Parameters

**`action`** &nbsp; [ButtonAction](buttonaction.md)  
The UI action.

##### Returns

[IEnumerable](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)<SButton>

  A sequence of SButton elements each of which is considered to perform the specified `action`.

-----

#### GetButtonAction(SButton)

Determines the action that should be performed by a button.

```cs
public static StardewUI.Input.ButtonAction GetButtonAction(StardewModdingAPI.SButton button);
```

##### Parameters

**`button`** &nbsp; SButton  
The action button.

##### Returns

[ButtonAction](buttonaction.md)

  The action for the specified `button`.

-----

#### GetPressedButton(SButton)

Attempts to determine the actual physically pressed key for a "representative" or logical button reported by the underlying menu system that may not actually be down.

```cs
public static StardewModdingAPI.SButton GetPressedButton(StardewModdingAPI.SButton logicalButton);
```

##### Parameters

**`logicalButton`** &nbsp; SButton  
The button that was reported.

##### Returns

SButton

  A button that performs the same action as `logicalButton` and is currently pressed; or the original `logicalButton` if no match can be found.

-----

