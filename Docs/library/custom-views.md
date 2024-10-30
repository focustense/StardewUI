# Custom Views

If the [standard views](standard-views.md) aren't enough to implement a particular design, StardewUI supports the creation of custom views.

There are several types of custom views, each best suited for a different scenario.

## :material-toy-brick: Reusable Widgets

**Strategy: [`ComponentView` &nbsp; :material-book-open-variant:](../reference/stardewui/widgets/componentview.md)**

A custom view can simply be a collection of [standard views](standard-views.md) (or a combination of standard views and other custom views) that are designed to work together, and in a particular way. Many of the standard views follow this pattern themselves; for example, a [Checkbox](standard-views.md#checkbox) is nothing more than a [Lane](standard-views.md#lane) with an [Image](standard-views.md#image) and [Label](standard-views.md#label), with some properties and event handlers to tie them together.

It is usually possible to do this using [StarML](../framework/starml.md) alone, via the use of [included views](../framework/included-views.md). However, this may start to feel awkward or clumsy if internal UI/layout state finds its way into the data model; or the view may require the use of [overlays](overlays.md) or [animation](animation.md) which are cumbersome or impractical to control via the data binding system. For those cases, the solution tends to be a small amount of code that performs the same basic function as a StarML-based view, but without the requirement of a backing data model and with more flexibility to interact directly with the view properties and sometimes with the outside environment.

These types of widgets are referred to as _[component views](../reference/stardewui/widgets/componentview-1.md)_ and the process for implementing one is simple and straightforward:

1. Create a class inheriting from `ComponentView` or `ComponentView<T>`, where `T` is the type of the "root" view—generally one of the standard [layout views](standard-views.md#layouts).
2. Implement the [`CreateView`](../reference/stardewui/widgets/componentview-1.md#createview) method by creating the entire [view tree](../concepts.md#layout-views-and-view-trees). If some aspects of the tree are dynamic—for example, the aforementioned `Checkbox` may or may not include a text label—then create the "initial" or "default" state of the tree here.
3. If any views within the view tree are meant to be controlled by properties, or otherwise change in response to user input or other view state, then add fields for those views and assign their values in `CreateView`[^1].
4. Add properties, event handlers, and any other logic required to coordinate behavior and state between different views in the tree.
5. For any new properties, ensure that property notifications are sent, i.e. by calling [`OnPropertyChanged`](../reference/stardewui/view.md#onpropertychangedstring) when the value changes. Also forward the [`PropertyChanged` event](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged.propertychanged) and any other important events from the root view and/or descendants, if they are meant to be handled from a StarML template.

??? note "Note on `DecoratorView`"

    For rare scenarios where the entire inner view needs to change, there are also _[decorator views](../reference/stardewui/widgets/decoratorview-1.md)_. These have the same concept, but the `View` has to be set directly, which means it is also nullable; inheriting from `DecoratorView<T>` is therefore more difficult and error prone than `ComponentView<T>` because a decorator's code has to be null-safe everywhere. Component views are inherently null-safe because the view tree is created on construction.

    The only built-in view that uses `DecoratorView` directly is the internal `DynamicDropDownList` used to automagically detect data binding types and recreate the internal dropdown via reflection. Unless you are dealing with a similarly specialized scenario, prioritize `ComponentView` first and only use `DecoratorView` when necessary.

The relative simplicity of the implementation does not mean it can only be used for views with simple behavior; the [Drop-Down List](standard-views.md#drop-down-list) and [Slider](standard-views.md#slider) are fairly complex widgets, but both are based on `ComponentView`.

Rather than provide contrived examples here, the best reference for implementing component views is the StardewUI source itself. All of the following widgets are component views:

- [Buttons](https://github.com/focustense/StardewUI/blob/dev/Core/Widgets/Button.cs)
- [Checkboxes](https://github.com/focustense/StardewUI/blob/dev/Core/Widgets/CheckBox.cs)
- [Drop-down lists](https://github.com/focustense/StardewUI/blob/dev/Core/Widgets/DropDownList.cs)
- [Expanders](https://github.com/focustense/StardewUI/blob/dev/Core/Widgets/Expander.cs)
- [Sliders](https://github.com/focustense/StardewUI/blob/dev/Core/Widgets/Slider.cs)

There is no difference between a `ComponentView`-based widget in StardewUI's core library and one implemented in a mod or addon; standard views do not receive any special treatment.

[^1]: For projects enforcing nullable reference types, initialize the field to `null!` in order to prevent the error/warning; since `ComponentView` invokes `CreateView` from its constructor, it is virtually impossible for them to be `null` when referenced from any user code.

## :material-view-carousel: Custom Layout or Drawing

**Strategy: [`View` &nbsp; :material-book-open-variant:](../reference/stardewui/view.md)**

Views that are not [component views](#reusable-widgets) will most often inherit from the [`View`](../reference/stardewui/view.md) base class, which is designed so that it is relatively easy to create a very simple view, with many methods to situationally override as the scenario becomes more complex.

The main reason to use a `View` subclass instead of `ComponentView` is when it needs direct control over the layout or drawing phases. While the [standard layouts](standard-views.md#layouts) should cover the majority of possible UI designs, it is always possible to run into situations that don't fit the mold, e.g. if an equivalent to WPF's [WrapPanel](https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.wrappanel?view=windowsdesktop-8.0) were required. Similarly, if you wanted to draw using a [custom effect](https://docs.monogame.net/articles/getting_started/content_pipeline/custom_effects.html) (shader) instead of basic text and sprites, or need to control clipping (as in the [Marquee](standard-views.md#marquee)), or are [desaturating and re-tinting some other view](https://github.com/focustense/StardewUI/blob/dev/Core/Widgets/GhostView.cs), then `ComponentView` doesn't offer enough flexibility and you will need to inherit from `View` instead.

As the heading suggests, there are only two _required_ method implementations for `View`:

- [`OnMeasure(Vector2)`](../reference/stardewui/view.md#onmeasurevector2), the view's contribution to the combined [measure and layout pass](../concepts.md#layout);
- [`OnDrawContent(ISpriteBatch)`](../reference/stardewui/view.md#ondrawcontentispritebatch), which is where the actual rendering occurs.

The [Spacer](https://github.com/focustense/StardewUI/blob/dev/Core/Widgets/Spacer.cs) source demonstrates the bare minimum, although the spacer does not actually do anything other than occupy space, so a custom `View` will be more involved.

The main thing that `OnMeasure` **must** always do before returning is set the view's [`ContentSize`](../reference/stardewui/view.md#contentsize), as this and only this is what layout views use to arrange content. If `ContentSize` is not set, the view will be "laid out" with a size of zero, and anything it tries to draw may overlap with or be overwritten by other views.

Almost every `OnMeasure` method should start with the line:

```cs
var limits = Layout.GetLimits(availableSize);
```

This translates the view's [LayoutParameters](../reference/stardewui/iview.md#layout) into an actual pixel size representing the maximum width and height available for use by that view. The `availableSize` argument is what the parent is willing to give; the `limits` derived from `GetLimits` are what the view is willing to take.

Similarly, `ContentSize` should _normally_ be set using some variant of:

```cs
ContentSize = Layout.Resolve(availableSize, () => childrenSize);
```

Where `maxChildSize` is the accumulated size of all child views _after_ layout has finished on them. Using [`Resolve`](../reference/stardewui/layout/layoutparameters.md#resolvevector2-funcvector2) in this context ensures that the [length type](../reference/stardewui/layout/length.md#type) of each dimension and other constraints such as min/max width and height are all respected.

Aside from setting `ContentSize`, `OnMeasure` is also the place to perform any updates in response to the view's size or content changing; for example, performing layout on a [NineSlice](../reference/stardewui/graphics/nineslice.md#layoutrectangle-simplerotation), breaking/wrapping text into lines, etc.

### View Performance

As a `View` implementer, it becomes your responsibility to ensure that the layout and rendering methods are efficient and not janky. To start with, _avoid doing anything complex in the draw method_. Drawing should, in most cases, require no calculations, no `new` objects except for scoped state (e.g. saved transforms and clip states from `ISpriteBatch`), and definitely no _writing_ any mutable state, whether internal or external.

Expensive operations should happen during layout instead. Layout is cached, while drawing is not; `OnMeasure` only runs when something has actually changed that requires a new layout, whereas `OnDrawContent` runs every single frame during which the UI is on screen.

To ensure that layout is, in fact, correctly cached, the majority of views should implement two other methods:

- [`IsContentDirty()`](../reference/stardewui/view.md#iscontentdirty), which queries whether any state has changed and therefore whether the view actually needs layout this frame;
- [`ResetDirty()`](../reference/stardewui/view.md#resetdirty), which runs at the end of every successful layout and ensures that `IsContentDirty()` returns `false` on the next frame, unless something else changes between the reset and the next frame.

Some useful tools are provided to help facilitate dirty checks, mainly [`DirtyTracker<T>`](../reference/stardewui/layout/dirtytracker-1.md) for single properties and [`DirtyTrackingList<T>`](../reference/stardewui/layout/dirtytrackinglist-1.md) for collections, the latter being especially useful for tracking collections of child views. These are ubiquitous throughout StardewUI's own library code and are designed to minimize the amount of boilerplate needed.

A typical implementation requires only a `readonly` field declaration, a property wrapper, and lines in the dirty-check and reset methods.

!!! example

    ```cs
    public class MyView : View
    {
        public string Text
        {
            get => text.Value;
            set
            {
                if (text.SetIfChanged(value))
                {
                    OnPropertyChanged(nameof(Text));
                }
            }
        }

        private readonly DirtyTracker<string> text = new("");

        protected override bool IsContentDirty()
        {
            return text.IsDirty;
        }
        
        protected override void OnDrawContent(ISpriteBatch b) { ... }

        protected override void OnMeasure(Vector2 availableSize) { ... }

        protected override void ResetDirty()
        {
            text.ResetDirty();
        }
    }
    ```

**Always make sure that dirty checks are properly paired with dirty resets**, otherwise the view may never reach a clean state, forcing layout to happen for the entire view tree on every frame.

### View Children

When subclassing `View` for the purposes of layout, as opposed to custom drawing, additional considerations are required for managing child views. The more complex the layout, the more (potentially) involved the process will be.

Roughly, the steps for implementing a layout view are:

1. Decide whether the view can have only one child, like a [Frame](standard-views.md#frame), or multiple children, such as a [Panel](standard-views.md#panel). By convention, single-child views tend to refer to `Content` instead of `Children`, but this makes no difference to the layout system or to the StarML renderer.
2. Add a dirty-tracking field; for a single child, a `DirtyTracker<IView>` and for a list, use `DirtyTrackingList<IView>`.
3. Add an accessor property, dirty-check and dirty-reset as described in [View Performance](#view-performance) above. When implementing dirty checking, be sure to check **both the tracker itself and the tracked view(s).** For example, the logic for a list of children is:  
    `children.IsDirty || children.Any(child => child.IsDirty())`
4. Perform child layout in `OnMeasure`. This depends on the specifics of your layout, so there is no standard set of sub-steps to follow, but the [Grid](https://github.com/focustense/StardewUI/blob/dev/Core/Widgets/Grid.cs) and [Panel](https://github.com/focustense/StardewUI/blob/dev/Core/Widgets/Panel.cs) implementations are good places to start. It is usually a good idea to store the results in a `List<ViewChild>` or other collection of `ViewChild`, as other methods will need to retrieve that specific type.
5. Implement (override) [`GetLocalChildren`](../reference/stardewui/view.md#getlocalchildren) and [`FindFocusableDescendant`](../reference/stardewui/view.md#findfocusabledescendantvector2-direction). The former is required to propagate events and updates, and the latter is required for [focus searches](#focus-search). You can also override [`GetLocalChildrenAt`](../reference/stardewui/view.md#getlocalchildrenatvector2) if the view might have a very large number of children and a more optimal implementation than the default linear search is available, but this is optional.

You'll notice that all of the overridable `View` methods use _local_ coordinates, so they do not need to be concerned about screen positions or their overall place in the hierarchy. Except for certain edge cases such as negative margins, a view's boundaries are always from `(0, 0)` to its `OuterSize` (which is `ContentSize` plus padding, border and margins).

### Focus Search

The most difficult aspect of implementing any custom layout is generally going to be the focus search behavior, i.e. the [`FindFocusableDescendant`](../reference/stardewui/view.md#findfocusabledescendantvector2-direction) method.

Focus search is analogous to HTML [tab index](https://developer.mozilla.org/en-US/docs/Web/HTML/Global_attributes/tabindex), Qt [tab order](https://doc.qt.io/qt-6/focus.html) and so on, with one crucial difference: Stardew Valley does not have true explicit input/accessibility focus. Instead, the "focused" element is whichever focusable control is underneath the mouse/gamepad cursor at that exact moment. Focus search is therefore defined as determining the "best" (generally what users would perceive as _nearest_) focusable view in a given direction.

There are no specific instructions because, like the `OnMeasure` implementation, it is completely dependent on the specific layout; a layout that arranges children in a horizontal line is going to have a very different implementation from one that arranges them in a grid, or in the same overlapping position. However, the following tips should prove helpful in getting to a correct implementation:

- All focus searches are recursive; if a direct child is not [focusable](../reference/stardewui/iview.md#isfocusable), one of its own children/descendants may be. Therefore, when "considering" a child as a candidate for focus search, layout views must recursively run the focus search on that child.
- As a corollary to the above, finding an adjacent child that fails focus search (returns `null` from [`FocusSearch`](../reference/stardewui/iview.md#focussearchvector2-direction)) does _not_ mean that the search should stop; it means that the next child in the same direction must be queried, and so on until a match is found or there are no more views left.
- Focus search is not constrained to the container; it is a normal and expected part of the flow for focus to move from the last child of one layout view to the first child of another layout view. Do not expect the `contentPosition` to always be within the view's boundary, and do not skip focus searches when this occurs. Instead, try to return whichever focusable view the cursor might land on (or _near_) if it moved continuously from its current position—which may be negative or out of bounds—in the specified `direction`.
- Use the [`ViewChild.IsInDirection`](../reference/stardewui/viewchild.md#isindirectionvector2-direction) helper to help with matching instead of writing `switch` statements/expressions.
- If the `contentPosition` is completely out of bounds on _both axes_, or if movement in the specified `direction` will never intersect with any point within the layout boundaries, then return `null`.

D-pad navigation in a non-uniform layout can often be ambiguous and there is not necessarily a single "right answer" for any given focus search. Instead, try to ensure that every focusable control on screen is reachable somehow, even if the path to get there seems slightly unintuitive.

### General Interactivity

The `View` base class already provides everything required to emit events such as `Click`, `PointerEnter` and `PointerLeave` with no additional code. However, it may be the case that the custom view is expected to have its own consistent behavior, independent of whatever event handlers are attached. For example, a [Text Input](standard-views.md#text-input) must handle clicks in order to move the caret; it should not rely on the caller setting up an event handler to do this.

These situations are actually rare. `TextInput` handles its own click events and [Scrollable Views](standard-views.md#scrollable-view) handle mouse wheel events; aside from these two instances, there are no built-in views that override the default event emitters, but it can be done when needed.

The overridable event-related methods are:

- [`OnButtonPress`](../reference/stardewui/view.md#onbuttonpressbuttoneventargs)
- [`OnClick`](../reference/stardewui/view.md#onclickclickeventargs)
- [`OnDrag`](../reference/stardewui/view.md#ondragpointereventargs)
- [`OnDrop`](../reference/stardewui/view.md#ondroppointereventargs)
- [`OnPointerMove`](../reference/stardewui/view.md#onpointermovepointermoveeventargs)
- [`OnWheel`](../reference/stardewui/view.md#onwheelwheeleventargs)

Other public events, such as `LeftClick` and `DragEnd`, are derived from the implementations of the above methods.

!!! warning

    When overriding the above event-raising methods, remember to invoke the base method, e.g. `base.OnClick(e)`, unless you actually intend to prevent regular event handlers from detecting the same event. Suppressing overrides may also want to set the [`Handled`](../reference/stardewui/events/bubbleeventargs.md#handled) property in order to suppress the event not only from their own children, but also their ancestors and siblings.

## :material-space-invaders: Starting from Scratch

**Strategy: [`IView` &nbsp; :material-book-open-variant:](../reference/stardewui/iview.md)**

!!! danger

    **Advanced users only.** You are now heading into the untamed wilderness without a map. There are no guards, no guardrails, nor even many useful signposts. You should only be attempting this if you have already attempted all the other methods and found it impossible to achieve what you want.

Implementing [IView](../reference/stardewui/iview.md) directly is essentially opting out of the entire built-in layout, drawing and event _system_ and deciding to make your own. StardewUI allows this—all view trees are based on the `IView` interface and not the `View` base class—however, it requires significantly more code to achieve, and significantly more care and testing to get right.

Most `IView` implementations that are not based on `View` are really just "pass-through" or "wrapper" views that take some inner `IView` and forward most of the properties and events; the most prominent of these is [ComponentView](../reference/stardewui/widgets/componentview-1.md), which many widgets and the Framework's internal `DocumentView` are based on. Even then, the implementation is fairly complex; refer to the [DecoratorView](https://github.com/focustense/StardewUI/blob/dev/Core/Widgets/DecoratorView.cs) source as an example of what is involved.

From-scratch implementations have to take on all the responsibilities of a `View` and also:

- Translating coordinates from screen-space or parent-space to local space;
- Implementing their own dirty-checking cycle, since there is no automatically-called `Reset` method;
- Handling _and dispatching_ game update ticks, as well as all the events described in [general interactivity](#general-interactivity);
- Providing their own margins, padding, or any other non-content dimensions that can affect the difference between the different bounds and size properties (e.g. [ActualBounds](../reference/stardewui/iview.md#actualbounds) vs. [ContentBounds](../reference/stardewui/iview.md#contentbounds)), and defining their own `OuterSize` implementation;
- Implementing accessors for all the basic view attributes: focusability, visibility, tags, tooltips, and so on.

A full guide to implementing `IView` would be out of scope for this page. Consider it the _"I Know What I'm Doing Mode"_ of StardewUI and only reach for this option if you are certain that you know what you're doing.