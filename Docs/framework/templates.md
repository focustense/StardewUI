---
title: Templates
---

# Templates  :material-test-tube:{ title="Experimental" }

Templates are a very quick-to-use and lightweight method for reusing snippets of markup, reducing repetition and making it easier to customize/mod a UI.

The main differences from [included views](included-views.md) are:

- Each template is scoped to the [StarML](starml.md) (`.sml`) file that declares it and cannot be used outside that file[^1];
- Templates are processed before [data binding](../concepts.md#data-binding) and are typically more efficient than includes;
- Elements inside templates are allowed to use [template attributes](#template-attributes) (`{&attr}`) and [event arguments](binding-events.md#event-attributes) (`&attr`) which are replaced by the actual attributes provided to the template.

The ideal use of templates is in situations where there will be several similar but not quite identical elements, or when the overall structure is clearly a UI decision and not data-dependent, making use of [`*repeat`](starml.md#structural-attributes) infeasible or impractical. Examples:

- Form rows in a settings page, for setting up a uniform arrangement of label, tooltip, configuration widget, etc.
- Tab pages with similar primary layout but different individual sections and features;
- Organizational UI where the same thing (e.g. game `Item`) can be moved between multiple sections or containers, and should preserve its appearance in each section.

Of course, templates can also be used with `*repeat` and any other attributes as a way of refactoring, especially in complex templates that would otherwise be heavily nested.

[^1]: This behavior is likely to change in a future version that supports template inclusions.

## Declaration and Usage

Declaring a template is simple: just add a `<template>` element to the top level of a StarML file. Then, use it by writing a tag with the same name.

!!! example

    ```html
    <lane orientation="vertical">
        <hello npc="Robin" />
        <hello npc="Sebastian" />
    </lane>
    
    <template name="hello">
        <lane>
            <image sprite={@Mods/MyMod/Sprites/UI:Wave} />
            <label text={&npc} />
        </lane>
    </template>
    ```

Template elements can appear before or after the main view, though we recommend putting them after so that it is easier to understand the overall "flow" of a view by reading from top to bottom.

Multiple templates can be added to the same file:

!!! example

    ```html
    <lane orientation="vertical">
        <hello npc="Robin" />
        <goodbye npc="Sebastian" />
    </lane>
    
    <template name="hello">
        <lane>
            <image sprite={@Mods/MyMod/Sprites/UI:Wave} />
            <label text={&npc} />
        </lane>
    </template>

    <template name="goodbye">
        <lane>
            <image sprite={@Mods/MyMod/Sprites/UI:SadFace} />
            <label text={&npc} />
        </lane>
    </template>
    ```

Templates can even be nested:

!!! example

    **Important:** Templates that reference other templates **must** appear in the order that they are used, with the "outer" template being declared before the "inner" template.

    ```html
    <lane orientation="vertical">
        <hello npc="Robin" />
        <goodbye npc="Sebastian" />
    </lane>
    
    <template name="hello">
        <greeting npc={&npc} sprite={@Mods/MyMod/Sprites/UI:Wave} />
    </template>

    <template name="goodbye">
        <greeting npc={&npc} sprite={@Mods/MyMod/Sprites/UI:SadFace} />
    </template>
    
    <template name="greeting">
        <lane>
            <image sprite={&sprite} />
            <label text={&npc} />
        </lane>
    </template>
    ```

## Template Attributes

In the above examples, you will have probably noticed the unusual attributes such as `{&npc}` and `{&sprite}`. These are _template attributes_, a special type of [attribute](starml.md#attribute-flavors) allowed only within `<template>` elements.

All template attributes are implicit and don't require any special declaration. The attribute `{&npc}` means: "take the `npc` attribute that was provided to the template and substitute it here."

Since the entire attribute is substituted, it means that for our hypothetical `hello` template:

```html
<template name="hello">
    <lane>
        <image sprite={@Mods/MyMod/Sprites/UI:Wave} />
        <label text={&npc} />
    </lane>
</template>
```

Any of the following substitutions are valid:

```html
<hello npc="Clint" />
<hello npc={NpcName} />
<hello npc={#CustomNpcName} />
```

All of the above will have the exact same behavior as the equivalent StarML written without any templates.

### Event Arguments

Template attributes can also be passed to event arguments by omitting the braces:

```html
<hello-button npc="Leah" />

<template name="hello-button">
    <button text={&npc} left-click=|Greet("Hello", &npc)| />
</template>
```

This works the same as attribute substitution, with one important caveat: it will not allow the use of any argument flavor that is not also one of the supported by [event arguments](binding-events.md#event-attributes).

Specifically, literals and context bindings are allowed:

!!! success "OK"

    ```html
    <hello-button npc="Sandy" />
    <hello-button npc={NpcName} />
    ```

But translation and asset bindings are **not** allowed:

!!! failure "Broken"

    ```html
    <hello-button npc={@Path/To/Asset} />
    <hello-button npc={#CustomNpcName} />
    ```

The more surprising of the two above might be the translation binding; however, translations are not currently allowed in event arguments, which means that they cannot be passed from template attributes to event arguments either.

## Outlets

Templates support the equivalent of [children outlets](starml.md#outlets), using the `<outlet>` tag, which is only valid from within a `<template>`. When an instance of the template is created, any child nodes will be inserted into the outlet; if there are no children, the the outlet is removed.

!!! example

    This view from the [form example](https://github.com/focustense/StardewUI/blob/master/TestMod/assets/views/Example-Form.sml) uses an outlet:

    ```html
    <form-row title={#ExampleForm.TurboBoost.Title}>
        <checkbox is-checked={<>EnableTurboBoost} />
    </form-row>

    <template name="form-row">
        <lane layout="stretch content" margin="16,0" vertical-content-alignment="middle">
            <label layout="280px content" margin="0,8" text={&title} />
            <outlet />
        </lane>
    </template>
    ```

    Which is equivalent to writing:

    ```html
    <lane layout="stretch content" margin="16,0" vertical-content-alignment="middle">
        <label layout="280px content" margin="0,8" text={#ExampleForm.TurboBoost.Title} />
        <checkbox is-checked={<>EnableTurboBoost} />
    </lane>
    ```

Named outlets, and outlets with multiple children, are also supported. To create and use a named outlet:

1. Add an `<outlet>` tag with non-blank `name` attribute to the template;
2. When using the template, add one or more elements with an [`*outlet` attribute](starml.md#structural-attributes).

!!! example

    Modifying the above example to include an additional, named outlet:

    ```html
    <form-row title={#ExampleForm.TurboBoost.Title}>
        <checkbox is-checked={<>EnableTurboBoost} />
        <label text="(highly recommended!)" />
        <image *outlet="overlay"
               layout="stretch"
               fit="stretch"
               sprite={@Mods/MyMod/Sprites/UI:TurboBoostOverlay} />
    </form-row>

    <template name="form-row-with-overlay">
        <panel>
            <lane layout="stretch content" margin="16,0" vertical-content-alignment="middle">
                <label layout="280px content" margin="0,8" text={&title} />
                <outlet />
            </lane>
            <outlet name="overlay" />
        </panel>
    </template>
    ```

    Expands to:
    
    ```html
    <panel>
        <lane layout="stretch content" margin="16,0" vertical-content-alignment="middle">
            <label layout="280px content" margin="0,8" text={#ExampleForm.TurboBoost.Title} />
            <checkbox is-checked={<>EnableTurboBoost} />
            <label text="(highly recommended!)" />
        </lane>
        <image layout="stretch" fit="stretch" sprite={@Mods/MyMod/Sprites/UI:TurboBoostOverlay} />
    </panel>
    ```

Template outlets come with a few caveats:

- They cannot cause any of the normal [child limits](starml.md#child-limits) to be broken. For example, if an `<outlet>` is the child of a `<frame>`, then that outlet can only have a single child.
- Named template outlets are not recommended to be used in the same scope as a view with named outlets, such as an [Expander](../library/standard-views.md#expander); doing so may break or cause unexpected behavior.
- The `<outlet>` tag is not a real view, and any children of the `<outlet>` itself (as opposed to children of the template _instance_) will be ignored.
- Outlets, like [template attributes](#template-attributes), are expanded inline, so keep this in mind for [binding context redirects](binding-context.md#redirects) and any other data binding behavior; the outlet children will have the same context as the real parent in the expanded view.