using Bumbershoots.Ext.Verse;
using HarmonyLib;
using RimWorld;
using Verse.AI;

namespace Bumbershoots.Patch.Verse.AI;

[HarmonyPatch(typeof(Pawn_MindState), nameof(Pawn_MindState.CanGainGainThoughtNow))]
internal static class Pawn_MindState_CanGainGainThoughtNow
{
    private static void Postfix(ref bool __result, Pawn_MindState __instance, ThoughtDef thought)
    {
        if (!__result) return;
        if (thought != __instance.pawn.Map.weatherManager.CurWeatherLerped.weatherThought) return;
        if (__instance.pawn.UmbrellaComp() is not UmbrellaComp umbrellaComp) return;
        if (!umbrellaComp.BlockingWeather) return;
        __result = false;
    }
}
