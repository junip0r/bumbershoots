using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace Bumbershoots.Ext.Verse;

internal static class Pawn_HealthTrackerExt
{
    private static List<Hediff> CollectEncumbrances(Pawn_HealthTracker t)
    {
        List<Hediff> present = [];
        for (var i = 0; i < t.hediffSet.hediffs.Count; i++)
        {
            var h = t.hediffSet.hediffs[i];
            if (h.def.IsUmbrellaEncumbrance())
            {
                present.Add(h);
            }
        }
        return present;
    }

    private static void AddEncumbrances(Pawn_HealthTracker t)
    {
        static int FindIndex(List<Hediff> hediffs, string defName)
        {
            for (var i = 0; i < hediffs.Count; i++)
            {
                if (hediffs[i].def.defName == defName)
                {
                    return i;
                }
            }
            return -1;
        }

        var present = CollectEncumbrances(t);
        for (var i = 0; i < HediffDefExt.UmbrellaEncumbrances.Count; i++)
        {
            var defName = HediffDefExt.UmbrellaEncumbrances[i];
            var defEnabled = HediffDefExt.UmbrellaEncumbranceEnabled[i];
            var defIndex = FindIndex(present, defName);
            if (defEnabled())
            {
                if (defIndex != -1) continue;
                var def = DefDatabase<HediffDef>.GetNamed(defName);
                var h = HediffMaker.MakeHediff(def, t.hediffSet.pawn);
                h.Severity = 1;
                h.canBeThreateningToPart = false;
                t.AddHediff(h);
            }
            else if (defIndex != -1)
            {
                t.RemoveHediff(present[defIndex]);
                present.RemoveAt(defIndex);
            }
        }
    }

    private static void RemoveEncumbrances(Pawn_HealthTracker t)
    {
        var present = CollectEncumbrances(t);
        for (var i = 0; i < present.Count; i++)
        {
            t.RemoveHediff(present[i]);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void UpdateUmbrellaHediffs(this Pawn_HealthTracker t)
    {
        if (t.hediffSet.pawn.IsUmbrellaDeployed())
        {
            AddEncumbrances(t);
        }
        else
        {
            RemoveEncumbrances(t);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool HasUmbrellaProsthetic(this Pawn_HealthTracker t)
    {
        for (var i = 0; i < t.hediffSet.hediffs.Count; i++)
        {
            if (t.hediffSet.hediffs[i].def.IsUmbrellaProsthetic()) return true;
        }
        return false;
    }
}
