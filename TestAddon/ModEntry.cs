using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewUI;
using StardewUI.Framework;
using StardewValley;

namespace StardewUITestAddon;

internal class ModEntry : Mod
{
    private static readonly KeybindList menuHotkey1 = new(
        new Keybind(SButton.LeftControl, SButton.LeftShift, SButton.Divide)
    );
    private static readonly KeybindList menuHotkey2 = new(
        new Keybind(SButton.LeftControl, SButton.LeftShift, SButton.Multiply)
    );

    // Initialized in GameLaunched
    private IGalleryApi? galleryApi;
    private IViewEngine viewEngine = null!;

    public override void Entry(IModHelper helper)
    {
        I18n.Init(helper.Translation);
        UI.RegisterAddon(new ExampleAddon(ModManifest.UniqueID));

        helper.Events.GameLoop.GameLaunched += GameLoop_GameLaunched;
        helper.Events.Input.ButtonsChanged += Input_ButtonsChanged;
    }

    private IMenuController CreateCarousel()
    {
        var materialItems = PurchasableItem.LoadAll(
            ["(O)330", "(O)382", "(O)380", "(O)388", "(O)390", "(O)709"],
            0.25f
        );
        var artifactItems = PurchasableItem.LoadAll(["(O)113", "(O)585", "(O)109", "(O)126"], 0.6f);
        var foodItems = PurchasableItem.LoadAll(["(O)239", "(O)730", "(O)214", "(O)207", "(O)218"], 0.12f);
        return viewEngine.CreateMenuControllerFromAsset(
            $"Mods/{ModManifest.UniqueID}/Views/CarouselDemo",
            new CarouselMenuViewModel()
            {
                Pages =
                [
                    new("Materials", TrainCarType.Freight, TrainCovering.Minerals, materialItems),
                    new("Artifacts", TrainCarType.Freight, TrainCovering.Boxes, artifactItems),
                    new("Dining", TrainCarType.Passenger, items: foodItems),
                ],
            }
        );
    }

    private void GameLoop_GameLaunched(object? sender, GameLaunchedEventArgs e)
    {
        viewEngine = Helper.ModRegistry.GetApi<IViewEngine>("focustense.StardewUI")!;
        viewEngine.RegisterSprites($"Mods/{ModManifest.UniqueID}/Sprites", "assets/sprites");
        viewEngine.RegisterViews($"Mods/{ModManifest.UniqueID}/Views", "assets/views");
        viewEngine.EnableHotReloading();

        galleryApi = Helper.ModRegistry.GetApi<IGalleryApi>("focustense.StardewUITest");
        galleryApi?.RegisterExample(
            I18n.Gallery_Example_Carousel_Title,
            I18n.Gallery_Example_Carousel_Description,
            "(O)108",
            CreateCarousel
        );
    }

    private void Input_ButtonsChanged(object? sender, ButtonsChangedEventArgs e)
    {
        if (!Context.IsPlayerFree || Game1.activeClickableMenu is not null)
        {
            return;
        }
        if (menuHotkey1.JustPressed())
        {
            Game1.activeClickableMenu = viewEngine.CreateMenuFromAsset(
                $"Mods/{ModManifest.UniqueID}/Views/FramedSprite",
                new ItemViewModel()
            );
        }
        else if (menuHotkey2.JustPressed())
        {
            Game1.activeClickableMenu = CreateCarousel().Menu;
        }
    }
}
