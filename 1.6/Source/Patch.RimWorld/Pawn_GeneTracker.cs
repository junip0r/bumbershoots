using Bumbershoots.Ext.Verse;
using HarmonyLib;
using RimWorld;

namespace Bumbershoots.Patch.RimWorld;

[HarmonyPatch(typeof(Pawn_GeneTracker), nameof(Pawn_GeneTracker.Notify_GenesChanged))]
internal static class Pawn_GeneTracker_Notify_GenesChanged
{
    private static void Postfix(Pawn_GeneTracker __instance)
    {
        if (__instance.pawn.UmbrellaComp() is not UmbrellaComp umbrellaComp) return;
        umbrellaComp.Notify_GenesChanged();
    }
}