# Stardew Markup Language (StarML)

StarML is the markup language used by the [UI Framework](UI-Framework). It is an HTML-like syntax based on a tree of [elements](#elements), each corresponding to a [view](Concepts#Views).

It shares many traits with [Angular templates](https://angular.dev/guide/templates), particularly an enhanced set of [attributes](#attributes) that perform [data binding](Concepts#data-binding) and other special functions.

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

Every StarML element corresponds to a [View](Concepts#views), whose type is decided by its tag; every attribute corresponds to a property or event on that view (except [structural attributes](#structural-attributes)), and child elements correspond to the view's child views.

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

:material-sign-caution: Because the [UI Framework](UI-Framework) is an [abstraction](https://en.wikipedia.org/wiki/Abstraction_(computer_science)) over regular Views, it will not enable you to do anything with a View that would be impossible or prohibited in the [Core Library](Core-Library), such as add children to a non-layout view.

### Tags

A tag is anything between a pair of angle brackets `<  >`, one of:

- An opening tag: `<panel>`
- A closing tag: `</panel>`
- A self-closing tag: `<panel />`.

While the term "tag" may sometimes be used synonymously with "element", tags refer more narrowly to the specific markup above, i.e. not including any [attributes](#attributes) or [children](#children).

In StarML, tags are not arbitrary; except for `<include>`, the tag defines the specific type of [view](Concepts#views) that is to be created, which in turn determines what attributes are allowed and how many children it is allowed to have.

These are the standard tags available in the UI framework:

!!! note

    The full list of tags currently supported can always be found in the [ViewFactory source](https://github.com/focustense/StardewUI/blob/dev/Framework/Binding/ViewFactory.cs).

| <div style="min-width:120px">Tag</div> | <div style="min-width:150px">Widget/Behavior</div> | Description |
| -------------- | --------------- | -------------- |
| `<banner>`     | [Banner](Standard-Widgets#Banner) | Displays a banner, aka ["scroll"](https://www.kdau.com/scrollish/), using a cartoonish font. Background optional. |
| `<button>`     | [Button](Standard-Widgets#Button) | Simple raised button with optional hover effect. |
| `<checkbox>`   | [Checkbox](Standard-Widgets#Checkbox) | Checkbox with optional clickable label. |
| `<digits>`     | [Tiny Number Label](Standard-Widgets#Tiny-Number-Label) | Displays a number in extra-small font; used for item quantities. |
| `<dropdown>`   | [Drop-Down List](Standard-Widgets#Drop-Down-List) | Select from a list of options. |
| `<expander>`   | [Expander](Standard-Widgets#Expander) | Can be clicked to show or hide more content. |
| `<frame>`      | [Frame](Standard-Widgets#Frame) | Draws a border and/or background around another view. |
| `<grid>`       | [Grid](Standard-Widgets#Grid) | Uniform grid layout using either fixed size per item or fixed number of items per row/column. |
| `<image>`      | [Image](Standard-Widgets#Image) | Displays one image using a variety of scaling and fit options. |
| `<include>`    | [Included View](Included-Views) | Insert a different StarML view in this position, using its asset `name` to load the content. |
| `<label>`      | [Label](Standard-Widgets#Label) | Displays single- or multi-line text using a standard `SpriteFont`. |
| `<lane>`       | [Lane](Standard-Widgets#Lane) | Arranges other views along one axis, either horizontal (left to right) or vertical (top to bottom). |
| `<marquee>`    | [Marquee](Standard-Widgets#Marquee) | Animates scrolling text or other content horizontally; named after the [HTML Marquee](https://developer.mozilla.org/en-US/docs/Web/HTML/Element/marquee). |
| `<panel>`      | [Panel](Standard-Widgets#Panel) | Displays all children as layers ordered by z-index. Positions can be adjusted using margins. |
| `<scrollable>` | [Scrollable View](Standard-Widgets#Scrollable-View) | Shows scroll bars and arrows when content is too large to fit. |
| `<slider>`     | [Slider](Standard-Widgets#Slider) | A numeric slider that can be moved between a minimum and maximum range. |
| `<spacer>`     | [Spacer](Standard-Widgets#Spacer) | Draws nothing, but takes up space in the layout; used to "push" siblings to one side. |
| `<textinput>`  | [Text Input](Standard-Widgets#Text-Input) | Input box for entering text; includes on-screen keyboard when activated by gamepad. |

## Attributes

Tags define what view will be created; attributes define _how_ it will look and behave, and specifically its [properties](#common-attributes) and [events](#common-events).

An attribute is any string appearing inside a tag that has the form `attr=value`, where `value` is one of the supported [flavors](#attribute-flavors), such as a quoted string like `text="Hello"` or data binding expression like `text={HelloMessage}`.

There are also [structural attributes](#structural-attributes) which are a separate topic.

### Common Attributes

!!! note

    These are the attributes common to **all** types of views (tags). Specific widgets usually have additional properties. Refer to the [standard widgets](Standard-Widgets) documentation for details.

    In StarML, the name of an attribute is always the [kebab-case](https://developer.mozilla.org/en-US/docs/Glossary/Kebab_case) version of the property name, e.g. `HorizontalContentAlignment` becomes `horizontal-content-alignment`.

<div class="annotate attribute-table" markdown>

| Attribute | Direction&nbsp;(1) | Type&nbsp;(2) | Explanation |
| --------- | ---------- | -------- | ----------- |
| `actual-bounds` | Out | `Bounds` | True outer bounds of the view relative to its parent, including margins. |
| `border-size` | Out | `Vector2` | Size of the view's content plus padding and border width. |
| `content-bounds` | Out | `Bounds` | True bounds of the view's content relative to its parent, excluding margins. |
| `content-size` | Out | `Vector2` | Size of the view's content, not including padding, borders or margins. |
| `draggable` | In/Out | `bool` | Allows this view to receive [drag events](#common-events) (`drag`, `drag-start` and `drag-end`). |
| `focusable` | In/Out | `bool` | Allows this view to receive focus, or "snap", when using a gamepad. Automatically enabled for most interactive elements like `button` or `dropdown`. |
| `inner-size` | Out | `Vector2` | Size of the view's content and padding, excluding borders and margins. |
| `layout` | In/Out | `LayoutParameters` | The intended width and height. See [conversions](#type-conversions) for allowed values. |
| `margin` | In/Out | `Edges` | Pixel sizes for whitespace outside the border/background. |
| `name` | In/Out | `string` | For `<include>` elements, the view's asset name; for all other elements, a user-defined name used mainly for logging and troubleshooting. |
| `outer-size` | Out | `Bounds` | Total layout size occupied by the element, including padding, borders and margins. |
| `padding` | In/Out | `Edges` | Pixel sizes for whitespace inside the border/background. |
| `pointer-events-enabled` | In/Out | `bool` | Can be set to `false` to prevent receiving clicks, mouseovers, etc. Use when a transparent view is drawn on top of an interactive view. |
| `scroll-with-children` | In/Out | `Orientation` | Forces the entire view to be visible when navigating in the specified direction. See the [ScrollableView](Standard-Widgets#Scrollable-View) documentation for details. |
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
| `*context` | Any              | Changes the [context](Binding-Context) that all child nodes bind to; used for heavily-nested data models. |
| `*if`      | `bool`           | Removes the element unless the specified condition is met. |
| `*repeat`  | `IEnumerable` | Repeats the element over a collection, creating a new view for every item and setting its [context](Binding-Context) to that item. Applies to both regular and structural attributes; e.g. if `*repeat` and `*if` are both specified, then `*repeat` applies first. |
| `*switch`  | Any              | Sets the object that any subsequent `*case` attributes must match in order for their elements to show. |

### Events

Event attributes look similar to property attributes, but deal specifically with .NET [events](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/event) raised by views. More generally, they are one of the two ways it is possible for the UI to communicate something **back** to your mod (the other being [output/two-way bindings](#binding-modifiers)).

While some UI might be purely informational (e.g. a tooltip or HUD), any _interactive_ UI will probably involve one or more event bindings.

Bindings for events use a specific [flavor](#attribute-flavor), where the handler and its arguments are enclosed in a pair of pipes (`|`):

```html
<image click=|PlantCrops("corn", ^Quantity, $Button)| />
```

To experienced C# programmers, this may look like an ordinary method call, but it isn't. [Event bindings](Binding-Events) are a powerful and complex feature, and reading the documentation on them is strongly recommended before using them.

!!! note

    The events described below are the events common to **all** types of views (tags). Specific widgets may have additional events. Refer to the [standard widgets](Standard-Widgets) documentation for details.

    In StarML, the name of an attribute is always the [kebab-case](https://developer.mozilla.org/en-US/docs/Glossary/Kebab_case) version of the event name, e.g. `LeftClick` becomes `left-click`.

<div class="annotate event-table" markdown>

| Event | Arguments&nbsp;Type&nbsp;(1) | Condition |
| --------- | -------- | ----------- |
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

1.  Provided here as a reference for looking up the properties in [Events Source](https://github.com/focustense/StardewUI/tree/dev/Core/Events). You don't consume these types directly in your event handlers; consult the [Event Docs](Binding-Events) for details on how to set up handlers.

### Attribute Flavors

Regular HTML uses quoted attributes; to support the more complex behaviors where the attribute value should not be interpreted _literally_ (as in, the exact value inside the quotes), StarML uses different "flavors" of attributes using different punctuation.

!!! danger "Bindings are not tokens"

    Those accustomed to [Content Patcher Tokens](https://github.com/Pathoschild/StardewMods/blob/develop/ContentPatcher/docs/author-guide.md#tokens) may need to unlearn certain habits, because what goes on behind the scenes with StarML is far more complicated than string replacement. If you attempt to write "tokens" of the form `attr="A {{value}} B"`, you are going to be disappointed.

| <div style="width: 240px">Format</div> | Meaning    |
| --------------------------- | ----------- |
| `attr="value"`              | The literal ([converted](#type-conversions)) value inside the quotes. |
| `attr={PropertyName}`       | The current value of the specified [context property](Binding-Context). |
| `attr={@AssetName}`         | The current content of the [named asset](https://stardewvalleywiki.com/Modding:Modder_Guide/APIs/Content#What.27s_an_.27asset_name.27.3F). |
| `attr=|Handler(Arg1, ...)|` | Call the specified [context method](Binding-Context), with the specified arguments; only valid for [event attributes](#common-events). |

??? note

    Double-braces (`{{` and `}}`) are allowed in place of single braces, for those heavily accustomed to Content Patcher syntax and JSON tokens in general, but are not recommended due to the inconsistency with other attributes and reduced readability.

### Binding Modifiers

In addition to the different [attribute flavors](#attribute-flavors),  context binding attributes—that is, those of the form `attr={PropertyName}`—can use modifier prefixes to either redirect to a different [context](Binding-Context) or change the direction of synchronization between context and view.

These modifiers work **only** with context property and event bindings; they cannot be used on literal attributes or assets.

| Modifier | <div style="width: 100px">Example</div> | Effect |
| --- | --- | --- |
| `^` | `{^Prop}` | Binds to the [parent context](Binding-Context#redirects) instead of the current context. Multiple `^` characters can be appended to go farther up, e.g. `^^^Prop`.
| `~` | `{~Foo.Prop}` | Binds to the [typed ancestor](Binding-Context#redirects) instead of the current context. |
| `<` | `{<Prop}` | Specifies an input binding, where the view receives its value from the model but does not write back. This is the default behavior when no modifier is used, and can generally be omitted. |
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
9.  Sprites can be bound to model properties, but should only be done for sprites that _must_ be dynamic. In the majority of cases, you should use [sprite assets](API#asset-registration) instead.
10.  `n` is any positive integer; lays out the [grid](Standard-Widgets#grid) using _n_ items per row/column and adjusts their size accordingly.
11.  `n` is any positive integer; lays out the [grid](Standard-Widgets#grid) using a fixed width/height of `n` per item, and wraps to the next row/column when reaching the end.

If a type shows "N/A" for conversions, that means no conversion is available, either because it is not meant to be used in that scenario, or because it is already a shared type. Shared types such as any of the XNA/MonoGame types can be used directly in your model and therefore don't require any conversions, except from `string` to be used in literal attributes.

Unfortunately, allowing mods to arbitrarily extend this list would be a chicken-egg problem: you need direct access to the destination type in order to implement a converter. However, if anything important is missing, feel free to make a [request](https://github.com/focustense/StardewUI/issues) or a [contribution](https://github.com/focustense/StardewUI/blob/dev/Framework/Converters/ValueConverterFactory.cs) (writing a converter is usually very simple, often a single line of code).

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

Compare to a StarML [context redirect](Binding-Context#redirects):

```html
<label text={^DisplayName} />
<label text={~Foo.DisplayName} />
```

And this is without any [binding modifiers](#binding-modifiers) which add even more noise to XAML.

This is not intended as a slight against XAML, which is actually a very powerful and mature tool for cross-platform development and far better than most of the available alternatives. However, it reflects a different use case: backing an _enormous_ framework (WPF) that is difficult to update once released, needs to scale to almost any imaginable UI scenario, and can expect a very high level of expertise from dev-users.

It is precisely because XML is so extensible that XML-based languages become convoluted over time; they encourage a way of thinking wherein new features are designed to fit into the XML structure somehow, even if the existing syntax is already confusing, verbose or requires some separately-interpreted micro-grammar, instead of the alternative of making a tiny change to the parser to support a relatively simple addition, like the `<>` or `^` modifiers above.

StardewUI is a mod, which means it is easy to change the parser, easy to roll out said updates, and has fewer users and UI scenarios to cover, so more effort can go into making easy things easy, instead of making nearly-impossible things possible. For scenarios that StarML can't cover, the [Core Library](Core-Library) is the escape hatch.

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