using HarmonyLib;
using Bumbershoots.Ext.Verse;
using Bumbershoots.Ext.Verse.AI;
using Verse.AI;

namespace Bumbershoots.Patch.Verse.AI;

[HarmonyPatch(typeof(Pawn_PathFollower))]
internal static class Pawn_PathFollowerPatch
{
    [HarmonyPostfix]
    [HarmonyPatch("TryEnterNextPathCell")]
    private static void TryEnterNextPathCell(Pawn_PathFollower __instance)
    {
        var p = __instance.Pawn();
        p.Map?.MapState().OnPawnStep(p);
    }
}
