---
title: KeybindOverlay
description: Overlay control for editing a keybinding, or list of bindings.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class KeybindOverlay

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Widgets.Keybinding](index.md)  
Assembly: StardewUI.dll  

</div>

Overlay control for editing a keybinding, or list of bindings.

```cs
public class KeybindOverlay : StardewUI.Overlays.FullScreenOverlay
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ [FullScreenOverlay](../../overlays/fullscreenoverlay.md) ⇦ KeybindOverlay

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [KeybindOverlay(ISpriteMap&lt;SButton&gt;)](#keybindoverlayispritemapsbutton) | Overlay control for editing a keybinding, or list of bindings. | 

### Properties

 | Name | Description |
| --- | --- |
| [AddButtonText](#addbuttontext) | Text to display on the button used to add a new binding. | 
| [CapturingInput](../../overlays/fullscreenoverlay.md#capturinginput) | Whether the overlay wants to capture all keyboard and gamepad inputs, i.e. prevent them from being dispatched to the parent menu.<br><span class="muted" markdown>(Inherited from [FullScreenOverlay](../../overlays/fullscreenoverlay.md))</span> | 
| [DeleteButtonTooltip](#deletebuttontooltip) | Tooltip to display for the delete (trash can) button beside each existing binding. | 
| [DimmingAmount](../../overlays/fullscreenoverlay.md#dimmingamount) | Amount to dim whatever is underneath the overlay.<br><span class="muted" markdown>(Inherited from [FullScreenOverlay](../../overlays/fullscreenoverlay.md))</span> | 
| [HorizontalAlignment](../../overlays/fullscreenoverlay.md#horizontalalignment) | Horizontal alignment of the overlay relative to the [Parent](../../overlays/ioverlay.md#parent) edge.<br><span class="muted" markdown>(Inherited from [FullScreenOverlay](../../overlays/fullscreenoverlay.md))</span> | 
| [HorizontalParentAlignment](../../overlays/fullscreenoverlay.md#horizontalparentalignment) | Specifies which edge of the [Parent](../../overlays/ioverlay.md#parent) (or screen, if no parent is specified) will be used to align the overlay edge denoted by its [HorizontalAlignment](../../overlays/ioverlay.md#horizontalalignment).<br><span class="muted" markdown>(Inherited from [FullScreenOverlay](../../overlays/fullscreenoverlay.md))</span> | 
| [KeybindList](#keybindlist) | The current keybinds to display in the list. | 
| [KeybindType](#keybindtype) | Specifies what kind of keybind is being edited. | 
| [Parent](../../overlays/fullscreenoverlay.md#parent) | The parent of this overlay, used for positioning. If not specified, then the overlay will be positioned relative to the entire UI viewport.<br><span class="muted" markdown>(Inherited from [FullScreenOverlay](../../overlays/fullscreenoverlay.md))</span> | 
| [ParentOffset](../../overlays/fullscreenoverlay.md#parentoffset) | Additional pixel offset to apply to the overlay's position, after alignments.<br><span class="muted" markdown>(Inherited from [FullScreenOverlay](../../overlays/fullscreenoverlay.md))</span> | 
| [VerticalAlignment](../../overlays/fullscreenoverlay.md#verticalalignment) | Vertical alignment of the overlay relative to the [Parent](../../overlays/ioverlay.md#parent) edge.<br><span class="muted" markdown>(Inherited from [FullScreenOverlay](../../overlays/fullscreenoverlay.md))</span> | 
| [VerticalParentAlignment](../../overlays/fullscreenoverlay.md#verticalparentalignment) | Specifies which edge of the [Parent](../../overlays/ioverlay.md#parent) (or screen, if no parent is specified) will be used to align the overlay edge denoted by its [VerticalAlignment](../../overlays/ioverlay.md#verticalalignment).<br><span class="muted" markdown>(Inherited from [FullScreenOverlay](../../overlays/fullscreenoverlay.md))</span> | 
| [View](../../overlays/fullscreenoverlay.md#view) | The view to be displayed/interacted with as an overlay.<br><span class="muted" markdown>(Inherited from [FullScreenOverlay](../../overlays/fullscreenoverlay.md))</span> | 

### Methods

 | Name | Description |
| --- | --- |
| [CreateView()](#createview) | Creates the content view that will be displayed as an overlay.<br><span class="muted" markdown>(Overrides [FullScreenOverlay](../../overlays/fullscreenoverlay.md).[CreateView()](../../overlays/fullscreenoverlay.md#createview))</span> | 
| [OnClose()](../../overlays/fullscreenoverlay.md#onclose) | Runs when the overlay is removed from the active stack.<br><span class="muted" markdown>(Inherited from [FullScreenOverlay](../../overlays/fullscreenoverlay.md))</span> | 
| [RequireView&lt;TChild&gt;(Func&lt;TChild&gt;)](../../overlays/fullscreenoverlay.md#requireviewtchildfunctchild) | Ensures that the overlay view is created before attempting to access a child view.<br><span class="muted" markdown>(Inherited from [FullScreenOverlay](../../overlays/fullscreenoverlay.md))</span> | 
| [StartCapturing()](#startcapturing) | Starts capturing a new keybind. | 
| [Update(TimeSpan)](#updatetimespan) | Runs on every game update tick.<br><span class="muted" markdown>(Overrides [FullScreenOverlay](../../overlays/fullscreenoverlay.md).[Update(TimeSpan)](../../overlays/fullscreenoverlay.md#updatetimespan))</span> | 

### Events

 | Name | Description |
| --- | --- |
| [Close](../../overlays/fullscreenoverlay.md#close) | Event raised when the overlay is closed - i.e. removed from the current context stack.<br><span class="muted" markdown>(Inherited from [FullScreenOverlay](../../overlays/fullscreenoverlay.md))</span> | 

## Details

### Constructors

#### KeybindOverlay(ISpriteMap&lt;SButton&gt;)

Overlay control for editing a keybinding, or list of bindings.

```cs
public KeybindOverlay(StardewUI.Graphics.ISpriteMap<StardewModdingAPI.SButton> spriteMap);
```

##### Parameters

**`spriteMap`** &nbsp; [ISpriteMap](../../graphics/ispritemap-1.md)<SButton>  
Map of bindable buttons to sprite representations.

-----

### Properties

#### AddButtonText

Text to display on the button used to add a new binding.

```cs
public string AddButtonText { get; set; }
```

##### Property Value

[string](https://learn.microsoft.com/en-us/dotnet/api/system.string)

##### Remarks

If not specified, the button will use a generic "+" image instead.

-----

#### DeleteButtonTooltip

Tooltip to display for the delete (trash can) button beside each existing binding.

```cs
public string DeleteButtonTooltip { get; set; }
```

##### Property Value

[string](https://learn.microsoft.com/en-us/dotnet/api/system.string)

##### Remarks

If not specified, the delete buttons will have no tooltips.

-----

#### KeybindList

The current keybinds to display in the list.

```cs
public StardewModdingAPI.Utilities.KeybindList KeybindList { get; set; }
```

##### Property Value

KeybindList

-----

#### KeybindType

Specifies what kind of keybind is being edited.

```cs
public StardewUI.Widgets.Keybinding.KeybindType KeybindType { get; set; }
```

##### Property Value

[KeybindType](keybindtype.md)

##### Remarks

This determines the behavior of the capturing as well as what happens after capture:  Typically when using single-bind or single-button modes, the caller should [StartCapturing()](keybindoverlay.md#startcapturing) upon creation of the overlay in order to minimize redundant clicks.

-----

### Methods

#### CreateView()

Creates the content view that will be displayed as an overlay.

```cs
protected override StardewUI.IView CreateView();
```

##### Returns

[IView](../../iview.md)

-----

#### StartCapturing()

Starts capturing a new keybind.

```cs
public void StartCapturing();
```

##### Remarks

This makes the capture area start flashing and hides the "Add" button; any buttons pressed in the capturing state are recorded and combined into a single keybind after the capture ends, when all buttons are released.

-----

#### Update(TimeSpan)

Runs on every game update tick.

```cs
public override void Update(System.TimeSpan elapsed);
```

##### Parameters

**`elapsed`** &nbsp; [TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan)  
The amount of real time elapsed since the last tick.

-----

