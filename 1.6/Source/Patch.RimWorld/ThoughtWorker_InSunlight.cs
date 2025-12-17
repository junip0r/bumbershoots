using Bumbershoots.Ext.Verse;
using HarmonyLib;
using RimWorld;
using Verse;

namespace Bumbershoots.Patch.RimWorld;

[HarmonyPatch(typeof(ThoughtWorker_InSunlight), nameof(ThoughtWorker_InSunlight.CurrentStateInternal))]
internal static class ThoughtWorker_InSunlight_CurrentStateInternal
{
    private const int InactiveIndex = -99999;

    private static void Postfix(ref ThoughtState __result, Pawn p)
    {
        if (__result.StageIndex == InactiveIndex) return;
        if (p.UmbrellaComp() is not UmbrellaComp umbrellaComp) return;
        if (!umbrellaComp.BlockingSunlight) return;
        __result = ThoughtState.Inactive;
    }
}
