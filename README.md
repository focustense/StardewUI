![Logo](Docs/images/logo-text.png)

A convenient, fast and scalable UI framework for [Stardew Valley](https://www.stardewvalley.net/) mods, dedicated to making UI development a breeze rather than a chore. Inspired in part by [Angular](https://angular.dev/) and [XAML](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/xaml/?view=netdesktop-8.0).

* [:seedling: StardewUI Core](https://focustense.github.io/StardewUI/library/) is an includable code library in the form of a .NET Shared Project, containing the layout and rendering services, fundamental widgets, and integrations with the Stardew Valley base game, in particular its Menu system.

* [:deciduous_tree: StardewUI Framework](https://focustense.github.io/StardewUI/framework/) is a standalone mod that hosts its own version of the Core library and provides a [Model-View-Whatever](https://www.beyondjava.net/model-view-whatever) (MVW) binding system using [StarML](https://focustense.github.io/StardewUI/framework/starml/) as well as many other quality-of-life features not found in the Core.

New to Stardew UI and wondering what this library is about? Check out the [examples](https://focustense.github.io/StardewUI/examples/) to see the kinds of UI you can build in 10 minutes or less!

## Documentation

StardewUI has [official documentation](https://focustense.github.io/StardewUI). If you're looking to quickly get up and running, the [Getting Started](https://focustense.github.io/StardewUI/getting-started/) guide is a great place to begin.

_Still have questions? Visit the [SV Discord](https://discord.com/invite/stardewvalley) and ping [@focustense](https://discordapp.com/users/831917573204738069)._

## Example

The following is taken from the official [scrolling item grid](https://focustense.github.io/StardewUI/examples/scrolling-item-grid) example.

**ScrollableItemGrid.sml**

```html
<lane orientation="vertical" horizontal-content-alignment="middle">
    <banner background={@Mods/StardewUI/Sprites/BannerBackground}
            background-border-thickness="48,0"
            padding="12"
            text={HeaderText} />
    <frame layout="880px 640px"
           margin="0,16,0,0"
           padding="32,24"
           background={@Mods/StardewUI/Sprites/ControlBorder}>
        <scrollable peeking="128">
            <grid layout="stretch content"
                  item-layout="length: 64"
                  item-spacing="16,16"
                  horizontal-item-alignment="middle">
                <image *repeat={Items}
                       layout="stretch content"
                       sprite={this}
                       tooltip={DisplayName}
                       focusable="true" />
            </grid>
        </scrollable>
    </frame>
</lane>
```

**EdiblesViewModel.cs**

```cs
internal record EdiblesViewModel(string HeaderText, ParsedItemData[] Items)
{
    public static EdiblesViewModel LoadFromGameData()
    {
        int[] edibleCategories = [
            StardewValley.Object.CookingCategory,
            StardewValley.Object.EggCategory,
            StardewValley.Object.FishCategory,
            StardewValley.Object.FruitsCategory,
            StardewValley.Object.meatCategory,
            StardewValley.Object.MilkCategory,
            StardewValley.Object.VegetableCategory,
        ];
        var items = ItemRegistry.ItemTypes
            .Single(type => type.Identifier == ItemRegistry.type_object)
            .GetAllIds()
            .Select(id => ItemRegistry.GetDataOrErrorItem(id))
            .Where(data => edibleCategories.Contains(data.Category))
            .ToArray();
        return new("All Edibles", items);
    }
}
```

## Contributions

StardewUI is a big, ambitious project, so contributions are always welcome.

- To see what's at the top of the list, check the latest [milestones](https://github.com/focustense/StardewUI/milestones).
- More [examples](https://focustense.github.io/StardewUI/examples/) are always nice to have, especially if they show off some new trick or little-used feature that other examples don't already cover.
- If you're interested in a feature that's not planned, please start a [discussion](https://github.com/focustense/StardewUI/discussions) first, or ping/ask in Discord, in order to get a sense of whether or not a PR is likely to be accepted.
- Please **do** start [issues](https://github.com/focustense/StardewUI/issues) for any confirmed bugs in the framework, as well as errors or missing sections in the [official documentation](https://focustense.github.io/StardewUI). (If it's something small, like a typo, it's better to ping instead). As with any project, an [SSCCE](https://www.sscce.org/) is strongly preferred when filing bugs.

## For Users

This space is primarily for mod authors and current/future contributors. If you got here because you have an issue with a mod that depends on StardewUI, **please turn around** and head to the [Modded Tech Support](https://discord.com/channels/137344473976799233/1272025932932055121) channel on the [SV Discord](https://discord.com/invite/stardewvalley) instead. The volunteers there can help to collect the necessary log files and determine where the issue actually originates, e.g. in StardewUI itself vs. the mod that is using it vs. an incompatibility between multiple mods.