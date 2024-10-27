---
title: StardewUI.Widgets
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# StardewUI.Widgets Namespace

## Classes

| Name | Description |
| --- | --- |
| [Banner](banner.md) | Draws banner-style text with an optional background. |
| [Button](button.md) | Simple button with optional hover background. |
| [CheckBox](checkbox.md) | A togglable checkbox. |
| [ComponentView](componentview.md) | Base class for custom widgets and "app views" with potentially complex hierarchy using a single root view. |
| [ComponentView&lt;T&gt;](componentview-1.md) | Base class for custom widgets and "app views" with potentially complex hierarchy using a single root view. |
| [DecoratorView](decoratorview.md) | A view that owns and delegates to an inner view. |
| [DecoratorView&lt;T&gt;](decoratorview-1.md) | A view that owns and delegates to an inner view. |
| [DecoratorView&lt;T&gt;](decoratorview-1.md).[DecoratedProperty&lt;T, TValue&gt;](decoratorview-1.decoratedproperty-1.md) | Helper for propagating a single property to and from the inner view. |
| [DropDownList&lt;T&gt;](dropdownlist-1.md) | Button/text field with a drop-down menu. |
| [Expander](expander.md) | A widget that can be clicked to expand/collapse with additional content. |
| [FormBuilder](formbuilder.md) | Fluent builder style API for creating form-like tables within a view. |
| [Frame](frame.md) | A view that holds another view, typically for the purpose of adding a border or background, or in some cases swapping out the content. |
| [GhostView](ghostview.md) | A view that draws an exact copy of another view, generally with a tint and transparency to indicate that it is not the original view. Can be used for dragging, indicating target snap positions, etc. |
| [Grid](grid.md) | A uniform grid containing other views. |
| [GridItemLayout](griditemlayout.md) | Describes the layout of all items in a [Grid](grid.md). |
| [GridItemLayout](griditemlayout.md).[Count](griditemlayout.count.md) | A [GridItemLayout](griditemlayout.md) specifying the maximum divisions - rows or columns, depending on the grid's [Orientation](../layout/orientation.md); items will be sized distributed uniformly along that axis. |
| [GridItemLayout](griditemlayout.md).[Length](griditemlayout.length.md) | A [GridItemLayout](griditemlayout.md) specifying that each item is to have the same fixed length (width or height, depending on the grid's [Orientation](../layout/orientation.md)) and to wrap to the next row/column afterward. |
| [Image](image.md) | A view that draws a sprite, scaled to the layout size. |
| [Label](label.md) | A view that renders a read-only text string. |
| [Lane](lane.md) | Simple unidirectional layout that draws multiple child views in a row or column arrangement. |
| [Marquee](marquee.md) | A scrolling marquee supporting any inner content. |
| [NineGridPlacementEditor](ninegridplacementeditor.md) | Editor widget for a [NineGridPlacement](../layout/ninegridplacement.md), which brings up a [PositioningOverlay](positioningoverlay.md) on click. |
| [OutletAttribute](outletattribute.md) | Marks a child/children property as a named outlet. |
| [Panel](panel.md) | A layout view whose children all overlap the same boundaries. |
| [PositioningOverlay](positioningoverlay.md) | An overlay that can be used to edit the position of some arbitrary content. |
| [PositioningOverlay](positioningoverlay.md).[ControlScheme](positioningoverlay.controlscheme.md) | Configures the mapping of buttons to positioning actions in a [PositioningOverlay](positioningoverlay.md). |
| [PositioningOverlay](positioningoverlay.md).[GamepadControlScheme](positioningoverlay.gamepadcontrolscheme.md) | Configures the mapping of buttons to positioning actions in a [PositioningOverlay](positioningoverlay.md). Includes the generic [ControlScheme](positioningoverlay.controlscheme.md) settings as well as grid-movement settings specific to gamepads. |
| [ScrollableFrameView](scrollableframeview.md) | Layout widget for a sectioned menu including a scrollable content area. |
| [ScrollableView](scrollableview.md) | Provides a content container and accompanying scrollbar. |
| [Scrollbar](scrollbar.md) | Controls the scrolling of a [ScrollContainer](scrollcontainer.md). |
| [ScrollContainer](scrollcontainer.md) | Renders inner content clipped to a boundary and with a modifiable scroll offset. |
| [Slider](slider.md) | A horizontal track with draggable thumb (button) for choosing a numeric value in a range. |
| [Spacer](spacer.md) | An empty view whose sole purpose is to separate other elements. |
| [TextInput](textinput.md) | A text input field that allows typing from a physical or virtual keyboard. |
| [TinyNumberLabel](tinynumberlabel.md) | Renders a single-line numeric label using custom digit sprites. |

## Enums

| Name | Description |
| --- | --- |
| [ImageFit](imagefit.md) | Specifies how an image should be scaled to fit the content area when the available size is different from the image size, and especially when it has a different aspect ratio. |

