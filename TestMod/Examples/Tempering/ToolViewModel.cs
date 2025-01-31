using System.Collections.ObjectModel;
using System.ComponentModel;
using PropertyChanged.SourceGenerator;
using StardewValley;
using StardewValley.ItemTypeDefinitions;
using StardewValley.Tools;

namespace StardewUITest.Examples.Tempering;

internal partial class ToolViewModel(Tool tool) : INotifyPropertyChanged
{
    public static ToolViewModel Create(
        Tool tool,
        string noUpgradePrefix,
        EffectRepository allEffects,
        Dictionary<string, int> initialMaterialQuantities
    )
    {
        var dataName = tool is Slingshot ? "Slingshot" : tool.GetType().Name;
        var displayName = tool.UpgradeLevel > 0 ? tool.DisplayName : noUpgradePrefix + ' ' + tool.DisplayName;
        var materialLimit = (tool.UpgradeLevel + 1) * 20;
        // This is OK to do in a demo; it wouldn't be OK to do in a properly localized mod since (a) hardcoding these
        // prefixes wouldn't be compatible with other material mods like Prismatic Tools, and (b) the next-most obvious
        // step of splitting off the first word of the localized name like "Copper Bar" isn't going to be accurate in
        // every language because the word order might change.
        var (baseMaterialName, baseMaterialItemId) = tool.ItemId switch
        {
            _ when tool.ItemId.StartsWith("Copper") => ("Copper", "(O)334"),
            _ when tool.ItemId.StartsWith("Steel") => ("Steel", "(O)335"),
            _ when tool.ItemId.StartsWith("Gold") => ("Gold", "(O)336"),
            _ when tool.ItemId.StartsWith("Iridium") => ("Iridium", "(O)337"),
            _ => ("Rusty Iron", "(O)111"),
        };
        var initialComposition = initialMaterialQuantities.Select(p => new MaterialViewModel(p.Key, p.Value)
        {
            EffectOnSelectedTool = allEffects.Get(dataName, p.Key, p.Value),
        });
        return new(tool)
        {
            DataName = dataName,
            DisplayName = displayName,
            MaterialLimit = materialLimit,
            BaseMaterialName = baseMaterialName,
            BaseMaterialItem = ItemRegistry.GetDataOrErrorItem(baseMaterialItemId),
            CurrentComposition = [.. initialComposition],
        };
    }

    public ParsedItemData? BaseMaterialItem { get; init; }
    public string BaseMaterialName { get; init; } = "";
    public Tuple<string, string>? BaseMaterialTooltip =>
        Tuple.Create(
            BaseMaterialName,
            $"Base material\n\nTempering capacity: {MaterialLimit}\n"
                + $"Materials used: {FinalMaterialCount}\n"
                + $"Remaining capacity: {RemainingMaterialCount}"
        );
    public bool CanUpgrade => Cost.Count > 0;
    public ObservableCollection<MaterialViewModel> CurrentComposition
    {
        get => currentComposition;
        init
        {
            currentComposition = value;
            finalComposition =
            [
                .. value.Select(m => new MaterialViewModel(m.Item.QualifiedItemId, m.Quantity, enableEchoes: true)
                {
                    EffectOnSelectedTool = m.EffectOnSelectedTool,
                }),
            ];
        }
    }
    public string DataName { get; init; } = "";
    public string DisplayName { get; init; } = "";

    // In a perfect world, PropertyChanged.SourceGenerator would be handling all these dependencies automatically.
    // In reality, it's not, and I don't care enough to chase down the reason why.
    [DependsOn(nameof(CurrentComposition))]
    public int CurrentMaterialCount => CurrentComposition.Sum(c => c.Quantity);

    [DependsOn(nameof(CurrentMaterialCount))]
    public float CurrentMaterialProgress => (float)CurrentMaterialCount / MaterialLimit;

    [DependsOn(nameof(CurrentMaterialProgress))]
    public string CurrentMaterialProgressClipLayout => $"{CurrentMaterialProgress:0%} stretch";

    [DependsOn(nameof(CurrentComposition), nameof(NewMaterials))]
    public IEnumerable<MaterialViewModel> FinalComposition => finalComposition;

    [DependsOn(nameof(FinalComposition))]
    public int FinalMaterialCount => FinalComposition.Sum(m => m.Quantity);

    [DependsOn(nameof(FinalMaterialCount), nameof(MaterialLimit))]
    public float FinalMaterialProgress => (float)FinalMaterialCount / MaterialLimit;

    [DependsOn(nameof(FinalMaterialProgress))]
    public string FinalMaterialProgressClipLayout => $"{FinalMaterialProgress:0%} stretch";
    public ParsedItemData ItemData { get; } = ItemRegistry.GetDataOrErrorItem(tool.QualifiedItemId);
    public int MaterialLimit { get; init; }

    [DependsOn(nameof(NewMaterials))]
    public int NewMaterialCount => NewMaterials.Sum(m => m.Quantity);
    public ObservableCollection<MaterialViewModel> NewMaterials { get; } = [];

    [DependsOn(nameof(CurrentMaterialProgress), nameof(FinalMaterialProgress))]
    public string ProgressText => $"{CurrentMaterialProgress:0%} used ({FinalMaterialProgress:0%} after tempering)";

    [DependsOn(nameof(FinalMaterialCount))]
    public int RemainingMaterialCount => MaterialLimit - FinalMaterialCount;

    public float UpgradeButtonOpacity => CanUpgrade ? 1f : 0.5f;

    [DependsOn(nameof(RemainingMaterialCount))]
    public string UsageLabel => $"{RemainingMaterialCount}/{MaterialLimit}";

    [DependsOn(nameof(CurrentMaterialProgress), nameof(FinalMaterialProgress))]
    public string UsageTooltip => $"{CurrentMaterialProgress:0%} used ({FinalMaterialProgress:0%} after tempering";

    [Notify(Setter.Private)]
    private IReadOnlyList<UpgradeCostViewModel> cost = [];

    [Notify]
    private bool isSelected;

    private readonly ObservableCollection<MaterialViewModel> finalComposition = [];

    private ObservableCollection<MaterialViewModel> currentComposition = [];

    public void AddMaterial(MaterialViewModel material)
    {
        if (material.Quantity == 0 || material.EffectOnSelectedTool is null || FinalMaterialCount >= MaterialLimit)
        {
            Game1.playSound("cowboy_monsterhit");
            return;
        }
        Game1.playSound("clank");
        material.Quantity--;
        AddMaterial(NewMaterials, material);
        OnPropertyChanged(new(nameof(NewMaterials)));
        AddMaterial(finalComposition, material, enableEchoes: true);
        OnPropertyChanged(new(nameof(FinalComposition)));
        Cost = GetCost().ToList();
    }

    private static void AddMaterial(
        ICollection<MaterialViewModel> collection,
        MaterialViewModel material,
        bool enableEchoes = false
    )
    {
        var existingMaterial = collection.FirstOrDefault(m => m.Item.QualifiedItemId == material.Item.QualifiedItemId);
        if (existingMaterial is null)
        {
            existingMaterial = new(material.Item.QualifiedItemId, 0, enableEchoes: enableEchoes)
            {
                EffectOnSelectedTool = material.EffectOnSelectedTool?.WithAdditionalQuantity(0), // Clone,
            };
            existingMaterial.Quantity++;
            collection.Add(existingMaterial);
        }
        else
        {
            existingMaterial.Quantity++;
            existingMaterial.EffectOnSelectedTool = existingMaterial.EffectOnSelectedTool?.WithAdditionalQuantity(1);
        }
    }

    private IEnumerable<UpgradeCostViewModel> GetCost()
    {
        if (NewMaterials.Count == 0)
        {
            yield break;
        }
        yield return new(ItemRegistry.GetData("(O)74"), 1);
        yield return new(
            ItemRegistry.GetData("(O)382"),
            (int)MathF.Ceiling((float)NewMaterials.Sum(m => m.Quantity) / 4)
        );
        foreach (var material in NewMaterials)
        {
            yield return new(material.Item, material.Quantity);
        }
    }
}

internal partial record UpgradeCostViewModel(ParsedItemData Item, int Quantity);
