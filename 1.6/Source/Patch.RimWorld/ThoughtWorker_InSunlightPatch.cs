using Bumbershoots.Ext.Verse;
using HarmonyLib;
using RimWorld;
using Verse;

namespace Bumbershoots.Patch.RimWorld;

[HarmonyPatch(typeof(ThoughtWorker_InSunlight))]
internal static class ThoughtWorker_InSunlightPatch
{
    private const int InactiveIndex = -99999;

    [HarmonyPostfix]
    [HarmonyPatch("CurrentStateInternal")]
    private static void CurrentStateInternal(ref ThoughtState __result, Pawn p)
    {
        if (__result.StageIndex == InactiveIndex) return;
        if (p.UmbrellaComp() is not UmbrellaComp umbrellaComp) return;
        if (!umbrellaComp.BlockingSunlight) return;
        __result = ThoughtState.Inactive;
    }
}
