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
        if (!__result) return;
        if (req.Thing is not Pawn p) return;
        if (p.UmbrellaComp() is not UmbrellaComp umbrellaComp) return;
        if (!umbrellaComp.blockingSunlight) return;
        __result = false;
    }
}
