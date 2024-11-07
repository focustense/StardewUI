# Stardew Markup Language (StarML)

StarML is the markup language used by the [UI Framework](index.md). It is an HTML-like syntax based on a tree of [elements](#elements), each corresponding to a [view](../concepts.md#views).

It shares many traits with [Angular templates](https://angular.dev/guide/templates), particularly an enhanced set of [attributes](#attributes) that perform [data binding](../concepts.md#data-binding) and other special functions.

!!! example

    ```html
    <frame layout="850px 500px"
           background={@Mods/StardewUI/Sprites/MenuBackground}
           border={@Mods/StardewUI/Sprites/MenuBorder}
           border-thickness="36, 36, 40, 36">
        <lane layout="stretch content" orientation="vertical">
            <label font="dialogue" text="Hello from StardewUI!" />
            <label text={IntroParagraph} />
            <textinput layout="200px content" text={<>FarmerName} />
            <image layout="400px 100px" sprite={@Mods/TestMod/Sprites/Hello} />
            <button text="Launch" click=|LaunchCoolFeature("now")| />
        </lane>
    </frame>
    ```

## Elements

As with most markup languages, StarML is built around elements. An element is:

- A single [tag](#tags)
- with any number of distinct [attributes](#attributes)
- and zero or more [children](#children).

Every StarML element corresponds to a [View](../concepts.md#views), whose type is decided by its tag; every attribute corresponds to a property or event on that view (except [structural attributes](#structural-attributes)), and child elements correspond to the view's child views.

Like HTML and XML, elements can either have an explicit closing tag, or be self-closing:

<div class="grid" markdown>

!!! example "Opening/Closing Tags"

    ```html
    <frame layout="50px 50px">
        ...
    </frame>
    ```

!!! example "Self-Closing Tag"

    ```html
    <label text="Hello" />
    ```

</div>

There are no strict rules around the use of opening/closing vs. self-closing tags—any element can be written using either style. The difference is that self-closing tags cannot have any children; thus tend to be used for "simple" views such as `<label />` and `<image />`, while open tags are used for _layout_ views like `<frame>`, `<lane>` and `<panel>`.

:material-sign-caution: Because the [UI Framework](index.md) is an [abstraction](https://en.wikipedia.org/wiki/Abstraction_(computer_science)) over regular Views, it will not enable you to do anything with a View that would be impossible or prohibited in the [Core Library](../library/index.md), such as add children to a non-layout view.

### Tags

A tag is anything between a pair of angle brackets `<  >`, one of:

- An opening tag: `<panel>`
- A closing tag: `</panel>`
- A self-closing tag: `<panel />`.

While the term "tag" may sometimes be used synonymously with "element", tags refer more narrowly to the specific markup above, i.e. not including any [attributes](#attributes) or [children](#children).

In StarML, tags are not arbitrary; except for `<include>`, the tag defines the specific type of [view](../concepts.md#views) that is to be created, which in turn determines what attributes are allowed and how many children it is allowed to have.

These are the standard tags available in the UI framework:

!!! note

    The full list of tags currently supported can always be found in the [ViewFactory source](https://github.com/focustense/StardewUI/blob/dev/Framework/Binding/ViewFactory.cs).

| <div style="min-width:120px">Tag</div> | <div style="min-width:150px">View/Behavior</div> | Description |
| -------------- | --------------- | -------------- |
| `<banner>`     | [Banner](../library/standard-views.md#banner) | Displays a banner, aka ["scroll"](https://www.kdau.com/scrollish/), using a cartoonish font. Background optional. |
| `<button>`     | [Button](../library/standard-views.md#button) | Simple raised button with optional hover effect. |
| `<checkbox>`   | [Checkbox](../library/standard-views.md#checkbox) | Checkbox with optional clickable label. |
| `<digits>`     | [Tiny Number Label](../library/standard-views.md#tiny-number-label) | Displays a number in extra-small font; used for item quantities. |
| `<dropdown>`   | [Drop-Down List](../library/standard-views.md#drop-down-list) | Select from a list of options. |
| `<expander>`   | [Expander](../library/standard-views.md#expander) | Can be clicked to show or hide more content. |
| `<frame>`      | [Frame](../library/standard-views.md#frame) | Draws a border and/or background around another view. |
| `<grid>`       | [Grid](../library/standard-views.md#grid) | Uniform grid layout using either fixed size per item or fixed number of items per row/column. |
| `<image>`      | [Image](../library/standard-views.md#image) | Displays one image using a variety of scaling and fit options. |
| `<include>`    | [Included View](included-views.md) | Insert a different StarML view in this position, using its asset `name` to load the content. |
| `<label>`      | [Label](../library/standard-views.md#label) | Displays single- or multi-line text using a standard `SpriteFont`. |
| `<lane>`       | [Lane](../library/standard-views.md#lane) | Arranges other views along one axis, either horizontal (left to right) or vertical (top to bottom). |
| `<marquee>`    | [Marquee](../library/standard-views.md#marquee) | Animates scrolling text or other content horizontally; named after the [HTML Marquee](https://developer.mozilla.org/en-US/docs/Web/HTML/Element/marquee). |
| `<panel>`      | [Panel](../library/standard-views.md#panel) | Displays all children as layers ordered by z-index. Positions can be adjusted using margins. |
| `<scrollable>` | [Scrollable View](../library/standard-views.md#scrollable-view) | Shows scroll bars and arrows when content is too large to fit. |
| `<slider>`     | [Slider](../library/standard-views.md#slider) | A numeric slider that can be moved between a minimum and maximum range. |
| `<spacer>`     | [Spacer](../library/standard-views.md#spacer) | Draws nothing, but takes up space in the layout; used to "push" siblings to one side. |
| `<textinput>`  | [Text Input](../library/standard-views.md#text-input) | Input box for entering text; includes on-screen keyboard when activated by gamepad. |

## Attributes

Tags define what view will be created; attributes define _how_ it will look and behave, and specifically its [properties](#common-attributes) and [events](#events).

An attribute is any string appearing inside a tag that has the form `attr=value`, where `value` is one of the supported [flavors](#attribute-flavors), such as a quoted string like `text="Hello"` or data binding expression like `text={HelloMessage}`.

There are also [structural attributes](#structural-attributes) which are a separate topic.

### Common Attributes

!!! note

    These are the attributes common to **all** types of views (tags). Specific views usually have additional properties. Refer to the [standard views](../library/standard-views.md) documentation for details.
    
    In StarML, the name of an attribute is always the [kebab-case](https://developer.mozilla.org/en-US/docs/Glossary/Kebab_case) version of the property name, e.g. `HorizontalContentAlignment` becomes `horizontal-content-alignment`.

<div class="annotate attribute-table" markdown>

| Attribute | Direction&nbsp;(1) | Type&nbsp;(2) | Explanation |
| --------- | ---------- | -------- | ----------- |
| `actual-bounds` | Out | `Bounds` | True outer bounds of the view relative to its parent, including margins. |
| `border-size` | Out | `Vector2` | Size of the view's content plus padding and border width. |
| `content-bounds` | Out | `Bounds` | True bounds of the view's content relative to its parent, excluding margins. |
| `content-size` | Out | `Vector2` | Size of the view's content, not including padding, borders or margins. |
| `draggable` | In/Out | `bool` | Allows this view to receive [drag events](#events) (`drag`, `drag-start` and `drag-end`). |
| `focusable` | In/Out | `bool` | Allows this view to receive focus, or "snap", when using a gamepad. Automatically enabled for most interactive elements like `button` or `dropdown`. |
| `inner-size` | Out | `Vector2` | Size of the view's content and padding, excluding borders and margins. |
| `layout` | In/Out | `LayoutParameters` | The intended width and height. See [conversions](#type-conversions) for allowed values. |
| `margin` | In/Out | `Edges` | Pixel sizes for whitespace outside the border/background. |
| `name` | In/Out | `string` | For `<include>` elements, the view's asset name; for all other elements, a user-defined name used mainly for logging and troubleshooting. |
| `outer-size` | Out | `Bounds` | Total layout size occupied by the element, including padding, borders and margins. |
| `padding` | In/Out | `Edges` | Pixel sizes for whitespace inside the border/background. |
| `pointer-events-enabled` | In/Out | `bool` | Can be set to `false` to prevent receiving clicks, mouseovers, etc. Use when a transparent view is drawn on top of an interactive view. |
| `scroll-with-children` | In/Out | `Orientation` | Forces the entire view to be visible when navigating in the specified direction. See the [ScrollableView](../library/standard-views.md#scrollable-view) documentation for details. |
| `tags` | In/Out | `Tags` | Allows arbitrary data to be associated with the view. **Not supported in StarML yet** - may be supported in the future. |
| `tooltip` | In/Out | `string` | Tooltip to show when hovered with the mouse, or focused on via game controller. |
| `visibility` | In/Out | `Visibility` | Whether to show or hide the view; hiding does *not* remove it from the layout, use `*if` for that. |
| `z-index` | In/Out | `int` | Drawing order within the parent; higher indices are drawn later (on top). |

</div>

1.  **In/Out** properties can accept any directional [binding modifier](#binding-modifiers); **Out** properties are read-only and can only be used to write **to** the model, e.g. if you need to receive the view's actual pixel size after a layout.

    Attempting to bind an `Out` property without the `>` modifier, or attempting to assign it a literal value with a `="value"` type attribute, will cause it to fail.

2.  StardewUI's views will often have property types that can't be carried across the API boundary due to current limitations on [SMAPI](https://smapi.io/) and its version of [Pintail](https://github.com/Nanoray-pl/Pintail).

    To help you through this, [automatic conversions](#type-conversions) to and from other common types, such as tuples and XNA/MonoGame structures, are often provided if you want to bind one to your model and would rather not make everything a `string`.

    Note that some conversions may be _lossy_, i.e. if there is a difference in numeric precision.

### Structural Attributes

Structural attributes look like regular attributes, but with a `*` prefix. Instead of binding to properties or events, they control aspects of how the view tree is constructed.

| Attribute  | <div style="width: 120px">Expected Type</div>    | Description |
| ---------- | ---------------- | ----------- |
| `*case`    | Any              | Removes the element unless the value is equal to the most recent `*switch`. The types of `*switch` and `*case` must either match exactly or be [convertible](#type-conversions). |
| `*context` | Any              | Changes the [context](binding-context.md) that all child nodes bind to; used for heavily-nested data models. |
| `*if`      | `bool`           | Removes the element unless the specified condition is met. |
| `*outlet`  | `string`         | Specifies which of the parent node's [outlets](#outlets) will receive this node.<br>**Does not support bindings.** The attribute value must be a quoted string. |
| `*repeat`  | `IEnumerable` | Repeats the element over a collection, creating a new view for every item and setting its [context](binding-context.md) to that item. Applies to both regular and structural attributes; e.g. if `*repeat` and `*if` are both specified, then `*repeat` applies first. |
| `*switch`  | Any              | Sets the object that any subsequent `*case` attributes must match in order for their elements to show. |

### Events

Event attributes look similar to property attributes, but deal specifically with .NET [events](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/event) raised by views. More generally, they are one of the two ways it is possible for the UI to communicate something **back** to your mod (the other being [output/two-way bindings](#binding-modifiers)).

While some UI might be purely informational (e.g. a tooltip or HUD), any _interactive_ UI will probably involve one or more event bindings.

Bindings for events use a specific [flavor](#attribute-flavors), where the handler and its arguments are enclosed in a pair of pipes (`|`):

```html
<image click=|PlantCrops("corn", ^Quantity, $Button)| />
```

To experienced C# programmers, this may look like an ordinary method call, but it isn't. [Event bindings](binding-events.md) are a powerful and complex feature, and reading the documentation on them is strongly recommended before using them.

!!! note

    The events described below are the events common to **all** types of views (tags). Specific views may have additional events. Refer to the [standard views](../library/standard-views.md) documentation for details.
    
    In StarML, the name of an attribute is always the [kebab-case](https://developer.mozilla.org/en-US/docs/Glossary/Kebab_case) version of the event name, e.g. `LeftClick` becomes `left-click`.

<div class="annotate event-table" markdown>

| Event | Arguments&nbsp;Type&nbsp;(1) | Condition |
| --------- | -------- | ----------- |
| `button-press` | `ButtonEventArgs` | Any keyboard/gamepad button pressed. |
| `click` | `ClickEventArgs` | Any mouse/gamepad button clicked. |
| `drag` | `PointerEventArgs` | Ongoing drag operation; mouse was moved while button is still held. |
| `drag-end` | `PointerEventArgs` | End of a drag operation, mouse button was released. |
| `drag-start` | `PointerEventArgs` | First frame of a drag operation. |
| `left-click` | `ClickEventArgs` | Left mouse button or controller A was pressed. |
| `pointer-enter` | `PointerEventArgs` | Cursor just moved inside the view's bounds. |
| `pointer-leave` | `PointerEventArgs` | Cursor just moved outside the view's bounds. |
| `right-click` | `ClickEventArgs` | Right mouse button or controller X was pressed. |
| `wheel` | `WheelEventArgs` | Mouse wheel movement was detected. |

</div>

1.  Provided here as a reference for looking up the properties in [Events Source](https://github.com/focustense/StardewUI/tree/dev/Core/Events). You don't consume these types directly in your event handlers; consult the [Event Docs](binding-events.md) for details on how to set up handlers.

### Attribute Flavors

Regular HTML uses quoted attributes; to support the more complex behaviors where the attribute value should not be interpreted _literally_ (as in, the exact value inside the quotes), StarML uses different "flavors" of attributes using different punctuation.

!!! danger "Bindings are not tokens"

    Those accustomed to [Content Patcher Tokens](https://github.com/Pathoschild/StardewMods/blob/develop/ContentPatcher/docs/author-guide.md#tokens) may need to unlearn certain habits, because what goes on behind the scenes with StarML is far more complicated than string replacement. If you attempt to write "tokens" of the form `attr="A {{value}} B"`, you are going to be disappointed.

| <div style="width: 240px">Format</div> | Meaning    |
| --------------------------- | ----------- |
| `attr="value"`              | The literal ([converted](#type-conversions)) value inside the quotes. |
| `attr={PropertyName}`       | The current value of the specified [context property](binding-context.md). |
| `attr={@AssetName}`         | The current content of the [named asset](https://stardewvalleywiki.com/Modding:Modder_Guide/APIs/Content#What.27s_an_.27asset_name.27.3F). |
| `attr={#TranslationKey}`    | The translated string for a given [translation key](https://stardewvalleywiki.com/Modding:Modder_Guide/APIs/Translation). Can be either unqualified (`foo.bar`) if referring to a translation in the same mod that provided the view, or qualified (`authorname.ModName:foo.bar`) if referring to a translation in any other mod. |
| `attr=|Handler(Arg1, ...)|` | Call the specified [context method](binding-context.md), with the specified arguments; only valid for [event attributes](#events). |

??? note

    Double-braces (`{{` and `}}`) are allowed in place of single braces, for those heavily accustomed to Content Patcher syntax and JSON tokens in general, but are not recommended due to the inconsistency with other attributes and reduced readability.

### Binding Modifiers

In addition to the different [attribute flavors](#attribute-flavors),  context binding attributes—that is, those of the form `attr={PropertyName}`—can use modifier prefixes to either redirect to a different [context](binding-context.md) or change the direction of synchronization between context and view.

These modifiers work **only** with context property and event bindings; they cannot be used on literal attributes or assets.

| Modifier | <div style="width: 100px">Example</div> | Effect |
| --- | --- | --- |
| `^` | `{^Prop}` | Binds to the [parent context](binding-context.md#redirects) instead of the current context. Multiple `^` characters can be appended to go farther up, e.g. `^^^Prop`.
| `~` | `{~Foo.Prop}` | Binds to the [typed ancestor](binding-context.md#redirects) instead of the current context. |
| `<` | `{<Prop}` | Specifies an input binding, where the view receives its value from the model but does not write back. This is the default behavior when no modifier is used, and can generally be omitted. |
| `:` or `<:` | `{:Prop}` | Specifies a one-time input binding, which is the same as an ordinary input binding except that subsequent changes to the value will be ignored. |
| `>` | `{>Prop}` | Specifies an output binding, where the view writes its value to the model but does not read back. |
| `<>` | `{<>Prop}` | Specifies an in/out binding, where the view both receives its value from the model _and_ writes back to the model. |

!!! tip

    Context modifiers can be combined with direction modifiers, but order matters; the direction must come first. You can write `{<>^^Prop}` or `{>~Foo.Prop}`, but not `{^^<>Prop}` or `{~>Foo.Prop}`.

### Type Conversions

In the [SMAPI](https://smapi.io/) world, integrations between mods, including framework mods, are accomplished by [duck typing](https://en.wikipedia.org/wiki/Duck_typing), specifically through the [Pintail](https://github.com/Nanoray-pl/Pintail) library. This is a highly effective system for backward-compatibility and in some cases forward-compatibility; however, it presents many challenges for a UI library that uses many complex types.

Many [attribute types](#common-attributes) like `Bounds` and `Edges` simply can't be transmitted across the API boundary, even if you copy their definitions. Interface types such as `IView` are far too complex to copy over. StardewUI doesn't want you to have to deal with these issues on a one-off basis. Instead, it has a highly integrated system of type conversions: regardless of the actual, _real_ type of a view's property or event argument, it can be assigned or bound to any property with a _convertible_ type.

In the table below, the _String Format_ is what you can put in a [literal attribute](#attribute-flavors) or a `string` typed context property; _Converts From_ can be used with [input bindings](#binding-modifiers) and _Converts To_ can be used with output bindings or [event arguments](#events). For two-way bindings, the type must either be `string` or be in both the "from" and "to" lists.

!!! note

    All primitive types (numeric and `bool`) can be converted from their string representation, and are not shown explicitly in the conversion table. All types can also be converted _to_ a string, though whether or not the string is useful depends on its `ToString()` implementation.
    
    :material-decimal-decrease: indicates a possible loss of numeric precision.

<div class="annotate" markdown>

::spantable:: class="type-conversions"

| Type                     | String Format                      | Converts From                         | Converts To                             |
| ------------------------ | ---------------------------------- | ------------------------------------- | --------------------------------------- |
| Any `enum`               | (Field Name)                       | N/A                                   | N/A                                     |
| `Point`                  | `"x, y"`                           | N/A                                   | N/A                                     |
| `Vector2`                | `"x, y"`                           | N/A                                   | N/A                                     |
| `Rectangle`              | `"x, y, width, height"`            | N/A                                   | N/A                                     |
| `LayoutParameters` @span | `"<Length>"`&nbsp;(1)              | N/A @span                             | N/A @span                               |
|                          | `"<Width> <Height>"`               |                                       |                                         |
| `Length` @span           | `"<num>px"` (2)                    | N/A @span                             | N/A @span                               |
|                          | `"<num>%"` (3)                     |                                       |                                         |
|                          | `"content"` (4)                    |                                       |                                         |
|                          | `"stretch"` (5)                    |                                       |                                         |
| `Edges` @span            | `"left, right, top, bottom"` @span | `Tuple<int, int, int, int>`           | `Tuple<int, int, int, int>`             |
|                          |                                    | :material-decimal-decrease: `Vector4` | `Vector4`                               |
|                          |                                    | `int`                                 | @span                                   |
|                          |                                    | `Point`                               |                                         |
|                          |                                    | `Tuple<int, int>`                     |                                         |
|                          |                                    | :material-decimal-decrease: `Vector2` |                                         |
|                          |                                    | `Tuple<Point, Point>`                 |                                         |
|                          |                                    | :material-decimal-decrease: `Tuple<Vector2, Vector2>` |                         |
| `Bounds` @span           | N/A @span                          | N/A @span                             | `Tuple<float, float, float, float>`     |
|                          |                                    |                                       | `Tuple<Vector2, Vector2>`               |
|                          |                                    |                                       | `Vector4`                               |
|                          |                                    |                                       | :material-decimal-decrease: `Rectangle` |
| `Color` @span            | `"#rgb"`                           | N/A @span                             | N/A @span                               |
|                          | `"#rgba"`                          |                                       |                                         |
|                          | `"#rrggbb"`                        |                                       |                                         |
|                          | `"#rrggbbaa"`                      |                                       |                                         |
| `Sprite` (9) @span       | N/A @span                          | `Texture2D`                           | N/A @span                               |
|                          |                                    | `Tuple<Texture2D, Rectangle>`         |                                         |
|                          |                                    | `ParsedItemData`                      |                                         |
| `SpriteFont` @span       | `"dialogue"` (6)                   | N/A @span                             | N/A @span                               |
|                          | `"small"` (7)                      |                                       |                                         |
|                          | `"tiny"` (8)                       |                                       |                                         |
| `GridItemLayout` @span   | `"count: n"` (10)                  | N/A @span                             | N/A @span                               |
|                          | `"length: n"` (11)                 |                                       |                                         |

::end-spantable::

</div>

1.  Applies the same value to both the `Width` and `Height`. See `Length` conversions below for what values are allowed for `<Length>`, `<Width>` or `<Height>`.
2.  Specifies an exact value in pixels, e.g. `100px`.
3.  Percentage of the container's available width or height, e.g. `50%`.
4.  As wide/tall as the content wants itself to be, up to the available container size.
5.  Use the entire width/height available, _after_ any siblings that are not stretched.
6.  Reference to `Game1.dialogueFont`
7.  Reference to `Game1.smallFont`
8.  Reference to `Game1.tinyFont`
9.  Sprites can be bound to model properties, but should only be done for sprites that _must_ be dynamic. In the majority of cases, you should use [sprite assets](../getting-started/adding-ui-assets.md) instead.
10.  `n` is any positive integer; lays out the [grid](../library/standard-views.md#grid) using _n_ items per row/column and adjusts their size accordingly.
11.  `n` is any positive integer; lays out the [grid](../library/standard-views.md#grid) using a fixed width/height of `n` per item, and wraps to the next row/column when reaching the end.

If a type shows "N/A" for conversions, that means no conversion is available, either because it is not meant to be used in that scenario, or because it is already a shared type. Shared types such as any of the XNA/MonoGame types can be used directly in your model and therefore don't require any conversions, except from `string` to be used in literal attributes.

### Duck Typing :material-test-tube:{ title="Experimental" }

If a particular type conversion is not in the table above, it may be available for automatic implicit conversion. See the page on [duck typing](duck-typing.md) for rules and additional information on when and how this occurs.

## Children

In StarML—as in HTML or XML—an element's children are any tags appearing between the parent element's opening and closing tags:

!!! example

    ```html
    <lane>
        <label text="Title" />
        <frame>
            <label text="Content" />
        </frame>
    </lane>
    ```

In the above example:

* `<label text="Title" />` and `<frame>` are children of the `<lane>`
* `<label text="Content"` is a child of the `<frame>`

In general, only [layout views](../library/standard-views.md#layouts) can have children; attempting to add children to any other view type will cause an error.

Children can only be added to elements with separate opening and closing tags, e.g. `<lane>...</lane>`. Any self-closing tag, **even if** it corresponds to a layout view, cannot contain children, because a self-closing tag cannot be paired with a regular closing tag; just as with HTML or XML, `<lane/>...</lane>` is simply invalid StarML and will fail to parse.
{.no-code-break}

Because of these constraints, all documentation and examples on this site use opening/closing tags for layout views, and self-closing tags for other views. While this is not a requirement for valid StarML, it is recommended that you do the same in order to avoid confusion and lower the chances of creating invalid markup.

### Child Limits

Some layout views can have only **one** child, for example [frames](../library/standard-views.md#frame) and [scrollables](../library/standard-views.md#scrollable-view). That means the following is invalid markup:

!!! failure

    ```html
    <frame>
        <label text="Item 1" />
        <label text="Item 2" />
    </frame>
    ```

This will parse, but will either fail to display at all or fail to display correctly, because the actual frame _view_ only has a single `Content` view, not a list of views like a [lane](../library/standard-views.md#lane) or [panel](../library/standard-views.md#panel).

However, this rule applies only to the constructed view, not the markup itself. If only one of the children can actually display at a time, then there is no problem.

!!! warning "Risky"

    ```html
    <frame>
        <label *if={Item1Visible} text="Item 1" />
        <label *if={Item2Visible} text="Item 2" />
    </frame>
    ```

This is a "maybe" because, while you might personally know – or expect – that `Item1Visible` and `Item2Visible` cannot both be `true` at the same time, the framework itself does not know that and cannot enforce it, and doing it this way could cause it to fail when you least expect it, e.g. long after your mod has been released and been downloaded several times.

A better way is to use `*switch`:

!!! success

    ```html
    <frame *switch={VisibleItem}>
        <label *case="Item1" text="Item 1" />
        <label *case="Item2" text="Item 2" />
    </frame>
    ```

This version _cannot_ fail because `VisibleItem` cannot be both `Item1` and `Item2` at the same time. In other words, it is always OK to have multiple child nodes underneath a single-view layout, **if** all of those nodes have a distinct `*case`. Otherwise, there is the possibility of failure.

### Outlets

Children are grouped into "outlets", which represent specific areas or subcomponents of a layout.

Usually, this is invisible to you, as a layout view only performs one type of layout and therefore only has one child or collection of children. However, there are a few exceptions; one of them is the [Expander](../library/standard-views.md#expander), which allows specifying both a header view (the part that is always shown) and the content view (the part that can collapse).

To solve for these problems, StarML supports the `*outlet` [structural attribute](#structural-attributes), which allows targeting a specific outlet with a specific element:

```html
<expander>
    <button *outlet="header" text={ExpandCollapseText} />
    <label layout="stretch content" text={LongContent} />
</expander>
```

Outlet names are determined by the view itself, via the [OutletAttribute](../reference/stardewui/widgets/outletattribute.md) being applied to specific properties. In the case of `Expander`, there is one named outlet for the [Header](../reference/stardewui/widgets/expander.md#header) property, named "header". [Custom views](../library/custom-views.md) can create named outlets using the same attribute.

If an `*outlet` is not specified in the markup, then the default (unnamed) outlet is assumed. When multiple outlets are available, the [child limits](#child-limits) still apply per-outlet; if any given outlet, default or named, only allows a single view, then attempting to assign multiple views to that outlet would be invalid. For example, the following would _not_ be allowed for an `<expander>` element:

!!! failure "Broken"

    ```html
    <expander>
        <label *outlet="header" text="Hello" />
        <label *outlet="header" text="World" />
        <label layout="stretch content" text={LongContent} />
    </expander>
    ```

This is not allowed because the expander's `header` outlet requires a single content view. However, if this were to use an `*if*` condition then it would be valid again:

!!! success

    ```html
    <expander>
        <label *outlet="header" *if={IsCollapsed} text="Show help" />
        <label *outlet="header" *if={IsExpanded} text="Hide help" />
        <label layout="stretch content" text={LongContent} />
    </expander>
    ```

If there is a real need to have multiple views in the outlet, then this can also be achieved using a single layout view to hold them:

!!! success

    ```html
    <expander>
        <lane *outlet="header">
            <label text="Hello" />
            <label text="World" />
        </lane>
        <label layout="stretch content" text={LongContent} />
    </expander>
    ```

## Why not HTML?

Some web frameworks, like Vue, are a subset of HTML; their templates are syntactically valid HTML, constructed in such a way that any special behavior can be understood using special tags, attributes with unusual but valid prefixes like `:class`, and so on. So, why not simply pull in an HTML (or XHTML) parser and use that?

This went through careful consideration but ultimately seemed to have more negatives than positives. The potential positives:

- Several preexisting parsers are available for .NET;
- Common editors (Visual Studio Code, Notepad++, etc.) provide built-in syntax highlighting and validation;
- Already familiar to anyone with web development experience.

The negatives:

<div class="no-code-break" markdown>

- Using a third-party parser (or any third-party library) in a SMAPI environment is risky, and most parsers are not totally optimized for memory and speed.
- Common editors provide built-in syntax highlighting and validation **that could be misleading** given the real constraints of a Stardew UI. For example, interpolations like `attr="{value1} {value2}"` are valid HTML but not actually supported.
- Prior web development experience only helps through the shallowest part of the learning curve, not with binding/event attributes, model design, asset organization, etc.
- If all attributes are quoted, then character escaping becomes a more significant issue, especially for literal expressions; imagine documents full of `<button @click="Foo(&quot;Bar&quot;, Baz)" />`.
- An HTML parser only breaks the raw text down into stringly-typed elements and attributes; StardewUI would then need to do a second round of parsing (i.e. twice as much work) on all the tags and attribute values to determine which attributes are standard vs. structural, which values are literals vs. bindings vs. events, determine all the property names and types involved in an event handler, and so on.
- The actual runtime model (widget tree) is not comparable to an HTML DOM; does not understand unknown tags or standard HTML tags, does not distinguish between block/inline/other styles, and so on. As an abstraction over native UI, the inner workings are more like Qt or Android than a web browser; therefore, an "it's just HTML" approach would eventually end up causing more friction and confusion than a custom language.

</div>

**What about XML?**

An alternative would be something similar to XAML, but these have a way of getting out of control; consider, for example, a "simple" binding redirect in XAML:

```xml
<Label Content="{Binding Path=DisplayName, RelativeSource={RelativeSource Mode=FindAncestor AncestorLevel=1}}"/>
<Label Content="{Binding Path=DisplayName, RelativeSource={RelativeSource Mode=FindAncestor AncestorType={x:Type Foo}}}"/>
```

Compare to a StarML [context redirect](binding-context.md#redirects):

```html
<label text={^DisplayName} />
<label text={~Foo.DisplayName} />
```

And this is without any [binding modifiers](#binding-modifiers) which add even more noise to XAML.

This is not intended as a slight against XAML, which is actually a very powerful and mature tool for cross-platform development and far better than most of the available alternatives. However, it reflects a different use case: backing an _enormous_ framework (WPF) that is difficult to update once released, needs to scale to almost any imaginable UI scenario, and can expect a very high level of expertise from dev-users.

It is precisely because XML is so extensible that XML-based languages become convoluted over time; they encourage a way of thinking wherein new features are designed to fit into the XML structure somehow, even if the existing syntax is already confusing, verbose or requires some separately-interpreted micro-grammar, instead of the alternative of making a tiny change to the parser to support a relatively simple addition, like the `<>` or `^` modifiers above.

StardewUI is a mod, which means it is easy to change the parser, easy to roll out said updates, and has fewer users and UI scenarios to cover, so more effort can go into making easy things easy, instead of making nearly-impossible things possible. For scenarios that StarML can't cover, the [Core Library](../library/index.md) is the escape hatch.

## Why not JSON?

In many ways, JSON is the lingua franca of Stardew modding; it is the notation used by every content pack, not only for [Content Patcher](https://stardewvalleywiki.com/Modding:Content_Patcher) but for every [other framework](https://stardewvalleywiki.com/Modding:Content_pack_frameworks#Other_popular_frameworks) out there. Why should StardewUI be different?

While sharing some of the same concerns as HTML, such as double-parsing and character-escaping, it does route around other issues such as third-party libraries ([Json.NET](https://www.newtonsoft.com/json) is technically third-party, but is already available in every SMAPI environment).

To understand why it's not a good fit, we can take a look at what kind of structure might be required for a very simple view. In this example, we display a frame border, a single header line, and the names of a list of NPCs.

=== "StarML"

    ```html
    <frame layout="800px content">
        <lane orientation="vertical">
            <label font="dialogue" text="Header Text" />
            <label *repeat={Npcs} text={DisplayName} />
        </lane>
    </frame>
    ```

=== "Hypothetical JSON"

    ```json
    {
        "Type": "Frame",
        "Layout": "800px content",
        "Children": [
            {
                "Type": "Lane",
                "Orientation": "Vertical",
                "Children": [
                    {
                        "Type": "Label",
                        "Font": "dialogue",
                        "Text": "Header Text"
                    },
                    {
                        "Type": "Repeat",
                        "Collection": "{{Npcs}}",
                        "Content": {
                            "Type": "Label",
                            "Text": "{{DisplayName}}"
                        }
                    }
                ]
            }
        ]
    }
    ```

Notice how much longer, more verbose, and more _indented_ the JSON version is? Now picture how it would look in a view that is 5 levels deep, or 10. This is also the _best case_ scenario for syntax, assuming we make some negative tradeoffs on the parsing side and use `JObject` instead of any concrete type, which is what allows us to use dictionary-style attributes for `Orientation`, `Font` and so on instead of defining yet another `"Attributes": [...]` array with objects inside it.

At scale, the JSON version becomes unreadable; it is difficult to look at this data and even understand what it is supposed to do in broad strokes, to say nothing of keeping track of which indentation level you're at, what the context is at that level, etc. JSON _can_ be used to represent a syntax tree, but it is not very good at it (or any other tree).

JSON is native to the web; the "J" stands for "JavaScript", and yet it is not used by any mainstream web frameworks for defining the UI. This is because, while JSON is a good format for data storage, a full user interface cannot be represented very efficiently as plain data; it is not what we call _ergonomic_. That is exactly the problem that markup languages like HTML evolved to solve, and why we see similar mechanisms continue to evolve organically in every domain from [.NET](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/xaml/?view=netdesktop-8.0) to [Qt](https://doc.qt.io/qt-6/qtqml-index.html) to [Rust](https://slint.dev/).