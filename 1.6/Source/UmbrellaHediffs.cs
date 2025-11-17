using System.Collections.Generic;
using Verse;

namespace Bumbershoots;

internal class UmbrellaHediffs
{
    private readonly ICollection<string> defNames;
    private readonly List<HediffDef> defs = [];
    private readonly List<Hediff> hediffs = [];

    internal UmbrellaHediffs(UmbrellaProps umbrellaProps)
    {
        if (!umbrellaProps.HasHediffs) return;
        int count = umbrellaProps.hediffs.Count;
        defNames = count < 4 ? new List<string>(count) : new HashSet<string>();
        defs.Capacity = count;
        hediffs.Capacity = count;
        for (var i = 0; i < count; i++)
        {
            var defName = umbrellaProps.hediffs[i];
            if (DefDatabase<HediffDef>.GetNamed(defName) is not HediffDef def) continue;
            defNames.Add(defName);
            defs.Add(def);
        }
    }

    private bool IsHediffEnabled(string defName)
    {
        if (defName == DefOf.Bumber_DebuffCombat.defName) return Settings.EncumberCombat;
        if (defName == DefOf.Bumber_DebuffWork.defName) return Settings.EncumberWork;
        return true;
    }

    internal void Activate(Pawn pawn)
    {
        for (var i = 0; i < defs.Count; i++)
        {
            if (!IsHediffEnabled(defs[i].defName)) continue;
            var hediff = HediffMaker.MakeHediff(defs[i], pawn);
            hediff.Severity = defs[i].stages[0].minSeverity;
            pawn.health.AddHediff(hediff);
            hediffs.Add(hediff);
        }
    }

    internal void Deactivate(Pawn pawn)
    {
        if (defs.Count == 0) return;
        if (hediffs.Count == 0)
        {
            for (var i = 0; i < pawn.health.hediffSet.hediffs.Count; i++)
            {
                var hediff = pawn.health.hediffSet.hediffs[i];
                if (defNames.Contains(hediff.def.defName))
                {
                    hediffs.Add(hediff);
                }
            }
        }
        for (var i = 0; i < hediffs.Count; i++)
        {
            pawn.health.RemoveHediff(hediffs[i]);
        }
        hediffs.Clear();
    }
}
