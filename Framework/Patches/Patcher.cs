using HarmonyLib;
using StardewValley.BellsAndWhistles;

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

        // Workarounds for broken Stardew-specific drawing functions.
        harmony.TryPatch(
            typeof(SpriteText),
            nameof(SpriteText.drawString),
            transpiler: new(typeof(SpriteTextPatches), nameof(SpriteTextPatches.DrawString_Transpiler))
        );
    }

    private static void TryPatch(
        this Harmony harmony,
        Type type,
        string methodName,
        HarmonyMethod? prefix = null,
        HarmonyMethod? postfix = null,
        HarmonyMethod? transpiler = null,
        HarmonyMethod? finalizer = null
    )
    {
        try
        {
            var original =
                AccessTools.Method(type, methodName)
                ?? throw new MissingMethodException($"Couldn't find method named '{methodName}' on type {type.Name}.");
            harmony.Patch(original, prefix, postfix, transpiler, finalizer);
        }
        catch (Exception ex)
        {
            Logger.Log(
                $"Failed to patch on {type.FullName}.{methodName}. This is a recoverable error, but some aspects of UI "
                    + "may display incorrectly.\n\nDetails:\n"
                    + ex.ToString()
            );
        }
    }
}
