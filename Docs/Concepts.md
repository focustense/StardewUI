# Concepts

An extended introduction to StardewUI's major ideas and building blocks; this is a good place to start if you're coming from other UI frameworks (Qt, WPF/Avalonia, etc.) and wondering about the similarities and differences, or if you've never used a UI framework before and wondering why you can't—or shouldn't—simply do everything directly with [`SpriteBatch`](https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.SpriteBatch.html) and `IClickableMenu` as is typical in Stardew.

## Views

:material-grid: :material-view-column: :material-button-cursor: :octicons-checkbox-16: :octicons-sliders-16: :octicons-image-16: :fontawesome-solid-scroll: :fontawesome-solid-up-down: :material-text-box-outline: :octicons-sidebar-collapse-16:

These go by many different names in many different frameworks. In [Qt](https://doc.qt.io/qt-6/qwidget.html) and [Flutter](https://docs.flutter.dev/ui), they're _widgets_. In [Windows Presentation Foundation](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/controls/?view=netframeworkdesktop-4.8), [Windows Forms](https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.control?view=windowsdesktop-8.0) and [Swift](https://developer.apple.com/tutorials/swiftui/working-with-ui-controls), they're _controls_. In [Android](https://developer.android.com/reference/android/view/View) and [iOS](https://developer.apple.com/documentation/uikit/uiview), they are _views_. On the web, they are [elements](https://developer.mozilla.org/en-US/docs/Web/API/Element) in pure HTML, and [Angular](https://angular.dev/guide/components) or [React](https://react.dev/learn/your-first-component) might refer to them as _components_.

Regardless of what we call them, and the many subtle differences between their behavior across frameworks and platforms, they always refer to the same basic concept: a self-contained, reusable object that controls its own appearance on screen and, sometimes, the user interactions it supports, such as clicks or hovers.

StardewUI calls them _Views_, not because of any affinity with mobile frameworks, but because of the "View" in Model-View-Whatever ([MVP](https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93presenter), [MVC](https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93controller), [MVVM](https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93viewmodel) and so on). It may on occasion be referred to as a "widget" in other parts of documentation for clarity, or when discussing a view in terms of its _behavior_ rather than what it technically is.

### Everything is a view

Nothing useful happens in StardewUI without a View. Underneath it all is still MonoGame's [`SpriteBatch`](https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.SpriteBatch.html)—we do need a destination for those pixels—but Views are what enable [layout](#layout) and reuse.

Some views might seem very simple, even too trivial to be a view at all. Is a [Label](library/standard-views.md#label) really anything more than a wrapper around [`SpriteBatch.DrawString`](https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.SpriteBatch.html#Microsoft_Xna_Framework_Graphics_SpriteBatch_DrawString_Microsoft_Xna_Framework_Graphics_SpriteFont_System_String_Microsoft_Xna_Framework_Vector2_Microsoft_Xna_Framework_Color_System_Single_Microsoft_Xna_Framework_Vector2_Microsoft_Xna_Framework_Vector2_Microsoft_Xna_Framework_Graphics_SpriteEffects_System_Single_), perhaps with a `Game1.parseText` thrown in?

In a word, yes:

* What if we want to truncate/add an ellipsis after _n_ lines?
* What if we want multiple lines to be horizontally centered, or even right-aligned?
* What if we want to simulate bold, or draw outlined or shadowed text?

Text seems easy at first, but rarely stays that way. It's the same with images, which are all, in a sense, just `SpriteBatch.Draw` underneath; but is that all they are? What if the image isn't perfectly-sized for the area it needs to fit in - or if a single region needs to accommodate images of multiple sizes? Do we scale or stretch? Clip or crop? Can it be [animated](library/animation.md)?

The `SpriteBatch` is just a blank canvas on which to draw; views provide the reusability, interactivity, reactivity, and everything else we've come to expect from a user interface.

### Layout views and view trees

So far, we've only touched on basic view types like labels and images, but the real power of StardewUI is in its [layout](#layout) system which is based on layout views.

A layout view isn't a specific type, or supertype; it describes any view that is responsible for laying out (positioning and sizing) at least one other view, with those other views being considered the _children_ of the layout view. Together they form a tree:

<div class="grid tree-table" markdown>

!!! example

    ![Settings Form](images/screenshot-form.png)

::spantable:: class="table-stretch"

| Lane: Menu @span=5 | | | | |
| | Banner: Settings @span=4 | | | |
| | Frame: Border @span=4 | | | |
| | | Label: Speed @span=3 | | |
| | | Lane: Row 1 @span=3 | |
| | | | Label: Enable turbo boost @span=2 |
| | | | Checkbox @span=2 |
| | | Lane: Row 2 @span=3 | |
| | | | Label: Speed multiplier @span=2 |
| | | | Slider @span=2 |

::end-spantable::

</div>

The structure is the same whether you're using the [Core Library](library/index.md) or [StarML](framework/starml.md). At the end of the day, everything resolves to a pixel position, but you almost never deal with pixel positions directly; instead, you work with views (or elements, in StarML), and the layout views—in the above example, the [Frame](library/standard-views.md#frame) and [Lanes](library/standard-views.md#lane)—decide how to lay out their children—in this case, the various [Labels](library/standard-views.md#label), [Checkbox](library/standard-views.md#checkbox) and [Slider](library/standard-views.md#slider).

## Layout

Many of the pages on this site refer to layout, but what actually **is** layout?

[Microsoft's definition](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/advanced/layout?view=netframeworkdesktop-4.8) is useful here:

> At its simplest, layout is a recursive system that leads to an element being sized, positioned, and drawn. More specifically, layout describes the process of measuring and arranging the members of a [[Layout View]](#layout-views-and-view-trees) element's `Children` collection.

The original definition refers to a "Panel"; the equivalent concept in StardewUI is a "layout view".

Much like Microsoft's WPF – and Android, and Apple's UIKit, and the majority of UI frameworks – StardewUI layout is completed in two recursive passes:

1. The **Measure Pass**, in which children (which may themselves be layout views) are assigned sizes based on (a) the amount of available space, or _limits_, and (b) the amount of space they want, which could be based on the limits, or be a fixed size, or be a function of the content, or multiple or none of the above. The important thing is that each view is told how much space, horizontal and vertical, that it is _allowed_ to use, and then reports back how much it _will_ use.
2. The **Layout Pass**, in which all the size measurements are combined in order to determine the pixel positions (relative to the parent) of each view. Sometimes this might just mean stacking left-to-right or top-to-bottom, e.g. in a `Lane` but for other layout types it could involve alignments, wrapping, clipping, offsetting, or doing nothing at all (e.g. drawing its children/content in place at position `(0, 0)`).

These passes are somewhat implicit in StardewUI—there are not separate methods named "measure" and "layout". However, it is how every layout view works inside its `Measure` method.

More importantly, layout is inherently _recursive_. In order to measure its own width, a layout view must measure all of its children first, which requires measuring grandchildren, and so on. If we were to do this on every update tick, it would perform poorly, or at least no better than the equivalent [immediate mode](https://en.wikipedia.org/wiki/Immediate_mode_(computer_graphics)) UI. To avoid this, StardewUI makes use of a common technique called [dirty checking](#dirty-checking).

Separate measure and layout passes allow us to do useful things that single-pass layouts tend to have trouble with—for example, aligning content to both the left and right sides of a container, where the main content stretches to fill the remaining width:

::spantable:: class="table-stretch"

| Navigation | Main Content @class="width-lg" | Info |
| Page 1 | Contents of the current page; an item grid or list, NPC profile, options form, etc. @span | Additional sidebar content @span |
| Page 2 | | |
| Page 3 | | |

::end-spantable::

This layout is only possible with a measure pass, as we need to know the width of both the Navigation and Info columns (lanes) before it is possible to lay out the Main Content lane. In fact, this requires two measure passes (called _initial_ and _deferred_) although that is an implementation detail of the layout view and not all layouts require a deferred pass.

### Dirty Checking

Rerunning layout on every tick would be expensive, so in order to avoid doing this, StardewUI—like many frameworks—employs a form of dirty-checking. This is sometimes referred to as "invalidation", and while there are some subtle differences between invalidation and dirty-checking, they are close enough to be considered the same for the purposes of this section.

Invalidation/dirty checking is the process of keeping track of _what state was modified_ (since the last completed layout) in addition to a view's current state. "State" is complex and view-specific but can generally be described as the combination of:

1. Details of the layout request, e.g. the previous [limits](#layout) compared to new limits
2. Values of any layout-affecting properties, such as the `LayoutParameters` (obviously) but also padding, margins, current text of a label[^1], current value of a slider, etc.
3. The dirty state of any and all child views.

Layout always proceeds from the top down, but dirty-checking cascades from the bottom up. This might seem like a bad thing—yes, a change at _any_ level of the view hierarchy means that the _entire_ layout must be done again—but in fact it is what allows large parts of the layout, and often the entire layout, to be skipped in each pass, reducing the "typical" workload to almost nothing. This is the basic principle behind a retained mode (as opposed to immediate-mode) UI.

Consider our [earlier example](#layout-views-and-view-trees) and suppose the slider value was changed. Because the slider is dirty, and that dirtiness cascades upward, the entire Menu Lane is effectively dirty. **However:**

1. When the `Banner: Settings` view is measured, it sees that the limits have not changed, and since the banner itself is not dirty, it does not need to perform layout again; it can reuse the same layout as last frame.
2. The `Frame: Border` is part of the dirty cascade, so it has to perform layout again. However, `Label: Speed` and `Lane: Row 1` end up in the same state as the banner; they are not dirty, and their limits are the same as before, so they skip layout.
3. We then get to `Lane: Row 2`, which is dirty, and `Slider`, which of course requires new layout, although `Label: Speed multiplier` can also be skipped for the same reason as previous views.

Thus in the end, we have only actually performed layout on 4 out of the 10 total views; the Menu Lane, Border Frame, Row 2 Lane and Slider. The larger and more complex a view tree gets, the more is saved by this branch-elimination; in a tree of 100 views, a single dirty property may involve fewer than 10 views in the layout update.

This same logic can also apply if the change happens at the top level. For example, consider if `Lane: Menu` had its width increased (perhaps because we change the title of the "Settings" banner to something much longer), but `Frame: Border` is configured with a fixed width or maximum width. When layout is triggered, the entire `Frame: Border` can be skipped because its _limit_ width hasn't changed and therefore the change in available width cannot affect it.

If you ever run into a poorly-performing UI, use this knowledge to help. If you are frequently changing some content and that is causing slowdowns due to frequent layouts, you may be able to mitigate most of the impact by using a fixed-width container somewhere in between.

[^1]:  Whether or not a particular property truly alters the layout is not always certain; for example, a label that is constrained to 1 line may have no cascading effect if its parent is a horizontally-stretched or fixed vertical lane. However, the relationships in an arbitrary hierarchy can be surprisingly complex—for example, a vertical lane that is horizontally content-sized _might_ be affected by a change to the text of an inner label, but only if it is the longest label or becomes the longest label, etc.

    Because the effects can be so unpredictable, attempting to check whether layout _will_ be affected can be almost as expensive and far more complex and bug-prone than simply redoing the layout; therefore, StardewUI generally just assumes that any dirty property means a new layout is required.

## Data Binding

A term from the heyday of visual database tools like Microsoft Access, the phrase _Data Binding_ in modern usage refers to the ability of a UI framework to keep the state of the UI in sync with some data that is _not_ part of the UI.

Most real-world UI is not static. For example, you have a [label](library/standard-views.md#label) that is supposed to display the name of some item. But which item?

```html
<label text="Parsnip" />
```

isn't very helpful unless you know the item is always going to be a Parsnip. When you build your own UI from scratch using `SpriteBatch`, you tell it exactly what text to display on every `draw` call. However, when you write [StarML](framework/starml.md) and use the [Framework API](framework/index.md) to display it, you don't have access to the actual `Label` instance, nor any ability to set its `Text`. How do we make this display the name of _any_ item, not just static text?

The answer is a data binding:

```html
<label text={ItemName} />
```

Along with a model such as:

```cs
public class ViewData
{
    public string ItemName { get; set; } = "";
}
```

The full details are covered in the [StarML guide](framework/starml.md#attribute-flavors); at a conceptual level, what matters is that StardewUI does the synchronization work for you. You don't control the `Label` directly, but you don't need to, because you have _bound_ its text to a model (`ViewData`) that you _do_ control. `ViewData` is part of your mod.

This style of UI development favors the "Model-View" family – informally, [Model-View-Whatever](https://www.beyondjava.net/model-view-whatever).

**But, there's a catch...** if you want these data bindings to be more than a one-shot deal—that is, if you want to be able to _change_ the `ItemName` and see the changes show up immediately in the UI—then you need to implement [INotifyPropertyChanged](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged?view=net-8.0) (aka: "INPC"). Doing so can be very tedious, so head on over to [Binding Context](framework/binding-context.md) for helpful tips and shortcuts to make it much faster and easier.
