using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewUI;
using StardewUI.Framework;
using StardewValley;

namespace StardewUITest;

internal sealed class ModEntry : Mod
{
    // Initialized in Entry
    private ModConfig config = null!;

    // Initialized in GameLaunched
    private string viewAssetPrefix = null!;
    private IViewEngine viewEngine = null!;

    public override void Entry(IModHelper helper)
    {
        I18n.Init(helper.Translation);
        UI.Initialize(helper, Monitor);
        config = helper.ReadConfig<ModConfig>();
        viewAssetPrefix = $"Mods/{ModManifest.UniqueID}/Views";

        helper.Events.GameLoop.GameLaunched += GameLoop_GameLaunched;
        helper.Events.Input.ButtonPressed += Input_ButtonPressed;
    }

    private void GameLoop_GameLaunched(object? sender, GameLaunchedEventArgs e)
    {
        viewEngine = Helper.ModRegistry.GetApi<IViewEngine>("focustense.StardewUI")!;
        viewEngine.RegisterSprites($"Mods/{ModManifest.UniqueID}/Sprites", "assets/sprites");
        viewEngine.RegisterViews(viewAssetPrefix, "assets/views");
        viewEngine.EnableHotReloading();
    }

    private void Input_ButtonPressed(object? sender, ButtonPressedEventArgs e)
    {
        if (!Context.IsPlayerFree)
        {
            return;
        }
        switch (e.Button)
        {
            case SButton.F8:
                ShowExampleMenu1();
                break;
            case SButton.F9:
                ShowExampleMenu2();
                break;
        }
    }

    private void ShowExampleMenu1()
    {
        var context = new
        {
            HeaderText = "Example Menu Title",
            ItemData = ItemRegistry.GetData("(O)117"),
        };
        Game1.activeClickableMenu = viewEngine.CreateMenuFromAsset($"{viewAssetPrefix}/TestView", context);
    }

    private void ShowExampleMenu2()
    {
        int[] edibleCategories = [
                StardewValley.Object.CookingCategory,
                StardewValley.Object.EggCategory,
                StardewValley.Object.FishCategory,
                StardewValley.Object.FruitsCategory,
                StardewValley.Object.meatCategory,
                StardewValley.Object.MilkCategory,
                StardewValley.Object.VegetableCategory,
            ];
        var items = ItemRegistry.ItemTypes
            .Single(type => type.Identifier == ItemRegistry.type_object)
            .GetAllIds()
            .Select(id => ItemRegistry.GetDataOrErrorItem(id))
            .Where(data => edibleCategories.Contains(data.Category))
            .ToList();
        var context = new
        {
            HeaderText = "All Edibles",
            Items = items,
        };
        Game1.activeClickableMenu = viewEngine.CreateMenuFromAsset($"{viewAssetPrefix}/Example-ScrollingItemGrid", context);
    }
}
