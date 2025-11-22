#pragma warning disable IDE0044

using System;
using Verse;

namespace Bumbershoots;

public class MapComp(Map map) : MapComponent(map)
{
    private static float SkyGlowDarkness = 0.1f;

    internal Action<bool> SunlightChanged;
    internal Action<WeatherDef> WeatherChanged;
    internal bool IsSunlight;
    private bool prevIsSunlight;
    internal WeatherDef CurWeatherLerped;
    private WeatherDef prevCurWeatherLerped;

    public override void MapComponentTick()
    {
        if (Settings.UmbrellasBlockSun)
        {
            prevIsSunlight = IsSunlight;
            IsSunlight = map.skyManager.CurSkyGlow > SkyGlowDarkness;
            if (IsSunlight != prevIsSunlight)
            {
                SunlightChanged?.Invoke(IsSunlight);
            }
        }
        prevCurWeatherLerped = CurWeatherLerped;
        CurWeatherLerped = map.weatherManager.CurWeatherLerped;
        if (CurWeatherLerped != prevCurWeatherLerped)
        {
            WeatherChanged?.Invoke(CurWeatherLerped);
        }
    }

    internal void Notify_SettingsChanged()
    {
        IsSunlight = Settings.UmbrellasBlockSun && map.skyManager.CurSkyGlow > SkyGlowDarkness;
    }

    public override void ExposeData()
    {
        Scribe_Values.Look(ref IsSunlight, nameof(IsSunlight));
        Scribe_Values.Look(ref prevIsSunlight, nameof(prevIsSunlight));
        Scribe_Defs.Look(ref CurWeatherLerped, nameof(CurWeatherLerped));
        Scribe_Defs.Look(ref prevCurWeatherLerped, nameof(prevCurWeatherLerped));
    }
}
