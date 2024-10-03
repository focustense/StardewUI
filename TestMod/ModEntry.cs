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
        viewAssetPrefix = $"Mods/{ModManifest.UniqueID}/Views/";

        helper.Events.Content.AssetRequested += Content_AssetRequested;
        helper.Events.GameLoop.GameLaunched += GameLoop_GameLaunched;
        helper.Events.Input.ButtonPressed += Input_ButtonPressed;
    }

    private void Content_AssetRequested(object? sender, AssetRequestedEventArgs e)
    {
        if (e.NameWithoutLocale.StartsWith(viewAssetPrefix))
        {
            var relativePath = e.NameWithoutLocale.Name[viewAssetPrefix.Length..];
            e.LoadFromModFile<object>($"views/{relativePath}.sml", AssetLoadPriority.Exclusive);
        }
    }

    private void GameLoop_GameLaunched(object? sender, GameLaunchedEventArgs e)
    {
        viewEngine = Helper.ModRegistry.GetApi<IViewEngine>("focustense.StardewUI")!;
    }

    private void Input_ButtonPressed(object? sender, ButtonPressedEventArgs e)
    {
        if (Context.IsPlayerFree && e.Button == SButton.F8)
        {
            // Game1.activeClickableMenu = new TestMenu();

            var context = new
            {
                HeaderText = "Example Menu Title",
                ItemData = ItemRegistry.GetData("(O)117"),
            };
            Game1.activeClickableMenu = viewEngine.CreateMenuFromAsset($"{viewAssetPrefix}TestView", context);
        }
    }
}
