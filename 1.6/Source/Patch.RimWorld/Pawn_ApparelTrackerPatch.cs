using Bumbershoots.Ext.RimWorld;
using Bumbershoots.Ext.Verse;
using HarmonyLib;
using RimWorld;
using Verse;

namespace Bumbershoots.Patch.RimWorld;

[HarmonyPatch(typeof(Pawn_ApparelTracker))]
internal static class Pawn_ApparelTrackerPatch
{

    [HarmonyPostfix]
    [HarmonyPatch(nameof(Pawn_ApparelTracker.Notify_ApparelAdded))]
    private static void Notify_ApparelAdded(Pawn_ApparelTracker __instance, Apparel apparel)
    {
        if (!apparel.def.IsUmbrellaOrHat()) return;
        var p = __instance.pawn;
        if (p.IsWildMan() || p.MapHeld is null) return;
        PawnState.Add(p);
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(Pawn_ApparelTracker.Notify_ApparelRemoved))]
    private static void Notify_ApparelRemoved(Pawn_ApparelTracker __instance, Apparel apparel)
    {
        if (!apparel.def.IsUmbrellaOrHat()) return;
        var p = __instance.pawn;
        if (p.apparel.HasUmbrellaOrHat()) return;
        PawnState.Remove(p);
    }
}
