using System.Collections.Generic;
using Verse;

namespace Bumbershoots;

public class UmbrellaHediffs
{
    public readonly List<HediffDef> defs = [];
    public readonly HashSet<string> defNames = [];
    public readonly List<Hediff> hediffs = [];

    private int Stage
    {
        set
        {
            for (var i = 0; i < hediffs.Count; i++)
                hediffs[i].Severity = hediffs[i].def.stages[value].minSeverity;
        }
    }

    public UmbrellaHediffs(UmbrellaProps umbrellaProps)
    {
        var allDefNames = umbrellaProps.hediffs;
        if (allDefNames is null) return;
        var count = allDefNames.Count;
        if (count == 0) return;
        defs.Capacity = count;
        for (var i = 0; i < count; i++)
        {
            var defName = allDefNames[i];
            var def = DefDatabase<HediffDef>.GetNamed(defName);
            if (def == null) continue;
            defs.Add(def);
            defNames.Add(defName);
        }
    }

    private static bool IsHediffEnabled(HediffDef def)
    {
        if (def.defName == DefOf.Bumber_CombatDebuff.defName) return Settings.encumberCombat;
        if (def.defName == DefOf.Bumber_WorkDebuff.defName) return Settings.encumberWork;
        return true;
    }

    public void Add(Pawn pawn)
    {
        if (hediffs.Count > 0) return;
        for (var i = 0; i < defs.Count; i++)
        {
            var def = defs[i];
            if (!IsHediffEnabled(def)) continue;
            var hediff = HediffMaker.MakeHediff(def, pawn);
            hediff.Severity = def.stages[0].minSeverity;
            pawn.health.AddHediff(hediff);
            hediffs.Add(hediff);
        }
    }

    public void Remove(Pawn pawn)
    {
        for (var i = 0; i < hediffs.Count; i++)
            pawn.health.RemoveHediff(hediffs[i]);
        hediffs.Clear();
    }

    public void Load(Pawn pawn)
    {
        for (var i = 0; i < pawn.health.hediffSet.hediffs.Count; i++)
        {
            var hediff = pawn.health.hediffSet.hediffs[i];
            if (defNames.Contains(hediff.def.defName)) hediffs.Add(hediff);
        }
        Add(pawn);
    }

    public void Activate()
    {
        Stage = 1;
    }

    public void Deactivate()
    {
        Stage = 0;
    }
}
