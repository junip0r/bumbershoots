using Bumbershoots.Ext.Verse;
using RimWorld;
using System.Runtime.CompilerServices;
using Verse;

namespace Bumbershoots;

public class UmbrellaComp : ThingComp
{
    internal UmbrellaProps umbrellaProps;
    private Pawn pawn;
    private PawnComp pawnComp;
    private bool pawnDislikesSunlight;
    private bool ticking;
    internal bool activated;
    private bool blockingSunlight;
    private bool blockingWeather;
    private UmbrellaHediffs hediffs;

    internal bool BlockingSunlight
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => activated && blockingSunlight;
    }

    internal bool BlockingWeather
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => activated && blockingWeather;
    }

    private bool ShouldDisable()
    {
        if (!umbrellaProps.HasDefName) return false;
        return umbrellaProps.defName != parent.def.defName;
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
        hediffs = new(umbrellaProps);
    }

    public override void PostExposeData()
    {
        Scribe_References.Look(ref pawn, nameof(pawn));
        Scribe_Values.Look(ref pawnDislikesSunlight, nameof(pawnDislikesSunlight));
        Scribe_Values.Look(ref ticking, nameof(ticking));
        Scribe_Values.Look(ref activated, nameof(activated));
        Scribe_Values.Look(ref blockingSunlight, nameof(blockingSunlight));
        Scribe_Values.Look(ref blockingWeather, nameof(blockingWeather));
        if (Scribe.mode == LoadSaveMode.PostLoadInit)
        {
            if (pawn != null)
            {
                pawnComp ??= pawn.PawnComp();
                pawnComp.UmbrellaComp = this;
            }
        }
    }

    public override void Notify_Equipped(Pawn pawn)
    {
        pawnComp = pawn.PawnComp();
        if (pawnComp is null) return;
        pawnComp.UmbrellaComp = this;
        this.pawn = pawn;
        pawnDislikesSunlight = pawn.HasSunlightSensitivity();
        if (pawnComp.MapComp != null) Notify_MapLoad();
    }

    public override void Notify_Unequipped(Pawn pawn)
    {
        if (pawn is null) return;
        if (activated) Deactivate();
        if (pawnComp.MapComp != null) Notify_MapUnload();
        pawnComp.UmbrellaComp = null;
        pawnComp = null;
        this.pawn = null;
    }

    internal void Notify_MapLoad()
    {
        ticking = !pawn.Dead && (!umbrellaProps.clothing || Settings.UmbrellaClothing);
        pawnComp.MapComp.SunlightChanged += Notify_SunlightChanged;
        pawnComp.MapComp.WeatherChanged += Notify_WeatherChanged;
        Notify_SunlightChanged(pawnComp.MapComp.IsSunlight);
        Notify_WeatherChanged(pawnComp.MapComp.CurWeatherLerped);
    }

    internal void Notify_MapUnload()
    {
        ticking = false;
        pawnComp.MapComp.SunlightChanged -= Notify_SunlightChanged;
        pawnComp.MapComp.WeatherChanged -= Notify_WeatherChanged;
        blockingSunlight = false;
        blockingWeather = false;
    }

    internal void Notify_PawnGenesChanged()
    {
        pawnDislikesSunlight = pawn.HasSunlightSensitivity();
        Notify_SunlightChanged(pawnComp.MapComp.IsSunlight);
    }

    internal void Notify_SunlightChanged(bool isSunlight)
    {
        blockingSunlight = pawnDislikesSunlight
            && isSunlight
            && umbrellaProps.blocksSunlight;
    }

    internal void Notify_WeatherChanged(WeatherDef def)
    {
        if (def is null) return;
        blockingWeather = umbrellaProps.blocksWeather != null
            && umbrellaProps.blocksWeather.Contains(def.defName);
    }

    public void Notify_SettingsChanged()
    {
        var p = pawn;
        Notify_Unequipped(p);
        Notify_Equipped(p);
        Notify_SunlightChanged(pawnComp.MapComp.IsSunlight);
    }

    public override void CompTick()
    {
        if (!ticking) return;
        if (pawn.Position.Roofed(pawnComp.MapComp.map))
        {
            if (activated) Deactivate();
        }
        else if (blockingWeather || blockingSunlight)
        {
            if (!activated) Activate();
        }
        else
        {
            if (activated) Deactivate();
        }
    }

    private void Activate()
    {
        activated = true;
        hediffs.Activate(pawn);
        UpdateGraphics();
    }

    private void Deactivate()
    {
        activated = false;
        hediffs.Deactivate(pawn);
        UpdateGraphics();
    }

    private void UpdateGraphics()
    {
        if (!umbrellaProps.hideable) return;
        pawn.apparel.Notify_ApparelChanged();
        PortraitsCache.SetDirty(pawn);
    }
}
