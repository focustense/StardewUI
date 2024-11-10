# Binding Events

All views in StardewUI emit [events](starml.md#events), especially pointer events such as clicks, hovers, drags, etc. These tend to the form the basis of user interaction in most UI.

In the [core library](../library/index.md), events are literally [.NET events](https://learn.microsoft.com/en-us/dotnet/standard/Events/). When using the [framework](index.md) to write [StarML](starml.md), however, there are many problems with consuming events directly, mostly owing to the design of SMAPI's mod integration:

- Neither the specific view type nor even an `IView` instance is available on which to add an event handler;
- Even if the view were available, the event handler type, like `PointerEventArgs`, can't be sent over SMAPI's integration framework ([Pintail](https://github.com/Nanoray-pl/Pintail));
- Even if the arguments could be sent, actual `View` instances can often be destroyed, recreated, etc., e.g. when using `*repeat`, `*if`, or other [structural attributes](starml.md#structural-attributes).

Conventional event listeners are effectively a dead-end, but there is a solution. [StarML](starml.md) supports event bindings as a _micro-grammar_ or _micro-syntax_, i.e. a "syntax within another syntax".

## Event Attributes

The quickest way to explain the micro-syntax used for event bindings is to jump right in and dissect a live one:

=== "StarML"

    ```html
    <button click=|~ShopViewModel.Buy("Cheese", ^Quantity, $Button)| />
    ```

=== "C#"

    ```cs
    public class ShopViewModel
    {
        public void Buy(string name, int quantity, SButton button)
        {
            // Implementation intentionally omitted.
        }
    }
    ```

The individual parts of this are as follows:

/// html | div.no-code-break

| Syntax | Explanation |
| ---- | ---- |
| `<button ... />` | The element/view that the event is being attached to, in this case a [Button](../reference/stardewui/widgets/button.md). |
| `click=|...|` | The name of the view's [event](https://learn.microsoft.com/en-us/dotnet/standard/Events/) property or field, in [kebab-case](https://developer.mozilla.org/en-US/docs/Glossary/Kebab_case); in this example, it is the [`Click`](../reference/stardewui/iview.md#click) event available on every view.<p>To be recognized as an event binding, the value **must be enclosed in pipe characters**, i.e. `event=|...|` but not `event="..."` or `event={...}`. |
| `~ShopViewModel.Buy` | The name of the event handler (method) to run when the event is raised. [Context redirects](binding-context.md#redirects) are supported here—although not required. In this example, we want to run the method named `Buy` on the class `ShopViewModel`. |
| `(...)` | The arguments to provide to the above method. Note that even if the method takes no arguments, it still must be given an empty argument list `()`, as would be the case for any C# method call. |
| `"Cheese"` | A _literal_ argument value. The exact value is read as a literal string, and [converted](starml.md#type-conversions) as necessary to the required argument value; e.g. if the method takes an `int` then we can write `"42"`. |
| `^Quantity` | A _context property_ value. This has the exact same behavior as a [property data binding](starml.md#attribute-flavors), but **without** the enclosing braces. [Redirects](binding-context.md#redirects) are supported with individual arguments of this kind, and are unrelated to the event handler's target. |
| `$Button` | An _event property_ value. The `$` token is **only** valid for event arguments, and refers to the same-named property on the real `EventArgs` object. Specifically, we want the [`Button`](../reference/stardewui/events/clickeventargs.md#button) property of `ClickEventArgs`—for example, we might use this to implement different behavior for left-click vs. right-click. |

///

This example covers all possible argument types: literals, context properties, and event properties. Other types of bindings, such as assets (`@path`) are **not** supported in event arguments.

## Return Values

Event handler methods, such as the hypothetical `Buy` method above, will generally be `void`, but there is one special case: a handler may return `bool` in order to control the [`Handled`](../reference/stardewui/events/bubbleeventargs.md#handled) property of the event and prevent it from bubbling up.

"Event bubbling" is the process by which an event not considered "handled" by a child view is given another chance to run at the parent. Consider the following two scenarios:

1. A list or table in which the currently-hovered item or row is highlighted, and within that row is a button with its own special highlighting or hover animation.
2. A large frame that can be clicked to perform some action (e.g. buy an item), with a smaller inner button that performs a different action (e.g. displaying additional info).

In case (1), if the user hovers on the button, then we want both actions to run – the list item _and_ the button should both be highlighted. However, in case (2), we do not want the "outer" action (buy) to run when the user clicks on the "inner" (info) button.

These two cases might very well be part of the _same_ user interface, so let's invent one:

!!! failure "Partially Broken"

    === "C#"
    
        ```cs
        public class ListItemViewModel : INotifyPropertyChanged
        {
            public Color BackgroundTint { get; set; } = Color.White;
            public Color InfoButtonTint { get; set; } = Color.White;
            public string ExtraInfo { get; set; } = "Extra item info";
            public string Text { get; set; } = "Item Text";
            
            public void Buy()
            {
                Game1.addHUDMessage(new($"You bought {Text}"));
            }
            
            public void DisplayExtraInfo()
            {
                Game1.addHUDMessage(new(ExtraInfo));
            }
    
            public void SetBackgroundHover(bool hover)
            {
                BackgroundTint = hover ? Color.Yellow : Color.White;
            }
    
            public void SetInfoButtonHover(bool hover)
            {
                InfoButtonTint = hover ? Color.Blue : Color.White;
            }
        }
        ```
    
    === "StarML"
    
        ```html
        <frame background={@Mods/StardewUI/Sprites/ControlBorder}
               background-tint={BackgroundTint}
               click=|Buy()|
               pointer-enter=|SetBackgroundHover("true")|
               pointer-leave=|SetBackgroundHover("false")|>
            <lane layout="400px 80px" vertical-content-alignment="middle">
                <label layout="stretch content" text={Text} />
                <image layout="32px"
                       sprite={@Mods/MyMod/Sprites/Info}
                       tint={InfoButtonTint}
                       click=|DisplayExtraInfo()|
                       pointer-enter=|SetInfoButtonHover("true")|
                       pointer-leave=|SetInfoButtonHover("false")| />
            </lane>
        </frame>
        ```

In this example, hovers should work as described above, but clicks will not. If you click on the info button (image), you will see **both** HUD messages, because the clicked position is inside the `<image>` _and also_ inside the `<frame>`.

Resolving this is very simple; all we need to do is instruct StardewUI not to bubble the click event from the info button.

!!! success

    ```cs
    public class ListItemViewModel : INotifyPropertyChanged
    {
        // Unchanged properties/methods omitted
    
        public bool DisplayExtraInfo()
        {
            Game1.addHUDMessage(new(ExtraInfo));
            return true;
        }
    }
    ```

StardewUI automatically recognizes that `DisplayExtraInfo` now returns a `bool`, and since we return `true`, it sets the event's `Handled` property to `true`, preventing `Buy()` from being triggered at the same time.

This leaves us with the behavior we wanted originally: clicking the info button (`<image>`) _only_ shows info, while clicking anywhere else within the list item (`<frame>`) runs the "buy" logic.