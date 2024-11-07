---
title: StardewUI.Framework
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# StardewUI.Framework Namespace

## Classes

| Name | Description |
| --- | --- |
| [BackoffRule](backoffrule.md) | Defines the rules for exponential backoff. |
| [BackoffState](backoffstate.md) | State of an exponential backoff, e.g. as used in a `StardewUI.Framework.BackoffTracker<T>`. |
| [ModConfig](modconfig.md) | Configuration settings for StardewUI.Framework. |
| [UIException](uiexception.md) | Base class for all exceptions specific to StardewUI. |

## Interfaces

| Name | Description |
| --- | --- |
| [IAnyCast](ianycast.md) | A marker interface that, when used in place of [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object), forces the framework to attempt an explicit conversion/cast to the expected destination type. |
| [IViewDrawable](iviewdrawable.md) | Provides methods to update and draw a simple, non-interactive UI component, such as a HUD widget. |
| [IViewEngine](iviewengine.md) | Public API for StardewUI, abstracting away all implementation details of views and trees. |

