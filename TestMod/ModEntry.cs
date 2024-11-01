using System.ComponentModel;
using PropertyChanged.SourceGenerator;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewUI;
using StardewUI.Framework;
using StardewUITest.Examples;
using StardewValley;

namespace StardewUITest;

internal sealed partial class ModEntry : Mod
{
    // Initialized in Entry
    private ModConfig config = null!;

    // Initialized in GameLaunched
    private string viewAssetPrefix = null!;
    private IViewEngine viewEngine = null!;

    // Mod state
    private BestiaryViewModel? bestiary;
    private CropsGridViewModel? cropsGrid;
    private IViewDrawable? hudWidget;

    public override void Entry(IModHelper helper)
    {
        I18n.Init(helper.Translation);
        UI.Initialize(helper, Monitor);
        config = helper.ReadConfig<ModConfig>();
        viewAssetPrefix = $"Mods/{ModManifest.UniqueID}/Views";

        helper.Events.Display.RenderedHud += Display_RenderedHud;
        helper.Events.GameLoop.GameLaunched += GameLoop_GameLaunched;
        helper.Events.GameLoop.OneSecondUpdateTicked += GameLoop_OneSecondUpdateTicked;
        helper.Events.GameLoop.UpdateTicked += GameLoop_UpdateTicked;
        helper.Events.Input.ButtonPressed += Input_ButtonPressed;
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
        viewEngine.EnableHotReloading();
    }

    private void GameLoop_OneSecondUpdateTicked(object? sender, OneSecondUpdateTickedEventArgs e)
    {
        cropsGrid?.SelectedCrop.NextPhase();
    }

    private void GameLoop_UpdateTicked(object? sender, UpdateTickedEventArgs e)
    {
        bestiary?.Update(Game1.currentGameTime.ElapsedGameTime);
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
                ShowExampleMenu3();
                break;
            case SButton.F9:
                if (e.IsDown(SButton.LeftShift))
                {
                    ShowBestiary();
                }
                else
                {
                    ShowExampleMenu2();
                }
                break;
        }
    }

    private void ShowBestiary()
    {
        bestiary = BestiaryViewModel.LoadFromGameData();
        Game1.activeClickableMenu = viewEngine.CreateMenuFromAsset($"{viewAssetPrefix}/Example-Bestiary", bestiary);
    }

    private void ShowExampleMenu1()
    {
        var context = new { HeaderText = "Example Menu Title", ItemData = ItemRegistry.GetData("(O)117") };
        Game1.activeClickableMenu = viewEngine.CreateMenuFromAsset($"{viewAssetPrefix}/TestView", context);
    }

    private void ShowExampleMenu2()
    {
        var context = EdiblesViewModel.LoadFromGameData();
        Game1.activeClickableMenu = viewEngine.CreateMenuFromAsset(
            $"{viewAssetPrefix}/Example-ScrollingItemGrid",
            context
        );
    }

    partial class Example3Model : INotifyPropertyChanged
    {
        [Notify]
        private bool enableTurboBoost = true;

        [Notify]
        private float speedMultiplier = 25;
    }

    private void ShowExampleMenu3()
    {
        var context = new Example3Model();
        Game1.activeClickableMenu = viewEngine.CreateMenuFromAsset($"{viewAssetPrefix}/Example-Form", context);
    }

    private void ShowCropsGridExample()
    {
        cropsGrid = new();
        Game1.activeClickableMenu = viewEngine.CreateMenuFromAsset($"{viewAssetPrefix}/Example-CropsGrid", cropsGrid);
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
