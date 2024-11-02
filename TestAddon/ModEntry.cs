using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewUI;
using StardewUI.Framework;
using StardewValley;

namespace StardewUITestAddon;

internal class ModEntry : Mod
{
    private static readonly KeybindList menuHotkey = new(new Keybind(SButton.LeftControl, SButton.LeftShift, SButton.Divide));

    // Initialized in GameLaunched
    private IViewEngine viewEngine = null!;

    public override void Entry(IModHelper helper)
    {
        UI.RegisterAddon(new ExampleAddon(ModManifest.UniqueID));

        helper.Events.GameLoop.GameLaunched += GameLoop_GameLaunched;
        helper.Events.Input.ButtonsChanged += Input_ButtonsChanged;
    }

    private void GameLoop_GameLaunched(object? sender, GameLaunchedEventArgs e)
    {
        viewEngine = Helper.ModRegistry.GetApi<IViewEngine>("focustense.StardewUI")!;
        viewEngine.RegisterViews($"Mods/{ModManifest.UniqueID}/Views", "assets/views");
        viewEngine.EnableHotReloading();
    }

    private void Input_ButtonsChanged(object? sender, ButtonsChangedEventArgs e)
    {
        if (Context.IsPlayerFree && Game1.activeClickableMenu is null && menuHotkey.JustPressed())
        {
            Game1.activeClickableMenu = viewEngine.CreateMenuFromAsset(
                $"Mods/{ModManifest.UniqueID}/Views/FramedSprite",
                new ItemViewModel());
        }    
    }
}
