using Bumbershoots.Ext.Verse;
using RimWorld;
using Verse;

namespace Bumbershoots;

public class UmbrellaComp : ThingComp
{
    internal UmbrellaProps umbrellaProps;
    private Pawn pawn;
    private PawnComp pawnComp;
    internal bool activated;
    internal bool blockingSunlight;
    internal bool blockingWeather;
    private UmbrellaHediffs hediffs;

    public override void Initialize(CompProperties props)
    {
        umbrellaProps = (UmbrellaProps)props;
        if (ShouldDisable())
        {
            parent.AllComps.Remove(this);
            return;
        }
        base.Initialize(props);
        hediffs = new(umbrellaProps);
    }

    public override void PostExposeData()
    {
        Scribe_References.Look(ref pawn, nameof(pawn));
        Scribe_Values.Look(ref activated, nameof(activated));
        Scribe_Values.Look(ref blockingSunlight, nameof(blockingSunlight));
        Scribe_Values.Look(ref blockingWeather, nameof(blockingWeather));
        if (Scribe.mode == LoadSaveMode.PostLoadInit)
        {
            if (pawn != null) Reattach();
        }
    }

    public override void Notify_Equipped(Pawn pawn) => Attach(pawn);
    public override void Notify_Unequipped(Pawn pawn) => Detach();
    public void Notify_SettingsChanged() => Reactivate(true);

    private bool ShouldDisable()
    {
        if (!umbrellaProps.HasDefName) return false;
        return umbrellaProps.defName != parent.def.defName;
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
        Deactivate();
        pawnComp.umbrellaComp = null;
        pawn = null;
        pawnComp = null;
    }

    private void Reattach()
    {
        pawnComp ??= pawn.PawnComp();
        pawnComp.umbrellaComp = this;
        Reactivate(false);
    }

    public override void CompTick()
    {
        if (pawn is null) return;
        if (pawnComp.mapComp is not MapComp mapComp) return;
        Update(mapComp);
        if (ShouldActivate())
        {
            Activate();
        }
        else
        {
            Deactivate();
        }
    }

    private void Update(MapComp mapComp)
    {
        if (umbrellaProps.clothing && !Settings.UmbrellaClothing) goto Off;
        var map = mapComp.map;
        var cell = pawn.Position;
        if (cell.Roofed(map)) goto Off;
        blockingSunlight = mapComp.IsUmbrellaSunlight
            && umbrellaProps.blocksSunlight
            && pawn.HasSunlightSensitivity()
            && cell.InSunlight(map);
        blockingWeather = mapComp.IsUmbrellaWeather
            && umbrellaProps.blocksWeather.Contains(map.weatherManager.CurWeatherLerped.defName);
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
        if (activated) return;
        activated = true;
        hediffs.Activate(pawn);
        if (updateGraphics) UpdateGraphics();
    }

    private void Deactivate(bool updateGraphics = true)
    {
        if (!activated) return;
        activated = false;
        hediffs.Deactivate(pawn);
        if (updateGraphics) UpdateGraphics();
    }

    private void Reactivate(bool updateGraphics)
    {
        if (!activated) return;
        Deactivate(false);
        Activate(updateGraphics);
    }

    private void UpdateGraphics()
    {
        if (!umbrellaProps.hideable) return;
        pawn.apparel.Notify_ApparelChanged();
        PortraitsCache.SetDirty(pawn);
    }
}
