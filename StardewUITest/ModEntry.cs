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
    private IViewEngine viewEngine = null!;

    private string ViewMarkup =>
        @"<lane orientation=""vertical"" horizontal-content-alignment=""middle"">
            <banner background={{@Mods/StardewUI/Sprites/BannerBackground}} background-border-thickness=""48,0"" padding=""12"" text={{HeaderText}} />
            <frame background={{@Mods/StardewUI/Sprites/ControlBorder}} margin=""0,16,0,0"" padding=""32,24"">
                <lane orientation=""vertical"" horizontal-content-alignment=""middle"">
                    <label font=""dialogue"" text=""Hello, world!"" margin=""0,0,0,8"" />
                    <label text=""This is an example paragraph."" />
                    <panel margin=""0,8,0,0"" horizontal-content-alignment=""middle"" vertical-content-alignment=""middle"">
                        <image layout=""stretch"" fit=""stretch"" sprite={{@Mods/StardewUI/Sprites/MenuSlotInset}} />
                        <image layout=""64px 64px"" margin=""8"" sprite={{ItemData}} />
                    </panel>
                </lane>
            </frame>
        </lane>";

    public override void Entry(IModHelper helper)
    {
        I18n.Init(helper.Translation);
        UI.Initialize(helper, Monitor);
        config = helper.ReadConfig<ModConfig>();

        helper.Events.GameLoop.GameLaunched += GameLoop_GameLaunched;
        helper.Events.Input.ButtonPressed += Input_ButtonPressed;
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
            Game1.activeClickableMenu = viewEngine.CreateMenuFromMarkup(ViewMarkup, context);
        }
    }
}
