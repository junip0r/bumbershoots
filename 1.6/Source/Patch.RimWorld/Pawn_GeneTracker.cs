using Bumbershoots.Ext.Verse;
using HarmonyLib;
using RimWorld;

namespace Bumbershoots.Patch.RimWorld;

[HarmonyPatch(typeof(Pawn_GeneTracker), nameof(Pawn_GeneTracker.Notify_GenesChanged))]
public static class Pawn_GeneTracker_Notify_GenesChanged
{
    private static void Postfix(Pawn_GeneTracker __instance)
    {
        if (__instance.pawn.PawnComp() is not PawnComp pawnComp) return;
        pawnComp.Notify_GenesChanged();
    }
}
