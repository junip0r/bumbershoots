using Bumbershoots.Ext.System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace Bumbershoots.Ext.Verse;

internal static class Pawn_HealthTrackerExt
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static List<Hediff> CollectEncumbrances(Pawn_HealthTracker t)
    {
        return [.. t.hediffSet.hediffs.Where(h => h.def.IsUmbrellaEncumbrance())];
    }

    private static void AddEncumbrance(
        Pawn_HealthTracker t,
        List<Hediff> present,
        string defName,
        Func<bool> defEnabled
    ) {
        if (defEnabled())
        {
            if (!present.Any(h => h.def.defName == defName))
            {
                t.AddHediff(DefDatabase<HediffDef>.GetNamed(defName));
            }
        }
        else
        {
            bool match(Hediff h) => h.def.defName == defName;
            for (var i = present.FindIndex(match);
                 i >= 0;
                 i = present.FindIndex(match))
            {
                t.RemoveHediff(present[i]);
                present.RemoveAt(i);
            }
        }
    }

    private static void AddEncumbrances(Pawn_HealthTracker t)
    {
        if (HediffDefExt.UmbrellaEncumbrances.Count == 0) return;
        var present = CollectEncumbrances(t);
        HediffDefExt.UmbrellaEncumbrances
            .Zip(HediffDefExt.UmbrellaEncumbranceEnabled, (n, e) => (n, e))
            .ForEach(_ => AddEncumbrance(t, present, _.n, _.e));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void RemoveEncumbrances(Pawn_HealthTracker t)
    {
        CollectEncumbrances(t).ForEach(t.RemoveHediff);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool HasUmbrellaProsthetic(this Pawn_HealthTracker t)
    {
        return t.hediffSet.hediffs.Any(h => h.def.IsUmbrellaProsthetic());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void UpdateUmbrellaState(this Pawn_HealthTracker t)
    {
        if (PawnState.IsUmbrellaDeployed(t.hediffSet.pawn))
            AddEncumbrances(t);
        else
            RemoveEncumbrances(t);
    }
}
