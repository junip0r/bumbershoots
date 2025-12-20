using Bumbershoots.Ext.Verse;
using HarmonyLib;
using RimWorld;
using Verse;

namespace Bumbershoots.Patch.RimWorld;

[HarmonyPatch(typeof(ThoughtWorker_InSunlight), nameof(ThoughtWorker_InSunlight.CurrentStateInternal))]
public static class ThoughtWorker_InSunlight_CurrentStateInternal
{
    public const int InactiveIndex = -99999;

    public static void Postfix(ref ThoughtState __result, Pawn p)
    {
        if (__result.StageIndex == InactiveIndex) return;
        if (p.UmbrellaComp() is not UmbrellaComp umbrellaComp) return;
        if (!umbrellaComp.BlockingSunlight) return;
        __result = ThoughtState.Inactive;
    }
}
