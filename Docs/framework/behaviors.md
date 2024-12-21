# Behaviors

Occasionally referred to as _extension attributes_ for their similarity to [extension methods](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods), StardewUI behaviors are standalone, stateful components that can attach to an existing view and provide continuous or event-based functionality. This allows for a wide range of extensibility without having to creating a [custom view](../library/custom-views.md), or directly modify a built-in view.

Behaviors are, in a sense, the opposite of [structural attributes](starml.md#structural-attributes). While the latter operate on the view hierarchy (i.e. the "document structure") and exist to rearrange the various nodes and bindings, behaviors are completely shielded from that part of the framework and can only see and mutate the specific view they are attached to _after_ the structure is settled and all data bindings have been processed.

This makes them ideal for "just in time" functionality such as animated transitions, and with the aid of a specialized helper called [view state](../reference/stardewui/framework/behaviors/iviewstate.md), able to do certain things fairly easily that would otherwise be very difficult even when using the [core library](../library/index.md) directly.

## Anatomy

Most behaviors inherit from [ViewBehavior](../reference/stardewui/framework/behaviors/viewbehavior-2.md)—with the corresponding `IViewBehavior` interface being intended as a low-level framework type. Behaviors implemented in this way have three basic properties:

- The [View](../reference/stardewui/iview.md) to which the behavior is actually being attached;
- The [View State](#view-state) for applying transient, behavior-specific overrides (see below); and
- The [Data](#data), which is received from the attribute value in the [StarML](starml.md#attribute-flavors) template.

`ViewBehavior<TView, TData>` is generic so that behaviors can be written for specific view types and/or properties. For example, it is possible to write a behavior that only operates on [labels](../library/standard-views.md#label) in order to modify their [`Text`](../reference/stardewui/widgets/label.md#text). Instead of having to perform explicit casting (and safety checks) within the behavior, the framework ensures type safety automatically, preventing behaviors from being attached to incompatible views. Similarly, behavior data gets the same automatic conversion and data-binding treatment as any other attribute value.

## Lifecycle

Behavior events happen in a specific and predictable order which is important for event-based behaviors:

1. [`OnInitialize`](../reference/stardewui/framework/behaviors/viewbehavior-2.md#oninitialize) runs after the behavior has received its View and Data. This is the best time to add event handlers.
2. [`OnNewData`](../reference/stardewui/framework/behaviors/viewbehavior-2.md#onnewdatatdata) runs whenever the `Data` changes. On frames where this occurs, it will always occur before the `Update`. The most common reason to implement `OnNewData` is to replace any item in the [view state](#view-state) associated with this behavior.
3. [`Update`](../reference/stardewui/framework/behaviors/viewbehavior-2.md#updatetimespan) runs on every game update tick after initialization, and as long as the Data is valid (not null, and convertible to the required type). If the behavior has some ongoing function—such as animation—this is where it should run.
4. [`OnDispose`](../reference/stardewui/framework/behaviors/viewbehavior-2.md#ondispose) runs when the behavior is no longer valid, i.e. when the View has been removed or the entire menu/HUD closed. This is the best time to remove any event handlers.

!!! warning

    Behaviors must not try to access any state in the constructor; the `Data`, `View` and `ViewState` properties are not guaranted to have values until `OnInitialize`.

    Accessing `Data` outside of the `Update` method is allowed, but not recommended; the value _may_ be `null` if `TData` is a reference type, even if non-nullable reference types are enabled and the type system implies it cannot be `null`.

## Arguments

Behaviors often take at least one constructor argument, which is derived from the [factory argument](../reference/stardewui/framework/behaviors/behaviorfactory.md#registerstring-funcstring-iviewbehavior). Parameterized behaviors are easier to write, and also easier to use.

As an example, consider a complex hover state, in which we want to change **all** of the following on hover:

- Scale up and reposition the content slightly
- Tint the content or background a different color
- Raise the opacity, i.e. from semitransparent to fully opaque.

Doing this all in a single attribute is extremely awkward and hard to parse, e.g.

`<image +hover="transform:scale:1.2,translate:0, -2; background-tint:#ccf; opacity:1" />`

In fact this is nearly impossible to parse correctly due to the overloaded `:` character, so we might have to change it to an `=`, but that makes it harder to read since the attribute assignment itself uses `=`. There simply isn't a good textual representation for this much data, barring an entire JSON serialization or other hack.

Moreover, it would be very hard to implement this in a single behavior object because all the property types are different. `transform` takes a [Transform](../reference/stardewui/graphics/transform.md), `background-tint` takes a [Color](https://docs.monogame.net/api/Microsoft.Xna.Framework.Color.html), and `opacity` takes a `float`. This can't be represented in a single generic type without also having to use reflection.

Enter arguments, which allow us to deal with a _single_ property at a time instead, cleaning up both the syntax and implementation:

```html
<image +hover:transform="scale: 1.2, translate: 0, -2"
       +hover:background-tint="#ccf"
       +hover:opacity="1" />
```

Now the string values have the exact same format as the [original attributes](starml.md#common-attributes), and the behavior can be implemented using the type-safe method, requiring only a single generic parameter because it only deals with a single value.

Argument parsing does potentially add some complexity to the behavior factory, especially if a variable type is involved; see for example the [StateBehaviorFactory](https://github.com/focustense/StardewUI/blob/dev/Framework/Behaviors/StateBehaviorFactory.cs) implementation. However, the small increase in factory complexity is usually worth the tradeoff for a much more significant decrease in behavior complexity, since behaviors are the part that the user actually sees, and factories run far less often and in a more constrained, predictable context.

## View State

Since behaviors are designed to look and feel like [ordinary attributes](starml.md#common-attributes), and are only able to act on existing views instead of replacing them, it logically follows that many if not most of them will perform the same function as an ordinary attribute (that is, writing a new value to one of the view's properties) but with more logic around their conditions or timing.

A problem arises when multiple behaviors overlap. Consider a hypothetical case where:

1. A view initially starts with a scale (transform) of 0.
2. Behavior A ("show") changes the scale to 1 as soon as the view becomes visible.
3. Behavior B ("hover") increases the scale to 1.2 while the pointer is inside the view.
4. Behavior C ("press") decreases the scale to 1.1 while the mouse button is clicked.

Note that the "press" state isn't really used in Stardew, since most "clicks" take effect immediately on click, rather than release. Nevertheless, most desktop and web apps do have a pressed state, so it makes for a useful example.

A naive approach is to think of each of these behaviors simply as "mutations"—implemented exactly as they are described above, by simply changing [`IView.Transform`](../reference/stardewui/iview.md#transform) to the target value when their event is raised.

This becomes almost immediately unworkable because there is no way to return to the default visible scale of 1.0 after a hover/press. We could add another 3 behaviors ("hide", "leave", "release") each with their own parameter, leading to significant duplication and bloat. Or, we could change behaviors A, B and C so that they handle both "entering" **and "exiting"** their respective state.

A naive solution to this _new_ problem would be to simply save the previous value before performing the mutation, and reverting it when exiting the state. Starting after the "show" state:

!!! success

    1. Behavior B saves the previous scale of 1.0, and sets a new scale of 1.2, when the pointer enters the view.
    2. Behavior C saves the previous scale of 1.2, and sets a new scale of 1.1, when the button is pressed.
    3. Behavior C reverts to the value of 1.1 when the button is released.
    4. Behavior B reverts to the value of 1.0 when the pointer leaves the view.

This is coding for the "happy path", when the user doesn't do anything "strange" or unexpected. However, when you start to reach 1k, 10k, 100k or more users, you must expect the unexpected. In this case, the unexpected is B and C exiting their states in reverse order. The mouse button being held and the pointer staying within a rectangular boundary are independent states. Users typically press and release without moving the pointer, but some may "drag" the pointer out, either intentionally or unintentionally due to lower mobility.

The sad path starts out the same way but ends in the wrong state:

!!! failure

    1. Behavior B saves the previous scale of 1.0, and sets a new scale of 1.2, when the pointer enters the view.
    2. Behavior C saves the previous scale of 1.2, and sets a new scale of 1.1, when the button is pressed.
    3. Behavior B reverts to the value of 1.0 when the pointer leaves the view.
    4. Behavior C reverts to the value of 1.1 when the button is released.

The view is now stuck in the "hover" state even though it is no longer hovered. Users will—correctly—perceive this as a bug.

While this outcome may seem benign—you can "fix" it by simply hovering over the view again—it is actually the simplest statement of the problem, which becomes exponentially more complicated if any transitional states are involved. If even one of these transitions is animated, then the "saved" value can literally end up being any value at all between 0 and 1.2, and the default visible value of 1.0 can become permanently lost.

To deal with this, StardewUI introduces an abstraction called the [View State](../reference/stardewui/framework/behaviors/iviewstate.md), which provides two essential functions:

- An API to [retrieve the default value](../reference/stardewui/framework/behaviors/iviewstate.md#getdefaultvaluetstring) for a given property, which takes into account any attributes/bindings used in the view;
- A [priority queue](https://en.wikipedia.org/wiki/Priority_queue)-like structure for each property that contains all active states and their desired overrides, where the state on top is the one that takes effect in any given frame.

Returning to the aforementioned example, we can now make this work regardless of the path the user takes:

!!! success

    1. Behavior B pushes a "hover" state with scale of 1.2 when the pointer enters the view; effective scale is 1.2.
    2. Behavior C pushes a "press" state with a scale of 1.1 when the button is pressed; effective scale is 1.1.
    3. Behavior B removes the "hover" state; now the "press" state is on top, so effective scale is still 1.1.
    4. Behavior C removes the "press" state, and now no state is on top, so the view reverts to the default value of 1.0.

More importantly, this works perfectly for transitional states because those behaviors do not read the transitional values from the view, they read the intended values from the property's state list, and therefore can never transition to a nonsense value due to a previous transition being prematurely stopped.

!!! info "Takeaway"

    If you are writing a behavior that will modify one or more view properties, **use the view state** to ensure the behavior neither breaks nor is broken by overlapping behaviors. Avoid writing directly to the view whenever possible, and only read from the view directly if you actually depend on the _immediate_ value and not the _expected_ value (e.g. after all transitions finish).

## Data

A behavior's [Data](../reference/stardewui/framework/behaviors/viewbehavior-2.md#data) is the same as the value of a regular [attribute](starml.md#common-attributes). In the markup, it can be specified as either a literal string or some type of [data binding](../concepts.md#data-binding), and the framework automatically handles all type checking and conversion.

Data is the only part of a behavior that can change after the behavior is initialized, i.e. due to a data-bound value changing on the underlying model.

!!! tip

    Behaviors that write all or part of their data to the [view state](#view-state) should override [`OnNewData`](../reference/stardewui/framework/behaviors/viewbehavior-2.md#onnewdatatdata) to replace any existing state with the new data.

All behaviors must specify a data type, because all StarML attributes require a value assignment. If a particular behavior does not care about the attribute value, it can use a data type of `string` and the attribute value can be left empty.

Data should not be confused with [arguments](#arguments). Arguments are constant strings embedded in the attribute name, while Data is the attribute value. In the example:

```html
<panel hover:opacity="0.9" />
```

The **argument** is the property name (`"opacity"`) and the **value** is the floating-point value `0.9`.