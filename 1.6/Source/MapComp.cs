#pragma warning disable IDE0044

using Bumbershoots.Ext.Verse;
using System;
using Verse;

namespace Bumbershoots;

public class MapComp(Map map) : MapComponent(map)
{
    private static float SkyGlowDarkness = 0.1f;

    internal event Action SunlightChanged;
    internal event Action WeatherChanged;

    internal bool isSunlight;
    internal bool isRain;
    internal WeatherDef curWeatherLerped;
    private bool prevIsSunlight;
    private WeatherDef prevCurWeatherLerped;

    public override void MapComponentTick()
    {
        if (Settings.umbrellasBlockSun)
        {
            prevIsSunlight = isSunlight;
            isSunlight = map.skyManager.CurSkyGlow > SkyGlowDarkness;
            if (isSunlight != prevIsSunlight)
            {
                SunlightChanged?.Invoke();
            }
        }
        prevCurWeatherLerped = curWeatherLerped;
        curWeatherLerped = map.weatherManager.CurWeatherLerped;
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

    public override void ExposeData()
    {
        Scribe_Values.Look(ref isSunlight, nameof(isSunlight));
        Scribe_Values.Look(ref prevIsSunlight, nameof(prevIsSunlight));
        Scribe_Defs.Look(ref curWeatherLerped, nameof(curWeatherLerped));
        Scribe_Defs.Look(ref prevCurWeatherLerped, nameof(prevCurWeatherLerped));
    }
}
