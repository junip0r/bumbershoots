using Bumbershoots.Ext.Verse;
using HarmonyLib;
using RimWorld;
using Verse;

namespace Bumbershoots.Patch.RimWorld;

[HarmonyPatch(typeof(ApparelUtility))]
internal static class ApparelUtilityPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(HasPartsToWear))]
    private static void HasPartsToWear(ref bool __result, Pawn p, ThingDef apparel)
    {
        if (__result) return;
        if (!ThingDefExt.Umbrellas.Contains(apparel.defName)) return;
        if (!p.health.HasUmbrellaProsthetic()) return;
        __result = true;
    }
}
