using Bumbershoots.Ext.Verse;
using HarmonyLib;
using RimWorld;
using Verse;

namespace Bumbershoots.Patch.RimWorld;

[HarmonyPatch(typeof(ResurrectionUtility), nameof(ResurrectionUtility.TryResurrect))]
internal static class ResurrectionUtility_TryResurrect
{
    private static void Postfix(bool __result, Pawn pawn)
    {
        if (!__result) return;
        if (pawn.PawnComp() is not PawnComp pawnComp) return;
        pawnComp.Notify_Resurrected();
    }
}