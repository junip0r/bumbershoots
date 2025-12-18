using Bumbershoots.Ext.Verse;
using HarmonyLib;
using RimWorld;
using Verse;

namespace Bumbershoots.Patch.RimWorld;

[HarmonyPatch(typeof(ApparelUtility), nameof(ApparelUtility.HasPartsToWear))]
public static class ApparelUtility_HasPartsToWear
{
    private static void Postfix(ref bool __result, Pawn p, ThingDef apparel)
    {
        if (__result) return;
        if (!apparel.IsUmbrella()) return;
        if (!p.health.HasUmbrellaProsthetic()) return;
        __result = true;
    }
}
