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
        if (__instance.pawn is not Pawn p) return;
        if (p.Map is not Map m) return;
        m.GetComponent<MapState>().OnApparelAdded(p, apparel);
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(Pawn_ApparelTracker.Notify_ApparelRemoved))]
    private static void Notify_ApparelRemoved(Pawn_ApparelTracker __instance, Apparel apparel)
    {
        if (__instance.pawn is not Pawn p) return;
        if (p.Map is not Map m) return;
        m.GetComponent<MapState>().OnApparelRemoved(p, apparel);
    }
}
