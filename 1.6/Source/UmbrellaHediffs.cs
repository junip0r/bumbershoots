using System.Collections.Generic;
using Verse;

namespace Bumbershoots;

internal class UmbrellaHediffs
{
    private static readonly List<Hediff> hediffs = [];

    private readonly List<HediffDef> defs = [];
    private readonly HashSet<string> defNames = [];

    internal UmbrellaHediffs(UmbrellaProps umbrellaProps)
    {
        var hediffs = umbrellaProps.hediffs;
        if (hediffs is null || hediffs.Count == 0) return;
        int count = umbrellaProps.hediffs.Count;
        defs.Capacity = count;
        for (var i = 0; i < count; i++)
        {
            var defName = umbrellaProps.hediffs[i];
            if (DefDatabase<HediffDef>.GetNamed(defName) is not HediffDef def) continue;
            defs.Add(def);
            defNames.Add(defName);
        }
        hediffs.Capacity = defs.Count;
    }

    private bool IsHediffEnabled(HediffDef def)
    {
        if (def.defName == DefOf.Bumber_DebuffCombat.defName) return Settings.encumberCombat;
        if (def.defName == DefOf.Bumber_DebuffWork.defName) return Settings.encumberWork;
        return true;
    }

    internal void Activate(Pawn pawn)
    {
        for (var i = 0; i < defs.Count; i++)
        {
            var def = defs[i];
            if (!IsHediffEnabled(def)) continue;
            var hediff = HediffMaker.MakeHediff(def, pawn);
            hediff.Severity = def.stages[0].minSeverity;
            pawn.health.AddHediff(hediff);
        }
    }

    internal void Deactivate(Pawn pawn)
    {
        if (defs.Count == 0) return;
        for (var i = 0; i < pawn.health.hediffSet.hediffs.Count; i++)
        {
            var hediff = pawn.health.hediffSet.hediffs[i];
            if (defNames.Contains(hediff.def.defName)) hediffs.Add(hediff);
        }
        if (hediffs.Count > 0)
        {
            for (var i = 0; i < hediffs.Count; i++)
            {
                pawn.health.RemoveHediff(hediffs[i]);
            }
            hediffs.Clear();
        }
    }
}
