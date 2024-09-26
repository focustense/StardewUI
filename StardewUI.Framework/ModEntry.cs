namespace StardewUI.Framework;

internal sealed class ModEntry : Mod
{
    // Initialized in Entry
    private ModConfig config = null!;

    public override void Entry(IModHelper helper)
    {
        I18n.Init(helper.Translation);
        config = helper.ReadConfig<ModConfig>();
    }
}
