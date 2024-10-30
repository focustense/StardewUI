# Example: Bestiary [:material-file-code-outline:](https://github.com/focustense/StardewUI/blob/dev/TestMod/Examples/BestiaryViewModel.cs){ title="C# Source" } [:material-file-star-outline:](https://github.com/focustense/StardewUI/blob/dev/TestMod/assets/views/Example-Bestiary.sml){ title="StarML Source" }

_Contributed by [:material-github:Mushymato](https://github.com/Mushymato) and [:material-github:focustense](https://github.com/focustense)_
{ .example-header }

<div class="grid cards dense" markdown>

- :material-table-cog: Bindings
- :material-sync: Two-Way
- :material-mouse-left-click: Events
- :octicons-stack-16: Repeaters
- :material-tag-hidden: Conditionals
- :material-tab: Tabs
- :material-grid: Grid
- :material-motion: Animation

</div>

Shows information about all monsters in the game, including their first animation, dangerous variants, and combat stats and loot drops in a tabbed view.

=== "Demo"

    <video controls>
      <source src="../../videos/example-bestiary.mp4" type="video/mp4">
    </video>

=== "BestiaryViewModel.cs"

    ```cs
    internal enum BestiaryTab { General, Combat, Loot }
    
    internal partial class BestiaryViewModel : INotifyPropertyChanged
    {
        public IReadOnlyList<MonsterViewModel> AllMonsters { get; set; } = [];
        public IReadOnlyList<BestiaryTabViewModel> AllTabs { get; } =
            Enum.GetValues<BestiaryTab>()
                .Select(tab =>
                    new BestiaryTabViewModel(tab, tab == BestiaryTab.General))
                .ToArray();
        public MonsterViewModel? SelectedMonster =>
            SelectedIndex >= 0 ? AllMonsters[SelectedIndex] : null;
        public string SelectedMonsterName => SelectedMonster?.DisplayName ?? "";
    
        [Notify] private int selectedIndex;
        [Notify] private BestiaryTab selectedTab;
    
        public void NextMonster()
        {
            SelectedIndex++;
            if (SelectedIndex >= AllMonsters.Count)
            {
                SelectedIndex = 0;
            }
            SelectedMonster?.CurrentAnimation?.Reset();
        }
    
        public void PreviousMonster()
        {
            SelectedIndex--;
            if (SelectedIndex < 0)
            {
                SelectedIndex = AllMonsters.Count - 1;
            }
            SelectedMonster?.CurrentAnimation?.Reset();
        }
    
        public void SelectTab(BestiaryTab tab)
        {
            SelectedTab = tab;
            foreach (var tabViewModel in AllTabs)
            {
                tabViewModel.IsActive = tabViewModel.Value == tab;
            }
        }
    
        public void Update(TimeSpan elapsed)
        {
            SelectedMonster?.CurrentAnimation?.Animate(elapsed);
        }
    }
    
    internal partial class BestiaryTabViewModel(BestiaryTab value, bool active)
        : INotifyPropertyChanged
    {
        public Tuple<int, int, int, int> Margin =>
            IsActive ? new(0, 0, -12, 0) : new(0, 0, 0, 0);
        public BestiaryTab Value { get; } = value;
    
        [Notify] private bool isActive = active;
    }
    ```

=== "MonsterViewModel.cs"

    ```cs
    internal partial class MonsterViewModel : INotifyPropertyChanged
    {
        public string Name { get; set; } = "";
        public string DisplayName { get; set; } = "";
        public MonsterAnimation? DefaultAnimation { get; set; }
        public MonsterAnimation? DangerousAnimation { get; set; }
        public MonsterAnimation? CurrentAnimation =>
            IsDangerousSelected ? DangerousAnimation : DefaultAnimation;
        public bool HasDangerousVariant => DangerousAnimation is not null;
        public int Health { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int Speed { get; set; }
        public int Accuracy { get; set; }
        public int Experience { get; set; }
        public IReadOnlyList<MonsterDrop> Drops { get; set; } = [];
    
        [Notify] private bool isDangerousSelected;
    }
    
    public partial record MonsterAnimation(Texture2D Texture, int Width, int Height)
        : INotifyPropertyChanged
    {
        public string Layout => $"{Width * 4}px {Height * 4}px";
        public Tuple<Texture2D, Rectangle> Sprite =>
            Tuple.Create(Texture, new Rectangle(Width * FrameIndex, 0, Width, Height));
    
        private static readonly TimeSpan animationInterval =
            TimeSpan.FromMilliseconds(150);
    
        private TimeSpan animationProgress;
        [Notify] private int frameIndex;
    
        public void Animate(TimeSpan elapsed)
        {
            animationProgress += elapsed;
            if (animationProgress >= animationInterval)
            {
                FrameIndex = (FrameIndex + 1) % 4;
                animationProgress = TimeSpan.Zero;
            }
        }
    
        public void Reset()
        {
            FrameIndex = 0;
            animationProgress = TimeSpan.Zero;
        }
    }
    
    internal record MonsterDrop(ParsedItemData Item, float Chance)
    {
        public Color ChanceColor =>
            Chance switch
            {
                < 0.1f => Color.DarkRed,
                <= 0.25f => Game1.textColor,
                <= 0.5f => Color.Blue,
                _ => Color.Green,
            };
        public string FormattedChance = string.Format("{0:0.0%}", Chance);
        public string ItemDisplayName => Item.DisplayName;
    }
    ```

=== "Bestiary.sml"

    ```html
    <lane orientation="vertical" horizontal-content-alignment="middle">
        <lane vertical-content-alignment="middle">
            <image layout="48px 48px"
                   horizontal-alignment="middle"
                   vertical-alignment="middle"
                   sprite={@Mods/StardewUI/Sprites/SmallLeftArrow}
                   focusable="true"
                   click=|PreviousMonster()| />
            <banner layout="350px content"
                    margin="16, 0"
                    background={@Mods/StardewUI/Sprites/BannerBackground}
                    background-border-thickness="48, 0"
                    padding="12"
                    text={SelectedMonsterName} />
            <image layout="48px 48px"
                   horizontal-alignment="middle"
                   vertical-alignment="middle"
                   sprite={@Mods/StardewUI/Sprites/SmallRightArrow}
                   focusable="true"
                   click=|NextMonster()| />
        </lane>
        <lane>
            <lane layout="150px content"
                  margin="0, 16, 0, 0"
                  orientation="vertical"
                  horizontal-content-alignment="end"
                  z-index="2">
                <frame *repeat={AllTabs}
                       layout="120px 64px"
                       margin={Margin}
                       padding="16, 0"
                       horizontal-content-alignment="middle"
                       vertical-content-alignment="middle"
                       background={@Mods/focustense.StardewUITest/Sprites/MenuTiles:TabButton}
                       focusable="true"
                       click=|^SelectTab(Value)|>
                    <label text={Value} />
                </frame>
            </lane>
            <frame *switch={SelectedTab}
                   layout="400px 300px"
                   margin="0, 16, 0, 0"
                   padding="32, 24"
                   background={@Mods/StardewUI/Sprites/ControlBorder}>
                <lane *case="General"
                      *context={SelectedMonster}
                      layout="stretch content"
                      orientation="vertical"
                      horizontal-content-alignment="middle">
                    <panel *context={CurrentAnimation}
                           layout="stretch 128px"
                           margin="0, 0, 0, 12"
                           horizontal-content-alignment="middle"
                           vertical-content-alignment="middle">
                        <image layout={Layout} sprite={Sprite} />
                    </panel>
                    <label margin="0, 8" color="#136" text={Name} />
                    <lane *if={HasDangerousVariant}
                          margin="0, 16"
                          vertical-content-alignment="middle">
                        <checkbox layout="content 32px"
                                  label-text="Dangerous"
                                  is-checked={<>IsDangerousSelected} />
                    </lane>
                </lane>
                <lane *case="Combat"
                      *context={SelectedMonster}
                      orientation="vertical">
                    <lane margin="0, 0, 0, 6" vertical-content-alignment="middle">
                        <image layout="20px content"
                               sprite={@Mods/focustense.StardewUITest/Sprites/Cursors:HealthIcon} />
                        <label layout="140px content" margin="8, 0" text="Health" />
                        <label color="#136" text={Health} />
                    </lane>
                    <lane margin="0, 6" vertical-content-alignment="middle">
                        <image layout="20px content"
                               sprite={@Mods/focustense.StardewUITest/Sprites/Cursors:AttackIcon} />
                        <label layout="140px content" margin="8, 0" text="Attack" />
                        <label color="#136" text={Attack} />
                    </lane>
                    <lane margin="0, 6" vertical-content-alignment="middle">
                        <image layout="20px content"
                               sprite={@Mods/focustense.StardewUITest/Sprites/Cursors:DefenseIcon} />
                        <label layout="140px content" margin="8, 0" text="Defense" />
                        <label color="#136" text={Defense} />
                    </lane>
                    <lane margin="0, 6" vertical-content-alignment="middle">
                        <image layout="20px content"
                               sprite={@Mods/focustense.StardewUITest/Sprites/Cursors:SpeedIcon} />
                        <label layout="140px content" margin="8, 0" text="Speed" />
                        <label color="#136" text={Speed} />
                    </lane>
                    <lane margin="0, 6" vertical-content-alignment="middle">
                        <image layout="20px content"
                               sprite={@Mods/focustense.StardewUITest/Sprites/Cursors:LuckIcon} />
                        <label layout="140px content" margin="8, 0" text="Hit Chance" />
                        <label color="#136" text={Accuracy} />
                    </lane>
                </lane>
                <grid *case="Loot"
                      *context={SelectedMonster}
                      layout="stretch"
                      item-layout="count: 5"
                      item-spacing="16, 16"
                      horizontal-item-alignment="middle">
                  <lane *repeat={Drops} orientation="vertical" horizontal-content-alignment="middle">
                      <image layout="64px" margin="0, 0, 0, 4" sprite={Item} />
                      <label color={ChanceColor} scale="0.66" text={FormattedChance} />
                  </lane>
                </grid>
            </frame>
            <spacer layout="50px content" />
        </lane>
    </lane>
    ```