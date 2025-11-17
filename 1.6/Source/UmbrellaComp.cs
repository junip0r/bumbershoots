using Bumbershoots.Ext.RimWorld;
using Bumbershoots.Ext.Verse;
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Bumbershoots;

public class UmbrellaComp : ThingComp
{
    internal UmbrellaProps umbrellaProps;
    internal Pawn pawn;
    internal PawnComp pawnComp;
    internal bool activated;
    private bool blockingSunlight;
    private bool blockingWeather;
    private bool hasEncumbrances;
    private readonly HashSet<string> hediffDefNames = [];
    private readonly List<HediffDef> hediffDefs = [];
    private readonly List<Hediff> hediffs = [];

    public UmbrellaProps UmbrellaProps
    {
        get => umbrellaProps;
    }

    public Pawn Pawn => Pawn;

    public PawnComp PawnComp => pawnComp;

    public bool Activated => activated;

    public bool BlockingSunlight => activated && blockingSunlight;

    public bool BlockingWeather => activated && blockingWeather;

public override void Initialize(CompProperties props)
    {
        umbrellaProps = (UmbrellaProps)props;
        if (ShouldDisable())
        {
            parent.AllComps.Remove(this);
            return;
        }
        base.Initialize(props);
        hasEncumbrances = umbrellaProps.HasEncumbrances;
        PrepareHediffDefs();
    }

    public override void PostExposeData()
    {
        Scribe_References.Look(ref pawn, nameof(pawn));
        Scribe_Values.Look(ref activated, nameof(activated));
        Scribe_Values.Look(ref blockingSunlight, nameof(blockingSunlight));
        Scribe_Values.Look(ref blockingWeather, nameof(blockingWeather));
        if (Scribe.mode == LoadSaveMode.PostLoadInit)
        {
            if (pawn != null) Reattach(false);
        }
    }

    public override void Notify_Equipped(Pawn pawn)
    {
        Attach(pawn);
    }

    public override void Notify_Unequipped(Pawn pawn)
    {
        Detach();
    }

    public void Notify_SettingsChanged()
    {
        Reattach(true);
    }

    private bool ShouldDisable()
    {
        var defName = umbrellaProps.defName;
        if (string.IsNullOrWhiteSpace(defName)) return false;
        return defName != parent.def.defName;
    }

    private void PrepareHediffDefs()
    {
        if (!hasEncumbrances) return;
        for (var i = 0; i < umbrellaProps.encumbrances.Count; i++)
        {
            var defName = umbrellaProps.encumbrances[i];
            if (DefDatabase<HediffDef>.GetNamed(defName) is not HediffDef def) continue;
            hediffDefNames.Add(def.defName);
            hediffDefs.Add(def);
        }
    }

    private void Attach(Pawn pawn)
    {
        if (pawn.PawnComp() is not PawnComp pawnComp) return;
        this.pawn = pawn;
        this.pawnComp = pawnComp;
        pawnComp.umbrellaComp = this;
    }

    private void Detach()
    {
        if (pawn is null) return;
        if (activated) Deactivate();
        pawnComp.umbrellaComp = null;
        pawn = null;
        pawnComp = null;
    }

    private void Reattach(bool updateGraphics)
    {
        pawnComp ??= pawn.PawnComp();
        pawnComp.umbrellaComp = this;
        var activated = this.activated;
        if (activated) Deactivate(false);
        if (activated) Activate(updateGraphics);
    }

    public override void CompTick()
    {
        if (pawnComp?.mapComp is not MapComp mapComp) return;
        Update(mapComp);
        if (ShouldActivate())
        {
            if (activated) return;
            Activate();
        }
        else if (activated)
        {
            Deactivate();
        }
    }

    private void Update(MapComp mapComp)
    {
        if (umbrellaProps.clothing && !Settings.UmbrellaClothing) goto Off;
        var cell = pawn.Position;
        if (cell.Roofed(mapComp.map)) goto Off;
        blockingSunlight = mapComp.IsUmbrellaSunlight
            && umbrellaProps.blocksSunlight
            && pawn.DislikesSunlight()
            && cell.InSunlight(mapComp.map);
        blockingWeather = mapComp.IsUmbrellaWeather
            && umbrellaProps.blocksWeather.Contains(mapComp.map.weatherManager.CurWeatherLerped.defName);
        return;
    Off:
        blockingSunlight = false;
        blockingWeather = false;
    }

    private bool ShouldActivate()
    {
        return blockingSunlight || blockingWeather;
    }

    private void Activate(bool updateGraphics = true)
    {
        activated = true;
        for (var i = 0; i < hediffDefs.Count; i++)
        {
            if (!IsEncumbranceEnabled(hediffDefs[i].defName)) continue;
            var hediff = HediffMaker.MakeHediff(hediffDefs[i], pawn);
            hediff.Severity = 1;
            hediff.canBeThreateningToPart = false;
            pawn.health.AddHediff(hediff);
            hediffs.Add(hediff);
        }
        if (updateGraphics && umbrellaProps.hideable) pawn.apparel.UpdateUmbrellaGraphics();
    }

    private void Deactivate(bool updateGraphics = true)
    {
        activated = false;
        if (!hasEncumbrances) return;
        if (hediffs.Count == 0)
        {
            for (var i = 0; i < pawn.health.hediffSet.hediffs.Count; i++)
            {
                var hediff = pawn.health.hediffSet.hediffs[i];
                if (hediffDefNames.Contains(hediff.def.defName))
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
        if (updateGraphics && umbrellaProps.hideable) pawn.apparel.UpdateUmbrellaGraphics();
    }

    private bool IsEncumbranceEnabled(string defName)
    {
        if (defName == DefOf.Bumber_UmbrellaEncumbranceCombat.defName) return Settings.EncumberCombat;
        if (defName == DefOf.Bumber_UmbrellaEncumbranceWork.defName) return Settings.EncumberWork;
        return true;
    }
}
