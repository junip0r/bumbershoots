using Bumbershoots.Ext.Verse;
using HarmonyLib;
using RimWorld;
using Verse;

namespace Bumbershoots.Patch.Verse;

[HarmonyPatch(typeof(ConditionalStatAffecter_InSunlight))]
internal static class ConditionalStatAffecter_InSunlightPatch
{
    [HarmonyPostfix]
    [HarmonyPatch("Applies")]
    private static void Applies(ref bool __result, StatRequest req)
    {
        if (__result
            && req.Thing is Pawn p
            && p.IsBlockingSunlight())
        {
            __result = false;
        }
    }
}
