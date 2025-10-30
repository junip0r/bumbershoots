using Bumbershoots.Ext.Verse;
using HarmonyLib;
using System.Collections.Generic;
using Verse;

namespace Bumbershoots.Patch.Verse;

[HarmonyPatch(typeof(PawnRenderNode_Apparel))]
internal static class PawnRenderNode_ApparelPatch
{
    [HarmonyPostfix]
    [HarmonyPatch("GraphicsFor")]
    private static void GraphicsFor(
        ref IEnumerable<Graphic> __result,
        PawnRenderNode_Apparel __instance,
        Pawn pawn
    ) {
        if (__instance.apparel.def.IsUmbrella()
            && !pawn.IsUmbrellaDeployed())
        {
            __result = [];
        }
    }
}
