using System.Collections.Generic;
using Verse;

namespace Bumbershoots.Ext.Verse;

internal static class WeatherDefExt
{
    internal static string[] SnowWeatherDefNames = null;

    internal static bool IsSnow(this WeatherDef @this)
    {
        if (SnowWeatherDefNames is null)
        {
            var defs = new List<string>(8);
            foreach (var def in DefDatabase<WeatherDef>.AllDefs)
            {
                if (def.defName.ToLowerInvariant().Contains("snow")) defs.Add(def.defName);
            }
            SnowWeatherDefNames = [.. defs];

        }
        for (var i = 0; i < SnowWeatherDefNames.Length; i++)
        {
            if (SnowWeatherDefNames[i] == @this.defName) return true;
        }
        return false;
    }

    internal static bool IsRain(this WeatherDef @this)
    {
        return @this.rainRate > 0 && !@this.IsSnow();
    }
}
