using Bumbershoots.Ext.Verse;
using HarmonyLib;
using Verse;

namespace Bumbershoots.Patch.Verse;

[HarmonyPatch(typeof(Pawn_HealthTracker), nameof(Pawn_HealthTracker.Notify_Resurrected))]
internal static class Apparel_Notify_PawnResurrected
{
    private static void Postfix(Pawn_HealthTracker __instance)
    {
        if (__instance.pawn.PawnComp() is not PawnComp pawnComp) return;
        pawnComp.Notify_Resurrected();
    }
}
