using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        viewEngine.EnableHotReloadingWithSourceSync();
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
                    ShowExampleMenu3();
                }
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
        var bestiary = BestiaryViewModel.LoadFromGameData();
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

        [Notify]
        private bool allowClose = true;

        [Notify]
        private float opacity = 1f;
    }

    private void ShowExampleMenu3()
    {
        var context = new Example3Model();
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
        Game1.activeClickableMenu = controller.Menu;
    }

    private void ShowCropsGridExample()
    {
        cropsGrid = new();
        Game1.activeClickableMenu = viewEngine.CreateMenuFromAsset($"{viewAssetPrefix}/Example-CropsGrid", cropsGrid);
    }

    partial class TabData(string name, Texture2D texture, Rectangle sourceRect) : INotifyPropertyChanged
    {
        public string Name { get; } = name;
        public Tuple<Texture2D, Rectangle> Sprite { get; } = Tuple.Create(texture, sourceRect);

        [Notify]
        private bool active;
    }

    partial class TabsViewModel
    {
        public IReadOnlyList<TabData> Tabs { get; set; } = [];

        public void OnTabActivated(string name)
        {
            foreach (var tab in Tabs)
            {
                if (tab.Name != name)
                {
                    tab.Active = false;
                }
            }
        }
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
