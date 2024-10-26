# UI Framework

The recommended, and fastest way to [start building UI](../getting-started/index.md) with StardewUI is via the Framework API. This is a standalone mod, which on mod sites is simply named [StardewUI](https://www.nexusmods.com/stardewvalley/mods/28870).

## Using the Framework

If you're new to the framework, **please check out the [Getting Started guide](../getting-started/index.md)**. It includes everything needed to spawn a menu or HUD using just a few lines of code along with a [StarML](starml.md) file.

Summary of the steps in that guide:

1. [Import the API](../getting-started/index.md#adding-the-api)
2. [Register asset paths](../getting-started/adding-ui-assets.md)
3. [Create a view](starml.md) (some [examples](../examples/index.md) are provided)
4. Make sure your view is [deployed with your project](../getting-started/index.md#next-steps)
5. [Display the menu or HUD](../getting-started/displaying-ui.md)

The above steps are intentionally very specific; aside from creating the view, each "step" is usually only 1-2 clicks or lines of code.

## Comparison with Content Packs

The "framework" label is an analogy to other UI frameworks such as Angular and WPF, and should not be confused with ["Content Pack" frameworks](https://stardewvalleywiki.com/Modding:Content_pack_frameworks) providing add-ons for completely codeless mods. While StarML and Spritesheet JSON are definitely "content" for StardewUI, the crucial element that cannot be fully specified in a content pack is the _model_ – what the UI does, as opposed to how it looks.

A menu or other UI normally exists to provide some _function_; it's a way to navigate the features of your mod, which are up to you. It would be impractical to try to anticipate every possible reason for a UI to exist, which is an almost infinite space: mod configuration, inventory, shops, NPC interactions, quests, minigames and more. StardewUI requires very little code to use, but it is still a C# framework.

If you're interested in making, contributing to, or helping to define a more limited, content-pack friendly version, possibly based on [Game State Queries](https://stardewvalleywiki.com/Modding:Game_state_queries), [Trigger Actions](https://stardewvalleywiki.com/Modding:Trigger_actions) and similar features, please consider either creating your own [extension mod](extensions.md) or starting/participating in a [discussion on GitHub](https://github.com/focustense/StardewUI/discussions).