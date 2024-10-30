using System.ComponentModel;
using PropertyChanged.SourceGenerator;
using StardewValley;

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
