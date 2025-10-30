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
        if (__result.StageIndex != InactiveIndex
            && p.IsBlockingSunlight())
        {
            __result = ThoughtState.Inactive;
        }
    }
}
