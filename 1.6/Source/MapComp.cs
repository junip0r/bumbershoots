#pragma warning disable IDE0044

using Bumbershoots.Ext.Verse;
using System;
using Verse;

namespace Bumbershoots;

public class MapComp(Map map) : MapComponent(map)
{
    public static float SkyGlowDarkness = 0.1f;

    public event Action SunlightChanged;
    public event Action WeatherChanged;

    public bool? curIsSunlight;
    public bool? prevIsSunlight;
    public WeatherDef curWeatherLerped;
    public WeatherDef prevWeatherLerped;
    public bool isRain;

    public bool GetIsSunlight() => map.skyManager.CurSkyGlow > SkyGlowDarkness;

    public WeatherDef GetCurWeatherLerped() => map.weatherManager.CurWeatherLerped;

    public override void ExposeData()
    {
        Scribe_Values.Look(ref curIsSunlight, nameof(curIsSunlight));
        Scribe_Defs.Look(ref curWeatherLerped, nameof(curWeatherLerped));
        Scribe_Values.Look(ref isRain, nameof(isRain));
    }

    public override void MapComponentTick()
    {
        if (Settings.umbrellasBlockSun)
        {
            prevIsSunlight = curIsSunlight;
            curIsSunlight = GetIsSunlight();
            if (curIsSunlight != prevIsSunlight) SunlightChanged?.Invoke();
        }
        prevWeatherLerped = curWeatherLerped;
        curWeatherLerped = GetCurWeatherLerped();
        if (curWeatherLerped != prevWeatherLerped)
        {
            isRain = curWeatherLerped.IsRain();
            WeatherChanged?.Invoke();
        }
    }

    public void Notify_SettingsChanged()
    {
        if (Settings.umbrellasBlockSun)
        {
            curIsSunlight = GetIsSunlight();
            SunlightChanged?.Invoke();
        }
    }
}
