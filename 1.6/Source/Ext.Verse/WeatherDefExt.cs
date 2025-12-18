using System.Collections.Generic;
using Verse;

namespace Bumbershoots.Ext.Verse;

public static class WeatherDefExt
{
    private static readonly HashSet<string> SnowWeatherDefNames = [];

    public static bool IsSnow(this WeatherDef @this)
    {
        if (SnowWeatherDefNames.Count == 0)
            foreach (var def in DefDatabase<WeatherDef>.AllDefs)
                if (def.defName.ToLowerInvariant().Contains("snow"))
                    SnowWeatherDefNames.Add(def.defName);
        return SnowWeatherDefNames.Contains(@this.defName);
    }

    public static bool IsRain(this WeatherDef @this)
    {
        return @this.rainRate > 0 && !@this.IsSnow();
    }
}
