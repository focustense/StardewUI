using System.ComponentModel;
using Microsoft.Xna.Framework;
using PropertyChanged.SourceGenerator;
using StardewUITest.Examples.Tempering;
using StardewValley;
using StardewValley.ItemTypeDefinitions;

namespace StardewUITest.Examples;

internal partial class MaterialViewModel(string itemId, int initialQuantity, bool enableEchoes = false)
    : INotifyPropertyChanged
{
    private static readonly Color ChangedColor = new(0x00, 0x22, 0x88);
    private static readonly string NoEffectDescription = "No effect on this tool.";
    private static readonly Color UnusableColor = new(0xff, 0x99, 0x99);
    private static readonly Color UsableColor = new(0xcc, 0xff, 0xaa);

    public Color BackgroundColor => EffectOnSelectedTool is not null ? UsableColor : UnusableColor;
    public string BaseDisplayText => $"{EffectOnSelectedTool?.BaseMagnitude:0.0%} {EffectOnSelectedTool?.Name}";
    public string DisplayText => $"{EffectOnSelectedTool?.Magnitude:0.0%} {EffectOnSelectedTool?.Name}";
    public TimedListViewModel<MaterialEchoViewModel>? Echoes { get; } =
        enableEchoes ? new(TimeSpan.FromMilliseconds(500)) : null;
    public Tuple<string, string> EffectTooltip =>
        Tuple.Create(
            Item.DisplayName,
            EffectOnSelectedTool is not null
                ? $"{BaseDisplayText}\n\n{EffectOnSelectedTool.Description}."
                : NoEffectDescription
        );
    public ParsedItemData Item { get; } = ItemRegistry.GetDataOrErrorItem(itemId);
    public float SlotOpacity => EffectOnSelectedTool is not null && Quantity > 0 ? 1 : 0.5f;
    public Color TextColor => WasQuantityChanged ? ChangedColor : Game1.textColor;

    [Notify]
    private bool wasQuantityChanged;

    public void Update(TimeSpan elapsed)
    {
        Echoes?.Update(elapsed);
    }

    [Notify]
    private MaterialEffectViewModel? effectOnSelectedTool;

    [Notify]
    private int quantity = initialQuantity;

    private void OnQuantityChanged(int oldValue, int newValue)
    {
        WasQuantityChanged = true;
        if (Echoes is null)
        {
            return;
        }
        for (int i = oldValue; i < newValue; i++)
        {
            Echoes.Add(new(Item));
        }
    }
}

internal record MaterialEchoViewModel(ParsedItemData Item);

internal partial class MaterialEffectViewModel(
    string id,
    string name,
    string description,
    float baseMagnitude,
    int initialQuantity = 0
) : INotifyPropertyChanged
{
    public string Id { get; } = id;
    public string Name { get; } = name;
    public string Description { get; } = description;
    public float BaseMagnitude { get; } = baseMagnitude;
    public float Magnitude => BaseMagnitude * Quantity;

    [Notify]
    private int quantity = initialQuantity;

    public MaterialEffectViewModel WithAdditionalQuantity(int quantity)
    {
        return new(Id, Name, Description, BaseMagnitude, Quantity + quantity);
    }
}
