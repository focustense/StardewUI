# Example: Crop Calendar [:material-file-code-outline:](https://github.com/focustense/StardewUI/blob/dev/TestMod/Examples/CropsGridViewModel.cs){ title="C# Source" } [:material-file-star-outline:](https://github.com/focustense/StardewUI/blob/dev/TestMod/assets/views/Example-CropsGrid.sml){ title="StarML Source" }

_Contributed by [:material-github:Mushymato](https://github.com/Mushymato), edited by [:material-github:focustense](https://github.com/focustense)_
{ .example-header }

<div class="grid cards dense" markdown>

- :material-table-cog: Bindings
- :material-mouse-left-click: Events
- :octicons-stack-16: Repeaters
- :material-grid: Grid
- :fontawesome-solid-arrows-up-down: Scrolling
- :material-motion: Animation

</div>

Displays harvest timing and an animation of the growth phases for any given crop, selectable from a grid.

=== "Demo"

    <video controls>
      <source src="../../videos/example-crop-calendar.mp4" type="video/mp4">
    </video>

=== "CropsGridViewModel.cs"

    ```cs
    internal partial class CropsGridViewModel : INotifyPropertyChanged
    {
        public IReadOnlyList<CropInfoboxModel> AllCrops { get; }

        [Notify]
        private CropInfoboxModel selectedCrop;

        public CropsGridViewModel()
        {
            AllCrops = Game1
                .objectData.Select(
                    (kv) =>
                        Game1.cropData.TryGetValue(kv.Key, out CropData? crop)
                            ? new CropInfoboxModel(ItemRegistry.GetData(kv.Key), crop)
                            : null
                )
                .Where(cropInfo => cropInfo is not null)
                .ToList()!;
            selectedCrop = AllCrops[0];
        }

        public void SelectCrop(CropInfoboxModel cropInfo)
        {
            cropInfo.ResetPhase();
            SelectedCrop = cropInfo;
        }
    }
    ```

=== "CropInfoboxModel.cs"

    ```cs
    internal partial record CropInfoboxModel(ParsedItemData Seed, CropData Crop)
        : INotifyPropertyChanged
    {
        public string Name => Produce.DisplayName;
        public IReadOnlyList<bool> Harvest { get; } = GetHarvest(Crop).ToList();
        public ParsedItemData Produce { get; } =
            ItemRegistry.GetDataOrErrorItem(Crop.HarvestItemId);

        private readonly Texture2D cropTexture =
            Game1.content.Load<Texture2D>(Crop.Texture);

        private int phaseIndex = 0;

        [Notify]
        private Tuple<Texture2D, Rectangle> phase = null!;

        internal void NextPhase()
        {
            Phase = new Tuple<Texture2D, Rectangle>(
                cropTexture,
                new Rectangle(
                    // odd number sprite index = right side
                    Crop.SpriteIndex
                        % 2
                        * 128
                        + phaseIndex * 16,
                    // zigzag
                    Crop.SpriteIndex
                        / 2
                        * 32,
                    16,
                    32
                )
            );

            if (phaseIndex < 2)
                phaseIndex = 2;
            else if (phaseIndex == Crop.DaysInPhase.Count + 1)
                phaseIndex = Random.Shared.Next(0, 2);
            else
                phaseIndex++;
        }

        internal void ResetPhase()
        {
            phaseIndex = Random.Shared.Next(0, 2);
            NextPhase();
        }

        private static IEnumerable<bool> GetHarvest(CropData crop)
        {
            int growDays = crop.DaysInPhase.Sum();
            int regrowDays = crop.RegrowDays;
            if (regrowDays < 1)
            {
                yield return false;
                for (int day = 1; day < WorldDate.DaysPerMonth; day++)
                    yield return day % growDays == 0;
            }
            else
            {
                yield return false;
                for (int day = 1; day < growDays; day++)
                    yield return false;
                yield return true;
                for (int day = 1; day < WorldDate.DaysPerMonth - growDays; day++)
                    yield return day % regrowDays == 0;
            }
        }
    }
    ```

=== "CropsGrid.sml"

    ```html
    <lane layout="content 700px"
          orientation="horizontal"
          horizontal-content-alignment="start">
        <frame layout="400px stretch"
               background={@Mods/StardewUI/Sprites/ControlBorder}
               padding="32, 20"
               horizontal-content-alignment="middle">
            <lane *context={SelectedCrop}
                  orientation="vertical"
                  horizontal-content-alignment="middle">
                <banner text={Name} />
                <lane orientation="horizontal" >
                    <image sprite={Seed} layout="64px 64px" margin="4"/>
                    <image sprite={Produce} layout="64px 64px" margin="4"/>
                </lane>
                <grid layout="400px content"
                      item-layout="count: 7"
                      item-spacing="8,8"
                      horizontal-item-alignment="middle">
                    <frame *repeat={Harvest}
                           layout="64px 64px"
                           background={@Mods/StardewUI/Sprites/ButtonLight}>
                        <image *if={this}
                               layout="48px 48px"
                               sprite={^Produce}
                               margin="8"/>
                    </frame>
                </grid>
                <image sprite={Phase} layout="64px 128px" margin="0,16,0,0"/>
            </lane>
        </frame>
        <frame layout="600px stretch"
               background={@Mods/StardewUI/Sprites/ControlBorder}
               padding="20">
            <scrollable>
                <grid layout="content content"
                      item-layout="count: 6"
                      item-spacing="8, 8"
                      horizontal-item-alignment="middle">
                    <image *repeat={AllCrops}
                           layout="64px 64px"
                           sprite={Seed}
                           tooltip={Name}
                           focusable="true"
                           click=|^SelectCrop(this)| />
                </grid>
            </scrollable>
        </frame>
    </lane>
    ```