#pragma warning disable IDE0044

using Bumbershoots.Ext.Verse;
using System;
using Verse;

namespace Bumbershoots;

public class MapComp(Map map) : MapComponent(map)
{
    private static float SkyGlowDarkness = 0.1f;

    public event Action SunlightChanged;
    public event Action WeatherChanged;

    public bool? isSunlight;
    public bool? prevIsSunlight;
    public WeatherDef curWeatherLerped;
    public WeatherDef prevCurWeatherLerped;
    public bool isRain;

    private bool GetIsSunlight() => map.skyManager.CurSkyGlow > SkyGlowDarkness;

    private WeatherDef GetCurWeatherLerped() => map.weatherManager.CurWeatherLerped;

    public override void ExposeData()
    {
        Scribe_Values.Look(ref isSunlight, nameof(isSunlight));
        Scribe_Defs.Look(ref curWeatherLerped, nameof(curWeatherLerped));
        Scribe_Values.Look(ref isRain, nameof(isRain));
    }

    public override void MapComponentTick()
    {
        if (Settings.umbrellasBlockSun)
        {
            prevIsSunlight = isSunlight;
            isSunlight = GetIsSunlight();
            if (isSunlight != prevIsSunlight) SunlightChanged?.Invoke();
        }
        prevCurWeatherLerped = curWeatherLerped;
        curWeatherLerped = GetCurWeatherLerped();
        if (curWeatherLerped != prevCurWeatherLerped)
        {
            isRain = curWeatherLerped.IsRain();
            WeatherChanged?.Invoke();
        }
    }

    public void Notify_SettingsChanged()
    {
        if (Settings.umbrellasBlockSun)
        {
            isSunlight = GetIsSunlight();
            SunlightChanged?.Invoke();
        }
    }
}
