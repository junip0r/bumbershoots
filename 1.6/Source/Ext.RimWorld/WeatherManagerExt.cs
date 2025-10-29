using System.Runtime.CompilerServices;
using Bumbershoots.Ext.Verse;
using RimWorld;
using Verse;

namespace Bumbershoots.Ext.RimWorld;

internal static class WeatherManagerExt
{
    private static readonly float rainRateDry = 0.1f;

    internal static WeatherDef UmbrellaWeatherDef(this WeatherManager w)
    {
        var curIsRain = w.curWeather.IsRain();
        var lastIsRain = w.lastWeather.IsRain();
        if (curIsRain && lastIsRain) return w.CurWeatherLerped;
        else if (curIsRain) return w.curWeather;
        else if (lastIsRain) return w.lastWeather;
        return null;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool IsUmbrellaWeather(this WeatherManager w)
    {
        return w.RainRate > rainRateDry;
    }
}
