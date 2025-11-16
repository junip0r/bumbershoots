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
    private List<Hediff> encumbrances = [];

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
    }

    public override void PostExposeData()
    {
        Scribe_References.Look(ref pawn, nameof(pawn));
        Scribe_Values.Look(ref activated, nameof(activated));
        Scribe_Values.Look(ref blockingSunlight, nameof(blockingSunlight));
        Scribe_Values.Look(ref blockingWeather, nameof(blockingWeather));
        Scribe_Collections.Look(ref encumbrances, nameof(encumbrances), LookMode.Reference);
        if (Scribe.mode == LoadSaveMode.PostLoadInit)
        {
            encumbrances ??= [];
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

    private void PrepareEncumbrances()
    {
        if (umbrellaProps.encumbrances is null) return;
        for (var i = 0; i < umbrellaProps.encumbrances.Count; i++)
        {
            var defName = umbrellaProps.encumbrances[i];
            if (!IsEncumbranceEnabled(defName)) continue;
            if (DefDatabase<HediffDef>.GetNamed(defName) is not HediffDef def) continue;
            var h = HediffMaker.MakeHediff(def, pawn);
            h.Severity = 1;
            h.canBeThreateningToPart = false;
            encumbrances.Add(h);
        }
    }

    private void Attach(Pawn pawn)
    {
        if (pawn.PawnComp() is not PawnComp pawnComp) return;
        this.pawn = pawn;
        this.pawnComp = pawnComp;
        pawnComp.UmbrellaComp = this;
        PrepareEncumbrances();
    }

    private void Detach()
    {
        if (pawn is null) return;
        if (activated) Deactivate();
        pawnComp.UmbrellaComp = null;
        pawn = null;
        pawnComp = null;
        encumbrances.Clear();
    }

    private void Reattach(bool updateGraphics)
    {
        pawnComp ??= pawn.PawnComp();
        pawnComp.UmbrellaComp = this;
        var activated = this.activated;
        if (activated) Deactivate(false);
        encumbrances.Clear();
        PrepareEncumbrances();
        if (activated) Activate(updateGraphics);
    }

    public override void CompTick()
    {
        // Update(), Activate() and Deactivate() are about as optimized as I can
        // make them. Update() seems to be very fast. Activate() and Deactivate()
        // are comparatively slow, but the vast majority of their time is spent
        // in vanilla code adding/removing hediffs or dirtying pawn graphics, so
        // there's not much I can do.
        //
        // Testing a release build with no other mods, 25 umbrella-wearing pawns,
        // a few umbrellas laying on the ground, with alternating roofed and
        // unroofed sections:
        //
        // On a clear day, Dub's clocks this CompTick() at 3-5us average (8-10us
        // max) per tick, 0.07us per call average.
        //
        // With rainy weather, we get 10us average (127us max) per tick, 0.24us
        // per call average. The 127us max happens when Activate() or Deactivate()
        // are called, when a pawn transitions between roofed and unroofed. Dub's
        // graph shows spikes when a pawn walks outside or inside. As we've scrubbed
        // everything else from those two methods, it's the hediff add/remove and
        // the dirtying of pawn/portrait graphics using all the time.
        //
        // When a map transitions from clear to rainy, or vice versa, every outdoor
        // pawn will call either Activate() or Deactivate() at the same time. Dub's
        // clocks this event as an anywhere from 1ms to 3ms max per tick, probably
        // depending on the number of pawns who happen to be outside when the event
        // occurs.
        //
        // I don't think there's anything I can do about these spikes, though they
        // are not bad in practice, and likely happen to all mods that dirty pawn
        // graphics.
        //
        // Broadly, these times are excellent, and I'm quite happy with the results.
        //
        // :)

        if (pawnComp?.MapComp is not MapComp mapComp) return;
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
        for (var i = 0; i < encumbrances.Count; i++)
        {
            pawn.health.AddHediff(encumbrances[i]);
        }
        if (updateGraphics && umbrellaProps.hideable) pawn.apparel.UpdateUmbrellaGraphics();
    }

    private void Deactivate(bool updateGraphics = true)
    {
        activated = false;
        for (var i = 0; i < encumbrances.Count; i++)
        {
            pawn.health.RemoveHediff(encumbrances[i]);
        }
        if (updateGraphics && umbrellaProps.hideable) pawn.apparel.UpdateUmbrellaGraphics();
    }

    private bool IsEncumbranceEnabled(string defName)
    {
        if (defName == DefOf.Bumber_UmbrellaEncumbranceCombat.defName) return Settings.EncumberCombat;
        if (defName == DefOf.Bumber_UmbrellaEncumbranceWork.defName) return Settings.EncumberWork;
        return true;
    }
}
