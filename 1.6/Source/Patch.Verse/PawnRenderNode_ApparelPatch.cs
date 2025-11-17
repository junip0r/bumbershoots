using Bumbershoots.Ext.RimWorld;
using HarmonyLib;
using System.Collections.Generic;
using Verse;

namespace Bumbershoots.Patch.Verse;

[HarmonyPatch(typeof(PawnRenderNode_Apparel))]
internal static class PawnRenderNode_ApparelPatch
{
    [HarmonyPostfix]
    [HarmonyPatch("GraphicsFor")]
    private static void GraphicsFor(ref IEnumerable<Graphic> __result, PawnRenderNode_Apparel __instance)
    {
        if (__instance.apparel.UmbrellaComp() is not UmbrellaComp umbrellaComp) return;
        if (!umbrellaComp.umbrellaProps.hideable) return;
        if (Settings.ShowUmbrellas && umbrellaComp.activated) return;
        __result = [];
    }
}
