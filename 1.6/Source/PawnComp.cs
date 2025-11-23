#pragma warning disable IDE0060

using Bumbershoots.Ext.Verse;
using Verse;

namespace Bumbershoots;

public class PawnComp : ThingComp
{
    internal bool ticking;
    internal Pawn pawn;
    internal MapComp mapComp;
    internal UmbrellaComp umbrellaComp;
    internal bool roofed;
    internal bool prevRoofed;
    internal bool blockingSunlight;
    internal bool blockingWeather;

    private bool pawnDislikesSunlight;

    private bool ShouldDisable => parent is not Pawn p || p.IsWildMan();

    public override void PostExposeData()
    {
        Scribe_Values.Look(ref ticking, nameof(ticking));
        Scribe_Values.Look(ref roofed, nameof(roofed));
        Scribe_Values.Look(ref prevRoofed, nameof(prevRoofed));
        Scribe_Values.Look(ref blockingSunlight, nameof(blockingSunlight));
        Scribe_Values.Look(ref blockingWeather, nameof(blockingWeather));
    }

    public override void Initialize(CompProperties props)
    {
        if (ShouldDisable)
        {
            parent.AllComps.Remove(this);
            return;
        }
        base.Initialize(props);
        pawn = (Pawn)parent;
    }

    public override void CompTick()
    {
        if (!ticking) return;
        prevRoofed = roofed;
        roofed = pawn.Position.Roofed(mapComp.map);
        if (roofed != prevRoofed) umbrellaComp.Update();
    }

    private bool GetBlockingSunlight(bool isSunlight)
    {
        return pawnDislikesSunlight
            && isSunlight
            && umbrellaComp.umbrellaProps.blocksSunlight;
    }

    private bool GetBlockingWeather(WeatherDef def)
    {
        if (def is null) return false;
        return umbrellaComp.umbrellaProps.BlocksWeather(def.defName);
    }

    private void UpdateTicking()
    {
        ticking = !pawn.Dead && (blockingSunlight || blockingWeather);
    }

    private void Update()
    {
        if (pawn.Dead && umbrellaComp != null)
        {
            UpdateTicking();
            return;
        }
        if (mapComp is null || umbrellaComp is null)
        {
            blockingSunlight = false;
            blockingWeather = false;
        }
        else
        {
            blockingSunlight = GetBlockingSunlight(mapComp.isSunlight);
            blockingWeather = GetBlockingWeather(mapComp.curWeatherLerped);
        }
        UpdateTicking();
        umbrellaComp?.Update();
    }

    public override void PostSpawnSetup(bool respawningAfterLoad)
    {
        pawnDislikesSunlight = pawn.HasSunlightSensitivity();
        mapComp = pawn.MapHeld.MapComp();
        mapComp.SunlightChanged += Notify_SunlightChanged;
        mapComp.WeatherChanged += Notify_WeatherChanged;
        Update();
    }

    public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
    {
        mapComp.SunlightChanged -= Notify_SunlightChanged;
        mapComp.WeatherChanged -= Notify_WeatherChanged;
        mapComp = null;
        Update();
    }

    internal void Notify_Resurrected()
    {
        Update();
    }

    public override void Notify_Killed(Map prevMap, DamageInfo? dinfo = null)
    {
        UpdateTicking();
    }

    internal void Notify_UmbrellaEquipped(UmbrellaComp umbrellaComp)
    {
        this.umbrellaComp = umbrellaComp;
        Update();
    }

    internal void Notify_UmbrellaUnequipped(UmbrellaComp umbrellaComp)
    {
        this.umbrellaComp = null;
        Update();
    }

    private void Notify_SunlightChanged(bool isSunlight)
    {
        if (mapComp is null || umbrellaComp is null || !pawnDislikesSunlight || pawn.Dead) return;
        blockingSunlight = GetBlockingSunlight(isSunlight);
        UpdateTicking();
        umbrellaComp.Update();
    }

    private void Notify_WeatherChanged(WeatherDef def)
    {
        if (mapComp is null || umbrellaComp is null || pawn.Dead) return;
        blockingWeather = GetBlockingWeather(def);
        UpdateTicking();
        umbrellaComp.Update();
    }

    internal void Notify_GenesChanged()
    {
        pawnDislikesSunlight = pawn.HasSunlightSensitivity();
        if (mapComp != null) Notify_SunlightChanged(mapComp.isSunlight);
    }

    internal void Notify_SettingsChanged()
    {
        Update();
    }
}
