using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewUI.Framework;
using StardewUITest.Examples;
using StardewUITest.Examples.Tempering;
using StardewValley;

namespace StardewUITest;

internal sealed partial class ModEntry : Mod
{
    private readonly GalleryApi api = new();

    // Initialized in Entry
    private ModConfig config = null!;

    // Initialized in GameLaunched
    private string viewAssetPrefix = null!;
    private IViewEngine viewEngine = null!;

    // Mod data, also initialized in GameLaunched
    private Dictionary<string, Dictionary<string, EffectData>> temperingData = null!;

    // Mod state
    private CropsGridViewModel? cropsGrid;
    private IViewDrawable? hudWidget;

    public override void Entry(IModHelper helper)
    {
        I18n.Init(helper.Translation);
        config = helper.ReadConfig<ModConfig>();
        viewAssetPrefix = $"Mods/{ModManifest.UniqueID}/Views";

        helper.Events.Display.RenderedHud += Display_RenderedHud;
        helper.Events.GameLoop.GameLaunched += GameLoop_GameLaunched;
        helper.Events.GameLoop.OneSecondUpdateTicked += GameLoop_OneSecondUpdateTicked;
        helper.Events.Input.ButtonPressed += Input_ButtonPressed;
    }

    public override object? GetApi()
    {
        return api;
    }

    private void Display_RenderedHud(object? sender, RenderedHudEventArgs e)
    {
        hudWidget?.Draw(e.SpriteBatch, new(0, 100));
    }

    private void GameLoop_GameLaunched(object? sender, GameLaunchedEventArgs e)
    {
        viewEngine = Helper.ModRegistry.GetApi<IViewEngine>("focustense.StardewUI")!;
        viewEngine.RegisterSprites($"Mods/{ModManifest.UniqueID}/Sprites", "assets/sprites");
        viewEngine.RegisterViews(viewAssetPrefix, "assets/views");
        viewEngine.EnableHotReloadingWithSourceSync();

        temperingData = Helper.ModContent.Load<Dictionary<string, Dictionary<string, EffectData>>>(
            "assets/data/tempering.json"
        );
    }

    private void GameLoop_OneSecondUpdateTicked(object? sender, OneSecondUpdateTickedEventArgs e)
    {
        cropsGrid?.SelectedCrop.NextPhase();
    }

    private void Input_ButtonPressed(object? sender, ButtonPressedEventArgs e)
    {
        if (!Context.IsPlayerFree)
        {
            return;
        }
        switch (e.Button)
        {
            case SButton.Multiply:
                ToggleHud();
                break;
            case SButton.F8:
                if (e.IsDown(SButton.LeftControl))
                {
                    ShowTabsExample();
                }
                else
                {
                    ShowGallery();
                }
                break;
        }
    }

    private void OpenExample(string assetName, object? context)
    {
        viewEngine.CreateMenuControllerFromAsset(assetName, context).Launch();
    }

    private void ShowTempering()
    {
        var context = new TemperingViewModel(temperingData);
        OpenExample($"{viewAssetPrefix}/Example-Tempering", context);
    }

    private void ShowBestiary()
    {
        var bestiary = BestiaryViewModel.LoadFromGameData();
        OpenExample($"{viewAssetPrefix}/Example-Bestiary", bestiary);
    }

    private void ShowCropsGrid()
    {
        cropsGrid = new();
        OpenExample($"{viewAssetPrefix}/Example-CropsGrid", cropsGrid);
    }

    private void ShowForm()
    {
        var context = new FormViewModel();
        var controller = viewEngine.CreateMenuControllerFromAsset($"{viewAssetPrefix}/Example-Form", context);
        controller.EnableCloseButton();
        controller.CanClose = () => context.AllowClose;
        controller.Closing += () => Monitor.Log("Menu Closing", LogLevel.Info);
        controller.Closed += () => Monitor.Log("Menu Closed", LogLevel.Info);
        if (Helper.Input.IsDown(SButton.RightShift))
        {
            var position = Game1.getMousePosition(true);
            controller.PositionSelector = () => position;
        }
        controller.Launch();
    }

    private void ShowGallery()
    {
        var context = new GalleryViewModel(
            [
                new(
                    I18n.Gallery_Example_ItemGrid_Title(),
                    I18n.Gallery_Example_ItemGrid_Description(),
                    "(BC)232",
                    ShowItemGrid
                ),
                new(
                    I18n.Gallery_Example_Bestiary_Title(),
                    I18n.Gallery_Example_Bestiary_Description(),
                    "(O)Book_Void",
                    ShowBestiary
                ),
                new(
                    I18n.Gallery_Example_Tempering_Title(),
                    I18n.Gallery_Example_Tempering_Description(),
                    "(O)749",
                    ShowTempering
                ),
                new(
                    I18n.Gallery_Example_CropCalendar_Title(),
                    I18n.Gallery_Example_CropCalendar_Description(),
                    "(O)24",
                    ShowCropsGrid
                ),
                new(I18n.Gallery_Example_Form_Title(), I18n.Gallery_Example_Form_Description(), "(O)867", ShowForm),
                .. api.Registrations.Select(register => register()),
            ]
        );
        Game1.activeClickableMenu = viewEngine.CreateMenuFromAsset($"{viewAssetPrefix}/Gallery", context);
    }

    private void ShowItemGrid()
    {
        var context = EdiblesViewModel.LoadFromGameData();
        OpenExample($"{viewAssetPrefix}/Example-ScrollingItemGrid", context);
    }

    private void ShowTabsExample()
    {
        var context = new TabsViewModel()
        {
            Tabs =
            [
                new("Home", Game1.mouseCursors, new(20, 388, 8, 8)) { Active = true },
                new("Social", Game1.mouseCursors, new(36, 374, 7, 8)),
                new("Money", Game1.mouseCursors, new(4, 388, 8, 8)),
                new("Seasons", Game1.mouseCursors, new(420, 1204, 8, 8)),
            ],
        };
        Game1.activeClickableMenu = viewEngine.CreateMenuFromAsset($"{viewAssetPrefix}/Example-Tabs", context);
    }

    private void ToggleHud()
    {
        if (hudWidget is not null)
        {
            hudWidget.Dispose();
            hudWidget = null;
        }
        else
        {
            hudWidget = viewEngine.CreateDrawableFromAsset($"{viewAssetPrefix}/Example-Hud");
            hudWidget.Context = new { Title = "I'm a HUD!" };
        }
    }
}
