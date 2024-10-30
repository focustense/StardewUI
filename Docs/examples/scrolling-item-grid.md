# Example: Scrolling Item Grid [:material-file-code-outline:](https://github.com/focustense/StardewUI/blob/dev/TestMod/Examples/EdiblesViewModel.cs){ title="C# Source" } [:material-file-star-outline:](https://github.com/focustense/StardewUI/blob/dev/TestMod/assets/views/Example-ScrollingItemGrid.sml){ title="StarML Source" }

_Contributed by [:material-github:focustense](https://github.com/focustense)_
{ .example-header }

<div class="grid cards dense" markdown>

- :material-table-cog: Bindings
- :octicons-stack-16: Repeaters
- :material-grid: Grid
- :fontawesome-solid-arrows-up-down: Scrolling

</div>

Simple grid showing a large number of items inside a scrollable container. This is the same example as the one on the [home page](../index.md).

=== "Demo"

    <video controls>
      <source src="../../videos/example-itemgrid.mp4" type="video/mp4">
    </video>

=== "EdiblesViewModel.cs"

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

=== "ScrollableItemGrid.sml"

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