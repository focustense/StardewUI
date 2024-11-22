using StardewUI.Framework;
using StardewValley;

namespace StardewUITest;

internal static class ExampleLauncher
{
    public static void Launch(this IMenuController controller)
    {
        if (Game1.activeClickableMenu is not null)
        {
            controller.DimmingAmount = 0.88f;
            Game1.activeClickableMenu.SetChildMenu(controller.Menu);
        }
        else
        {
            Game1.activeClickableMenu = controller.Menu;
        }
    }
}
