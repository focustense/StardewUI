using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using StardewValley.BellsAndWhistles;

namespace StardewUI.Framework.Patches;

/// <summary>
/// Patches for <see cref="SpriteText"/>.
/// </summary>
internal static class SpriteTextPatches
{
    private static readonly MethodInfo GetViewIsDrawing = AccessTools.PropertyGetter(
        typeof(View),
        nameof(View.IsDrawing)
    );

    // Patches SpriteText.drawString to skip bounds checking when invoked from StardewUI.
    //
    // The vanilla code isn't transform-aware, and naively tries to force drawing within viewport bounds even though it
    // is legal to draw outside those bounds (and the bounds may be within the viewport after transformation).
    public static IEnumerable<CodeInstruction> DrawString_Transpiler(
        IEnumerable<CodeInstruction> instructions,
        ILGenerator gen,
        MethodBase original
    )
    {
        var drawBGScrollIndex = original
            .GetParameters()
            .Select((parameter, index) => (parameter, index))
            .Where(x => x.parameter.Name == "drawBGScroll")
            .Select(x => x.index)
            .FirstOrDefault(10);
        var match = new CodeMatcher(instructions, gen)
            .MatchEndForward(
                new CodeMatch(ci => ci.IsLdarg(drawBGScrollIndex)),
                new CodeMatch(OpCodes.Ldc_I4_1),
                new CodeMatch(OpCodes.Beq_S)
            )
            .ThrowIfNotMatch(
                "Couldn't find bounds checking operations in SpriteText for transpilation. Banners and other sprite "
                    + "text will still be drawn, but may display incorrectly in some cases, e.g. when used with clipping "
                    + "and transforms."
            );
        var skipBoundsCheckLabel = match.Operand;
        return match
            .Advance(1)
            .Insert(
                new CodeInstruction(OpCodes.Call, GetViewIsDrawing),
                new CodeInstruction(OpCodes.Brtrue_S, skipBoundsCheckLabel)
            )
            .InstructionEnumeration();
    }
}
