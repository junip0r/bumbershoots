using Bumbershoots.Ext.RimWorld;
using HarmonyLib;
using System.Collections.Generic;
using Verse;

namespace Bumbershoots.Patch.Verse;

[HarmonyPatch(typeof(PawnRenderNode_Apparel), nameof(PawnRenderNode_Apparel.GraphicsFor))]
public static class PawnRenderNode_Apparel_GraphicsFor
{
    public static void Postfix(ref IEnumerable<Graphic> __result, PawnRenderNode_Apparel __instance)
    {
        if (__instance.apparel.UmbrellaComp() is not UmbrellaComp umbrellaComp) return;
        if (!umbrellaComp.umbrellaProps.hideable) return;
        if (Settings.showUmbrellas && umbrellaComp.activated) return;
        __result = [];
    }
}
