using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PropertyChanged.SourceGenerator;
using StardewValley;
using StardewValley.ItemTypeDefinitions;

namespace StardewUITest.Examples;

internal partial class BestiaryViewModel : INotifyPropertyChanged
{
    public static BestiaryViewModel LoadFromGameData()
    {
        var monsterData = DataLoader.Monsters(Game1.content);
        return new() { AllMonsters = monsterData.Select(x => MonsterViewModel.Build(x.Key, x.Value)).ToArray() };
    }

    public IReadOnlyList<MonsterViewModel> AllMonsters { get; set; } = [];
    public IReadOnlyList<BestiaryTabViewModel> AllTabs { get; } =
        Enum.GetValues<BestiaryTab>()
            .Select(tab => new BestiaryTabViewModel(tab, tab == BestiaryTab.General))
            .ToArray();

    public MonsterViewModel? SelectedMonster => SelectedIndex >= 0 ? AllMonsters[SelectedIndex] : null;
    public string SelectedMonsterName => SelectedMonster?.DisplayName ?? "";

    [Notify]
    private int selectedIndex;

    [Notify]
    private BestiaryTab selectedTab;

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

internal enum BestiaryTab
{
    General,
    Combat,
    Loot,
}

internal partial class BestiaryTabViewModel(BestiaryTab value, bool active) : INotifyPropertyChanged
{
    public Tuple<int, int, int, int> Margin => IsActive ? new(0, 0, -12, 0) : new(0, 0, 0, 0);
    public BestiaryTab Value { get; } = value;

    [Notify]
    private bool isActive = active;
}

internal partial class MonsterViewModel : INotifyPropertyChanged
{
    public static MonsterViewModel Build(string name, string data)
    {
        string[] fields = data.Split('/');
        return new()
        {
            Name = name,
            DisplayName = fields[14],
            Health = int.Parse(fields[0]),
            Attack = int.Parse(fields[1]),
            Defense = int.Parse(fields[7]),
            Speed = int.Parse(fields[10]),
            Accuracy = (int)MathF.Round((1.0f - float.Parse(fields[11])) * 100),
            Experience = int.Parse(fields[13]),
            DefaultAnimation = MonsterAnimation.Load(name, false) ?? MonsterAnimation.Missing,
            DangerousAnimation = MonsterAnimation.Load(name, true),
            Drops = fields[6]
                .Split(' ')
                .Chunk(2)
                .Select(x => new MonsterDrop(ItemRegistry.GetDataOrErrorItem(x[0]), float.Parse(x[1])))
                .OrderByDescending(drop => drop.Chance)
                .ToArray(),
        };
    }

    public string Name { get; set; } = "";
    public string DisplayName { get; set; } = "";
    public MonsterAnimation? DefaultAnimation { get; set; }
    public MonsterAnimation? DangerousAnimation { get; set; }
    public MonsterAnimation? CurrentAnimation => IsDangerousSelected ? DangerousAnimation : DefaultAnimation;
    public bool HasDangerousVariant => DangerousAnimation is not null;
    public int Health { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int Speed { get; set; }
    public int Accuracy { get; set; }
    public int Experience { get; set; }
    public IReadOnlyList<MonsterDrop> Drops { get; set; } = [];

    [Notify]
    private bool isDangerousSelected;
}

public partial record MonsterAnimation(Texture2D Texture, int Width, int Height) : INotifyPropertyChanged
{
    public static readonly MonsterAnimation Missing = new(Game1.staminaRect, 1, 1);

    public string Layout => $"{Width * 4}px {Height * 4}px";

    public Tuple<Texture2D, Rectangle> Sprite =>
        Tuple.Create(Texture, new Rectangle(Width * FrameIndex, 0, Width, Height));

    public static MonsterAnimation? Load(string name, bool dangerous)
    {
        var assetName = $@"Characters\Monsters\{name}";
        if (dangerous)
        {
            assetName += "_dangerous";
        }
        if (!Game1.content.DoesAssetExist<Texture2D>(assetName))
        {
            return null;
        }
        var texture = Game1.content.Load<Texture2D>(assetName);
        var (width, height) = GetSpriteSize(name);
        return new(texture, width, height);
    }

    private static readonly TimeSpan animationInterval = TimeSpan.FromMilliseconds(150);

    [Notify]
    private int frameIndex;

    private TimeSpan animationProgress;

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

    private static (int, int) GetSpriteSize(string name)
    {
        return name switch
        {
            "Bat"
            or "Frost Bat"
            or "Lava Bat"
            or "Iridium Bat"
            or "Ghost"
            or "Carbon Ghost"
            or "Putrid Ghost"
            or "Fly"
            or "Grub"
            or "Rock Crab"
            or "Lava Crab"
            or "Iridium Crab"
            or "Truffle Crab"
            or "Duggy"
            or "Magma Duggy"
            or "Stone Golem"
            or "Wilderness Golem"
            or "Iridium Golem"
            or "False Magma Cap"
            or "Dust Spirit"
            or "Shadow Shaman" => (16, 24),

            "Blue Squid" => (24, 24),

            "Green Slime"
            or "Tiger Slime"
            or "Skeleton"
            or "Skeleton Mage"
            or "Mummy"
            or "Shadow Brute"
            or "Shadow Guy" => (16, 32),

            "Shadow Sniper" or "Big Slime" or "Pepper Rex" or "Spider" or "Serpent" or "Royal Serpent" => (32, 32),

            "Crow" => (64, 64),

            _ => (16, 16),
        };
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
