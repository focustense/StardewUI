# Stardew UI

Retained-mode UI framework and widget library for Stardew mods.

## Background

The conventional approach to building UI in Stardew/[SMAPI](https://smapi.io/) land is to extend a class (not interface) named `IClickableMenu` for each new menu and essentially reimplement the entire layout and drawing system from scratch, each time, unless it is possible to reuse an existing specific menu type. Focus search - that is, controller support, or what the game calls "snappy menus" - is supported only through an antiquated system of "clickable components" and explicit neighbor mappings.

Stardew modding is highly approachable in general, but UI is a major rough patch. A single menu in a more complex mod can easily stretch to thousands of lines of code; "simpler" might only be a few hundred. Some authors have come up with their own frameworks, e.g. [SpaceShared/UI](https://github.com/spacechase0/StardewValleyMods/tree/develop/SpaceShared/UI), most of which are designed around narrow assumptions and are only usable within their respective author's uber-repo.

The situation is not very sustainable for authors with nontrivial UI goals, which is why this framework exists.

Primary design goals include:

- Standard widget-tree organization similar to other desktop/mobile frameworks like Qt, Android or Windows Forms.
- A good variety of built-in layouts and widgets that can handle most common scenarios.
- High performance, i.e. using an independent and skippable (via dirty-checking) layout pass where the majority of expensive work can be done as infrequently as possible.
- Low-boilerplate; a new menu or widget type could be implemented in fewer than 100 lines if it is not highly dynamic or visually complex.
- Automatic or mostly-automatic controller support without any complicated rituals.
- Events using actual `event` and `EventHandler` types hooked up to individual widgets. Responding to a click should be as easy as adding `button.Click += OnButtonClick`.
- Easy integration with `IClickableMenu`.

In a nutshell, the goal is to make UI development in Stardew mods as easy (or almost as easy) as UI development anywhere else.

## Quick Start

Mod integration primarily relies on [`ViewMenu`](src/ViewMenu.cs). A very simple menu might look as follows:

```csharp
class MyMenu(string text) : ViewMenu<Label>
{
    protected override Label CreateView()
    {
        return Label.Simple(text, Game1.dialogueFont);
    }
}
```

While this is an extremely trivial "menu" showing only static text, it shows all that is necessary to use a `View` as a menu; the `MyMenu` class and all `ViewMenu` descendants extend `IClickableMenu` and can therefore be assigned directly to `Game1.activeClickableMenu`.

Most UIs won't be this simple, and will therefore want to create their own custom view to use as the root. While it is possible to directly implement [`IView`](src/IView.cs), or extend from [`View`](src/View.cs), the more common scenario is to extend from [`WrapperView`](src/WrapperView.cs) which allows composing views in the widget-tree style. Suppose want to show a list of in-game items:

```csharp
class MyData
{
    public List<Item> Items { get; set; } = [];
}

class MyMenu(MyData data) : ViewMenu<MyView>
{
	protected override MyView CreateView()
    {
        return new MyView(data);
    }
}

class MyView(MyData data) : WrapperView<Frame>
{
    protected override Frame CreateView()
    {
        var content = new Lane()
        {
            Layout = LayoutParameters.AutoRow(),
            Orientation = Orientation.Vertical,
            Children = data.Items.Select(CreateItemRow).ToList(),
        };
        return new Frame()
        {
            Background = Sprites.FrameBackground,
            Padding = new(16, 8),
        };
    }
    
    private Lane CreateItemRow(Item item)
    {
        return new Lane()
        {
        	Layout = LayoutParameters.FitContent(),
            Margin = new(0, 8),
            Orientation = Orientation.Horizontal,	// Optional, this is the default
            Children = [
                new Image()
                {
                    Layout = LayoutParameters.FixedSize(64, 64),
                    Margin = new(Right: 16),
                    Sprite = Sprites.ForItem(item),
                },
                Label.Simple(item.DisplayName),
            ]
        };
    }
}
```

And there we have our item-list view, with correct whitespace and framing, in barely 30 executable LOC. (Note, `Sprites` is typically a very short static class defined by the individual mod, holding all the sprite data that will be used in its UI; since Stardew has thousands of sprites, StardewUI doesn't include individual definitions in order to avoid bloat).

## What's Included

The example above is just scratching the surface. These widgets are standard:

| Widget          | Purpose/Behavior                                             |
| --------------- | ------------------------------------------------------------ |
| Label           | Displays read-only, multi-line text.                         |
| TinyNumberLabel | Displays a numeric value using digit sprites, such as the strongly-outlined numbers used to display item stack sizes. |
| Banner          | Displays text using the special "heading font" (`SpriteText` in game), enclosed in the banner or "scroll" style background. Typically used for menu titles. |
| TextInput       | An editable text control. Supports flashing I-beam, and movable caret position using either the mouse or arrow keys. |
| Image           | Renders a `Sprite` using a variety of different scaling and animation options. (A sprite can be built from any in-game `Texture2d`). |
| Panel           | The simplest layout type, renders all its children using the same overlapping boundary rectangle. Children can be "positioned" using margins, and ordered by z-index. Typically used for overlays. |
| Frame           | Renders a single content view inside a border and/or background. |
| Lane            | Displays any number of content views laid out in a single direction (left to right or top to bottom). |
| Grid            | Displays any number of content views in a 2D uniform grid. (For non-uniform grids, it is better to use a combination of horizontal and vertical `Lane`s). |
| ScrollContainer | Allows a single content view to grow to any size, but have drawing clipped to the scroll region. Scrolling can be controlled programmatically, but this is generally always combined with a `Scrollbar`. |
| Scrollbar       | Tracks and controls the scrolling state of a `ScrollContainer`. Automatically hides itself if there is not enough content to scroll. |
| NineSlice       | Not a view itself, but used by many other views including `Image` and `Frame`. Can take any `Sprite` (from a `Texture2d` and `Rectangle`) and scale it proportionally to any size using [9-slice scaling](https://en.wikipedia.org/wiki/9-slice_scaling). Especially important with sprites such as menu/frame/button borders and backgrounds. |
| Animator        | A convenient way to start animating any property of a view; for example, animating `Image.Scale` on hover, which itself is captured in the `HoverScale` utility class. **Note:** Like all frameworks, animating a property that has cascading layout effects can cause performance slowdowns. |

Other widgets and features may come later if there is any specific need for them. Many types of common widgets, such as "tabs" and "checkboxes", tend to be very trivial wrappers around an `Image` since all game UI is done with sprites and not canvases.

## How to Install/Use

The Stardew/SMAPI setup is not friendly toward binary dependencies, which is why so many mods use monorepos with "Common" or "Shared" projects. StardewUI falls along similar lines, but is designed to be maintained as a standalone project and imported as a submodule.

To include this in your mod:

1. `cd` into your _solution_ directory (that is, the directory containing the `.sln`, _not_ the `.csproj`)
2. Add the submodule: `git submodule add https://github.com/focustense/StardewUI.git`
3. Add the shared project to your solution. In Visual Studio, right-click on the Solution in the Solution Explorer, then **Add** -> **Existing Project...**, find and select the `StardewUI.shproj` inside the `StardewUI` folder created in step 1.
4. Add a project reference to your mod. In Visual Studio, right-click on on your Project (_not_ the solution, and _not_ the StardewUI project; the project for _your mod_), then **Add** -> **Shared Project Reference**, and tick `StardewUI`. If you don't see `StardewUI` in the list, then you made a mistake in one of the previous steps.
5. Double-check; if you open your `.csproj` file in VS or a text editor, it should have the following line:
   `<Import Project="..\StardewUI\StardewUI.projitems" Label="Shared" />`

The dependencies are now set up and you can start writing your first `ViewMenu`, `WrapperView` etc.

## Documentation

...just kidding. There isn't any right now. However, the [source](src/) is extensively marked up with Doxygen comments, so it should be a good source of additional information.

I don't anticipate having a lot of users of this library. If there turns out to be high demand, then more docs may be added in the future.

## Questions/Bugs?

StardewUI is a developer tool, for mod _creators_. If you are not making C#/SMAPI-based mods yourself, and somehow found your way to this page, then you are in the wrong place; please, do not file bugs here relating to specific mods, or contact me on Discord about any mod that I did not personally author.

Mod authors are always welcome to file a [GitHub issue](https://github.com/focustense/StardewUI/issues) or ping me on the [SV Discord](https://discord.com/invite/stardewvalley).