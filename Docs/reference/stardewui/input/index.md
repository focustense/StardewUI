---
title: StardewUI.Input
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# StardewUI.Input Namespace

## Classes

| Name | Description |
| --- | --- |
| [ActionRepeat](actionrepeat.md) | Configures the repeat rate of an action used in an [ActionState&lt;T&gt;](actionstate-1.md). |
| [ActionState&lt;T&gt;](actionstate-1.md) | Translates raw input to semantic actions. |
| [ButtonResolver](buttonresolver.md) | Helper for resolving button state reported by vanilla menu code. |
| [FocusSearchResult](focussearchresult.md) | The result of a [FocusSearch(Vector2, Direction)](../iview.md#focussearchvector2-direction). Identifies the specific view/position found, as well as the path to that view from the search root. |

## Interfaces

| Name | Description |
| --- | --- |
| [ICaptureTarget](icapturetarget.md) | Denotes a view or other UI element that can be the active IKeyboardSubscriber. Allows view hosts to provide deterministic release, e.g. when the mouse is clicked outside the target. |

## Enums

| Name | Description |
| --- | --- |
| [ButtonAction](buttonaction.md) | The actions that a given button can trigger in a UI context. For details see [ButtonResolver](buttonresolver.md). |

