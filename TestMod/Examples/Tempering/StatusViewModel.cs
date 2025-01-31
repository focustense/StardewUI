using System.Collections.ObjectModel;
using System.ComponentModel;
using PropertyChanged.SourceGenerator;

namespace StardewUITest.Examples.Tempering;

internal partial class StatusViewModel(string name) : INotifyPropertyChanged
{
    public static Transition OffTransition => new(TransitionTime);
    public static Transition OnTransition => new(TransitionTime, TransitionTime * 1.25);

    private static TimeSpan TransitionTime => TimeSpan.FromMilliseconds(120);

    public ObservableCollection<MaterialViewModel> CombinedMaterials { get; } = [];
    public string Name { get; } = name;

    [DependsOn(nameof(CombinedMaterials))]
    public bool IsEmpty => CombinedMaterials.Count == 0;

    [Notify]
    private float opacity = 0;

    [Notify]
    private Transition transition = new(TimeSpan.FromMilliseconds(500), TimeSpan.FromMilliseconds(500));

    public void SetMaterials(IEnumerable<MaterialViewModel> combinedMaterials)
    {
        var materialsById = combinedMaterials.ToDictionary(m => m.Item.QualifiedItemId);
        for (int i = CombinedMaterials.Count - 1; i >= 0; i--)
        {
            var existingMaterial = CombinedMaterials[i];
            if (materialsById.TryGetValue(existingMaterial.Item.QualifiedItemId, out var updatedMaterial))
            {
                existingMaterial.Quantity = updatedMaterial.Quantity;
                existingMaterial.EffectOnSelectedTool = updatedMaterial.EffectOnSelectedTool;
            }
            else
            {
                CombinedMaterials.RemoveAt(i);
            }
            materialsById.Remove(existingMaterial.Item.QualifiedItemId);
        }
        foreach (var remainingMaterial in materialsById.Values)
        {
            CombinedMaterials.Add(
                new(remainingMaterial.Item.QualifiedItemId, remainingMaterial.Quantity)
                {
                    EffectOnSelectedTool = remainingMaterial.EffectOnSelectedTool,
                    WasQuantityChanged = remainingMaterial.WasQuantityChanged,
                }
            );
        }
        OnPropertyChanged(new(nameof(CombinedMaterials)));
    }
}
