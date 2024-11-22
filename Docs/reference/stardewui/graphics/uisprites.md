---
title: UiSprites
description: Included game sprites that are required for many UI/menu widgets.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class UiSprites

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Graphics](index.md)  
Assembly: StardewUI.dll  

</div>

Included game sprites that are required for many UI/menu widgets.

```cs
public static class UiSprites
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ UiSprites

## Members

### Properties

 | Name | Description |
| --- | --- |
| [BannerBackground](#bannerbackground) | Background for the a banner or "scroll" style text, often used for menu/dialogue titles. | 
| [ButtonDark](#buttondark) | Button with a darker background, usually the neutral state. | 
| [ButtonLight](#buttonlight) | Button with a lighter background, usually used to show hover state. | 
| [CaretLeft](#caretleft) | A caret-style directional arrow pointing left. | 
| [CaretRight](#caretright) | A caret-style directional arrow pointing right. | 
| [CheckboxChecked](#checkboxchecked) | Checkbox with a green "X" through it. | 
| [CheckboxUnchecked](#checkboxunchecked) | Unchecked checkbox, i.e. only the border. | 
| [CloseButton](#closebutton) | Red X with border/background, generally used as upper-right close button for menus. | 
| [ControlBorder](#controlborder) | Border/background sprite for an individual control, such as a button. Less prominent than [MenuBorder](uisprites.md#menuborder). | 
| [ControlBorderUncolored](#controlborderuncolored) | Colorless border/background sprite for an individual control, such as a button. Less prominent than [MenuBorderUncolored](uisprites.md#menuborderuncolored). | 
| [Digits](#digits) | List of sprites for the outlined "tiny digits" 0-9, in that order. | 
| [DropDownBackground](#dropdownbackground) | Background of a drop-down menu. | 
| [DropDownButton](#dropdownbutton) | Button to pull down a drop-down menu. | 
| [GenericHorizontalDivider](#generichorizontaldivider) | Simpler, lighter horizontal divider than the [MenuHorizontalDivider](uisprites.md#menuhorizontaldivider), used as a horizontal rule to separate content areas without sectioning the entire menu. | 
| [GenericHorizontalDividerUncolored](#generichorizontaldivideruncolored) | Colorless horizontal divider that is simpler and lighter the [MenuHorizontalDividerUncolored](uisprites.md#menuhorizontaldivideruncolored), used as a horizontal rule to separate content areas without sectioning the entire menu. | 
| [LargeDownArrow](#largedownarrow) | Large down arrow, used for macro navigation. | 
| [LargeLeftArrow](#largeleftarrow) | Large left arrow, used for macro navigation. | 
| [LargeRightArrow](#largerightarrow) | Large right arrow, used for macro navigation. | 
| [LargeUpArrow](#largeuparrow) | Large up arrow, used for macro navigation. | 
| [MenuBackground](#menubackground) | Background used for the in-game menu, not including borders. | 
| [MenuBackgroundUncolored](#menubackgrounduncolored) | Colorless background used for the in-game menu, not including borders. | 
| [MenuBorder](#menuborder) | Modified 9-slice sprite used for the menu border, based on menu "tiles". Used for drawing the outer border of an entire menu UI. | 
| [MenuBorderThickness](#menuborderthickness) | The actual distance from the outer edges of the [MenuBorder](uisprites.md#menuborder) sprite to where the actual "border" really ends, in terms of pixels. The border tiles are quite large, so this tends to be needed in order to determine where the content should go without adding a ton of extra padding. | 
| [MenuBorderUncolored](#menuborderuncolored) | Colorless version of the modified 9-slice sprite used for the menu border, based on menu "tiles". Used for drawing the outer border of an entire menu UI. | 
| [MenuHorizontalDivider](#menuhorizontaldivider) | Modified 9-slice sprite used for the menu's horizontal divider, meant to be drawn over top of the [MenuBorder](uisprites.md#menuborder) to denote separate "sub-panels" or "sections" of the menu to group logically very different menu functions (as opposed to lines on a grid). | 
| [MenuHorizontalDividerMargin](#menuhorizontaldividermargin) | Margin adjustment to apply to content adjacent to a [MenuHorizontalDivider](uisprites.md#menuhorizontaldivider) to make content flush with the border; adjusts for internal sprite padding. | 
| [MenuHorizontalDividerUncolored](#menuhorizontaldivideruncolored) | Colorless version of the modified 9-slice sprite used for the menu's horizontal divider, meant to be drawn over top of the [MenuBorderUncolored](uisprites.md#menuborderuncolored) to denote separate "sub-panels" or "sections" of the menu to group logically very different menu functions (as opposed to lines on a grid). | 
| [MenuSlotInset](#menuslotinset) | Inset-style background and border, often used to hold an item or represent a slot. | 
| [MenuSlotInsetUncolored](#menuslotinsetuncolored) | Colorless inset-style background and border, often used to hold an item or represent a slot. | 
| [MenuSlotOutset](#menuslotoutset) | Outset-style background and border, often used to hold an item or represent a slot. | 
| [MenuSlotTransparent](#menuslottransparent) | Single-line rectangular border with a slight inset look. | 
| [MenuSlotTransparentUncolored](#menuslottransparentuncolored) | Colorless single-line rectangular border with a slight inset look. | 
| [MenuVerticalDivider](#menuverticaldivider) | Modified 9-slice sprite used for the menu's vertical divider, meant to be drawn over top of the [MenuBorder](uisprites.md#menuborder) to denote separate "sub-panels" or "sections" of the menu to group logically very different menu functions (as opposed to lines on a grid). | 
| [MenuVerticalDividerMargin](#menuverticaldividermargin) | Margin adjustment to apply to content adjacent to a [MenuVerticalDivider](uisprites.md#menuverticaldivider) to make content flush with the border; adjusts for internal sprite padding. | 
| [MenuVerticalDividerUncolored](#menuverticaldivideruncolored) | Colorless version of the modified 9-slice sprite used for the menu's vertical divider, meant to be drawn over top of the [MenuBorderUncolored](uisprites.md#menuborderuncolored) to denote separate "sub-panels" or "sections" of the menu to group logically very different menu functions (as opposed to lines on a grid). | 
| [ScrollBarTrack](#scrollbartrack) | Background for the scroll bar track (which the thumb is inside). | 
| [SliderBackground](#sliderbackground) | Background of a slider control. | 
| [SliderButton](#sliderbutton) | The movable part of a slider control ("button"). | 
| [SmallDownArrow](#smalldownarrow) | Small down arrow, typically used for scroll bars. | 
| [SmallGreenPlus](#smallgreenplus) | A small green "+" icon. | 
| [SmallLeftArrow](#smallleftarrow) | Small left arrow, typically used for top-level list navigation. | 
| [SmallRightArrow](#smallrightarrow) | Small right arrow, typically used for top-level list navigation. | 
| [SmallTrashCan](#smalltrashcan) | Small and tall trash can, larger than the [TinyTrashCan](uisprites.md#tinytrashcan) and more suitable for tall rows. | 
| [SmallUpArrow](#smalluparrow) | Small up arrow, typically used for scroll bars. | 
| [TabTopEmpty](#tabtopempty) | Top-facing tab with no inner content, used for tab controls. | 
| [TextBox](#textbox) | Border/background for a text input box. | 
| [ThinHorizontalDivider](#thinhorizontaldivider) | Simple horizontal divider, typically used to divide sections of uniform content, e.g. grid rows. | 
| [ThinHorizontalDividerUncolored](#thinhorizontaldivideruncolored) | Simple, colorless horizontal divider, typically used to divide sections of uniform content, e.g. grid rows. | 
| [ThinVerticalDivider](#thinverticaldivider) | Simple vertical divider, typically used to divide sections of uniform content, e.g. grid columns. | 
| [ThinVerticalDividerUncolored](#thinverticaldivideruncolored) | Simple, colorless vertical divider, typically used to divide sections of uniform content, e.g. grid columns. | 
| [TinyTrashCan](#tinytrashcan) | Very small trash can, e.g. to be used in lists/subforms as "remove" button. | 
| [VerticalScrollThumb](#verticalscrollthumb) | Thumb sprite used for vertical scroll bars. | 
| [White](#white) | A single white pixel. | 

## Details

### Properties

#### BannerBackground

Background for the a banner or "scroll" style text, often used for menu/dialogue titles.

```cs
public static StardewUI.Graphics.Sprite BannerBackground { get; }
```

##### Property Value

[Sprite](sprite.md)

-----

#### ButtonDark

Button with a darker background, usually the neutral state.

```cs
public static StardewUI.Graphics.Sprite ButtonDark { get; }
```

##### Property Value

[Sprite](sprite.md)

-----

#### ButtonLight

Button with a lighter background, usually used to show hover state.

```cs
public static StardewUI.Graphics.Sprite ButtonLight { get; }
```

##### Property Value

[Sprite](sprite.md)

-----

#### CaretLeft

A caret-style directional arrow pointing left.

```cs
public static StardewUI.Graphics.Sprite CaretLeft { get; }
```

##### Property Value

[Sprite](sprite.md)

##### Remarks

Can be used to show expanded/collapsed state, or illustrate a movement direction.

-----

#### CaretRight

A caret-style directional arrow pointing right.

```cs
public static StardewUI.Graphics.Sprite CaretRight { get; }
```

##### Property Value

[Sprite](sprite.md)

##### Remarks

Can be used to show expanded/collapsed state, or illustrate a movement direction.

-----

#### CheckboxChecked

Checkbox with a green "X" through it.

```cs
public static StardewUI.Graphics.Sprite CheckboxChecked { get; }
```

##### Property Value

[Sprite](sprite.md)

-----

#### CheckboxUnchecked

Unchecked checkbox, i.e. only the border.

```cs
public static StardewUI.Graphics.Sprite CheckboxUnchecked { get; }
```

##### Property Value

[Sprite](sprite.md)

-----

#### CloseButton

Red X with border/background, generally used as upper-right close button for menus.

```cs
public static StardewUI.Graphics.Sprite CloseButton { get; }
```

##### Property Value

[Sprite](sprite.md)

-----

#### ControlBorder

Border/background sprite for an individual control, such as a button. Less prominent than [MenuBorder](uisprites.md#menuborder).

```cs
public static StardewUI.Graphics.Sprite ControlBorder { get; }
```

##### Property Value

[Sprite](sprite.md)

-----

#### ControlBorderUncolored

Colorless border/background sprite for an individual control, such as a button. Less prominent than [MenuBorderUncolored](uisprites.md#menuborderuncolored).

```cs
public static StardewUI.Graphics.Sprite ControlBorderUncolored { get; }
```

##### Property Value

[Sprite](sprite.md)

-----

#### Digits

List of sprites for the outlined "tiny digits" 0-9, in that order.

```cs
public static System.Collections.Generic.IReadOnlyList<StardewUI.Graphics.Sprite> Digits { get; }
```

##### Property Value

[IReadOnlyList](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlylist-1)<[Sprite](sprite.md)>

-----

#### DropDownBackground

Background of a drop-down menu.

```cs
public static StardewUI.Graphics.Sprite DropDownBackground { get; }
```

##### Property Value

[Sprite](sprite.md)

-----

#### DropDownButton

Button to pull down a drop-down menu.

```cs
public static StardewUI.Graphics.Sprite DropDownButton { get; }
```

##### Property Value

[Sprite](sprite.md)

-----

#### GenericHorizontalDivider

Simpler, lighter horizontal divider than the [MenuHorizontalDivider](uisprites.md#menuhorizontaldivider), used as a horizontal rule to separate content areas without sectioning the entire menu.

```cs
public static StardewUI.Graphics.Sprite GenericHorizontalDivider { get; }
```

##### Property Value

[Sprite](sprite.md)

-----

#### GenericHorizontalDividerUncolored

Colorless horizontal divider that is simpler and lighter the [MenuHorizontalDividerUncolored](uisprites.md#menuhorizontaldivideruncolored), used as a horizontal rule to separate content areas without sectioning the entire menu.

```cs
public static StardewUI.Graphics.Sprite GenericHorizontalDividerUncolored { get; }
```

##### Property Value

[Sprite](sprite.md)

-----

#### LargeDownArrow

Large down arrow, used for macro navigation.

```cs
public static StardewUI.Graphics.Sprite LargeDownArrow { get; }
```

##### Property Value

[Sprite](sprite.md)

-----

#### LargeLeftArrow

Large left arrow, used for macro navigation.

```cs
public static StardewUI.Graphics.Sprite LargeLeftArrow { get; }
```

##### Property Value

[Sprite](sprite.md)

-----

#### LargeRightArrow

Large right arrow, used for macro navigation.

```cs
public static StardewUI.Graphics.Sprite LargeRightArrow { get; }
```

##### Property Value

[Sprite](sprite.md)

-----

#### LargeUpArrow

Large up arrow, used for macro navigation.

```cs
public static StardewUI.Graphics.Sprite LargeUpArrow { get; }
```

##### Property Value

[Sprite](sprite.md)

-----

#### MenuBackground

Background used for the in-game menu, not including borders.

```cs
public static StardewUI.Graphics.Sprite MenuBackground { get; }
```

##### Property Value

[Sprite](sprite.md)

-----

#### MenuBackgroundUncolored

Colorless background used for the in-game menu, not including borders.

```cs
public static StardewUI.Graphics.Sprite MenuBackgroundUncolored { get; }
```

##### Property Value

[Sprite](sprite.md)

-----

#### MenuBorder

Modified 9-slice sprite used for the menu border, based on menu "tiles". Used for drawing the outer border of an entire menu UI.

```cs
public static StardewUI.Graphics.Sprite MenuBorder { get; }
```

##### Property Value

[Sprite](sprite.md)

-----

#### MenuBorderThickness

The actual distance from the outer edges of the [MenuBorder](uisprites.md#menuborder) sprite to where the actual "border" really ends, in terms of pixels. The border tiles are quite large, so this tends to be needed in order to determine where the content should go without adding a ton of extra padding.

```cs
public static StardewUI.Layout.Edges MenuBorderThickness { get; }
```

##### Property Value

[Edges](../layout/edges.md)

-----

#### MenuBorderUncolored

Colorless version of the modified 9-slice sprite used for the menu border, based on menu "tiles". Used for drawing the outer border of an entire menu UI.

```cs
public static StardewUI.Graphics.Sprite MenuBorderUncolored { get; }
```

##### Property Value

[Sprite](sprite.md)

-----

#### MenuHorizontalDivider

Modified 9-slice sprite used for the menu's horizontal divider, meant to be drawn over top of the [MenuBorder](uisprites.md#menuborder) to denote separate "sub-panels" or "sections" of the menu to group logically very different menu functions (as opposed to lines on a grid).

```cs
public static StardewUI.Graphics.Sprite MenuHorizontalDivider { get; }
```

##### Property Value

[Sprite](sprite.md)

-----

#### MenuHorizontalDividerMargin

Margin adjustment to apply to content adjacent to a [MenuHorizontalDivider](uisprites.md#menuhorizontaldivider) to make content flush with the border; adjusts for internal sprite padding.

```cs
public static StardewUI.Layout.Edges MenuHorizontalDividerMargin { get; }
```

##### Property Value

[Edges](../layout/edges.md)

-----

#### MenuHorizontalDividerUncolored

Colorless version of the modified 9-slice sprite used for the menu's horizontal divider, meant to be drawn over top of the [MenuBorderUncolored](uisprites.md#menuborderuncolored) to denote separate "sub-panels" or "sections" of the menu to group logically very different menu functions (as opposed to lines on a grid).

```cs
public static StardewUI.Graphics.Sprite MenuHorizontalDividerUncolored { get; }
```

##### Property Value

[Sprite](sprite.md)

-----

#### MenuSlotInset

Inset-style background and border, often used to hold an item or represent a slot.

```cs
public static StardewUI.Graphics.Sprite MenuSlotInset { get; }
```

##### Property Value

[Sprite](sprite.md)

-----

#### MenuSlotInsetUncolored

Colorless inset-style background and border, often used to hold an item or represent a slot.

```cs
public static StardewUI.Graphics.Sprite MenuSlotInsetUncolored { get; }
```

##### Property Value

[Sprite](sprite.md)

-----

#### MenuSlotOutset

Outset-style background and border, often used to hold an item or represent a slot.

```cs
public static StardewUI.Graphics.Sprite MenuSlotOutset { get; }
```

##### Property Value

[Sprite](sprite.md)

-----

#### MenuSlotTransparent

Single-line rectangular border with a slight inset look.

```cs
public static StardewUI.Graphics.Sprite MenuSlotTransparent { get; }
```

##### Property Value

[Sprite](sprite.md)

-----

#### MenuSlotTransparentUncolored

Colorless single-line rectangular border with a slight inset look.

```cs
public static StardewUI.Graphics.Sprite MenuSlotTransparentUncolored { get; }
```

##### Property Value

[Sprite](sprite.md)

-----

#### MenuVerticalDivider

Modified 9-slice sprite used for the menu's vertical divider, meant to be drawn over top of the [MenuBorder](uisprites.md#menuborder) to denote separate "sub-panels" or "sections" of the menu to group logically very different menu functions (as opposed to lines on a grid).

```cs
public static StardewUI.Graphics.Sprite MenuVerticalDivider { get; }
```

##### Property Value

[Sprite](sprite.md)

-----

#### MenuVerticalDividerMargin

Margin adjustment to apply to content adjacent to a [MenuVerticalDivider](uisprites.md#menuverticaldivider) to make content flush with the border; adjusts for internal sprite padding.

```cs
public static StardewUI.Layout.Edges MenuVerticalDividerMargin { get; }
```

##### Property Value

[Edges](../layout/edges.md)

-----

#### MenuVerticalDividerUncolored

Colorless version of the modified 9-slice sprite used for the menu's vertical divider, meant to be drawn over top of the [MenuBorderUncolored](uisprites.md#menuborderuncolored) to denote separate "sub-panels" or "sections" of the menu to group logically very different menu functions (as opposed to lines on a grid).

```cs
public static StardewUI.Graphics.Sprite MenuVerticalDividerUncolored { get; }
```

##### Property Value

[Sprite](sprite.md)

-----

#### ScrollBarTrack

Background for the scroll bar track (which the thumb is inside).

```cs
public static StardewUI.Graphics.Sprite ScrollBarTrack { get; }
```

##### Property Value

[Sprite](sprite.md)

-----

#### SliderBackground

Background of a slider control.

```cs
public static StardewUI.Graphics.Sprite SliderBackground { get; }
```

##### Property Value

[Sprite](sprite.md)

-----

#### SliderButton

The movable part of a slider control ("button").

```cs
public static StardewUI.Graphics.Sprite SliderButton { get; }
```

##### Property Value

[Sprite](sprite.md)

-----

#### SmallDownArrow

Small down arrow, typically used for scroll bars.

```cs
public static StardewUI.Graphics.Sprite SmallDownArrow { get; }
```

##### Property Value

[Sprite](sprite.md)

-----

#### SmallGreenPlus

A small green "+" icon.

```cs
public static StardewUI.Graphics.Sprite SmallGreenPlus { get; }
```

##### Property Value

[Sprite](sprite.md)

##### Remarks

Technically used to represent energy buffs, can sometimes be tinted to communicate a concept like "add to list".

-----

#### SmallLeftArrow

Small left arrow, typically used for top-level list navigation.

```cs
public static StardewUI.Graphics.Sprite SmallLeftArrow { get; }
```

##### Property Value

[Sprite](sprite.md)

-----

#### SmallRightArrow

Small right arrow, typically used for top-level list navigation.

```cs
public static StardewUI.Graphics.Sprite SmallRightArrow { get; }
```

##### Property Value

[Sprite](sprite.md)

-----

#### SmallTrashCan

Small and tall trash can, larger than the [TinyTrashCan](uisprites.md#tinytrashcan) and more suitable for tall rows.

```cs
public static StardewUI.Graphics.Sprite SmallTrashCan { get; }
```

##### Property Value

[Sprite](sprite.md)

-----

#### SmallUpArrow

Small up arrow, typically used for scroll bars.

```cs
public static StardewUI.Graphics.Sprite SmallUpArrow { get; }
```

##### Property Value

[Sprite](sprite.md)

-----

#### TabTopEmpty

Top-facing tab with no inner content, used for tab controls.

```cs
public static StardewUI.Graphics.Sprite TabTopEmpty { get; }
```

##### Property Value

[Sprite](sprite.md)

-----

#### TextBox

Border/background for a text input box.

```cs
public static StardewUI.Graphics.Sprite TextBox { get; }
```

##### Property Value

[Sprite](sprite.md)

-----

#### ThinHorizontalDivider

Simple horizontal divider, typically used to divide sections of uniform content, e.g. grid rows.

```cs
public static StardewUI.Graphics.Sprite ThinHorizontalDivider { get; }
```

##### Property Value

[Sprite](sprite.md)

-----

#### ThinHorizontalDividerUncolored

Simple, colorless horizontal divider, typically used to divide sections of uniform content, e.g. grid rows.

```cs
public static StardewUI.Graphics.Sprite ThinHorizontalDividerUncolored { get; }
```

##### Property Value

[Sprite](sprite.md)

-----

#### ThinVerticalDivider

Simple vertical divider, typically used to divide sections of uniform content, e.g. grid columns.

```cs
public static StardewUI.Graphics.Sprite ThinVerticalDivider { get; }
```

##### Property Value

[Sprite](sprite.md)

-----

#### ThinVerticalDividerUncolored

Simple, colorless vertical divider, typically used to divide sections of uniform content, e.g. grid columns.

```cs
public static StardewUI.Graphics.Sprite ThinVerticalDividerUncolored { get; }
```

##### Property Value

[Sprite](sprite.md)

-----

#### TinyTrashCan

Very small trash can, e.g. to be used in lists/subforms as "remove" button.

```cs
public static StardewUI.Graphics.Sprite TinyTrashCan { get; }
```

##### Property Value

[Sprite](sprite.md)

-----

#### VerticalScrollThumb

Thumb sprite used for vertical scroll bars.

```cs
public static StardewUI.Graphics.Sprite VerticalScrollThumb { get; }
```

##### Property Value

[Sprite](sprite.md)

-----

#### White

A single white pixel.

```cs
public static StardewUI.Graphics.Sprite White { get; }
```

##### Property Value

[Sprite](sprite.md)

-----

