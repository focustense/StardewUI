---
title: StardewUI.Framework.Behaviors
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# StardewUI.Framework.Behaviors Namespace

## Classes

| Name | Description |
| --- | --- |
| [BehaviorFactory](behaviorfactory.md) | A behavior factory based on per-name delegates. Can be used as a base class for other factories. |
| [BehaviorTarget](behaviortarget.md) | Encapsulates the target of an [IViewBehavior](iviewbehavior.md). |
| [ConditionalFlagBehavior](conditionalflagbehavior.md) | Updates a view state flag with a boolean value corresponding to the behavior's data. |
| [FlagEventArgs](flageventargs.md) | Arguments for events relating to a flag included in an [IViewState](iviewstate.md). |
| [FlagStateBehavior&lt;TValue&gt;](flagstatebehavior-1.md) | Behavior that applies a property override when a named view state flag is detected. |
| [HoverStateBehavior&lt;TValue&gt;](hoverstatebehavior-1.md) | Behavior that applies a property override when a view enters a hover state. |
| [PropertyStateList&lt;T&gt;](propertystatelist-1.md) | Simple list-based implementation of [IPropertyStates&lt;T&gt;](ipropertystates-1.md) optimized for low override counts, typically fewer than 5 and never more than 10-20. |
| [StateBehaviorFactory](statebehaviorfactory.md) | Factory for creating behaviors that apply single-property overrides on state transitions, such as `hover:transform`. |
| [TransitionBehavior&lt;TValue&gt;](transitionbehavior-1.md) | Behavior that applies gradual transitions (AKA tweens) to view properties. |
| [ViewBehavior&lt;TView, TData&gt;](viewbehavior-2.md) | Base class for a behavior extension, which enables self-contained, stateful behaviors to be "attached" to an arbitrary view without having to extend the view itself. |
| [ViewState](viewstate.md) | View state manager based on the view's runtime descriptor and defaults. |
| [VisibilityStateBehavior&lt;TValue&gt;](visibilitystatebehavior-1.md) | Behavior that applies a property override when a view enters or leaves the visible state. |

## Interfaces

| Name | Description |
| --- | --- |
| [IBehaviorFactory](ibehaviorfactory.md) | Factory for creating [IViewBehavior](iviewbehavior.md) instances from markup data. |
| [IPropertyStates&lt;T&gt;](ipropertystates-1.md) | Provides methods for tracking and modifying state-based overrides for a view's property. |
| [IViewBehavior](iviewbehavior.md) | Provides methods for attaching arbitrary data-dependent behavior to a view. |
| [IViewState](iviewstate.md) | Provides access to all state-based overrides associated with a view. |

