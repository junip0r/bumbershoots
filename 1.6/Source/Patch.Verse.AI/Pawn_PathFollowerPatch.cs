using Bumbershoots.Ext.Verse;
using Bumbershoots.Ext.Verse.AI;
using HarmonyLib;
using Verse.AI;

namespace Bumbershoots.Patch.Verse.AI;

[HarmonyPatch(typeof(Pawn_PathFollower))]
internal static class Pawn_PathFollowerPatch
{
    [HarmonyPostfix]
    [HarmonyPatch("TryEnterNextPathCell")]
    private static void TryEnterNextPathCell(Pawn_PathFollower __instance)
    {
        __instance.MapState()?.OnPawnStep(__instance);
    }
}
