using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewUI.Animation;
using StardewValley;

namespace StardewUI;

/// <summary>
/// Entry point for Stardew UI. Must be called from <see cref="Mod.Entry(IModHelper)"/>.
/// </summary>
public static partial class UI
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
        if (modHelper is not null)
        {
            throw new InvalidOperationException("UI is already initialized.");
        }
        modHelper = helper;
        Logger.Monitor = monitor;
        helper.Events.GameLoop.UpdateTicked += GameLoop_UpdateTicked;
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

    private static void GameLoop_UpdateTicked(object? sender, UpdateTickedEventArgs e)
    {
        AnimationRunner.Tick(Game1.currentGameTime.ElapsedGameTime);
    }
}
