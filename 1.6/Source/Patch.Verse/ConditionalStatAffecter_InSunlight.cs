using Bumbershoots.Ext.Verse;
using HarmonyLib;
using RimWorld;
using Verse;

namespace Bumbershoots.Patch.Verse;

[HarmonyPatch(typeof(ConditionalStatAffecter_InSunlight), nameof(ConditionalStatAffecter_InSunlight.Applies))]
public static class ConditionalStatAffecter_InSunlight_Applies
{
    public static void Postfix(ref bool __result, StatRequest req)
    {
        if (!__result) return;
        if (req.Thing is not Pawn p) return;
        if (p.UmbrellaComp() is not UmbrellaComp umbrellaComp) return;
        if (!umbrellaComp.BlockingSunlight) return;
        __result = false;
    }
}
