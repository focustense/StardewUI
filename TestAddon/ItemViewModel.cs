using System.ComponentModel;
using PropertyChanged.SourceGenerator;
using StardewValley;

namespace StardewUITestAddon;

internal partial class ItemViewModel : INotifyPropertyChanged
{
    public IReadOnlyList<string> AllIds { get; } = ItemRegistry.GetObjectTypeDefinition().GetAllIds().ToList();
    public string SelectedItemId => AllIds[ItemIndex];

    [Notify] private int itemIndex;

    public void NextItem()
    {
        ItemIndex = (ItemIndex + 1) % AllIds.Count;
    }

    public void PreviousItem()
    {
        ItemIndex = ItemIndex == 0 ? AllIds.Count - 1 : ItemIndex - 1;
    }
}
