---
title: IPageable
description: Signals that an IView implements paging controls.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Interface IPageable

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Layout](index.md)  
Assembly: StardewUI.dll  

</div>

Signals that an [IView](../iview.md) implements paging controls.

```cs
public interface IPageable
```

## Remarks

Paging controls are a gamepad function. While next/previous arrows are usually clickable and have their own navigation logic, controller users generally should be able to use the shoulder buttons to navigate pages, which this interface enables.

## Members

### Methods

 | Name | Description |
| --- | --- |
| [NextPage()](#nextpage) | Advance to the next page, within the current tab. | 
| [PreviousPage()](#previouspage) | Advance to the previous page, within the current tab. | 

## Details

### Methods

#### NextPage()

Advance to the next page, within the current tab.

```cs
bool NextPage();
```

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  `true` if any navigation was performed; `false` if there are no more pages.

-----

#### PreviousPage()

Advance to the previous page, within the current tab.

```cs
bool PreviousPage();
```

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  `true` if any navigation was performed; `false` if there are no more pages.

-----

