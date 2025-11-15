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
    private readonly List<HediffDef> encumbranceDefs = [];
    private List<Hediff> encumbranceHediffs = [];

    public UmbrellaProps UmbrellaProps
    {
        get => umbrellaProps;
    }

    public Pawn Pawn
    {
        get => pawn;
    }

    public PawnComp PawnComp
    {
        get => pawnComp;
    }

    public bool Activated
    {
        get => activated;
    }

    public bool BlockingSunlight
    {
        get => activated && blockingSunlight;
    }

    public bool BlockingWeather
    {
        get => activated && blockingWeather;
    }

    public override void Initialize(CompProperties props)
    {
        umbrellaProps = (UmbrellaProps)props;
        if (ShouldDisable())
        {
            parent.AllComps.Remove(this);
            return;
        }
        base.Initialize(props);
        if (umbrellaProps.encumbrances is null) return;
        for (var i = 0; i < umbrellaProps.encumbrances.Count; i++)
        {
            var defName = umbrellaProps.encumbrances[i];
            if (DefDatabase<HediffDef>.GetNamed(defName) is not HediffDef def) continue;
            encumbranceDefs.Add(def);
        }
    }

    public override void PostExposeData()
    {
        Scribe_References.Look(ref pawn, nameof(pawn));
        Scribe_Values.Look(ref activated, nameof(activated));
        Scribe_Values.Look(ref blockingSunlight, nameof(blockingSunlight));
        Scribe_Values.Look(ref blockingWeather, nameof(blockingWeather));
        Scribe_Collections.Look(ref encumbranceHediffs, nameof(encumbranceHediffs), LookMode.Reference);
        if (Scribe.mode == LoadSaveMode.PostLoadInit)
        {
            if (pawn != null && pawnComp is null) Reattach();
            encumbranceHediffs ??= [];
            if (activated) Reactivate(); // re-apply encumbrance hediffs in case of mod update
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
        if (!activated) return;
        Reactivate(); // re-apply encumbrance hediffs using current settings
    }

    public override void CompTick()
    {
        if (pawn is null) return;
        if (pawnComp.MapComp is not MapComp mapComp) return;
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

    private bool ShouldDisable()
    {
        var defName = umbrellaProps.defName;
        if (string.IsNullOrWhiteSpace(defName)) return false;
        return defName != parent.def.defName;
    }

    private void Attach(Pawn pawn)
    {
        if (pawn.PawnComp() is not PawnComp pawnComp) return;
        this.pawn = pawn;
        this.pawnComp = pawnComp;
        pawnComp.UmbrellaComp = this;
    }

    private void Reattach()
    {
        pawnComp = pawn.PawnComp();
        pawnComp.UmbrellaComp = this;
    }

    private void Detach()
    {
        if (pawn is null) return;
        blockingSunlight = false;
        blockingWeather = false;
        if (pawn is null) return;
        if (activated) Deactivate();
        pawnComp.UmbrellaComp = null;
        pawn = null;
        pawnComp = null;
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

    private void Activate()
    {
        activated = true;
        for (var i = 0; i < encumbranceDefs.Count; i++)
        {
            var def = encumbranceDefs[i];
            if (!IsEncumbranceEnabled(def)) continue;
            var h = HediffMaker.MakeHediff(def, pawn);
            h.Severity = 1;
            h.canBeThreateningToPart = false;
            pawn.health.AddHediff(h);
            encumbranceHediffs.Add(h);
        }
        if (umbrellaProps.clothing) return;
        pawn.apparel.UpdateUmbrellaGraphics();
    }

    private void Reactivate()
    {
        Deactivate();
        Activate();
    }

    private void Deactivate()
    {
        activated = false;
        for (var i = 0; i < encumbranceHediffs.Count; i++)
        {
            pawn.health.RemoveHediff(encumbranceHediffs[i]);
        }
        encumbranceHediffs.Clear();
        if (umbrellaProps.clothing) return;
        pawn.apparel.UpdateUmbrellaGraphics();
    }

    private bool IsEncumbranceEnabled(HediffDef def)
    {
        if (def == DefOf.Bumber_UmbrellaEncumbranceCombat) return Settings.EncumberCombat;
        if (def == DefOf.Bumber_UmbrellaEncumbranceWork) return Settings.EncumberWork;
        return true;
    }
}
