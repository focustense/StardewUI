using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PropertyChanged.SourceGenerator;
using StardewValley;
using StardewValley.GameData.Crops;
using StardewValley.ItemTypeDefinitions;

namespace StardewUITest.Examples;

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

internal partial record CropInfoboxModel(ParsedItemData Seed, CropData Crop) : INotifyPropertyChanged
{
    public string Name => Produce.DisplayName;
    public IReadOnlyList<bool> Harvest { get; } = GetHarvest(Crop).ToList();
    public ParsedItemData Produce { get; } = ItemRegistry.GetDataOrErrorItem(Crop.HarvestItemId);

    private readonly Texture2D cropTexture = Game1.content.Load<Texture2D>(Crop.Texture);

    private int phaseIndex = 0;

    [Notify]
    private Tuple<Texture2D, Rectangle> phase = null!;

    /// <summary>
    /// This model uses SMAPI OneSecondUpdateTicked evemt instead of StardewUI context Update to animate.
    /// <code>
    /// helper.Events.GameLoop.OneSecondUpdateTicked += GameLoop_OneSecondUpdateTicked;
    /// private void GameLoop_OneSecondUpdateTicked(object? sender, OneSecondUpdateTickedEventArgs e)
    /// {
    ///     cropsGrid?.SelectedCrop.NextPhase();
    /// }
    /// </code>
    /// </summary>
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
