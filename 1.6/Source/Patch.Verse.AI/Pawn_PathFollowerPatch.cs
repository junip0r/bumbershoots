using Bumbershoots.Ext.RimWorld;
using Bumbershoots.Ext.Verse;
using Bumbershoots.Ext.Verse.AI;
using HarmonyLib;
using RimWorld;
using Verse;
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
        if (p.AnimalOrWildMan()) return;
        if (PawnState.For(p) is not PawnState ps) return;
        var m = ps.Map;
        var last = __instance.LastCellLazy();
        var next = __instance.nextCell;
        var dirty = m.weatherManager.IsUmbrellaWeather()
            && last.Value.Roofed(m) != next.Roofed(m);
        dirty = dirty || m.skyManager.IsUmbrellaSunlight()
            && last.Value.InSunlight(m) != next.InSunlight(m);
        if (dirty) ps.Dirty();
    }
}
