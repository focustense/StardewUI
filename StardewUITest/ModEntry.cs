using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewUI;
using StardewValley;

namespace StardewUITest;

internal sealed class ModEntry : Mod
{
    // Initialized in Entry
    private ModConfig config = null!;

    public override void Entry(IModHelper helper)
    {
        I18n.Init(helper.Translation);
        UI.Initialize(helper, Monitor);
        config = helper.ReadConfig<ModConfig>();

        helper.Events.Input.ButtonPressed += Input_ButtonPressed;
    }

    private void Input_ButtonPressed(object? sender, ButtonPressedEventArgs e)
    {
        if (Context.IsPlayerFree && e.Button == SButton.F8)
        {
            Game1.activeClickableMenu = new TestMenu();
        }
    }
}
