#pragma warning disable IDE0044

using System;
using Verse;

namespace Bumbershoots;

public class MapComp(Map map) : MapComponent(map)
{
    private static float SkyGlowDarkness = 0.1f;

    internal event Action<bool> SunlightChanged;
    internal event Action<WeatherDef> WeatherChanged;

    internal bool isSunlight;
    private bool prevIsSunlight;
    internal WeatherDef curWeatherLerped;
    private WeatherDef prevCurWeatherLerped;

    public override void MapComponentTick()
    {
        if (Settings.UmbrellasBlockSun)
        {
            prevIsSunlight = isSunlight;
            isSunlight = map.skyManager.CurSkyGlow > SkyGlowDarkness;
            if (isSunlight != prevIsSunlight)
            {
                SunlightChanged?.Invoke(isSunlight);
            }
        }
        prevCurWeatherLerped = curWeatherLerped;
        curWeatherLerped = map.weatherManager.CurWeatherLerped;
        if (curWeatherLerped != prevCurWeatherLerped)
        {
            WeatherChanged?.Invoke(curWeatherLerped);
        }
    }

    internal void Notify_SettingsChanged()
    {
        isSunlight = Settings.UmbrellasBlockSun && map.skyManager.CurSkyGlow > SkyGlowDarkness;
    }

    public override void ExposeData()
    {
        Scribe_Values.Look(ref isSunlight, nameof(isSunlight));
        Scribe_Values.Look(ref prevIsSunlight, nameof(prevIsSunlight));
        Scribe_Defs.Look(ref curWeatherLerped, nameof(curWeatherLerped));
        Scribe_Defs.Look(ref prevCurWeatherLerped, nameof(prevCurWeatherLerped));
    }
}
