![Logo](images/logo-text.png)

A convenient, fast and scalable UI framework for [Stardew Valley](https://www.stardewvalley.net/) mods, dedicated to making UI development a breeze rather than a chore. Inspired in part by [Angular](https://angular.dev/) and [XAML](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/xaml/?view=netdesktop-8.0).

* [:seedling: StardewUI Core](Core-Library.md) is an includable code library in the form of a .NET Shared Project, containing the layout and rendering services, fundamental widgets, and integrations with the Stardew Valley base game, in particular its Menu system.

* [:deciduous_tree: StardewUI Framework](UI-Framework.md) is a standalone mod that hosts its own version of the Core library and provides a [Model-View-Whatever](https://www.beyondjava.net/model-view-whatever) (MVW) binding system using [StarML](StarML.md) as well as many other quality-of-life features not found in the Core.

## Features

<div class="grid cards" markdown>

-   :material-view-quilt:{ .lg .middle } __Dynamic layout__

    ---

    Don't fuss with pixel positions; design with flows, grids and other layouts that adapt to your content.

    [:octicons-arrow-right-24: Built-in layouts](Standard-Views.md#layouts)

-   :material-speedometer:{ .lg .middle } __High performance__

    ---

    Retained-mode UI only updates the things that change, when they change. Your UI will never stutter, even on potatoes.

    [:octicons-arrow-right-24: Details and benchmarks](Performance.md)

-   :video_game:{ .lg .middle } __Controller ready__

    ---

    No fussy neighbor lists, clunky clickable components or other boilerplate. Mouse or gamepad,  _it just works_.

    [:octicons-arrow-right-24: Focus and interaction](Focus-and-Interaction.md)


-   :material-table-refresh:{ .lg .middle } __Model-View-Whatever__

    ---

    Keep your views and data separate, using an enhanced HTML-like markup with data binding and hot reload.

    [:octicons-arrow-right-24: StarML guide](StarML.md)

-   :material-form-dropdown:{ .lg .middle } __Don't reinvent the wheel__

    ---

    Pre-made widgets cover everything from simple text and images to drop-down lists, sliders, input boxes and scrollbars.

    [:octicons-arrow-right-24: Standard views](Standard-Views.md#widgets)


-   :octicons-cpu-24:{ .lg .middle } __Made for modding__

    ---

    Designed for [SMAPI](https://stardewvalleywiki.com/Modding:Modder_Guide/APIs/Integrations#Using_an_API). StarML documents are normal game assets, and can be edited or replaced without Harmony patching.

    [:octicons-arrow-right-24: All about assets](API.md#register-assets)

</div>

## Quick Start

In this introductory example, we'll be using the [Framework](UI-Framework.md), which is recommended if you're new to StardewUI. You'll need to be familiar with [basic C# modding](https://stardewvalleywiki.com/Modding:Modder_Guide/Get_Started).

!!! tip "Try it out"

    Want to see the complete working example in game? The full source can be found in the [test mod](https://github.com/focustense/StardewUI/blob/dev/TestMod/ModEntry.cs). Install it along with StardewUI and press <kbd>F9</kbd> after loading a save.

=== "ModEntry.cs"

    ```cs
    internal sealed class ModEntry : Mod
    {
        private IViewEngine viewEngine;

        public override void Entry(IModHelper helper)
        {
            helper.Events.GameLoop.GameLaunched += GameLoop_GameLaunched;
            helper.Events.Input.ButtonPressed += Input_ButtonPressed;
        }

        private void GameLoop_GameLaunched(object? sender, GameLaunchedEventArgs e)
        {
            viewEngine = Helper.ModRegistry.GetApi<IViewEngine>("focustense.StardewUI");
            viewEngine.RegisterViews("Mods/TestMod/Views", "assets/views");
            viewEngine.EnableHotReloading();
        }
        
        private void Input_ButtonPressed(object? sender, ButtonPressedEventArgs e)
        {
            if (Context.IsPlayerFree && e.Button == SButton.F8)
            {
                var context = MenuData.Edibles();
                Game1.activeClickableMenu = viewEngine.CreateMenuFromAsset(
                    "Mods/TestMod/Views/ScrollingItemGrid",
                    context);
            }
        }
    }
    ```

=== "MenuData.cs"

    ```cs    
    public class MenuData
    {
        public string HeaderText { get; set; } = "";
        public List<ParsedItemData> Items { get; set; } = [];
        
        public static MenuData Edibles()
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
                .ToList();
            return new()
            {
                HeaderText = "All Edibles",
                Items = items,
            };
        }
    }
    ```

=== "ScrollingItemGrid.sml"

    ```html
    <lane orientation="vertical"
          horizontal-content-alignment="middle">
        <banner background={@Mods/StardewUI/Sprites/BannerBackground}
                background-border-thickness="48,0"
                padding="12"
                text={HeaderText} />
        <frame layout="880px 640px"
               background={@Mods/StardewUI/Sprites/ControlBorder}
               margin="0,16,0,0"
               padding="32,24">
            <scrollableview>
                <grid layout="stretch content"
                      item-layout="length: 64"
                      item-spacing="16,16"
                      horizontal-item-alignment="middle">
                    <image layout="stretch content"
                           *repeat={Items}
                           sprite={this}
                           focusable="true" />
                </grid>
            </scrollableview>
        </frame>
    </lane>
    ```

The result:

![Video of scrolling item grid](images/example1.webp)

## Next Steps

To get started on your own UI, you can either browse the [Examples](Examples.md) or follow through the recommended reading order below:

1. [StarML](StarML.md): Features, rules, and a handy syntax reference.
2. [Concepts](Concepts.md): Semi-technical shallow dive into the framework's underlying concepts and design. Learn what "views" and "bindings" are, and get the necessary background for troubleshooting bugs or performance problems.
3. [Binding Context](Binding-Context.md): How to design a good data model for powering StarML views and menus, and make your UI responsive to changes in game/user state.
4. [Events](Binding-Events.md): Eventually you'll probably want to make your UI _do something_, like interact with the game world or update some of your mod's state or setting. Event bindings are powerful enough to deserve their own page.
5. [Includes](Included-Views.md): As UIs grow in complexity, you'll often find yourself wanting to create reusable components to be used in many different menus, HUDs, etc. This tutorial goes over the process and potential pitfalls.
6. [The Core Library](Core-Library.md): How and when to use it; both the "safe" and "unsafe" ways.
7. [Custom Views](Custom-Views.md): If the Framework doesn't have what you need, you can create your own, whether it's something simple like the `Banner` to an entirely new kind of layout.
8. [Framework Extensibility](Framework-Extensibility.md): Adding custom tags, conversions, attributes and more; most useful to those who have created one or more custom widgets and want to make them available in StarML.

## Happy Modding!