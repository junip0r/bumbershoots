using HarmonyLib;
using RimWorld;
using Verse.AI;
using Verse;

namespace Bumbershoots.Patch.Verse;

[HarmonyPatch(typeof(Pawn_MindState))]
internal static class Pawn_MindStatePatch
{
    [HarmonyPostfix]
    [HarmonyPatch("CanGainGainThoughtNow")]
    private static void CanGainGainThoughtNow(ref bool __result, Pawn_MindState __instance, ThoughtDef thought)
    {
        var p = __instance.pawn;
        if (__result
            && thought == ThoughtDefOf.SoakingWet
            && PawnState.IsBlockingWeather(p))
        {
            __result = false;
        }
    }
}
