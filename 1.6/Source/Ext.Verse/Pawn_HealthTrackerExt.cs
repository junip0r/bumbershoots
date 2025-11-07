using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace Bumbershoots.Ext.Verse;

internal static class Pawn_HealthTrackerExt
{
    private static readonly List<Hediff> present = [];

    private static void CollectActiveEncumbrances(this Pawn_HealthTracker @this)
    {
        present.Clear();
        for (var i = 0; i < @this.hediffSet.hediffs.Count; i++)
        {
            var h = @this.hediffSet.hediffs[i];
            if (h.def.IsUmbrellaEncumbrance())
            {
                present.Add(h);
            }
        }
    }

    private static void AddEncumbrances(this Pawn_HealthTracker @this)
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

        @this.CollectActiveEncumbrances();
        for (var i = 0; i < HediffDefExt.UmbrellaEncumbrances.Count; i++)
        {
            var defName = HediffDefExt.UmbrellaEncumbrances[i];
            var defEnabled = HediffDefExt.UmbrellaEncumbranceEnabled[i];
            var defIndex = FindIndex(present, defName);
            if (defEnabled())
            {
                if (defIndex != -1) continue;
                if (DefDatabase<HediffDef>.GetNamedSilentFail(defName) is not HediffDef def) continue;
                var h = HediffMaker.MakeHediff(def, @this.hediffSet.pawn);
                h.Severity = 1;
                h.canBeThreateningToPart = false;
                @this.AddHediff(h);
            }
            else if (defIndex != -1)
            {
                @this.RemoveHediff(present[defIndex]);
            }
        }
    }

    private static void RemoveEncumbrances(this Pawn_HealthTracker @this)
    {
        @this.CollectActiveEncumbrances();
        for (var i = 0; i < present.Count; i++)
        {
            @this.RemoveHediff(present[i]);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void UpdateUmbrellaHediffs(this Pawn_HealthTracker @this)
    {
        if (@this.hediffSet.pawn.IsUmbrellaDeployed())
        {
            @this.AddEncumbrances();
        }
        else
        {
            @this.RemoveEncumbrances();
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool HasUmbrellaProsthetic(this Pawn_HealthTracker @this)
    {
        for (var i = 0; i < @this.hediffSet.hediffs.Count; i++)
        {
            if (@this.hediffSet.hediffs[i].def.IsUmbrellaProsthetic()) return true;
        }
        return false;
    }
}
