using System.Collections.Generic;
using Verse;

namespace Bumbershoots;

public class UmbrellaHediffs
{
    public readonly List<HediffDef> defs = [];
    public readonly HashSet<string> defNames = [];
    public readonly List<Hediff> hediffs = [];

    public int Stage
    {
        set
        {
            var count = hediffs.Count;
            for (var i = 0; i < count; i++)
                hediffs[i].Severity = hediffs[i].def.stages[value].minSeverity;
        }
    }

    public void Activate() => Stage = 1;
    public void Deactivate() => Stage = 0;

    public UmbrellaHediffs(UmbrellaProps umbrellaProps)
    {
        var allDefNames = umbrellaProps.hediffs;
        if (allDefNames == null) return;
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

    public static bool IsHediffEnabled(HediffDef def)
    {
        if (def == DefOf.Bumber_CombatDebuff) return Settings.encumberCombat;
        if (def == DefOf.Bumber_WorkDebuff) return Settings.encumberWork;
        return true;
    }

    public void Add(Pawn pawn)
    {
        if (hediffs.Count > 0) return;
        var count = defs.Count;
        for (var i = 0; i < count; i++)
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
        var count = hediffs.Count;
        for (var i = 0; i < count; i++)
            pawn.health.RemoveHediff(hediffs[i]);
        hediffs.Clear();
    }

    public void Load(Pawn pawn)
    {
        var count = pawn.health.hediffSet.hediffs.Count;
        for (var i = 0; i < count; i++)
        {
            var hediff = pawn.health.hediffSet.hediffs[i];
            if (defNames.Contains(hediff.def.defName)) hediffs.Add(hediff);
        }
        Add(pawn);
    }
}
