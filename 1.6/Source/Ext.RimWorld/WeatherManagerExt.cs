using System.Runtime.CompilerServices;
using Bumbershoots.Ext.Verse;
using RimWorld;
using Verse;

namespace Bumbershoots.Ext.RimWorld;

internal static class WeatherManagerExt
{
    internal static float RainRateDry = 0.1f;

    internal static WeatherDef UmbrellaWeatherDef(this WeatherManager @this)
    {
        var curIsRain = @this.curWeather.IsRain();
        var lastIsRain = @this.lastWeather.IsRain();
        if (curIsRain && lastIsRain) return @this.CurWeatherLerped;
        else if (curIsRain) return @this.curWeather;
        else if (lastIsRain) return @this.lastWeather;
        return null;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool IsUmbrellaWeather(this WeatherManager @this)
    {
        return @this.RainRate > RainRateDry;
    }
}
