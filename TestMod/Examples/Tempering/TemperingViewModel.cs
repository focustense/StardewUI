using System.ComponentModel;
using PropertyChanged.SourceGenerator;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Tools;

namespace StardewUITest.Examples.Tempering;

using TemperingData = Dictionary<string, Dictionary<string, EffectData>>;

internal partial class TemperingViewModel : INotifyPropertyChanged
{
    public IReadOnlyList<MaterialViewModel> AvailableMaterials { get; }
    public IEnumerable<StatusViewModel> Statuses => statusByToolName.Values;
    public IReadOnlyList<ToolViewModel> Tools { get; }

    [Notify]
    private ToolViewModel? selectedTool;

    [Notify]
    private StatusViewModel? status;

    private readonly EffectRepository allEffects;
    private readonly Dictionary<string, StatusViewModel> statusByToolName;

    public TemperingViewModel(TemperingData toolData)
    {
        allEffects = new(toolData);
        Tools =
        [
            ToolViewModel.Create(new Axe(), "Rusty", allEffects, ExampleCompositions.Axe),
            ToolViewModel.Create(new Pickaxe(), "Blunt", allEffects, ExampleCompositions.Pickaxe),
            ToolViewModel.Create(new Hoe() { UpgradeLevel = 1 }, "Bent", allEffects, ExampleCompositions.Hoe),
            ToolViewModel.Create(new WateringCan(), "Cheap", allEffects, []),
            ToolViewModel.Create(new Pan(), "Dented", allEffects, []),
            ToolViewModel.Create(new Slingshot(), "Worn", allEffects, []),
        ];
        foreach (var tool in Tools)
        {
            tool.PropertyChanged += Tool_PropertyChanged;
        }
        statusByToolName = Tools.ToDictionary(tool => tool.DataName, tool => new StatusViewModel(tool.DisplayName));
        var fakeInventoryRandom = new Random(1082794366);
        AvailableMaterials = allEffects
            .MaterialIds.Select(itemId => new MaterialViewModel(itemId, fakeInventoryRandom.Next(20, 120)))
            .ToArray();
        if (Tools.Count > 0)
        {
            SelectedTool = Tools[0];
            ShowToolStatus(SelectedTool);
        }
    }

    public void HandleButtonPress(SButton button)
    {
        if (Tools.Count == 0)
        {
            return;
        }
        switch (button)
        {
            case SButton.LeftShoulder:
                Game1.playSound("smallSelect");
                SelectedTool = Tools[(FindToolIndex(SelectedTool) + Tools.Count - 1) % Tools.Count];
                ShowToolStatus(SelectedTool);
                break;
            case SButton.RightShoulder:
                Game1.playSound("smallSelect");
                SelectedTool = Tools[(FindToolIndex(SelectedTool) + 1) % Tools.Count];
                ShowToolStatus(SelectedTool);
                break;
        }
        ;
    }

    public void SelectTool(ToolViewModel tool)
    {
        if (SelectedTool == tool)
        {
            return;
        }
        Game1.playSound("smallSelect");
        SelectedTool = tool;
    }

    public void ShowToolStatus(ToolViewModel? tool = null)
    {
        foreach (var status in statusByToolName.Values)
        {
            status.Transition = StatusViewModel.OffTransition;
            status.Opacity = 0;
        }
        if (tool is not null)
        {
            var status = statusByToolName[tool.DataName];
            status.SetMaterials(tool.FinalComposition);
            status.Transition = StatusViewModel.OnTransition;
            status.Opacity = 1;
        }
    }

    private int FindToolIndex(ToolViewModel? tool)
    {
        for (int i = 0; i < Tools.Count; i++)
        {
            if (Tools[i] == tool)
            {
                return i;
            }
        }
        return -1;
    }

    private void OnSelectedToolChanged(ToolViewModel? oldValue, ToolViewModel? newValue)
    {
        if (oldValue is not null)
        {
            oldValue.IsSelected = false;
        }
        if (newValue is not null)
        {
            newValue.IsSelected = true;
        }
        UpdateAvailableMaterials();
    }

    private void Tool_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender is ToolViewModel tool && e.PropertyName == nameof(ToolViewModel.FinalComposition))
        {
            statusByToolName[tool.DataName].SetMaterials(tool.FinalComposition);
        }
    }

    private void UpdateAvailableMaterials()
    {
        foreach (var material in AvailableMaterials)
        {
            material.EffectOnSelectedTool = SelectedTool is { } tool
                ? allEffects.Get(tool.DataName, material.Item.QualifiedItemId)
                : null;
        }
    }
}
