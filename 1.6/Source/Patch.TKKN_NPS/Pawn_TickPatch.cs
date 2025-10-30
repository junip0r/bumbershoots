using Bumbershoots.Ext.RimWorld;
using HarmonyLib;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using Verse;

namespace Bumbershoots.Patch.TKKN_NPS;

using NpsSettings = Ext.TKKN_NPS.Settings;

internal static class Pawn_TickPatch
{
    private static readonly LazyDef<HediffDef> TKKN_Wetness = new("TKKN_Wetness");
    private const string TKKN_Wet = "TKKN_Wet";

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ApplyWet(Pawn pawn)
    {
        if (TKKN_Wetness.Def is not HediffDef def) return;
        if (pawn.health.hediffSet.hediffs.Any(h => h.def == def)) return;
        Hediff h = HediffMaker.MakeHediff(def, pawn);
        h.Severity = 0f;
        pawn.health.AddHediff(h);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool WetFromWeather(Map m, Pawn p, IntVec3 pos) =>
        m.weatherManager.IsUmbrellaWeather()
            && !pos.Roofed(m)
            && !PawnState.IsBlockingWeather(p);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool WetFromTerrain(Map m, IntVec3 pos) =>
        GridsUtility.GetTerrain(pos, m).HasTag(TKKN_Wet);

    private static bool MakeWet(Pawn pawn)
    {
        // Block soaking wet from weather if carrying an appropriate umbrella,
        // but umbrellas will not block soaking wet from terrain.

        if (!NpsSettings.AllowPawnsToGetWet.GetValueOrDefault()) return false;
        if (pawn.MapHeld is not Map m) return false;
        var pos = pawn.Position;
        if (WetFromWeather(m, pawn, pos) || WetFromTerrain(m, pos)) ApplyWet(pawn);
        return false;
    }

    internal static void Patch(Harmony harmony)
    {
        if (AccessTools.TypeByName("TKKN_NPS.Pawn_Tick") is not Type type) return;
        var flags = BindingFlags.Static | BindingFlags.NonPublic;
        if (type.GetMethod("MakeWet", flags) is not MethodInfo src) return;
        var dst = new HarmonyMethod(MakeWet);
        harmony.Patch(src, prefix: dst);
    }
}
