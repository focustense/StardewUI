using PropertyChanged.SourceGenerator;
using StardewModdingAPI;
using StardewValley;
using StardewValley.ItemTypeDefinitions;

namespace StardewUITestAddon;

internal partial class CarouselMenuViewModel
{
    public int HeaderIndex => SelectedPageIndex + 1;
    public List<CarouselMenuPage> Pages { get; set; } = [];

    [Notify] private int selectedPageIndex;

    public bool HandleButtonPress(SButton button)
    {
        switch (button)
        {
            case SButton.RightShoulder:
                if (SelectedPageIndex < (Pages.Count - 1))
                {
                    SelectedPageIndex++;
                    return true;
                }
                break;
            case SButton.LeftShoulder:
                if (SelectedPageIndex > 0)
                {
                    SelectedPageIndex--;
                    return true;
                }
                break;
        }
        return false;
    }
}

internal class CarouselMenuPage(string title = "", TrainCarType carType = default, TrainCovering covering = default, IReadOnlyList<PurchasableItem>? items = null)
{
    public string Title { get; } = title;

    public TrainCarType CarType { get; } = carType;

    public TrainCovering Covering { get; } = covering;

    public IReadOnlyList<PurchasableItem> Items { get; set; } = items ?? [];
}

internal enum TrainCarType { Passenger, Freight }

internal enum TrainCovering { None, Minerals, Boxes }

internal class PurchasableItem(ParsedItemData data, int price)
{
    public static PurchasableItem Load(string id, float discount)
    {
        var itemData = ItemRegistry.GetDataOrErrorItem(id);
        var item = ItemRegistry.Create(id);
        return new(itemData, (int)MathF.Round(item.salePrice() * (1 - discount)));
    }

    public static IReadOnlyList<PurchasableItem> LoadAll(IEnumerable<string> ids, float discount)
    {
        return ids.Select(id => Load(id, discount)).ToList();
    }

    public ParsedItemData Data { get; } = data;
    public string DisplayName => Data.DisplayName;
    public int Price { get; } = price;
}