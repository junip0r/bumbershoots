using Bumbershoots.Ext.Verse;
using HarmonyLib;
using RimWorld;
using Verse.AI;

namespace Bumbershoots.Patch.Verse;

[HarmonyPatch(typeof(Pawn_MindState))]
internal static class Pawn_MindStatePatch
{
    [HarmonyPostfix]
    [HarmonyPatch("CanGainGainThoughtNow")]
    private static void CanGainGainThoughtNow(ref bool __result, Pawn_MindState __instance, ThoughtDef thought)
    {
        if (__result
            && thought == ThoughtDefOf.SoakingWet
            && __instance.pawn.IsBlockingWeather())
        {
            __result = false;
        }
    }
}
