using StardewModdingAPI;

namespace StardewUI;

/// <summary>
/// Entry point for Stardew UI. Must be called from <see cref="Mod.Entry(IModHelper)"/>.
/// </summary>
public static class UI
{
    /// <summary>
    /// Helper for game input.
    /// </summary>
    internal static IInputHelper InputHelper => EnsureInitialized(() => modHelper.Input);

    private static IModHelper modHelper = null!;

    /// <summary>
    /// Initialize the framework.
    /// </summary>
    /// <param name="helper">Helper for the calling mod.</param>
    /// <param name="monitor">SMAPI logging helper.</param>
    public static void Initialize(IModHelper helper, IMonitor monitor)
    {
        modHelper = helper;
        Logger.Monitor = monitor;
    }

    private static T EnsureInitialized<T>(Func<T> selector)
    {
        if (modHelper is null)
        {
            throw new InvalidOperationException(
                "StardewUI has not been initialized. Ensure you've called UI.Initialize(helper) from your mod's "
                    + "Entry method."
            );
        }
        return selector();
    }
}
