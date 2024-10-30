---
title: ITabbable
description: Signals that an IView implements tab controls.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Interface ITabbable

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Layout](index.md)  
Assembly: StardewUI.dll  

</div>

Signals that an [IView](../iview.md) implements tab controls.

```cs
public interface ITabbable
```

## Remarks

Tab controls are a gamepad function. While tabs are usually clickable and have their own navigation logic, controller users should be able to use the trigger buttons to navigate tabs, which this interface enables.

## Members

### Methods

 | Name | Description |
| --- | --- |
| [NextTab()](#nexttab) | Advance to the next top-level tab. | 
| [PreviousTab()](#previoustab) | Advance to the previous top-level tab. | 

## Details

### Methods

#### NextTab()

Advance to the next top-level tab.

```cs
bool NextTab();
```

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  `true` if any navigation was performed; `false` if there are no more tabs.

-----

#### PreviousTab()

Advance to the previous top-level tab.

```cs
bool PreviousTab();
```

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  `true` if any navigation was performed; `false` if there are no more tabs.

-----

