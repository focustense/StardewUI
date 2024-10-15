using HarmonyLib;

namespace StardewUI.Framework.Patches;

/// <summary>
/// Entry point for all patches managed by StardewUI.
/// </summary>
internal static class Patcher
{
    /// <summary>
    /// Applies all patches.
    /// </summary>
    /// <param name="harmonyId">Unique ID for the harmony instance; generally the mod ID.</param>
    public static void Patch(string harmonyId)
    {
        var harmony = new Harmony(harmonyId);
        var smapiAssembly = typeof(IMod).Assembly;
        var modContentManagerType = smapiAssembly.GetType(
            "StardewModdingAPI.Framework.ContentManagers.ModContentManager"
        );
        // It's not clear if prefixing HandleUnknownFileType (vs. transpiling LoadExact) is really the best way to add
        // our loader, but some historical comments in SMAPI's change logs seem to lean that way, i.e.
        // https://github.com/Pathoschild/SMAPI/blob/d9a46f64be0c31e10b99f4567f3da511937c55f1/docs/release-notes.md?plain=1#L252
        harmony.Patch(
            AccessTools.Method(modContentManagerType, "HandleUnknownFileType"),
            prefix: new(typeof(ModContentManagerPatches), nameof(ModContentManagerPatches.HandleUnknownFileType_Prefix))
        );
    }
}
