#pragma warning disable IDE0044

using Bumbershoots.Ext.Verse;
using System;
using System.Runtime.CompilerServices;
using Verse;

namespace Bumbershoots;

public class MapComp(Map map) : MapComponent(map)
{
    private static float SkyGlowDarkness = 0.1f;

    internal event Action SunlightChanged;
    internal event Action WeatherChanged;

    internal bool? isSunlight;
    internal bool? prevIsSunlight;
    internal WeatherDef curWeatherLerped;
    internal WeatherDef prevCurWeatherLerped;
    internal bool isRain;

    internal bool Ready
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => curWeatherLerped != null;
    }

    private bool GetIsSunlight() => map.skyManager.CurSkyGlow > SkyGlowDarkness;

    private WeatherDef GetCurWeatherLerped() => map.weatherManager.CurWeatherLerped;

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

    internal void Notify_SettingsChanged()
    {
        isSunlight = Settings.umbrellasBlockSun && map.skyManager.CurSkyGlow > SkyGlowDarkness;
        SunlightChanged?.Invoke();
    }
}
